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
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace AppTest.Map
{
    public partial class Page1 : PhoneApplicationPage,INotifyPropertyChanged
                               
    {

        internal const string MapId = "AmrN4VSDWPmWlRbaLZ4GwdYkVjChjQgh6ilyfEh4x5Gdq5FdKa-jIbK9iaaQ6kO1";

        private readonly CredentialsProvider _credentialsProvider = new ApplicationIdCredentialsProvider(MapId);

        private const double DefaultZoomLevel = 18.0;
        private const double MaxZoomLevel = 21.0;
        private const double MinZoomLevel = 1.0;
        private double _zoom = 1.0;


        private static readonly GeoCoordinate DefaultLocation = new GeoCoordinate(47.639631, -122.127713);

        private GeoCoordinate _center;

        public double Zoom
        {
            get { return _zoom; }
            set
            {
                var coercedZoom = Math.Max(MinZoomLevel, Math.Min(MaxZoomLevel, value));
                if (_zoom != coercedZoom)
                {
                    _zoom = value;
                    NotifyPropertyChanged("Zoom");
                }
            }
        }
        public CredentialsProvider CredentialsProvider
        {
            get { return _credentialsProvider; }
        }


        public GeoCoordinate Center
        {
            get { return _center; }
            set
            {
                if (_center != value)
                {
                    _center = value;
                    NotifyPropertyChanged("Center");
                }
            }
        }

        private void CenterLocation()
        {
            Center = DefaultLocation;

            Zoom = DefaultZoomLevel;
        }








        private void ChangeMapMode()
        {
            if (map1.Mode is AerialMode)
            {
                map1.Mode = new RoadMode();
            }
            else
            {
                map1.Mode = new AerialMode(true);
            }
        }
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            ChangeMapMode();
        }

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            Zoom += 1;
        }

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            Zoom -= 1;
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            CenterLocation();


            CenterPushpinsPopup(new Point(100, 100));
        }




        private readonly ObservableCollection<PushpinModel> _pushpins = new ObservableCollection<PushpinModel>
        {
             new PushpinModel
            {
                Location = DefaultLocation,
                Icon = new Uri("/Map/images/PushpinLocation.png", UriKind.Relative)
            }
        };

        public ObservableCollection<PushpinModel> Pushpins
        {
            get { return _pushpins; }
        }

        private void CenterPushpinsPopup(Point touchPoint)
        {
            // Reposition the pushpins popup to the center of the touch point.
            Canvas.SetTop(PushpinPopup, touchPoint.Y - ListBoxPushpinCatalog.Height / 2);
        }

    }
}