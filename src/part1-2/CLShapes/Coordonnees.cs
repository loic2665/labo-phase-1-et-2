using System;

using MathUtil;

namespace CLShapes
{
    [Serializable]
    public class Coordonnees : CartoObj
    {

        #region VARIABLES MEMBRES

        protected double _longitude;
        protected double _latitude;

        #endregion


        #region PROPRIETES

        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }
        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        #endregion


        #region CONSTRUCTEURS

        public Coordonnees() : this(0, 0) { }
        public Coordonnees(double longitude, double latitude) : base()
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        #endregion

        #region METHODES

        public override string ToString()
        {
            return base.ToString() + " / Long: " + Longitude + " / Lat: " + Latitude;
        }



        #endregion

        #region METHODES VIRTUELLES / ABSTRAITES

        public override bool IsPointClose(Coordonnees point, double precision)
        {

            double distance = Distance.Entre2Points(Latitude, Longitude, point.Latitude, point.Longitude);
            if (distance < precision)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
