using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneList.xaml
    /// </summary>
    public partial class ViewDroneList : Window
    {

        private BlApi.IBL BLObject;
        private CollectionView sourceCollectionView;

        public ViewDroneList()
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

            sourceCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(BLObject.ViewDroneToList());
            DroneListView.ItemsSource = sourceCollectionView;
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
            if (new DroneActions().ShowDialog() == false)
            {
                DroneListView.ItemsSource = BLObject.ViewDroneToList();
                DroneListView.Items.Refresh();
            }
        }
        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DroneListView.SelectedIndex >= 0)
            {
                DroneToList selectedDrone = (DroneToList)sourceCollectionView.GetItemAt(DroneListView.SelectedIndex);
                if (new DroneActions(selectedDrone).ShowDialog() == false)
                    DroneListView.Items.Refresh();
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

        private void ViewDroneListGrouping_Click(object sender, RoutedEventArgs e)
        {
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("DroneStatus");
            sourceCollectionView.GroupDescriptions.Add(groupDescription);
            DroneListView.ItemsSource = sourceCollectionView;
        }

        //private void DroneListView_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Delete)
        //    {
        //        new DroneActions(DroneListView.SelectedIndex).DeleteDroneButton_Click(sender,e);
        //    }
        //}
    }
}
