using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSharpUtils.ModelGenerator.WPF.DBFetching
{
    public class Schema : INotifyPropertyChanged
    {

        #region >> Fields <<

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion >> Fields <<

        #region >> Properties <<

        #region Name (Notification Property)
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { if (_Name != value) { _Name = value; OnPropertyChanged("Name"); } }
        }
        #endregion

        #region Schema_ID (Notification Property)
        private int _Schema_ID;

        public int Schema_ID
        {
            get { return _Schema_ID; }
            set { if (_Schema_ID != value) { _Schema_ID = value; OnPropertyChanged("Schema_ID"); } }
        }
        #endregion

        #region Principal_ID (Notification Property)
        private int _Principal_ID;

        public int Principal_ID
        {
            get { return _Principal_ID; }
            set { if (_Principal_ID != value) { _Principal_ID = value; OnPropertyChanged("Principal_ID"); } }
        }
        #endregion

        #endregion >> Properties <<

        #region >> CTOR <<

        #endregion >> CTOR <<

        #region >> Public Methods <<

        public static IEnumerable<Schema> LoadSchemasFromCatalog(SqlConnection con, Catalog catalog)
        {
            SqlCommand command = con.CreateCommand();
            command.CommandText = string.Format("SELECT name, schema_id, principal_id FROM {0}.sys.schemas", catalog.Name);
            SqlDataReader reader = command.ExecuteReader();
            IEnumerable<Schema> ret = ListFromReader(reader);
            reader.Close();
            return ret;
        }

        public static Schema FromReader(IDataReader reader)
        {
            Schema ret = new Schema();
            ret.Name = (string)reader["name"];
            ret.Schema_ID = (int)reader["schema_id"];
            ret.Principal_ID = (int)reader["principal_id"];
            return ret;
        }

        public static IEnumerable<Schema> ListFromReader(IDataReader reader)
        {
            List<Schema> ret = new List<Schema>();
            while (reader.Read())
            {
                ret.Add(FromReader(reader));
            }
            return ret;
        }

        #endregion >> Public Methods <<

        #region >> Private Methods <<

        #endregion >> Private Methods <<

        #region >> Override Methods <<

        #endregion >> Override Methods <<

        #region >> Events <<

        #endregion >> Events <<


    }
}
