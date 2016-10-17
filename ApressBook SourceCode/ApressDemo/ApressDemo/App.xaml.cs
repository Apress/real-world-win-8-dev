using ApressDemo.Common;
using ApressDemo.Models;
using ApressDemo.ViewModels;
using ApressDemo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
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
using Windows.ApplicationModel.Search;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Windows.Networking.PushNotifications;
using Microsoft.WindowsAzure.MobileServices;

using Microsoft.Live;
using MC.MetroGridHelper;


namespace ApressDemo
{
    sealed partial class App : Application
    {
        #region "Global Members"

        private FlipViewDemoViewModel _flipViewDemoVM;
        private FeaturedApressBookListViewModel _featuredBookListVM;
        private LiveConnectSession _session = null;

        public static PushNotificationChannel CurrentChannel { get; private set; }
        public static MobileServiceClient ZumoClient = new MobileServiceClient("https://apresszumo.azure-mobile.net/", "pWnmnZYMQEnkguLnbOJdTdSFdjBsuV34");

        #endregion

        #region "Local Members"

        // Reference to Local Application Settings.
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
    
        // Reference to Roaming Application Settings.
        Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

        // Reference to Local Folder.
        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        #endregion

        #region "Properties"

        public FlipViewDemoViewModel FlipViewDemoVM
        {
            get { return _flipViewDemoVM; }
            set { _flipViewDemoVM = value; }
        }

        public FeaturedApressBookListViewModel FeaturedBookListVM
        {
            get { return _featuredBookListVM; }
            set { _featuredBookListVM = value; }
        }

        public LiveConnectSession LiveSession
        {
            get { return _session; }
            set {_session = value; }
        }

        #endregion

        #region "Constructor"

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;

            FlipViewDemoVM = new FlipViewDemoViewModel();
            FlipViewDemoVM.LoadFlipViewDemoVM(FlipViewDemoVM);

            FeaturedBookListVM = new FeaturedApressBookListViewModel();

            // DemoDataPersistenceCodeThroughSettings();
            // DemoDataPersistenceCodeThroughFiles();
            // SaveCustomData();
            // ReadCustomData();
            // SQLiteDemo();

            // Windows.Storage.ApplicationData.Current.DataChanged += new TypedEventHandler<ApplicationData, object>(DataChangeHandler);

            // this.BackgroundAgentRegistration();
            this.UpdateTiles();
            // this.UpdateSecondaryTile();
            this.UpdateBadge();
            // this.SendToasts();
        }

        #endregion

