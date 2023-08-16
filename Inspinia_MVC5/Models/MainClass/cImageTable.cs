using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class cImageTable
    {
        public long ImageID { get; set; }
        public string ImageCode { get; set; }
        public string ImageDetail { get; set; }
        public string ImageURL { get; set; }
        public string ImagePath { get; set; }
        public string ImageIcon { get; set; }
        public Nullable<long> ImageTypeID { get; set; }
    }
}