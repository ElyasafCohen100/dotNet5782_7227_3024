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

        public DroneActions(IBL.IBL BlObject, ViewDroneList viewDroneList, DroneToList selcetedDrone)
        {
            InitializeComponent();
            this.BlObject = BlObject;
            this.viewDroneList = viewDroneList;
            this.selcetedDrone = selcetedDrone;
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
    }
}
