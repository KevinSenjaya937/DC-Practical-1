﻿using System;
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
using System.ComponentModel;

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
                ErrorMsgBox.Text = ex.Reason.ToString();
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var regexItem = new System.Text.RegularExpressions.Regex("^[a-zA-Z]*$");
            
            try
            {
                ErrorMsgBox.Text = String.Empty;
                switchOnReadOnly(true);
                search = SearchDB;
                AsyncCallback callback;
                callback = onSearchCompletion;
                IAsyncResult result = search.BeginInvoke(SearchLastNameBox.Text, callback, null);
                StatusLabel.Content = "Search Started..."; 
            }
            catch (FaultException ex)
            {
                ErrorMsgBox.Text = ex.Reason.ToString();
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
            catch (CommunicationException ex)
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
                FirstNameBox.Dispatcher.Invoke(new Action(() => FirstNameBox.Text = "First Name"));
                LastNameBox.Dispatcher.Invoke(new Action(() => LastNameBox.Text = "Last Name"));
                BalanceBox.Dispatcher.Invoke(new Action(() => BalanceBox.Text = "Balance"));
                AcctNoBox.Dispatcher.Invoke(new Action(() => AcctNoBox.Text = "Account Number"));
                PinNumBox.Dispatcher.Invoke(new Action(() => PinNumBox.Text = "Pin Number"));
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
            StatusLabel.Dispatcher.Invoke(new Action(() => StatusLabel.Content = "Search Ended..."));
        }

        private void switchOnReadOnly(Boolean switchBool)
        {
            if (switchBool)
            {
                SearchProgressBar.Dispatcher.Invoke(new Action(() => SearchProgressBar.IsIndeterminate = true));
                SearchBtn.Dispatcher.Invoke(new Action(() => SearchBtn.IsEnabled = false));
                GoBtn.Dispatcher.Invoke(new Action(() => GoBtn.IsEnabled = false));
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
                SearchProgressBar.Dispatcher.Invoke(new Action(() => SearchProgressBar.IsIndeterminate = false));
                SearchBtn.Dispatcher.Invoke(new Action(() => SearchBtn.IsEnabled = true));
                GoBtn.Dispatcher.Invoke(new Action(() => GoBtn.IsEnabled = true));
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
