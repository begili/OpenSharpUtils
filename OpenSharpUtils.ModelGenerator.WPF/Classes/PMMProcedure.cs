using OpenSharpUtils.ModelGenerator.WPF.DBFetching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSharpUtils.ModelGenerator.WPF.Classes
{
    public class PMMProcedure : INotifyPropertyChanged
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

        #region FullName (NotificationProperty)

        private string _FullName;

        public string FullName
        {
            get { return _FullName; }
            set { if (value != _FullName) { _FullName = value; OnPropertyChanged("FullName"); } }
        }

        #endregion FullName (NotificationProperty)

        #region ParameterDisplayString (NotificationProperty)

        private string _ParameterDisplayString = "No Parameters";

        public string ParameterDisplayString
        {
            get { return _ParameterDisplayString; }
            set { if (value != _ParameterDisplayString) { _ParameterDisplayString = value; OnPropertyChanged("ParameterDisplayString"); } }
        }

        #endregion ParameterDisplayString (NotificationProperty)


        #region Parameters (NotificationProperty)

        private ObservableCollection<PMMParameter> _Parameters = null;

        public ObservableCollection<PMMParameter> Parameters
        {
            get { return _Parameters; }
            set { if (value != _Parameters) { ObservableCollection<PMMParameter> oldValue = _Parameters; _Parameters = value; ParametersChanged(oldValue, value); OnPropertyChanged("Parameters"); } }
        }

        private void ParametersChanged(ObservableCollection<PMMParameter> oldValue, ObservableCollection<PMMParameter> newValue)
        {
            if (oldValue != null)
            {
                oldValue.CollectionChanged -= Parameters_CollectionChanged;
            }
            if (newValue != null)
            {
                newValue.CollectionChanged += Parameters_CollectionChanged;
            }
            CreateParameterString();
        }

        private void Parameters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CreateParameterString();
        }

        private void CreateParameterString()
        {
            if (Parameters == null || !Parameters.Any())
            {
                ParameterDisplayString = "No Parameters";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var par in Parameters.OrderBy(it => it.ParameterId))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(par.Name);
                    sb.Append("(");
                    sb.Append(par.TypeName);
                    sb.Append(")");
                }
                ParameterDisplayString = sb.ToString();
            }
        }

        #endregion Parameters (NotificationProperty)


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
