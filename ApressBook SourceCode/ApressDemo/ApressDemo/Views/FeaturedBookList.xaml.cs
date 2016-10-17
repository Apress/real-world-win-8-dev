using ApressDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Media.Animation;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.ApplicationModel.Background;


namespace ApressDemo.Views
{
    public sealed partial class FeaturedBookList : ApressDemo.Common.LayoutAwarePage
    {
        #region "Members"

        private SimpleOrientationSensor _simpleorientation;
        DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();

        private Popup settingsPopup;

        // Used to determine the correct height to ensure our custom UI fills the screen.
        private Rect windowBounds;
  
        // Desired width for the settings UI. UI guidelines specify this should be 346 or 646 depending on your needs.
        private double settingsWidth = 346;

        #endregion

        #region "Constructor"

        public FeaturedBookList()
        {
            this.InitializeComponent();

            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Window.Current.SizeChanged += VisualStateChanged;

            _simpleorientation = SimpleOrientationSensor.GetDefault();

            // Assign an event handler for the sensor orientation-changed event.
            // Check to make sure the sensor is available on the device first.
            if (_simpleorientation != null)
            {
                _simpleorientation.OrientationChanged += new TypedEventHandler<SimpleOrientationSensor, SimpleOrientationSensorOrientationChangedEventArgs>(OrientationChanged);
            }
        }

        #endregion

        #region "Event Handlers"

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Application.Current.Suspending += new SuspendingEventHandler(App_Suspending);
            Application.Current.Resuming += new EventHandler<Object>(App_Resuming);

            // Listen in on when the user invokes the Share charm.
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(dataTransferManager_DataRequested);

            windowBounds = Window.Current.Bounds;

            // Listen to Settings Pane events.
            SettingsPane.GetForCurrentView().CommandsRequested += Settings_CommandsRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            Application.Current.Suspending -= App_Suspending;
            Application.Current.Resuming -= App_Resuming;

            dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(dataTransferManager_DataRequested);
            SettingsPane.GetForCurrentView().CommandsRequested -= Settings_CommandsRequested;
        }

        protected void App_Suspending(Object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            // Reference to Local Application Settings.
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            localSettings.Values["lastViewedTimeStamp"] = DateTime.Now.ToString();
        }

        protected void App_Resuming(Object sender, Object e)
        {
            // Reference to Local Application Settings.
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            string lastViewedTimeStamp = (string)localSettings.Values["lastViewedTimeStamp"];
        }
        
        protected override async void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // Set the Data-Binding context to the Grouped Book collection.
            if (((App)Application.Current).FeaturedBookListVM.FeaturedApressBooks != null)
                this.DefaultViewModel["GroupedFeaturedApressBooks"] = ((App)Application.Current).FeaturedBookListVM.GroupedFeaturedApressBooks;
            else
                // Allow for awaitable Data Binding.
                this.DefaultViewModel["GroupedFeaturedApressBooks"] = await ((App)Application.Current).FeaturedBookListVM.GetAwaitableGroupedFeaturedApressBooks();

            // Support zoomed-out semantics.
            var groupedBooks = groupedItemsViewSource.View.CollectionGroups;
            (semanticZoomer.ZoomedOutView as ListView).ItemsSource = groupedBooks;
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void GroupHeader_Clicked(object sender, RoutedEventArgs e)
        {
            // Figure out which Book Group header the user clicked on.
            var bookGroup = (sender as FrameworkElement).DataContext;

            // Navigate to Group Details page & carry along selected Group name.
            this.Frame.Navigate(typeof(BookGroupsByTechnology), ((GroupedApressBooks)bookGroup).ApressBookGroupName);
        }

        private void bookItem_Click(object sender, ItemClickEventArgs e)
        {
            // Figure out which Book item the user clicked on.
            var bookItem = e.ClickedItem;

            // Navigate to Book Details page & carry along selected Book's ISBN identifier.
            this.Frame.Navigate(typeof(BookDetails), ((ApressBook)bookItem).ApressBookISBN);
        }

