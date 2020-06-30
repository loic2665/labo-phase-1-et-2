using System;
using System.Collections.Generic;

using System.Windows.Media;

using MathUtil;

namespace CLShapes
{
    [Serializable]
    public class Polygon : CartoObj, IIsPointClose, IPointy, ICartoObj
    {
        #region VARIABLES MEMBRES

        private List<Coordonnees> _coord;
        private double _opacity;
        private BoundingBox _bbox;

        [NonSerialized]
        private Color _fillColor;
        [NonSerialized]
        private Color _borderColor;

        #endregion


        #region PROPRIETES

        public List<Coordonnees> Coordonnees
        {
            get { return _coord; }
            set { _coord = value; }
        }
        public Color FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; }
        }

        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        public double Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }

        public BoundingBox Bbox
        {
            get { return _bbox; }
            set { _bbox = value; }
        }

        public int Nbpoints
        {
            get
            {
                return Coordonnees.Count;
                // retourne le nb de pts en passant par l'interface qui me retourne ne nb de points
            }
        }

        #endregion


        #region CONSTRUCTEURS


        public Polygon() : this(new List<Coordonnees> { new Coordonnees(0, 0) }, Colors.Blue, Colors.Red, 1.0) { }

        public Polygon(List<Coordonnees> coord, Color fillColor, Color borderColor, double opacityLevel) : base()
        {
            Coordonnees = coord;
            FillColor = fillColor;
            BorderColor = borderColor;
            Bbox = new BoundingBox();
            Bbox.InitBox(Coordonnees);

            if (opacityLevel > 1)
            {
                Opacity = 1.0;
            }
            else if (opacityLevel < 0.0)
            {
                Opacity = 0.0;
            }
            else
            {
                Opacity = opacityLevel;
            }

            Bbox = new BoundingBox();

        }

        #endregion

        #region METHODES

        public override string ToString()
        {
            return base.ToString() + "/ Coord: " + Coordonnees + " / FillColor: " + FillColor + " / BorderColor: " + BorderColor + " / NbPoint: " + Nbpoints;
        }

        public override void Draw()
        {
            Console.WriteLine(base.ToString());
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
                distance = Distance.EntrePointEtLigne(point.Latitude, point.Longitude, Coordonnees[i].Longitude, Coordonnees[i].Latitude, Coordonnees[i + 1].Longitude, Coordonnees[i + 1].Longitude);
                if (distance < precision)
                {
                    return true;
                }
            }

            return false;

            #endregion
        }


    }
}
