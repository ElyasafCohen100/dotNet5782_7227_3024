using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneActions.xaml
    /// </summary>
    public partial class DroneActions : Window
    {
        private BlApi.IBL BLObject = BlApi.BlFactory.GetBl();
        private DroneToList selectedDroneToList;


        //Drone actions c-tor.
        public DroneActions(DroneToList droneToList)
        {
            InitializeComponent();

            Drone drone = BLObject.FindDroneByIdBL(droneToList.Id);
            this.selectedDroneToList = droneToList;

            DataContext = false;
            grid1.DataContext = drone;
            IdTextBox.IsEnabled = false;
            ModelTextBox.IsEnabled = false;
            BatteryTB.IsEnabled = false;
            MaxWeightTB.IsEnabled = false;
            StatusTB.IsEnabled = false;
            DeliveryTB.IsEnabled = false;
            DeliveryTB.Text = drone.ParcelInDelivery.ToString();
            LatitudeTB.IsEnabled = false;
            LongitudeTB.IsEnabled = false;

            WeightTextBlock.Visibility = Visibility.Hidden;
            StationTextBlock.Visibility = Visibility.Hidden;

            BaseStationCB.Visibility = Visibility.Hidden;
            MaxWeightCB.Visibility = Visibility.Hidden;

            AddButton.Visibility = Visibility.Hidden;
            if (selectedDroneToList.DeliveryParcelId <= 0)
                ViewParcelButton.Visibility = Visibility.Hidden;
        }

        //Add new Drone c-tor.
        public DroneActions()
        {
            InitializeComponent();

            DataContext = false;
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

            BatteryTextBlock.Visibility = Visibility.Hidden;
            LatitudeTextBlock.Visibility = Visibility.Hidden;
            LongitudeTextBlock.Visibility = Visibility.Hidden;
            MaxweightTextBlock.Visibility = Visibility.Hidden;
            StatusTextBlock.Visibility = Visibility.Hidden;
            ViewParcelButton.Visibility = Visibility.Hidden;

            AddButton.IsEnabled = false;

            var stationsIdList = from stationToList in BLObject.ViewStationsWithAvailableChargingSlotstBL() select stationToList.Id;
            BaseStationCB.ItemsSource = stationsIdList;
            BaseStationCB.SelectedItem = stationsIdList.First();

            MaxWeightCB.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            MaxWeightCB.SelectedItem = (WeightCategories)0;
        }


        //-------------------  DroneIdTextBox -------------------//

        private void DroneIdTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (IdTextBox.Text == "Id")
            {
                IdTextBox.Clear();
            }
        }
        private void DroneIdTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IdTextBox.Text == String.Empty)
                IdTextBox.Text = "Id";

            else if (ModelTextBox.Text != "Model")
                AddButton.IsEnabled = true;

            //Bouns.
            if (IdTextBox.Text != "Id")
            {
                int.TryParse(IdTextBox.Text, out int Id);
                if (Id > 10000 || Id < 1000)
                {
                    IdTextBox.BorderBrush = Brushes.Red;
                    IdTextBox.Foreground = Brushes.Red;
                }
            }

        }
        private void DroneIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void ModelIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-z,A-Z,0-9]+");
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

        //Drone actions.
        #region Drone Actions
        private void UpdateDroneModel_Click(object sender, RoutedEventArgs e)
        {
            bool? flag = new UpdateDroneModel(selectedDroneToList.Id).ShowDialog();
            if (flag == false)
            {
                GetDroneFields(selectedDroneToList.Id);
            }
        }

        private void UpdateDroneToChargingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.UpdateDroneToChargingBL(selectedDroneToList.Id);
                MessageBox.Show("Drone has been update to charging sucssesfuly",
                    "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                GetDroneFields(selectedDroneToList.Id);
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input",
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ObjectAlreadyExistException)
            {
                MessageBox.Show("Drone is already in charging",
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (OutOfBatteryException)
            {
                MessageBox.Show("Could not send the drone to charging because there is no enough battery",
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ObjectNotFoundException)
            {
                MessageBox.Show("Could not update drone to charging",
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
      
        private void UpdateDroneFromChargingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.UpdateDroneFromChargingBL(selectedDroneToList.Id);
                MessageBox.Show("Drone has been updated sucssesfuly",
                    "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                GetDroneFields(selectedDroneToList.Id);
            }
            catch (ObjectNotFoundException)
            {
                MessageBox.Show("Could not update drone from charging",
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input",
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NotValidRequestException)
            {
                MessageBox.Show("Could not update drone status because the drone is not in maintenance status",
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    
        private void SendDroneToDelivery_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.UpdateDroneIdOfParcelBL(selectedDroneToList.Id);
                MessageBox.Show("Drone was sent sucssesfuly",
                    "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                GetDroneFields(selectedDroneToList.Id);
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input",
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException exeption)
            {
                MessageBox.Show(exeption.Message,
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ObjectNotFoundException exception)
            {
                MessageBox.Show(exception.Message,
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateParcelStatusToDelivered_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.UpdateDeliveredParcelByDroneIdBL(selectedDroneToList.Id);
                MessageBox.Show("Parcel status has been updated to delivered sucssesfuly",
                    "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                GetDroneFields(selectedDroneToList.Id);
            }
            catch (InvalidInputException exeption)
            {
                MessageBox.Show(exeption.Message,
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NotValidRequestException exeption)
            {
                MessageBox.Show(exeption.Message,
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    
        private void UpdateParcelToPickedUp_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.UpdatePickedUpParcelByDroneIdBL(selectedDroneToList.Id);
                MessageBox.Show("Parcel status updated sucssesfuly",
                    "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                GetDroneFields(selectedDroneToList.Id);
            }
            catch (InvalidInputException exeption)
            {
                MessageBox.Show(exeption.Message,
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NotValidRequestException exeption)
            {
                MessageBox.Show(exeption.Message,
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }
        #region Add Drone
     
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
                MessageBox.Show("Drone has been added sucssesfuly",
                    "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                GetDroneFields(Id);

                this.Close();
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input",
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ObjectAlreadyExistException)
            {
                MessageBox.Show("Drone is already exist",
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        //Bouns.
        private void IdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IdTextBox.Text != "Id")
            {
                int.TryParse(IdTextBox.Text, out int Id);
                if (Id > 10000 || Id < 1000)
                {
                    IdTextBox.BorderBrush = Brushes.Red;
                    IdTextBox.Foreground = Brushes.Red;
                }
                else
                {
                    IdTextBox.BorderBrush = Brushes.Gray;
                    IdTextBox.Foreground = Brushes.Black;
                }
            }
        }
       
        private void GetDroneFields(int droneId)
        {

            Drone drone = BLObject.FindDroneByIdBL(droneId);

            IdTextBox.Text = drone.Id.ToString();
            ModelTextBox.Text = drone.Model;
            BatteryTB.Text = drone.BatteryStatus.ToString();
            MaxWeightTB.Text = drone.MaxWeight.ToString();
            StatusTB.Text = drone.DroneStatus.ToString();
            DeliveryTB.Text = drone.ParcelInDelivery.ToString();
            LatitudeTB.Text = drone.CurrentLocation.Latitude.ToString();
            LongitudeTB.Text = drone.CurrentLocation.Longitude.ToString();
        }

        //Bouns.
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext.Equals(false)) e.Cancel = true;
        }
     
        private void ViewParcel_Click(object sender, RoutedEventArgs e)
        {
            ParcelToList parcelToList = BLObject.FindParcelToList(selectedDroneToList.DeliveryParcelId);
            new ParcelActions(parcelToList).Show();
        }
       
        private void DeleteDroneButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.DeleteDrone(selectedDroneToList.Id);
                MessageBox.Show("Drone has been removed",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Button_Click(sender, e);
            }
            catch (ObjectNotFoundException exception)
            {
                MessageBox.Show(exception.Message,
                                "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}