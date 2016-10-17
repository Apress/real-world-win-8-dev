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
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Devices.Sensors;
using Windows.UI.Core;


namespace ApressDemo.Views
{
    
    public sealed partial class Media : ApressDemo.Common.LayoutAwarePage
    {
        #region "Members"

        Compass compassSensor = Compass.GetDefault();
        Accelerometer accelerometerSensor = Accelerometer.GetDefault();
        Inclinometer inclinometerSensor = Inclinometer.GetDefault();
        Gyrometer gyrometerSensor = Gyrometer.GetDefault();
        LightSensor lightSensor = LightSensor.GetDefault();

        #endregion

        public Media()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            if (compassSensor != null)
                compassSensor.ReadingChanged += compassSensor_ReadingChanged;

            if (accelerometerSensor != null)
                accelerometerSensor.ReadingChanged += accelerometerSensor_ReadingChanged;

            if (inclinometerSensor != null)
                inclinometerSensor.ReadingChanged += inclinometerSensor_ReadingChanged;

            if (gyrometerSensor != null)
                gyrometerSensor.ReadingChanged += gyrometerSensor_ReadingChanged;

            if (lightSensor != null)
                lightSensor.ReadingChanged += lightSensor_ReadingChanged;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (compassSensor != null)
                compassSensor.ReadingChanged -= compassSensor_ReadingChanged;

            if (accelerometerSensor != null)
                accelerometerSensor.ReadingChanged -= accelerometerSensor_ReadingChanged;

            if (inclinometerSensor != null)
                inclinometerSensor.ReadingChanged -= inclinometerSensor_ReadingChanged;

            if (gyrometerSensor != null)
                gyrometerSensor.ReadingChanged -= gyrometerSensor_ReadingChanged;

            if (lightSensor != null)
                lightSensor.ReadingChanged -= lightSensor_ReadingChanged;
        }

        private async void playButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker mediaFilePicker = new Windows.Storage.Pickers.FileOpenPicker();
            mediaFilePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;

            mediaFilePicker.FileTypeFilter.Add(".wmv");
            mediaFilePicker.FileTypeFilter.Add(".mp4");

            StorageFile file = await mediaFilePicker.PickSingleFileAsync();
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

            mediaControl.SetSource(stream, file.ContentType);
            mediaControl.Visibility = Windows.UI.Xaml.Visibility.Visible;
            mediaControl.Play();

            //mediaControl.Source = new Uri("http://media.ch9.ms/ch9/b5a6/9b11ab16-bcc5-41b7-b727-a593951db5a6/kona_Source.wmv", UriKind.Absolute);
            //mediaControl.Play();
        }

        private async void compassSensor_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CompassReading reading = args.Reading;

                if (reading.HeadingMagneticNorth != null)
                    compassMagNorth.Text = String.Format("{0,5:0.00}", reading.HeadingMagneticNorth);

                if (reading.HeadingTrueNorth != null)
                    compassTrueNorth.Text = String.Format("{0,5:0.00}", reading.HeadingTrueNorth);
            });
        }

        private async void accelerometerSensor_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                AccelerometerReading reading = args.Reading;
               
                xAxis.Text = String.Format("{0,2:0.00}", reading.AccelerationX.ToString());
                yAxis.Text = String.Format("{0,2:0.00}", reading.AccelerationY.ToString());
                zAxis.Text = String.Format("{0,2:0.00}", reading.AccelerationZ.ToString());
            });
        }

        private async void inclinometerSensor_ReadingChanged(Inclinometer sender, InclinometerReadingChangedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                InclinometerReading reading = args.Reading;

                yawDegrees.Text = String.Format("{0,2:0.00}", reading.YawDegrees.ToString());
                pitchDegrees.Text = String.Format("{0,2:0.00}", reading.PitchDegrees.ToString());
                rollDegrees.Text = String.Format("{0,2:0.00}", reading.RollDegrees.ToString());
            });
        }

        private async void gyrometerSensor_ReadingChanged(Gyrometer sender, GyrometerReadingChangedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                GyrometerReading reading = args.Reading;

                xAxisVel.Text = String.Format("{0,2:0.00}", reading.AngularVelocityX.ToString());
                yAxisVel.Text = String.Format("{0,2:0.00}", reading.AngularVelocityY.ToString());
                zAxisVel.Text = String.Format("{0,2:0.00}", reading.AngularVelocityZ.ToString());
            });
        }

        private async void lightSensor_ReadingChanged(LightSensor sender, LightSensorReadingChangedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                LightSensorReading reading = args.Reading;

                illumination.Text = String.Format("{0,2:0.00}", reading.IlluminanceInLux.ToString());
            });
        }
    }
}
