using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

using AppTest.DB;

namespace AppTest
{
    // Definitions of data collections for UI view
    public class DataCollectionServiceDef
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ID { get;  set; }
        public bool Free { get;  set; }
        public string Genre { get;  set; }

        // current playing content
        public string CurrentContentName { get; set; }
        public DateTime CurrentContentStartTime { get; set; }
        public DateTime CurrentContentEndTime { get; set; }
        public string CurrentContentTimeDuration
        {
            get
            {
                return CurrentContentStartTime.ToShortTimeString() + " - " + CurrentContentEndTime.ToShortTimeString();
            }
            //   private set;
        }

        //public DataCollectionContentDef CurrentContent { get; set; }
    }

    public class DataCollectionServiceExtDef
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
    }

    public class DataCollectionContentDef
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Free { get; set; }
        public string TimeDuration
        {
            get
            {
                return StartTime.ToShortTimeString() + " - " + EndTime.ToShortTimeString();
            }
         //   private set;
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.ServiceCollection = new ObservableCollection<DataCollectionServiceDef>();
            this.ServiceExtCollection = new ObservableCollection<DataCollectionServiceExtDef>();
            this.ContentCollection = new ObservableCollection<DataCollectionContentDef>();
        }

        // used for ext setting
        public ObservableCollection<DataCollectionServiceExtDef> ServiceExtCollection
        { get; private set; }

        public ObservableCollection<DataCollectionServiceDef> ServiceCollection
        { get; private set; }

        public ObservableCollection<DataCollectionContentDef> ContentCollection
        { get; private set; }

        private DBEngine _dbEngine = App.DBEngineInstance;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

       // private bool isAddedToCollection = false;

        public void ReadServiceToCollection()
        {
            //if (!isAddedToCollection)

            // refresh the view everytime
            this.ServiceCollection.Clear();
            IEnumerator<ServiceTable> enumerator = App.DBEngineInstance.CmtvDBContext.Service_items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                // get the content from service ID
                var contents = _dbEngine.QueryContentFromServiceID(enumerator.Current.ID);
                // only pickup the first item
                
                this.ServiceCollection.Add
                    (new DataCollectionServiceDef()
                    {
                        Name = enumerator.Current.ServiceName,
                        Description = enumerator.Current.Description,
                        ID = enumerator.Current.ID,
                        Genre = enumerator.Current.Genre,
                        Free = enumerator.Current.ForFree,

                        CurrentContentName = contents.FirstOrDefault().Name,
                        CurrentContentStartTime = contents.FirstOrDefault().StartTime,
                        CurrentContentEndTime = contents.FirstOrDefault().EndTime
                    });
            }
        }

        public void ReadServiceExtToCollection(List<DataCollectionServiceExtDef> d)
        {
            this.ServiceExtCollection.Clear();
            foreach (var v in d)
            {
                this.ServiceExtCollection.Add(v);
            }
        }

        public ObservableCollection<DataCollectionContentDef> ReadContentsToCollectionFromServiceID(string id)
        {
            ContentCollection.Clear();

            var re = _dbEngine.QueryContentFromServiceID(id);
            foreach (var v in re)
            {
                this.ContentCollection.Add(v);
            }
            return ContentCollection;
        }
    }
}