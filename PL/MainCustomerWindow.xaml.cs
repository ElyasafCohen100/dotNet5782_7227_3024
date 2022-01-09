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
using System.Windows.Shapes;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainCustomerWindow.xaml
    /// </summary>
    public partial class MainCustomerWindow : Window
    {
        private BlApi.IBL BLObject = BlApi.BlFactory.GetBl();
        private string userName;
        public MainCustomerWindow(string userName)
        {
            InitializeComponent();
            this.userName = userName;

        }

        private void UpdateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            //Customer customer = BLObject.FindCustomerByUserName(userName);
            //CustomerToList customerToList = BLObject.FindCustomerToList(customer.Id);
            //new CustomerActions(customerToList);
        }
    }
}
