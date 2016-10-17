using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace ApressDemo.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class AccountPage : ApressDemo.Common.LayoutAwarePage
    {
        public AccountPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void Camera_Click(object sender, RoutedEventArgs e)
        {
            //CameraCaptureUI webCam = new CameraCaptureUI();
            //webCam.PhotoSettings.CroppedAspectRatio = new Size(16, 9);
            //StorageFile capturedPhoto = await webCam.CaptureFileAsync(CameraCaptureUIMode.Photo);

            //if (capturedPhoto != null)
            //{
            //    BitmapImage imageBitmap = new BitmapImage();
            //    using (IRandomAccessStream stream = await capturedPhoto.OpenAsync(FileAccessMode.Read))
            //    {
            //        imageBitmap.SetSource(stream);
            //    }

            //    cameraFeed.Source = imageBitmap;
            //}

            MediaCapture captureMgr = new MediaCapture();
            await captureMgr.InitializeAsync();
            videoFeed.Source = captureMgr;

            await captureMgr.StartPreviewAsync();


            //var devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            //DeviceInformation frontCamera = devices.FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);
            //DeviceInformation backCamera = devices.FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);

            //MediaCapture specificCamera = new MediaCapture();
            //await specificCamera.InitializeAsync(
            //    new MediaCaptureInitializationSettings
            //    {
            //        VideoDeviceId = frontCamera.Id
            //    });
        }
    }
}
