using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
            StationListView.ItemsSource = BLObject.GetAllBaseStationsToList();
            DataContext = false;
        }

        private void GroupByStationListWithAvailableChargingSlots_Click(object sender, RoutedEventArgs e)
        {

            IEnumerable<IGrouping<int, BO.StationToList>> stationGroup = from station in BLObject.GetStationsWithAvailableChargingSlotstBL()
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
                BO.StationToList selectedStation = BLObject.GetAllBaseStationsToList().ToList()[StationListView.SelectedIndex];
                if (new StationActions(selectedStation).ShowDialog() == false)
                    StationListView.ItemsSource = BLObject.GetAllBaseStationsToList();
            }
        }

        private void AddNewStationButton_Click(object sender, RoutedEventArgs e)
        {
            if (new StationActions().ShowDialog() == false)
                StationListView.ItemsSource = BLObject.GetAllBaseStationsToList();
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

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}