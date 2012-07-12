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
using System.Collections.Generic;
using System.Linq;

namespace AppTest.Map
{
    public class PushpinCatalog
    {
        private List<PushpinModel> _items;
        public IEnumerable<PushpinModel> Items
        {
            get { return _items; }
        }
        public PushpinCatalog()
        {
            InitializePushins();
        }

        private void InitializePushins()
        {
            string[] pushinIcons =
            {
            "PushpinBicycle.png",
            "PushpinCar.png",
            "PushpinDrink.png",
            "PushpinFuel.png",
            "PushpinHouse.png",
            "PushpinRestaurant.png",
            "PushpinShop.png"
            };

            var pushpins = from icon in pushinIcons
                           select new PushpinModel
                               {
                                   Icon = new Uri("/Map/images/" + icon, UriKind.Relative),
                                   TypeName = System.IO.Path.GetFileNameWithoutExtension(icon)
                               };

            _items = pushpins.ToList();
        }
    }
}
