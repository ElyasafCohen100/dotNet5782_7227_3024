//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace PL
//{
//    /// <summary>
//    /// Interaction logic for DroneActions.xaml
//    /// </summary>
//    public partial class DroneActions : Window
//    {
//        private IBL.IBL BlObject;

//        public DroneActions(IBL.IBL blObject)
//        {
//            InitializeComponent();
//            this.BlObject = blObject;
//        }

//        private void UpdateDroneModel_Click(object sender, RoutedEventArgs e)
//        {
//            new UpdateDroneWindow(BlObject).Show();
//        }
//    }
//}
