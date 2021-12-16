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
using System.Text.RegularExpressions;
using IBL.BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for DroneActions.xaml
    /// </summary>
    public partial class DroneActions : Window
    {
        private IBL.IBL BLObject;
        private ViewDroneList viewDroneList;
        private DroneToList selcetedDrone;


        //Drone actions c-tor.
        public DroneActions(IBL.IBL BlObject, DroneToList droneToList, ViewDroneList viewDroneList)
        {
            InitializeComponent();

            Drone drone = BlObject.FindDroneByIdBL(droneToList.Id);
            selcetedDrone = droneToList;
            this.BLObject = BlObject;
            this.viewDroneList = viewDroneList;

            IdTextBox.IsEnabled = false;
            IdTextBox.Text = drone.Id.ToString();

            ModelTextBox.IsEnabled = false;
            ModelTextBox.Text = drone.Model;

            BatteryTB.IsEnabled = false;
            BatteryTB.Text = drone.BatteryStatus.ToString();

            MaxWeightTB.IsEnabled = false;
            MaxWeightTB.Text = drone.MaxWeight.ToString();

            StatusTB.IsEnabled = false;
            StatusTB.Text = drone.DroneStatus.ToString();

            DeliveryTB.IsEnabled = false;
            DeliveryTB.Text = drone.ParcelInDelivery.ToString();

            LatitudeTB.IsEnabled = false;
            LatitudeTB.Text = drone.CurrentLocation.Latitude.ToString();

            LongitudeTB.IsEnabled = false;
            LongitudeTB.Text = drone.CurrentLocation.Longitude.ToString();

            AddButton.Visibility = Visibility.Hidden;
            ClearButton.Visibility = Visibility.Hidden;
            BaseStationCB.Visibility = Visibility.Hidden;
            MaxWeightCB.Visibility = Visibility.Hidden;
        }

        //Add new Drone c-tor.
        public DroneActions(IBL.IBL BlObject, ViewDroneList viewDroneList)
        {
            InitializeComponent();
            this.BLObject = BlObject;
            this.viewDroneList = viewDroneList;
            UpdateModel.Visibility = Visibility.Hidden;
            SendDroneToDelivery.Visibility = Visibility.Hidden;
            UpdateDroneToCharging.Visibility = Visibility.Hidden;
            UpdateDroneFromCharging.Visibility = Visibility.Hidden;
            UpdateParcelToPickedUp.Visibility = Visibility.Hidden;
            UpdateParcelToDeliverd.Visibility = Visibility.Hidden;

            DeliveryTB.Visibility = Visibility.Hidden;
            LongitudeTB.Visibility = Visibility.Hidden;
            LatitudeTB.Visibility = Visibility.Hidden;
            BatteryTB.Visibility = Visibility.Hidden;
            MaxWeightTB.Visibility = Visibility.Hidden;
            StatusTB.Visibility = Visibility.Hidden;

            var stationsId = from stationToList in BlObject.ViewStationsWithAvailableChargingSlotstBL() select stationToList.Id;
            BaseStationCB.ItemsSource = stationsId;
            BaseStationCB.SelectedItem = stationsId.First();

            MaxWeightCB.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            MaxWeightCB.SelectedItem = (WeightCategories)0;
        }


        //-------------------  DroneIdTextBox -------------------//

        private void DroneIdTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (IdTextBox.Text == "Id")
                IdTextBox.Clear();

        }
        private void DroneIdTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IdTextBox.Text == String.Empty)
                IdTextBox.Text = "Id";
            else if (ModelTextBox.Text != "Model")
                AddButton.IsEnabled = true;


        }
        private void DroneIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //-------------------  ModelTextBox -------------------//

        private void ModelTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ModelTextBox.Text == "Model")
                ModelTextBox.Clear();
        }

        private void ModelTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ModelTextBox.Text == String.Empty)
                ModelTextBox.Text = "Model";
            else if (IdTextBox.Text != "Id")
                AddButton.IsEnabled = true;

        }
        private void UpdateDroneModel_Click(object sender, RoutedEventArgs e)
        {
            new UpdateDroneModel(BLObject, this.viewDroneList, selcetedDrone.Id).Show();
        }

        private void UpdateDroneToChargingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.UpdateDroneToChargingBL(selcetedDrone.Id);
                MessageBox.Show("Drone has been update to charging sucssesfuly");
                viewDroneList.DroneListView.Items.Refresh();
                this.Close();
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input");
            }
            catch (ObjectAlreadyExistException)
            {
                MessageBox.Show("Drone is already in charging");
            }
            catch (OutOfBatteryException)
            {
                MessageBox.Show("Could not send the drone to charging because there is no enough battery");
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
                BLObject.UpdateDroneFromChargingBL(selcetedDrone.Id);
                MessageBox.Show("Drone has been updated sucssesfuly");
                viewDroneList.DroneListView.Items.Refresh();
                this.Close();
            }
            catch (ObjectNotFoundException)
            {
                MessageBox.Show("Could not update drone from charging");
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input");
            }
            catch (NotValidRequestException)
            {
                MessageBox.Show("Could not update drone status because the drone is not in maintenance status");
            }

        }

        private void SendDroneToDelivery_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.UpdateDroneIdOfParcelBL(selcetedDrone.Id);
                MessageBox.Show("Drone was sent sucssesfuly");
                viewDroneList.DroneListView.Items.Refresh();
                this.Close();
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input");
            }
            catch (InvalidOperationException exeption)
            {
                MessageBox.Show(exeption.Message);
            }
            catch (ObjectNotFoundException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void UpdateParcelStatusToDelivered_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.UpdateDeliveredParcelByDroneIdBL(selcetedDrone.Id);
                MessageBox.Show("Parcel status has been updated to delivered sucssesfuly");
                viewDroneList.DroneListView.Items.Refresh();
                this.Close();
            }
            catch (InvalidInputException exeption)
            {
                MessageBox.Show(exeption.Message);
            }
            catch (NotValidRequestException exeption)
            {
                MessageBox.Show(exeption.Message);
            }
        }
        private void UpdateParcelToPickedUp_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.UpdatePickedUpParcelByDroneIdBL(selcetedDrone.Id);
                MessageBox.Show("Parcel status updated sucssesfuly");
                viewDroneList.DroneListView.Items.Refresh();
                this.Close();
            }
            catch (InvalidInputException exeption)
            {
                MessageBox.Show(exeption.Message);
            }
            catch (NotValidRequestException exeption)
            {
                MessageBox.Show(exeption.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int Id;
            int.TryParse(IdTextBox.Text, out Id);
            String Model = ModelTextBox.Text;
            Drone newDrone = new();
            newDrone.Id = Id;
            newDrone.Model = Model;
            newDrone.MaxWeight = (WeightCategories)MaxWeightCB.SelectedItem;

            try
            {
                BLObject.AddNewDroneBL(newDrone, (int)BaseStationCB.SelectedItem);
                MessageBox.Show("Drone has been added sucssesfuly");
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
        }
    }
}