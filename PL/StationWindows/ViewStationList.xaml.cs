using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BO;

namespace PL
{
    public partial class ViewStationList : Window
    {
        private BlApi.IBL BLObject;

        #region Constructor
        public ViewStationList()
        {
            InitializeComponent();
            try
            {
                BLObject = BlApi.BlFactory.GetBl();
            }
            catch (DalApi.DalConfigException e)
            {
                MessageBox.Show(e.Message, "Opeartion Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            StationListView.ItemsSource = BLObject.GetAllBaseStationsToList();
            DataContext = false;
        }
        #endregion


        #region Station List View
        private void StationListView_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (StationListView.SelectedIndex >= 0)
            {
                StationToList selectedStation = BLObject.GetAllBaseStationsToList().ToList()[StationListView.SelectedIndex];
                if (new StationActions(selectedStation).ShowDialog() == false)
                {
                    StationListView.ItemsSource = BLObject.GetAllBaseStationsToList();
                }
            }        
        }
        #endregion


        #region Order The Station's List By The Number Of Charging Slot (Grouping)
        private void GroupByStationListWithAvailableChargingSlots_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<IGrouping<int,StationToList>> stationGroup = from station in BLObject.GetStationsWithAvailableChargingSlotstBL()
                                                                         group station by station.AvailableChargeSlots;

            List<StationToList> stationList = new();
            foreach (var group in stationGroup)
            {
                foreach (var station in group)
                {
                    stationList.Add(station);
                }
            }
            StationListView.ItemsSource = stationList;
        }
        #endregion


        #region Add A New Station
        private void AddNewStationButton_Click(object sender, RoutedEventArgs e)
        {
            if (new StationActions().ShowDialog() == false)
            {
                StationListView.ItemsSource = BLObject.GetAllBaseStationsToList(); // the list is update
            }
        }
        #endregion


        #region Close Window
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
        #endregion
    }
}