﻿using System;
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

namespace PanoramaAppTest
{
    public partial class PivotPage1 : PhoneApplicationPage
    {
        public PivotPage1()
        {
            InitializeComponent();

            DataContext = App.ViewModel;
            this.Loaded +=new RoutedEventHandler(PivotPage1_Loaded);
        }
        private void PivotPage1_Loaded(object sender, RoutedEventArgs a)
        {
            App.ViewModel.ReadDataToCollection();
            
        }

    }
}