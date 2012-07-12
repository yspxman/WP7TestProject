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
using System.Xml;
using System.IO;
using System.Linq;
using System.Data.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Globalization;


namespace AppTest.YWeather.Engine
{
    public class WeatherEngine
    {
        private DataModel _dataModel;
        private const string  GoogleWeatherURI = "http://www.google.com/ig/api?hl=zh-cn&weather=";

        public WeatherEngine()
        {
            initialize();
        }

        public DataModel GetDataModel()
        {
            if (_dataModel != null)
                return _dataModel;
            else
                return null;
        }

        public void initialize()
        {
            if (_dataModel == null)
                _dataModel = new DataModel();

            // dictionary 的遍历
            foreach(KeyValuePair<string, WeatherDataModel> c in _dataModel.GetWeatherDictionary())
            {
                UpdateWeather(c.Key);
            }

            
            //UpdateWeather();
        }

        public void UpdateWeather(string cityId)
        {
            string UpdateURI = GoogleWeatherURI + cityId;

            WebClient wc = new WebClient();
            wc.Encoding = new GB2312.GB2312Encoding();
            wc.DownloadStringAsync(new Uri(UpdateURI));
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //int a;
            //string ss;
            
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                return;
            }
            // gb2312 如何转换成UTF8
            string s = e.Result.ToString();
            XmlReader xmlReader = XmlReader.Create(new StringReader(s));

            // XmlDocument 在silverlight中不支持。只能试试XDocument
            XDocument xdoc = XDocument.Parse(s);
            
               
            var cityinfo = from c in xdoc.Descendants("forecast_information")
                    select new
                    {
                       date = c.Element("forecast_date").Attribute("data").Value,
                       cityId = c.Element("postal_code").Attribute("data").Value,
                    };

            WeatherDataModel cityDM;
            _dataModel.GetWeatherDictionary().TryGetValue(cityinfo.FirstOrDefault().cityId, out cityDM);



            cityDM.cityinfo.Current_date = cityinfo.FirstOrDefault().date;

            try
            {
                var current_condition = from c in xdoc.Descendants("current_conditions")
                                        select new
                                        {
                                            condition = c.Element("condition").Attribute("data").Value,
                                            temp_c = c.Element("temp_c").Attribute("data").Value,
                                            humidity = c.Element("humidity").Attribute("data").Value,
                                            wind_condition = c.Element("wind_condition").Attribute("data").Value,
                                        };

                cityDM.cityinfo.Current_Temp = current_condition.FirstOrDefault().temp_c;
                cityDM.cityinfo.Update_time = DateTime.Now.ToShortTimeString() + "更新";

            }
            catch
            { 
            }
    

            var f = from c in  xdoc.Descendants("forecast_conditions")
                    select new 
                    {
                        dayOfWeek = c.Element("day_of_week").Attribute("data").Value,
                        lowTemp = c.Element("low").Attribute("data").Value,
                        highTemp = c.Element("high").Attribute("data").Value,
                        condition_string = c.Element("condition").Attribute("data").Value,
                    };

            
            foreach(var item in f)
            {
                cityDM.DC_WeatherByDay.Add
                    (
                    new WeatherForecastDataModel() { Date_string = item.dayOfWeek, 
                        condition_string = item.condition_string, temp_low = item.lowTemp, 
                        temp_high = item.highTemp }
                );
            }
        }


        void fun1(XmlReader xmlReader)
        {
            // XML 的全解析

            while (xmlReader.Read())
            {
                // Response.Write("<li>节点类型：" + xmlReader.NodeType + "==<br>");
                switch (xmlReader.NodeType)
                {                    
                    case XmlNodeType.Attribute:
                        for (int i = 0; i < xmlReader.AttributeCount; i++)
                        {
                            xmlReader.MoveToAttribute(i);
                            //Response.Write("属性：" + xmlReader.Name + "=" + xmlReader.Value + "&nbsp;");
                        }
                        break;
                    case XmlNodeType.CDATA:
                        //Response.Write("CDATA:" + xmlReader.Value + "&nbsp;");
                        break;
                    case XmlNodeType.Element:
                        //Response.Write("节点名称：" + xmlReader.LocalName + "<br>")
                        
                        for (int i = 0; i < xmlReader.AttributeCount; i++)
                        {
                            xmlReader.MoveToAttribute(i);
                            //Response.Write("属性：" + xmlReader.Name + "=" + xmlReader.Value + "&nbsp;");
                        }
                        break;
                    case XmlNodeType.Comment:
                        //Response.Write("Comment:" + xmlReader.Value);
                        break;
                    case XmlNodeType.Whitespace:
                        //Response.Write("Whitespace:" + "&nbsp;");
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        //Response.Write("ProcessingInstruction:" + xmlReader.Value);
                        break;
                    case XmlNodeType.Text:
                        //Response.Write("Text:" + xmlReader.Value);
                        break;
                }
            }
        }
    }


    public class WeatherIconConverter : System.Windows.Data.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
                return Math.Round((double)value);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
