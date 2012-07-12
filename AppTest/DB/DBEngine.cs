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
using System.Linq;
using System.Data.Linq.Mapping;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;



namespace AppTest.DB
{
    public class DBEngine
    {
        //数据库定义
        private CmtvDBDataContex _dbContext;
        public CmtvDBDataContex CmtvDBContext 
        {
            get { return _dbContext; }
        }

        public  DBEngine()
        {
            _dbContext = new CmtvDBDataContex();
            if (!_dbContext.DatabaseExists())
            {
                _dbContext.CreateDatabase();
                SaveTestDataToDB();
            }
            
        }

        public void SaveTestDataToDB()
        {
            // 1. add service items
            SaveServiceData(new ServiceTable() { ID = "1", ServiceName = "BTV", Description = "test1" });
            SaveServiceData(new ServiceTable() { ID = "2", ServiceName = "CCTV-2", Description = "一个频道可包括若干个节目" });
            SaveServiceData(new ServiceTable() { ID = "3", ServiceName = "CCTV-3", Description = "该分片的版本号。新版本的分片可以从接收到的时候开始" });
            SaveServiceData(new ServiceTable() { ID = "4", ServiceName = "CCTV-4", Description = "节目唯一对应于一个频道（一个content分片只" });

            // 2. add service extention items
            SaveServiceExtData(new Service_ExtentionTable() { ServiceID = "1", ExtensionURL = "url:xxx.com", ExtensionDescription = "service id =1" });
            SaveServiceExtData(new Service_ExtentionTable() { ServiceID = "1", ExtensionURL = "url:4364564.com", ExtensionDescription = "service id =1" });
            SaveServiceExtData(new Service_ExtentionTable() { ServiceID = "2", ExtensionURL = "url:2.com", ExtensionDescription = "service id =2" });
            
            //3. add content      
            SaveContentData(new ContentTable() { ServiceID = "1",  ID = "C1", ContentName = "天下音乐：睛彩天下", ContentClass ="Video", StartTime =DateTime.Now, EndTime = DateTime.Now   });
            SaveContentData(new ContentTable() { ServiceID = "4",  ID = "C2", ContentName = "朝闻天下", ContentClass = "Video", StartTime = DateTime.Now, EndTime = DateTime.Now });
            SaveContentData(new ContentTable() { ServiceID = "1", ID = "C3", ContentName = "生活早参考", ContentClass = "Video", StartTime = DateTime.Now, EndTime = DateTime.Now });
            SaveContentData(new ContentTable() { ServiceID = "4", ID = "C4", ContentName = "焦点访谈:用事实说话", ContentClass = "Video", StartTime = DateTime.Now, EndTime = DateTime.Now });
            SaveContentData(new ContentTable() { ServiceID = "1", ID = "C5", ContentName = "国际艺苑", ContentClass = "Video", StartTime = DateTime.Now.AddHours(-2), EndTime = DateTime.Now });

            SaveContentData(new ContentTable() { ServiceID = "2", ID = "C6", ContentName = "焦点访谈:", ContentClass = "Video", StartTime = DateTime.Now.AddMinutes(33), EndTime = DateTime.Now.AddHours(5) });
            SaveContentData(new ContentTable() { ServiceID = "2", ID = "C7", ContentName = "说话", ContentClass = "Video", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) });
            SaveContentData(new ContentTable() { ServiceID = "3", ID = "C8", ContentName = "睛彩天下", ContentClass = "Video", StartTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(13) });

        }

