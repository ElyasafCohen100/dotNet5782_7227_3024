using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BO;

namespace PL
{
    public partial class ViewDroneList : Window
    {

        private BlApi.IBL BLObject;
        private CollectionView sourceCollectionView;

        #region Constructor
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

            sourceCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(BLObject.GetAllDroneToList());
            DroneListView.ItemsSource = sourceCollectionView;
            DroneStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            DroneWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
        #endregion


        #region Add a new drone
        private void AddNewDrone_Click(object sender, RoutedEventArgs e)
        {
            //---- sound while you're clicking on the button ----//
            System.Media.SoundPlayer player = new(@"sources/clickSound.wav");
            player.Load();
            player.PlaySync();

            if (new DroneActions().ShowDialog() == false)
            {
                DroneListView.ItemsSource = BLObject.GetAllDroneToList();
                DroneListView.Items.Refresh();
            }
        }
        #endregion


        #region Drone List View
        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DroneListView.SelectedIndex >= 0)
            {
                DroneToList selectedDrone = (DroneToList)sourceCollectionView.GetItemAt(DroneListView.SelectedIndex);
                if (new DroneActions(selectedDrone).ShowDialog() == false)
                {
                    DroneListView.Items.Refresh();
                }
            }
        }

        private void DroneStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneListView.ItemsSource = BLObject.GetDronesToList(x => x.DroneStatus == (DroneStatuses)DroneStatusSelector.SelectedItem);
        }

        private void DroneWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneListView.ItemsSource = BLObject.GetDronesToList(x => x.MaxWeight == (WeightCategories)DroneWeightSelector.SelectedItem);
        }
          
        private void RegularViewButton_Checked(object sender, RoutedEventArgs e)
        {
            DroneListView.ItemsSource = BLObject.GetAllDroneToList();
        }

        private void GrupViewButton_Checked(object sender, RoutedEventArgs e)
        {
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("DroneStatus");
            sourceCollectionView.GroupDescriptions.Add(groupDescription);
            DroneListView.ItemsSource = sourceCollectionView;
        }
        #endregion


        #region Close Window
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

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        #endregion


        #region Overloading the "Delete" Key 
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
               if(DroneListView.SelectedItem != null)
                {
                    try
                    {
                        DroneToList drone = (DroneToList)sourceCollectionView.GetItemAt(DroneListView.SelectedIndex);
                        BLObject.DeleteDrone(drone.Id);
                        DroneListView.ItemsSource = (CollectionView)CollectionViewSource.GetDefaultView(BLObject.GetAllDroneToList());
                        DroneListView.Items.Refresh();
                    }
                    catch(InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    catch(ObjectNotFoundException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        #endregion
    }
}
