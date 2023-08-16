using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class cProject
    {
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNameEng { get; set; }
        public string ProjectNameTitleDeed { get; set; }
        public string ProjectType { get; set; }
        public Nullable<int> RealEstateType { get; set; }
        public string BUID { get; set; }
        public Nullable<int> SubBUID { get; set; }
        public string BrandID { get; set; }
        public string CompanyID { get; set; }
        public Nullable<int> TotalUnit { get; set; }
        public Nullable<int> TotalTitleDeed { get; set; }
        public string ProjectStatus { get; set; }
        public Nullable<System.DateTime> ProjectOpen { get; set; }
        public Nullable<System.DateTime> ProjectClose { get; set; }
        public string ProjectOwner { get; set; }
        public string ProjectTel { get; set; }
        public string ProjectFax { get; set; }
        public string ProjectEmail { get; set; }
        public string ProjectWebsite { get; set; }
        public Nullable<System.DateTime> BuildCompleteDate { get; set; }
        public Nullable<decimal> ProjectValues { get; set; }
        public Nullable<int> AreaRai { get; set; }
        public Nullable<int> Areangan { get; set; }
        public Nullable<decimal> AreaSquareWah { get; set; }
        public string Remark { get; set; }
        public Nullable<int> JuristicID { get; set; }
        public string JuristicName { get; set; }
        public string JuristicNameEng { get; set; }
        public Nullable<System.DateTime> JuristicDate { get; set; }
        public string ImgPath { get; set; }
        public Nullable<double> BudgetAlertPerc { get; set; }
        public Nullable<decimal> BudgetAlertAmt { get; set; }
        public Nullable<bool> isRenovate { get; set; }
        public Nullable<bool> isDelete { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public string BOQID { get; set; }
        public string MoFinanceNameTH { get; set; }
        public string MoFinanceNameEN { get; set; }
        public string Port { get; set; }
        public string AbProjectName { get; set; }
        public string ProjectImagePath { get; set; }
        public string SAPWBSCode { get; set; }
        public string ACCWBSCode { get; set; }
        public string COMWBSCode { get; set; }
        public string PlantCode { get; set; }
        public string SAPProfitCenter { get; set; }
        public string SAPProfixCenter { get; set; }
        public string SAPPostCenter { get; set; }
        public string SAPCostCenter { get; set; }
        public Nullable<bool> AllowSendSAP { get; set; }
        public string SAPCostCenter2 { get; set; }
        public string SAPBandCode { get; set; }
        public string SAPPlantCode { get; set; }
        public string SAPPlantCode2 { get; set; }
        public string SAPWBSCode47 { get; set; }
        public string SAPCostCenter47 { get; set; }
        public string SAPCostCenter472 { get; set; }
        public string AccountStaffName { get; set; }
        public Nullable<decimal> ProjectValues2 { get; set; }
        public string ContractType { get; set; }
        public string AccountProject { get; set; }
        public string Base64Image { get; set; }
        public Nullable<bool> isReserve { get; set; }
        public string AllocateLand { get; set; }
    }
}