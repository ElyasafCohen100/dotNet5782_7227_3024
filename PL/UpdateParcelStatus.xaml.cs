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
    public partial class UpdateParcelStatus : Window
    {
        private IBL.IBL BLObject;
        private ViewDroneList viewDroneList;
        private int droneId;
        public UpdateParcelStatus(IBL.IBL BLObject, ViewDroneList viewDroneList, int droneId)
        {
            InitializeComponent();
            this.BLObject = BLObject;
            this.viewDroneList = viewDroneList;
            this.droneId = droneId;
        }
        private void SendDroneToDeliveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.UpdateDroneIdOfParcelBL(droneId);
                MessageBox.Show("Drone sent sucssesfuly");
                viewDroneList.DroneListView.Items.Refresh();
                this.Close();
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input");
            }
        }

        private void UpdateParcelToPickedUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.UpdatePickedUpParcelByDroneIdBL(droneId);
                MessageBox.Show("Parcel status updated sucssesfuly");
                viewDroneList.DroneListView.Items.Refresh();
                this.Close();
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input, there is no parcel to delivered");
            }
            catch (NotValidRequestException)
            {
                MessageBox.Show("Could not update parcel status to picked up");
            }
        }
        private void UpdateParcelToDelivered_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.UpdateDeliveredParcelByDroneIdBL(droneId);
                MessageBox.Show("Parcel status updated sucssesfuly");
                viewDroneList.DroneListView.Items.Refresh();
                this.Close();
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input, there is no parcel to delivered");
            }
            catch (NotValidRequestException)
            {
                MessageBox.Show("Could not update parcel status to delivered beacause the drone has already delivered the parcel");
            }
        }
    }
}
