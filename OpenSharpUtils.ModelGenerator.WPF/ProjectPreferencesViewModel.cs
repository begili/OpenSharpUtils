using OpenSharpUtils.ModelGenerator.WPF.Configs;
using OpenSharpUtils.ModelGenerator.WPF.DBFetching;
using OpenSharpUtils.ModelGenerator.WPF.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSharpUtils.ModelGenerator.WPF
{
    public class ProjectPreferencesViewModel : INotifyPropertyChanged
    {

        #region >> Fields <<

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion >> Fields <<

        #region >> Properties <<


        #region Server (NotificationProperty)

        private string _Server = @"DESKTOP-BJIFP3J\LOCALDB";

        public string Server
        {
            get { return _Server; }
            set { if (value != _Server) { _Server = value; OnPropertyChanged("Server"); } }
        }

        #endregion Server (NotificationProperty)


        #region UseWindowsAuth (NotificationProperty)

        private bool _UseWindowsAuth = true;

        public bool UseWindowsAuth
        {
            get { return _UseWindowsAuth; }
            set { if (value != _UseWindowsAuth) { bool oldValue = _UseWindowsAuth; _UseWindowsAuth = value; UseWindowsAuthChanged(oldValue, value); OnPropertyChanged("UseWindowsAuth"); } }
        }

        private void UseWindowsAuthChanged(bool oldValue, bool newValue)
        {
            IsUsernameAndPasswordEnabled = !newValue;
        }

        #endregion UseWindowsAuth (NotificationProperty)

        #region IsUsernameAndPasswordEnabled (NotificationProperty)

        private bool _IsUsernameAndPasswordEnabled = false;

        public bool IsUsernameAndPasswordEnabled
        {
            get { return _IsUsernameAndPasswordEnabled; }
            set { if (value != _IsUsernameAndPasswordEnabled) { _IsUsernameAndPasswordEnabled = value; OnPropertyChanged("IsUsernameAndPasswordEnabled"); } }
        }

        #endregion IsUsernameAndPasswordEnabled (NotificationProperty)



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


        #region ConnectionState (NotificationProperty)

        private string _ConnectionState;

        public string ConnectionState
        {
            get { return _ConnectionState; }
            set { if (value != _ConnectionState) { _ConnectionState = value; OnPropertyChanged("ConnectionState"); } }
        }

        #endregion ConnectionState (NotificationProperty)



        #region Catalogs (NotificationProperty)

        private ObservableCollection<CheckboxListItemLogic<Catalog>> _Catalogs;

        public ObservableCollection<CheckboxListItemLogic<Catalog>> Catalogs
        {
            get { return _Catalogs; }
            set { if (value != _Catalogs) { _Catalogs = value; OnPropertyChanged("Catalogs"); } }
        }

        #endregion Catalogs (NotificationProperty)


        #region CurrentProject (NotificationProperty)

        private PMMProject _CurrentProject = null;

        public PMMProject CurrentProject
        {
            get { return _CurrentProject; }
            set { if (value != _CurrentProject) { PMMProject oldValue = _CurrentProject; _CurrentProject = value; CurrentProjectChanged(oldValue, value); OnPropertyChanged("CurrentProject"); } }
        }

        private void CurrentProjectChanged(PMMProject oldValue, PMMProject newValue)
        {
            if (newValue != null)
            {
                Server = newValue.ServerName;
                if (Server == null)
                {
                    Server = @"DESKTOP-BJIFP3J\LOCALDB";
                }
                UseWindowsAuth = newValue.UseWindownsAuthentication;
                Username = newValue.UserName;
                Password = newValue.Password;
            }
        }

        #endregion CurrentProject (NotificationProperty)


        #endregion >> Properties <<

        #region >> CTOR <<

        #endregion >> CTOR <<

        #region >> Public Methods <<

        public async Task TestConnectionAsync()
        {
            ConnectionTestResult res = await Task.Run(() =>
            {
                try
                {
                    SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
                    csb.DataSource = Server;
                    csb.IntegratedSecurity = UseWindowsAuth;
                    if (!UseWindowsAuth)
                    {
                        csb.UserID = Username;
                        csb.Password = Password;
                    }
                    SqlConnection con = new SqlConnection(csb.ToString());
                    con.Open();
                    return new ConnectionTestResult(csb, con);
                }
                catch (Exception e)
                {
                    return new ConnectionTestResult(e);
                }
            });
            if (res.Successful)
            {
                ConnectionState = "Connected";
                Catalogs = new ObservableCollection<CheckboxListItemLogic<Catalog>>((await Task.Run(() =>
                {
                    try
                    {
                        return Catalog.LoadCatalogsFromDb(res.Connection);
                    }
                    catch (Exception e)
                    {
                        return new Catalog[0];
                    }
                })).Select(it => new CheckboxListItemLogic<Catalog>(it)));
                res.Connection.Close();
            }
            else
            {
                ConnectionState = "Connection failed";
            }
        }

        public void ApplySettings()
        {
            CurrentProject.ServerName = Server;
            CurrentProject.UseWindownsAuthentication = UseWindowsAuth;
            CurrentProject.UserName = Username;
            CurrentProject.Password = Password;
            CurrentProject.Catalogs = Catalogs.Where(it => it.IsSelected).Select(it => new CatalogConfig() { Name = it.Content.Name });
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

    public class ConnectionTestResult
    {
        public bool Successful { get; private set; }

        public string ErrorMessage { get; private set; }

        public string ConnectionString { get; private set; }

        public SqlConnection Connection { get; private set; }

        public ConnectionTestResult(Exception e)
        {
            Successful = false;
            ErrorMessage = e.Message;
        }

        public ConnectionTestResult(SqlConnectionStringBuilder sb, SqlConnection connection)
        {
            Successful = true;
            ConnectionString = sb.ToString();
            this.Connection = connection;
        }
    }
}
