using PrismDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PrismDemo
{
    public sealed partial class PhotoControl : UserControl
    {
        public PhotoControl(Photo photo)
        {
            this.InitializeComponent();

            double widh = Window.Current.Bounds.Width;

            this.Width = widh / 2;
            this.Height = widh / 2;

            grid.Width = this.Width;
            grid.Height = this.Height;
            BitmapImage bitmap = new BitmapImage(new Uri(photo.source,UriKind.RelativeOrAbsolute));
            img.Source = bitmap;
            title.Text = photo.title;


        }
    }
}
