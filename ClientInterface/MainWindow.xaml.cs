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
using System.Drawing;
using BusinessTier;
using System.ServiceModel;

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

                if (index > 0 && index < 21)
                {
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
                else
                {
                    ErrorMsgBox.Text = "Index entered is out of range. Please check the total number of items.";

                }

            }
            
            catch(FormatException ex)
            {
                Console.WriteLine(ex.Message);
                ErrorMsgBox.Text = "Index entered is not in the correct format. Please try again.";
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            string fName = "", lName = "", profPicPath = "";
            int bal = 0;
            uint acct = 0, pin = 0;

            try
            {
                int index = foob.SearchCustomer(SearchLastNameBox.Text);
                foob.GetValuesForEntry(index, out acct, out pin, out bal, out fName, out lName, out profPicPath);

                FirstNameBox.Text = fName;
                LastNameBox.Text = lName;
                BalanceBox.Text = bal.ToString("C");
                AcctNoBox.Text = acct.ToString("D4");
                PinNumBox.Text = pin.ToString("D4");
                IndexBox.Text = index.ToString();

                BitmapImage profilePicture = new BitmapImage();
                profilePicture.BeginInit();
                profilePicture.UriSource = new Uri(profPicPath);
                profilePicture.EndInit();

                ProfileImage.Source = profilePicture;
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                ErrorMsgBox.Text = "Bad Input detected";
            }
            catch (FaultException<ArgumentOutOfRangeException>)
            {
                ErrorMsgBox.Text = "Out of Range";
            }
            
        }
    }
}
