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
    /// Interaction logic for ViewParcelDetails.xaml
    /// </summary>
    public partial class ViewParcelDetails : Window
    {
        private BlApi.IBL BLObject;
        
        public ViewParcelDetails(BlApi.IBL BLObject, int parcelId)
        {
            InitializeComponent();

            this.BLObject = BLObject;
            DataContext = false;

            BO.Parcel parcel = BLObject.FindParcelByIdBL(parcelId);

            ParcelID.Text = parcel.Id.ToString();

            SenderId.Text = parcel.senderCustomer.Id.ToString();
            SenderName.Text = parcel.senderCustomer.Name;

            ReciverId.Text = parcel.receiverCustomer.Id.ToString();
            ReciverName.Text = parcel.receiverCustomer.Name;

            DroneId.Text = parcel.Drone.Id.ToString();
            Priority.Text = parcel.Priority.ToString();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext.Equals(false)) e.Cancel = true;
        }
    }
}