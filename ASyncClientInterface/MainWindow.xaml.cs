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
using System.Runtime.Remoting.Messaging;

namespace ASyncClientInterface
{
    public delegate Customer Search(string searchValue);
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusinessServerInterface foob;
        private Search search;
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
                ErrorMsgBox.Text = "Index entered is not in the correct format. Please try again.";
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var regexItem = new System.Text.RegularExpressions.Regex("^[a-zA-Z]*$");

            if (regexItem.IsMatch(SearchLastNameBox.Text))
            {
                switchOnReadOnly(true);
                
                search = SearchDB;
                AsyncCallback callback;
                callback = onSearchCompletion;
                IAsyncResult result = search.BeginInvoke(SearchLastNameBox.Text, callback, null);
            }
            else
            {
                ErrorMsgBox.Text = "Bad Input Detected. Input must be a valid last name with no special characters.";
            }
        }

        private Customer SearchDB(string value)
        {
            try
            {
                foob.SearchCustomer(value, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string profPicPath);

                Customer customer = new Customer
                {
                    acctNo = acctNo,
                    pin = pin,
                    balance = bal,
                    firstname = fName,
                    lastname = lName,
                    profPicPath = profPicPath
                };
                return customer;
            }
            catch (FaultException ex)
            {
                ErrorMsgBox.Dispatcher.Invoke(new Action(() => ErrorMsgBox.Text = ex.Message.ToString()));
                return null;
            }
        }

        private void UpdateGUI(Customer customer)
        {
            if (customer != null)
            {
                FirstNameBox.Dispatcher.Invoke(new Action(() => FirstNameBox.Text = customer.firstname));
                LastNameBox.Dispatcher.Invoke(new Action(() => LastNameBox.Text = customer.lastname));
                BalanceBox.Dispatcher.Invoke(new Action(() => BalanceBox.Text = customer.balance.ToString("C")));
                AcctNoBox.Dispatcher.Invoke(new Action(() => AcctNoBox.Text = customer.acctNo.ToString("D4")));
                PinNumBox.Dispatcher.Invoke(new Action(() => PinNumBox.Text = customer.pin.ToString("D4")));

                BitmapImage profilePicture = new BitmapImage();
                profilePicture.BeginInit();
                profilePicture.UriSource = new Uri(customer.profPicPath);
                profilePicture.EndInit();
                profilePicture.Freeze();
                ProfileImage.Dispatcher.Invoke(new Action(() => ProfileImage.Source = profilePicture));
            }
            else
            {
                FirstNameBox.Dispatcher.Invoke(new Action(() => FirstNameBox.Text = String.Empty));
                LastNameBox.Dispatcher.Invoke(new Action(() => LastNameBox.Text = String.Empty));
                BalanceBox.Dispatcher.Invoke(new Action(() => BalanceBox.Text = String.Empty));
                AcctNoBox.Dispatcher.Invoke(new Action(() => AcctNoBox.Text = String.Empty));
                PinNumBox.Dispatcher.Invoke(new Action(() => PinNumBox.Text = String.Empty));
                ProfileImage.Dispatcher.Invoke(new Action(() => ProfileImage.Source =  null));
            }
        }

        private void onSearchCompletion(IAsyncResult asyncResult)
        {
            Customer customer;
            Search search;
            AsyncResult asyncObject = (AsyncResult)asyncResult;
            if (asyncObject.EndInvokeCalled == false)
            {
                search = (Search)asyncObject.AsyncDelegate;
                customer = search.EndInvoke(asyncObject);
                UpdateGUI(customer);      
            }
            asyncObject.AsyncWaitHandle.Close();
            switchOnReadOnly(false);
        }

        private void switchOnReadOnly(Boolean switchBool)
        {
            if (switchBool)
            {
                FirstNameBox.Dispatcher.Invoke(new Action(() => FirstNameBox.IsReadOnly = true));
                LastNameBox.Dispatcher.Invoke(new Action(() => LastNameBox.IsReadOnly = true));
                BalanceBox.Dispatcher.Invoke(new Action(() => BalanceBox.IsReadOnly = true));
                AcctNoBox.Dispatcher.Invoke(new Action(() => AcctNoBox.IsReadOnly = true));
                PinNumBox.Dispatcher.Invoke(new Action(() => PinNumBox.IsReadOnly = true));
                IndexBox.Dispatcher.Invoke(new Action(() => IndexBox.IsReadOnly = true));
            }
            else
            {
                FirstNameBox.Dispatcher.Invoke(new Action(() => FirstNameBox.IsReadOnly = false));
                LastNameBox.Dispatcher.Invoke(new Action(() => LastNameBox.IsReadOnly = false));
                BalanceBox.Dispatcher.Invoke(new Action(() => BalanceBox.IsReadOnly = false));
                AcctNoBox.Dispatcher.Invoke(new Action(() => AcctNoBox.IsReadOnly = false));
                PinNumBox.Dispatcher.Invoke(new Action(() => PinNumBox.IsReadOnly = false));
                IndexBox.Dispatcher.Invoke(new Action(() => IndexBox.IsReadOnly = false));
            }
        }
    }
}
