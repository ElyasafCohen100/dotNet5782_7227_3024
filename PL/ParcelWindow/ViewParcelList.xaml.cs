using System;
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
        private BlApi.IBL BLObject = BlApi.BlFactory.GetBl();
        public ViewParcelList()
        {
            InitializeComponent();
            ParcelListView.ItemsSource = BLObject.ViewParcelToList();
        }

        private void ViewReceivedParcelsList_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<IGrouping<string, ParcelToList>> parcelGroup = from parcel in BLObject.ViewParcelToList() group parcel by parcel.ReceiverName;
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

        private void ViewSenderParcelList_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<IGrouping<string, ParcelToList>> parcelGroup = from parcel in BLObject.ViewParcelToList() group parcel by parcel.SenderName;
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

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            var parcelList = from parcel in BLObject.ViewParcelsList()
                             where FirstDate.SelectedDate.Value.Date.CompareTo(parcel.Requested) <= 0 && LastDate.SelectedDate.Value.Date.CompareTo(parcel.Requested) >= 0
                             select parcel;

            List<ParcelToList> parcels = new();
            foreach (var par in parcelList)
            {
                parcels.Add(BLObject.FindParcelToList(par.Id));
            }

            ParcelListView.ItemsSource = parcels;
        }

        private void ParcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ParcelListView.SelectedIndex >= 0)
            {
                ParcelToList selectedParcel = BLObject.ViewParcelToList().ToList()[ParcelListView.SelectedIndex];
                if (new ParcelActions(selectedParcel).ShowDialog() == false)
                    ParcelListView.ItemsSource = BLObject.ViewParcelToList();
            }
        }

        private void AddParcel_Click(object sender, RoutedEventArgs e)
        {
            if (new ParcelActions().ShowDialog() == false)
                ParcelListView.ItemsSource = BLObject.ViewParcelToList();
        }
    }
}