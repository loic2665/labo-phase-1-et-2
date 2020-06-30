using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CLShapes;

using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Design;
using Microsoft.Win32;
using Polygon = CLShapes.Polygon;
using Polyline = CLShapes.Polyline;

namespace MyCartoObjWindow
{
    /// <summary>
    /// Interaction logic for MapWindow.xaml
    /// </summary>
    ///
    public enum ModeEnum { add, delete, edit, none };

    public enum TypeEnum { poi, line, poly, none };

    public enum LineStatus { first, adding };

    public partial class MapWindow : Window
    {
        private ModeEnum _mode;
        private TypeEnum _type;
        private LineStatus _status;
        private MyPersonalMapData _userData;
        private double _precision;

        private List<Coordonnees> _listCoords;

        private delegate void Add(Location loc);
        private Add AddDelegate;

        private delegate void Edit(Location loc);
        private Edit EditDelegate;

        private delegate void Delete(Location loc);
        private Delete RemoveDelegate;



        public ModeEnum Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public TypeEnum Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public LineStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private MyPersonalMapData UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        private List<Coordonnees> ListCoords
        {
            get { return _listCoords; }
            set { _listCoords = value; }
        }

        public double Precision
        {

            get { return _precision; }
            set { _precision = value; }
        }

        private LocationConverter locationConverter = new LocationConverter();

        public MapWindow(MyPersonalMapData data)
        {
            InitializeComponent();
            myMap.ZoomLevel = 2;
            Title = data.Nom + " - " + data.Prenom;
            Mode = ModeEnum.none;
            UserData = data;
            this.MapListBox.ItemsSource = UserData.Collection;
            statusText.Text = "Connecté !";

            AddDelegate = AddPushPin;
            EditDelegate = EditPushPin;
            RemoveDelegate = RemovePushPin;


            polylineWPF.Locations = new LocationCollection();
            polylineWPF.Stroke = new SolidColorBrush(Colors.Blue);
            polylineWPF.StrokeThickness = 5;
            polylineWPF.Opacity = 0.7;

            polylineWPF.Visibility = Visibility.Hidden;




            RenderAll();
            //statusBarMapWindow = "Utilisateur " + data.Nom + " " + data.Prenom + " connecté !";
        }

        private void MapModeLabelMode_Click(object sender, RoutedEventArgs e)
        {
            myMap.Mode = new RoadMode();
        }

        private void MapModeAerialMode_Click(object sender, RoutedEventArgs e)
        {
            myMap.Mode = new AerialMode();
        }

        private void MapModeAerialwithlabels_Click(object sender, RoutedEventArgs e)
        {
            myMap.Mode = new AerialMode();
        }

        private void MapAboutDialog_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }

        private void RadioAdd_Click(object sender, RoutedEventArgs e)
        {
            Mode = ModeEnum.add;
        }

        private void RadioRemove_Click(object sender, RoutedEventArgs e)
        {
            Mode = ModeEnum.delete;

        }

