using System;

namespace MathUtil
{
    public static class Distance
    {
        public static double Entre2Points(double x1, double y1, double x2, double y2)
        {

            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));

        }


        public static double EntrePointEtLigne(double x, double y, double x1, double y1, double x2, double y2)
        {


            // formule d'internet

            double A = x - x1;
            double B = y - y1;
            double C = x2 - x1;
            double D = y2 - y1;

            double dot = A * C + B * D;

            double len_sq = C * C + D * D;
            double param = -1;

            if (len_sq != 0)
                param = dot / len_sq;

            double xx, yy;

            if (param < 0)
            {
                xx = x1;
                yy = y1;
            }
            else
            if (param > 1)
            {
                xx = x2;
                yy = y2;
            }
            else
            {
                xx = x1 + param * C;
                yy = y1 + param * D;
            }

            double dx = x - xx;
            double dy = y - yy;

            return Math.Sqrt(dx * dx + dy * dy);
        

    }



    }
}
