using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class cDepartment
    {
        public long DeptRowID { get; set; }
        public long DeptId { get; set; }
        public string DeptTypeId { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public string DeptCode { get; set; }
        public string DeptDescription { get; set; }
        public Nullable<bool> IsRootDept { get; set; }
        public string CostCenter { get; set; }
        public Nullable<long> Creator { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<long> Reviser { get; set; }
        public Nullable<System.DateTime> ReviserDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<long> DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public Nullable<long> DeptParentId { get; set; }
        public Nullable<long> DeptParentDeleteId { get; set; }
        public Nullable<int> SortIndex { get; set; }
        public string CompanyCode { get; set; }
        public string LC_CODE { get; set; }
        public string SL_CODE { get; set; }
        public string DeptDescriptionEN { get; set; }
        public Nullable<System.DateTime> UnActiveDate { get; set; }
    }
}