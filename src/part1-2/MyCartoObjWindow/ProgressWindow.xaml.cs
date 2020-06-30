using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

using CLShapes;

using Microsoft.Maps.MapControl.WPF;
using Microsoft.Win32;

//arhhh

namespace MyCartoObjWindow
{
    /// <summary>
    /// Interaction logic for ImportWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        private Map _map;
        private ObservableCollection<ICartoObj> _userList;
        private float _nbObj;
        private float _currNbItem;

        private String _fileName;

        private Mutex mut = new Mutex();

        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }

        public ObservableCollection<ICartoObj> UserList
        {
            get { return _userList; }
            set { _userList = value; }
        }

        public float NbObj // total
        {
            get { return _nbObj; }
            set { _nbObj = value; }
        }

        public float CurrNbObj //
        {
            get { return _currNbItem; }
            set { _currNbItem = value; }
        }

        public String FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public ProgressWindow(ObservableCollection<ICartoObj> userList, Map map, String typeToImport, String labelText)
        {
            InitializeComponent();
            ProgressBarProgression.Value = 0;

            UserList = userList;
            Map = map;

            CurrNbObj = 0;
            CurrentNumItem.Content = CurrNbObj;

            Dispatcher pdDispatcher = this.Dispatcher;
            BackgroundWorker worker = new BackgroundWorker();

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            if (typeToImport == "POI-Import")
            {
                /// *************************
                /// POI IMPORT
                /// *************************

                LabelAction.Content = labelText;

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Fichiers CSV (.csv)|*.csv";
                dialog.DefaultExt = ".csv";
                bool? result = dialog.ShowDialog(); // idem as Nullable<bool>;

                if (result == true)
                {
                    //background worker
                    FileName = dialog.FileName;
                    worker.DoWork += delegate (object sender, DoWorkEventArgs args)
                    {
                        String completeFile = File.ReadAllText(FileName);
                        String[] lines = Regex.Split(completeFile, "\r\n");
                        NbObj = lines.Length;

                        /*
                         * Test perso wpf progression
                         */

                        foreach (String line in lines)
                        {
                            String[] cols = Regex.Split(line, ";");
                            if (cols.Length <= 2)
                            {
                                CurrNbObj++;
                                continue;
                            }

                            POI poi = new POI(Convert.ToDouble(cols[1]), Convert.ToDouble(cols[0]), cols[2]);

                            mut.WaitOne();
                            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate ()
                            {
                                Pushpin pin = new Pushpin();
                                pin.Location = new Location(poi.Latitude, poi.Longitude);
                                pin.Tag = poi;
                                this.Map.Children.Add(pin);
                                UserList.Add(poi);
                                // progress bar
                                CurrNbObj++;
                                worker.ReportProgress(Convert.ToInt32((CurrNbObj / NbObj) * 100));
                                //Thread.Sleep(1);
                            }
                            ));
                            mut.ReleaseMutex();
                        }
                    };

                    worker.ProgressChanged += delegate (object s, ProgressChangedEventArgs args)
                    {
                        ProgressBarProgression.Value = args.ProgressPercentage;
                        CurrentNumItem.Content = (int)CurrNbObj;
                        TotalNbItem.Content = (int)NbObj;
                    };

                    worker.RunWorkerCompleted += delegate (object s, RunWorkerCompletedEventArgs args)
                    {
                        this.Close();
                    };

                    worker.RunWorkerAsync();
                    this.Show();
                }
            }
            else if (typeToImport == "PolyLine-Import")
            {
                /// ****************************
                /// POLY LINE IMPORT
                /// ****************************

                LabelAction.Content = labelText;

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Fichiers CSV (.csv)|*.csv";
                dialog.DefaultExt = ".csv";
                bool? result = dialog.ShowDialog(); // idem as Nullable<bool>;

                if (result == true)
                {
                    //background worker
                    FileName = dialog.FileName;
                    worker.DoWork += delegate (object sender, DoWorkEventArgs args)
                    {
                        String completeFile = File.ReadAllText(FileName);
                        String[] lines = Regex.Split(completeFile, "\r\n");
                        NbObj = lines.Length;

                        // Test perso wpf progression

                        

                        this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate ()
                        {
                            MapPolyline mapPolyline = new MapPolyline();
                            mapPolyline.Locations = new LocationCollection();
                            Polyline polyline = new Polyline();
                            List<Coordonnees> listCoord = new List<Coordonnees>();

                            foreach (String line in lines)
                            {
                                mut.WaitOne();
                                String[] cols = Regex.Split(line, ";");
                                if (cols.Length < 2)
                                {
                                    CurrNbObj++;
                                    mut.ReleaseMutex();
                                    continue;
                                }

                                Coordonnees coord = new Coordonnees(Convert.ToDouble(cols[1]), Convert.ToDouble(cols[0]));
                                

                                listCoord.Add(coord);
                                polyline.Coordonnees.Add(coord);

                                Location location = new Location(coord.Latitude, coord.Longitude);
                                mapPolyline.Locations.Add(location);

                                if (cols.Length != 2 && cols[2] != String.Empty)
                                {
                                    // c'est un point avec POI
                                    POI poi = new POI(coord.Longitude, coord.Latitude, cols[2]);
                                    UserList.Add(poi);

                                    Pushpin pin = new Pushpin();
                                    pin.Location = new Location(location);
                                    pin.Tag = poi;

                                    Map.Children.Add(pin);
                                }

                                // progress bar
                                CurrNbObj++;
                                worker.ReportProgress(Convert.ToInt32((CurrNbObj / NbObj) * 100));
                                mut.ReleaseMutex();
                            }

                            mapPolyline.StrokeThickness = 5;
                            mapPolyline.Stroke = new SolidColorBrush(Colors.Blue);
                            mapPolyline.Tag = polyline;
                            UserList.Add(polyline);
                            Map.Children.Add(mapPolyline);
                        }
                        ));
                    };

                    worker.ProgressChanged += delegate (object s, ProgressChangedEventArgs args)
                    {
                        ProgressBarProgression.Value = args.ProgressPercentage;
                        CurrentNumItem.Content = (int)CurrNbObj;
                        TotalNbItem.Content = (int)NbObj;
                    };

                    worker.RunWorkerCompleted += delegate (object s, RunWorkerCompletedEventArgs args)
                    {
                        this.Close();
                    };

                    worker.RunWorkerAsync();
                    this.ShowDialog();
                }
            } else if (typeToImport == "POI-Export")
            {


                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Fichier .CSV (sep. ';') (*.csv)|*.csv";
                if (saveFileDialog.ShowDialog() == true)
                {
                    FileName = saveFileDialog.FileName;
                    FileStream fs = File.OpenWrite(FileName);
                    StreamWriter sr = new StreamWriter(fs);


                    worker.DoWork += delegate(object sender, DoWorkEventArgs args)
                    {

                        NbObj = UserList.Count(x => x is POI);
                        CurrNbObj = 0;
                        foreach (ICartoObj item in UserList)
                        {
                            if (item is POI poi)
                            {
                                sr.WriteLine(poi.Longitude + ";" + poi.Latitude + ";" + poi.Description);
                                CurrNbObj++;
                                worker.ReportProgress(Convert.ToInt32((CurrNbObj / NbObj) * 100));
                            }

                        }

                        sr.Dispose();
                        fs.Dispose();

                    };

                    worker.ProgressChanged += delegate (object s, ProgressChangedEventArgs args)
                    {
                        ProgressBarProgression.Value = args.ProgressPercentage;
                        CurrentNumItem.Content = (int)CurrNbObj;
                        TotalNbItem.Content = (int)NbObj;
                    };

                    worker.RunWorkerCompleted += delegate (object s, RunWorkerCompletedEventArgs args)
                    {
                        this.Close();
                    };

                    worker.RunWorkerAsync();
                    this.ShowDialog();
                }

            }
            else if (typeToImport == "PolyLine-Export")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Fichier .CSV (sep. ';') (*.csv)|*.csv";
                saveFileDialog.AddExtension = false;
                if (saveFileDialog.ShowDialog() == true)
                {


                    worker.DoWork += delegate (object sender, DoWorkEventArgs args)
                    {

                        NbObj = UserList.Count(x => x is Polyline);
                        CurrNbObj = 0;
                        foreach (ICartoObj item in UserList)
                        {
                            if (item is Polyline pl)
                            {
                                
                                DirectoryInfo dir = new DirectoryInfo(saveFileDialog.FileName);
                                dir = dir.Parent;
                                FileName = dir.FullName + "\\" + saveFileDialog.SafeFileName + "-" + CurrNbObj + "-" + pl.Nom + ".csv";

                                FileStream fs = File.OpenWrite(FileName);
                                StreamWriter sr = new StreamWriter(fs);
                                foreach (Coordonnees coord in pl.Coordonnees)
                                {
                                    sr.WriteLine(coord.Longitude + ";" + coord.Latitude + ";");
                                }

                                sr.Dispose();
                                fs.Dispose();

                                CurrNbObj++;
                                worker.ReportProgress(Convert.ToInt32((CurrNbObj / NbObj) * 100));

                            }


                        }


                    };

                    worker.ProgressChanged += delegate (object s, ProgressChangedEventArgs args)
                    {
                        ProgressBarProgression.Value = args.ProgressPercentage;
                        CurrentNumItem.Content = (int)CurrNbObj;
                        TotalNbItem.Content = (int)NbObj;
                    };

                    worker.RunWorkerCompleted += delegate (object s, RunWorkerCompletedEventArgs args)
                    {
                        this.Close();
                    };

                    worker.RunWorkerAsync();
                    this.ShowDialog();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            if (CurrNbObj >= NbObj)
            {
                e.Cancel = false;
            }
        }
    }
}

// String.empty