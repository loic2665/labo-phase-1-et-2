using System.Collections.Generic;

namespace CLShapes
{
    public class MyCartoObjComparer : IComparer<CartoObj>
    {

        public int Compare(CartoObj x, CartoObj y)
        {
            if (x is IPointy && y is IPointy)
            {
                IPointy x1 = x as IPointy;
                IPointy y1 = y as IPointy;
                return x1.Nbpoints.CompareTo(y1.Nbpoints);
            }
            else
            {
                if (x is IPointy)
                {
                    return 1; // fonctionne comme strcmp (si il est avant c'est -1 y est IPointy) sinon, le contraire
                }
                else
                {
                    return -1;
                }
            }
        }

    }
}
