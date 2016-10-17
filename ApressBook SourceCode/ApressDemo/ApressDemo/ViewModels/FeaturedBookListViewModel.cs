using ApressDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ApressDemo.ViewModels
{
    public partial class FeaturedApressBookListViewModel
    {
        #region "Members"

        private ObservableCollection<GroupedApressBooks> _groupedFeaturedApressBooks;

        #endregion

        #region "Properties"

        public ObservableCollection<ApressBook> FeaturedApressBooks { get; set; }

        public ObservableCollection<GroupedApressBooks> GroupedFeaturedApressBooks
        {
            get
            {
                if (_groupedFeaturedApressBooks == null)
                    _groupedFeaturedApressBooks = new ObservableCollection<GroupedApressBooks>();
    
                // Put Books into Grouped buckets based on Technology.
                var query = from individualApressBook in FeaturedApressBooks
                            orderby individualApressBook.ApressBookTechnology
                            group individualApressBook by individualApressBook.ApressBookTechnology into g
                            select new GroupedApressBooks
                           {
                               ApressBookGroupName = g.Key,
                               ApressBookGroupCount = g.Count(),
                               BookCollection = new ObservableCollection<ApressBook>(g.ToList())
                           };

                _groupedFeaturedApressBooks = new ObservableCollection<GroupedApressBooks>(query.ToList());

                return _groupedFeaturedApressBooks;
            }
            set
            {
                _groupedFeaturedApressBooks = value;
            }
        }

        #endregion

        #region "Methods"

        public void LoadFeaturedBooksDemoVM(FeaturedApressBookListViewModel featuredApressBookListVM)
        {
            featuredApressBookListVM.FeaturedApressBooks = new ObservableCollection<Models.ApressBook>();

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

            ApressBook WPAppDev = new ApressBook();
            WPAppDev.ApressBookISBN = "978-1-4302-3936-9";
            WPAppDev.ApressBookName = "Windows Phone App Development";
            WPAppDev.ApressBookTechnology = "Windows Phone";
            WPAppDev.ApressBookAuthor = "Rob Cameron";
            WPAppDev.ApressBookDescription = "Pro Windows Phone 7 Development helps you unlock the potential of Microsoft's newest mobile platform and updates—NoDo and Mango—to develop visually rich, highly functional applications for the Windows Phone Marketplace.";
            WPAppDev.ApressBookImageURI = "/Assets/WPAppDev.png";
            WPAppDev.ApressBookPublishedDate = new DateTime(2011, 12, 26);
            WPAppDev.ApressBookUserLevel = "Intermediate to Advanced";

            ApressBook AzurePlatform = new ApressBook();
            AzurePlatform.ApressBookISBN = "978-1-4302-3563-7";
            AzurePlatform.ApressBookName = "Windows Azure Platform";
            AzurePlatform.ApressBookTechnology = "Windows Azure";
            AzurePlatform.ApressBookAuthor = "Tejaswi Redkar , Tony Guidici";
            AzurePlatform.ApressBookDescription = "Learn to architect and build cloud services using the Windows Azure platform. Best practices and techniques for using Windows Azure, SQL Azure and Windows Azure AppFabric are supported by a thorough overview of architectural concepts.";
            AzurePlatform.ApressBookImageURI = "/Assets/AzurePlatform.png";
            AzurePlatform.ApressBookPublishedDate = new DateTime(2011, 12, 19);
            AzurePlatform.ApressBookUserLevel = "Intermediate to Advanced";

            ApressBook SQLAzure = new ApressBook();
            SQLAzure.ApressBookISBN = "978-1-4302-2961-2";
            SQLAzure.ApressBookName = "Pro SQL Azure";
            SQLAzure.ApressBookTechnology = "Windows Azure";
            SQLAzure.ApressBookAuthor = "Scott Klein , Herve Roggero";
            SQLAzure.ApressBookDescription = "Pro SQL Azure introduces you to Microsoft's cloud-based delivery of its enterprise-caliber, SQL Server database management system—showing you how to program and administer it in a variety of cloud computing scenarios.";
            SQLAzure.ApressBookImageURI = "/Assets/SQLAzure.png";
            SQLAzure.ApressBookPublishedDate = new DateTime(2010, 10, 29);
            SQLAzure.ApressBookUserLevel = "Intermediate to Advanced";

            ApressBook UltraFastASP = new ApressBook();
            UltraFastASP.ApressBookISBN = "978-1-4302-4338-0";
            UltraFastASP.ApressBookName = "Ultra-Fast ASP.NET 4.5";
            UltraFastASP.ApressBookTechnology = "ASP.NET";
            UltraFastASP.ApressBookAuthor = "Rick Kiessig";
            UltraFastASP.ApressBookDescription = "Ultra-Fast ASP.NET 4.5 provides a practical guide to building extremely fast and scalable web sites using ASP.NET and SQL Server, with eminently usable advice and all of the detail you need to understand the recommendations.";
            UltraFastASP.ApressBookImageURI = "/Assets/UltraFastASP.png";
            UltraFastASP.ApressBookPublishedDate = new DateTime(2012, 7, 25);
            UltraFastASP.ApressBookUserLevel = "Intermediate to Advanced";

            ApressBook ASPWebAPI = new ApressBook();
            ASPWebAPI.ApressBookISBN = "978-1-4302-4725-8";
            ASPWebAPI.ApressBookName = "Pro ASP.NET Web API";
            ASPWebAPI.ApressBookTechnology = "ASP.NET";
            ASPWebAPI.ApressBookAuthor = "Tugberk Ugurlu , Alexander Zeitler";
            ASPWebAPI.ApressBookDescription = "With the new ASP.NET Web API framework, HTTP has become a first-class citizen of .NET. Pro ASP.NET Web API shows you how to put this new technology into practice to build flexible, extensible web services that run seamlessly on a range of operating systems and devices.";
            ASPWebAPI.ApressBookImageURI = "/Assets/ASPWebAPI.png";
            ASPWebAPI.ApressBookPublishedDate = new DateTime(2013, 3, 13);
            ASPWebAPI.ApressBookUserLevel = "Intermediate to Advanced";

            featuredApressBookListVM.FeaturedApressBooks.Add(WPMigration);
            featuredApressBookListVM.FeaturedApressBooks.Add(WPRecipes);
            featuredApressBookListVM.FeaturedApressBooks.Add(WPAppDev);
            featuredApressBookListVM.FeaturedApressBooks.Add(AzurePlatform);
            featuredApressBookListVM.FeaturedApressBooks.Add(SQLAzure);
            featuredApressBookListVM.FeaturedApressBooks.Add(UltraFastASP);
            featuredApressBookListVM.FeaturedApressBooks.Add(ASPWebAPI);
        }

        public async Task<ObservableCollection<GroupedApressBooks>> GetAwaitableGroupedFeaturedApressBooks()
        {
            this.FeaturedApressBooks = await ((App)Application.Current).ReadCustomDataDeferred();

            // Put Books into Grouped buckets based on Technology.
            var query = from individualApressBook in FeaturedApressBooks
                            orderby individualApressBook.ApressBookTechnology
                            group individualApressBook by individualApressBook.ApressBookTechnology into g
                            select new GroupedApressBooks
                            {
                                ApressBookGroupName = g.Key,
                                ApressBookGroupCount = g.Count(),
                                BookCollection = new ObservableCollection<ApressBook>(g.ToList())
                            };

                if (_groupedFeaturedApressBooks == null)
                    _groupedFeaturedApressBooks = new ObservableCollection<GroupedApressBooks>();

                _groupedFeaturedApressBooks = new ObservableCollection<GroupedApressBooks>(query.ToList());

                return _groupedFeaturedApressBooks;
        }

        #endregion
    }
}
