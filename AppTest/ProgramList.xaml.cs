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
    public partial class ProgramList : PhoneApplicationPage
    {
        public ProgramList()
        {
            InitializeComponent();
            App.ViewModel.ReadServiceToCollection();
            
            CreateItems();
            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        private void CreateItems()
        {
            foreach (DataCollectionServiceDef d in App.ViewModel.ServiceCollection)
            {
                PivotItem item = new PivotItem();
                ListBox lb = new ListBox();
                lb.ItemTemplate = this.LbTemplate;

                //！！！ Listbox 绑定数据源是用itemSource 而不是 DataContext！！！！！
                lb.ItemsSource = App.ViewModel.ContentCollection;
                item.Header = d.Name;
                item.Content = lb;
                item.DataContext = d;   // assign v to the current item,

                this.PgmLstPivot.Items.Add(item);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs a)
        {
           // App.ViewModel.ReadServiceToCollection();
            // update content
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

        private void PgmLstPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
  
        }

        private void PgmLstPivot_LoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            var v = e.Item.Parent;

           // var v = e.AddedItems;
            PivotItem p = e.Item;//v[0] as PivotItem;
            if (p != null)
            {
                DataCollectionServiceDef d = p.DataContext as DataCollectionServiceDef;
                if (d != null)
                {
                    //MessageBox.Show(d.ID + " " + d.Name);
                    App.ViewModel.ReadContentsToCollectionFromServiceID(d.ID);
                    // find the listbox then assign the itemSource
                  /*
                    ListBox lb = FindFirstElementInVisualTree<ListBox>(p);
                    if (lb != null)
                        lb.ItemsSource = App.ViewModel.ContentCollection;
                   * */
                }
            }
        }
    }
}