using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using BO;
using System.ComponentModel;

namespace PL
{
    public partial class DroneActions : Window
    {
        private BlApi.IBL BLObject;
        private DroneToList selectedDroneToList;
        BackgroundWorker backgroundWorker = new BackgroundWorker();

        #region Drone Action Constructor
        public DroneActions(DroneToList droneToList)
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
            try
            {
                Drone drone = BLObject.GetDroneByIdBL(droneToList.Id);
                this.selectedDroneToList = droneToList;

                DataContext = false;
                grid1.DataContext = drone;

                IdTextBox.IsEnabled = false;
                ModelTextBox.IsEnabled = false;
                BatteryTB.IsEnabled = false;
                MaxWeightTB.IsEnabled = false;
                StatusTB.IsEnabled = false;
                DeliveryTB.IsEnabled = false;
                LocationTB.IsEnabled = false;
                LocationTB.Text = drone.CurrentLocation.ToString();
                DeliveryTB.Text = drone.ParcelInDelivery.ToString();

                Pb.Value = droneToList.BatteryStatus;
                progressBarColor();

                grid3.Visibility = Visibility.Hidden;
                AddButton.Visibility = Visibility.Hidden;

                if (selectedDroneToList.DeliveryParcelId <= 0)
                    ViewParcelButton.Visibility = Visibility.Hidden;

                if (selectedDroneToList.DeliveryParcelId == 0)
                {
                    DeliveryTB.Visibility = Visibility.Hidden;
                    ParcelInDelivery.Visibility = Visibility.Hidden;
                }
            }
            catch (ObjectNotFoundException exception)
            {
                MessageBox.Show(exception.Message,
                                "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion


        #region Add Constructor
        public DroneActions()
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
            DeleteDroneButton.Visibility = Visibility.Hidden;
            SimulatorButton.Visibility = Visibility.Hidden;
            ManualButton.Visibility = Visibility.Hidden;
            grid2.Visibility = Visibility.Hidden;
            grid4.Visibility = Visibility.Hidden;


            AddButton.IsEnabled = false;

            var stationsIdList = from stationToList in BLObject.GetStationsWithAvailableChargingSlotstBL() select stationToList.Id;
            BaseStationCB.ItemsSource = stationsIdList;
            BaseStationCB.SelectedItem = stationsIdList.First();

            MaxWeightCB.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            MaxWeightCB.SelectedItem = (WeightCategories)0;
        }
        #endregion


        #region Drone's TextBox Function
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
        #endregion


        #region Drone Actions
        /// <summary>
        /// getting all the details of the drone that has been chosen, from drone to list at the BL layer.
        /// </summary>
        /// <param name="droneId">property of drone - ID</param>
        private void GetDroneFields(int droneId)
        {
            Drone drone = BLObject.GetDroneByIdBL(droneId);

            IdTextBox.Text = drone.Id.ToString();
            ModelTextBox.Text = drone.Model;
            BatteryTB.Text = String.Format("{0:0.000}%", drone.BatteryStatus);
            Pb.Value = drone.BatteryStatus;
            progressBarColor();
            MaxWeightTB.Text = drone.MaxWeight.ToString();
            StatusTB.Text = drone.DroneStatus.ToString();
            DeliveryTB.Text = drone.ParcelInDelivery.ToString();
            LocationTB.Text = drone.CurrentLocation.ToString();
        }

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
                BLObject.AssociateDroneTofParcelBL(selectedDroneToList.Id);
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
            catch (ObjectNotFoundException)
            {
                MessageBox.Show("NO parcel was faund in this drone",
                    "Operation failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
  
        //Bouns.
        private void ViewParcel_Click(object sender, RoutedEventArgs e)
        {
            ParcelToList parcelToList = BLObject.GetParcelToList(selectedDroneToList.DeliveryParcelId);
            new ParcelActions(parcelToList).Show();
        }
        
        private void AddNewDroneButton_Click(object sender, RoutedEventArgs e)
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
            this.Close_Button_Click(sender, e);
        }
       
        private void DeleteDroneButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.DeleteDrone(selectedDroneToList.Id);
                MessageBox.Show("Drone has been removed",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close_Button_Click(sender, e);
            }
            catch (ObjectNotFoundException exception)
            {
                MessageBox.Show(exception.Message,
                                "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Drone could not be deleted because the drone is in shipment",
                "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion


        #region Simulator
        private void SimulatorButton_Click(object sender, RoutedEventArgs e)
        {
            SimulatorButton.Visibility = Visibility.Hidden;
            ManualButton.Visibility = Visibility.Visible;
            grid4.Visibility = Visibility.Hidden;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;


            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Drone drone = BLObject.GetDroneByIdBL(selectedDroneToList.Id);
            StatusTB.Text = drone.DroneStatus.ToString();
            LocationTB.Text = drone.CurrentLocation.ToString();

            DeliveryTB.Text = drone.ParcelInDelivery.ToString();
            if (selectedDroneToList.DeliveryParcelId != 0)
            {
                DeliveryTB.Visibility = Visibility.Visible;
                ParcelInDelivery.Visibility = Visibility.Visible;
            }

            if (selectedDroneToList.DroneStatus == DroneStatuses.Maintenance)
            {
                DeliveryTB.Visibility = Visibility.Hidden;
                ParcelInDelivery.Visibility = Visibility.Hidden;

                double batteryStatus;
                DroneCharge droneCharge = BLObject.FindDroneChargeByDroneIdBL(drone.Id);

                batteryStatus = BLObject.BatteryCalc(selectedDroneToList, droneCharge);
                BatteryTB.Text = String.Format("{0:0.000}%", batteryStatus);
                Pb.Value = batteryStatus;
                progressBarColor();
            }
            else
            {
                BatteryTB.Text = String.Format("{0:0.000}%", drone.BatteryStatus);
                Pb.Value = drone.BatteryStatus;
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BLObject.StartSimulator(selectedDroneToList.Id,
                                        UpdateAction,
                                        () => { return backgroundWorker.CancellationPending; });
            }
            catch (ObjectNotFoundException)
            {
                MessageBox.Show("There is no parcels to delivered",
                    "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NoParcelsMatchToDroneException)
            {
                MessageBox.Show("There is no parcels this drone can delivered",
                                 "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void UpdateAction()
        {
            backgroundWorker.ReportProgress(0);
        }
       
        private void ManualButton_Click(object sender, RoutedEventArgs e)
        {
            backgroundWorker.CancelAsync();
            ManualButton.Visibility = Visibility.Hidden;
            SimulatorButton.Visibility = Visibility.Visible;
            grid4.Visibility = Visibility.Visible;
        }
      
        private void progressBarColor()
        {
            if (Pb.Value <= 33) Pb.Foreground = Brushes.Red;
            else if (Pb.Value <= 66) Pb.Foreground = Brushes.Yellow;
            else Pb.Foreground = Brushes.LimeGreen;
        }
        #endregion


        #region Overloading the "Enter" key
        private void AddButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.AddNewDroneButton_Click(sender, e);
            }
        }
        #endregion


        #region Close Window
        private void Close_Button_Click(object sender, RoutedEventArgs e)
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