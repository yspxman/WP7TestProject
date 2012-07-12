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
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls;

namespace PanoramaAppTest
{
    public partial class Storage : PhoneApplicationPage
    {
        public Storage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            IsfFileWriteTest();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            IsfFileReadTest();
        }
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // when this page is loaded.

            base.OnNavigatedTo(e);
            string msg = "";
            if (NavigationContext.QueryString.TryGetValue("SelectedItem", out msg))
            {
                this.PageTitle.Text =  msg;
                //animationBlock.Text = msg;
            }
        }


        public void IsfFileWriteTest()
        {
            // create ISO file
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            isf.CreateDirectory("Data");
            StreamWriter sw = new StreamWriter(new IsolatedStorageFileStream("Data\\file.txt", FileMode.Create, isf));
            sw.WriteLine("test data");
            sw.Close();
        }

        public void IsfFileReadTest()
        {
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(new IsolatedStorageFileStream("Data\\file.txt", FileMode.Open, isf));
                textBlock1.Text = sr.ReadLine();
                sr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
        }

        public void IsSettingTestWrite()
        {
            IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;

            try
            {
                iss.Remove("Channel");
                iss.Remove("Channel1");
                iss.Remove("Channel2");
                iss.Remove("Channel3");
                iss.Add("Channel1", "cctv1");
                iss.Add("Channel2", "CCTV2");
                iss.Add("Channel3", "BTV");

                iss.Save();
            }
            catch
            { }
        }

        public void IsSettingTestRead()
        {
            string value = "";
            IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;
            try
            {
                setting.TryGetValue("Channel1", out value);
                MessageBox.Show("Channel1: " + value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void buttonSetting_Click(object sender, RoutedEventArgs e)
        {
            //IsSettingTestWrite();
            //IsSettingTestRead();

            //tryChannelData();
            MessageBox.Show("Saving complete!");
        }

        public void tryChannelData()
        {
            var setting = IsolatedStorageSettings.ApplicationSettings;
            Channel ch = new Channel { Name = "CCTV", Status = Channel.ChannelStatus.Free };
            if (setting.Contains("Channel"))
            {
                setting["Channel"] = ch;
            }
            else
            {
                setting.Add("Channel", ch);
            }

            setting.Save();

            // test read out
            Channel ch1;
            try
            {
                setting.TryGetValue("Channel", out ch1);
                MessageBox.Show("Channel: " + ch1.Name + " " + ch1.Status.ToString());
            }
            catch { }

        }

        // the simple channel class
        public class Channel
        {
            public enum ChannelStatus
            {
                Subscribed,
                Unsubscribed,
                Encrypted,
                Free,
                TestState
            }

            public string Name{ get; set; }
            public ChannelStatus Status { get; set; }
        }
    }
}