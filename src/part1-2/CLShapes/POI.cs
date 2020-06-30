using System;



namespace CLShapes
{
    [Serializable]
    public class POI : Coordonnees, ICartoObj
    {


        #region VARIABLES MEMBRES

        private String _description;

        #endregion


        #region PROPRIETES

        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }





        #endregion


        #region CONSTRUCTEURS

        public POI() : this(50.620796, 5.581418, "HEPL") { }
        public POI(double longitude, double latitude, String description) : base(longitude, latitude)
        {
            Description = description;
        }



        #endregion

        #region METHODES

        public override string ToString()
        {
            //return base.ToString() + " / Desc: " + Description;
            return base.Id + ": " + Description;
        }

        public override void Draw()
        {
            Console.WriteLine(ToString());
        }

        #endregion


    }
}
