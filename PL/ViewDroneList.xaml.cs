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

            DroneListView.ItemsSource = this.BLObject.ViewDroneToList();
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
            new DroneActions(BLObject, this).Show();
        }
       
        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList selectedDrone = BLObject.ViewDroneToList().ToList()[DroneListView.SelectedIndex];
            new DroneActions(BLObject, selectedDrone, this).Show();
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


        //--------------------- groupListButton -------------------------//
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DroneListView.ItemsSource = from drone in BLObject.ViewDroneToList() orderby drone.DroneStatus select drone;
            DroneListView.Items.Refresh();
        }
    }
}
