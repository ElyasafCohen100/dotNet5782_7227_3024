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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!BLObject.IsAdminRegistered(UserNameTB.Text))
            {
                if (!BLObject.IsCustomerRegisered(UserNameTB.Text))
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
        private void UserNameTB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UserNameTB.Text == "User Name")
            {
                UserNameTB.Clear();
            }
        }
        private void PasswordTB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordTB.Text == "Password")
            {
                PasswordTB.Clear();
            }
        }
        private void UserNameTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UserNameTB.Text == String.Empty)
            {
                UserNameTB.Text = "User Name";
            }


            //if (UserNameTB.Text != "User Name")
            //{
            //    int.TryParse(UserNameTB.Text, out int Id);
            //    if (Id > 10000 || Id < 1000)
            //    {
            //        UserNameTB.BorderBrush = Brushes.Red;
            //        UserNameTB.Foreground = Brushes.Red;
            //    }
            //}
        }
        private void PasswordTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordTB.Text == String.Empty)
            {
                PasswordTB.Text = "Password";
            }

            //if (UserNameTB.Text != "User Name")
            //{
            //    int.TryParse(UserNameTB.Text, out int Id);
            //    if (Id > 10000 || Id < 1000)
            //    {
            //        UserNameTB.BorderBrush = Brushes.Red;
            //        UserNameTB.Foreground = Brushes.Red;
            //    }
            //}
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }
    }
}