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

namespace YWeather.Engine
{
    public class WeatherCondition
    {
       public enum TWeatherConditions
        {
            suny = 0,
            mostly_suny_1,
            mostly_suny_2,
            cloudy,
            chance_of_rain,
            rain_small,
            rain_big,
            fog,
            snow_small, 
            snow_big,
            thunderstorm,
            chance_of_storm,
            ConditionEnd
        };

        public static TWeatherConditions MappingToWeatherCondition(string des)
        {
            TWeatherConditions condition = TWeatherConditions.suny;

            switch (des)
            {
                case "晴":                
                case "sunny":
                    condition = TWeatherConditions.suny;
                    break;

                case "以晴为主":
                case "晴间多云":
                
                case "mostly_sunny":
                    condition = TWeatherConditions.mostly_suny_1;
                    break;

                case "多云":
                    condition = TWeatherConditions.cloudy;
                    break;

                case "雨":
                case "小雨":  
                case "rain_small":
                    condition = TWeatherConditions.rain_small;
                    break;


                case "可能有雨":
                case "chance_of_rain":
                    condition = TWeatherConditions.chance_of_rain;
                    break;

                case "大雨":
                case "big_rain":
                    condition = TWeatherConditions.rain_big;
                    break;

                case "雾":
                case "fog":
                    condition = TWeatherConditions.fog;
                    break;

                case "雷阵雨":
                case "thunderstorm":
                    condition = TWeatherConditions.thunderstorm;
                    break;

                case "可能有暴风雨":
                case "chance_of_storm":
                    condition = TWeatherConditions.chance_of_storm;
                    break;

                case "小雪":
                case "small_snow":
                    condition = TWeatherConditions.snow_small;
                    break;

                case "大雪":
                case "big_snow":
                    condition = TWeatherConditions.snow_big;
                    break;
            }            
            return condition;
        }

        public static string GetWeatherIconByCondition(TWeatherConditions condition)
        {
            string str = "Resources/Icons/";
            switch (condition)
            { 
                case TWeatherConditions.suny:
                    str += "suny.png";
                    break;
                case TWeatherConditions.mostly_suny_1:
                    str += "suny_cloudy_small.png";
                    break;
                case TWeatherConditions.mostly_suny_2:
                    str += "suny_cloudy_big.png";
                    break;
                
                case TWeatherConditions.cloudy:
                    str += "cloudy.png";
                    break;

                case TWeatherConditions.chance_of_storm:
                    str += "chance_of_storm.png";
                    break;
                case TWeatherConditions.fog:
                    str += "fog.png";
                    break;

                case TWeatherConditions.chance_of_rain:
                    str += "rain_suny.png";
                    break;

                case TWeatherConditions.rain_big:
                    str += "rain_big.png";
                    break;
                case TWeatherConditions.rain_small:
                    str += "rain_small.png";
                    break;
                case TWeatherConditions.snow_big:
                    str += "snow_big.png";
                    break;
                case TWeatherConditions.snow_small:
                    str += "snow_small.png";
                    break;
                case TWeatherConditions.thunderstorm:
                    str += "thunderstorm.png";
                    break;
            }

            return str;
        }
    }
}
