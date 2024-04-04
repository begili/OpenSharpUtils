using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSharpUtils.ModelGenerator.WPF.Helper
{
    public class CheckboxListItemLogic<T> : INotifyPropertyChanged
    {

        #region >> Fields <<


        public event PropertyChangedEventHandler PropertyChanged;



        #endregion >> Fields <<

        #region >> Properties <<


        #region Content (NotificationProperty)

        private T _Content;

        public T Content
        {
            get { return _Content; }
            set { if (!value.Equals(_Content)) { _Content = value; OnPropertyChanged("Content"); } }
        }

        #endregion Content (NotificationProperty)


        #region IsSelected (NotificationProperty)

        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set { if (value != _IsSelected) { _IsSelected = value; OnPropertyChanged("IsSelected"); } }
        }

        #endregion IsSelected (NotificationProperty)




        #endregion >> Properties <<

        #region >> CTOR <<

        public CheckboxListItemLogic(T content)
        {
            this.Content = content;
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

        #endregion >> Events <<

    }
}
