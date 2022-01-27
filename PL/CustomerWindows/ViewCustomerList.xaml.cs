using System.Linq;
using System.Windows;
using System.Windows.Input;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ViewCustomerList.xaml
    /// </summary>
    public partial class ViewCustomerList : Window
    {
        private BlApi.IBL BLObject;

        #region Constructor
        public ViewCustomerList()
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

            CustomerListView.ItemsSource = BLObject.GetAllCustomerToList();
            DataContext = false;
        }
        #endregion


        #region Customer List View
        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CustomerListView.SelectedIndex >= 0)
            {
                CustomerToList selectedCustomer = BLObject.GetAllCustomerToList().ToList()[CustomerListView.SelectedIndex];
                if (new CustomerActions(selectedCustomer).ShowDialog() == false)
                {
                    CustomerListView.ItemsSource = BLObject.GetAllCustomerToList();
                }
            }
        }
        #endregion


        #region Add A New Customer Button
        private void AddNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (new CustomerActions().ShowDialog() == false) // if the window close
            {
                CustomerListView.ItemsSource = BLObject.GetAllCustomerToList();
            }
        }
        #endregion


        #region Close Window
        private void CLoseButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }

        /// <summary>
        /// cancel the "X" close Bottun
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext.Equals(false)) e.Cancel = true;
        }

        /// <summary>
        /// hidden the upper line (Minimize closing and window enlargement)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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