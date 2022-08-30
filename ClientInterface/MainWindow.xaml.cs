using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using BusinessTier;
using System.ServiceModel;
using InterfaceToDLL;
using Newtonsoft.Json;
using RestSharp;
using APIClassLibrary;

namespace ClientInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            
        }

        private void GoBtn_Click(object sender, RoutedEventArgs e)
        {
            string fName = "", lName = "", profPicPath = "";
            int bal = 0;
            uint acct = 0, pin = 0;

            try
            {
                int index = Int32.Parse(IndexBox.Text);
                ErrorMsgBox.Text = String.Empty;

                string URL = "https://localhost:44352/";
                RestClient client = new RestClient(URL);
                RestRequest request = new RestRequest("api/customer/get/" + index.ToString());
                RestResponse response = client.Get(request);

                APIClassLibrary.DataIntermed dataIntermed = JsonConvert.DeserializeObject<APIClassLibrary.DataIntermed>(response.Content);

                FirstNameBox.Text = dataIntermed.fName;
                LastNameBox.Text = dataIntermed.lName;
                BalanceBox.Text = dataIntermed.bal.ToString("C");
                AcctNoBox.Text = dataIntermed.acctNo.ToString("D4");
                PinNumBox.Text = dataIntermed.pin.ToString("D4");

                BitmapImage profilePicture = new BitmapImage();
                profilePicture.BeginInit();
                profilePicture.UriSource = new Uri(dataIntermed.profPicPath);
                profilePicture.EndInit();
                
                ProfileImage.Source = profilePicture;
            }
            catch(FormatException ex)
            {
                Console.WriteLine(ex.Message);
                ErrorMsgBox.Text = "Index entered is not in the correct format. Please try again.";
            }
            catch(FaultException ex)
            {
                ErrorMsgBox.Text = ex.Message.ToString();
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {

            var regexItem = new System.Text.RegularExpressions.Regex("^[a-zA-Z]*$");

            if (regexItem.IsMatch(SearchLastNameBox.Text))
            {
                ErrorMsgBox.Text = String.Empty;
                try
                {
                    APIClassLibrary.SearchData mySearch = new APIClassLibrary.SearchData();
                    mySearch.searchStr = SearchLastNameBox.Text;
                    string URL = "https://localhost:44352/";
                    RestClient client = new RestClient(URL);
                    RestRequest request = new RestRequest("api/search/");
                    request.AddJsonBody(mySearch);

                    RestResponse response = client.Post(request);
                    APIClassLibrary.DataIntermed dataIntermed = JsonConvert.DeserializeObject<APIClassLibrary.DataIntermed>(response.Content);


                    if (dataIntermed.acctNo == 0)
                    {
                        ErrorMsgBox.Text = "No user with matching last name found";
                    }
                    else
                    {
                        FirstNameBox.Text = dataIntermed.fName;
                        LastNameBox.Text = dataIntermed.lName;
                        BalanceBox.Text = dataIntermed.bal.ToString("C");
                        AcctNoBox.Text = dataIntermed.acctNo.ToString("D4");
                        PinNumBox.Text = dataIntermed.pin.ToString("D4");
                        
                        BitmapImage profilePicture = new BitmapImage();
                        profilePicture.BeginInit();
                        profilePicture.UriSource = new Uri(dataIntermed.profPicPath);
                        profilePicture.EndInit();

                        ProfileImage.Source = profilePicture;
                    }
                }
                catch (FaultException ex)
                {
                    ErrorMsgBox.Text = ex.Reason.ToString();
                }
            }
            else
            {
                ErrorMsgBox.Text = "Bad Input Detected. Input must be a valid last name with no special characters.";
            }
            
        }
    }
}
