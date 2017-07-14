using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Windows.Phone.Media.Capture;

namespace Flashlight
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region Initialization

        private AudioVideoCaptureDevice _captureDevice;
        private bool _flashOn;
        private const CameraSensorLocation _sensorLocation = CameraSensorLocation.Back;

        public MainPage()
        {
            InitializeComponent();
            InitializeCaptureDevice();
        }

        #endregion

        private async void InitializeCaptureDevice()
        {
            _captureDevice = await GetCaptureDevice();
        }


        private void ToggleFlash()
        {
            
                IReadOnlyList<object> supportedCameraModes =
                    AudioVideoCaptureDevice.GetSupportedPropertyValues(_sensorLocation,
                                                                       KnownCameraAudioVideoProperties.VideoTorchMode);
                if (supportedCameraModes.ToList().Contains((UInt32)VideoTorchMode.On))
                {
                    if (!_flashOn)
                    {
                        _captureDevice.SetProperty(KnownCameraAudioVideoProperties.VideoTorchMode, VideoTorchMode.On);
                        _captureDevice.SetProperty(KnownCameraAudioVideoProperties.VideoTorchPower,
                                                   AudioVideoCaptureDevice.GetSupportedPropertyRange(_sensorLocation,
                                                                                                     KnownCameraAudioVideoProperties
                                                                                                         .VideoTorchPower)
                                                                          .Max);
                        _flashOn = true;
                    }
                    else
                    {
                        _captureDevice.SetProperty(KnownCameraAudioVideoProperties.VideoTorchMode, VideoTorchMode.Off);
                        _flashOn = false;
                    }
                }
            
        }

        private async Task<AudioVideoCaptureDevice> GetCaptureDevice()
        {
            AudioVideoCaptureDevice captureDevice =
                await
                AudioVideoCaptureDevice.OpenAsync(_sensorLocation,
                                                  AudioVideoCaptureDevice.GetAvailableCaptureResolutions(_sensorLocation)
                                                                         .First());
            return captureDevice;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {

                ToggleFlash();
            });
        }
    }
}