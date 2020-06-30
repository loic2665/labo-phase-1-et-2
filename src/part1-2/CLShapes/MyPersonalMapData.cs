using System;

using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CLShapes
{
    public class MyPersonalMapData
    {

        #region VAR MEMBRES

        private String _nom;
        private String _prenom;
        private String _email;
        private String _filename;

        private String _path;

        private ObservableCollection<ICartoObj> _obj;

        #endregion

        #region PROPRIETES

        public String Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        public String Prenom
        {
            get { return _prenom; }
            set { _prenom = value; }
        }
        public String Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public String Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public ObservableCollection<ICartoObj> Collection
        {
            get { return _obj; }
            set { _obj = value; }
        }

        #endregion


        #region CONSTRUCTEURS


        public MyPersonalMapData() : this("inconnu", "inconnu", "inconnu@domain.ext") { }
        public MyPersonalMapData(String nom, String prenom, String email)
        {
            Nom = nom;
            Prenom = prenom;
            Email = email;
            Collection = null;
            Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Filename = Nom + Prenom + ".sav";

        }

        #endregion




        #region METHODES

        public void Reset()
        {
            Collection.Clear();
        }

        public void Load()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (!Directory.Exists(Path))
            {
                throw new DirectoryNotFoundException();
            }

            using (Stream fStream = File.OpenRead(Path + "\\" +Filename))
            {
                Collection = (ObservableCollection<ICartoObj>)formatter.Deserialize(fStream);
            }




        }

        public void Save()
        {

            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }


            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream fStream = new FileStream(Path + "\\" + Filename, FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(fStream, Collection);
            }


        }

        #endregion



    }
}
