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
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace AppTest.DB
{
    [Table] 
    //Service也就是频道，channel
    //Service 分片用于描述手机电视业务的频道的信息，一个Service分片可以用来提供一个电视频道（例如CCTV-5）的信息。
    //一个频道可以包含多个节目（一个Service分片可以被多个Content分片所引用）。
    public class ServiceTable 
    {
        //频道ID (1)
        [Column(IsPrimaryKey = true, CanBeNull = false)]
        public string ID { get; set; }

        // 和extention的 约束关系
        private EntitySet<Service_ExtentionTable> _extensions;
        [Association(Storage = "_extensions", OtherKey = "ServiceID", ThisKey = "ID")]
        public EntitySet<Service_ExtentionTable> Extentions
        {
            get { return _extensions; }
            set { this._extensions.Assign(value); }
        }

        // 和content的 约束关系
        private EntitySet<ContentTable> _contents;
        [Association(Storage = "_contents", OtherKey = "ServiceID", ThisKey = "ID")]
        public EntitySet<ContentTable> Contents
        {
            get { return _contents; }
            set { this._contents.Assign(value); }
        }

        //Service 的版本号 (1)
        [Column(CanBeNull = false)]
        public int Version { get; set; }

        // Global ID (0-1)
        [Column]
        public string GlobalServiceID { get; set; }

        // (0-1)该频道相对于其他频道展现给用户的顺序。该值提供了一种频道列表顺序的组织方法。
        [Column]
        public int Weight { get; set; }

        // Free? (0-1)
        [Column]
        public bool ForFree { get; set; }

        //servicetype   (0-n) format:Json 
        [Column]
        public string ServiceType { get; set; }

        // service name (1-n) format:Json , 注意，可能该字段附带 lang 属性
        [Column(CanBeNull = false)]
        public string ServiceName { get; set; }

        // Service _description (0-n) format:Json 

        [Column]
        public string Description { get; set; }

        // Service Genre  (0-n) format:Json 
        [Column]
        public string Genre { get; set; }

        // Service Provider  (0-1) format:Json (Lang, content)
        [Column]
        public string Provider {get; set;}

        // CADescriptor  (0-1) 
        [Column]
        public string CADescriptor { get; set; }


        /// <summary>
        ///  entity set 初始化
        /// </summary>
        public ServiceTable()
        {
            this._extensions = new EntitySet<Service_ExtentionTable>(
                new Action<Service_ExtentionTable>(this.ExtensionAdd),
                 new Action<Service_ExtentionTable>(this.ExtensionRemove)

                 );

            this._contents = new EntitySet<ContentTable>(
                                 new Action<ContentTable>(this.ContentAdd),
                 new Action<ContentTable>(this.ContentRemove)
                );

        }

        private void ExtensionAdd(Service_ExtentionTable a)
        {
            a.Service = this;
        }
        private void ExtensionRemove(Service_ExtentionTable a)
        {
            a.Service = null;
        }

        private void ContentAdd(ContentTable a)
        {
            a.Service = this;
        }
        private void ContentRemove(ContentTable a)
        {
            a.Service = null;
        }
    }

    [Table]
    public class Service_ExtentionTable
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ID { get; set; }

        [Column]
        public string ServiceID { get; set; }

        // _services
        private EntityRef<ServiceTable> _services;
        [Association(Storage = "_services", ThisKey = "ServiceID", OtherKey = "ID", IsForeignKey = true)]
        public ServiceTable Service
        {
            get { return _services.Entity; }
            set { ServiceID = value.ID; }
        }

        // Service Extension  (0-1)
        [Column]
        public string ExtensionDescription { get; set; }
        
        // URL (1) 
        [Column]
        public string ExtensionURL { get; set; }
    }

    [Table]
    public class Service_PreviewDataReference
    {
        private string _serviceID;
        [Column]
        public string ServiceID
        {
            get { return _serviceID; }
            set { _serviceID = value; }
        }

        [Column]
        public String IDRef { get; set; }

        [Column]
        public String Usage { get; set; }
    }


    [Table]
    public class ContentTable
    {
        //Content ID (1)
        [Column(IsPrimaryKey = true, CanBeNull = false)]
        public string ID { get; set; }

        [Column]
        public string ServiceID { get; set; }

        // Foreign Key service ID
        private EntityRef<ServiceTable> _services;
        [Association(Storage = "_services", ThisKey = "ServiceID", OtherKey = "ID", IsForeignKey = true)]
        public ServiceTable Service
        {
            get { return _services.Entity; }
            set { ServiceID = value.ID; }
        }

        //版本号 (1)
        [Column(CanBeNull = false)]
        public int Version { get; set; }

        // Global ID (0-1)
        [Column]
        public string GlobalContentID { get; set; }

        // Free? (0-1)
        [Column]
        public bool ForFree { get; set; }

        //Live? (0-1)
        [Column]
        public bool Live  { get; set; }

        //Repeat? (0-1)
        [Column]
        public bool Repeat { get; set; }

        //Keyword  (0-n) Json 封装
        [Column]
        public string Keyword { get; set; }

        //ContentClass  (1-n) Json 封装
        // “text”、“image”、“audio”、“video
        [Column(CanBeNull = false)]
        public string ContentClass  { get; set; }

        //  name (1-n) format:Json , 注意，可能该字段附带 lang 属性
        [Column(CanBeNull = false)]
        public string ContentName { get; set; }

        //  _description (0-n) format:Json 
        [Column]
        public string Description { get; set; }

        [Column]
        public DateTime StartTime { get; set; }

        [Column]
        public DateTime EndTime { get; set; }

        [Column]
        public string AudioLanguage { get; set; }

        [Column]
        public string TextLanguage { get; set; }

        // Content Genre  (0-n) format:Json 
        [Column]
        public string Genre { get; set; }

        // CADescriptor  (0-1) 
        [Column]
        public string CADescriptor { get; set; }

        //TODO to be continued ... PreviewDataReference PrivateExt  
    }

    public class CmtvDBDataContex : DataContext
    {
        public const string ConnnectionStr = "Data Source=isostore:/cmtvDB.sdf";
        public Table<ServiceTable> Service_items 
        {
            get {return this.GetTable<ServiceTable>();}
        }
        public Table<Service_ExtentionTable> Servicext_items
        {
            get { return this.GetTable<Service_ExtentionTable>(); }
        }

        public Table<ContentTable> Content_items
        {
            get { return this.GetTable<ContentTable>(); }
        }

        public CmtvDBDataContex()
            : base(ConnnectionStr)
        { 
        }
    }
}
