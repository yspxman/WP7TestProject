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

namespace AppTest
{
    public partial class Storage : PhoneApplicationPage
    {
        private DataCollectionServiceDef currentService = null;
        private List<DataCollectionServiceExtDef> serviceExts = null;
        private int curServiceExtIdx = -1;

        public Storage()
        {
            InitializeComponent();
            //DataContext = App.ViewModel;
            
        }
 
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // when this page is loaded.

            base.OnNavigatedTo(e);
            string id = "";
            if (NavigationContext.QueryString.TryGetValue("SelectedItem", out id))
            {
                
                // Initializing data collections
                var dataset = App.DBEngineInstance.QueryServiceFromID(id);
                currentService = dataset.FirstOrDefault();

                serviceExts = App.DBEngineInstance.QueryServiceExtFromID(currentService.ID);
            }

            UpdateView();
        }

        private void UpdateView()
        {
            if (currentService == null)
                return;

            this.txName.Text = this.currentService.Name;
            this.txDesp.Text = this.currentService.Description;
            this.txGenre.Text = (this.currentService.Genre == null) ? "" : this.currentService.Genre;
            this.cbFree.IsChecked = currentService.Free;
            this.ApplicationTitle.Text = this.ApplicationTitle.Text + "  ID = " + currentService.ID;
            this.PageTitle.Text = this.currentService.Name;

            if (serviceExts == null)
                return;

            App.ViewModel.ReadServiceExtToCollection(serviceExts);

            LstboxExt.ItemsSource = App.ViewModel.ServiceExtCollection;

            //LstboxExt.ItemContainerGenerator
            //LstboxExt.ItemsSource = App.ViewModel.ServiceExtCollection;  
            //LstboxExt.da
            //var d = this.LstboxExt.Items;
            //ListBoxItem item = LstboxExt.ItemContainerGenerator.ContainerFromIndex(LbSetting.SelectedIndex) as ListBoxItem;
            //LstboxExt.DataContext = curServiceExts;
            //LstboxExt.ItemsSource = curServiceExts;
            //MessageBox.Show(  d.First<string>()）；
        }

     
     
        private void LstboxExt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.LstboxExt.SelectedIndex > -1)           
            {
                this.curServiceExtIdx = LstboxExt.SelectedIndex;
            }

            if (serviceExts != null && serviceExts.Count > 0)
            {
                // update view 
                this.txExtDesp.Text = this.serviceExts[curServiceExtIdx].Description;
                this.txExtURL.Text = this.serviceExts[curServiceExtIdx].Url;
            }

            // get the selected item and its DataContext
            ListBoxItem item = LstboxExt.ItemContainerGenerator.ContainerFromIndex(LstboxExt.SelectedIndex) as ListBoxItem;
            if (item != null)
            {
                DataCollectionServiceExtDef c = item.DataContext as DataCollectionServiceExtDef;
            }
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            //1. collect service info
            currentService.Name = txName.Text;
            currentService.Description = txDesp.Text;
            currentService.Genre = txGenre.Text;
            currentService.Free = (bool)cbFree.IsChecked;
        
            //2.
            bool re = App.DBEngineInstance.UpdateServiceItem(currentService);
            string msg = re? "Update complete":"Update failed!";
            MessageBox.Show(msg);

            //3. refresh current view
            UpdateView();
        }
    }
}