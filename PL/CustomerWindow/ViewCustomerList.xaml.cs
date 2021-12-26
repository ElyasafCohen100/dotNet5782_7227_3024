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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ViewCustomerList.xaml
    /// </summary>
    public partial class ViewCustomerList : Window
    {
        private BlApi.IBL BLObject;
        public ViewCustomerList(BlApi.IBL BLobject)
        {
            InitializeComponent();
            this.BLObject = BLObject;
            DataContext = false;

            CustomerListView.ItemsSource = BLobject.ViewCustomerToList();
        }

        private void AddNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerToList selectCustomer = new();
            if (new CustomerAction(BLObject,selectCustomer).ShowDialog() == false)
            {
                CustomerListView.Items.Refresh();
            }
        }

        private void CLoseButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }

        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //---- sound while you're clicking on the button ----//
            System.Media.SoundPlayer player = new (@"sources/clickSound.wav");
            player.Load();
            player.PlaySync();

            if (CustomerListView.SelectedIndex >= 0)
            {
                CustomerToList selectCustomer = BLObject.ViewCustomerToList().ToList()[CustomerListView.SelectedIndex];
                if(new CustomerAction(BLObject,selectCustomer).ShowDialog() == false)
                {
                    CustomerListView.Items.Refresh();
                }
            }
        }
    }
}
        