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
using System.Windows.Shapes;
using System.Text.RegularExpressions;


namespace PL
{
    /// <summary>
    /// Interaction logic for AddNewDroneWindow.xaml
    /// </summary>
    public partial class AddNewDroneWindow : Window
    {
        private IBL.IBL BLObject;
        private ViewDroneList viewDroneListWindow;

        public AddNewDroneWindow(IBL.IBL BLObject, ViewDroneList viewDroneListWindow)
        {
            InitializeComponent();
            this.BLObject = BLObject;
            this.viewDroneListWindow = viewDroneListWindow;

            MaxWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            MaxWeightSelector.SelectedItem = (WeightCategories)0;

            var baseStationsId = from b in BLObject.ViewBaseStationsToList() select b.Id;
            BaseStationIdSelector.ItemsSource = baseStationsId;
            BaseStationIdSelector.SelectedItem = baseStationsId.First();
        }

        //------------------------------  DroneIdTextBox ------------------------------//

        private void DroneIdTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(DroneIdTextBox.Text == "Enter Id")
                DroneIdTextBox.Clear();
            
        }
        private void DroneIdTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DroneIdTextBox.Text == String.Empty)
                DroneIdTextBox.Text = "Enter Id";
        }
        private void DroneIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //------------------------------  ModelTextBox ------------------------------//

        private void ModelTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ModelTextBox.Text == "Enter Model")
                ModelTextBox.Clear();
        }

        private void ModelTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ModelTextBox.Text == String.Empty)
                ModelTextBox.Text = "Enter Model";
        }

        //------------------------------  AddDroneButton ------------------------------//

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            int Id;
            int.TryParse(DroneIdTextBox.Text, out Id);
            String Model = ModelTextBox.Text;
            Drone newDrone = new();
            newDrone.Id = Id;
            newDrone.Model = Model;
            newDrone.MaxWeight = (WeightCategories)MaxWeightSelector.SelectedItem;
            try
            {
                BLObject.AddNewDroneBL(newDrone, (int)BaseStationIdSelector.SelectedItem);
                MessageBox.Show("Drone added sucssesfuly");
                this.Close();
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("incorrect input");
            }
            viewDroneListWindow.DroneListView.Items.Refresh();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
           this.Close();
        }
    }
}
