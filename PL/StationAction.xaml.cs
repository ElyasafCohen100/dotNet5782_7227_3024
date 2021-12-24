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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationActions.xaml
    /// </summary>
    public partial class StationActions : Window
    {
        private BlApi.IBL BLObject;
        private ViewStationList viewStationList;
        private BO.StationToList selcetedStationToList;

        public StationActions(BlApi.IBL Blobject, ViewStationList viewStationList, BO.StationToList selectedStationToList)
        {
            this.BLObject = Blobject;
            this.viewStationList = viewStationList;
            this.selcetedStationToList = selectedStationToList;

            InitializeComponent();

            BO.Station station = BLObject.FindStationByIdBL(selcetedStationToList.Id);
            StationIdTB.Text = station.Id.ToString();
            StationNameTB.Text = station.Name.ToString();
            StationLatitudeTB.Text = station.Location.Latitude.ToString();
            StationLongitudeTB.Text = station.Location.Longitude.ToString();
            AvailableChargeSlotsTB.Text = station.AvailableChargeSlots.ToString();
            var strings = from myStation in station.DroneChargesList select myStation.ToString();
            string descreption = "";
            foreach (var v in strings)
            {
                descreption += v;
            }
            DroneChargesListTB.Text = descreption;

            StationIdTB.IsEnabled = false;
            StationLatitudeTB.IsEnabled = false;
            StationLongitudeTB.IsEnabled = false;
            DroneChargesListTB.IsEnabled = false;

            StationId.IsEnabled = false;
            StationName.IsEnabled = false;
            StationLatitude.IsEnabled = false;
            StationLongitude.IsEnabled = false;
            AvailableChargeSlots.IsEnabled = false;
            DroneChargesList.IsEnabled = false;

            AddStation.Visibility = Visibility.Hidden;


        }

        public StationActions(BlApi.IBL Blobject, ViewStationList viewStationList)
        {
            this.BLObject = Blobject;
            this.viewStationList = viewStationList;

            InitializeComponent();

            DroneChargesListTB.Visibility = Visibility.Hidden;
            DroneChargesList.Visibility = Visibility.Hidden;
            UpdateStationButton.Visibility = Visibility.Hidden;

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
                viewStationList.StationListView.ItemsSource = BLObject.ViewBaseStationsToList();
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
                viewStationList.StationListView.ItemsSource = BLObject.ViewBaseStationsToList();
                this.Close();

            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input", "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
