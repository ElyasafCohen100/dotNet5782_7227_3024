using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IBL.BO;
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
    /// Interaction logic for UpdateDroneWindow.xaml
    /// </summary>
    public partial class UpdateDroneModel : Window
    {
        private IBL.IBL BlObject;
        private ViewDroneList viewDroneList;
        private int droneId;
        public UpdateDroneModel(IBL.IBL BlObject, ViewDroneList viewDroneList, int droneId)
        {
            InitializeComponent();
            this.BlObject = BlObject;
            this.viewDroneList = viewDroneList;
            this.droneId = droneId;
            UpdateButton.IsEnabled = false;
            DataContext = false;
        }


        //----------------  ModelTextBox ----------------//

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
        private void ModelIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-z,A-Z,0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        //---------------- UpdateDroneModelButton ----------------//

        private void UpdateDroneModelButton_Click(object sender, RoutedEventArgs e)
        {
            if (ModelTextBox.Text != "Enter Model")
            {

                String Model = ModelTextBox.Text;
                try
                {
                    BlObject.UpdateDroneModelBL(droneId, Model);
                    MessageBox.Show("Drone has been update sucssesfuly",
                        "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

                    viewDroneList.DroneListView.Items.Refresh();
                    this.Close();
                }
                catch (InvalidInputException)
                {
                    MessageBox.Show("Invalid input", "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Enter Model to update", "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void ModelTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (ModelTextBox.Text != String.Empty) UpdateButton.IsEnabled = true;
            else UpdateButton.IsEnabled = false;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }

        //Bouns.
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext.Equals(false)) e.Cancel = true;
        }
    }
}