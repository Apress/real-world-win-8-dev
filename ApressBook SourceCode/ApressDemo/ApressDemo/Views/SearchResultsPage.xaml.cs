using ApressDemo.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace ApressDemo
{
    public sealed partial class SearchResultsPage : ApressDemo.Common.LayoutAwarePage
    {
        #region "Members"

        private string searchQueryText = string.Empty;

        // Reference to Local Folder.
        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        #endregion

        #region "Constructor"

        public SearchResultsPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region "Event Handlers"

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SearchPane searchPane = SearchPane.GetForCurrentView();
            searchPane.SuggestionsRequested += searchPane_SuggestionsRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            SearchPane searchPane = SearchPane.GetForCurrentView();
            searchPane.SuggestionsRequested -= searchPane_SuggestionsRequested;
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            this.searchQueryText = navigationParameter as String;

            // Communicate results through the view model
            this.DefaultViewModel["QueryText"] = '\u201c' + searchQueryText + '\u201d';

            // Apply search filter on Book Name based on search query.
            IEnumerable<ApressBook> searchResults = from book in ((App)Application.Current).FeaturedBookListVM.FeaturedApressBooks
                                                    where book.ApressBookName.ToLower().Contains(searchQueryText.ToLower())
                                                    select book;         
                
            this.DefaultViewModel["Results"] = searchResults;

            if (searchResults.Count() > 0)
                VisualStateManager.GoToState(this, "ResultsFound", true);
            else
                VisualStateManager.GoToState(this, "NoResultsFound", true);
        }

        private void authorNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (authorNameTextBox.Text.Trim() != string.Empty)
            {
                // Apply search filter on Book Name based on search query.
                IEnumerable<ApressBook> searchResults = from book in ((App)Application.Current).FeaturedBookListVM.FeaturedApressBooks
                                                        where book.ApressBookName.ToLower().Contains(searchQueryText.ToLower())
                                                        && book.ApressBookAuthor.ToLower().Contains(authorNameTextBox.Text.Trim().ToLower())
                                                        select book;

                this.DefaultViewModel["Results"] = searchResults;

                if (searchResults.Count() > 0)
                    VisualStateManager.GoToState(this, "ResultsFound", true);
                else
                    VisualStateManager.GoToState(this, "NoResultsFound", true);
            }
        }

        private void technologyDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (technologyDropdown != null && technologyDropdown.SelectedValue.ToString() != "All")
            {
                ComboBoxItem selectedTech = (ComboBoxItem)technologyDropdown.SelectedItem;

                // Apply search filter on Book Name based on search query.
                IEnumerable<ApressBook> searchResults = from book in ((App)Application.Current).FeaturedBookListVM.FeaturedApressBooks
                                                        where book.ApressBookName.ToLower().Contains(searchQueryText.ToLower())
                                                        && book.ApressBookTechnology == selectedTech.Content.ToString()
                                                        select book;

                this.DefaultViewModel["Results"] = searchResults;

                if (searchResults.Count() > 0)
                    VisualStateManager.GoToState(this, "ResultsFound", true);
                else
                    VisualStateManager.GoToState(this, "NoResultsFound", true);
            }
        }

        private async void searchPane_SuggestionsRequested(SearchPane sender, SearchPaneSuggestionsRequestedEventArgs args)
        {
            args.Request.SearchSuggestionCollection.AppendQuerySuggestions((from book in ((App)Application.Current).FeaturedBookListVM.FeaturedApressBooks
                                                                            where book.ApressBookName.ToLower().StartsWith(args.QueryText.ToLower())
                                                                            select book.ApressBookName).Take(5));

            IEnumerable<ApressBook> recommendedBooks = from book in ((App)Application.Current).FeaturedBookListVM.FeaturedApressBooks
                                                       where book.ApressBookName.Substring(0, 5).ToLower() == args.QueryText.ToLower()
                                                       select book;

            if (recommendedBooks.Count() > 0)
            {
                ApressBook firstRecommendedBook = null;
                foreach (ApressBook book in recommendedBooks)
                {
                    firstRecommendedBook = book;
                    break;
                }

                StorageFile imageToRead = await localFolder.GetFileAsync("MigratingToWP.png");

                args.Request.SearchSuggestionCollection.AppendResultSuggestion(firstRecommendedBook.ApressBookName, firstRecommendedBook.DisplayablePublishDate, firstRecommendedBook.ApressBookISBN, RandomAccessStreamReference.CreateFromFile(imageToRead), string.Empty);
            }
        }

        #endregion
    }
}
