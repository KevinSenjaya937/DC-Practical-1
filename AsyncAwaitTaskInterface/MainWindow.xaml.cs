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
using System.ServiceModel;
using Newtonsoft.Json;
using RestSharp;
using APIClassLibrary;
using System.Web.Http;
using System.Net.Http;

namespace AsyncAwaitTaskInterface
{

    public partial class MainWindow : Window
    {
        private static string URL = "https://localhost:44352/";
        private static RestClient client = new RestClient(URL);
        private string searchvalue;
        public MainWindow()
        {
            InitializeComponent();
            RestRequest request = new RestRequest("api/values/numEntries");
            RestResponse response = client.Get(request);

            TotalNumText.Text = response.Content;

        }

        private void GoBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = Int32.Parse(IndexBox.Text);
                index--;
                ErrorMsgBox.Text = String.Empty;

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
            catch (HttpResponseException ex)
            {
                ErrorMsgBox.Text = ex.Message;
            }
            catch (HttpRequestException)
            {
                ErrorMsgBox.Text = "Index provided is out of range.";
            }
        }

        private async void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ErrorMsgBox.Text = String.Empty;
                searchvalue = SearchLastNameBox.Text;
                Task<DataIntermed> task = new Task<DataIntermed>(SearchDB);
                switchOnReadOnly(true);
                task.Start();
                StatusLabel.Content = "Search started...";
                DataIntermed customer = await task;
                UpdateGUI(customer);
                StatusLabel.Content = "Search ended...";

            }
            catch (FaultException ex)
            {
                ErrorMsgBox.Text = ex.Reason.ToString();
            }
        }

        private DataIntermed SearchDB()
        {
            try
            {
                SearchData mySearch = new SearchData
                {
                    searchStr = searchvalue
                };
                RestRequest request = new RestRequest("api/search/post");
                request.AddJsonBody(mySearch);

                RestResponse response = client.Post(request);
                DataIntermed dataIntermed = JsonConvert.DeserializeObject<DataIntermed>(response.Content);

                return dataIntermed;
            }
            catch (HttpResponseException ex)
            {
                ErrorMsgBox.Text = ex.Message.ToString();
            }
            catch (HttpRequestException)
            {
                ErrorMsgBox.Dispatcher.Invoke(new Action(() => ErrorMsgBox.Text = "No matching customer with last name found."));
            }
            return null;
        }

        private void UpdateGUI(DataIntermed customer)
        {
            if (customer == null)
            {
                FirstNameBox.Text = "First Name";
                LastNameBox.Text = "Last Name";
                AcctNoBox.Text = "Account Number";
                PinNumBox.Text = "Pin Number";
                BalanceBox.Text = "Balance";
                ProfileImage.Source = null;
            }
            else
            {
                FirstNameBox.Text = customer.fName;
                LastNameBox.Text = customer.lName;
                AcctNoBox.Text = customer.acctNo.ToString("D4");
                PinNumBox.Text = customer.pin.ToString("D4");
                BalanceBox.Text = customer.bal.ToString("C");

                BitmapImage profilePicture = new BitmapImage();
                profilePicture.BeginInit();
                profilePicture.UriSource = new Uri(customer.profPicPath);
                profilePicture.EndInit();

                ProfileImage.Source = profilePicture;
            }
            switchOnReadOnly(false);
        }

        private void switchOnReadOnly(Boolean switchBool)
        {
            if (switchBool)
            {
                SearchBtn.IsEnabled = false;
                GoBtn.IsEnabled = false;
                SetBaseURL.IsEnabled = false;
                SearchProgressBar.IsIndeterminate = true;
                FirstNameBox.Dispatcher.Invoke(new Action(() => FirstNameBox.IsReadOnly = true));
                LastNameBox.Dispatcher.Invoke(new Action(() => LastNameBox.IsReadOnly = true));
                BalanceBox.Dispatcher.Invoke(new Action(() => BalanceBox.IsReadOnly = true));
                AcctNoBox.Dispatcher.Invoke(new Action(() => AcctNoBox.IsReadOnly = true));
                PinNumBox.Dispatcher.Invoke(new Action(() => PinNumBox.IsReadOnly = true));
                IndexBox.Dispatcher.Invoke(new Action(() => IndexBox.IsReadOnly = true));
                SearchLastNameBox.Dispatcher.Invoke(new Action(() => SearchLastNameBox.IsReadOnly = true));
                SetURLBox.Dispatcher.Invoke(new Action(() => SetURLBox.IsReadOnly = true));
            }
            else
            {
                SearchBtn.IsEnabled = true;
                GoBtn.IsEnabled = true;
                SetBaseURL.IsEnabled = true;
                SearchProgressBar.IsIndeterminate = false;
                FirstNameBox.Dispatcher.Invoke(new Action(() => FirstNameBox.IsReadOnly = false));
                LastNameBox.Dispatcher.Invoke(new Action(() => LastNameBox.IsReadOnly = false));
                BalanceBox.Dispatcher.Invoke(new Action(() => BalanceBox.IsReadOnly = false));
                AcctNoBox.Dispatcher.Invoke(new Action(() => AcctNoBox.IsReadOnly = false));
                PinNumBox.Dispatcher.Invoke(new Action(() => PinNumBox.IsReadOnly = false));
                IndexBox.Dispatcher.Invoke(new Action(() => IndexBox.IsReadOnly = false));
                SearchLastNameBox.Dispatcher.Invoke(new Action(() => SearchLastNameBox.IsReadOnly = false));
                SetURLBox.Dispatcher.Invoke(new Action(() => SetURLBox.IsReadOnly = false));
            }
        }

        private void SetBaseURL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ErrorMsgBox.Text = String.Empty;
                MainWindow.URL = SetURLBox.Text;
                MainWindow.client = new RestClient(URL);
            }
            catch (UriFormatException)
            {
                MainWindow.URL = "https://localhost:44352/";
                MainWindow.client = new RestClient(URL);
                ErrorMsgBox.Text = "Invalid URL format";
            }            
        }
    }
}
