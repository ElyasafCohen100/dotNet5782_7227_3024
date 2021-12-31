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
    /// Interaction logic for ViewCustomerList.xaml
    /// </summary>
    public partial class ViewCustomerList : Window
    {
        private BlApi.IBL BLObject = BlApi.BlFactory.GetBl();
        public ViewCustomerList()
        {
            InitializeComponent();
            CustomerListView.ItemsSource = BLObject.ViewCustomerToList();
        }
        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CustomerListView.SelectedIndex >= 0)
            {
                CustomerToList selectedCustomer = BLObject.ViewCustomerToList().ToList()[CustomerListView.SelectedIndex];
                if (new CustomerActions(selectedCustomer).ShowDialog() == false)
                    CustomerListView.ItemsSource = BLObject.ViewCustomerToList();
            }
        }
        private void AddNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (new CustomerActions().ShowDialog() == false)
                CustomerListView.ItemsSource = BLObject.ViewCustomerToList();
        }

        private void CLoseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}