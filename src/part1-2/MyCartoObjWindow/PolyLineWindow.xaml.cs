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
using Polyline = CLShapes.Polyline;

namespace MyCartoObjWindow
{
    /// <summary>
    /// Interaction logic for PolyLineWindow.xaml
    /// </summary>
    public partial class PolyLineWindow : Window
    {
        private Polyline _poly;
        private int _thickness;

        public Polyline Poly
        {
            get { return _poly; }
            set { _poly = value; }
        }

        public int Thickness
        {
            get { return _thickness; }
            set { _thickness = value; }
        }

        public PolyLineWindow(Polyline polyline, String title)
        {
            InitializeComponent();
            Poly = polyline;
            PolylineTitle.Content = title;
            this.Title = title;

            PolyLineThicknessSlider.Value = Poly.Epaisseur;
            PolyLineName.Text = Poly.Nom;
            PolyLineLongitude.Text = Poly.Coordonnees[0].Longitude.ToString();
            PolyLineLatitude.Text = Poly.Coordonnees[0].Latitude.ToString(); ;
            PolyLineLongitude.IsReadOnly = true;
            PolyLineLatitude.IsReadOnly = true;

        }

        

        private void PolyLineThicknessSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Thickness = Convert.ToInt32(e.NewValue);
            PolyLineThicknessLabel.Content = Thickness + " px";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (PolyLineName.Text != Poly.Nom ||
                Thickness != Poly.Epaisseur)
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
            Poly.Epaisseur = Thickness;
            Poly.Nom = PolyLineName.Text;
            if (PolyLineColor.SelectedColor == null)
            {
                Poly.Couleur = Colors.Blue;
            }
            else
            {
                Poly.Couleur = (Color)PolyLineColor.SelectedColor;
            }
            this.Close();
        }
    }
}