        #region "Event Handlers"

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            // EstablishPushNotificationChannel();

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated || args.PreviousExecutionState == ApplicationExecutionState.NotRunning) 
                {
                    // Restore the saved session state only when appropriate.
                    try
                    {
                        // Hydrate with sample data.
                        FeaturedBookListVM.LoadFeaturedBooksDemoVM(FeaturedBookListVM);

                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }
                else
                {
                    // Hydrate with sample data.
                    FeaturedBookListVM.LoadFeaturedBooksDemoVM(FeaturedBookListVM);
                }

                // MetroGridHelper.IsVisible = true;

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(FeaturedBookList)))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // Save regular settings or App level data.
            SuspensionManager.SessionState["LastViewedTimeStamp"] = DateTime.Now.ToString();
            await SuspensionManager.SaveAsync();

            // Save custom data.
            this.SaveCustomData();

            deferral.Complete();
        }

        private void OnResuming(object sender, object e)
        {
            string lastViewTimeStamp = (string)SuspensionManager.SessionState["LastViewedTimeStamp"];
        }

        private void DataChangeHandler(Windows.Storage.ApplicationData appData, object someObject)
        {
            // Refresh local data here.
        }

        protected async override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {
            // TODO: Register the Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted
            // event in OnWindowCreated to speed up searches once the application is already running

            // If the Window isn't already using Frame navigation, insert our own Frame
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            // If the app does not contain a top-level frame, it is possible that this 
            // is the initial launch of the app. Typically this method and OnLaunched 
            // in App.xaml.cs can call a common method.
            if (frame == null)
            {
                // Create a Frame to act as the navigation context and associate it with
                // a SuspensionManager key
                frame = new Frame();
                ApressDemo.Common.SuspensionManager.RegisterFrame(frame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await ApressDemo.Common.SuspensionManager.RestoreAsync();
                    }
                    catch (ApressDemo.Common.SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }
            }

            frame.Navigate(typeof(SearchResultsPage), args.QueryText);
            Window.Current.Content = frame;

            // Ensure the current window is active
            Window.Current.Activate();
        }

        #endregion

        #region "Methods"

        private void DemoDataPersistenceCodeThroughSettings()
        {
            // Persisting simple Application Settings.
            localSettings.Values["CurrentReadingBook"] = "Migrating to Windows Phone";
            roamingSettings.Values["LastPageReadOnCurrentBook"] = 20;

            // Organizing settings in containers.
            Windows.Storage.ApplicationDataContainer container = localSettings.CreateContainer("FavoriteBooks", Windows.Storage.ApplicationDataCreateDisposition.Always);
            if (localSettings.Containers.ContainsKey("FavoriteBooks"))
            {
                localSettings.Containers["FavoriteBooks"].Values["FavoriteWindowsPhoneBook"] = "Windows Phone Recipes";
            }

  
            // Reading settings back.
            string currentBook = string.Empty;
            int lastPageReadOnCurrentBook;

            if (localSettings.Values["CurrentReadingBook"] != null)
                currentBook = localSettings.Values["CurrentReadingBook"].ToString();

            if (roamingSettings.Values["LastPageReadOnCurrentBook"] != null)
                lastPageReadOnCurrentBook = Convert.ToInt16(roamingSettings.Values["LastPageReadOnCurrentBook"]);

            bool hasFavoritesContainer = localSettings.Containers.ContainsKey("FavoriteBooks");
            string favoriteWindowsPhoneBook = string.Empty;

            if (hasFavoritesContainer)
            {
                if (localSettings.Containers["FavoriteBooks"].Values.ContainsKey("FavoriteWindowsPhoneBook"))
                    favoriteWindowsPhoneBook = localSettings.Containers["FavoriteBooks"].Values["FavoriteWindowsPhoneBook"].ToString();
            }


            // Deleting Settings from Storage.
            localSettings.Values.Remove("CurrentReadingBook");
            roamingSettings.Values.Remove("LastPageReadOnCurrentBook");
            localSettings.DeleteContainer("FavoriteBooks");
        }

        private async void DemoDataPersistenceCodeThroughFiles()
        {
            StorageFile localFileToWrite = await localFolder.CreateFileAsync("FileTest.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(localFileToWrite, "This is a sample file!");

            StorageFile localFileToRead = await localFolder.GetFileAsync("FileTest.txt");
            string textRead = await FileIO.ReadTextAsync(localFileToRead);

            StorageFile localFileToDelete = await localFolder.GetFileAsync("FileTest.txt");
            await localFileToDelete.DeleteAsync();
        }

        private async void SaveCustomData()
        {
            MemoryStream customDataToSave = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<ApressBook>));
            serializer.WriteObject(customDataToSave, FeaturedBookListVM.FeaturedApressBooks);
            
                // Write serialized custom data to File on HardDisk.
                StorageFile fileToWrite = await localFolder.CreateFileAsync("CustomSerializedFile.xml", CreationCollisionOption.ReplaceExisting);
                using (Stream fileStream = await fileToWrite.OpenStreamForWriteAsync())
                {
                    customDataToSave.Seek(0, SeekOrigin.Begin);
                    await customDataToSave.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
        }

        private async void ReadCustomData()
        {
            StorageFile fileToRead = await localFolder.GetFileAsync("CustomSerializedFile.xml");
            using (IInputStream inStream = await fileToRead.OpenSequentialReadAsync())
            {
                // Read data from File & Deserialize.
                DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<ApressBook>));
                FeaturedBookListVM.FeaturedApressBooks = (ObservableCollection<ApressBook>)serializer.ReadObject(inStream.AsStreamForRead());
            }
        }

        public async Task<ObservableCollection<ApressBook>> ReadCustomDataDeferred()
        {
            StorageFile fileToRead = await localFolder.GetFileAsync("CustomSerializedFile.xml");
            using (IInputStream inStream = await fileToRead.OpenSequentialReadAsync())
            {
                // Read data from File & Deserialize.
                DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<ApressBook>));
                return (ObservableCollection<ApressBook>)serializer.ReadObject(inStream.AsStreamForRead());
            }
        }

        private void SQLiteDemo()
        {
            string sqliteDBPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "ApressBookDB.sqlite");

            using (var DB = new SQLite.SQLiteConnection(sqliteDBPath))
            {
                // Create Table from Class.
                DB.CreateTable<RelationalApressBookModel>();

                // Instantiate & add record to table.
                RelationalApressBookModel WPMigration = new RelationalApressBookModel();
                WPMigration.ApressBookISBN = "978-1-4302-3816-4";
                WPMigration.ApressBookName = "Migrating to Windows Phone";
                WPMigration.ApressBookTechnology = "Windows Phone";
                WPMigration.ApressBookAuthor = "Jesse Liberty , Jeff Blankenburg";
                WPMigration.ApressBookDescription = "This book offers everything you'll need to upgrade your existing programming knowledge and begin to develop applications for the Windows Phone.";
                WPMigration.ApressBookImageURI = "/Assets/MigratingToWP.png";
                WPMigration.ApressBookPublishedDate = new DateTime(2011, 12, 28);
                WPMigration.ApressBookUserLevel = "Intermediate";

                DB.Insert(WPMigration);


                // Read record from table using LINQ-like syntax.
                var apressBookFromDB = (DB.Table<RelationalApressBookModel>().Where(book => book.ApressBookISBN == "978-1-4302-3816-4")).Single();
            }
        }

        private void BackgroundAgentRegistration()
        {
            // Background Agent Registration.
            bool isBackgroundAgentRegistered = false;
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == "ApressBackgroundAgent")
                {
                    isBackgroundAgentRegistered = true;
                    break;
                }
            }
            if (!isBackgroundAgentRegistered)
            {
                var builder = new BackgroundTaskBuilder();

                builder.Name = "ApressBackgroundAgent";
                builder.TaskEntryPoint = "BackgroundAgentDemo.ApressBackgroundAgent";
                builder.SetTrigger(new SystemTrigger(SystemTriggerType.InternetAvailable, false));
                BackgroundTaskRegistration backgroundAgent = builder.Register();
            }
        }

        private void UpdateTiles()
        {
            //XmlDocument wideTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideText03);
            //XmlDocument squareTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText03);

            //XmlNodeList wideTileTextAttributes = wideTileXml.GetElementsByTagName("text");
            //wideTileTextAttributes[0].InnerText = "Hello Apress!";

            //XmlNodeList squareTileTextAttributes = squareTileXml.GetElementsByTagName("text");
            //squareTileTextAttributes[0].InnerText = "Line 1";
            //squareTileTextAttributes[1].InnerText = "Line 2";
            //squareTileTextAttributes[2].InnerText = "Line 3";




            XmlDocument wideTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideText09);
            XmlDocument squareTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareBlock);

            XmlNodeList wideTileTextAttributes = wideTileXml.GetElementsByTagName("text");
            wideTileTextAttributes[0].InnerText = "Hello Apress!";
            wideTileTextAttributes[1].InnerText = "This is our first Tile update";

            //var bindingElement = (XmlElement)wideTileXml.GetElementsByTagName("binding").Item(0);
            //bindingElement.SetAttribute("branding", "none");

            XmlNodeList squareTileTextAttributes = squareTileXml.GetElementsByTagName("text");
            squareTileTextAttributes[0].InnerText = "12";
            squareTileTextAttributes[1].InnerText = "February";




            //XmlDocument wideTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideImageAndText01);
            //XmlDocument squareTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareImage);

            //XmlNodeList wideTileTextAttributes = wideTileXml.GetElementsByTagName("text");
            //wideTileTextAttributes[0].InnerText = "Woot - Image + Text!";

            //XmlNodeList wideTileImageAttributes = wideTileXml.GetElementsByTagName("image");
            //((XmlElement)wideTileImageAttributes[0]).SetAttribute("src", "ms-appx:///Assets/ApressBooth.png");
            //((XmlElement)wideTileImageAttributes[0]).SetAttribute("alt", "apress booth");

            //var bindingElement = (XmlElement)wideTileXml.GetElementsByTagName("binding").Item(0);
            //bindingElement.SetAttribute("branding", "none");


            //XmlNodeList squareTileImageAttributes = squareTileXml.GetElementsByTagName("image");
            //((XmlElement)squareTileImageAttributes[0]).SetAttribute("src", "ms-appx:///Assets/HappyHolidays.png");
            //((XmlElement)squareTileImageAttributes[0]).SetAttribute("alt", "apress booth");



            //XmlDocument wideTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWidePeekImageAndText01);
            //XmlDocument squareTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquarePeekImageAndText04);

            //XmlNodeList wideTileTextAttributes = wideTileXml.GetElementsByTagName("text");
            //wideTileTextAttributes[0].InnerText = "Peek-A-Boo - Image + Text!";

            //XmlNodeList wideTileImageAttributes = wideTileXml.GetElementsByTagName("image");
            //((XmlElement)wideTileImageAttributes[0]).SetAttribute("src", "ms-appx:///Assets/ApressBooth.png");
            //((XmlElement)wideTileImageAttributes[0]).SetAttribute("alt", "apress booth");

            //var wideBindingElement = (XmlElement)wideTileXml.GetElementsByTagName("binding").Item(0);
            //wideBindingElement.SetAttribute("branding", "none");


            //XmlNodeList squareTileImageAttributes = squareTileXml.GetElementsByTagName("image");
            //((XmlElement)squareTileImageAttributes[0]).SetAttribute("src", "ms-appx:///Assets/HappyHolidays.png");
            //((XmlElement)squareTileImageAttributes[0]).SetAttribute("alt", "apress booth");

            //XmlNodeList squareTileTextAttributes = squareTileXml.GetElementsByTagName("text");
            //squareTileTextAttributes[0].InnerText = "Square Peeking Text";

            //var squareBindingElement = (XmlElement)wideTileXml.GetElementsByTagName("binding").Item(0);
            //squareBindingElement.SetAttribute("branding", "none");



            IXmlNode node = wideTileXml.ImportNode(squareTileXml.GetElementsByTagName("binding").Item(0), true);
            wideTileXml.GetElementsByTagName("visual").Item(0).AppendChild(node);
            TileNotification tileNotification = new TileNotification(wideTileXml);
            tileNotification.Tag = "PeekWideTile";

            //DateTime dueTime = DateTime.Now.AddSeconds(15);
            //ScheduledTileNotification scheduledTile = new ScheduledTileNotification(wideTileXml, dueTime);
            //TileUpdateManager.CreateTileUpdaterForApplication().AddToSchedule(scheduledTile);

            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            tileNotification.ExpirationTime = DateTimeOffset.UtcNow.AddDays(1);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);

            // TileUpdateManager.CreateTileUpdaterForApplication().Clear();
        }

        private void UpdateSecondaryTile()
        {
            XmlDocument squareTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareBlock);

            XmlNodeList squareTileTextAttributes = squareTileXml.GetElementsByTagName("text");
            squareTileTextAttributes[0].InnerText = "14";
            squareTileTextAttributes[1].InnerText = "February";

            TileNotification tileNotification = new TileNotification(squareTileXml);
            tileNotification.ExpirationTime = DateTimeOffset.UtcNow.AddDays(1);
            TileUpdater secondaryTileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile("978-1-4302-3816-4");
            secondaryTileUpdater.Update(tileNotification);
        }

        private void UpdateBadge()
        {
            XmlDocument badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);

            XmlElement badgeElement = (XmlElement)badgeXml.SelectSingleNode("/badge");
            badgeElement.SetAttribute("value", "99");

            //XmlDocument badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeGlyph);

            //XmlElement badgeElement = (XmlElement)badgeXml.SelectSingleNode("/badge");
            //badgeElement.SetAttribute("value", "attention");

            BadgeNotification badgeUpdate = new BadgeNotification(badgeXml);
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badgeUpdate);

            // BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();
        }

        private void SendToasts()
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText01);

            XmlNodeList toastTextElement = toastXml.GetElementsByTagName("text");
            toastTextElement[0].AppendChild(toastXml.CreateTextNode("First Toast: Happy Holidays from Apress!"));

            XmlNodeList toastImageAttribute = toastXml.GetElementsByTagName("image");
            ((XmlElement)toastImageAttribute[0]).SetAttribute("src", "ms-appx:///Assets/HappyHolidays.png");
            ((XmlElement)toastImageAttribute[0]).SetAttribute("alt", "apress booth");

            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "long");

            ((XmlElement)toastNode).SetAttribute("launch", "{\"type\":\"toast\",\"BookISBN\":\"978-1-4302-3816-4\"}");

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private async void EstablishPushNotificationChannel()
        {
            CurrentChannel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
        }

        public async Task AuthenticateUserThroughLive()
        {
            try
            {
                // Open Live Connect SDK client.
                LiveAuthClient LCAuth = new LiveAuthClient();
                LiveLoginResult LCLoginResult = await LCAuth.InitializeAsync();

                try
                {
                    LiveLoginResult loginResult = null;
                    loginResult = await LCAuth.LoginAsync(new string[] { "wl.signin", "wl.basic", "wl.calendars", "wl.skydrive", "wl.skydrive_update" });

                    if (loginResult.Status == LiveConnectSessionStatus.Connected)
                    {
                        this.LiveSession = loginResult.Session;
                    }
                }
                catch (LiveAuthException)
                {
                    // Handle exceptions.  
                }
            }
            catch (LiveAuthException)
            {
                // Handle exceptions. 
            }
        }

        public async Task UnAuthenticateUserThroughLive()
        {
            try
            {
                LiveAuthClient LCAuth = new LiveAuthClient();
                LiveLoginResult LCLoginResult = await LCAuth.InitializeAsync();

                // LCAuth.Logout();
                LiveSession = null;
            }
            catch (LiveAuthException)
            {
                // Handle exceptions.
            }
        }

        #endregion
    }
}