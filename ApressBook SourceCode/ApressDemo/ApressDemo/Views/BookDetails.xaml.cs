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
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.ApplicationModel;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using Windows.UI.StartScreen;

namespace ApressDemo.Views
{
    public sealed partial class BookDetails : ApressDemo.Common.LayoutAwarePage
    {
        #region "Members"

        DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();

        // Reference to Local Folder.
        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        #endregion

        #region "Constructor"

        public BookDetails()
        {
            this.InitializeComponent();
        }

        #endregion

        #region "Event Handlers"

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Listen in on when the user invokes the Share charm.
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(dataTransferManager_DataRequested);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // Unwire event handler.
            dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(dataTransferManager_DataRequested);
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // Allow saved page state to override the initial item to display.
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }

            // Get the selected Book Item from Navigation parameter.
            ApressBook selectedBook = (ApressBook)((App)Application.Current).FeaturedBookListVM.FeaturedApressBooks.First(X => X.ApressBookISBN == navigationParameter);

            // Also fetch the corresponding Technology Group.
            GroupedApressBooks selectedGroup = (GroupedApressBooks)((App)Application.Current).FeaturedBookListVM.GroupedFeaturedApressBooks.First(X => X.ApressBookGroupName == selectedBook.ApressBookTechnology);

            this.DefaultViewModel["BookGroup"] = selectedGroup;
            this.DefaultViewModel["BookItems"] = selectedGroup.BookCollection;
            this.flipView.SelectedItem = selectedBook;
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            var selectedItem = this.flipView.SelectedItem;
            // TODO: Derive a serializable navigation parameter and assign it to pageState["SelectedItem"]
        }

        private void dataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (this.flipView.SelectedItem != null)
            {
                ApressBook selectedBook = (ApressBook)this.flipView.SelectedItem;

                // Set what to share.

                //string textToShare = "Currently Reading Book: Name: " + selectedBook.ApressBookName + " | Author: " + selectedBook.ApressBookAuthor + " | Published: " + selectedBook.DisplayablePublishDate + ".";
                //args.Request.Data.Properties.Title = "Apress Featured BookShare:";
                //args.Request.Data.SetText(textToShare);

                //args.Request.Data.Properties.Title = "Apress Featured BookShare:";
                //args.Request.Data.SetUri(new Uri("http://www.msn.com"));

                //args.Request.Data.Properties.Title = "Apress Featured BookShare:";
                //string someHTML = "<strong>Bold Text</strong> <em>Emphasized Text</em>";
                //args.Request.Data.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(someHTML));


                // DataRequestDeferral deferral = args.Request.GetDeferral();
                //try
                //{

                //    StorageFile storageFile = await localFolder.GetFileAsync("CustomSerializedFile.xml");
                //    List<IStorageItem> storageItems = new List<IStorageItem>();
                //    storageItems.Add(storageFile);

                //    args.Request.Data.Properties.Title = "Apress Featured BookShare:";
                //    args.Request.Data.SetStorageItems(storageItems);
                //}
                //finally
                //{
                //    deferral.Complete();
                //}


                //DataRequestDeferral deferral = args.Request.GetDeferral();
                //try
                //{
                //    StorageFile imageFile = await localFolder.GetFileAsync("Logo.png");

                //    args.Request.Data.Properties.Title = "Apress Featured BookShare:";
                //    args.Request.Data.SetBitmap(RandomAccessStreamReference.CreateFromFile(imageFile));
                //}
                //finally
                //{
                //    deferral.Complete();
                //}
            }
        }

        private async void pinButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.flipView.SelectedItem != null)
            {
                ApressBook selectedBook = (ApressBook)this.flipView.SelectedItem;

                this.BottomAppBar.IsSticky = true;

                string uniqueTileID = selectedBook.ApressBookISBN;
                string shortTileName = selectedBook.ApressBookName;
                string displayTileName = selectedBook.ApressBookTechnology;
                string tileActivationArguments = uniqueTileID;
                Uri logo = new Uri("ms-appx://" + selectedBook.ApressBookImageURI);

                SecondaryTile secondaryTile = new SecondaryTile(uniqueTileID, shortTileName, displayTileName, tileActivationArguments, TileOptions.ShowNameOnLogo, logo);
                secondaryTile.ForegroundText = ForegroundText.Light;
                secondaryTile.SmallLogo = new Uri("ms-appx:///Assets/SmallLogo.png");

                FrameworkElement pinToStartButton = (FrameworkElement)pinButton;
                Windows.UI.Xaml.Media.GeneralTransform buttonTransform = pinToStartButton.TransformToVisual(null);
                Windows.Foundation.Point point = buttonTransform.TransformPoint(new Point());
                Windows.Foundation.Rect rect = new Rect(point, new Size(pinToStartButton.ActualWidth, pinToStartButton.ActualHeight));

                bool isPinned = await secondaryTile.RequestCreateForSelectionAsync(rect, Windows.UI.Popups.Placement.Above);
                this.BottomAppBar.IsSticky = false;
            }
        }

        #endregion
    }
}
