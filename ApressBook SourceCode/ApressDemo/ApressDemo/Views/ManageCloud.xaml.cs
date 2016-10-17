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

using System.Data.Services.Client;
using ApressDemo.ApressODataService;
using Microsoft.WindowsAzure.MobileServices;
using ApressDemo.Models;
using Windows.UI.Popups;


namespace ApressDemo.Views
{
    public sealed partial class ManageCloud : ApressDemo.Common.LayoutAwarePage
    {
        #region "Members"

        // Reference to Data Context & Object Collection from OData Service.
        DataServiceContext oDataContext;
        DataServiceCollection<ApressODataService.ApressBook> bookListFromService;

        private MobileServiceUser user;

        #endregion

        public ManageCloud()
        {
            this.InitializeComponent();
        }
       
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {   
            // Initialize.
            oDataContext = new DataServiceContext(new Uri("http://apressodataservice.cloudapp.net/ApressOData.svc"));
            bookListFromService = new DataServiceCollection<ApressODataService.ApressBook>(oDataContext);

            // Fetch all the entities.
            Uri query = new Uri("/ApressBooks", UriKind.Relative);

            // Custom queries.
            //var bookCatalog = new ApressBookDBEntities(new Uri("http://apressodataservice.cloudapp.net/ApressOData.svc"));
            //var customQuery = (from Book in bookCatalog.ApressBooks
            //                    where Book.ApressBookTechnology == "Windows Phone"
            //                    select Book).Take(5);  

            // Asynchronously load the fresh data from Service.
            bookListFromService.LoadAsync(query);
            // bookListFromService.LoadAsync(customQuery);

            // Completion event handler.
            bookListFromService.LoadCompleted += (sender, args) =>
            {
                 if (args.Error != null)
                 {
                     // Do appropriate error handling.
                 }
                 else
                 {
                     // Success. Bind to UI.
                     this.odataBookListView.ItemsSource = bookListFromService;
                 }
            };

            if (bookListFromService.Continuation != null)
            {
                bookListFromService.LoadNextPartialSetAsync();
            }

        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // Find the object to update from local list.
            ApressODataService.ApressBook selectedBook = bookListFromService.First(Book => Book.ApressBookISBN == "978-1-4302-3816-4");

            // Make local changes.
            selectedBook.ApressBookAuthor = "Samidip Basu";

            // Mark the object dirty & ready for update to DB.
            oDataContext.UpdateObject(selectedBook);

            // Commit all at once.
            oDataContext.BeginSaveChanges(new AsyncCallback(SaveDoneCallBack), null);   
        }

        private void SaveDoneCallBack(IAsyncResult asynchronousResult)
        {
            // Possibly on a different thread here!
            if (asynchronousResult.IsCompleted)
            {
                // Indicate completion.
                // Bubble event up to any listeners or update UI.                
            }
        }

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            await AuthenticateUser();

            ApressBookForZumo bookToInsert = new ApressBookForZumo();
            bookToInsert.ApressBookISBN = "978-1-4302-4338-0";
            bookToInsert.ApressBookName = "Ultra-Fast ASP.NET 4.5";
            bookToInsert.ApressBookTechnology = "ASP.NET";
            bookToInsert.ApressBookAuthor = "Rick Kiessig";
            bookToInsert.ApressBookPublishedDate = new DateTime(2012, 7, 25);

            bookToInsert.Channel = App.CurrentChannel.Uri;

            await App.ZumoClient.GetTable<ApressBookForZumo>().InsertAsync(bookToInsert);
        }

        private async System.Threading.Tasks.Task AuthenticateUser()
        {
            while (user == null)
            {
                string message;
                try
                {
                    user = await App.ZumoClient.LoginAsync(MobileServiceAuthenticationProvider.Twitter);
                    message = string.Format("You are now logged in as: {0}", user.UserId);
                }
                catch (InvalidOperationException)
                {
                    message = "Login Required to insert data.";
                }


                var dialog = new MessageDialog(message);
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
        }
    }
}
