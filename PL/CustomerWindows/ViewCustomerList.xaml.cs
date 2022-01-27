using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private BlApi.IBL BLObject;
        public ViewCustomerList()
        {
            InitializeComponent();
            try
            {
                BLObject = BlApi.BlFactory.GetBl();
            }
            catch (DalApi.DalConfigException e)
            {
                MessageBox.Show(e.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            CustomerListView.ItemsSource = BLObject.GetAllCustomerToList();
            DataContext = false;
        }

        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CustomerListView.SelectedIndex >= 0)
            {
                CustomerToList selectedCustomer = BLObject.GetAllCustomerToList().ToList()[CustomerListView.SelectedIndex];
                if (new CustomerActions(selectedCustomer).ShowDialog() == false)
                    CustomerListView.ItemsSource = BLObject.GetAllCustomerToList();
            }
        }

        private void AddNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (new CustomerActions().ShowDialog() == false)
                CustomerListView.ItemsSource = BLObject.GetAllCustomerToList();
        }

        private void CLoseButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext.Equals(false)) e.Cancel = true;
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}