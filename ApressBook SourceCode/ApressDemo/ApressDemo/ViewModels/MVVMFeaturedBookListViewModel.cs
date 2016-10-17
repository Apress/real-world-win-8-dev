using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using ApressDemo.Models;
using ApressDemo.Repository;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Command;

namespace ApressDemo.ViewModels
{
    public partial class MVVMFeaturedBookListViewModel :ViewModelBase
    {
        #region "Members"

        private ObservableCollection<ApressBook> featuredApressBooks;
        private ApressBook selectedBook;

        private IBookService apressBookService;

        #endregion

        #region "Properties"

        public ObservableCollection<ApressBook> MVVMFeaturedApressBooks
        {
            get
            {
                return featuredApressBooks;
            }
            set
            {
                featuredApressBooks = value;
                RaisePropertyChanged("MVVMFeaturedApressBooks");
            }
        }

        public ApressBook SelectedBook
        {
            get
            {
                return selectedBook;
            }
            set
            {
                selectedBook = value;
                RaisePropertyChanged("SelectedBook");
            }
        }

        public RelayCommand<ApressBook> BookSelectedCommand { get; set; }

        #endregion

        #region "Constructor"

        public MVVMFeaturedBookListViewModel()
        {
            apressBookService = SimpleIoc.Default.GetInstance<IBookService>();

            LoadFeaturedBooks();
            RespondToCommands();
        }

        #endregion

        #region "Methods"

        private async void LoadFeaturedBooks()
        {
            MVVMFeaturedApressBooks = await apressBookService.GetFeaturedBooks();
        }

        private void RespondToCommands()
        {
            BookSelectedCommand = new RelayCommand<ApressBook>((book) =>
                {
                    // Take some action with the user-selected book.
                    string chosenBookID = SelectedBook.ApressBookISBN;
                    string chosenBookName = selectedBook.ApressBookName;
                });
        }

        #endregion
    }
}
