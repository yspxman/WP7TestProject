using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using AppTest.YWeather.Engine;


namespace AppTest.YWeather
{
    public partial class YWeatherMain : PhoneApplicationPage
    {
        WeatherEngine _engine;
        DataModel _dataModel;

        public YWeatherMain()
        {
            InitializeComponent();

            _engine = new WeatherEngine();
            _dataModel = _engine.GetDataModel();

            CreateItems();

        }
        
        private void CreateItems()
        {
            // dictionary 的遍历
            foreach (KeyValuePair<string, WeatherDataModel> c in _dataModel.GetWeatherDictionary())
            {
                PanoramaItem item = new PanoramaItem();
                WeatherDataModel city = c.Value;

                item.HeaderTemplate = this.ItemHeaderTemplate;
                item.Header = city.cityinfo;
                
                
                ListBox lb = new ListBox();                
                lb.ItemTemplate = this.ListBoxTemplate;

                //！！！ Listbox 绑定数据源是用itemSource 而不是 DataContext！！！！！
                lb.ItemsSource = city.DC_WeatherByDay;
                item.Content = lb;
                //item.DataContext = channel_data;   // assign channel data to the current item,

                this.PanoramaCtrl.Items.Add(item);
                
            }

            PanoramaItem item2 = new PanoramaItem();
            item2.Header = "城市管理";
            this.PanoramaCtrl.Items.Add(item2);
        }


        private void Panorama_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Panorama_Loaded");

        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("Panorama_SelectionChanged");
        }

        private void Panorama_Unloaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Panorama_Unloaded");
        }
    }
}