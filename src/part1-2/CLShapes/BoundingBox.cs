using System;
using System.Collections.Generic;
using System.Linq;

using MathUtil;

namespace CLShapes
{
    [Serializable]
    public class BoundingBox : IComparable<BoundingBox>
    {
        #region VARIABLES MEMBRES

        private Coordonnees _min;
        private Coordonnees _max;

        #endregion

        #region PROPRIETES

        public Coordonnees Min
        {
            set { _min = value; }
            get { return _min; }
        }
        public Coordonnees Max
        {
            set { _max = value; }
            get { return _max; }
        }

        #endregion

        #region CONSTRUCTEURS

        public BoundingBox(Coordonnees c1, Coordonnees c2)
        {
            if (c1 == null)
            {
                Min = new Coordonnees();
            }
            else
            {
                Min = c1;
            }


            if (c2 == null)
            {
                Max = new Coordonnees();
            }
            else
            {
                Max = c2;
            }


        }

        public BoundingBox() : this(new Coordonnees(0, 0), new Coordonnees(0, 0)) { }




        #endregion

        #region METHODES

        public void InitBox(List<Coordonnees> Liste)
        {
            if (Liste.Count() == 0)
            {
                return;
            }

            double bBx1 = Liste[0].Latitude;
            double bBy1 = Liste[0].Longitude;

            double bBx2 = Liste[0].Latitude;
            double bBy2 = Liste[0].Longitude;

            // calcul bounding box

            foreach (Coordonnees coord in Liste)
            {
                if (coord.Longitude < bBx1)
                {
                    bBx1 = coord.Longitude;
                }
                if (coord.Longitude > bBx2)
                {
                    bBx2 = coord.Longitude;
                }
                if (coord.Latitude < bBy1)
                {
                    bBy1 = coord.Latitude;
                }
                if (coord.Latitude > bBy2)
                {
                    bBy2 = coord.Latitude;
                }
            }

            Min = new Coordonnees(bBx1, bBy1);
            Max = new Coordonnees(bBx2, bBy2);



        }


        public double CalculAir()
        {
            double air;
            air = Distance.Entre2Points(Max.Longitude, 0, Min.Longitude, 0) * Distance.Entre2Points(0, Max.Latitude, 0, Min.Latitude);
            return air;
        }

        public int CompareTo(BoundingBox other)
        {
            return CalculAir().CompareTo(other.CalculAir());
        }


        #endregion

        #region METHODES VIRTUELLES / ABSTRAITES
        #endregion

    }
}
