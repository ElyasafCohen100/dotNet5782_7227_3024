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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static BlApi.IBL BLObject;

        public MainWindow()
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
        }

        private void ViewDroneList_Click(object sender, RoutedEventArgs e)
        {
            new ViewDroneList(BLObject).Show();
        }

        private void ViewStationList_Click(object sender, RoutedEventArgs e)
        {
            new ViewStationList(BLObject).Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext.Equals(false)) e.Cancel = true;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }

        private void ViewCustomerList_Click(object sender, RoutedEventArgs e)
        {
            new ViewCustomerList(BLObject).Show();
        }
    }
}