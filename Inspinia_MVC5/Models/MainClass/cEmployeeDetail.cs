using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class cEmployeeDetail
    {
        public string EmpID { get; set; }
        public string TitleT { get; set; }
        public string TitleE { get; set; }
        public string DisplayName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ExtTel { get; set; }
        public int? CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyNameT { get; set; }
        public string CompanyNameE { get; set; }
        public int? MainDeptID { get; set; }
        public string MainDeptCode { get; set; }
        public string MainDeptNameT { get; set; }
        public string MainDeptNameE { get; set; }
        public int? ParentDeptID { get; set; }
        public string ParentDeptCode { get; set; }
        public string ParentDeptNameT { get; set; }
        public string ParentDeptNameE { get; set; }
        public int? PositionID { get; set; }
        public string PositionCode { get; set; }
        public string PositionNameT { get; set; }
        public string PositionNameE { get; set; }
        public string UserImage { get; set; }
        public bool? IsActive { get; set; }
        public long? UserTypeID { get; set; }
        public int? UserId { get; set; }
        public string UserLogon { get; set; }
    }
}