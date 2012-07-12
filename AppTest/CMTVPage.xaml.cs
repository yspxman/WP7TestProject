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
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded +=  new RoutedEventHandler(MainPage_Loaded);
            
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.ViewModel.ReadServiceToCollection();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listbox.SelectedIndex == -1)
            {
                return;
            }
            // navigate to the new page
            ListBoxItem item = listbox.ItemContainerGenerator.ContainerFromIndex(listbox.SelectedIndex) as ListBoxItem;
            

            TextBlock bl = FindFirstElementInVisualTree<TextBlock>(item);
            if (bl != null)
            {
                NavigationService.Navigate(new Uri("/player.xaml?SelectedItem=" + bl.Text,
              UriKind.Relative));
                listbox.SelectedIndex = -1;

            }
      
        }

        private T FindFirstElementInVisualTree<T>(DependencyObject parent) where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            if (count == 0)
                return null;
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    var result = FindFirstElementInVisualTree<T>(child);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        //not valid
        private void listbox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ListBoxItem item = listbox.ItemContainerGenerator.ContainerFromIndex(1) as ListBoxItem;
            item.Foreground = new SolidColorBrush(Colors.Green);
        }

        private void setting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
     
            if (this.LbSetting.SelectedIndex == -1)
            {
                return;
            }

            // navigate to the new page
            ListBoxItem item = LbSetting.ItemContainerGenerator.ContainerFromIndex(LbSetting.SelectedIndex) as ListBoxItem;
            //ListBoxItem item = LbSetting.Items[LbSetting.SelectedIndex] as ListBoxItem;
            //item.Foreground = new SolidColorBrush(Colors.Green);

            // 这样可以吗？
            DataCollectionServiceDef c = item.DataContext as DataCollectionServiceDef;
            if (c !=null)
            {
                NavigationService.Navigate(new Uri("/Settings.xaml?SelectedItem=" + c.ID, UriKind.Relative));
                LbSetting.SelectedIndex = -1;
            }

        }

        private void FavoriteList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.LbFavorite.SelectedIndex == -1)
            {
                return;
            }
            // navigate to the new page
            ListBoxItem item = LbFavorite.ItemContainerGenerator.ContainerFromIndex(LbFavorite.SelectedIndex) as ListBoxItem;
            TextBlock bl = FindFirstElementInVisualTree<TextBlock>(item);
            if (bl != null)
            {
                NavigationService.Navigate(new Uri("/player.xaml?SelectedItem=" + bl.Text,
              UriKind.Relative));
                LbFavorite.SelectedIndex = -1;
            }
        }

        private void AppBarProgramLst_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ProgramList.xaml", UriKind.Relative));
        }

        private void AppBarPackageManagement_Click(object sender, EventArgs e)
        {

        }
    }
}