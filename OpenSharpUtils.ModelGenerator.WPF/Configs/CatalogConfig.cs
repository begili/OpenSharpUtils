using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSharpUtils.ModelGenerator.WPF.Configs
{
    public class CatalogConfig : INotifyPropertyChanged
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
