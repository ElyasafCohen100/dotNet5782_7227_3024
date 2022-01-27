using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ViewParcelList.xaml
    /// </summary>
    public partial class ViewParcelList : Window
    {
        private BlApi.IBL BLObject;

        #region Constructor
        public ViewParcelList()
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
            ParcelListView.ItemsSource = BLObject.GetAllParcelToList();
        }
        #endregion


        #region Order The Parcel's List By The Receiver Name (Grouping)
        private void ViewReceivedParcelsList_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<IGrouping<string, ParcelToList>> parcelGroup = from parcel in BLObject.GetAllParcelToList() group parcel by parcel.ReceiverName;
            List<ParcelToList> parcelList = new();

            foreach (IGrouping<string, ParcelToList> group in parcelGroup)
            {
                foreach (var parcel in group)
                {
                    parcelList.Add(parcel);
                }
            }
            ParcelListView.ItemsSource = parcelList;
        }
        #endregion


        #region Order The Parcel's List By The Sender Name (Grouping)
        private void ViewSenderParcelList_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<IGrouping<string, ParcelToList>> parcelGroup = from parcel in BLObject.GetAllParcelToList() group parcel by parcel.SenderName;
            List<ParcelToList> parcelList = new();

            foreach (var group in parcelGroup)
            {
                foreach (var parcel in group)
                {
                    parcelList.Add(parcel);
                }
            }
            ParcelListView.ItemsSource = parcelList;
        }
        #endregion


        #region Select Button
        /// <summary>
        /// displaying all the requested parcels that have been requested at the chosen range
        /// </summary>
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var parcelList = from parcel in BLObject.GetAllParcels()
                                 where FirstDate.SelectedDate.Value.Date.CompareTo(parcel.Requested) <= 0 &&
                                       LastDate.SelectedDate.Value.Date.CompareTo(parcel.Requested) >= 0
                                 select parcel;


                List<ParcelToList> parcels = new();
                foreach (var par in parcelList)
                {
                    parcels.Add(BLObject.GetParcelToList(par.Id));
                }

                ParcelListView.ItemsSource = parcels;
            }
            catch (System.InvalidOperationException)
            {
                MessageBox.Show("Please select a date","Operation Failure", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion


        #region Parcel List View
        /// <summary>
        /// display a single parcel from "ParcelListView"
        /// </summary>
        private void ParcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ParcelListView.SelectedIndex >= 0)
            {
                ParcelToList selectedParcel = BLObject.GetAllParcelToList().ToList()[ParcelListView.SelectedIndex];
                if (new ParcelActions(selectedParcel).ShowDialog() == false)
                {
                    ParcelListView.ItemsSource = BLObject.GetAllParcelToList(); // refrash
                }
            }
        }
        #endregion


        #region Add A New Parcel Button
        private void AddParcel_Click(object sender, RoutedEventArgs e)
        {
            if (new ParcelActions().ShowDialog() == false)
            {
                ParcelListView.ItemsSource = BLObject.GetAllParcelToList(); // the list is update
            }
        }
        #endregion


        #region Order The Parcel's List By The Parcel Status
        private void ViewParcelStatus_Click(object sender, RoutedEventArgs e)
        {
            var ParcelList = from parcel in BLObject.GetAllParcelToList()
                             orderby parcel.ParcelStatus
                             select parcel;
            ParcelListView.ItemsSource = ParcelList;
        }
        #endregion
    }
}
