using OpenSharpUtils.ModelGenerator.WPF.DBFetching;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OpenSharpUtils.ModelGenerator.WPF
{
    /// <summary>
    /// Interaktionslogik für ProjectPreferencesWindow.xaml
    /// </summary>
    public partial class ProjectPreferencesWindow : Window, INotifyPropertyChanged
    {

        #region >> Fields <<

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion >> Fields <<

        #region >> Properties <<

        #region VM (NotificationProperty)

        private ProjectPreferencesViewModel _VM;

        public ProjectPreferencesViewModel VM
        {
            get { return _VM; }
            set { if (value != _VM) { _VM = value; OnPropertyChanged("VM"); } }
        }

        #endregion VM (NotificationProperty)

        #endregion >> Properties <<

        #region >> CTOR <<

        public ProjectPreferencesWindow()
        {
            InitializeComponent();
            VM = new ProjectPreferencesViewModel();
            this.DataContext = VM;
        }

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

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            VM.ApplySettings();
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            VM.ApplySettings();
        }

        private async void btnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            VM.Password = txtPassword.Password;
            await VM.TestConnectionAsync();

            //select * from information_schema.parameters
            //where specific_name = 'your_procedure_name'
        }

        private void btnSelectDb_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void lbServers_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VM.CurrentProject = MasterViewModel.Instance.CurrentProject;
        }

        #endregion >> Events <<
    }
}
