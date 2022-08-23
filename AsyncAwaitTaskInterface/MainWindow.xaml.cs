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
using DC_Practical_1;
using BusinessTier;

namespace AsyncAwaitTaskInterface
{
    public delegate Customer Search(string value);
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusinessServerInterface foob;
        private string searchvalue;
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
            try
            {
                int index = Int32.Parse(IndexBox.Text);

                if (index > 0 && index < 100001)
                {
                    ErrorMsgBox.Text = String.Empty;

                    foob.GetValuesForEntry(index, out uint acct, out uint pin, out int bal, out string fName, out string lName, out string profPicPath);

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
                else
                {
                    ErrorMsgBox.Text = "Index entered is out of range. Please check the total number of items.";
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        private async void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var regexItem = new System.Text.RegularExpressions.Regex("^[a-zA-Z]*$");

            if (regexItem.IsMatch(SearchLastNameBox.Text))
            {
                searchvalue = SearchLastNameBox.Text;
                Task<Customer> task = new Task<Customer>(SearchDB);
                task.Start();
                StatusLabel.Content = "Search started.............";
                Customer customer = await task;
                UpdateGUI(customer);
                StatusLabel.Content = "Search ended...............";
            }
            else
            {
                ErrorMsgBox.Text = "Bad Input Detected. Input must be a valid last name with no special characters.";
            }
        }

        private Customer SearchDB()
        {
            string firstName, lastName, profilePicPath;
            uint pin, acctNum;
            int bal;

            try
            {
                foob.SearchCustomer(searchvalue, out acctNum, out pin, out bal, out firstName, out lastName, out profilePicPath);
                Customer customer = new Customer
                {
                    acctNo = acctNum,
                    pin = pin,
                    balance = bal,
                    firstname = firstName,
                    lastname = lastName,
                    profPicPath = profilePicPath
                };
                return customer;
            }
            catch (FaultException)
            {
                return null;
            }
        }

        private void UpdateGUI(Customer customer)
        {
            if (customer == null)
            {
                ErrorMsgBox.Text = "No matching user found.";
            }
            else
            {
                FirstNameBox.Text = customer.firstname;
                LastNameBox.Text = customer.lastname;
                AcctNoBox.Text = customer.acctNo.ToString("D4");
                PinNumBox.Text = customer.pin.ToString("D4");
                BalanceBox.Text = customer.balance.ToString("C");

                BitmapImage profilePicture = new BitmapImage();
                profilePicture.BeginInit();
                profilePicture.UriSource = new Uri(customer.profPicPath);
                profilePicture.EndInit();

                ProfileImage.Source = profilePicture;
            }
        }
    }
}
