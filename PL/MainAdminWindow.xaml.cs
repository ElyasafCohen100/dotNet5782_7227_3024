using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BO;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainAdminWindow : Window
    {
        internal static BlApi.IBL BLObject = BlApi.BlFactory.GetBl();

        public MainAdminWindow()
        {
            InitializeComponent();
            DataContext = false;
        }

        private void ViewDroneList_Click(object sender, RoutedEventArgs e)
        {
            new ViewDroneList().Show();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext.Equals(false)) e.Cancel = true;
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
    }
}
