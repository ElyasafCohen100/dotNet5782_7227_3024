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
        internal static BlApi.IBL BLObject = new BL.BL();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Show_Drone_List(object sender, RoutedEventArgs e)
        {
            //---- sound while you're clicking on the button ----//
            System.Media.SoundPlayer player = new(@"sources\ES_Apple Mouse Click 1 - SFX Producer.wav");
            player.Load();
            player.PlaySync();

            new ViewDroneList(BLObject).Show();
        }
    }
}