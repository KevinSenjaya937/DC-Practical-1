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
using DatabaseServer;
using System.ServiceModel;
using InterfaceToDLL;
using System.Drawing;

namespace ClientInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BankingInterface foob;
        public MainWindow()
        {
            InitializeComponent();
            ChannelFactory<BankingInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8100/CustomerService";
            foobFactory = new ChannelFactory<BankingInterface>(tcp, URL);
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
                
            }
            
            catch(FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
