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
using IBL.BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for DroneActions.xaml
    /// </summary>
    public partial class DroneActions : Window
    {
        private IBL.IBL BlObject;
        private ViewDroneList viewDroneList;
        private DroneToList selcetedDrone;


        // Drone actions c-tor
        public DroneActions(IBL.IBL BlObject, DroneToList droneToList, ViewDroneList viewDroneList)
        {
            InitializeComponent();

            Drone drone = BlObject.FindDroneByIdBL(droneToList.Id);
            selcetedDrone = droneToList;
            this.BlObject = BlObject;
            this.viewDroneList = viewDroneList;

            IdTB.IsEnabled = false;
            IdTB.Text = drone.Id.ToString();

            ModelTB.IsEnabled = false;
            ModelTB.Text = drone.Model;

            BatteryTB.IsEnabled = false;
            BatteryTB.Text = drone.BatteryStatus.ToString();

            MaxWeightTB.IsEnabled = false;
            MaxWeightTB.Text = drone.MaxWeight.ToString();

            StatusTB.IsEnabled = false;
            StatusTB.Text = drone.DroneStatus.ToString();

            DeliveryTB.IsEnabled = false;
            DeliveryTB.Text = drone.ParcelInDelivery.ToString();

            LatitudeTB.IsEnabled = false;
            LatitudeTB.Text = drone.CurrentLocation.Latitude.ToString();
            
            LongitudeTB.IsEnabled = false;
            LongitudeTB.Text = drone.CurrentLocation.Longitude.ToString();

            AddButton.Visibility = Visibility.Hidden;
            ClearButton.Visibility = Visibility.Hidden;
            BaseStationCB.Visibility = Visibility.Hidden;
            MaxWeightCB.Visibility = Visibility.Hidden;

        }

        // Add new Drone c-tor
        public DroneActions(IBL.IBL BlObject, ViewDroneList viewDroneList)
        {
            InitializeComponent();
            this.BlObject = BlObject;
            this.viewDroneList = viewDroneList;


            DeliveryTB.Visibility = Visibility.Hidden;
            LongitudeTB.Visibility = Visibility.Hidden;
            LatitudeTB.Visibility = Visibility.Hidden;
            BatteryTB.Visibility = Visibility.Hidden;
            MaxWeightTB.Visibility = Visibility.Hidden;
            StatusTB.Visibility = Visibility.Hidden;

            var stationsId = from stationToList in BlObject.ViewStationsWithAvailableChargingSlotstBL() select stationToList.Id;
            BaseStationCB.ItemsSource = stationsId;
            BaseStationCB.SelectedItem = stationsId.First();

            MaxWeightCB.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            MaxWeightCB.SelectedItem = (WeightCategories)0;

        }


        private void UpdateDroneModel_Click(object sender, RoutedEventArgs e)
        {
            new UpdateDroneModel(BlObject, this.viewDroneList, selcetedDrone.Id).Show();
        }

        private void UpdateDroneCharging_Click(object sender, RoutedEventArgs e)
        {
            new UpdateDroneCharging(BlObject, this.viewDroneList, selcetedDrone.Id).Show();
        }

        private void UpdateParcelStatus_Click(object sender, RoutedEventArgs e)
        {
            new UpdateParcelStatus(BlObject, this.viewDroneList, selcetedDrone.Id).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int Id;
            int.TryParse(IdTB.Text, out Id);
            String Model = ModelTB.Text;
            Drone newDrone = new();
            newDrone.Id = Id;
            newDrone.Model = Model;
            newDrone.MaxWeight = (WeightCategories)MaxWeightCB.SelectedItem;

            try
            {
                BlObject.AddNewDroneBL(newDrone, (int)BaseStationCB.SelectedItem);
                MessageBox.Show("Drone added sucssesfuly");
                viewDroneList.DroneListView.Items.Refresh();
                this.Close();
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input");
            }
            catch (ObjectAlreadyExistException)
            {
                MessageBox.Show("Drone is already exist");
            }
        }
    }
}
