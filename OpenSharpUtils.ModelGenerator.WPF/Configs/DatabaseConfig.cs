using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSharpUtils.ModelGenerator.WPF.Configs
{
    public class DatabaseConfig : INotifyPropertyChanged
    {

        #region >> Fields <<

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion >> Fields <<

        #region >> Properties <<
        
        #region ServerName (NotificationProperty)

        private string _ServerName;

        public string ServerName
        {
            get { return _ServerName; }
            set { if (value != _ServerName) { _ServerName = value; OnPropertyChanged("ServerName"); } }
        }

        #endregion ServerName (NotificationProperty)
        
        #region UseWindowsAuthentication (NotificationProperty)

        private bool _UseWindowsAuthentication;

        public bool UseWindowsAuthentication
        {
            get { return _UseWindowsAuthentication; }
            set { if (value != _UseWindowsAuthentication) { _UseWindowsAuthentication = value; OnPropertyChanged("UseWindowsAuthentication"); } }
        }

        #endregion UseWindowsAuthentication (NotificationProperty)
        
        #region Username (NotificationProperty)

        private string _Username;

        public string Username
        {
            get { return _Username; }
            set { if (value != _Username) { _Username = value; OnPropertyChanged("Username"); } }
        }

        #endregion Username (NotificationProperty)
        
        #region Password (NotificationProperty)

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { if (value != _Password) { _Password = value; OnPropertyChanged("Password"); } }
        }

        #endregion Password (NotificationProperty)
        
        #endregion >> Properties <<

        #region >> CTOR <<

        #endregion >> CTOR <<

        #region >> Public Methods <<

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