        private void myMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {


            Point mousePosition = e.GetPosition(myMap);
            //Convert the mouse coordinates to a locatoin on the map
            Location mouseLocation = myMap.ViewportPointToLocation(mousePosition);
            try
            {
                if (AddActionRadio.IsChecked == true)
                {
                    e.Handled = true;
                    AddDelegate?.Invoke(mouseLocation);
                }
                else if (EditActionRadio.IsChecked == true)
                {
                    e.Handled = true;
                    EditDelegate?.Invoke(mouseLocation);
                }
                else if (RemoveActionRadio.IsChecked == true)
                {
                    e.Handled = true;
                    RemoveDelegate?.Invoke(mouseLocation);
                }

                MapListBox.Items.Refresh();
            }
            catch (NotImplementedException exception)
            {
                MessageBox.Show("Fonction non implémentée !\n\nStack:\n\n"+ exception.StackTrace, "Oops !", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            

        }


        private void AddPushPin(Location loc)
        {
            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = loc;
            
            // Adds the pushpin to the map.
            myMap.Children.Add(pin);

            // code perso
            POI poi = new POI(pin.Location.Longitude, pin.Location.Latitude, "");

            POIWindow poiWindow = new POIWindow(poi, "Ajouter un POI");
            poiWindow.ShowDialog();

            UserData.Collection.Add(poi);

            pin.Tag = poi; // quand on traversara les collection, on pourra chercher
            // le poi en fonction de notre collection

            statusText.Text = "Point ajouté :\nLong : " + pin.Location.Longitude + " / Lat: "+ pin.Location.Latitude;
        }

        private void AddPolyLine(Location loc)
        {
            if (Status == LineStatus.first)
            {
                /* mon code */
                ListCoords = new List<Coordonnees>();
                Coordonnees coord = new Coordonnees(loc.Longitude, loc.Latitude);
                ListCoords.Add(coord);
                Status = LineStatus.adding;

                /* wpf map */
                polylineWPF.Locations.Add(loc);
                polylineWPF.Visibility = Visibility.Visible;

            }
            else if (Status == LineStatus.adding && !(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl))) {
                
                /* perso */
                Coordonnees coord = new Coordonnees(loc.Longitude, loc.Latitude);
                ListCoords.Add(coord);

                /* wpf map */
                polylineWPF.Locations.Add(loc);

            } else if (Status == LineStatus.adding && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl))) {
                
                /* perso */
                Coordonnees coord = new Coordonnees(loc.Longitude, loc.Latitude);
                ListCoords.Add(coord);
                Status = LineStatus.first;

                Polyline polyline = new Polyline(ListCoords, 5, Colors.Blue, "");

                polylineWPF.Locations.Add(loc);

                PolyLineWindow polylineWindow = new PolyLineWindow(polyline, "Ajouter une polyline");
                polylineWindow.ShowDialog();

                UserData.Collection.Add(polyline);

                /* wpf map */

                polylineWPF.Visibility = Visibility.Hidden;
                CreateNewPolyline(polyline);
                polylineWPF.Locations.Clear();


                

                statusText.Text = "Polyline ajoutée";
                // pour eviter le cas :
                // ajouter une deuxieme polyline, et qu'il reprenne les
                // anciens points + les nouveaux

            }
        }

        private void AddPolygon(Location loc)
        {
            throw new NotImplementedException();
        }

        private void EditPushPin(Location loc)
        {
            POI poi = getNearPOI(loc);
            if (poi != null)
            {
                POIWindow poiWindow = new POIWindow(poi, "Editer un POI");
                poiWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Aucun point trouvé à proximité !", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void EditPolyLine(Location loc)
        {
            Console.WriteLine("EditPolyLine !!!");
            Polyline polyline = getNearPolyline(loc);
            if (polyline != null)
            {
                PolyLineWindow polylineWindow = new PolyLineWindow(polyline, "Editer une polyline");
                polylineWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Aucune polyline trouvé à proximité !", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EditPolygon(Location loc)
        {

            throw new NotImplementedException();
        }



        private void RemovePushPin(Location loc)
        {
            Coordonnees mouseCoordonnees = new Coordonnees(loc.Longitude, loc.Latitude);

            bool trouve = false;
            POI currentItem = null;
            int i;


            i = 0;
            while (i < UserData.Collection.Count && trouve == false)
            {
                if (UserData.Collection[i] is POI)
                {
                    currentItem = UserData.Collection[i] as POI;
                    if (currentItem.IsPointClose(mouseCoordonnees, Precision))
                    {
                        trouve = true;
                        UserData.Collection.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

            }

            if (trouve)
            {
                i = 0;
                while (i < myMap.Children.Count)
                {
                    if (myMap.Children[i] is Pushpin pp && pp.Tag == currentItem)
                    {
                        myMap.Children.Remove(pp);
                        // on enlève la PushPin en fonction du POI a supprimer
                        statusText.Text = "PushPin supprimé";
                    }

                    i++;
                }
            }
           
        }



        private void RemovePolyLine(Location loc)
        {
            Coordonnees mouseCoordonnees = new Coordonnees(loc.Longitude, loc.Latitude);

            bool trouve = false;
            Polyline currentItem = null;
            int i;

            i = 0;
            while (i < UserData.Collection.Count && trouve == false)
            {
                if (UserData.Collection[i] is Polyline)
                {
                    currentItem = UserData.Collection[i] as Polyline;
                    if (currentItem.IsPointClose(mouseCoordonnees, Precision))
                    {
                        trouve = true;
                    }
                }

                i++;

            }

            if (trouve)
            {

                i = 0;
                while (i < myMap.Children.Count)
                {
                    if (myMap.Children[i] is MapPolyline mp && mp.Tag == currentItem)
                    {
                        myMap.Children.Remove(mp);
                        // on enlève la MapPolyline en fonction de la polyline a supprimer
                    }

                    i++;
                }


                UserData.Collection.Remove(currentItem);
                Console.WriteLine("Ligne supprimé :\nLong : {0} / Lat: {1}", mouseCoordonnees.Longitude, mouseCoordonnees.Latitude);

            }
        }

        private void RemovePolygon(Location loc)
        {

            throw new NotImplementedException();
        }

        private void CreateNewPolyline(Polyline polyline)
        {
            MapPolyline wpfMapPolyline = new MapPolyline();
            
            wpfMapPolyline.Stroke = new SolidColorBrush(polyline.Couleur);
            wpfMapPolyline.StrokeThickness = polyline.Epaisseur;
            wpfMapPolyline.Opacity = 1;
            wpfMapPolyline.Locations = new LocationCollection();
            
            // il faut initialiser la liste sinon NullRefException !!! 

            foreach (Coordonnees item in polyline.Coordonnees)
            {
                Location loc = new Location(item.Latitude, item.Longitude);
                wpfMapPolyline.Locations.Add(loc);
            }

            myMap.Children.Add(wpfMapPolyline);

            wpfMapPolyline.Tag = polyline;



        }

        private void myMap_MouseClick(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteAllButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer tout les éléments de la carte ?", "Êtes-vous sûr ?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                myMap.Children.Clear();
                UserData.Reset();
            }
        }


        private void NavActionButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Object obj in ToolBarButtons.Items)
            {
                if (obj is RadioButton)
                {
                    RadioButton radio = obj as RadioButton;
                    radio.IsChecked = false;
                    
                }
            }
        }



        private void myMap_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            double nbZero = 1;
            nbZero = myMap.ZoomLevel / 6.5;
            nbZero = Math.Ceiling(nbZero);
            Precision = 1 / Math.Pow(10, nbZero);
            Console.WriteLine(Precision);
        }

        private void RadioEdit_Click(object sender, RoutedEventArgs e)
        {
            Mode = ModeEnum.edit;
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboType.SelectedIndex)
            {
                case 0:
                    AddDelegate = AddPushPin;
                    EditDelegate = EditPushPin;
                    RemoveDelegate = RemovePushPin;
                    break;
                case 1:
                    AddDelegate = AddPolyLine;
                    EditDelegate = EditPolyLine;
                    RemoveDelegate = RemovePolyLine;
                    break;
                case 2:
                    AddDelegate = AddPolygon;
                    EditDelegate = EditPolygon;
                    RemoveDelegate = RemovePolygon;
                    break; // polygon non fait
            }
        }


        private POI getNearPOI(Location loc)
        {
            Coordonnees mouseCoordonnees = new Coordonnees(loc.Longitude, loc.Latitude);

            bool trouve = false;
            POI currentItem = null;
            int i;


            i = 0;
            while (i < UserData.Collection.Count && trouve == false)
            {
                if (UserData.Collection[i] is POI)
                {
                    currentItem = UserData.Collection[i] as POI;
                    if (currentItem.IsPointClose(mouseCoordonnees, Precision))
                    {
                        trouve = true;
                    }
                }
                i++;

            }

            if (trouve)
            {
                return currentItem;
            }
            else
            {
                return null;
            }
        }


        private Polyline getNearPolyline(Location loc)
        {

            Coordonnees mouseCoordonnees = new Coordonnees(loc.Longitude, loc.Latitude);

            bool trouve = false;
            Polyline currentItem = null;
            int i;

            i = 0;
            while (i < UserData.Collection.Count && trouve == false)
            {
                if (UserData.Collection[i] is Polyline)
                {
                    currentItem = UserData.Collection[i] as Polyline;
                    if (currentItem.IsPointClose(mouseCoordonnees, Precision))
                    {
                        trouve = true;
                    }
                    
                }

                i++;

            }

            if (trouve)
            {
                return currentItem;
            }
            else
            {
                return null;
            }

        }

        private void POIImport_Click(object sender, RoutedEventArgs e)
        {
            ProgressWindow progressWindow = new ProgressWindow(UserData.Collection, myMap, "POI-Import", "Importation...");
            
            statusText.Text = "Fichier importé.";
        }

        private void POIExport_Click(object sender, RoutedEventArgs e)
        {
            ProgressWindow progressWindow = new ProgressWindow(UserData.Collection, myMap, "POI-Export", "Exportation...");

            statusText.Text = "POIs exportés.";
        }

        private void PolyImport_Click(object sender, RoutedEventArgs e)
        {
            ProgressWindow progressWindow = new ProgressWindow(UserData.Collection, myMap, "PolyLine-Import", "Importation...");
            statusText.Text = "Fichier importé.";
        }

        private void PolyExport_Click(object sender, RoutedEventArgs e)
        {
            ProgressWindow progressWindow = new ProgressWindow(UserData.Collection, myMap, "PolyLine-Export", "Exportation...");

            statusText.Text = "PolyLines exportés.";
        }

        private void MapListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UserData.Collection.Count < 1)
            {
                return;
            }
            ICartoObj item = MapListBox.SelectedItems[0] as ICartoObj;
            Location loc = new Location();
            
            if (item is POI poi)
            {
                loc.Longitude = poi.Longitude;
                loc.Latitude = poi.Latitude;
            }
            else if (item is Polyline poly)
            {

                loc.Longitude = poly.Coordonnees[0].Longitude;
                loc.Latitude = poly.Coordonnees[0].Latitude;
            }
            else if (item is Polygon plg)
            {
                // non fait mais l'integration est facile
                loc.Longitude = plg.Coordonnees[0].Longitude;
                loc.Latitude = plg.Coordonnees[0].Latitude;
            }
            myMap.SetView(loc, 15, 0);
        }

        private void OptionWindow_Click(object sender, RoutedEventArgs e)
        {

            OptionWindow optWindow = new OptionWindow(UserData.Path,
                                           ((SolidColorBrush) MapListBox.Background).Color,
                                           ((SolidColorBrush) MapListBox.Foreground).Color
                                        );
            optWindow.Show();
            optWindow.OnApply += OptWindow_OnApply;

        }

        private void OptWindow_OnApply(string path, Color bgColor, Color fgColor)
        {
            MapListBox.Background = new SolidColorBrush(bgColor);
            MapListBox.Foreground = new SolidColorBrush(fgColor);
            UserData.Path = path;
            
        }

        private void SaveSession_Click(object sender, RoutedEventArgs e)
        {
            UserData.Save();
        }

        private void LoadSession_OnClickSession_Click(object sender, RoutedEventArgs e)
        {
            myMap.Children.Clear();
            UserData.Collection.Clear();
            UserData.Load();
            RenderAll();
        }

        private void RenderAll()
        {
            foreach (ICartoObj obj in UserData.Collection)
            {
                if (obj is POI pp)
                {
                    Location loc = new Location(pp.Latitude, pp.Longitude);
                    Pushpin pushpin = new Pushpin();
                    pushpin.Location = loc;
                    pushpin.Tag = obj;
                    myMap.Children.Add(pushpin);
                }else if (obj is Polyline poly)
                {
                    MapPolyline mapPoly = new MapPolyline();
                    foreach (Coordonnees coord in poly.Coordonnees)
                    {
                        Location loc = new Location(coord.Latitude, coord.Longitude);
                        mapPoly.Locations.Add(loc);
                    }

                    mapPoly.Tag = obj;
                    myMap.Children.Add(mapPoly);
                }
            }
        }
    }
}