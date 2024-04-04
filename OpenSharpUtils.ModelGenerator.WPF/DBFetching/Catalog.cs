using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSharpUtils.ModelGenerator.WPF.DBFetching
{
    public class Catalog : INotifyPropertyChanged
    {

        #region >> Fields <<

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion >> Fields <<

        #region >> Properties <<

        #region Name (NotificationProperty)

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { if (value != _Name) { _Name = value; OnPropertyChanged("Name"); } }
        }

        #endregion Name (NotificationProperty)

        #endregion >> Properties <<

        #region >> CTOR <<

        public Catalog(string name)
        {
            this.Name = name;
        }

        #endregion >> CTOR <<

        #region >> Public Methods <<

        public static IEnumerable<Catalog> LoadCatalogsFromDb(SqlConnection con)
        {
            SqlCommand command = con.CreateCommand();
            command.CommandText = "SELECT name FROM sys.databases";
            SqlDataReader reader = command.ExecuteReader();
            List<Catalog> catalogs = new List<Catalog>();
            while (reader.Read())
            {
                catalogs.Add(new Catalog((string)reader["name"]));
            }
            reader.Close();
            return catalogs;
        }

        #endregion >> Public Methods <<

        #region >> Private Methods <<

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion >> Private Methods <<

        #region >> Override Methods <<

        #endregion >> Override Methods <<

        #region >> Events <<

        #endregion >> Events <<

    }
}
