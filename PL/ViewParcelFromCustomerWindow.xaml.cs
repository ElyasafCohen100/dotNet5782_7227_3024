using BO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for ViewParcelFromCustomerWindow.xaml
    /// </summary>
    public partial class ViewParcelFromCustomerWindow : Window
    {
        private BlApi.IBL BLObject;
        private string userName;

        #region Constructor
        public ViewParcelFromCustomerWindow(string useName)
        {
            InitializeComponent();

            try
            {
                BLObject = BlApi.BlFactory.GetBl();
                this.userName = userName;
            }
            catch (DalApi.DalConfigException e)
            {
                MessageBox.Show(e.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Customer customer = BLObject.GetCustomerByUserName(userName);

            ParcelListView.ItemsSource = from parcelToList in customer.ParcelFromCustomerList select BLObject.GetParcelToList(parcelToList.Id);
           
            DataContext = false;
        }
        #endregion


        #region Parcel List View
        private void ParcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ParcelListView.SelectedIndex >= 0)
            {
                ParcelToList selectedParcel = (ParcelToList)ParcelListView.SelectedItem;
                if (new ParcelActions(selectedParcel).ShowDialog() == false)
                {
                    Customer customer = BLObject.GetCustomerByUserName(userName);
                    ParcelListView.ItemsSource = from parcelToList in customer.ParcelFromCustomerList select BLObject.GetParcelToList(parcelToList.Id);
                }
            }
        }
        #endregion


        #region Window Closing
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