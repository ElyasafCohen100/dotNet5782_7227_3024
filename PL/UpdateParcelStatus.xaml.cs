using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IBL.BO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace PL
{
    /// <summary>
    /// Interaction logic for SendDroneToDelivery.xaml
    /// </summary>
    public partial class UpdateDroneStatus : Window
    {
        private IBL.IBL BLObject;
        public UpdateDroneStatus(IBL.IBL BLObject)
        {
            InitializeComponent();
            this.BLObject = BLObject;
        }
        //----------------  DroneIdTextBox ----------------//

        private void DroneIdTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DroneIdTextBox.Text == "Enter Id")
                DroneIdTextBox.Clear();

        }
        private void DroneIdTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DroneIdTextBox.Text == String.Empty)
                DroneIdTextBox.Text = "Enter Id";
        }
        private void DroneIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void SendDroneToDeliveryButton_Click(object sender, RoutedEventArgs e)
        {
            int Id;
            int.TryParse(DroneIdTextBox.Text, out Id);
            try
            {
                BLObject.UpdateDroneIdOfParcelBL(Id);
                MessageBox.Show("Drone sent sucssesfuly");
                this.Close();
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input");
            }
        }

        private void UpdateParcelToPickedUp_Click(object sender, RoutedEventArgs e)
        {
            int Id;
            int.TryParse(DroneIdTextBox.Text, out Id);
            try
            {
                BLObject.UpdatePickedUpParcelByDroneIDBL(Id);
                MessageBox.Show("Parcel status updated sucssesfuly");
                this.Close();
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input");
            }
            catch (NotValidRequestException)
            {
                MessageBox.Show("Invalid input");
            }
        }

        private void UpdateParcelToDelivered_Click(object sender, RoutedEventArgs e)
        {
            int Id;
            int.TryParse(DroneIdTextBox.Text, out Id);
            try
            {
                BLObject.UpdateDeliveredParcelByDroneIdBL(Id);
                MessageBox.Show("Parcel status updated sucssesfuly");
                this.Close();
            }
            catch (NotValidRequestException)
            {
                MessageBox.Show("Invalid input");
            }
        }
    }
}
