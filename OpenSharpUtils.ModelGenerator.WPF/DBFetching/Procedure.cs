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
    public class Procedure : INotifyPropertyChanged
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

        #region Object_ID (Notification Property)
        private int _Object_ID;

        public int Object_ID
        {
            get { return _Object_ID; }
            set { if (_Object_ID != value) { _Object_ID = value; OnPropertyChanged("Object_ID"); } }
        }
        #endregion

        #region Schema_Id (NotificationProperty)

        private int _Schema_Id;

        public int Schema_Id
        {
            get { return _Schema_Id; }
            set { if (value != _Schema_Id) { _Schema_Id = value; OnPropertyChanged("Schema_Id"); } }
        }

        #endregion Schema_Id (NotificationProperty)

        #region CreationDate (NotificationProperty)

        private DateTime _CreationDate;

        public DateTime CreationDate
        {
            get { return _CreationDate; }
            set { if (value != _CreationDate) { _CreationDate = value; OnPropertyChanged("CreationDate"); } }
        }

        #endregion CreationDate (NotificationProperty)

        #region ModifyDate (NotificationProperty)

        private DateTime _ModifyDate;

        public DateTime ModifyDate
        {
            get { return _ModifyDate; }
            set { if (value != _ModifyDate) { _ModifyDate = value; OnPropertyChanged("ModifyDate"); } }
        }

        #endregion ModifyDate (NotificationProperty)

        #endregion >> Properties <<

        #region >> CTOR <<

        #endregion >> CTOR <<

        #region >> Public Methods <<

        public static IEnumerable<Procedure> LoadProceduresFromCatalog(SqlConnection con, Catalog catalog)
        {
            SqlCommand command = con.CreateCommand();
            command.CommandText = string.Format("SELECT object_id, name, schema_id, create_date, modify_date FROM {0}.sys.procedures", catalog.Name);
            SqlDataReader reader = command.ExecuteReader();
            IEnumerable<Procedure> ret = ListFromReader(reader);
            reader.Close();
            return ret;
        }

        public static Procedure FromReader(IDataReader reader)
        {
            Procedure ret = new Procedure();
            ret.Object_ID = (int)reader["object_id"];
            ret.Name = (string)reader["name"];
            ret.Schema_Id = Convert.ToInt32(reader["schema_id"]);
            ret.CreationDate = Convert.ToDateTime(reader["create_date"]);
            ret.ModifyDate = Convert.ToDateTime(reader["modify_date"]);
            return ret;
        }

        public static IEnumerable<Procedure> ListFromReader(IDataReader reader)
        {
            List<Procedure> ret = new List<Procedure>();
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
