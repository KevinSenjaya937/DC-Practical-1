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

namespace ClientInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CustomerServerInterface foob;
        public MainWindow()
        {
            InitializeComponent();
            ChannelFactory<CustomerServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8100/CustomerService";
            foobFactory = new ChannelFactory<CustomerServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            TotalNumText.Text = foob.GetNumEntries().ToString();

        }

        private void GoBtn_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            string fName = "", lName = "";
            int bal = 0;
            uint acct = 0, pin = 0;

            index = Int32.Parse(IndexBox.Text);
            foob.GetValuesForEntry(index, out acct, out pin, out bal, out fName, out lName);

            FirstNameBox.Text = fName;
            LastNameBox.Text = lName;
            BalanceBox.Text = bal.ToString("C");
            AcctNoBox.Text = acct.ToString();
            PinNumBox.Text = pin.ToString("D4");
        }
    }
}
