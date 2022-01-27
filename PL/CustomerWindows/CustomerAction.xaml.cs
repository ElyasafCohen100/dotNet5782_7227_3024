using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using BO;

namespace PL
{
    public partial class CustomerActions : Window
    {
        private BlApi.IBL BLObject;
        private CustomerToList selcetedCustomerToList;

        #region Action's Constructor
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

            Customer customer = BLObject.GetCustomerByIdBL(selcetedCustomerToList.Id);

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
        #endregion


        #region Add's Conctructor
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
        #endregion


        #region Add a new Customer
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

                this.CloseButton_Click(sender, e);
            }
            catch (InvalidInputException exeption)
            {
                MessageBox.Show(exeption.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ObjectAlreadyExistException)
            {
                MessageBox.Show("Customer has already exist" ,"Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion


        #region Update Customer
        private void UpdateCustomerDetailes_Click(object sender, RoutedEventArgs e)
        {
            string newName = CustomerNameTB.Text;
            string newPhone = CustomerPhoneTB.Text;
            try
            {
                BLObject.UpdateCustomerDetailesBL(selcetedCustomerToList.Id, newName, newPhone);
                MessageBox.Show("Customer has been update sucssesfuly",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                this.CloseButton_Click(sender, e);
            }
            catch (InvalidInputException exepion)
            {
                MessageBox.Show(exepion.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion


        #region Display Parcel From Customer
        /// <summary>
        /// finding the parcel that the customer send, and send it to the ParcelAction window.
        /// </summary>
        private void ParcelFromCustomerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Customer customer = BLObject.GetCustomerByIdBL(selcetedCustomerToList.Id);
            ParcelToList selectedParcel = BLObject.GetParcelToList(customer.ParcelFromCustomerList[ParcelFromCustomerList.SelectedIndex].Id);
            new ParcelActions(selectedParcel).Show();
        }
        #endregion


        #region Display parcel To Customer
        /// <summary>
        /// finding the parcel that the customer needs to get, and send it to the ParcelAction window.
        /// </summary>
        private void ParcelToCustomerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Customer customer = BLObject.GetCustomerByIdBL(selcetedCustomerToList.Id);
            ParcelToList selectedParcel = BLObject.GetParcelToList(customer.ParcelToCustomerList[ParcelToCustomerList.SelectedIndex].Id);
            new ParcelActions(selectedParcel).Show();
        }
        #endregion


        #region Customer TextBoxe's function
        private void CustomerNameTB_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateCustomerButton.IsEnabled = true;
        }

        private void CustomerPhoneTB_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateCustomerButton.IsEnabled = true;
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
        #endregion


        #region Delete
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
            catch(System.InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message,
                                "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
     
        
        #region Close Window
        private void CloseButton_Click(object sender, RoutedEventArgs e)
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
        #endregion
    }
}