        public bool SaveContentData(ContentTable item)
        {
            try
            {
                _dbContext.Content_items.InsertOnSubmit(item);
                _dbContext.SubmitChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

        public bool SaveServiceExtData(Service_ExtentionTable item)
        {
            try
            {
                _dbContext.Servicext_items.InsertOnSubmit(item);
                _dbContext.SubmitChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

        public void QueryTest()
        {
            //EnumerableQuery
            //EnumerableRowCollection

            //1. 试着做一个 查询
            var dataset = from c in _dbContext.Service_items where c.ID == "1" select c;
            List<ServiceTable> serviceList = dataset.ToList();

            //2. 查询ext中所有的items
            var dataset2 = from c in _dbContext.Servicext_items select c;

            //3. 从两张表中查1个表的结果，查询service name 是CCTV-1的所有service 的ext， 返回的结果集是一个匿名的新对象的集合
            var dataset3 = from c in _dbContext.Servicext_items
                           where c.Service.ServiceName == "CCTV-1"  // 这里用到了EntityRef
                           select new               // Create a  new anonymous type
                           {
                               c.ServiceID,
                               c.Service.ServiceName,   // 可以通过、entityRef来取 另一张表的内容
                               c.ExtensionURL
                           };

            //4. 取出第一个数据，在update 和 delete中有用
            var dataset4 = _dbContext.Service_items.First(c => c.ServiceName == "CCTV-2");

            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("services:");
            foreach (/*Service_ExtentionTable service in list*/ var v in dataset3)
            {
                messageBuilder.AppendLine(v.ServiceID + "  " + v.ServiceName + "  " + v.ExtensionURL);
            }
            MessageBox.Show(messageBuilder.ToString());
        }

        public List<DataCollectionServiceDef> QueryServiceFromID(string id)
        {
            //1. 试着做一个 查询

            IQueryable<DataCollectionServiceDef> dataset = from c in _dbContext.Service_items
                          where c.ID == id
                          select new DataCollectionServiceDef() 
                          {
                              Name = c.ServiceName,
                              Description = c.Description,
                              Free = c.ForFree,
                              Genre = c.Genre,
                              ID = c.ID                              
                          };

            // 直到调用 ToList ，dataset 才会被实例化，查询才真正执行
            return dataset.ToList<DataCollectionServiceDef>();
        }



        public List<DataCollectionServiceExtDef> QueryServiceExtFromID(string id)
        {
            var dataset = from c in _dbContext.Servicext_items
                           where c.Service.ID == id  // 这里用到了EntityRef
                           select new  DataCollectionServiceExtDef             // Create a  new anonymous type
                           {
                                Description = c.ExtensionDescription, 
                                Url = c.ExtensionURL,
                                ID = c.ID
                           };

            return dataset.ToList<DataCollectionServiceExtDef>();
        }



        public List<DataCollectionContentDef> QueryContentFromServiceID(string id)
        {
            var dataset = from c in _dbContext.Content_items
                          where c.Service.ID == id  // 这里用到了EntityRef
                          select new DataCollectionContentDef             // Create a  new anonymous type
                          {
                              ID = c.ID,
                              Name = c.ContentName,
                              StartTime = c.StartTime,
                              EndTime = c.EndTime,
                              Free = c.ForFree
                          };

            return dataset.ToList<DataCollectionContentDef>();
        }

        public bool UpdateServiceItem(DataCollectionServiceDef data)
        {
            // 1. find the item in DB to update
            var item = from c in _dbContext.Service_items where c.ID == data.ID select c;

            ServiceTable t = item.FirstOrDefault();
            
            // 2. assign values
            t.ServiceName = data.Name;
            t.Description = data.Description;
            t.ForFree = data.Free;
            t.Genre = data.Genre;

            // 3. submit changes
            try
            {
                _dbContext.SubmitChanges();
            }
            catch 
            {
                return false;
            }

            return true;
        }

        public bool SaveServiceData(ServiceTable item)
        {
            try
            {
                //1. 把数据加入到本地Collection中， 这样绑定DataContext的控件就可以直接得到数据了 
                //this.ServiceCollection.Add(item);
                // 2. 把数据加到数据库中                
                _dbContext.Service_items.InsertOnSubmit(item);
                _dbContext.SubmitChanges();
            }
            
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
