using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using CLShapes;


namespace Dossier1
{
    class Program
    {
        static void Main(string[] args)
        {

            List<CartoObj> cartoList = new List<CartoObj>();

            Coordonnees coord1 = new Coordonnees(2, 3);
            Coordonnees coord2 = new Coordonnees(3, 1);

            POI poi1 = new POI(1, 2, "HEPL");
            POI poi2 = new POI(4, 5, "ISIL");

            Polygon poly1 = new Polygon();
            Polygon poly2 = new Polygon();

            Polyline polyLine1 = new Polyline();
            Polyline polyLine2 = new Polyline();

            cartoList.Add(poly1);
            cartoList.Add(coord1);
            cartoList.Add(poi2);
            cartoList.Add(polyLine1);
            cartoList.Add(poly2);
            cartoList.Add(polyLine2);
            cartoList.Add(poi1);
            cartoList.Add(coord2);

            /*Console.WriteLine("\n\n\n**********\n Lister carto list\n***********\n\n");
            foreach (CartoObj carto in cartoList)
            {
                Console.WriteLine(carto);
            }

            Console.WriteLine("\n\n\n**********\n Lister carto list is iPointy ?\n***********\n\n");

            foreach (CartoObj carto in cartoList)
            {
                if (carto is IPointy) {
                    Console.WriteLine(carto + " is Pointy !");
                }
                else
                {
                    Console.WriteLine(carto + " is NOT Pointy !");
                }
            }


            List<Polyline> polyLinesList = new List<Polyline>{
                new Polyline(new List<Coordonnees>{
                    coord1, coord2
                }, 2, Colors.Red),
                
                new Polyline(),
                
                new Polyline(new List<Coordonnees>{
                    new Coordonnees(15, 35),
                    new Coordonnees(20, 23), 
                    new Coordonnees(27, 24)
                }, 2, Colors.Green),
                
                new Polyline(new List<Coordonnees>{
                    new Coordonnees(10, 12),
                    new Coordonnees(74, 32)
                }, 2, Colors.Red),
                
                new Polyline(new List<Coordonnees>{
                    new Coordonnees(11, 22), 
                    new Coordonnees(23, 23), 
                    new Coordonnees(55, 66),
                    new Coordonnees(44, 30),
                }, 2, Colors.White),


                new Polyline(),


            };

            Console.WriteLine("\n\n\n**********\n Polyline list\n***********\n\n");
            foreach (Polyline poly in polyLinesList) {
                Console.WriteLine(poly);
            }

            polyLinesList.Sort();

            Console.WriteLine("\n\n\n**********\n Polyline list ordered \n***********\n\n");

            foreach (Polyline poly in polyLinesList)
            {
                Console.WriteLine(poly);
            }

            Console.WriteLine("\n\n\n**********\n Polyline list boxComparer\n***********\n\n");

            MyPolylineBoundingBoxComparer boxComparer = new MyPolylineBoundingBoxComparer();

            polyLinesList.Sort(boxComparer);

            Console.WriteLine("\n\n\n**********\n Polyline calcul air\n***********\n\n");
            foreach (Polyline poly in polyLinesList)
            {
                Console.WriteLine(poly + " || Aire: " + poly.Bbox.CalculAir());
            }


            Console.WriteLine("\n\n\n**********\n Polyline list calcul longueur find simple\n***********\n\n");


            Console.WriteLine(polyLinesList.Find(x => x.CalculLongueur() == 0) + " FIND SIMPLE");

            Console.WriteLine("\n\n\n**********\n Polyline list calcul longueur find all\n***********\n\n");
            foreach (Polyline poly in polyLinesList.FindAll(x => x.CalculLongueur() == 0))
            {
                Console.WriteLine(poly + " FIND ALL");
            }


            Console.WriteLine("\n\n\n**********\n polyline is point close\n***********\n\n");

            Coordonnees c3 = new Coordonnees(10, 12);
            
            foreach(Polyline line in polyLinesList)
            {

                if (line.IsPointClose(c3, 3)) {
                    Console.WriteLine("Point close ({0} et {1}) !", c3, line);
                }
                else
                {
                    Console.WriteLine("Point PAS close ({0} et {1}) !", c3, line);
                }

            }



            Console.WriteLine("\n\n\n**********\n carto obj not ordered\n***********\n\n");

            foreach (CartoObj obj in cartoList)
            {
                Console.WriteLine(obj);
            }

            MyCartoObjComparer cartoObjComparer = new MyCartoObjComparer();

            cartoList.Sort(cartoObjComparer);

            Console.WriteLine("\n\n\n**********\n carto obj ordered\n***********\n\n");
            foreach (CartoObj obj in cartoList)
            {
                Console.WriteLine(obj);
            }

            Console.WriteLine("\n\n\n**********\n serialization\n***********\n\n");*/

            MyPersonalMapData MyData = new MyPersonalMapData("loic", "collette", "loic.collette@gmail.com");
            MyData.Collection = new ObservableCollection<ICartoObj>
            {
                poly1,
                poi2,
                polyLine1,
                poly2,
                polyLine2,
                poi1
            };

            MyData.Save();

            Console.ReadKey();


        }
    }
}
