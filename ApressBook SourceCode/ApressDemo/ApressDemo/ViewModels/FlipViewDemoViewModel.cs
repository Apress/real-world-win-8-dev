using ApressDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApressDemo.ViewModels
{
    public partial class FlipViewDemoViewModel
    {
        #region "Properties"

        public ObservableCollection<ApressBook> FeaturedWindowsPhoneApressBooks { get; set; }

        #endregion

        #region "Methods"

        public void LoadFlipViewDemoVM(FlipViewDemoViewModel flipViewDemoVM)
        {
            flipViewDemoVM.FeaturedWindowsPhoneApressBooks = new ObservableCollection<Models.ApressBook>();

            ApressBook WPMigration = new ApressBook();
            WPMigration.ApressBookISBN = "978-1-4302-3816-4";
            WPMigration.ApressBookName = "Migrating to Windows Phone";
            WPMigration.ApressBookAuthor = "Jesse Liberty , Jeff Blankenburg";
            WPMigration.ApressBookDescription = "This book offers everything you'll need to upgrade your existing programming knowledge and begin to develop applications for the Windows Phone.";
            WPMigration.ApressBookImageURI = "/Assets/MigratingToWP.png";
            WPMigration.ApressBookPublishedDate = new DateTime(2011, 12, 28);
            WPMigration.ApressBookUserLevel = "Intermediate";

            ApressBook WPRecipes = new ApressBook();
            WPRecipes.ApressBookISBN = "978-1-4302-4137-9";
            WPRecipes.ApressBookName = "Windows Phone Recipes";
            WPRecipes.ApressBookAuthor = "Fabio Claudio Ferracchiati , Emanuele Garofalo";
            WPRecipes.ApressBookDescription = "Are you interested in smartphone development? Windows Phone 7.5 (code-named Mango) is packed with new features and functionality that make it a .NET developer's dream. This book contains extensive code samples and detailed walkthroughs that will have you writing sophisticated apps in no time!";
            WPRecipes.ApressBookImageURI = "/Assets/WPRecipes.png";
            WPRecipes.ApressBookPublishedDate = new DateTime(2011, 12, 14);
            WPRecipes.ApressBookUserLevel = "Beginner to Intermediate";

            ApressBook WPAppDev = new ApressBook();
            WPAppDev.ApressBookISBN = "978-1-4302-3936-9";
            WPAppDev.ApressBookName = "Windows Phone App Development";
            WPAppDev.ApressBookAuthor = "Rob Cameron";
            WPAppDev.ApressBookDescription = "Pro Windows Phone 7 Development helps you unlock the potential of Microsoft's newest mobile platform and updates—NoDo and Mango—to develop visually rich, highly functional applications for the Windows Phone Marketplace.";
            WPAppDev.ApressBookImageURI = "/Assets/WPAppDev.png";
            WPAppDev.ApressBookPublishedDate = new DateTime(2011, 12, 26);
            WPAppDev.ApressBookUserLevel = "Intermediate to Advanced";

            flipViewDemoVM.FeaturedWindowsPhoneApressBooks.Add(WPMigration);
            flipViewDemoVM.FeaturedWindowsPhoneApressBooks.Add(WPRecipes);
            flipViewDemoVM.FeaturedWindowsPhoneApressBooks.Add(WPAppDev);
        }

        #endregion
    }
}
