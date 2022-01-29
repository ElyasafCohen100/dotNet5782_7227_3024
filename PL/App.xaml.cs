using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
      
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //---- sound while you're clicking on the button ----//
            System.Media.SoundPlayer player = new(@"sources/clickSound.wav");
            player.Load();
            player.PlaySync();
        }
    }
}