        private async void OrientationChanged(object sender, SimpleOrientationSensorOrientationChangedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                SimpleOrientation orientation = args.Orientation;
                switch (orientation)
                {
                    case SimpleOrientation.NotRotated:
                        break;
                    case SimpleOrientation.Rotated90DegreesCounterclockwise:
                        break;
                    case SimpleOrientation.Rotated180DegreesCounterclockwise:
                        break;
                    case SimpleOrientation.Rotated270DegreesCounterclockwise:
                        break;
                    case SimpleOrientation.Faceup:
                        break;
                    case SimpleOrientation.Facedown:
                        break;
                    default:
                        break;
                }
            });
        }

        private void VisualStateChanged(object sender, WindowSizeChangedEventArgs e)
        {
            string visualState = DetermineVisualState(ApplicationView.Value);

            if (visualState == "Snapped")
            {             
                // Custom logic, like hiding App Bars if needed.
            }
            else
            {
                // Return to normal UI.
            }
        }

        private void dataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.FailWithDisplayText("The App cannot Share anything without a specific Book selection.");
        }

        private void Settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            UICommandInvokedHandler handler = new UICommandInvokedHandler(onSettingsCommand);
            SettingsCommand settingsCommand = new SettingsCommand("ApressSettings", "Apress Demo Settings", handler);
            args.Request.ApplicationCommands.Add(settingsCommand);
        }

        void onSettingsCommand(IUICommand command)
        {
            // Create a Popup window which will contain our flyout.
            settingsPopup = new Popup();
            settingsPopup.Closed += OnPopupClosed;
            Window.Current.Activated += OnWindowActivated;
            settingsPopup.IsLightDismissEnabled = true;
            settingsPopup.Width = settingsWidth;
            settingsPopup.Height = windowBounds.Height;
    
            // Add the proper animation for the panel.
            settingsPopup.ChildTransitions = new TransitionCollection();
            settingsPopup.ChildTransitions.Add(new PaneThemeTransition()
            {
                Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ?
                        EdgeTransitionLocation.Right :
                        EdgeTransitionLocation.Left
            });
   
            // Create a SettingsFlyout the same dimensions as the Popup.
            SettingsFlyout mypane = new SettingsFlyout();
            mypane.Width = settingsWidth;
            mypane.Height = windowBounds.Height;
    
            // Place the SettingsFlyout inside our Popup window.
            settingsPopup.Child = mypane;
    
            // Let's define the location of our Popup.
            settingsPopup.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (windowBounds.Width - settingsWidth) : 0);
            settingsPopup.SetValue(Canvas.TopProperty, 0);
            settingsPopup.IsOpen = true;
        }
    
        void OnPopupClosed(object sender, object e)
        {
           Window.Current.Activated -= OnWindowActivated;
        }
    
        private void OnWindowActivated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
           if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
           {
               settingsPopup.IsOpen = false;
           }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
           Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().Show();
        }

        private async void loadButton_Click(object sender, RoutedEventArgs e)
        {
           FileOpenPicker picker = new FileOpenPicker();
           picker.FileTypeFilter.Add(".png");
           picker.FileTypeFilter.Add(".jpg");
           picker.ViewMode = PickerViewMode.Thumbnail;
           picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
           // StorageFile pickedImageFile = await picker.PickSingleFileAsync();
           IReadOnlyList<StorageFile> selectedFiles = await picker.PickMultipleFilesAsync();
        }

        private void accountButton_Click(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(AccountPage));
        }

        private void mediaButton_Click(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(Media));
        }

        private void manageButton_Click(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(ManageCloud));
        }

        private void liveButton_Click(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(LiveIntegration));
        }

        private void chatButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SignalRChat));
        }

        private void mvvmButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MVVMFeaturedBookList));
        }

        #endregion
    }
}
