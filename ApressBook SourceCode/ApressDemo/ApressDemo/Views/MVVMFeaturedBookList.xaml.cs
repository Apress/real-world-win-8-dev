using ApressDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace ApressDemo.Views
{  
    public sealed partial class MVVMFeaturedBookList : ApressDemo.Common.LayoutAwarePage
    {
        public MVVMFeaturedBookList()
        {
            this.InitializeComponent();
            
            this.featuredBookListView.ItemsSource = (this.DataContext as MVVMFeaturedBookListViewModel).MVVMFeaturedApressBooks;
        }
    }
}
