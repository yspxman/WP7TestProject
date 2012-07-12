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
using System.Threading;
namespace TestProj
{
    public partial class Animation : PhoneApplicationPage
    {
        public Animation()
        {
            InitializeComponent();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            RotateTransform transf = btn.RenderTransform as RotateTransform;
            
            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 0;
            anim.To = 360;
            anim.Duration = new Duration(TimeSpan.FromSeconds(3));
            anim.BeginTime = TimeSpan.FromSeconds(0.5);

            Storyboard.SetTarget(anim, transf);
            //Storyboard.SetTargetProperty(anim, new PropertyPath(RotateTransform.AngleProperty));
            Storyboard.SetTargetProperty(anim, new PropertyPath("RotateTransform.Angle"));

            //Storyboard.SetTargetProperty(anim, new PropertyPath(RotateTransform.CenterXProperty));

            Storyboard sb = new Storyboard();
            sb.Children.Add(anim);
            sb.Begin();
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ScaleTransform transf = btn.RenderTransform as ScaleTransform;

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 1;
            anim.To = 2;
            anim.Duration = new Duration(TimeSpan.FromSeconds(1));
            anim.AutoReverse = true;
            anim.RepeatBehavior = new RepeatBehavior(3);
            //anim.EasingFunction = new  
            Storyboard.SetTarget(anim, transf);
            Storyboard.SetTargetProperty(anim, new PropertyPath(ScaleTransform.ScaleXProperty));
            Storyboard.SetTargetProperty(anim, new PropertyPath(ScaleTransform.ScaleYProperty));
            //Storyboard.SetTargetProperty(anim, new PropertyPath(RotateTransform.CenterXProperty));

            Storyboard sb = new Storyboard();
            sb.Children.Add(anim);
            sb.Begin();
        }
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            SkewTransform transf = btn.RenderTransform as SkewTransform;

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 0;
            anim.To = 90;
            anim.Duration = new Duration(TimeSpan.FromSeconds(3));
            anim.SpeedRatio = 2.3;
            anim.AutoReverse = true;

            Storyboard.SetTarget(anim, transf);
            Storyboard.SetTargetProperty(anim, new PropertyPath(SkewTransform.AngleXProperty));
            Storyboard.SetTargetProperty(anim, new PropertyPath(SkewTransform.AngleYProperty));
            //Storyboard.SetTargetProperty(anim, new PropertyPath(SkewTransform.CenterXProperty));
            //Storyboard.SetTargetProperty(anim, new PropertyPath(SkewTransform.CenterYProperty));
            
            Storyboard sb = new Storyboard();
            sb.Children.Add(anim);
            sb.Begin();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(5000);
        }
    }
}