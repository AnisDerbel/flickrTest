using Newtonsoft.Json;
using PrismDemo.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using System.Linq;

namespace PrismDemo.Services
{
  public  class ServicesPhoto : IServicesPhoto
    {
         // get recent photo from flickr api per 50 photos
        public async Task<List<Models.Photo>> GetRecentPhoto(int count)
        {
            double widh = 500;
            FlickrData flickrdata;

#if WINDOWS_PHONE_APP
             widh = Window.Current.Bounds.Width;
#endif


            HttpClient httpclient = new HttpClient();
            string url = "https://api.flickr.com/services/rest/?"+
                "method=flickr.photos.getRecent"+
                "&api_key=878e9d5d1f0e5b02bfc23a92223c720c"+
                "&format=json"+
                "&nojsoncallback=1"+
                "&page="+count+
                "&per_page=10";

            try
            {
                string flicklist = await httpclient.GetStringAsync(url);
                flickrdata = JsonConvert.DeserializeObject<FlickrData>(flicklist);
                if (flickrdata.stat == "ok")
                {
                    foreach (Photo item in flickrdata.photos.photo)
                    {
                        string photourl = "http://farm{0}.staticflickr.com/{1}/{2}_{3}_n.jpg";
                        string baseflickr = string.Format(photourl,
                            item.farm,
                            item.server,
                            item.id,
                            item.secret
                            );


                        item.source = baseflickr;

                        //size of the image should be half of the screen
                        item.width = widh / 2 - 10;
                        item.search = false;
                        Statique.listphotodesc.Add(item);


                    }

                    return flickrdata.photos.photo;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

      // search photo by an input text from the user.
        public async Task<List<Photo>> SearchPhoto(int count, string searchphoto)
        {
            double widh = 500;
            FlickrData flickrdata;

#if WINDOWS_PHONE_APP
             widh = Window.Current.Bounds.Width;
#endif
            HttpClient httpclient = new HttpClient();
            string url = "https://api.flickr.com/services/rest/?"+
            "method=flickr.photos.search"+
            "&api_key=878e9d5d1f0e5b02bfc23a92223c720c"+
            "&text=" + searchphoto + 
            "&format=json"+
            "&nojsoncallback=1"+
            "&page=" + count + 
            "&per_page=10";

            try
            {
                string flicklist = await httpclient.GetStringAsync(url);
                flickrdata = JsonConvert.DeserializeObject<FlickrData>(flicklist);
                if (flickrdata.stat == "ok")
                {
                    foreach (Photo item in flickrdata.photos.photo)
                    {
                        string photourl = "http://farm{0}.staticflickr.com/{1}/{2}_{3}_n.jpg";
                        string baseflickr = string.Format(photourl,
                            item.farm,
                            item.server,
                            item.id,
                            item.secret
                            );


                        item.source = baseflickr;
                        item.width = widh / 2 - 10;
                        item.search = true;

                       Statique.listphotodesc.Add(item);


                    }

                    return flickrdata.photos.photo;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
 


        // we check for the existing Geolocation of the photo for each swip of photo
        public async Task<Location> CheckforGeolocation(string photo_id)
        {
            GeoPhotoData flickrdata;

            try
            {
                HttpClient httpclient = new HttpClient();
                string url = "https://api.flickr.com/services/rest/?"+
                "method=flickr.photos.geo.getLocation"+
                "&api_key=878e9d5d1f0e5b02bfc23a92223c720c"+
                "&photo_id=" + photo_id + 
                "&format=json"+
                "&nojsoncallback=1";

                string flicklist = await httpclient.GetStringAsync(url);

                flickrdata = JsonConvert.DeserializeObject<GeoPhotoData>(flicklist);


                if (flickrdata.stat == "ok")
                {
                    return flickrdata.photo.location;
                }
            }
            catch (Exception)
            {

                return null;
            }

            return null;
        }
    }
}
