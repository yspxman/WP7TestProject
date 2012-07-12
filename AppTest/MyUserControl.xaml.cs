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

namespace AppTest
{
    public partial class MyUserControl : UserControl
    {
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(String), 
            typeof(MyUserControl), new PropertyMetadata(onLabelChanged));

        static void onLabelChanged(DependencyObject dp, DependencyPropertyChangedEventArgs args)
        {
            (dp as MyUserControl).colorLabel.Text = args.NewValue as String;
        }

        public String Label
        {
            get { return (String)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public MyUserControl()
        {
            InitializeComponent();
        }
    }
}
