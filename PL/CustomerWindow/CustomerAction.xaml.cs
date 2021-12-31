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
    /// Interaction logic for CustomerActions.xaml
    /// </summary>
    public partial class CustomerActions : Window
    {

        private BlApi.IBL BLObject = BlApi.BlFactory.GetBl();
        private CustomerToList selcetedCustomerToList;

        public CustomerActions(CustomerToList selcetedCustomerToList)
        {

            Customer customer = BLObject.FindCustomerByIdBL(selcetedCustomerToList.Id);

            InitializeComponent();
            this.selcetedCustomerToList = selcetedCustomerToList;

            DataContext = false;
            grid1.DataContext = customer;

            CustomerId.IsEnabled = false;
            CustomerIdTB.IsEnabled = false;

            CustomerLatitude.IsEnabled = false;
            CustomerLatitudeTB.IsEnabled = false;

            CustomerLongitude.IsEnabled = false;
            CustomerLongitudeTB.IsEnabled = false;
            AddNewCustomerButton.Visibility = Visibility.Hidden;

            ParcelFromCustomerList.ItemsSource = customer.ParcelFromCustomerList;
            ParcelToCustomerList.ItemsSource = customer.ParcelToCustomerList;
        }
        public CustomerActions()
        {
            InitializeComponent();
            DataContext = false;
            UpdateCustomerButton.Visibility = Visibility.Hidden;
            ParcelFromCustomerList.Visibility = Visibility.Hidden;
            ParcelToCustomerList.Visibility = Visibility.Hidden;
        }

        private void AddCustomerBustton_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = new();

            int.TryParse(CustomerIdTB.Text, out int Id);
            customer.Id = Id;
            customer.Name = CustomerNameTB.Text;
            customer.Phone = CustomerPhoneTB.Text;
            double.TryParse(CustomerLatitudeTB.Text, out double Latitude);
            customer.Location.Latitude = Latitude;

            double.TryParse(CustomerLongitudeTB.Text, out double Longitude);
            customer.Location.Longitude = Longitude;

            try
            {
                BLObject.AddNewCustomerBL(customer);
                MessageBox.Show("Customer has been added sucssesfuly",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input", "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCustomerDetailes_Click(object sender, RoutedEventArgs e)
        {
            string newName = CustomerNameTB.Text;
            string newPhone = CustomerPhoneTB.Text;
            try
            {
                BLObject.UpdateCustomerDetailesBL(selcetedCustomerToList.Id, newName, newPhone);
                MessageBox.Show("Customer has been added sucssesfuly",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input", "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext.Equals(false)) e.Cancel = true;
        }

        private void ParcelFromCustomerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList selectedCustomer = BLObject.ViewCustomerToList().ToList()[ParcelFromCustomerList.SelectedIndex];
        }

        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.DeleteCustomer(selcetedCustomerToList.Id);
                MessageBox.Show("Customer has been removed",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                this.CloseButton_Click(sender, e);
            }
            catch (ObjectNotFoundException exception)
            {
                MessageBox.Show(exception.Message,
                                "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
