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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneList.xaml
    /// </summary>
    public partial class ViewDroneList : Window
    {

        private BlApi.IBL BLObject;

        public ViewDroneList(BlApi.IBL BLObject)
        {
            InitializeComponent();
            this.BLObject = BLObject;
            DataContext = false;

            DroneListView.ItemsSource = BLObject.ViewDroneToList();
            DroneStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            DroneWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void DroneStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneListView.ItemsSource = BLObject.ViewDronesToList(x => x.DroneStatus == (DroneStatuses)DroneStatusSelector.SelectedItem);
        }

        private void DroneWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneListView.ItemsSource = BLObject.ViewDronesToList(x => x.MaxWeight == (WeightCategories)DroneWeightSelector.SelectedItem);
        }

        private void AddNewDrone_Click(object sender, RoutedEventArgs e)
        {    
            if(new DroneActions(BLObject).ShowDialog() == false)
            {
                DroneListView.Items.Refresh();
            }
        }
       
        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //---- sound while you're clicking on the button ----//
            System.Media.SoundPlayer player = new(@"sources/clickSound.wav");
            player.Load();
            player.PlaySync();

            if (DroneListView.SelectedIndex >= 0)
            {
                DroneToList selectedDrone = BLObject.ViewDroneToList().ToList()[DroneListView.SelectedIndex];
                if (new DroneActions(BLObject, selectedDrone).ShowDialog() == false)
                {
                    DroneListView.Items.Refresh();
                }
            }
        }
       
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }

        //Bouns.
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext.Equals(false)) e.Cancel = true;
        }

        private void GroupList_Button_Click(object sender, RoutedEventArgs e)
        {
            var droneGroup = from drone in BLObject.ViewDroneToList() group drone by drone.DroneStatus;
            List<DroneToList> droneList = new();
            foreach (var group in droneGroup)
            {
                switch (group.Key)
                {
                    case BO.DroneStatuses.Available:
                        foreach (var drone in group)
                        {
                            droneList.Add(drone);
                        }
                        break;
                    case BO.DroneStatuses.Maintenance:
                        foreach (var drone in group)
                        {
                            droneList.Add(drone);
                        }
                        break;
                    case BO.DroneStatuses.Shipment:
                        foreach (var drone in group)
                        {
                            droneList.Add(drone);
                        }
                        break;
                }
            }
            DroneListView.ItemsSource = droneList;
            DroneListView.Items.Refresh();
        }
    }
}
