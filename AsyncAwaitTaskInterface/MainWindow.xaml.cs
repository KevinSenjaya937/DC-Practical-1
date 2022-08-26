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
            tcp.OpenTimeout = new TimeSpan(1, 0, 0);
            tcp.SendTimeout = new TimeSpan(1, 0, 0);

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
            catch (FormatException ex)
            {
                ErrorMsgBox.Text = ex.Message.ToString();

            }
            catch (FaultException ex)
            {
                ErrorMsgBox.Text = ex.Message.ToString();
            }
        }

        private async void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ErrorMsgBox.Text = String.Empty;
                searchvalue = SearchLastNameBox.Text;
                Task<Customer> task = new Task<Customer>(SearchDB);
                switchOnReadOnly(true);
                task.Start();
                StatusLabel.Content = "Search started.............";
                Customer customer = await task;
                UpdateGUI(customer);
                StatusLabel.Content = "Search ended...............";

            }
            catch (FaultException ex)
            {
                ErrorMsgBox.Text = ex.Reason.ToString();
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
            catch (FaultException ex)
            {
                ErrorMsgBox.Dispatcher.Invoke(new Action(() => ErrorMsgBox.Text = ex.Message.ToString()));
                return null;
            }
            catch (CommunicationException ex)
            {
                ErrorMsgBox.Dispatcher.Invoke(new Action(() => ErrorMsgBox.Text = ex.Message.ToString()));
                return null;
            }
        }

        private void UpdateGUI(Customer customer)
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
            switchOnReadOnly(false);
        }

        private void switchOnReadOnly(Boolean switchBool)
        {
            if (switchBool)
            {
                SearchBtn.IsEnabled = false;
                GoBtn.IsEnabled = false;
                SearchProgressBar.IsIndeterminate = true;
                FirstNameBox.Dispatcher.Invoke(new Action(() => FirstNameBox.IsReadOnly = true));
                LastNameBox.Dispatcher.Invoke(new Action(() => LastNameBox.IsReadOnly = true));
                BalanceBox.Dispatcher.Invoke(new Action(() => BalanceBox.IsReadOnly = true));
                AcctNoBox.Dispatcher.Invoke(new Action(() => AcctNoBox.IsReadOnly = true));
                PinNumBox.Dispatcher.Invoke(new Action(() => PinNumBox.IsReadOnly = true));
                IndexBox.Dispatcher.Invoke(new Action(() => IndexBox.IsReadOnly = true));
                SearchLastNameBox.Dispatcher.Invoke(new Action(() => SearchLastNameBox.IsReadOnly = true));
            }
            else
            {
                SearchBtn.IsEnabled = true;
                GoBtn.IsEnabled = true;
                SearchProgressBar.IsIndeterminate = false;
                FirstNameBox.Dispatcher.Invoke(new Action(() => FirstNameBox.IsReadOnly = false));
                LastNameBox.Dispatcher.Invoke(new Action(() => LastNameBox.IsReadOnly = false));
                BalanceBox.Dispatcher.Invoke(new Action(() => BalanceBox.IsReadOnly = false));
                AcctNoBox.Dispatcher.Invoke(new Action(() => AcctNoBox.IsReadOnly = false));
                PinNumBox.Dispatcher.Invoke(new Action(() => PinNumBox.IsReadOnly = false));
                IndexBox.Dispatcher.Invoke(new Action(() => IndexBox.IsReadOnly = false));
                SearchLastNameBox.Dispatcher.Invoke(new Action(() => SearchLastNameBox.IsReadOnly = false));
            }
        }
    }
}
