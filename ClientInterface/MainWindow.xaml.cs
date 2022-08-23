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

namespace ClientInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusinessServerInterface foob;
        public MainWindow()
        {
            InitializeComponent();
            ChannelFactory<BusinessServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8200/BusinessService";
            foobFactory = new ChannelFactory<BusinessServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            TotalNumText.Text = foob.GetNumEntries().ToString();

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
                    
                foob.GetValuesForEntry(index, out acct, out pin, out bal, out fName, out lName, out profPicPath);

                FirstNameBox.Text = fName;
                LastNameBox.Text = lName;
                BalanceBox.Text = bal.ToString("C");
                AcctNoBox.Text = acct.ToString("D4");
                PinNumBox.Text = pin.ToString("D4");

                BitmapImage profilePicture = new BitmapImage();
                profilePicture.BeginInit();
                profilePicture.UriSource = new Uri(profPicPath);
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
                    foob.SearchCustomer(SearchLastNameBox.Text, out uint acctNumber, out uint pinNumber, out int balance, out string firstName, out string lastName, out string profilePicturePath);

                    if (acctNumber == 0)
                    {
                        ErrorMsgBox.Text = "No user with matching last name found";
                    }
                    else
                    {
                        FirstNameBox.Text = firstName;
                        LastNameBox.Text = lastName;
                        BalanceBox.Text = balance.ToString("C");
                        AcctNoBox.Text = acctNumber.ToString("D4");
                        PinNumBox.Text = pinNumber.ToString("D4");
                        
                        BitmapImage profilePicture = new BitmapImage();
                        profilePicture.BeginInit();
                        profilePicture.UriSource = new Uri(profilePicturePath);
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
