using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenSharpUtils.ModelGenerator.WPF.Configs
{
    public class PMMProject : INotifyPropertyChanged
    {
        #region >> Fields <<

        public const string FILE_EXT = ".pmmproject";
        public const string FILE_EXT_FILTER = "PMM-Project (*.pmmproject) | *.pmmproject";
        private const string XML_BASE_ELEMENT_TAG = "config";

        private const string XML_COMMON_ELEMENT_TAG = "common";
        private const string XML_SERVER_NAME_ATTRIBUTE = "servername";
        private const string XML_USE_WINDOWS_AUTH_ATTRIBUTE = "winauth";
        private const string XML_USERNAME_ATTRIBUTE = "username";
        private const string XML_PASSWORD_ATTRIBUTE = "password";
        private const string XML_CATALOGS_ATTRIBUTE = "catalogs";

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion >> Fields <<

        #region >> Properties <<

        public string ServerName { get; set; }

        public bool UseWindownsAuthentication { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        #endregion >> Properties <<

        #region >> CTOR <<

        #endregion >> CTOR <<

        #region >> Public Methods <<

        public static PMMProject ParseFromFile(string filePath)
        {
            try
            {
                PMMProject ret = new PMMProject();
                XDocument doc = XDocument.Load(filePath);
                XElement rootElement = doc.Element(XML_BASE_ELEMENT_TAG);

                XElement commonElement = rootElement.Element(XML_COMMON_ELEMENT_TAG);
                ret.ServerName = commonElement.Attribute(XML_SERVER_NAME_ATTRIBUTE).Value;
                ret.UseWindownsAuthentication = bool.Parse(commonElement.Attribute(XML_USE_WINDOWS_AUTH_ATTRIBUTE).Value);
                ret.UserName = commonElement.Attribute(XML_USERNAME_ATTRIBUTE).Value;
            }
            catch (Exception e)
            {
                //LOADING FAIL
            }
            return null;
        }

        public void SaveToFile(string filePath)
        {
            XDocument store = new XDocument();
            XElement rootElement = new XElement(XML_BASE_ELEMENT_TAG);
            XElement commonElement = new XElement(XML_COMMON_ELEMENT_TAG);
            commonElement.Add(new XAttribute(XML_SERVER_NAME_ATTRIBUTE, ServerName));
            commonElement.Add(new XAttribute(XML_USE_WINDOWS_AUTH_ATTRIBUTE, UseWindownsAuthentication));
            commonElement.Add(new XAttribute(XML_USERNAME_ATTRIBUTE, UserName ?? ""));
            rootElement.Add(commonElement);
            store.Add(rootElement);
            store.Save(filePath);
        }

        #region Catalogs (NotificationProperty)

        private IEnumerable<CatalogConfig> _Catalogs;

        public IEnumerable<CatalogConfig> Catalogs
        {
            get { return _Catalogs; }
            set { if (value != _Catalogs) { _Catalogs = value; OnPropertyChanged("Catalogs"); } }
        }

        #endregion Catalogs (NotificationProperty)

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
