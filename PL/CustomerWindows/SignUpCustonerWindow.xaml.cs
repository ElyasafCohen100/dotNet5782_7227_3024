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
            catch (DalApi.DalConfigException e)
            {
                MessageBox.Show(e.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            DataContext = false;
        }
        #endregion


        #region Sign up
        /// <summary>
        /// Check if the fields of the TextBox are full
        /// </summary>
        /// <returns>true if all the fields are filled, otherwise - flase</returns>
        private bool IsFieldsAreFull()
        {
            if (CustomerIdTB.Text != String.Empty &&
                CustomerNameTB.Text != String.Empty &&
                CustomerPhoneTB.Text != String.Empty &&
                CusomerLatitudeTB.Text != String.Empty &&
                CustomerLongitudeTB.Text != String.Empty &&
                UserNameTB.Text != String.Empty &&
                PasswordTB.Text != String.Empty)
            {
                return true;
            }
            return false;
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsFieldsAreFull())
            {
                Customer customer = new();

                int.TryParse(CustomerIdTB.Text, out int Id);
                customer.Id = Id;
                customer.Name = CustomerNameTB.Text;
                customer.Phone = CustomerPhoneTB.Text;

                double.TryParse(CustomerLongitudeTB.Text, out double Latitude);
                customer.Location.Latitude = Latitude;

                double.TryParse(CustomerLongitudeTB.Text, out double Longitude);
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
            else
            {
                MessageBox.Show("One or more of your fields is empty", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //---- sound while you're clicking on the button ----//
                System.Media.SoundPlayer player = new(@"sources/clickSound.wav");
                player.Load();
                player.PlaySync();

                new MainCustomerWindow(UserNameTB.Text).Show();
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
