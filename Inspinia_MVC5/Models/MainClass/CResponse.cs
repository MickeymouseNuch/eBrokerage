using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class CResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}