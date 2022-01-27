using System.Windows;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainAdminWindow : Window
    {
        internal static BlApi.IBL BLObject;

        #region Constructor
        public MainAdminWindow()
        {
            InitializeComponent();

            try
            {
                BLObject = BlApi.BlFactory.GetBl();
            }
            catch(DalApi.DalConfigException e)
            {
                MessageBox.Show(e.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            DataContext = false;
        }
        #endregion


        #region View Function (Buttons)
        private void ViewDroneList_Click(object sender, RoutedEventArgs e)
        {
            new ViewDroneList().Show();
        }
      
        private void ViewStationList_Click(object sender, RoutedEventArgs e)
        {
            new ViewStationList().Show();
        }

        private void ViewCustomerList_Click(object sender, RoutedEventArgs e)
        {
            new ViewCustomerList().Show();
        }
        
        private void ViewParcelList_Click(object sender, RoutedEventArgs e)
        {
            new ViewParcelList().Show();
        }
        #endregion


        #region Close Window
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            Close();
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
