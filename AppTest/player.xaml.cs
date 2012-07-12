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
    public partial class player : PhoneApplicationPage
    {
        private bool _updatingMediaTimeline = false;
        public player()
        {
            InitializeComponent();
            //CompositionTarget.Rendering += new EventHandler(OnCompositionTarget_Rendering);

            MediaPlayer.Position = System.TimeSpan.FromSeconds(0);
            
            MediaPlayer.Source = new Uri("/Resource/video1.wmv", UriKind.Relative);
            MediaPlayer.AutoPlay = false;
            CompositionTarget.Rendering += (s, e) =>
            {
                _updatingMediaTimeline = true;
                TimeSpan duration = MediaPlayer.NaturalDuration.TimeSpan;
                if (duration.TotalSeconds != 0)
                {
                    double percentComplete = MediaPlayer.Position.TotalSeconds / duration.TotalSeconds;
                    //SliderBar.Value = percentComplete;
                    TimeSpan mediaTime = MediaPlayer.Position;
                    string text = string.Format("{0:00}:{1:00}", (mediaTime.Hours * 60) + mediaTime.Minutes,
                                                                  mediaTime.Seconds);
                    if (StatusBar.Text != text)
                        StatusBar.Text = text;

                    
                    _updatingMediaTimeline = false;
                }
            };
        }

        /*
        void OnCompositionTarget_Rendering(object sender, EventArgs a)
        {
            //rotate.Angle = (rotate.Angle + 2) % 360;
        }
        */
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // when this page is loaded.
            
            base.OnNavigatedTo(e);
            string msg = "";
            if (NavigationContext.QueryString.TryGetValue("SelectedItem", out msg))
            {
                textBlock1.Text = "Current Playing: "+ msg;
               //animationBlock.Text = msg;
            }
        }

        private void buttonPause_Click(object sender, RoutedEventArgs e)
        {
            if (MediaPlayer.CanPause)
            {
                MediaPlayer.Pause();
                StatusBar.Text = "Paused";
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Stop();
            MediaPlayer.Position = System.TimeSpan.FromSeconds(0);
            StatusBar.Text = "Stopped";
        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Play();
        }
    }
}