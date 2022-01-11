using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerActions.xaml
    /// </summary>
    public partial class CustomerActions : Window
    {
        private BlApi.IBL BLObject;
        private CustomerToList selcetedCustomerToList;

        public CustomerActions(CustomerToList selcetedCustomerToList)
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
            this.selcetedCustomerToList = selcetedCustomerToList;

            Customer customer = BLObject.FindCustomerByIdBL(selcetedCustomerToList.Id);

            DataContext = false;
            grid1.DataContext = customer;

            CustomerId.IsEnabled = false;
            CustomerIdTB.IsEnabled = false;

            CustomerLatitude.IsEnabled = false;
            CustomerLatitudeTB.IsEnabled = false;

            CustomerLongitude.IsEnabled = false;
            CustomerLongitudeTB.IsEnabled = false;
            UserName.IsEnabled = false;
            UserNameTB.IsEnabled = false;

            Password.IsEnabled = false;
            PasswordTB.IsEnabled = false;

            AddNewCustomerButton.Visibility = Visibility.Hidden;

            ParcelFromCustomerList.ItemsSource = customer.ParcelFromCustomerList;
            ParcelToCustomerList.ItemsSource = customer.ParcelToCustomerList;
        }
        public CustomerActions()
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

            DataContext = false;
          
            UpdateCustomerButton.Visibility = Visibility.Hidden;
            DeleteCustomerButton.Visibility = Visibility.Hidden;
         
            ParcelFromCustomerList.Visibility = Visibility.Hidden;
            ParcelToCustomerList.Visibility = Visibility.Hidden;
          
            ParcelFromCustomer.Visibility = Visibility.Hidden;
            ParcelToCustomer.Visibility = Visibility.Hidden;
           
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
            customer.UserName = UserNameTB.Text;
            customer.Password = PasswordTB.Text;

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
            catch (ObjectAlreadyExistException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.CloseButton_Click(sender,e);
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
            this.CloseButton_Click(sender, e);
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
            }catch(System.InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message,
                                "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CustomerNameTB_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateCustomerButton.IsEnabled = true;
        }

        private void CustomerPhoneTB_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateCustomerButton.IsEnabled = true;
        }

        private void ParcelFromCustomerList_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            ParcelToList selectedParcel = BLObject.ViewParcelToList().ToList()[ParcelFromCustomerList.SelectedIndex];
            new ParcelActions(selectedParcel).Show();
        }

        private void ParcelToCustomerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList selectedParcel = BLObject.ViewParcelToList().ToList()[ParcelToCustomerList.SelectedIndex];
            new ParcelActions(selectedParcel).Show();
        }

        private void CustomerIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CustomerNameTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-z,A-Z,0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CustomerPhoneTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
