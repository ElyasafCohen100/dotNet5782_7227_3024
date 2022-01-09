using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for ViewStationList.xaml
    /// </summary>

    public partial class ViewStationList : Window
    {
        private BlApi.IBL BLObject = BlApi.BlFactory.GetBl();
        public ViewStationList()
        {
            InitializeComponent();
            StationListView.ItemsSource = BLObject.ViewBaseStationsToList();
            DataContext = false;
        }

        private void GroupByStationListWithAvailableChargingSlots_Click(object sender, RoutedEventArgs e)
        {

            IEnumerable<IGrouping<int, BO.StationToList>> stationGroup = from station in BLObject.ViewStationsWithAvailableChargingSlotstBL()
                                                                         group station by station.AvailableChargeSlots;

            List<BO.StationToList> stationList = new();
            foreach (var group in stationGroup)
            {
                foreach (var station in group)
                {
                    stationList.Add(station);
                }
            }
            StationListView.ItemsSource = stationList;
        }
        private void StationListView_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (StationListView.SelectedIndex >= 0)
            {
                BO.StationToList selectedStation = BLObject.ViewBaseStationsToList().ToList()[StationListView.SelectedIndex];
                if (new StationActions(selectedStation).ShowDialog() == false)
                    StationListView.ItemsSource = BLObject.ViewBaseStationsToList();
            }
        }

        private void AddNewStationButton_Click(object sender, RoutedEventArgs e)
        {
            if (new StationActions().ShowDialog() == false)
                StationListView.ItemsSource = BLObject.ViewBaseStationsToList();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }
    }
}