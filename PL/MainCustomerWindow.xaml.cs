using System.Windows;
using System.Windows.Input;

namespace PL
{
    public partial class MainCustomerWindow : Window
    {
        private BlApi.IBL BLObgect;
        string userName;


        #region Constructor
        public MainCustomerWindow(string usreName)
        {
            InitializeComponent();

            try
            {
                BLObgect = BlApi.BlFactory.GetBl();
            }
            catch (DalApi.DalConfigException e)
            {
                MessageBox.Show(e.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            DataContext = false;
        }
        #endregion

      
        #region Add Parcel
        private void AddParcelFromCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            new AddParcelFromCustomerWindow(userName).Show();
        }
        #endregion


        #region View Parcel From Customer
        private void ViewParcelFromCustomer_Click(object sender, RoutedEventArgs e)
        {
            new ViewParcelFromCustomerWindow(userName).Show();   
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
