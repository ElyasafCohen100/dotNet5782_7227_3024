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

namespace PL
{
    /// <summary>
    /// Interaction logic for ViewDrone.xaml
    /// </summary>
    public partial class ViewDrone : Window
    {
        private IBL.IBL BLObject;
        private ViewDroneList viewDroneListWindow;

        public ViewDrone(IBL.IBL BLObject, ViewDroneList viewDroneListWindow)
        {
            InitializeComponent();
            this.BLObject = BLObject;
        }

        private void AddNewDrone_Click(object sender, RoutedEventArgs e)
        {
            new AddNewDroneWindow(this.BLObject, this.viewDroneListWindow).Show();
        }
    }
}
