using BO;
using System;
using System.Windows;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for SignUnCustonerWindow.xaml
    /// </summary

    public partial class SignUpCustonerWindow : Window
    {

        private BlApi.IBL BLObject;

        #region Constructor
        public SignUpCustonerWindow()
        {
            InitializeComponent();

            try
            {
                BLObject = BlApi.BlFactory.GetBl();
            }
            catch(DalApi.DalConfigException e)
            {
                MessageBox.Show(e.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            DataContext = false;
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


        #region Sign up
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = new();

            int.TryParse(CustomerIdTB.Text, out int Id);
            customer.Id = Id;
            customer.Name = CustomerNameTB.Text;
            customer.Phone = CustomerPhone.Text;

            double.TryParse(CustomerLongitudeTB.Text, out Double Latitude);
            customer.Location.Latitude = Latitude;

            double.TryParse(CustomerLongitudeTB.Text, out Double Longitude);
            customer.Location.Longitude = Longitude;

            customer.UserName = UserNameTB.Text;
            customer.Password = PasswordTB.Text;

            try
            {
                BLObject.AddNewCustomerBL(customer);
                MessageBox.Show("you have been registered succssesfully", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                this.CloseButton_Click(sender, e);
                new MainCustomerWindow(customer.UserName).Show();
            }
            catch (InvalidInputException exception)
            {
                MessageBox.Show(exception.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ObjectAlreadyExistException)
            {
                MessageBox.Show("Customer already exist", "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
