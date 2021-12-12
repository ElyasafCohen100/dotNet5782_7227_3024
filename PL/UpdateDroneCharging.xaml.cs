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


namespace PL
{
    /// <summary>
    /// Interaction logic for UpdateDroneChrging.xaml
    /// </summary>
    public partial class UpdateDroneCharging : Window
    {
        private IBL.IBL BLObject;
        private ViewDroneList viewDroneList;
        private int droneId;
        public UpdateDroneCharging(IBL.IBL BLObject, ViewDroneList viewDroneList, int droneId)
        {
            InitializeComponent();
            this.BLObject = BLObject;
            this.viewDroneList = viewDroneList;
            this.droneId = droneId;
        }
        private void UpdateDroneToChargingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.UpdateDroneToChargingBL(droneId);
                MessageBox.Show("Drone updated sucssesfuly");
                viewDroneList.DroneListView.Items.Refresh();
                this.Close();
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input");
            }
            catch (ObjectAlreadyExistException)
            {
                MessageBox.Show("Drone is already exist");
            }
            catch (OutOfBatteryException)
            {
                MessageBox.Show("Could not send the drone to charging because there is not enough battery");
            }
            catch (ObjectNotFoundException)
            {
                MessageBox.Show("Could not update drone to charging");
            }
        }

        private void UpdateDroneFromChargingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.UpdateDroneFromChargingBL(droneId);
                MessageBox.Show("Drone updated sucssesfuly");
                this.Close();
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input");
            }
            catch (ObjectAlreadyExistException)
            {
                MessageBox.Show("Drone is already exist");
            }
            catch (OutOfBatteryException)
            {
                MessageBox.Show("Could not send the drone to charging because there is not enough battery");
            }
        }
    }
}
