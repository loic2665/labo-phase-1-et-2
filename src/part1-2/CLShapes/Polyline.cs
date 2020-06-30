using System;
using System.Collections.Generic;


using System.Windows.Media;

using MathUtil;


namespace CLShapes
{
    [Serializable]
    public class Polyline : CartoObj, IPointy, IComparable<Polyline>, IEquatable<Polyline>, ICartoObj
    {
        #region VARIABLES MEMBRES

        private List<Coordonnees> _coord;
        private String _nom;
        private int _epaisseur;
        private BoundingBox _bbox;
        [NonSerialized]
        private Color _couleur;

        #endregion


        #region PROPRIETES

        public Color Couleur
        {
            get { return _couleur; }
            set { _couleur = value; }
        }

        public List<Coordonnees> Coordonnees
        {
            get { return _coord; }
            set { _coord = value; }
        }

        public String Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        public int Epaisseur
        {
            get { return _epaisseur; }
            set { _epaisseur = value; }
        }

        public BoundingBox Bbox
        {
            get { return _bbox; }
            set { _bbox = value; }
        }

        public int Nbpoints => Coordonnees.Count;

        #endregion


        #region CONSTRUCTEURS



        public Polyline() : this(new List<Coordonnees> { new Coordonnees(0, 0) }, 5, Colors.Blue, "") { }

        public Polyline(List<Coordonnees> coord, int epaisseur, Color couleur, String nom) : base()
        {
            Coordonnees = coord;
            Epaisseur = epaisseur;
            Nom = nom;
            Couleur = couleur;
            Bbox = new BoundingBox();
            Bbox.InitBox(Coordonnees);

        }

        #endregion

        #region METHODES

        public override string ToString()
        {
            //return base.ToString() + " / Epaisseur :" + Epaisseur + " / Couleur :" + Couleur + " / Long: " + CalculLongueur() + " / NbPoint: " + Nbpoints;
            return base.Id + ": " + this.Nom;
        }

        public override void Draw()
        {
            Console.WriteLine(base.ToString());
        }

        public double CalculLongueur()
        {

            double taille = 0;

            for (int i = 0; i < Coordonnees.Count - 1; i++)
            {

                taille += Distance.Entre2Points(Coordonnees[i].Longitude, Coordonnees[i].Latitude, Coordonnees[i + 1].Longitude, Coordonnees[i].Latitude);

            }

            return taille;

        }

        public int CompareTo(Polyline other)
        {
            return CalculLongueur().CompareTo(other.CalculLongueur());
        }

        public bool Equals(Polyline other)
        {
            return CalculLongueur().Equals(other.CalculLongueur());
        }






        #endregion

        #region METHODES VIRTUELLES / ABSTRAITES





        public override bool IsPointClose(Coordonnees point, double precision)
        {
            if (Coordonnees.Count < 2)
            {
                return false;
            }


            // créer bound inbox > en drhosr = return false;

            for (int i = 0; i < Coordonnees.Count - 1; i++)
            {
                double distance;
                distance = Distance.EntrePointEtLigne(point.Longitude, point.Latitude, Coordonnees[i].Longitude, Coordonnees[i].Latitude, Coordonnees[i + 1].Longitude, Coordonnees[i + 1].Latitude);
                if (distance < precision)
                {
                    return true;
                }
            }

            return false;

        }

        #endregion
    }
}
