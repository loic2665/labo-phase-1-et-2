using System;
using System.Collections.ObjectModel;
using System.Windows;



using CLShapes;

namespace MyCartoObjWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnexionButton_Click(object sender, RoutedEventArgs e)
        {



            MyPersonalMapData userData = new MyPersonalMapData(InputNom.Text, InputPrenom.Text, InputEmail.Text)
            {
                Collection = new ObservableCollection<ICartoObj>()
            };

            try
            {
                userData.Load();
            }
            catch (Exception excp)
            {
                userData.Save();
            }

            MapWindow mapWindow = new MapWindow(userData);
            Hide();
            mapWindow.Show();
            Close();
            //this.Show(); // ou this.Close();

        }

    }
}
