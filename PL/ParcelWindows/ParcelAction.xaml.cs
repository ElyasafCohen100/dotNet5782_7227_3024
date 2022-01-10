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
    /// Interaction logic for ParcelActions.xaml
    /// </summary>
    public partial class ParcelActions : Window
    {
        private BlApi.IBL BLObject;
        private ParcelToList selecetedParcelToList;
        public ParcelActions(ParcelToList selecetedParcelToList)
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
            this.selecetedParcelToList = selecetedParcelToList;

            Parcel parcel = BLObject.FindParcelByIdBL(selecetedParcelToList.Id);
            grid1.DataContext = parcel;

            if (parcel.Scheduled == null || parcel.Delivered != null)
            {
                ViewDroneInParcel.Visibility = Visibility.Hidden;
                ViewReceiverCustomerInParcel.Visibility = Visibility.Hidden;
                ViewSenderCustomerInParcel.Visibility = Visibility.Hidden;
            }

            if (parcel.Scheduled == null) DeleteParcelButton.Visibility = Visibility.Visible;

            ParcelIdTB.Text = parcel.Id.ToString();
            ReceiverCustomerIdTB.Text = parcel.receiverCustomer.Id.ToString();
            ReceiverCustomerNameTB.Text = parcel.receiverCustomer.Name.ToString();

            SenderCustomerIdTB.Text = parcel.senderCustomer.Id.ToString();
            SenderCustomerNameTB.Text = parcel.senderCustomer.Id.ToString();

            DroneInParcelIdTB.Text = parcel.Drone.Id.ToString();
            DroneInParcelBatteryTB.Text = parcel.Drone.BatteryStatus.ToString();
          
            DroneInParcelLatitudeTB.Text = parcel.Drone.CurrentLocation.Latitude.ToString();
            DroneInParcelLongitudeTB.Text = parcel.Drone.CurrentLocation.Longitude.ToString();

            PriorityTB.Text = parcel.Priority.ToString();
            PrioritySelctor.Visibility = Visibility.Hidden;
         
            WeightSelctor.Visibility = Visibility.Hidden;
            AddParcelButton.Visibility = Visibility.Hidden;
           
            ReceiverCustomerIdSelector.Visibility = Visibility.Hidden;
            SenderCustomerIdSelector.Visibility = Visibility.Hidden;

        }

        public ParcelActions()
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
            SenderCustomerId.Visibility = Visibility.Hidden;
            SenderCustomerIdTB.Visibility = Visibility.Hidden;
          
            ReceiverCustomerId.Visibility = Visibility.Hidden;
            ReceiverCustomerIdTB.Visibility = Visibility.Hidden;
           
            ParcelId.Visibility = Visibility.Hidden;
            ParcelIdTB.Visibility = Visibility.Hidden;
           
            SenderCustomerName.Visibility = Visibility.Hidden;
            SenderCustomerNameTB.Visibility = Visibility.Hidden;
           
            ReceiverCustomerName.Visibility = Visibility.Hidden;
            ReceiverCustomerNameTB.Visibility = Visibility.Hidden;
         
            DroneInParcelId.Visibility = Visibility.Hidden;
            DroneInParcelIdTB.Visibility = Visibility.Hidden;
           
            DroneInParcelBattery.Visibility = Visibility.Hidden;
            DroneInParcelBatteryTB.Visibility = Visibility.Hidden;
           
            DroneInParcelLatitude.Visibility = Visibility.Hidden;
            DroneInParcelLatitudeTB.Visibility = Visibility.Hidden;
          
            DroneInParcelLongitude.Visibility = Visibility.Hidden;
            DroneInParcelLongitudeTB.Visibility = Visibility.Hidden;
          
            Priority.Visibility = Visibility.Hidden;
            PriorityTB.Visibility = Visibility.Hidden;
          
            Weight.Visibility = Visibility.Hidden;
            WeightTB.Visibility = Visibility.Hidden;
          
            RequestedTime.Visibility = Visibility.Hidden;
            RequestedTimeTB.Visibility = Visibility.Hidden;
          
            ScheduledTime.Visibility = Visibility.Hidden;
            ScheduledTimeTB.Visibility = Visibility.Hidden;
          
            PickedUpTime.Visibility = Visibility.Hidden;
            PickedUpTimeTB.Visibility = Visibility.Hidden;
          
            DeliveredTime.Visibility = Visibility.Hidden;
            DeliveredTimeTB.Visibility = Visibility.Hidden;
           
            ViewDroneInParcel.Visibility = Visibility.Hidden;
            ViewReceiverCustomerInParcel.Visibility = Visibility.Hidden;
            ViewSenderCustomerInParcel.Visibility = Visibility.Hidden;


            var customersList = from customer in BLObject.ViewCustomerToList() select customer.Id;

            if (customersList.Count() > 0)
            {
                ReceiverCustomerIdSelector.ItemsSource = customersList;
                SenderCustomerIdSelector.ItemsSource = customersList;

                ReceiverCustomerIdSelector.SelectedItem = customersList.First();
                SenderCustomerIdSelector.SelectedItem = customersList.First();
            }

            PrioritySelctor.ItemsSource = Enum.GetValues(typeof(Priorities));
            WeightSelctor.ItemsSource = Enum.GetValues(typeof(WeightCategories));
          
            PrioritySelctor.SelectedItem = (Priorities)0;
            WeightSelctor.SelectedItem = (WeightCategories)0;
        }

        private void AddParcelButton_Click(object sender, RoutedEventArgs e)
        {
            BO.Parcel newParcle = new();
            int.TryParse(ReceiverCustomerIdSelector.SelectedItem.ToString(), out int receiverId);
            int.TryParse(SenderCustomerIdSelector.SelectedItem.ToString(), out int senderId);

            newParcle.receiverCustomer.Id = receiverId;
            newParcle.senderCustomer.Id = senderId;
            newParcle.Priority = (BO.Priorities)PrioritySelctor.SelectedItem;
            newParcle.Weight = (BO.WeightCategories)WeightSelctor.SelectedItem;

            try
            {
                BLObject.AddNewParcelBL(newParcle);
                MessageBox.Show("Parcel has been added sucssesfuly",
                "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("Invalid input",
                  "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ViewDroneInParcel_Click(object sender, RoutedEventArgs e)
        {
            Parcel parcel = BLObject.FindParcelByIdBL(selecetedParcelToList.Id);
            var droneToList = (from drone in BLObject.ViewDroneToList() where drone.Id == parcel.Drone.Id select drone).FirstOrDefault();
            new DroneActions(droneToList).Show();
        }

        private void ViewReceiverCustomerInParcel_Click(object sender, RoutedEventArgs e)
        {
            Parcel parcel = BLObject.FindParcelByIdBL(selecetedParcelToList.Id);
            CustomerToList customerToList = BLObject.FindCustomerToList(parcel.receiverCustomer.Id);
            new CustomerActions(customerToList).Show();
        }

        private void ViewSenderCustomerInParcel_Click(object sender, RoutedEventArgs e)
        {
            Parcel parcel = BLObject.FindParcelByIdBL(selecetedParcelToList.Id);
            CustomerToList customerToList = BLObject.FindCustomerToList(parcel.senderCustomer.Id);
            new CustomerActions(customerToList).Show();
        }

        private void DeleteParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BLObject.DeleteParcel(selecetedParcelToList.Id);
                MessageBox.Show("Parcel has been removed", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                this.CloseButton_Click(sender, e);
            }
            catch (ObjectNotFoundException exception)
            {
                MessageBox.Show(exception.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }
    }
}
