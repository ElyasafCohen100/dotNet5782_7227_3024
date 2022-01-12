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
            DataContext = false;


            BO.Station station = BLObject.GetStationByIdBL(selcetedStationToList.Id);
            grid1.DataContext = station;

            StationIdTB.IsEnabled = false;
            StationLatitudeTB.IsEnabled = false;
            StationLongitudeTB.IsEnabled = false;
            StationId.IsEnabled = false;
            StationName.IsEnabled = false;
            StationLatitude.IsEnabled = false;
            StationLongitude.IsEnabled = false;
            AvailableChargeSlots.IsEnabled = false;
            DroneChargesList.IsEnabled = false;

            AddStation.Visibility = Visibility.Hidden;

            DroneChargeListView.ItemsSource = from droneCharge in station.DroneChargesList select droneCharge;

        }
        //addNewStation
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
            DataContext = false;
            DroneChargesList.Visibility = Visibility.Hidden;
            UpdateStationButton.Visibility = Visibility.Hidden;
            DroneChargeListView.Visibility = Visibility.Hidden;
            DeleteStationButton.Visibility = Visibility.Hidden;
        }

        private void UpdateStationButton_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(AvailableChargeSlotsTB.Text, out int chargeSlots);
            string newName = StationNameTB.Text;
            try
            {
                BLObject.UpdateBaseStationDetailsBL(selcetedStationToList.Id, newName, chargeSlots);
                MessageBox.Show("Station's details has been update sucssesfuly",
                    "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidInputException exception)
            {
                MessageBox.Show(exception.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.CloseButton_Click(sender, e);
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

        private void AddStation_Click(object sender, RoutedEventArgs e)
        {
            BO.Station station = new();
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

        private void DroneChargeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList selectedDrone = BLObject.GetAllDroneToList().ToList()[DroneChargeListView.SelectedIndex];
            if (new DroneActions(selectedDrone).ShowDialog() == false)
            {
                DroneChargeListView.Items.Refresh();
            }
        }

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

        private void DeleteStationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.DeleteStation(selcetedStationToList.Id);
                MessageBox.Show("Station has been removed",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (ObjectNotFoundException exception)
            {
                MessageBox.Show(exception.Message,
                                "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.CloseButton_Click(sender, e);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext.Equals(false)) e.Cancel = true;
        }

        private void StationNameTB_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateStationButton.IsEnabled = true;
        }

        private void AvailableChargeSlotsTB_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateStationButton.IsEnabled = true;
        }
    }
}
