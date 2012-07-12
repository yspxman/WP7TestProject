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
using System.Windows.Data;
using System.Globalization;

namespace AppTest
{
    public class GradientButton : Button
    {
        GradientStop gradientStop1, gradientStop2;
        public static readonly DependencyProperty Color1Property = DependencyProperty.Register
            ("Color1", typeof(Color), typeof(GradientButton), 
            new PropertyMetadata(Colors.Black, onColorChanged));

        public static readonly DependencyProperty Color2Property = DependencyProperty.Register
            ("Color2", typeof(Color), typeof(GradientButton),
            new PropertyMetadata(Colors.White, onColorChanged));

        // constructor 
        public GradientButton()
        {
            // set button background as gradient brush
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0);
            brush.EndPoint = new Point(1, 0);

            gradientStop1 = new GradientStop();
            gradientStop1.Offset = 0;
            gradientStop1.Color = ColorA;
            brush.GradientStops.Add(gradientStop1);

            gradientStop2 = new GradientStop();
            gradientStop2.Offset = 1;
            gradientStop2.Color = ColorB;
            brush.GradientStops.Add(gradientStop2);

            // set brush as foreground
            this.Foreground = brush;
        }

        public Color ColorA
        {
            get { return (Color)this.GetValue(Color1Property); }
            set { this.SetValue(Color1Property, value); }
        }
        public Color ColorB
        {
            get { return (Color)this.GetValue(Color2Property); }
            set { this.SetValue(Color2Property, value); }
        }

        static void onColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            GradientButton btn = obj as GradientButton;

            // usaully we can define no-static onColorChanged in instance, like this
            // btn.onColorChanged(args);

            // if dp porperty value changed, then change the gradient brush as well
            if (args.Property == Color1Property)
                btn.gradientStop1.Color = (Color)args.NewValue;

            if (args.Property == Color2Property)
                btn.gradientStop2.Color = (Color)args.NewValue;

        }
    }



    public class MyData : DependencyObject
    {
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("value", typeof(string), typeof(MyData),
            new PropertyMetadata("Initial Value from DP!"));

        public string value
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
    }


    public partial class DependencyPropertyPage : PhoneApplicationPage
    {
  
        private MyData _data;

        // Constructor
        public DependencyPropertyPage()
        {
            InitializeComponent();

            string str = "Red=244&Green=43&Blue=91";
            IDictionary<string, string> parameter;// = str;    
            //NavigationContext.
            byte R = Byte.Parse("144");
            byte G = Byte.Parse("17");
            byte B = Byte.Parse("145");

            this.TitlePanel.Background = new SolidColorBrush(Color.FromArgb(255, R, G, B));


            _data = new MyData();
            _data.value = "dynamic data binding";
            this.textBlock2.DataContext = _data;  // datacontext 必须是dependency object


            var binding = new Binding("value") { Source = _data };
           // _data.value = "data binding 2";
            this.textBlock3.SetBinding(TextBlock.TextProperty, binding);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //this.textBlock2.SetBinding(slider1, slider1.Value);
            _data.value = "text will be changed";
          

            // 设置一个dependency property
            this.button1.SetValue(Button.ContentProperty, "Set From DP!");

        }

    }

    public class MyConverter : System.Windows.Data.IValueConverter
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





    


 
