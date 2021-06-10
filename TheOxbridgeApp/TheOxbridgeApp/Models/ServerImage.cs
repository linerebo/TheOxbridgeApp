using System;
using System.Collections.Generic;
using System.Text;

namespace TheOxbridgeApp.Models
{
    public class ServerImage
    {
        public String Filename { get; set; }
        public String ContentType { get; set; }
        public String ImageBase64 { get; set; }
        public int ShipId_img { get; set; }
    }
}
