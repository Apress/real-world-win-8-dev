using ApressDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApressDemo.Repository
{
    public partial class ApressBooksRepository : IBookService
    {
        public async Task<ObservableCollection<ApressBook>> GetFeaturedBooks()
        {
            //ApressBookServiceClient client = new ApressBookServiceClient();
            //var result = await client.GetFeaturedBooksAsync();
            //return result;

            ObservableCollection<ApressBook> featuredBooksCollection = new ObservableCollection<ApressBook>();

            ApressBook WPMigration = new ApressBook();
            WPMigration.ApressBookISBN = "978-1-4302-3816-4";
            WPMigration.ApressBookName = "Migrating to Windows Phone";
            WPMigration.ApressBookTechnology = "Windows Phone";
            WPMigration.ApressBookAuthor = "Jesse Liberty , Jeff Blankenburg";
            WPMigration.ApressBookDescription = "This book offers everything you'll need to upgrade your existing programming knowledge and begin to develop applications for the Windows Phone.";
            WPMigration.ApressBookImageURI = "/Assets/MigratingToWP.png";
            WPMigration.ApressBookPublishedDate = new DateTime(2011, 12, 28);
            WPMigration.ApressBookUserLevel = "Intermediate";

            ApressBook WPRecipes = new ApressBook();
            WPRecipes.ApressBookISBN = "978-1-4302-4137-9";
            WPRecipes.ApressBookName = "Windows Phone Recipes";
            WPRecipes.ApressBookTechnology = "Windows Phone";
            WPRecipes.ApressBookAuthor = "Fabio Claudio Ferracchiati , Emanuele Garofalo";
            WPRecipes.ApressBookDescription = "Are you interested in smartphone development? Windows Phone 7.5 (code-named Mango) is packed with new features and functionality that make it a .NET developer's dream. This book contains extensive code samples and detailed walkthroughs that will have you writing sophisticated apps in no time!";
            WPRecipes.ApressBookImageURI = "/Assets/WPRecipes.png";
            WPRecipes.ApressBookPublishedDate = new DateTime(2011, 12, 14);
            WPRecipes.ApressBookUserLevel = "Beginner to Intermediate";

            featuredBooksCollection.Add(WPMigration);
            featuredBooksCollection.Add(WPRecipes);

            return featuredBooksCollection;
        }
    }
}
