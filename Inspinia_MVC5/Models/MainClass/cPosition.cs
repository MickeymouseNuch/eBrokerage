using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class cPosition
    {
        public long PositionID { get; set; }
        public string PositionName { get; set; }
        public string PositionNameEN { get; set; }
        public string PositionCode { get; set; }
        public string PositionDetail { get; set; }
        public Nullable<long> Creator { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<long> Reviser { get; set; }
        public Nullable<System.DateTime> ReviserDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public Nullable<long> DeleteBy { get; set; }
        public Nullable<long> PositionSubParentId { get; set; }
        public string CompanyCode { get; set; }
        public string PO_CODE { get; set; }
        public string PO_RUNNO { get; set; }
        public Nullable<System.DateTime> UnActiveDate { get; set; }
    }
}