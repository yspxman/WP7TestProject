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

namespace AppTest
{
    public partial class DBTestPage : PhoneApplicationPage
    {
        public DBTestPage()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            // Add
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // Update
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            //Delete
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            // Select
            App.DBEngineInstance.QueryTest();
        }
    }
}