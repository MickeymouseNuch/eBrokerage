using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class cCostCenter
    {
        public long CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameEN { get; set; }
        public long DeptId { get; set; }
        public string DeptCode { get; set; }
        public long CostCenterID { get; set; }
        public string CostCenterCode { get; set; }
        public string CostCenterDisplay { get; set; }
    }
}