using System.Collections.Generic;

namespace CLShapes
{
    public class MyPolylineBoundingBoxComparer : IComparer<Polyline>
    {
        public int Compare(Polyline x, Polyline y)
        {
            return x.Bbox.CalculAir().CompareTo(y.Bbox.CalculAir());
        }
    }
}
