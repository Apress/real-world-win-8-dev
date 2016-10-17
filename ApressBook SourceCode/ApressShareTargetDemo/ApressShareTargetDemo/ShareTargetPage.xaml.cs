using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Share Target Contract item template is documented at http://go.microsoft.com/fwlink/?LinkId=234241

namespace ApressShareTargetDemo
{
    /// <summary>
    /// This page allows other applications to share content through this application.
    /// </summary>
    public sealed partial class ShareTargetPage : ApressShareTargetDemo.Common.LayoutAwarePage
    {
        private Windows.ApplicationModel.DataTransfer.ShareTarget.ShareOperation shareOperation;

        public ShareTargetPage()
        {
            this.InitializeComponent();
        }
       
        public async void Activate(ShareTargetActivatedEventArgs args)
        {
            this.shareOperation = args.ShareOperation;

            // Plain Text.
            if (shareOperation.Data.Contains(StandardDataFormats.Text))
            {
                string text = await shareOperation.Data.GetTextAsync();
            }
            // Some URI.
            if (shareOperation.Data.Contains(StandardDataFormats.Uri))
            {
                Uri uri = await shareOperation.Data.GetUriAsync();
            }
            // Formatted HTML Content.
            if (shareOperation.Data.Contains(StandardDataFormats.Html))
            {
                string html = await shareOperation.Data.GetHtmlFormatAsync();
            }
            
            

            // Communicate metadata about the shared content through the view model
            var shareProperties = this.shareOperation.Data.Properties;
            var thumbnailImage = new BitmapImage();
            this.DefaultViewModel["Title"] = shareProperties.Title;
            this.DefaultViewModel["Description"] = shareProperties.Description;
            this.DefaultViewModel["Image"] = thumbnailImage;
            this.DefaultViewModel["Sharing"] = false;
            this.DefaultViewModel["ShowImage"] = false;
            this.DefaultViewModel["Comment"] = String.Empty;
            this.DefaultViewModel["SupportsComment"] = true;
            Window.Current.Content = this;
            Window.Current.Activate();

            // Update the shared content's thumbnail image in the background
            if (shareProperties.Thumbnail != null)
            {
                var stream = await shareProperties.Thumbnail.OpenReadAsync();
                thumbnailImage.SetSource(stream);
                this.DefaultViewModel["ShowImage"] = true;
            }
        }

        /// <summary>
        /// Invoked when the user clicks the Share button.
        /// </summary>
        /// <param name="sender">Instance of Button used to initiate sharing.</param>
        /// <param name="e">Event data describing how the button was clicked.</param>
        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            this.DefaultViewModel["Sharing"] = true;
            this.shareOperation.ReportStarted();

            // TODO: Perform work appropriate to your sharing scenario using
            //       this._shareOperation.Data, typically with additional information captured
            //       through custom user interface elements added to this page such as 
            //       this.DefaultViewModel["Comment"]

            this.shareOperation.ReportCompleted();
        }
    }
}
