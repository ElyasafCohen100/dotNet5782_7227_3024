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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BlApi.IBL BLObject;
        public MainWindow()
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

        private void Login()
        {
            if (!BLObject.IsAdminRegistered(UserNameTB.Text, PasswordPB.Password))
            {
                if (!BLObject.IsCustomerRegisered(UserNameTB.Text, PasswordPB.Password))
                    MessageBox.Show("The user is not exsist", "Operation Failure",
                                       MessageBoxButton.OK, MessageBoxImage.Error);

                else { }
                //TODO: CREATE CUSTOMER USER INTERFACE WINDOW
            }
            else
            {
                new MainAdminWindow().Show();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
        private void UserNameTB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UserNameTB.Text == "User Name")
            {
                UserNameTB.Clear();
            }
        }
        private void PasswordTB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordPB.Password == "Password")
            {
                PasswordPB.Clear();
            }
            
        }
        private void UserNameTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UserNameTB.Text == String.Empty)
            {
                UserNameTB.Text = "User Name";
            }
        }
        private void PasswordTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordPB.Password == String.Empty)
            {
                PasswordPB.Password = "Password";
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }
    }
}