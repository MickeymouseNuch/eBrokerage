using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class Employee
    {
        public string EmpId { get; set; }
        public Nullable<long> DeptId { get; set; }
        public Nullable<long> PositionId { get; set; }
        public Nullable<long> DefaultCompanyId { get; set; }
        public string JLCode { get; set; }
        public string EmpCode { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Gender { get; set; }
        public string TelNo { get; set; }
        public Nullable<decimal> CreditLimit { get; set; }
        public Nullable<bool> IsTemp { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> ImageID { get; set; }
        public Nullable<long> Creator { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> ReviserDate { get; set; }
        public Nullable<System.DateTime> UnActiveDate { get; set; }
        public Nullable<bool> IsNewEmployee { get; set; }
    }
}