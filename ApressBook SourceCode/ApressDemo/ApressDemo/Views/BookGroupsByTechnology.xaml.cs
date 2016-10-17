using ApressDemo.Models;
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
    public sealed partial class BookGroupsByTechnology : ApressDemo.Common.LayoutAwarePage
    {
        #region "Constructor"

        public BookGroupsByTechnology()
        {
            this.InitializeComponent();
        }

        #endregion

        #region "Event Handlers"

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // Get the selected Book Group from Navigation parameter & data bind.
            GroupedApressBooks selectedGroup = (GroupedApressBooks)((App)Application.Current).FeaturedBookListVM.GroupedFeaturedApressBooks.First(X => X.ApressBookGroupName == navigationParameter);
            this.DefaultViewModel["BookGroup"] = selectedGroup;
            this.DefaultViewModel["BookItems"] = selectedGroup.BookCollection;
        }

        private void bookItem_Click(object sender, ItemClickEventArgs e)
        {
            // Figure out which Book item the user clicked on.
            var bookItem = e.ClickedItem;

            // Navigate to Book Details page & carry along selected Book's ISBN identifier.
            this.Frame.Navigate(typeof(BookDetails), ((ApressBook)bookItem).ApressBookISBN);
        }

        #endregion
    }
}
