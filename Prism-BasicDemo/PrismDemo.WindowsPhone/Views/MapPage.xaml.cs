using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.StoreApps;
using PrismDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace PrismDemo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : VisualStateAwarePage,IView
    {
        public MapPage()
        {
            this.InitializeComponent();

            map1.MapServiceToken = " AiYzOMksI-ddkdcBMYlc6l5syQtbQJY4fNH2GOaTI7nDszWtPba6bNiRlNWz3Hi3";

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        /// 
        Location selectedphoto;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            await statusBar.HideAsync();

            selectedphoto = e.Parameter as Location;

            MapIcon mapIcon = new MapIcon();
            mapIcon.Title = selectedphoto.title;
            mapIcon.Location = new Geopoint(new BasicGeoposition()
            {
                Latitude = selectedphoto.latitude,
                Longitude = selectedphoto.longitude
            });

            map1.MapElements.Insert(0, mapIcon);
            BasicGeoposition basic = new BasicGeoposition() { Latitude = selectedphoto.latitude, Longitude = selectedphoto.longitude };
            MapControl.SetLocation(mapIcon, new Geopoint(basic));
            MapControl.SetNormalizedAnchorPoint(mapIcon, new Point(0.5, 0.5));
            await map1.TrySetViewAsync(mapIcon.Location, 18D, 0, 0, MapAnimationKind.Bow);  

        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
   
 
        private async void Landmarks_Checked(object sender, RoutedEventArgs e)
        {
          await  map1.TrySetViewAsync(map1.Center, map1.ZoomLevel, 340, 35, MapAnimationKind.Bow);
            map1.LandmarksVisible = true;
        }

        private async void Landmarks_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.LandmarksVisible = false;
            await map1.TrySetViewAsync(map1.Center, map1.ZoomLevel, 0, 0, MapAnimationKind.Bow);
        }

        private void Pedestrian_Checked(object sender, RoutedEventArgs e)
        {
            map1.PedestrianFeaturesVisible = true;
        }

        private void Pedestrian_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.PedestrianFeaturesVisible = false;
        }

        private void Dark_Checked(object sender, RoutedEventArgs e)
        {
            map1.ColorScheme = MapColorScheme.Dark;
        }

        private void Dark_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.ColorScheme = MapColorScheme.Light;
        }

        private void Traffic_Checked(object sender, RoutedEventArgs e)
        {
            map1.TrafficFlowVisible = true;
        }

        private void Traffic_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.TrafficFlowVisible = false;
        }

        private void TileSource_Checked(object sender, RoutedEventArgs e)
        {
            var httpsource = new HttpMapTileDataSource("http://a.tile.openstreetmap.org/{zoomlevel}/{x}/{y}.png");
            var ts = new MapTileSource(httpsource)
            {
                Layer = MapTileLayer.BackgroundReplacement
            };
            map1.Style = MapStyle.None;
            map1.TileSources.Add(ts);
        }

        private void TileSource_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.TileSources.Clear();
            map1.Style = MapStyle.Road;
        }

        private void CustTileSource_Checked(object sender, RoutedEventArgs e)
        {
            var customSource = new CustomMapTileDataSource();
            customSource.BitmapRequested += async (o, args) =>
            {
                var deferral = args.Request.GetDeferral();
                args.Request.PixelData = await CreateBitmapAsStreamAsync();
                deferral.Complete();
            };
            var ts = new MapTileSource(customSource)
            {
                Layer = MapTileLayer.BackgroundOverlay,
                IsTransparencyEnabled = true,
            };
            map1.TileSources.Add(ts);
        }


        private void CustTileSource_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.TileSources.Clear();
            map1.Style = MapStyle.Road;
        }

        private async System.Threading.Tasks.Task<IRandomAccessStreamReference> CreateBitmapAsStreamAsync()
        {
            const int pixelHeight = 256;
            const int pixelWidth = 256;
            const int bpp = 4;

            var bytes = new byte[pixelHeight * pixelWidth * bpp];

            for (var y = 0; y < pixelHeight; y++)
            {
                for (var x = 0; x < pixelWidth; x++)
                {
                    var pixelIndex = y * pixelWidth + x;
                    var byteIndex = pixelIndex * bpp;

                    if (x % 64 > 16)
                    {
                        bytes[byteIndex] = 0x20;        // Red
                        bytes[byteIndex + 1] = 0x20;    // Green
                        bytes[byteIndex + 2] = 0x80;    // Blue
                        bytes[byteIndex + 3] = 0x00;    // Alpha (0xff = fully opaque)
                    }
                    else
                    {
                        bytes[byteIndex + 3] = 0xff;    // Alpha (0xff = fully opaque)
                    }
                }
            }

            // Create RandomAccessStream from byte array
            var randomAccessStream =
                new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            var writer = new DataWriter(outputStream);
            writer.WriteBytes(bytes);
            await writer.StoreAsync();
            await writer.FlushAsync();
            return RandomAccessStreamReference.CreateFromStream(randomAccessStream);
        }

    }
}
