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

using CLShapes;

namespace MyCartoObjWindow
{
    /// <summary>
    /// Interaction logic for POIWindow.xaml
    /// </summary>
    /// 


    public partial class POIWindow : Window
    {

        private POI _poi;

        public POI Poi
        {
            get { return _poi; }
            set { _poi = value; }
        }



        public POIWindow(POI poi, String title)
        {
            InitializeComponent();
            Poi = poi;
            titlePOI.Content = title;
            this.Title = title;
            POIName.Text = Poi.Description;
            POILongitude.Text = Poi.Longitude.ToString();
            POILatitude.Text = Poi.Latitude.ToString();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (POIName.Text != Poi.Description || 
                POILongitude.Text != Poi.Longitude.ToString() ||
                POILatitude.Text != Poi.Latitude.ToString())
            {
                MessageBoxResult result = MessageBox.Show("Si vous continuez, les changements seront perdus.\n\nVoulez vous continuer ?", "Attention !", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Poi.Description = POIName.Text;
            Poi.Longitude = Convert.ToDouble(POILongitude.Text);
            Poi.Latitude = Convert.ToDouble(POILatitude.Text);
            this.Close();
        }
    }
}
