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

namespace PL
{
    /// <summary>
    /// Interaction logic for ViewStationList.xaml
    /// </summary>

    public partial class ViewStationList : Window
    {
        private BlApi.IBL BLObject;
        public ViewStationList(BlApi.IBL BLObject)
        {
            InitializeComponent();
            this.BLObject = BLObject;
            StationListView.ItemsSource = this.BLObject.ViewBaseStationsToList();
        }

        private void ViewStationList_Click(object sender, RoutedEventArgs e)
        {
            var stationList = from station in BLObject.ViewStationsWithAvailableChargingSlotstBL() select station;
            StationListView.ItemsSource = stationList;
            StationListView.Items.Refresh();
        }
        
        private void StationListView_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            BO.StationToList selectedStation = BLObject.ViewBaseStationsToList().ToList()[StationListView.SelectedIndex];
          
            if(new StationActions(BLObject, selectedStation).ShowDialog() == false)
            {
                StationListView.Items.Refresh();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(new StationActions(BLObject).ShowDialog() == false)
            {
                StationListView.Items.Refresh();
            }
        }
    }
}