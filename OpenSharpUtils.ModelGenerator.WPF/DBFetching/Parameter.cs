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
    public class Parameter : INotifyPropertyChanged
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

        #region ObjectId (Notification Property)
        private int _ObjectId;

        public int ObjectId
        {
            get { return _ObjectId; }
            set { if (_ObjectId != value) { _ObjectId = value; OnPropertyChanged("ObjectId"); } }
        }
        #endregion

        #region Name (Notification Property)
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { if (_Name != value) { _Name = value; OnPropertyChanged("Name"); } }
        }
        #endregion

        #region ParameterId (Notification Property)
        private int _ParameterId;

        public int ParameterId
        {
            get { return _ParameterId; }
            set { if (_ParameterId != value) { _ParameterId = value; OnPropertyChanged("ParameterId"); } }
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

        #region MaxLength (Notification Property)
        private short _MaxLength;

        public short MaxLength
        {
            get { return _MaxLength; }
            set { if (_MaxLength != value) { _MaxLength = value; OnPropertyChanged("MaxLength"); } }
        }
        #endregion

        #endregion >> Properties <<

        #region >> CTOR <<

        #endregion >> CTOR <<

        #region >> Public Methods <<

        public static IEnumerable<Parameter> LoadParametrsFromCatalog(SqlConnection con, Catalog catalog)
        {
            SqlCommand command = con.CreateCommand();
            command.CommandText = string.Format("SELECT object_id, name, parameter_id, system_type_id, user_type_id, max_length FROM {0}.sys.parameters", catalog.Name);
            SqlDataReader reader = command.ExecuteReader();
            IEnumerable<Parameter> ret = ListFromReader(reader);
            reader.Close();
            return ret;
        }

        public static Parameter FromReader(IDataReader reader)
        {
            Parameter ret = new Parameter();
            ret.ObjectId = (int)reader["object_id"];
            ret.Name = (string)reader["name"];
            ret.ParameterId = (int)reader["parameter_id"];
            ret.SystemTypeId = (byte)reader["system_type_id"];
            ret.UserTypeId = (int)reader["user_type_id"];
            ret.MaxLength = (short)reader["max_length"];
            return ret;
        }

        public static IEnumerable<Parameter> ListFromReader(IDataReader reader)
        {
            List<Parameter> ret = new List<Parameter>();
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
