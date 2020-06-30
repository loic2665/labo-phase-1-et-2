using System.Windows;

namespace MyCartoObjWindow
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void CloseAbout_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
