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
    /// Interaction logic for CustomerAction.xaml
    /// </summary>
    public partial class CustomerAction : Window
    {
        private BlApi.IBL Blobject;
        private CustomerToList selectedCustomerToList;

        //CustomerAction C-tor
        public CustomerAction(BlApi.IBL Blobject, CustomerToList selectedCustomerToList)
        {
            InitializeComponent();
            this.Blobject = Blobject;
            this.selectedCustomerToList = selectedCustomerToList;

            DataContext = false;

            CustomerIdTB.IsEnabled = false;
            CustomerNameTB.IsEnabled = false;
            CustomerPhoneTB.IsEnabled = false;
            CustomerLongitudeTB.IsEnabled = false;
            CoustomerLatitudeTB.IsEnabled = false;
        }

        private void AddNewCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            BO.Customer newCustomer = new();
            int.TryParse(CustomerIdTB.Text, out int id);
          
            newCustomer.Id = id;
            newCustomer.Name = CustomerNameTB.Text;
            newCustomer.Phone = CustomerPhoneTB.Text;

            double.TryParse(CustomerLongitudeTB.Text, out double Longitude);
            newCustomer.Location.Latitude = Longitude;

            double.TryParse(CoustomerLatitudeTB.Text, out double Latitude);
            newCustomer.Location.Latitude = Latitude;

            try
            {
                Blobject.AddNewCustomerBL(newCustomer);
                MessageBox.Show("Customer has been added sucssefuly", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input", "Operation failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }
    }
}
