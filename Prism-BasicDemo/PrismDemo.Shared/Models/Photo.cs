using System;
using System.Collections.Generic;
using System.Text;

namespace PrismDemo.Models
{
    public class Photo
    {
        public string id { get; set; }
        public string owner { get; set; }
        public string secret { get; set; }
        public string server { get; set; }
        public int farm { get; set; }
        public string title { get; set; }
        public int ispublic { get; set; }
        public int isfriend { get; set; }
        public int isfamily { get; set; }
        public string source { get; set; }
        public double width { get; set; }
        public bool search { get; set; }
       
    }
}
