using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BO;
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
        private BlApi.IBL BLObject;
        private int droneId;
        public UpdateDroneModel(int droneId)
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
            if (ModelTextBox.Text != String.Empty)
            {
                String Model = ModelTextBox.Text;
                try
                {
                    BLObject.UpdateDroneModelBL(droneId, Model);
                    MessageBox.Show("Drone has been update sucssesfuly",
                                    "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close_Button_Click(sender, e);
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
        
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = true;
            this.Close();
        }

        //Bouns.
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext.Equals(false)) e.Cancel = true;
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateDroneModelButton_Click(sender, e);
            }
        }
    }
}
