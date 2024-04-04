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
    public class Type : INotifyPropertyChanged
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

        #region SystemTypeId (Notification Property)
        private byte _SystemTypeId;

        public byte SystemTypeId
        {
            get { return _SystemTypeId; }
            set { if (_SystemTypeId != value) { _SystemTypeId = value; OnPropertyChanged("SystemTypeId"); } }
        }
        #endregion

        #region UserTypeId (Notification Property)
        private int _UserTypeId;

        public int UserTypeId
        {
            get { return _UserTypeId; }
            set { if (_UserTypeId != value) { _UserTypeId = value; OnPropertyChanged("UserTypeId"); } }
        }
        #endregion

        #region SchemaId (Notification Property)
        private int _SchemaId;

        public int SchemaId
        {
            get { return _SchemaId; }
            set { if (_SchemaId != value) { _SchemaId = value; OnPropertyChanged("SchemaId"); } }
        }
        #endregion

        #endregion >> Properties <<

        #region >> CTOR <<

        #endregion >> CTOR <<

        #region >> Public Methods <<

        public static IEnumerable<Type> LoadTypesFromCatalog(SqlConnection con, Catalog catalog)
        {
            SqlCommand command = con.CreateCommand();
            command.CommandText = string.Format("SELECT name, system_type_id, user_type_id, schema_id FROM {0}.sys.types", catalog.Name);
            SqlDataReader reader = command.ExecuteReader();
            IEnumerable<Type> ret = ListFromReader(reader);
            reader.Close();
            return ret;
        }

        public static Type FromReader(IDataReader reader)
        {
            Type ret = new Type();
            ret.Name = (string)reader["name"];
            ret.SystemTypeId = (byte)reader["system_type_id"];
            ret.UserTypeId = (int)reader["user_type_id"];
            ret.SchemaId = (int)reader["schema_id"];
            return ret;
        }

        public static IEnumerable<Type> ListFromReader(IDataReader reader)
        {
            List<Type> ret = new List<Type>();
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
