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
    public partial class UpdateDroneModel: Window
    {
        private IBL.IBL BlObject;
        private DroneActions droneActions;

        public UpdateDroneModel(IBL.IBL BlObject, DroneActions droneActions)
        {
            InitializeComponent();
            this.BlObject = BlObject;
            this.droneActions = droneActions;
        }

        //------------------------------  DroneIdTextBox ------------------------------//

        private void DroneIdTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DroneIdTextBox.Text == "Enter Id")
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


        //------------------------ UpdateDroneModelButton ----------------------------//

        private void UpdateDroneModelButton_Click(object sender, RoutedEventArgs e)
        {
            int Id;
            int.TryParse(DroneIdTextBox.Text, out Id);
            String Model = ModelTextBox.Text;

            try
            {
                BlObject.UpdateDroneModelBL(Id, Model);
                MessageBox.Show("Drone has been update sucssesfuly ");
                this.Close();
            }
            catch (InvalidInputException)
            {
                MessageBox.Show("incorrect input");
            }
        }
    }
}
