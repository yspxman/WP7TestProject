using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.ComponentModel;
using System.Windows.Controls.Primitives;



namespace AppTest.Map
{

    public partial class Page1 : INotifyPropertyChanged
    {
        private PushpinCatalog _pushpinCatalog;
        public Page1()
        {
            InitializeComponent();
            DataContext = this;

            _pushpinCatalog = new PushpinCatalog();

           // ListBoxPushpinCatalog.
            this.ListBoxPushpinCatalog.DataContext = _pushpinCatalog;

            
        }


        #region Property Changed

        private void NotifyPropertyChanged(string propertyName)
        {
        //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;// = delegate { };

        #endregion
    }

    
}
