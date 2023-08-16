using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class MenuList
    {
        public int MenuId { get; set; }
        public string MenuNameTH { get; set; }
        public Nullable<int> MenuParentId { get; set; }
        public string ControllerName { get; set; }
        public string VeiwName { get; set; }
        public string ReportURL { get; set; }
        public string ImageIcon { get; set; }
        public int UserId { get; set; }
        public string EmpID { get; set; }
        public Nullable<long> AppicationID { get; set; }
        public string ApplicationNameTH { get; set; }
        public Nullable<bool> IsGroup { get; set; }
        public Nullable<long> SortID { get; set; }
    }
}