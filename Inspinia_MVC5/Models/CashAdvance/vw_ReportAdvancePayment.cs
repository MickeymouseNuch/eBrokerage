//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inspinia_MVC5.Models.CashAdvance
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_ReportAdvancePayment
    {
        public long AdvancePaymentID { get; set; }
        public string AdvancePaymentNO { get; set; }
        public Nullable<System.DateTime> DocumentDate { get; set; }
        public string CostCenter { get; set; }
        public string DisplayName { get; set; }
        public string PositionName { get; set; }
        public string DeptDescription { get; set; }
        public string ProjectName { get; set; }
        public string BudgetAccountNameTH { get; set; }
        public string BudgetModelNameTH { get; set; }
        public decimal Amount { get; set; }
        public string PaymentRemark { get; set; }
        public Nullable<System.DateTime> ReceiveCashDate { get; set; }
        public Nullable<long> PaymentTypeID { get; set; }
        public string DeptCode { get; set; }
        public string DeptofCostCode { get; set; }
    }
}
