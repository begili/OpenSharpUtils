using OpenSharpUtils.ModelGenerator.WPF.Classes;
using OpenSharpUtils.ModelGenerator.WPF.Configs;
using OpenSharpUtils.ModelGenerator.WPF.DBFetching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBFType = OpenSharpUtils.ModelGenerator.WPF.DBFetching.Type;

namespace OpenSharpUtils.ModelGenerator.WPF
{
    public class MasterViewModel : INotifyPropertyChanged
    {

        #region >> Fields <<

        public event PropertyChangedEventHandler PropertyChanged;

        private SqlConnection activeConnection;

        private static MasterViewModel _Instance;

        public static MasterViewModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MasterViewModel();
                }
                return _Instance;
            }

        }

        #endregion >> Fields <<

        #region >> Properties <<


        #region CurrentProject (NotificationProperty)

        private PMMProject _CurrentProject;

        public PMMProject CurrentProject
        {
            get { return _CurrentProject; }
            set { if (value != _CurrentProject) { _CurrentProject = value; OnPropertyChanged("CurrentProject"); } }
        }

        #endregion CurrentProject (NotificationProperty)


        #region AvailableProcedures (NotificationProperty)

        private ObservableCollection<PMMProcedure> _AvailableProcedures;

        public ObservableCollection<PMMProcedure> AvailableProcedures
        {
            get { return _AvailableProcedures; }
            set { if (value != _AvailableProcedures) { _AvailableProcedures = value; OnPropertyChanged("AvailableProcedures"); } }
        }

        #endregion AvailableProcedures (NotificationProperty)



        #endregion >> Properties <<

        #region >> CTOR <<

        private MasterViewModel()
        {
            CurrentProject = new PMMProject();
        }

        #endregion >> CTOR <<

        #region >> Public Methods <<

        public async Task BuildupProject()
        {
            if (CurrentProject != null)
            {
                ConnectionTestResult res = await Task.Run(() =>
                {
                    try
                    {
                        SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
                        csb.DataSource = CurrentProject.ServerName;
                        csb.IntegratedSecurity = CurrentProject.UseWindownsAuthentication;
                        if (!CurrentProject.UseWindownsAuthentication)
                        {
                            csb.UserID = CurrentProject.UserName;
                            csb.Password = CurrentProject.Password;
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
                    activeConnection = res.Connection;
                    HashSet<string> selectedCatalognames = new HashSet<string>(CurrentProject.Catalogs.Select(it => it.Name));
                    IEnumerable<Catalog> catalogs = Catalog.LoadCatalogsFromDb(activeConnection).Where(it => selectedCatalognames.Contains(it.Name));
                    Dictionary<Catalog, Dictionary<int, Schema>> schemas = new Dictionary<Catalog, Dictionary<int, Schema>>();
                    Dictionary<Catalog, Dictionary<int, Procedure>> procedures = new Dictionary<Catalog, Dictionary<int, Procedure>>();
                    Dictionary<Catalog, Dictionary<int, Dictionary<int, PMMParameter>>> parameters = new Dictionary<Catalog, Dictionary<int, Dictionary<int, PMMParameter>>>();
                    Dictionary<Catalog, Dictionary<int, DBFType>> types = new Dictionary<Catalog, Dictionary<int, DBFType>>();
                    ObservableCollection<PMMProcedure> pmmProcedures = new ObservableCollection<PMMProcedure>();
                    foreach (var item in catalogs)
                    {
                        schemas.Add(item, Schema.LoadSchemasFromCatalog(res.Connection, item).ToDictionary(it => it.Schema_ID, it => it));
                        types.Add(item, DBFType.LoadTypesFromCatalog(res.Connection, item).ToDictionary(it => it.UserTypeId, it => it));
                        Dictionary<int, Dictionary<int, PMMParameter>> pardict = new Dictionary<int, Dictionary<int, PMMParameter>>();
                        foreach (var parameter in Parameter.LoadParametrsFromCatalog(res.Connection, item))
                        {
                            if (!pardict.ContainsKey(parameter.ObjectId))
                            {
                                pardict.Add(parameter.ObjectId, new Dictionary<int, PMMParameter>());
                            }
                            pardict[parameter.ObjectId].Add(parameter.ParameterId, new PMMParameter() { Name = parameter.Name, PMMType = types[item][parameter.UserTypeId], ParameterId = parameter.ParameterId });
                        }
                        parameters.Add(item, pardict);

                        procedures.Add(item, Procedure.LoadProceduresFromCatalog(res.Connection, item).ToDictionary(it => it.Object_ID, it => it));
                        foreach (var pr in procedures[item])
                        {
                            pmmProcedures.Add(new PMMProcedure() { FullName = string.Format("{0}.{1}.{2}", item.Name, schemas[item][pr.Value.Schema_Id].Name, pr.Value.Name), Parameters = parameters[item].ContainsKey(pr.Value.Object_ID) ? new ObservableCollection<PMMParameter>(parameters[item][pr.Value.Object_ID].Select(it => it.Value)) : null });
                        }
                    }
                    AvailableProcedures = pmmProcedures;
                }
                else
                {
                    //FAILURE
                }
            }
        }

        public void SaveToFile(string path)
        {
            CurrentProject.SaveToFile(path);
        }

        public void LoadFromFile(string path)
        {
            CurrentProject = PMMProject.ParseFromFile(path);
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
}
