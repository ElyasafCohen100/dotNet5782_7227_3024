using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for AddParcelFromCustomerWindow.xaml
    /// </summary>
    public partial class AddParcelFromCustomerWindow : Window
    {
        private BlApi.IBL BLObject;
        string userName;

        #region Constructor
        public AddParcelFromCustomerWindow(string userName)
        {
            InitializeComponent();
            this.userName = userName;
            try
            {
                BLObject = BlApi.BlFactory.GetBl();
            }
            catch (DalApi.DalConfigException e)
            {
                MessageBox.Show(e.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            var customersList = from customer in BLObject.GetAllCustomerToList() select customer.Id;
            Customer user = BLObject.GetCustomerByUserName(userName);
            if (customersList.Count() > 0)
            {
                ReceiverCustomerIdSelector.ItemsSource = customersList;
                ReceiverCustomerIdSelector.SelectedItem = customersList.First();
            }

            PrioritySelctor.ItemsSource = Enum.GetValues(typeof(Priorities));
            WeightSelctor.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            PrioritySelctor.SelectedItem = (Priorities)0;
            WeightSelctor.SelectedItem = (WeightCategories)0;
        }
        #endregion


        #region Add
        private void AddParcelButton_Click(object sender, RoutedEventArgs e)
        {
            Parcel newParcle = new();
            int.TryParse(ReceiverCustomerIdSelector.SelectedItem.ToString(), out int receiverId);


            newParcle.receiverCustomer.Id = receiverId;
            newParcle.senderCustomer.Id = BLObject.GetCustomerByUserName(userName).Id;
            newParcle.Priority = (Priorities)PrioritySelctor.SelectedItem;
            newParcle.Weight = (WeightCategories)WeightSelctor.SelectedItem;

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
        #endregion

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

    }
}
