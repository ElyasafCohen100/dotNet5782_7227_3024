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
        private BlApi.IBL BLObject = BlApi.BlFactory.GetBl();
        private StationToList selcetedStationToList;

        public StationActions(StationToList selectedStationToList)
        {
            this.selcetedStationToList = selectedStationToList;

            InitializeComponent();

            BO.Station station = BLObject.FindStationByIdBL(selcetedStationToList.Id);
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
        public StationActions()
        {

            InitializeComponent();

            DroneChargesList.Visibility = Visibility.Hidden;
            UpdateStationButton.Visibility = Visibility.Hidden;
            DroneChargeListView.Visibility = Visibility.Hidden;

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
        }

        private void StationdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
                this.Close();

            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input", "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DroneChargeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList selectedDrone = BLObject.ViewDroneToList().ToList()[DroneChargeListView.SelectedIndex];
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
        }
    }
}
