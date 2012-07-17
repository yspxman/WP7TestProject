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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace AppTest.YWeather.Engine
{
    public class WeatherForecastDataModel
    {
        public string ID { get; set; }
        public string CityName { get; set; }
        public string temp_low { get; set; }
        public string temp_high { get; set; }

        public string condition_string { get; set; }
        public WeatherCondition.TWeatherConditions condition { get; set; }

        private string _weatherIconPath;
        public string WeatherIconPath 
        {
            get
            {
              var c =  WeatherCondition.MappingToWeatherCondition(condition_string);
              return WeatherCondition.GetWeatherIconByCondition(c);
            }
            set{ _weatherIconPath = value;}
        
        }

        public DateTime Date { get; set; } //那一天的预告

        private string _date_string;
        public string Date_string
        {
            set
            {
                _date_string = value;
            }
            get
            {
                if (_date_string != "")
                    return _date_string;
                else
                    return Date.Month + "月" + Date.Day + "日";
            }
        }
        public string Temp_string
        {
            get
            {
                return temp_low.ToString() + "° / " + temp_high.ToString() + "°";
            }
        }
    }

    public class WeatherCityInfoDataModel : INotifyPropertyChanged
    {
        public string CityID { get; set; }

        private string _cityName;
        public string CityName
        {
            get { return _cityName; }
            set
            {
                _cityName = value;
                NotifyPropertyChanged("CityName");
            }
        }

        private string _current_Temp;
        public string Current_Temp
        {
            get { return _current_Temp+"°C"; }
            set{
                _current_Temp = value;
                NotifyPropertyChanged("Current_Temp");
            }
        }
        private string _current_date;
        public string Current_date 
        {
            get { return _current_date; }
            set
            {
                _current_date = value;
                NotifyPropertyChanged("Current_date");
            }
        }
        private string _update_time;
        public string Update_time 
        {
            get { return _update_time; }
            set
            {
                _update_time = value;
                NotifyPropertyChanged("Update_time");
            }
        }


        //必须实现interface的event
        public event PropertyChangedEventHandler PropertyChanged;

        //　NotifyPropertyChanged　will　raise　the　PropertyChanged　event　passing　the
        //　source　property　that　is　being　updated.
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

                var delegatelist = PropertyChanged.GetInvocationList();
            }
        }
    }

    public class WeatherDataModel
    {
        public WeatherCityInfoDataModel cityinfo { get; set; }
        public ObservableCollection<WeatherForecastDataModel> DC_WeatherByDay
        { get; private set; }

        public WeatherDataModel()
        {
            cityinfo = new WeatherCityInfoDataModel();
            DC_WeatherByDay = new ObservableCollection<WeatherForecastDataModel>();
        }
    }

    public class DataModel
    {
        private Dictionary<string, WeatherDataModel> weatherDictionary;

        public DataModel()
        {
            //weatherDMs = new List<WeatherDataModel>();

            weatherDictionary = new Dictionary<string, WeatherDataModel>();
            WeatherDataModel dm1 = new WeatherDataModel();
            dm1.cityinfo.CityName = "北京";
            dm1.cityinfo.CityID = "beijing";

            WeatherDataModel dm2 = new WeatherDataModel();
            dm2.cityinfo.CityName = "大连";
            dm2.cityinfo.CityID = "dalian";

            WeatherDataModel dm3 = new WeatherDataModel();
            dm3.cityinfo.CityName = "商洛";
            dm3.cityinfo.CityID = "shangluo";

            weatherDictionary.Add(dm1.cityinfo.CityID, dm1);
            weatherDictionary.Add(dm2.cityinfo.CityID, dm2);
            weatherDictionary.Add(dm3.cityinfo.CityID, dm3);
        }

        public Dictionary<string, WeatherDataModel>  GetWeatherDictionary()
        {
            return weatherDictionary;
        }

        //List<WeatherDataModel> weatherDMs;
        
        public void makeTestData()
        {
            /*
            DC_WeatherByDay.Clear();
            DC_WeatherByDay.Add(new WeatherForecastDataModel() { CityName = "北京", condition = "阴", temp_high = "17", temp_low = "2", Date = DateTime.Now });
            DC_WeatherByDay.Add(new WeatherForecastDataModel() { CityName = "北京", condition = "晴转多云", temp_high = "12", temp_low = "5", Date = DateTime.Now.AddDays(1) });
        
             */
        }

    }
}
