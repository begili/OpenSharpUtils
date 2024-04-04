using OpenSharpUtils.ModelGenerator.WPF.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSharpUtils.ModelGenerator.WPF.Classes
{
    public class PMMParameter : INotifyPropertyChanged
    {

        #region >> Fields <<

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

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

        #region ParameterId (NotificationProperty)

        private int _ParameterId;

        public int ParameterId
        {
            get { return _ParameterId; }
            set { if (value != _ParameterId) { _ParameterId = value; OnPropertyChanged("ParameterId"); } }
        }

        #endregion ParameterId (NotificationProperty)
        
        #region PMMType (NotificationProperty)

        private DBFetching.Type _PMMType = null;

        public DBFetching.Type PMMType
        {
            get { return _PMMType; }
            set { if (value != _PMMType) { DBFetching.Type oldValue = _PMMType; _PMMType = value; PMMTypeChanged(oldValue, value); OnPropertyChanged("PMMType"); } }
        }

        private void PMMTypeChanged(DBFetching.Type oldValue, DBFetching.Type newValue)
        {
            if (newValue != null)
            {
                TypeName = newValue.Name;
                CSType = newValue.Name.ToCSType();
            }
        }

        #endregion PMMType (NotificationProperty)
        
        #region TypeName (NotificationProperty)

        private string _TypeName;

        public string TypeName
        {
            get { return _TypeName; }
            set { if (value != _TypeName) { _TypeName = value; OnPropertyChanged("TypeName"); } }
        }

        #endregion TypeName (NotificationProperty)

        #region TypeLength (NotificationProperty)

        private short? _TypeLength;

        public short? TypeLength
        {
            get { return _TypeLength; }
            set { if (value != _TypeLength) { _TypeLength = value; OnPropertyChanged("TypeLength"); } }
        }

        #endregion TypeLength (NotificationProperty)

        #region CSType (NotificationProperty)

        private Type _CSType;

        public Type CSType
        {
            get { return _CSType; }
            set { if (value != _CSType) { _CSType = value; OnPropertyChanged("CSType"); } }
        }

        #endregion CSType (NotificationProperty)
        
        #endregion >> Properties <<

        #region >> CTOR <<

        #endregion >> CTOR <<

        #region >> Public Methods <<

        #endregion >> Public Methods <<

        #region >> Private Methods <<

        #endregion >> Private Methods <<

        #region >> Override Methods <<

        #endregion >> Override Methods <<

        #region >> Events <<

        #endregion >> Events <<

    }
}
