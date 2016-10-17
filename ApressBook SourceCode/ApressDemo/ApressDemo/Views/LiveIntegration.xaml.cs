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

using Microsoft.Live;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;


namespace ApressDemo.Views
{  
    public sealed partial class LiveIntegration : ApressDemo.Common.LayoutAwarePage
    {
        #region "Members"

        // A popup control to display the special Settings Flyouts.
        private Popup settingsPopup;

        // Used to determine the correct height to ensure our custom UI fills the screen.
        private Rect windowBounds;

        // Desired width for the settings UI. UI guidelines specify this should be 346 or 646 depending on your needs.
        private double settingsWidth = 346;

        #endregion

        public LiveIntegration()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            windowBounds = Window.Current.Bounds;

            // Listen to Settings Pane events.
            SettingsPane.GetForCurrentView().CommandsRequested += Settings_CommandsRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            SettingsPane.GetForCurrentView().CommandsRequested -= Settings_CommandsRequested;
        }

        private void Settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            UICommandInvokedHandler accountSettingshandler = new UICommandInvokedHandler(onAccountSettingsCommand);
            SettingsCommand accountSettingsCommand = new SettingsCommand("ApressAccountSettings", "Account", accountSettingshandler);
            args.Request.ApplicationCommands.Add(accountSettingsCommand);

            UICommandInvokedHandler privacySettingshandler = new UICommandInvokedHandler(onPrivacySettingsCommand);
            SettingsCommand privacySettingsCommand = new SettingsCommand("ApressPrivacySettings", "Privacy Statement", privacySettingshandler);
            args.Request.ApplicationCommands.Add(privacySettingsCommand);
        }

        void onAccountSettingsCommand(IUICommand command)
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

            // Create an Account Settings Flyout the same dimensions as the Popup.
            AccountSettingsFlyout mypane = new AccountSettingsFlyout();
            mypane.Width = settingsWidth;
            mypane.Height = windowBounds.Height;

            // Place the SettingsFlyout inside our Popup window.
            settingsPopup.Child = mypane;

            // Let's define the location of our Popup.
            settingsPopup.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (windowBounds.Width - settingsWidth) : 0);
            settingsPopup.SetValue(Canvas.TopProperty, 0);
            settingsPopup.IsOpen = true;
        }

        void onPrivacySettingsCommand(IUICommand command)
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
            PrivacyStatementSettingsFlyout mypane = new PrivacyStatementSettingsFlyout();
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
            GreetUserByName();
            FetchUserProfilePicture();
            FetchContacts();
            FetchCalendarEvents();
            FetchSkyDriveInfo();
            FileUpload();

            Window.Current.Activated -= OnWindowActivated;
        }

        private void OnWindowActivated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                settingsPopup.IsOpen = false;
            }
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void actionText_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Windows.UI.ViewManagement.ApplicationView.Value != Windows.UI.ViewManagement.ApplicationViewState.Snapped)
            {
                SettingsPane.Show();
            }
        }

        private async void GreetUserByName()
        {
            if (((App)Application.Current).LiveSession != null)
            {
                try
                {
                    LiveConnectClient liveClient = new LiveConnectClient(((App)Application.Current).LiveSession);

                    LiveOperationResult operationResult = await liveClient.GetAsync("me");
                    dynamic result = operationResult.Result;
                    if (result != null)
                    {
                        this.userInfoText.Text = "Hello " + result.name + "!";
                        this.actionText.Text = "Sign out Now!";
                    }
                }
                catch (LiveConnectException)
                {
                    // Handle exceptions.
                }
            }
            else
            {
                this.userInfoText.Text = string.Empty;
                this.actionText.Text = "Sign in Now!";
                this.profilePicture.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private async void FetchUserProfilePicture()
        {
            try
            {
                LiveConnectClient liveClient = new LiveConnectClient(((App)Application.Current).LiveSession);
                LiveOperationResult operationResult = await liveClient.GetAsync("me/picture");
                dynamic result = operationResult.Result;

                BitmapImage image = new BitmapImage(new Uri(result.location, UriKind.Absolute));
                this.profilePicture.Source = image;
            }
            catch (LiveConnectException)
            {
                // Handle exceptions.
            }

        }

        private async void FetchContacts()
        {
            if (((App)Application.Current).LiveSession != null)
            {
                try
                {
                    LiveConnectClient liveClient = new LiveConnectClient(((App)Application.Current).LiveSession);
                    LiveOperationResult operationResult = await liveClient.GetAsync("me/contacts");

                    dynamic contactsResult = operationResult.Result;
                    List<dynamic> contactsList = contactsResult.data;

                    this.contactsList.ItemsSource = contactsList;
                    this.contactsSection.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                catch (LiveConnectException)
                {
                    // Handle exceptions.
                }
            }
            else
            {
                this.contactsSection.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private async void FetchCalendarEvents()
        {
            if (((App)Application.Current).LiveSession != null)
            {
                try
                {
                    LiveConnectClient liveClient = new LiveConnectClient(((App)Application.Current).LiveSession);
                    LiveOperationResult operationResult = await liveClient.GetAsync("me/events");

                    dynamic calendarEventsResult = operationResult.Result;
                    List<dynamic> eventsList = calendarEventsResult.data;

                    this.eventsList.ItemsSource = eventsList;
                    this.calendarSection.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                catch (LiveConnectException)
                {
                    // Handle exceptions.
                }
            }
            else
            {
                this.calendarSection.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private async void FetchSkyDriveInfo()
        {
            if (((App)Application.Current).LiveSession != null)
            {
                try
                {
                    LiveConnectClient liveClient = new LiveConnectClient(((App)Application.Current).LiveSession);
                    LiveOperationResult operationResult = await liveClient.GetAsync("me/skydrive/files");

                    dynamic skydriveContentResult = operationResult.Result;
                    List<dynamic> skydriveContentsList = skydriveContentResult.data;

                    this.skyDriveList.ItemsSource = skydriveContentsList;
                    this.skyDriveSection.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                catch (LiveConnectException)
                {
                    // Handle exceptions.
                }
            }
            else
            {
                this.skyDriveSection.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private  void FileUpload()
        {
            if (((App)Application.Current).LiveSession != null)
            {
                this.uploadSection.Visibility = Windows.UI.Xaml.Visibility.Visible;
               
            }
            else
            {
                this.uploadSection.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private async void fileUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new Windows.Storage.Pickers.FileOpenPicker();
                picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add(".png");
                Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
    
               if (file != null)
               {
                   LiveConnectClient liveClient = new LiveConnectClient(((App)Application.Current).LiveSession);
                   await liveClient.BackgroundUploadAsync("me/skydrive", "MyUploadedPicture.pnsg", file, OverwriteOption.Overwrite);                   
               }
           }            
           catch (LiveConnectException)
           {
               // Handle exceptions. 
           }
        }
    }
}
