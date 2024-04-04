using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenSharpUtils.ModelGenerator.WPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region >> Fields <<

        private const string APP_FOLDER_NAME = "PMM";
        private const string APP_FOLDER_FILE_NAME = "test.pmmproject";

        private readonly string folderPath, filePath;

        #endregion >> Fields <<

        #region >> Properties <<

        #endregion >> Properties <<

        #region >> CTOR <<

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MasterViewModel.Instance;
            string appFolderBasePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folderPath = string.Format("{0}\\{1}", appFolderBasePath, APP_FOLDER_NAME);
            filePath = string.Format("{0}\\{1}", folderPath, APP_FOLDER_FILE_NAME);
        }

        #endregion >> CTOR <<

        #region >> Public Methods <<

        #endregion >> Public Methods <<

        #region >> Private Methods <<

        #endregion >> Private Methods <<

        #region >> Override Methods <<

        #endregion >> Override Methods <<

        #region >> Events <<

        private void mnuClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void mnuNewProject_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuOpenProject_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void mnuProjectPreferences_Click(object sender, RoutedEventArgs e)
        {
            (new ProjectPreferencesWindow()).ShowDialog();
            await MasterViewModel.Instance.BuildupProject();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(folderPath) && File.Exists(filePath))
            {
                MasterViewModel.Instance.LoadFromFile(filePath);
            }
            await MasterViewModel.Instance.BuildupProject();
        }

        private void mnuSaveProject_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            MasterViewModel.Instance.SaveToFile(filePath);
        }

        #endregion >> Events <<
    }
}
