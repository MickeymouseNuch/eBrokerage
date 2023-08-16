using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class cCompany
    {
        public long CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameEN { get; set; }
        public bool IsRequireProject { get; set; }
        public int SortIndex { get; set; }
        public Nullable<long> Creator { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<long> Reviser { get; set; }
        public Nullable<System.DateTime> ReviserDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<long> DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public Nullable<long> CompanyParentId { get; set; }
        public Nullable<long> CompanyTypeID { get; set; }
        public Nullable<long> ImageID { get; set; }
        public Nullable<bool> IsJobPost { get; set; }
        public Nullable<System.DateTime> UnActiveDate { get; set; }
        public string CompanyNameCode { get; set; }
    }
}