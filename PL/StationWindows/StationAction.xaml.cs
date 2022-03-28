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
    /// Interaction logic for StationActions.xaml
    /// </summary>
    public partial class StationActions : Window
    {
        private BlApi.IBL BLObject;
        private StationToList selcetedStationToList;
        private bool closeFlag = false;

        #region Action Constructor
        public StationActions(StationToList selectedStationToList)
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

            this.selcetedStationToList = selectedStationToList;
            


            Station station = BLObject.GetStationByIdBL(selcetedStationToList.Id);
            grid1.DataContext = station;

            StationId.IsEnabled = false;
            StationIdTB.IsEnabled = false;
           
            StationLatitude.IsEnabled = false;
            StationLatitudeTB.IsEnabled = false;
            
            StationLongitude.IsEnabled = false;
            StationLongitudeTB.IsEnabled = false;
            
            StationName.IsEnabled = false;
            AvailableChargeSlots.IsEnabled = false;
            DroneChargesList.IsEnabled = false;
            AddStation.Visibility = Visibility.Hidden;

            DroneChargeListView.ItemsSource = from droneCharge in station.DroneChargesList select droneCharge;
        }
        #endregion


        #region Add Constructor
        public StationActions()
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
            closeFlag = false;

            DroneChargesList.Visibility = Visibility.Hidden;
            UpdateStationButton.Visibility = Visibility.Hidden;
            DroneChargeListView.Visibility = Visibility.Hidden;
            DeleteStationButton.Visibility = Visibility.Hidden;
        }
        #endregion


        #region Update Station Details
        private void UpdateStationButton_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(AvailableChargeSlotsTB.Text, out int chargeSlots);
            string newName = StationNameTB.Text;
            try
            {
                BLObject.UpdateBaseStationDetailsBL(selcetedStationToList.Id, newName, chargeSlots);
                MessageBox.Show("Station's details has been update sucssesfuly",
                                 "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

            this.CloseButton_Click(sender, e);
            }
            catch (InvalidInputException exception)
            {
                MessageBox.Show(exception.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion


        #region Add A New Station
        private void AddStation_Click(object sender, RoutedEventArgs e)
        {
            Station station = new();
            int.TryParse(StationIdTB.Text, out int Id);
            station.Id = Id;

            station.Name = StationNameTB.Text;

            double.TryParse(StationLatitudeTB.Text, out double Latitude);
            station.Location.Latitude = Latitude;

            double.TryParse(StationLatitudeTB.Text, out double Longitude);
            station.Location.Longitude = Longitude;

            int.TryParse(AvailableChargeSlotsTB.Text, out int AvailableChargeSlot);
            station.AvailableChargeSlots = AvailableChargeSlot;

            try
            {
                BLObject.AddNewStationBL(station);
                MessageBox.Show("Station has been added sucssesfuly",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

                this.CloseButton_Click(sender, e);

            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input", "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion


        #region Delete Station
        private void DeleteStationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.DeleteStation(selcetedStationToList.Id);
                MessageBox.Show("Station has been removed",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            this.CloseButton_Click(sender, e);
            }
            catch (ObjectNotFoundException exception)
            {
                MessageBox.Show(exception.Message,
                                "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion


        #region DroneCharge List View
        private void DroneChargeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList selectedDrone = BLObject.GetAllDroneToList().ToList()[DroneChargeListView.SelectedIndex];
            if (new DroneActions(selectedDrone).ShowDialog() == false)
            {
                DroneChargeListView.Items.Refresh();
            }
        }
        #endregion


        #region Statin TextBox
        private void StationIdTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(StationIdTB.Text, out int Id);
            if (Id > 10000 || Id < 1000)
            {
                StationIdTB.BorderBrush = Brushes.Red;
                StationIdTB.Foreground = Brushes.Red;
            }
            else
            {
                StationIdTB.BorderBrush = Brushes.Gray;
                StationIdTB.Foreground = Brushes.Black;
            }
        }
  
        private void StationIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
      
        private void StationNameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-z,A-Z,0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }      

        private void StationNameTB_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateStationButton.IsEnabled = true;
        }

        private void AvailableChargeSlotsTB_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateStationButton.IsEnabled = true;
        }
        #endregion


        #region Close Window
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            closeFlag = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!closeFlag) e.Cancel = true;
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
