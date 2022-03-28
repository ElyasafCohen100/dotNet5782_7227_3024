using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BlApi.IBL BLObject;

        #region Constuctor
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
        #endregion


        #region Login
        private void Login()
        {
            if (!BLObject.IsAdminRegistered(UserNameTB.Text, PasswordPB.Password))
            {
                if (!BLObject.IsCustomerRegistered(UserNameTB.Text, PasswordPB.Password))
                {
                    MessageBox.Show("username or password incorrect", "Operation Failure",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else 
                {
                    new MainCustomerWindow(UserNameTB.Text).Show();
                }
            }
            else
            {
                new MainAdminWindow().Show();
            }
        }
       
        private void UserNameTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-z,A-Z,0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        
        private void PasswordPB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-z,A-Z,0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
       
        private void UserNameTB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UserNameTB.Text == "User Name")
            {
                UserNameTB.Clear();
            }
            else if (BLObject.IsAdminExsist(UserNameTB.Text))
            {
                UserNameMessage.Visibility = Visibility.Hidden;
            }
            else if (BLObject.IsCustomerExsist(UserNameTB.Text))
            {
                UserNameMessage.Visibility = Visibility.Hidden;
            }
        }
        
        private void UserNameTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UserNameTB.Text == String.Empty)
            {
                UserNameTB.Text = "User Name";
            }
            if (!BLObject.IsAdminExsist(UserNameTB.Text))
            {
                UserNameMessage.Visibility = Visibility.Visible;
            }
            else if (BLObject.IsAdminExsist(UserNameTB.Text))
            {
                UserNameMessage.Visibility = Visibility.Hidden;
            }
            else if (!BLObject.IsCustomerExsist(UserNameTB.Text))
            {
                UserNameMessage.Visibility = Visibility.Visible;
            }
            else if (BLObject.IsCustomerExsist(UserNameTB.Text))
            {
                UserNameMessage.Visibility = Visibility.Hidden;
            }
        }
       
        private void PasswordTB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordPB.Password == "Password")
            {
                PasswordPB.Clear();
            }
        }
        
        private void PasswordTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordPB.Password == String.Empty)
            {
                PasswordPB.Password = "Password";
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            PasswordTB.Text = PasswordPB.Password;
            PasswordPB.Visibility = Visibility.Hidden;
            PasswordTB.Visibility = Visibility.Visible;
        }

        private void ShowPassword_UnChecked(object sender ,RoutedEventArgs e)
        {
            PasswordPB.Password = PasswordTB.Text;
            PasswordTB.Visibility = Visibility.Hidden;
            PasswordPB.Visibility = Visibility.Visible;
        }
              
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //---- sound while you're clicking on the button ----//
                System.Media.SoundPlayer player = new(@"sources/clickSound.wav");
                player.Load();
                player.PlaySync();

                Login();
            }
        }
        #endregion


        #region Sign Up
        private void SingUpButton_Click(object sender, RoutedEventArgs e)
        {
            new SignUpCustonerWindow().Show();
        }
        #endregion


        #region Close
        private void Close_Click(object sender, RoutedEventArgs e)
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


        #region VIP
        private void VIP_Click(object sender, RoutedEventArgs e)
        {
            new MainAdminWindow().Show();
        }

        private void Script_Button_Click(object sender, RoutedEventArgs e)
        {

            string dir = Directory.GetCurrentDirectory();

            List<DO.Drone> list2 = new(Dal.DalObject.DalObj.GetDroneList());
            FileStream file2 = new FileStream(dir + @"\Data\Drones.xml", FileMode.Create);
            XmlSerializer x2 = new XmlSerializer(list2.GetType());
            x2.Serialize(file2, list2);
            file2.Close();

            List<DO.Admin> list3 = new(Dal.DalObject.DalObj.GetAdminsList());
            FileStream file3 = new FileStream(dir + @"\Data\Admins.xml", FileMode.Create);
            XmlSerializer x3 = new XmlSerializer(list3.GetType());
            x3.Serialize(file3, list3);
            file3.Close();

            List<DO.Customer> list4 = new(Dal.DalObject.DalObj.GetCustomerList());
            FileStream file4 = new FileStream(dir + @"\Data\Customers.xml", FileMode.Create);
            XmlSerializer x4 = new XmlSerializer(list4.GetType());
            x4.Serialize(file4, list4);
            file4.Close();

            List<DO.DroneCharge> list5 = new(Dal.DalObject.DalObj.GetDroneChargeList(x => x.DroneId == x.DroneId));
            FileStream file5 = new FileStream(dir + @"\Data\DroneCharges.xml", FileMode.Create);
            XmlSerializer x5 = new XmlSerializer(list5.GetType());
            x5.Serialize(file5, list5);
            file5.Close();

            List<DO.Parcel> list6 = new(Dal.DalObject.DalObj.GetParcelList());
            FileStream file6 = new FileStream(dir + @"\Data\Parcels.xml", FileMode.Create);
            XmlSerializer x6 = new XmlSerializer(list6.GetType());
            x6.Serialize(file6, list6);
            file6.Close();

            List<DO.Station> list7 = new(Dal.DalObject.DalObj.GetStations(x => x.Id == x.Id));
            FileStream file7 = new FileStream(dir + @"\Data\Stations.xml", FileMode.Create);
            XmlSerializer x7 = new XmlSerializer(list7.GetType());
            x7.Serialize(file7, list7);
            file7.Close();

        }
        #endregion
    }
}