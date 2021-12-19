using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBL.BO;
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
    public partial class MainWindow : Window
    {
        internal static IBL.IBL BLObject = new BL.BL();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Show_Drone_List(object sender, RoutedEventArgs e)
        {
            new ViewDroneList(BLObject).Show();
        }
    }
}