using System;
using System.Collections.Generic;
using System.Text;

namespace PrismDemo.Models
{
    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string title { get; set; }
        public string accuracy { get; set; }
        public string context { get; set; }
        public Locality locality { get; set; }
        public Region region { get; set; }
        public Country country { get; set; }
        public string place_id { get; set; }
        public string woeid { get; set; }
    }

}
