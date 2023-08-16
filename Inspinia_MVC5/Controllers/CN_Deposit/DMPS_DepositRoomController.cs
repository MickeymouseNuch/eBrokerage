using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Inspinia_MVC5.Models.DMPS;
using Inspinia_MVC5.API;
using Inspinia_MVC5;
using Inspinia_MVC5.Models;
using System.Data.Entity;

namespace UVG_Main.Controllers.CN_Deposit
{
    public class DMPS_DepositRoomController : Controller
    {
        PMdbEntities1 DMPS = new PMdbEntities1();
        cApiPortal cApi = new cApiPortal();
        MASDBEntities MASDB = new MASDBEntities();

        // GET: DMPS_DepositRoom
        public ActionResult Index(long _CustID = 0)

        {

            #region onload contacts
            var contact = DMPS.vw_CRM_Contract.Where(s => s.ContactsID == _CustID && s.CustomerTypeId == 1).SingleOrDefault();
            if (contact == null)
            {
                contact = new vw_CRM_Contract();
                contact.NameShortPre = "";
                contact.NameFullPre = "";
                contact.CustomerTypeNameThai = "";
                contact.CitizenID = "";
                contact.CitizenProvince = "";
                contact.PassportID = "";
                contact.Address = "";
                contact.FaxNo = "";
                contact.CountUnits = 0;
                contact.ContactsID = 0;
                contact.Email = "";
                contact.Gender = "";
                contact.BirthDate = DateTime.Now;
                contact.CustomerTypeId = 0;
                contact.CitizenIssue = DateTime.Now;
                contact.Nationality = "";
                contact.CitizenExp = DateTime.Now;
                contact.AddressNo = "";
                contact.Moo = "";
                contact.Soi = "";
                contact.Road = "";
                contact.SubDistrict = "0";
                contact.District = "0";
                contact.Province = "0";
                contact.ZipCode = "";
                contact.Tel1 = "";
                contact.Remark = "";
                contact.ContactsID = 0;
                contact.UnExpireIDCard = false;
                contact.IsForeign = false;
            }
            ViewBag.ContactDetail = contact;

            var lstBank = (from t1 in MASDB.BankTables.Where(s => s.isNonBank == false && s.isDelete == false) select new { BankID = t1.BankID, DisplayName = t1.BankID + " : " + t1.BankName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlBank = new SelectList(lstBank, "BankID", "DisplayName");

            var lstAccountType = (from t1 in MASDB.BankTypeTables.Where(s => s.IsDelete == false) select new { BankTypeID = t1.BankTypeID, DisplayName = t1.BankTypeNameTH }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlAccountType = new SelectList(lstAccountType, "BankTypeID", "DisplayName");


            var lstUnitType = (from t1 in DMPS.DMPS_UnitTypeTable.Where(s => s.IsDelete == false) select new { UnitTypeID = t1.ID, DisplayName = t1.UnitType }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlDS_UnitType = new SelectList(lstUnitType, "UnitTypeID", "DisplayName");
            ViewBag.ddlUnitType = new SelectList(lstUnitType, "UnitTypeID", "DisplayName");

            ViewBag.ddlUnitStatus = new SelectList(DMPS.UnitStatusMasterTables.OrderBy(s => s.ID).ToList(), "ID", "DescriptionTH");

            #endregion


            //ViewBag.ContactDetail = contact;

            //var qUnit = DMPS.vw_UnitsDetails.Where(s => s.DSContactID == _CustContract);
            //ViewBag.lstUnitDetails = qUnit.ToList();
            var _ProvinceID = Convert.ToInt32(contact.Province);
            var _DistinctID = Convert.ToInt32(contact.District);

            //var lstBank = (from t1 in MASDB.BankTables.Where(s => s.isNonBank == false && s.isDelete == false) select new { BankID = t1.BankID, DisplayName = t1.BankID + " : " + t1.BankName }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddlBank = new SelectList(lstBank, "BankID", "DisplayName", "BA");
            //var lstAccountType = (from t1 in MASDB.BankTypeTables.Where(s => s.IsDelete == false) select new { BankTypeID = t1.BankTypeID, DisplayName = t1.BankTypeNameTH }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddlAccountType = new SelectList(lstAccountType, "BankTypeID", "DisplayName");

            var lstEmployees = (from t1 in cApi.apiGetEmployeeDetailList().Where(s => s.IsActive == true && s.EmpID == "") select new { EmpID = t1.EmpID, DisplayName = t1.EmpID + " : " + t1.DisplayName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlInChargeBy = new SelectList(lstEmployees, "EmpID", "DisplayName");

            var lstProvice = (from t1 in MASDB.ProvinceTables.Where(s => s.PROVINCE_ID != 0) select new { ProviceID = t1.PROVINCE_ID, DisplayName = t1.PROVINCE_NAME }).OrderBy(s => s.ProviceID).ToList();
            ViewBag.ddlProvince = new SelectList(lstProvice, "ProviceID", "DisplayName", contact.Province);

            var lstCardFrom = (from t1 in MASDB.ProvinceTables.Where(s => s.PROVINCE_ID != 0) select new { ProviceID = t1.PROVINCE_ID, DisplayName = t1.PROVINCE_NAME }).OrderBy(s => s.ProviceID).ToList();
            ViewBag.dllCardFrom = new SelectList(lstCardFrom, "ProviceID", "DisplayName", contact.CitizenProvince);

            var lstDistinct = (from t1 in MASDB.AmphurTables.Where(s => s.PROVINCE_ID == _ProvinceID) select new { DistinctID = t1.AMPHUR_ID, DisplayName = t1.AMPHUR_NAME }).OrderBy(s => s.DistinctID).ToList();
            ViewBag.ddlDistrict = new SelectList(lstDistinct, "DistinctID", "DisplayName", contact.District);

            var lstSubDistinct = (from t1 in MASDB.DistrictTables.Where(s => s.PROVINCE_ID == _ProvinceID && s.AMPHUR_ID == _DistinctID) select new { SubDistinctID = t1.DISTRICT_ID, DisplayName = t1.DISTRICT_NAME }).OrderBy(s => s.SubDistinctID).ToList();
            ViewBag.ddlSubDistrict = new SelectList(lstSubDistinct, "SubDistinctID", "DisplayName", contact.SubDistrict);


            ViewBag.ddlCompany = new SelectList(cApi.apiGetCompanyList().OrderBy(s => s.CompanyName).ToList(), "CompanyId", "CompanyName");
            ViewBag.ddlDepartment = new SelectList(cApi.apiGetDepartmentList().Where(s => s.DeptTypeId != "999").OrderBy(s => s.DeptDescription).ToList(), "DeptCode", "DeptDescription");
            ViewBag.ddlPosition = new SelectList(cApi.apiGetPositionList().Where(s => s.IsDelete != true).OrderBy(s => s.PositionName).ToList(), "PositionID", "PositionName");

            //var lstDSBankDS = (from t1 in MASDB.BankTables.Where(s => s.isNonBank == false && s.isDelete == false) select new { BankID = t1.BankID, DisplayName = t1.BankID + " : " + t1.BankName }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddlDS_Bank = new SelectList(lstDSBankDS, "BankID", "DisplayName", contact.BankID);

            //var lstDSAccountTypeDS = (from t1 in MASDB.BankTypeTables.Where(s => s.IsDelete == false) select new { BankTypeID = t1.BankTypeID, DisplayName = t1.BankTypeNameTH }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddlDS_AccountType = new SelectList(lstDSAccountTypeDS, "BankTypeID", "DisplayName", contact.BankTypeID);

            ViewBag.ddlDS_Developer = new SelectList(DMPS.DevelopmentTables.OrderBy(s => s.DevelopmentName).ToList(), "DevelopmentID", "DevelopmentName");
            //var lstDeveloper = (from t1 in DMPS.DevelopmentTables.Where(s => s.IsDelete == false) select new { DeveloperID = t1.DevelopmentID, DisplayName = t1.DevelopmentName }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddlDS_Developer = new SelectList(lstDeveloper, "DeveloperID", "DisplayName");
            ViewBag.ddlDS_RentType = new SelectList(DMPS.RentTypeMasterTables.OrderBy(s => s.RentTypeID).ToList(), "RentTypeID", "RentTypeNameTH");


            ViewBag.ddlDS_Project = new SelectList(DMPS.ProjectTables.Where(s => s.IsDelete == false).OrderBy(s => s.ProjectName).ToList(), "ProjectID", "ProjectName");

            if (_CustID != 0)
            {
                var qContract = DMPS.vw_CRM_Contract.Where(s => s.ContactsID == _CustID);
                ViewBag.lstCustContract = qContract.ToList();

                var qUnit = DMPS.vw_UnitsDetails.Where(s => s.DSContactID == _CustID).OrderByDescending(s => s.ReviseDateTime);
                ViewBag.lstUnitDetails = qUnit.Where(s=> s.DepositStatus != 5 || s.DepositStatus != -1).ToList();

                var qCheckRoomMaster = DMPS.CheckRoomMasterTables;
                ViewBag.lstCheckRoomMaster = qCheckRoomMaster.OrderBy(s=> s.Sort).ToList();

                var qFacilityMaster = DMPS.FacilityMasterTables;
                ViewBag.lstFacilityMaster = qFacilityMaster.ToList();

             

            }
            else
            {
                List<vw_CRM_Contract> lsContract = new List<vw_CRM_Contract>();
                vw_CRM_Contract qContract = new vw_CRM_Contract();
                qContract.NameShortPre = "";
                qContract.NameFullPre = "";
                qContract.CustomerTypeNameThai = "";
                qContract.CitizenID = "";
                qContract.CitizenProvince = "";
                qContract.PassportID = "";
                qContract.Address = "";
                qContract.FaxNo = "";
                qContract.CountUnits = 0;
                qContract.ContactsID = 0;
                qContract.Email = "";
                qContract.Gender = "";
                qContract.BirthDate = DateTime.Now;
                qContract.CustomerTypeId = 0;
                qContract.CitizenIssue = DateTime.Now;
                qContract.Nationality = "";
                qContract.CitizenExp = DateTime.Now;
                qContract.AddressNo = "";
                qContract.Moo = "";
                qContract.Soi = "";
                qContract.Road = "";
                qContract.SubDistrict = "0";
                qContract.District = "0";
                qContract.Province = "0";
                qContract.ZipCode = "";
                qContract.Tel1 = "";
                qContract.Remark = "";
                contact.AddressNo_Work = "";
                //qContract.BankID = "";
                //qContract.BankTypeID = 0;
                //qContract.BankNo = "";
                //qContract.BankName = "";
                contact.UnExpireIDCard = false;
                contact.IsForeign = false;
                lsContract.Add(qContract);
                ViewBag.lstCustContract = lsContract;

                //ViewBag.lstCustContract = null;
                ViewBag.lstUnitDetails = null;

                var qCheckRoomMaster = DMPS.CheckRoomMasterTables;
                ViewBag.lstCheckRoomMaster = qCheckRoomMaster.OrderBy(s => s.Sort).ToList();

                var qFacilityMaster = DMPS.FacilityMasterTables;
                ViewBag.lstFacilityMaster = qFacilityMaster.ToList();

                
            };


            ViewBag.lblCustID = _CustID;

            return View();
        }

        public FileResult loadAgentDraft()
        {

            string pathSource = Server.MapPath("~/Report/eBrokerage/Daft_AgentContact.pdf");
            FileStream fsSource = new FileStream(pathSource, FileMode.Open, FileAccess.Read);

            return new FileStreamResult(fsSource, "application/pdf");
        }

        public ActionResult LoadDataListView(long id, long DS, string IsshowTab, DateTime? SDate = null, DateTime? EDate = null, bool? _chkDate = false , int _ddlUnitType = 0, string _ddlUnitStatus = "")
        {
            ViewBag.IsshowTab = IsshowTab;
            if (IsshowTab == "Cust")
            {
                var qContract = DMPS.vw_CRM_Contract.Where(s => s.ContactsID != 0 && s.CustomerTypeId == 1);

                //if (chkAdmin == "0")
                //{
                //    qWorkDocs = qWorkDocs.Where(s => s.Creator == EmployeeID || s.OwnerID == EmployeeID);
                //}

                //if (chkAdmin == "1")
                //{
                //    qWorkDocs = qWorkDocs.Where(s => s.DocumentStatusID != 0 || (s.OwnerID == EmployeeID || s.Creator == EmployeeID));
                //}

                if (id != 0)
                {
                    qContract = qContract.Where(s => s.ContactsID == id);
                }
                //if (StartDate != null && EndDate != null)
                //{
                //    qWorkDocs = qWorkDocs.Where(s => s.WorkDate >= StartDate && s.WorkDate <= EndDate);
                //}

                if (SDate != null)
                {

                    qContract = qContract.Where(s => s.ModifyDate >= SDate && s.ModifyDate <= EDate);
                }
              
                ViewBag.lstCustContract = qContract.Where(s=> s.IsDelete == false).OrderByDescending(S => S.ModifyDate).ToList();

            }

            if (IsshowTab == "Unit")
            {
                var qUnit = DMPS.vw_UnitsDetails.Where(s => s.DepositID != 0);


                if (DS != 0) {
                    qUnit = qUnit.Where(s => s.DepositID == DS);
                }

                if (_chkDate != false)
                {
                    if (_ddlUnitStatus != "Please Select")
                    {

                        if (_ddlUnitStatus != "" && _ddlUnitType == 0)
                        {
                            var query = qUnit.Where(s => s.DescriptionTH == _ddlUnitStatus);
                            if (SDate != null && EDate != null)
                            {
                                query = query.Where(s => s.DescriptionTH == _ddlUnitStatus && (s.ReviseDateTime >= SDate && s.ReviseDateTime <= EDate));

                            }

                            ViewBag.lstUnitDetails = query.OrderByDescending(s => s.ReviseDateTime);
                            return PartialView();
                        }

                        if (_ddlUnitType != 0 && _ddlUnitStatus != "")
                        {
                            var query = qUnit.Where(s => s.DescriptionTH == _ddlUnitStatus && (s.ID == _ddlUnitType));
                            if (SDate != null && EDate != null)
                            {
                                query = qUnit.Where(s => s.DescriptionTH == _ddlUnitStatus && (s.ID == _ddlUnitType) && (s.ReviseDateTime >= SDate && s.ReviseDateTime <= EDate));

                            }

                            ViewBag.lstUnitDetails = query.OrderByDescending(s => s.ReviseDateTime);
                            return PartialView();
                        }
                    }
                    if (_ddlUnitType != 0 && _ddlUnitStatus == "Please Select")
                    {
                        var query = qUnit.Where(s => s.ID == _ddlUnitType);
                        if (SDate != null && EDate != null)
                        {
                            query = qUnit.Where(s => s.ID == _ddlUnitType && (s.ReviseDateTime >= SDate && s.ReviseDateTime <= EDate));

                        }

                        ViewBag.lstUnitDetails = query.OrderByDescending(s => s.ReviseDateTime);
                        return PartialView();
                    }
                }
                else
                {
                    if (_ddlUnitStatus != "Please Select")
                    {

                        if (_ddlUnitStatus != "" && _ddlUnitType == 0)
                        {
                            var query = qUnit.Where(s => s.DescriptionTH == _ddlUnitStatus);

                            ViewBag.lstUnitDetails = query.OrderByDescending(s => s.ReviseDateTime);
                            return PartialView();
                        }

                        if (_ddlUnitType != 0 && _ddlUnitStatus != "")
                        {
                            var query = qUnit.Where(s => s.DescriptionTH == _ddlUnitStatus && (s.ID == _ddlUnitType));

                            ViewBag.lstUnitDetails = query.OrderByDescending(s => s.ReviseDateTime);
                            return PartialView();
                        }
                    }
                    if (_ddlUnitType != 0 && _ddlUnitStatus == "Please Select")
                    {
                        var query = qUnit.Where(s => s.ID == _ddlUnitType);

                        ViewBag.lstUnitDetails = query.OrderByDescending(s => s.ReviseDateTime);
                        return PartialView();
                    }
                }


                //if (SDate != null)
                //{
                //    qUnit = qUnit.Where(s => s.ReviseDateTime >= SDate && s.ReviseDateTime <= EDate);
                //}
                //if (_chkDate != false)
                //{
                //    qUnit = qUnit.Where(s => s.ID == 8);
                //    if (SDate != null )
                //    {
                //        qUnit = qUnit.Where(s => s.ID == 8 && (s.ReviseDateTime >= SDate && s.ReviseDateTime <= EDate));
                //    }
                //}
                qUnit = qUnit.OrderByDescending(s => s.ReviseDateTime);

                ViewBag.lstUnitDetails = qUnit.ToList();
            }


            return PartialView();
        }

        public ActionResult CustomerDeposit(long _CustContract = 0)// 
        {
            #region onload contacts
            var contact = DMPS.vw_CRM_Contract.Where(s => s.ContactsID == _CustContract && s.CustomerTypeId == 1).SingleOrDefault();
            if (contact == null) { contact = new vw_CRM_Contract();
                contact.NameShortPre = "";
                contact.NameFullPre = "";
                contact.CustomerTypeNameThai = "";
                contact.CitizenID = "";
                contact.CitizenProvince = "";
                contact.PassportID = "";
                contact.Address = "";
                contact.FaxNo = "";
                contact.CountUnits = 0;
                contact.ContactsID = 0;
                contact.Email = "";
                contact.Gender = "";
                contact.BirthDate = DateTime.Now;
                contact.CustomerTypeId = 0;
                contact.CitizenIssue = DateTime.Now;
                contact.Nationality = "";
                contact.CitizenExp = DateTime.Now;
                contact.AddressNo = "";
                contact.Moo = "";
                contact.Soi = "";
                contact.Road = "";
                contact.SubDistrict = "0";
                contact.District = "0";
                contact.Province = "0";
                contact.ZipCode = "";
                contact.Tel1 = "";
                contact.Remark = "";
                contact.AddressNo_Work = "";
                contact.ContactsID = 0;
                contact.UnExpireIDCard = false;
                contact.IsForeign = false;
            }
       ViewBag.ContactDetail = contact;

            var lstBank = (from t1 in MASDB.BankTables.Where(s => s.isNonBank == false && s.isDelete == false) select new { BankID = t1.BankID, DisplayName = t1.BankID + " : " + t1.BankName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlBank = new SelectList(lstBank, "BankID", "DisplayName");

            var lstAccountType = (from t1 in MASDB.BankTypeTables.Where(s => s.IsDelete == false) select new { BankTypeID = t1.BankTypeID, DisplayName = t1.BankTypeNameTH }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlAccountType = new SelectList(lstAccountType, "BankTypeID", "DisplayName");


            var lstUnitType = (from t1 in DMPS.DMPS_UnitTypeTable.Where(s => s.IsDelete == false) select new { UnitTypeID = t1.ID, DisplayName = t1.UnitType }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlDS_UnitType = new SelectList(lstUnitType, "UnitTypeID", "DisplayName");
            


            #endregion


            //ViewBag.ContactDetail = contact;

            //var qUnit = DMPS.vw_UnitsDetails.Where(s => s.DSContactID == _CustContract);
            //ViewBag.lstUnitDetails = qUnit.ToList();
            var _ProvinceID = Convert.ToInt32(contact.Province);
            var _DistinctID = Convert.ToInt32(contact.District);

            //var lstBank = (from t1 in MASDB.BankTables.Where(s => s.isNonBank == false && s.isDelete == false) select new { BankID = t1.BankID, DisplayName = t1.BankID + " : " + t1.BankName }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddlBank = new SelectList(lstBank, "BankID", "DisplayName", "BA");
            //var lstAccountType = (from t1 in MASDB.BankTypeTables.Where(s => s.IsDelete == false) select new { BankTypeID = t1.BankTypeID, DisplayName = t1.BankTypeNameTH }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddlAccountType = new SelectList(lstAccountType, "BankTypeID", "DisplayName");

            var lstEmployees = (from t1 in cApi.apiGetEmployeeDetailList().Where(s => s.IsActive == true && s.EmpID == "") select new { EmpID = t1.EmpID, DisplayName = t1.EmpID + " : " + t1.DisplayName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlInChargeBy = new SelectList(lstEmployees, "EmpID", "DisplayName");

            var lstProvice = (from t1 in MASDB.ProvinceTables.Where(s => s.PROVINCE_ID != 0) select new { ProviceID = t1.PROVINCE_ID, DisplayName = t1.PROVINCE_NAME }).OrderBy(s => s.ProviceID).ToList();
            ViewBag.ddlProvince = new SelectList(lstProvice, "ProviceID", "DisplayName", contact.Province);

            var lstCardFrom = (from t1 in MASDB.ProvinceTables.Where(s => s.PROVINCE_ID != 0) select new { ProviceID = t1.PROVINCE_ID, DisplayName = t1.PROVINCE_NAME }).OrderBy(s => s.ProviceID).ToList();
            ViewBag.dllCardFrom = new SelectList(lstCardFrom, "ProviceID", "DisplayName", contact.CitizenProvince);

            var lstDistinct = (from t1 in MASDB.AmphurTables.Where(s => s.PROVINCE_ID == _ProvinceID) select new { DistinctID = t1.AMPHUR_ID, DisplayName = t1.AMPHUR_NAME }).OrderBy(s => s.DistinctID).ToList();
            ViewBag.ddlDistrict = new SelectList(lstDistinct, "DistinctID", "DisplayName", contact.District);

            var lstSubDistinct = (from t1 in MASDB.DistrictTables.Where(s => s.PROVINCE_ID == _ProvinceID && s.AMPHUR_ID == _DistinctID) select new { SubDistinctID = t1.DISTRICT_ID, DisplayName = t1.DISTRICT_NAME }).OrderBy(s => s.SubDistinctID).ToList();
            ViewBag.ddlSubDistrict = new SelectList(lstSubDistinct, "SubDistinctID", "DisplayName", contact.SubDistrict);


            ViewBag.ddlCompany = new SelectList(cApi.apiGetCompanyList().OrderBy(s => s.CompanyName).ToList(), "CompanyId", "CompanyName");
            ViewBag.ddlDepartment = new SelectList(cApi.apiGetDepartmentList().Where(s => s.DeptTypeId != "999").OrderBy(s => s.DeptDescription).ToList(), "DeptCode", "DeptDescription");
            ViewBag.ddlPosition = new SelectList(cApi.apiGetPositionList().Where(s => s.IsDelete != true).OrderBy(s => s.PositionName).ToList(), "PositionID", "PositionName");

            //var lstDSBankDS = (from t1 in MASDB.BankTables.Where(s => s.isNonBank == false && s.isDelete == false) select new { BankID = t1.BankID, DisplayName = t1.BankID + " : " + t1.BankName }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddlDS_Bank = new SelectList(lstDSBankDS, "BankID", "DisplayName", contact.BankID);

            //var lstDSAccountTypeDS = (from t1 in MASDB.BankTypeTables.Where(s => s.IsDelete == false) select new { BankTypeID = t1.BankTypeID, DisplayName = t1.BankTypeNameTH }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddlDS_AccountType = new SelectList(lstDSAccountTypeDS, "BankTypeID", "DisplayName", contact.BankTypeID);

            ViewBag.ddlDS_Developer = new SelectList(DMPS.DevelopmentTables.OrderBy(s => s.DevelopmentName).ToList(), "DevelopmentID", "DevelopmentName");
            ViewBag.ddlDS_RentType = new SelectList(DMPS.RentTypeMasterTables.OrderBy(s => s.RentTypeID).ToList(), "RentTypeID", "RentTypeNameTH");


            ViewBag.ddlDS_Project = new SelectList(DMPS.ProjectTables.Where(s => s.IsDelete == false).OrderBy(s => s.ProjectName).ToList(), "ProjectID", "ProjectName");

            if (_CustContract != 0)
            {
                var qContract = DMPS.vw_CRM_Contract.Where(s => s.ContactsID == _CustContract);
                ViewBag.lstCustContract = qContract.ToList();

                var qUnit = DMPS.vw_UnitsDetails.Where(s => s.DSContactID == _CustContract).OrderByDescending(s => s.ReviseDateTime);
                ViewBag.lstUnitDetails = qUnit.Where(s=> s.DepositStatus != 5 && s.DepositStatus != -1).ToList();

                var qCheckRoomMaster = DMPS.CheckRoomMasterTables;
                ViewBag.lstCheckRoomMaster = qCheckRoomMaster.OrderBy(s => s.Sort).ToList();

                var qFacilityMaster = DMPS.FacilityMasterTables;
                ViewBag.lstFacilityMaster = qFacilityMaster.ToList();

                
            }
            else
            {
                List<vw_CRM_Contract> lsContract = new List<vw_CRM_Contract>();
                vw_CRM_Contract qContract = new vw_CRM_Contract();
                qContract.NameShortPre = "";
                qContract.NameFullPre = "";
                qContract.CustomerTypeNameThai = "";
                qContract.CitizenID = "";
                qContract.CitizenProvince = "";
                qContract.PassportID = "";
                qContract.Address = "";
                qContract.FaxNo = "";
                qContract.CountUnits = 0;
                qContract.ContactsID = 0;
                qContract.Email = "";
                qContract.Gender = "";
                qContract.BirthDate = DateTime.Now;
                qContract.CustomerTypeId = 0;
                qContract.CitizenIssue = DateTime.Now;
                qContract.Nationality = "";
                qContract.CitizenExp = DateTime.Now;
                qContract.AddressNo = "";
                qContract.Moo = "";
                qContract.Soi = "";
                qContract.Road = "";
                qContract.SubDistrict = "0";
                qContract.District = "0";
                qContract.Province = "0";
                qContract.ZipCode = "";
                qContract.Tel1 = "";
                qContract.Remark = "";
                contact.AddressNo_Work = "";
                //qContract.BankID = "";
                //qContract.BankTypeID = 0;
                //qContract.BankNo = "";
                //qContract.BankName = "";
                contact.UnExpireIDCard = false;
                contact.IsForeign = false;

                lsContract.Add(qContract);
                ViewBag.lstCustContract = lsContract;

                //ViewBag.lstCustContract = null;
                ViewBag.lstUnitDetails = null;

                var qCheckRoomMaster = DMPS.CheckRoomMasterTables;
                ViewBag.lstCheckRoomMaster = qCheckRoomMaster.OrderBy(s => s.Sort).ToList();

                var qFacilityMaster = DMPS.FacilityMasterTables;
                ViewBag.lstFacilityMaster = qFacilityMaster.ToList();

            };

            //var qContract = DMPS.vw_CRM_Contract.Where(s => s.ContactsID != 0);
            //ViewBag.lstCustContract = qContract.OrderBy(S => S.ContactsID).ToList();

            return View();
        }

        public string GetAdminSys(string EmpCode, int AppCode) {
            string result = "";
            var data = cApi.apiGetRoleAdminApp( EmpCode , AppCode);

            if (data == null) { result= ""; }
            else { result = data.ToObj2Json(); }
            return result;
        }

        public long CancelDeposit(string strData="",long EmpID=0,int Status =0)
        {
            var RootObjects = JsonConvert.DeserializeObject<List<ListCancelDeposit>>(strData);
            var _CustID = 0;
            foreach (var rootObject in RootObjects)
            {
                if (rootObject.CheckDel == true)
                {
                    _CustID = rootObject.CustID;
                    var _Data = DMPS.DepositTables.SingleOrDefault(s => s.DepositID == rootObject.DepositID );

                    if (_Data != null)
                    {
                        _Data.Reviser = EmpID;
                        _Data.ReviseDateTime = DateTime.Now;
                        _Data.DepositStatus = Status;
                        _Data.IsDelete = true;
                        DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                    }
                }
            }
           DMPS.SaveChanges();
    
            return _CustID;
        }

        public class FacilityTransAdd
        {
            public int FacilityID;
            public string FacilityDesc;
            public Boolean CheckFacility;
            //public string slug;
            //public string imageUrl;
        }

        public class CheckRoomTransAdd
        {
            public int CheckRoomID;
            public string CheckRoomDesc;
            public int CheckQTY;
            public Boolean CheckCheckRoom;
            //public string slug;
            //public string imageUrl;
        }

        public class ListCancelDeposit
        {
            public int UnitID;
            public int DepositID;
            public int CustID;
            public Boolean CheckDel;
            //public string slug;
            //public string imageUrl;
        }


        public string LoadEMP(string SysAdmin, string Depts, long EmpCode)
        {
            string result = string.Empty;

            //if (SysAdmin == "1")
            //{
            //    var lstEmployee = (from t1 in MASDB.Employees where t1.IsActive != false select new { EmpID = t1.EmpId, DisplayName = t1.EmpId + " : " + t1.DisplayName }).OrderBy(s => s.DisplayName).ToList();
            //    ViewBag.ddlWorkOwner = new SelectList(lstEmployee, "EmpID", "DisplayName");
            //    result = lstEmployee.ToObj2Json();
            //}
            //else
            //{

            //    //var lstEmployee = new cEmployee().getEmployeeOfDept(Depts);
            //    //ViewBag.ddlWorkOwner = new SelectList(lstEmployee, "EmpID", "DisplayName");

            //    //result = lstEmployee.ToObj2Json();

            //    var lstEmployee = new cEmployee().getEmployeeOfDept(Depts);
            //    var a = new SelectList(lstEmployee, "EmpID", "DisplayName");
            //    result = a.ToObj2Json();

            //};

            ///********************* Subject :::: Case 18/07/2019 ต้องการให้แก้ไขให้ทุกคนทำได้เหมือน Admin 
            ///                      Modify  :::: By Mr.Choknirun K.
            ///                      Date    :::: 2019-07-22 09:35

            /// 
            ////if (SysAdmin == "1")
            ////{
            ////    var lstEmployee = new cEmployee().getEmployeeOfDept();
            ////    var a = (from bb in lstEmployee select new { id = bb.EmpID, text = bb.EmpID + " : " + bb.DisplayName }).ToList();

            ////    result = a.ToObj2Json();
            ////}
            ////else
            ////{

            ////        var lstEmployee = new cEmployee().getEmployeeOfDept(Depts);
            ////        var a = (from bb in lstEmployee select new { id = bb.EmpID, text = bb.EmpID + " : " + bb.DisplayName }).ToList();
            ////        result = a.ToObj2Json();

            ////}

            var lstEmployee = cApi.apiGetEmployeeDetailList();
            var a = (from bb in lstEmployee select new { id = bb.EmpID, text = bb.EmpID + " : " + bb.DisplayName }).ToList();

            result = a.ToObj2Json();


            return result;
        }


        
        public string getDepartment(long CompanyID = 0)
        {
            string result = string.Empty;
            //var lstDepartment = (from dept in MASDB.DepartmentsTables select new { id = dept.DeptId, text = dept.DeptDescription }).ToList();
            var lstDepartment = (from dept in cApi.apiGetDepartmentList() select new { id = dept.DeptCode, text = dept.DeptDescription }).ToList();
            string aa = "";
            lstDepartment.Add(new { id = aa, text = "Please Select" });

            //var lstDepartment = (from dept in MASDB.DepartmentsTables where (dept.DeptTypeId == "4" || dept.DeptTypeId == "2") && dept.CompanyId == CompanyID select new { id = dept.DeptId, text = dept.DeptDescription }).ToList();
            //long aa = 0;
            //lstDepartment.Add(new { id = aa, text = "Please Select" });
            result = lstDepartment.ToObj2Json();
            return result;
        }


        public string getDetailEmployee(string EmployeeID = "")
        {
            string result = string.Empty;

            if (EmployeeID == "0" || EmployeeID == "") { result = "0|0|0"; }
            else
            {
                var OrganizeEmployee = cApi.apiGetEmployeeDetailList().Where(s => s.EmpID == EmployeeID).SingleOrDefault();
                if (OrganizeEmployee == null) { return "0|0|0"; }

                string _CompanyID = OrganizeEmployee.CompanyID.ToString();
                int? _DeptID = OrganizeEmployee.MainDeptID;
                string _DeptCode = OrganizeEmployee.MainDeptCode;
                string _PositionID = OrganizeEmployee.PositionID.ToString();



                result = _DeptCode.ToString() + "|" + _PositionID.ToString() + "|" + _CompanyID;
            }

            return result;
        }



        public string LoadCRMComtactData(long ContactsID)
        {
            var data = DMPS.vw_CRM_Contract.Where(s => s.ContactsID == ContactsID).SingleOrDefault();
            var result = data.ToObj2Json();
            return result;
        }

        public string getDevelopment(long _ID ) 
        {
            string result = string.Empty;
            var lstDevelopment = (from dept in DMPS.DevelopmentTables where (dept.IsDelete != true) select new { id = dept.DevelopmentID, text = dept.DevelopmentName }).ToList();
            //var lstDepartment = (from dept in MASDB.DepartmentsTables where (dept.DeptTypeId == "4" || dept.DeptTypeId == "2") && dept.CompanyId == CompanyID select new { id = dept.DeptId, text = dept.DeptDescription }).ToList();
            if (_ID != 0)
            {
                lstDevelopment = (from data in DMPS.DevelopmentTables select new { id = data.DevelopmentID, text = data.DevelopmentName }).OrderBy(data => data.text).ToList(); ;

            }
            long aa = 0;
            lstDevelopment.Add(new { id = aa, text = "Please Select" });
            result = lstDevelopment.ToObj2Json();
            return result;
        }

        public string getProject(long DeveloperID)
        {
            string result = string.Empty;
            var lstProject  = (from dept in DMPS.ProjectTables where ( dept.IsDelete != true) select new { id = dept.ProjectID, text = dept.ProjectName }).ToList(); ;
            if (DeveloperID != 0)
            {
                lstProject = (from dept in DMPS.ProjectTables where (dept.DevelopmentID == DeveloperID && dept.IsDelete != true) select new { id = dept.ProjectID, text = dept.ProjectName }).ToList(); ;

            }         
            //var lstProject = (from dept in DMPS.ProjectTables where (dept.DevelopmentID == DeveloperID && dept.IsDelete!= true  ) select new { id = dept.ProjectID, text = dept.ProjectName }).ToList();
            ////var lstDepartment = (from dept in MASDB.DepartmentsTables where (dept.DeptTypeId == "4" || dept.DeptTypeId == "2") && dept.CompanyId == CompanyID select new { id = dept.DeptId, text = dept.DeptDescription }).ToList();
            long aa = 0;
            lstProject.Add(new { id = aa, text = "Please Select" });
            result = lstProject.ToObj2Json();
            return result;
        }

        public string getUnitType(int ID = 0,int Project = 0)
        {
            string result = string.Empty;
            var lstUnitType = (from uType in DMPS.DMPS_UnitTypeTable where (uType.IsDelete != true) select new { id = uType.ID, text = uType.UnitType }).ToList(); ;

            //var lstProject = (from dept in DMPS.ProjectTables where (dept.DevelopmentID == DeveloperID && dept.IsDelete!= true  ) select new { id = dept.ProjectID, text = dept.ProjectName }).ToList();
            ////var lstDepartment = (from dept in MASDB.DepartmentsTables where (dept.DeptTypeId == "4" || dept.DeptTypeId == "2") && dept.CompanyId == CompanyID select new { id = dept.DeptId, text = dept.DeptDescription }).ToList();
            lstUnitType.Add(new { id = 0, text = "Please Select" });
            result = lstUnitType.ToObj2Json();
            return result;
        }

        public string getDistinct(long ProviceID)
        {

            string result = string.Empty;
            var lstData = (from data in MASDB.AmphurTables where (data.AMPHUR_ID != 0) select new { id = data.AMPHUR_ID, text = data.AMPHUR_NAME }).ToList(); ;
            if (ProviceID != 0)
            {
                lstData = (from data in MASDB.AmphurTables where (data.PROVINCE_ID == ProviceID) select new { id = data.AMPHUR_ID, text = data.AMPHUR_NAME }).ToList(); ;

            }
            //var lstProject = (from dept in DMPS.ProjectTables where (dept.DevelopmentID == DeveloperID && dept.IsDelete!= true  ) select new { id = dept.ProjectID, text = dept.ProjectName }).ToList();
            ////var lstDepartment = (from dept in MASDB.DepartmentsTables where (dept.DeptTypeId == "4" || dept.DeptTypeId == "2") && dept.CompanyId == CompanyID select new { id = dept.DeptId, text = dept.DeptDescription }).ToList();
            //lstData.Add(new { id = 0, text = "Please Select" });
            lstData.Insert(0, new { id = 0, text = "Please Select" });
            result = lstData.ToObj2Json();
            return result;
        }

        public string getSubDistinct(long ProviceID,long DistinctID)
        {

            string result = string.Empty;
            var lstData = (from data in MASDB.DistrictTables where (data.DISTRICT_ID != 0) select new { id = data.DISTRICT_ID, text = data.DISTRICT_NAME }).ToList(); ;
            if (ProviceID != 0)
            {
                lstData = (from data in MASDB.DistrictTables where (data.PROVINCE_ID == ProviceID && data.AMPHUR_ID == DistinctID ) select new { id = data.DISTRICT_ID, text = data.DISTRICT_NAME }).ToList(); ;

            }
            //var lstProject = (from dept in DMPS.ProjectTables where (dept.DevelopmentID == DeveloperID && dept.IsDelete!= true  ) select new { id = dept.ProjectID, text = dept.ProjectName }).ToList();
            ////var lstDepartment = (from dept in MASDB.DepartmentsTables where (dept.DeptTypeId == "4" || dept.DeptTypeId == "2") && dept.CompanyId == CompanyID select new { id = dept.DeptId, text = dept.DeptDescription }).ToList();
            //lstData.Add(new { id = 0, text = "Please Select" });
            lstData.Insert(0,new { id = 0, text = "Please Select" });
            result = lstData.ToObj2Json();
            return result;
        }
         public string SetZipCode(string _Province, string _Distinct, string _SubDistinct)
        {
            if (_Province == "กรุงเทพมหานคร")
            {
                var lstDataZipCode = DMPS.MasZipCodes.Where(s => s.ProvinceThai == _Province && s.DistrictThai == _Distinct && s.TambonThaiShort.Contains(_SubDistinct)).Select(s => s.PostCodeMain);
                return lstDataZipCode.ToObj2Json();
            }
            if (_Province != "กรุงเทพมหานคร")
            {
                var lstDataZipCode = DMPS.MasZipCodes.Where(s => s.ProvinceThai == _Province && s.DistrictThaiShort == _Distinct && s.TambonThaiShort.Contains(_SubDistinct)).Select(s => s.PostCodeMain);
                return lstDataZipCode.ToObj2Json();

            }

            return "false";
        }

        public string GetCustomerDeposit( long ID , long CustType)
        {
            var lstCustomer = DMPS.CRM_Contacts.Where(s => s.ContactsID == ID && s.CustomerTypeId == CustType && s.IsDelete == false).SingleOrDefault();
                   
            var result = lstCustomer.ToObj2Json();
            return result;
        }

        public long SaveDeveloper( DevelopmentTable _Developments)
        {
            string result = string.Empty;

            var _Data = DMPS.DevelopmentTables.SingleOrDefault(s => s.DevelopmentID == _Developments.DevelopmentID);

            if (_Data == null)
            {
                _Data = new DevelopmentTable();
                _Data.DevelopmentCode = _Developments.DevelopmentCode;
                //_Data.CheckType = _CheckRoomTrans.CheckType;
                _Data.DevelopmentName = _Developments.DevelopmentName;
                _Data.DevelopmentStatus = _Developments.DevelopmentStatus;
                _Data.Creator = _Developments.Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = _Developments.Creator;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.IsDelete = false;
                DMPS.DevelopmentTables.Add(_Data);
                DMPS.SaveChanges();
            }
            else
            {
                _Data.DevelopmentCode = _Developments.DevelopmentCode;
                //_Data.CheckType = _CheckRoomTrans.CheckType;
                _Data.DevelopmentName = _Developments.DevelopmentName;
                _Data.DevelopmentStatus = _Developments.DevelopmentStatus;
                _Data.Reviser = _Developments.Creator;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.IsDelete = _Developments.IsDelete;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();

            }

            return _Data.DevelopmentID;
        }


        public long SaveProject(ProjectTable _Project)
        {
            string result = string.Empty;

            var _Data = DMPS.ProjectTables.SingleOrDefault(s => s.ProjectID == _Project.ProjectID && s.DevelopmentID == _Project.DevelopmentID);

            if (_Data == null)
            {
                _Data = new ProjectTable();
                _Data.ProjectCode = _Project.ProjectCode;
                _Data.DevelopmentID = _Project.DevelopmentID;
                _Data.ProjectName = _Project.ProjectName;
                _Data.Creator = _Project.Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = _Project.Creator;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.IsDelete = false;
                DMPS.ProjectTables.Add(_Data);
                DMPS.SaveChanges();
            }
            else
            {
                _Data.ProjectCode = _Project.ProjectCode;
                _Data.DevelopmentID = _Project.DevelopmentID;
                _Data.ProjectName = _Project.ProjectName;
                _Data.Reviser = _Project.Creator;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.IsDelete = _Project.IsDelete;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();

            }

            return _Data.ProjectID;
        }

        public long SaveUnitType(DMPS_UnitTypeTable _UnitType)
        {
            string result = string.Empty;

            var _Data = DMPS.DMPS_UnitTypeTable.SingleOrDefault(s => s.UnitType.Replace(" ","").ToUpper() == _UnitType.UnitType.Replace(" ","").ToUpper());

            if (_Data == null)
            {
                _Data = new DMPS_UnitTypeTable();
                _Data.UnitType = _UnitType.UnitType.Replace(" ", "").ToUpper(); 
                _Data.Creator = _UnitType.Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = _UnitType.Creator;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.IsDelete = false;
                DMPS.DMPS_UnitTypeTable.Add(_Data);
                DMPS.SaveChanges();
            }
            else
            {
                _Data.UnitType = _UnitType.UnitType.Replace(" ", "").ToUpper();
                _Data.Reviser = _UnitType.Creator;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();

            }

            return _Data.ID;
        }

        public string CancelAllData(long ContactsID =0, bool IsDelete = false ,long Reviser = 0, int DepositStatus = 0, string _strDeposit = "" )
        {
            var _dataContact = DMPS.CRM_Contacts.Where(s => s.ContactsID == ContactsID).SingleOrDefault();
            if (_dataContact != null)
            {
                _dataContact.IsDelete = IsDelete;
                _dataContact.ModifyBy = Reviser.ToString() ;
                _dataContact.ModifyDate = DateTime.Now;
                DMPS.Entry(_dataContact).State = System.Data.Entity.EntityState.Modified;
            }

            var _dataContactBank = DMPS.ContactBankTables.Where(s => s.ContactsID == ContactsID).SingleOrDefault();
            if (_dataContactBank != null)
            {
                _dataContactBank.IsDelete = IsDelete;
                _dataContactBank.Reviser = Reviser;
                _dataContactBank.ReviseDateTime = DateTime.Now;
                DMPS.Entry(_dataContactBank).State = System.Data.Entity.EntityState.Modified;
            }

            var RootObjects = JsonConvert.DeserializeObject<List<ListCancelDeposit>>(_strDeposit);
            var _CustID = 0;
            foreach (var rootObject in RootObjects)
            {
               
                    _CustID = rootObject.CustID;
                    var _Data = DMPS.DepositTables.SingleOrDefault(s => s.DepositID == rootObject.DepositID);

                    if (_Data != null)
                    {
                        _Data.Reviser = Reviser;
                        _Data.ReviseDateTime = DateTime.Now;
                        _Data.DepositStatus = DepositStatus;
                        _Data.IsDelete = IsDelete;
                        DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                    }
                
            }

            DMPS.SaveChanges();
            return "";
        }

        public string LoadAddNewUnit(int _CustContract = 0 , int CustType = 0)
        {
            var data = DMPS.vw_CRM_Contract.Where(s => s.ContactsID == _CustContract && s.CustomerTypeId == CustType).SingleOrDefault();
            var result = data.ToObj2Json();
            return result;
        }

        //public string SaveDeposit(CRM_Contacts _Contacts, DepositTable _Deposit)
        public string SaveDeposit(string strFac,
                                string strCheckRoom,
                                int EmpID,
                                //string CustomerName,
                                //int Age,
                                //string IDCard,
                                //DateTime IDCardDateIsuse,
                                //DateTime IDCardDateExpire,
                                //string Nationality,
                                //string CardProvice,
                                //string Address,
                                //string Moo,
                                //string Soi,
                                //string Road,
                                //string SubDistrict,
                                //string District,
                                //string Province,
                                //string ZipCode,
                                //string Tel,
                                //string BankName,
                                //string AccountType,
                                //string AccountNo,
                                //string AccountName,
                                string RentType,
                                int Project,
                                string Floor,
                                string UnitNo,
                                string UnitAddressNo,
                                string UnitType,
                                decimal UnitArea,
                                int BedRoom,
                                int LvingRoom,
                                int ToiletRoom,
                                int KitchenRoom,
                                int CountKey,
                                decimal MaintenaceFee,
                                decimal WaterBill,
                                decimal RentPriceStart,
                                decimal RentPriceEnd,
                                decimal SalePriceStart,
                                decimal SalePriceEnd,
                                string Company,
                                string Departments,
                             
                                string DepositNo,
                                DateTime DepositDate,
                                int DepositStatus,
                                int DSContactID,
                                int DSTypeID,
                                Boolean IsRent,
                                Boolean IsSale,
                                string CheckRoomRemarkDeposit,
                                string FacilityOther,
                                string FacilityRemark,
                                DateTime DepositStart,
                                DateTime DepositEnd,
                                Boolean IsDelete,
                                int UnitID,
                                int DeveloperID,
                                int DepositID,
                               Boolean KeyIsAgent,
                               Boolean KeyIsJuristic,
                               string ProjectAddress,
                               string DocRef,
                               string Building,
                               string ReasonCancel,
                                  int InchargeBy = 0)
        {
            #region UnitsTable
            var dataUnit = new UnitsTable();
            dataUnit.UnitsID = UnitID;
            dataUnit.ProjectID = Project;
            dataUnit.ModelID = 0;
            dataUnit.UnitsCode = UnitNo;
            dataUnit.UnitsName = UnitNo;
            dataUnit.UnitsStatus = true;
            dataUnit.Creator = EmpID;
            dataUnit.CreateDateTime = DateTime.Now;
            dataUnit.Reviser = EmpID;
            dataUnit.ReviseDateTime = DateTime.Now;
            if (DepositStatus == 5) { dataUnit.IsDelete = true; }
            else { dataUnit.IsDelete = false; }
            #endregion
            /// Insert Unit 
            var _UnitID = SaveUnits(dataUnit);

            #region UnitsDetialTable
            var dataUnitDetails = new UnitsDetialTable();
            dataUnitDetails.UnitDetialID = 0;
            dataUnitDetails.UnitsID = _UnitID;
            dataUnitDetails.Floor = Floor;
            dataUnitDetails.RoomNo = UnitAddressNo;
            dataUnitDetails.RoomType = UnitType;
            dataUnitDetails.RoomArea = UnitArea;
            dataUnitDetails.BedRoomQty = BedRoom;
            dataUnitDetails.LivingRoomQty = LvingRoom;
            dataUnitDetails.ToiletRoomQty = ToiletRoom;
            dataUnitDetails.KitchenRoomQty = KitchenRoom;
            dataUnitDetails.Building = Building;
            dataUnitDetails.Creator = EmpID;
            dataUnitDetails.CreateDateTime = DateTime.Now;
            dataUnitDetails.Reviser = EmpID;
            dataUnitDetails.ReviseDateTime = DateTime.Now;

            if (DepositStatus == 5) { dataUnitDetails.IsDelete = true; }
            else { dataUnitDetails.IsDelete = false; }

            #endregion
            /// Insert Unit Details
            var _UnitDetailsID = SaveUnitsDetails(dataUnitDetails);

            //Insert Or Update DepositTable

            var data = new DepositTable();
            data.DepositID = DepositID;
            data.DepositNo = DepositNo;
            data.DepositDate = DepositDate;
            data.DepositStatus = DepositStatus;
            data.DSContactID = DSContactID;
            data.DSTypeID = DSTypeID;
            data.DeveloperID = DeveloperID;
            data.ProjectID = Project;
            data.UnitID = _UnitID;
            data.IsRent = IsRent;
            data.RentPriceStart = RentPriceStart;
            data.RentPriceEnd = RentPriceEnd;
            data.IsSale = IsSale;
            data.SalePriceStart = SalePriceStart;
            data.SalePriceEnd = SalePriceEnd;
            data.CheckRoomRemarkDeposit = CheckRoomRemarkDeposit;
            data.FacilityOther = FacilityOther;
            data.FacilityRemark = FacilityRemark;
            data.InChargeBy = InchargeBy;
            data.DepositStart = DepositStart;
            data.DepositEnd = DepositEnd;
            data.Creator = EmpID;
            data.CreateDateTime = DateTime.Now;
            data.Reviser = EmpID;
            data.ReviseDateTime = DateTime.Now;
            data.IsDelete = IsDelete;
            data.CountKey = CountKey;
            data.KeyIsAgent = KeyIsAgent;
            data.KeyIsJuristic = KeyIsJuristic;
            data.MaintenanceCharge = MaintenaceFee;
            data.WaterCharge = WaterBill;
            data.ProjectAddress = ProjectAddress;
            data.DocRef = DocRef;
            data.ReasonCancel = ReasonCancel;

            var _DepositID = SaveDepositTable(data);
            // //Insert Checkroomtrans Table ( CheckBox = True ) Keeps History ?
            var _Checkroomtrans = SaveCheckRoomTrans(strCheckRoom, _DepositID, EmpID,1);
            //// //Insert Facilitytrans Table ( CheckBox = True ) By DepositTable
            var _FacilityTrans = SaveFacilitytrans(strFac, _DepositID, EmpID);


            return DSContactID.ToString() + '|' + _DepositID.ToString(); 
        }

        public long SaveUnitsDetails(UnitsDetialTable _UnitDetails)
        {

            var _Data = DMPS.UnitsDetialTables.SingleOrDefault(s => s.UnitsID == _UnitDetails.UnitsID && s.IsDelete != true);

            if (_Data == null)
            {
                _Data = new UnitsDetialTable();
                _Data.UnitsID = _UnitDetails.UnitsID;
                _Data.Floor = _UnitDetails.Floor;
                _Data.RoomNo = _UnitDetails.RoomNo;
                _Data.RoomType = _UnitDetails.RoomType;
                _Data.RoomArea = _UnitDetails.RoomArea;
                _Data.BedRoomQty = _UnitDetails.BedRoomQty;
                _Data.LivingRoomQty = _UnitDetails.LivingRoomQty;
                _Data.ToiletRoomQty = _UnitDetails.ToiletRoomQty;
                _Data.KitchenRoomQty = _UnitDetails.KitchenRoomQty;
                _Data.Building = _UnitDetails.Building;
                _Data.Creator = _UnitDetails.Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = _UnitDetails.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.IsDelete = _UnitDetails.IsDelete;
                DMPS.UnitsDetialTables.Add(_Data);
                DMPS.SaveChanges();
            }
            else
            {

                _Data.Floor = _UnitDetails.Floor;
                _Data.RoomNo = _UnitDetails.RoomNo;
                _Data.RoomType = _UnitDetails.RoomType;
                _Data.RoomArea = _UnitDetails.RoomArea;
                _Data.BedRoomQty = _UnitDetails.BedRoomQty;
                _Data.LivingRoomQty = _UnitDetails.LivingRoomQty;
                _Data.ToiletRoomQty = _UnitDetails.ToiletRoomQty;
                _Data.KitchenRoomQty = _UnitDetails.KitchenRoomQty;
                _Data.Building = _UnitDetails.Building;
                _Data.Reviser = _UnitDetails.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.IsDelete = _UnitDetails.IsDelete;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }

            return _Data.UnitDetialID;
        }

        public long SaveUnits(UnitsTable _Unit)
        {

            var _Data = DMPS.UnitsTables.SingleOrDefault(s => s.UnitsID == _Unit.UnitsID && s.ProjectID == _Unit.ProjectID && s.IsDelete != true);

            if (_Data == null)
            {
                _Data = new UnitsTable();               
                _Data.ProjectID = _Unit.ProjectID;
                _Data.ModelID = _Unit.ModelID;
                _Data.UnitsCode = _Unit.UnitsCode;
                _Data.UnitsName = _Unit.UnitsName;
                _Data.UnitsStatus = _Unit.UnitsStatus;
                _Data.Creator = _Unit.Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = _Unit.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.IsDelete = _Unit.IsDelete;
                DMPS.UnitsTables.Add(_Data);
                DMPS.SaveChanges();
            }
            else
            {
                _Data.ProjectID = _Unit.ProjectID;
                _Data.ModelID = _Unit.ModelID;
                _Data.UnitsCode = _Unit.UnitsCode;
                _Data.UnitsName = _Unit.UnitsName;
                _Data.UnitsStatus = _Unit.UnitsStatus;
                _Data.Reviser = _Unit.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.IsDelete = _Unit.IsDelete;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }
            return _Data.UnitsID;
        }

        public ActionResult LoadRoomListView()
        {
            return PartialView();
        }

        public ActionResult LoadModal()
        {
            //var lstBank = (from t1 in MASDB.BankTables.Where(s => s.isNonBank == false && s.isDelete == false) select new { BankID = t1.BankID, DisplayName = t1.BankID + " : " + t1.BankName }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddlBank = new SelectList(lstBank, "BankID", "DisplayName");

            //var lstAccountType = (from t1 in MASDB.BankTypeTables.Where(s => s.IsDelete == false) select new { BankTypeID = t1.BankTypeID, DisplayName = t1.BankTypeNameTH }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddlAccountType = new SelectList(lstAccountType, "BankTypeID", "DisplayName");

            //var lstEmployees = (from t1 in MASDB.Employees.Where(s => s.IsActive == true && s.EmpId != "") select new { EmpID = t1.EmpId, DisplayName = t1.EmpId + " : " + t1.DisplayName }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddlInChargeBy = new SelectList(lstEmployees, "EmpID", "DisplayName");

            //ViewBag.ddlCompany = new SelectList(MASDB.CompanyTables.OrderBy(s => s.CompanyName).ToList(), "CompanyId", "CompanyName");
            //ViewBag.ddlDepartment = new SelectList(MASDB.DepartmentsTables.Where(s => s.DeptTypeId != "999").OrderBy(s => s.DeptDescription).ToList(), "DeptId", "DeptDescription");
            //ViewBag.ddlPosition = new SelectList(MASDB.PositionTables.Where(s => s.IsDelete != true).OrderBy(s => s.PositionName).ToList(), "PositionID", "PositionName");


            return PartialView();
        }


        public string LoadData2Modal(long _DepositID = 0)
        {
            var data = DMPS.vw_UnitsDetails.Where(s => s.DepositID == _DepositID).SingleOrDefault();

            var qCheckRoomMaster = DMPS.CheckRoomMasterTables;
            ViewBag.lstCheckRoomMaster = qCheckRoomMaster.OrderBy(s => s.Sort).ToList();

            var qFacilityMaster = DMPS.FacilityMasterTables;
            ViewBag.lstFacilityMaster = qFacilityMaster.ToList();

            ViewBag.lstCheckRoomMasterTB = null;
            ViewBag.lstFacilityMasterTB = null;

            if (data == null)
            {
                data = new vw_UnitsDetails();
                data.DepositID = 0;
                data.DepositNo = "New";
                data.UnitsID = 0;
                data.ProjectID = 0;
            }
            else
            {
                var qCheckRoomMasterTB = DMPS.CheckRoomTrans.Where(s => s.ID == _DepositID && s.IsDelete != true);
                ViewBag.lstCheckRoomMasterTB = qCheckRoomMasterTB.ToList();

                var qFacilityMasterTB = DMPS.CheckRoomTrans.Where(s => s.ID == _DepositID && s.IsDelete != true);
                ViewBag.lstFacilityMasterTB = qFacilityMasterTB.ToList();
            }
            var result = data.ToObj2Json();
            return result;
        }

        public DataTable getDataForPrint(long _DepositID = 0)
        {
            DataTable dt = new DataTable();
            var connection = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["Cnn_PM"]);
            var command = new SqlCommand("Select * from vw_rpt_Deposit where DepositID = " + "'" + _DepositID + "'", connection);
                connection.Open();
                SqlDataReader dr = command.ExecuteReader();
                dt.Load(dr);
                dr.Close();
        
            return dt;
        }

     
        //public string PrintReportDirecrt(long _DepositID) {


        //    try
        //    {
        //        ReportClass rptMemo = new ReportClass();
        //        ReportDocument rptDoc = new ReportDocument();

        //        //PrintDocument printDocument = new PrintDocument();
        //        //rptDoc.PrintOptions.PrinterName = printDocument.PrinterSettings.PrinterName;
        //        //rptDoc.FileName = Server.MapPath("~/Report/PM/rptContractRent_Condo.rpt");
        //        //rptDoc.SetDataSource(dt);
        //        //rptDoc.PrintOptions.PrinterName = printDocument.PrinterSettings.PrinterName;
        //        //rptDoc.PrintToPrinter(1, false, 0, 0);


        //        rptMemo.FileName = Server.MapPath("~/Report/PM/rptContract_Agent.rpt");
        //        rptMemo.Load();
        //        rptMemo.SetDataSource(getDataForPrint(_DepositID));


        //        Stream st = rptMemo.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        return File(st, "application/pdf");


        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Error.Write(ex);
        //        throw;
        //    }

        //}

        public ActionResult PrintReport(long ReportID)
        {
            // Your .rpt file path will be below DataTable dt = new DataTable();
            //var connection = new SqlConnection("Server=10.40.3.11\\sql2008r2;DataBase=CAD; User Id=sa;Password=p@ssw0rd");
            //var connection = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["PMDBSqlConnection"]);
            //var command = new SqlCommand("Select * from vw_ReportAdvancePayment where AdvancePaymentID = " + AdvancePaymentID, connection);
            //connection.Open();
            //SqlDataReader dr = command.ExecuteReader();
            //dt.Load(dr);
            //dr.Close();
            //return dt;

            DataTable dt = new DataTable();
            var connection = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["Cnn_PM"]);
            var command = new SqlCommand("Select * from vw_CheckRoomlist where ID = '"+ ReportID + "' and CheckType = 1 ", connection);
            var command2 = new SqlCommand("Select * from vw_rpt_Deposit where DepositID = '"+ ReportID + "'", connection);
            connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            dt.Load(dr);
            dr.Close();

            SqlDataReader dr2 = command2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(dr2);
            dr2.Close();

            ReportClass rptMemo = new ReportClass();
            rptMemo.FileName = Server.MapPath("~/Report/PM/rptContract_Agent.rpt");
            //rptMemo.Subreports[0].SetDataSource(dt);
            rptMemo.Load();
            rptMemo.SetDataSource(dt2);
            rptMemo.Subreports[0].SetDataSource(dt);
            //rptMemo.SetDataSource(dt);
            //rptMemo.SetParameterValue("Company", CompanyName);

            Stream st = rptMemo.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(st, "application/pdf");
        }


        public DataTable GetDataDeposit(long id)
        {
            DataTable dt = new DataTable();
            string Conn = System.Configuration.ConfigurationSettings.AppSettings["Cnn_PM"];
            var connection = new SqlConnection(Conn);

         
                var command = new SqlCommand(" select * from vw_rpt_Deposit where DepositID = '" + id + "'", connection);
                connection.Open();
                SqlDataReader dr = command.ExecuteReader();
                dt.Load(dr);
                dr.Close();
            return dt;
        }
        public string GetRoomCheck(long DepositID,long CheckType) {
            var data = DMPS.vw_CheckRoomlist.Where(s => s.ID == DepositID && s.CheckType == CheckType);
            var result = data.ToObj2Json();
            return result;
        }

        public string GetFacility(long DepositID)
        {
            var data = DMPS.vw_CheckFacility.Where(s => s.ID == DepositID);
            var result = data.ToObj2Json();
            return result;
        }

        public long SaveCheckRoomTrans(string strCheckRoom, long DepositID, int CreateBy,long _CheckType)
        {
            var RootObjects = JsonConvert.DeserializeObject<List<CheckRoomTransAdd>>(strCheckRoom);

            DMPS.Database.ExecuteSqlCommand("DELETE FROM CheckRoomTrans WHERE ID = " + DepositID + " AND CheckType = "+ _CheckType  + " ");

            foreach (var rootObject in RootObjects)
            {
                if (rootObject.CheckCheckRoom == true)
                {
                    var _Data = DMPS.CheckRoomTrans.SingleOrDefault(s => s.ID == DepositID && s.CheckRoomID == rootObject.CheckRoomID && s.IsDelete != true);

                    if (_Data == null)
                    {
                        _Data = new CheckRoomTran();
                        _Data.CheckRoomID = rootObject.CheckRoomID;
                        //_Data.CheckType = _CheckRoomTrans.CheckType;
                        _Data.ID = DepositID;
                        _Data.CheckRoomQty = rootObject.CheckQTY.ToString();
                        _Data.CheckType = _CheckType;
                        _Data.Creator = CreateBy;
                        _Data.CreateDateTime = DateTime.Now;
                        _Data.Reviser = CreateBy;
                        _Data.ReviseDateTime = DateTime.Now;
                        _Data.IsDelete = false;
                        DMPS.CheckRoomTrans.Add(_Data);
  
                    }
                    else
                    {
                        //ยังไม่เสร็จรอเพิ่มเติม
                        //_Data.CheckType = _CheckRoomTrans.CheckType;
                        _Data.CheckRoomQty = rootObject.CheckQTY.ToString();
                        //_Data.IsEnd = _CheckRoomTrans.IsEnd;
                        //_Data.IsEndDate = _CheckRoomTrans.IsEndDate;
                        _Data.Reviser = CreateBy;
                        _Data.ReviseDateTime = DateTime.Now;
                        //_Data.IsDelete = _CheckRoomTrans.IsDelete;
                        DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;

                    }
                }                
            }
            DMPS.SaveChanges();
            return DepositID;
        }

        public int CheckExistIDCard(string IDCard,int CustType,int ContactID)
        {

            //var _Data = DMPS.CRM_Contacts.SingleOrDefault(s => s.ContactsID == _Contacts.ContactsID && s.CustomerTypeId == _Contacts.CustomerTypeId);
            DataTable dt = new DataTable();
            string Conn = System.Configuration.ConfigurationSettings.AppSettings["Cnn_PM"];
            var connection = new SqlConnection(Conn);


            var command = new SqlCommand(" select * from CRM_Contacts where UPPER(REPLACE(REPLACE(CitizenID,' ','') ,'-','')) = '" + IDCard.Replace(" ","").Replace("-","").ToUpper() + "' and CustomerTypeId = '"+ CustType + "' and ContactsID <> '" + ContactID  + "' ", connection);
            connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            dt.Load(dr);
            dr.Close();


            if (dt.Rows.Count > 0) { return 1; }
            else { return 0; }

        }


        public int CheckExistUnit(string Unitcode, int Project , int UnitID)
        {

            //var _Data = DMPS.CRM_Contacts.SingleOrDefault(s => s.ContactsID == _Contacts.ContactsID && s.CustomerTypeId == _Contacts.CustomerTypeId);
            DataTable dt = new DataTable();
            string Conn = System.Configuration.ConfigurationSettings.AppSettings["Cnn_PM"];
            var connection = new SqlConnection(Conn);


            var command = new SqlCommand(" select * from UnitsTable where UPPER(REPLACE(REPLACE(UnitsCode,' ','') ,'-','')) = '" + Unitcode.Replace(" ", "").Replace("-", "").ToUpper() + "' and ProjectID = '" + Project + "' and IsDelete = 0 and UnitsID <> '" + UnitID + "' ", connection);
            connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            dt.Load(dr);
            dr.Close();


            if (dt.Rows.Count > 0) { return 1; }
            else { return 0; }

        }

        public int CheckExistDeveloper(string Developer)
        {
            //var _Data = DMPS.CRM_Contacts.SingleOrDefault(s => s.ContactsID == _Contacts.ContactsID && s.CustomerTypeId == _Contacts.CustomerTypeId);
            DataTable dt = new DataTable();
            string Conn = System.Configuration.ConfigurationSettings.AppSettings["Cnn_PM"];
            var connection = new SqlConnection(Conn);


            var command = new SqlCommand(" select * from DevelopmentTable where UPPER(REPLACE(DevelopmentName,' ','') ) = '" + Developer.Replace(" ", "").ToUpper() + "' and IsDelete = 0 ", connection);
            connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            dt.Load(dr);
            dr.Close();


            if (dt.Rows.Count > 0) { return 1; }
            else { return 0; }
        }

        public int CheckExistProject(int Developer,string ProjectName)
        {
            //var _Data = DMPS.CRM_Contacts.SingleOrDefault(s => s.ContactsID == _Contacts.ContactsID && s.CustomerTypeId == _Contacts.CustomerTypeId);
            DataTable dt = new DataTable();
            string Conn = System.Configuration.ConfigurationSettings.AppSettings["Cnn_PM"];
            var connection = new SqlConnection(Conn);


            var command = new SqlCommand(" select * from ProjectTable where DevelopmentID = '" + Developer + "' and UPPER(REPLACE(ProjectName,' ','') ) = '" + ProjectName.Replace(" ", "").ToUpper() + "' and IsDelete = 0 ", connection);
            connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            dt.Load(dr);
            dr.Close();


            if (dt.Rows.Count > 0) { return 1; }
            else { return 0; }
        }


        public int CheckExistUnitType(string UnitType)
        {
            //var _Data = DMPS.CRM_Contacts.SingleOrDefault(s => s.ContactsID == _Contacts.ContactsID && s.CustomerTypeId == _Contacts.CustomerTypeId);
            DataTable dt = new DataTable();
            string Conn = System.Configuration.ConfigurationSettings.AppSettings["Cnn_PM"];
            var connection = new SqlConnection(Conn);


            var command = new SqlCommand(" select * from UnitTypeTable where UPPER(REPLACE(UnitType,' ','') ) = '" + UnitType.Replace(" ", "").ToUpper() + "' and IsDelete = 0 ", connection);
            connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            dt.Load(dr);
            dr.Close();


            if (dt.Rows.Count > 0) { return 1; }
            else { return 0; }
        }



        public long SaveFacilitytrans(string strFac, long DepositID, int CreateBy)
        {
            var RootObjects = JsonConvert.DeserializeObject<List<FacilityTransAdd>>(strFac);
            DMPS.Database.ExecuteSqlCommand("DELETE FROM FacilityTrans WHERE ID = " + DepositID + " ");
            foreach (var rootObject in RootObjects)
            {
                if (rootObject.CheckFacility == true)
                {
                    var _Data = DMPS.FacilityTrans.SingleOrDefault(s => s.FacilityID == rootObject.FacilityID && s.ID == DepositID && s.IsDelete != true);

                    if (_Data == null)
                    {
                        _Data = new FacilityTran();
                        _Data.FacilityID = rootObject.FacilityID;
                        _Data.ID = DepositID;
                        //_Data.FacilityQty = rootObject.FacilityQTY;
                        _Data.Creator = CreateBy;
                        _Data.CreateDateTime = DateTime.Now;
                        _Data.Reviser = CreateBy;
                        _Data.ReviseDateTime = DateTime.Now;
                        _Data.IsDelete = false;
                        DMPS.FacilityTrans.Add(_Data);
                    }
                    else
                    {
                        //_Data.FacilityQty = rootObject.FacilityQTY;
                        _Data.Reviser = CreateBy;
                        _Data.ReviseDateTime = DateTime.Now;
                        //_Data.IsDelete = _FacilityTrnas.IsDelete;
                        DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                    }
                }                
            }
            DMPS.SaveChanges();
            return DepositID;
        }

        public long SaveCustomer(CRM_Contacts _Contacts)
        {

            var _Data = DMPS.CRM_Contacts.SingleOrDefault(s => s.ContactsID == _Contacts.ContactsID && s.CustomerTypeId == _Contacts.CustomerTypeId  );

            if (_Data == null)
            {
                _Data = new CRM_Contacts();
                _Data.CustomerTypeId = _Contacts.CustomerTypeId;
                _Data.Prefix = _Contacts.Prefix;
                _Data.FirstName = _Contacts.FirstName;
                _Data.LastName = _Contacts.LastName;
                _Data.Age = _Contacts.Age;
                _Data.CitizenID = _Contacts.CitizenID.Replace(" ","").Replace("-","").ToUpper();
                _Data.CitizenIssue = _Contacts.CitizenIssue;
                _Data.CitizenExp = _Contacts.CitizenExp;
                _Data.CitizenProvince = _Contacts.CitizenProvince;
                _Data.Tel1 = _Contacts.Tel1;
                _Data.Tel2 = _Contacts.Tel2;
                _Data.Tel3 = _Contacts.Tel3;
                _Data.Email = _Contacts.Email;
                _Data.FaxNo = _Contacts.FaxNo;
                _Data.BirthDate = _Contacts.BirthDate;
                _Data.Gender = _Contacts.Gender;
                _Data.AddressNo = _Contacts.AddressNo;
                _Data.Moo = _Contacts.Moo;
                _Data.Soi = _Contacts.Soi;
                _Data.Village = _Contacts.Village;
                _Data.Road = _Contacts.Road;
                _Data.SubDistrict = _Contacts.SubDistrict;
                _Data.District = _Contacts.District;
                _Data.Province = _Contacts.Province;
                _Data.ZipCode = _Contacts.ZipCode;
                _Data.Country = _Contacts.Country;
                _Data.Nationality = _Contacts.Nationality;
                _Data.PassportID = _Contacts.PassportID;
                _Data.FirstNameEng = _Contacts.FirstNameEng;
                _Data.LastNameEng = _Contacts.LastNameEng;
                _Data.IsJuristic = _Contacts.IsJuristic;
                _Data.AddressNo_Work = _Contacts.AddressNo_Work;
                _Data.CreateDate = DateTime.Now;
                _Data.CreateBy = _Contacts.CreateBy;
                _Data.ModifyDate = DateTime.Now;
                _Data.ModifyBy = _Contacts.ModifyBy;
                _Data.CustomerTypeId = _Contacts.CustomerTypeId;
                _Data.Remark = _Contacts.Remark;
                _Data.IsDelete = _Contacts.IsDelete;
                _Data.UnExpireIDCard = _Contacts.UnExpireIDCard;
                _Data.IsForeign = _Contacts.IsForeign;
                DMPS.CRM_Contacts.Add(_Data);
                DMPS.SaveChanges();
            }
            else
            {
                _Data.CustomerTypeId = _Contacts.CustomerTypeId;
                _Data.Prefix = _Contacts.Prefix;
                _Data.FirstName = _Contacts.FirstName;
                _Data.LastName = _Contacts.LastName;
                _Data.Age = _Contacts.Age;
                _Data.CitizenID = _Contacts.CitizenID.Replace(" ", "").Replace("-", "").ToUpper();
                _Data.CitizenIssue = _Contacts.CitizenIssue;
                _Data.CitizenExp = _Contacts.CitizenExp;
                _Data.CitizenProvince = _Contacts.CitizenProvince;
                _Data.Tel1 = _Contacts.Tel1;
                _Data.Tel2 = _Contacts.Tel2;
                _Data.Tel3 = _Contacts.Tel3;
                _Data.Email = _Contacts.Email;
                _Data.FaxNo = _Contacts.FaxNo;
                _Data.BirthDate = _Contacts.BirthDate;
                _Data.Gender = _Contacts.Gender;
                _Data.AddressNo = _Contacts.AddressNo;
                _Data.Moo = _Contacts.Moo;
                _Data.Soi = _Contacts.Soi;
                _Data.Village = _Contacts.Village;
                _Data.Road = _Contacts.Road;
                _Data.SubDistrict = _Contacts.SubDistrict;
                _Data.District = _Contacts.District;
                _Data.Province = _Contacts.Province;
                _Data.ZipCode = _Contacts.ZipCode;
                _Data.Country = _Contacts.Country;
                _Data.Nationality = _Contacts.Nationality;
                _Data.PassportID = _Contacts.PassportID;
                _Data.FirstNameEng = _Contacts.FirstNameEng;
                _Data.LastNameEng = _Contacts.LastNameEng;
                _Data.IsJuristic = _Contacts.IsJuristic;
                _Data.AddressNo_Work = _Contacts.AddressNo_Work;
                _Data.ModifyDate = DateTime.Now;
                _Data.ModifyBy = _Contacts.ModifyBy;
                _Data.Remark = _Contacts.Remark;
                _Data.IsDelete = _Contacts.IsDelete;
                _Data.UnExpireIDCard = _Contacts.UnExpireIDCard;
                _Data.IsForeign = _Contacts.IsForeign;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }

            return _Data.ContactsID;
        }

        public long SaveDepositTable(DepositTable _Deposit)
        {

            var _Data = DMPS.DepositTables.SingleOrDefault(s => s.DepositID == _Deposit.DepositID && s.IsDelete != true);

            var _genDepositNo = GenerateDocuments(0, 0);


            if (_Data == null)
            {
                _Data = new DepositTable();
                _Data.DepositNo = _genDepositNo;
                _Data.DepositDate = _Deposit.DepositDate;
                _Data.DSContactID = _Deposit.DSContactID;
                _Data.DepositStatus = _Deposit.DepositStatus;
                _Data.DSTypeID = _Deposit.DSTypeID;
                _Data.DeveloperID = _Deposit.DeveloperID;
                _Data.ProjectID = _Deposit.ProjectID;
                _Data.UnitID = _Deposit.UnitID;
                _Data.IsRent = _Deposit.IsRent;
                _Data.IsSale = _Deposit.IsSale;
                _Data.RentPriceStart = _Deposit.RentPriceStart;
                _Data.RentPriceEnd = _Deposit.RentPriceEnd;
                _Data.IsSale = _Deposit.IsSale;
                _Data.SalePriceStart = _Deposit.SalePriceStart;
                _Data.SalePriceEnd = _Deposit.SalePriceEnd;
                _Data.InChargeBy = _Deposit.InChargeBy;
                _Data.DepositStart = _Deposit.DepositStart;
                _Data.DepositEnd = _Deposit.DepositEnd;
                _Data.Creator = _Deposit.Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = _Deposit.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.CheckRoomRemarkDeposit = _Deposit.CheckRoomRemarkDeposit;
                _Data.FacilityOther = _Deposit.FacilityOther;
                _Data.FacilityRemark = _Deposit.FacilityRemark;
                _Data.CountKey = _Deposit.CountKey;
                _Data.KeyIsAgent = _Deposit.KeyIsAgent;
                _Data.KeyIsJuristic = _Deposit.KeyIsJuristic;
                _Data.MaintenanceCharge = _Deposit.MaintenanceCharge;
                _Data.WaterCharge = _Deposit.WaterCharge;
                _Data.ProjectAddress = _Deposit.ProjectAddress;
                _Data.DocRef = _Deposit.DocRef;
                _Data.IsDelete = _Deposit.IsDelete;
                _Data.ReasonCancel = _Deposit.ReasonCancel;
                DMPS.DepositTables.Add(_Data);
                DMPS.SaveChanges();
            }
            else
            {
                _Data.DepositDate = _Deposit.DepositDate;
                _Data.DSContactID = _Deposit.DSContactID;
                _Data.DSTypeID = _Deposit.DSTypeID;
                _Data.DeveloperID = _Deposit.DeveloperID;
                _Data.ProjectID = _Deposit.ProjectID;
                _Data.UnitID = _Deposit.UnitID;
                _Data.IsRent = _Deposit.IsRent;
                _Data.IsSale = _Deposit.IsSale;
                _Data.RentPriceStart = _Deposit.RentPriceStart;
                _Data.RentPriceEnd = _Deposit.RentPriceEnd;
                _Data.IsSale = _Deposit.IsSale;
                _Data.SalePriceStart = _Deposit.SalePriceStart;
                _Data.SalePriceEnd = _Deposit.SalePriceEnd;
                _Data.InChargeBy = _Deposit.InChargeBy;
                _Data.DepositStart = _Deposit.DepositStart;
                _Data.DepositEnd = _Deposit.DepositEnd;
                _Data.Reviser = _Deposit.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.CheckRoomRemarkDeposit = _Deposit.CheckRoomRemarkDeposit;
                _Data.FacilityOther = _Deposit.FacilityOther;
                _Data.FacilityRemark = _Deposit.FacilityRemark;
                _Data.CountKey = _Deposit.CountKey;
                _Data.KeyIsAgent = _Deposit.KeyIsAgent;
                _Data.KeyIsJuristic = _Deposit.KeyIsJuristic;
                _Data.MaintenanceCharge = _Deposit.MaintenanceCharge;
                _Data.WaterCharge = _Deposit.WaterCharge;
                _Data.ProjectAddress = _Deposit.ProjectAddress;
                _Data.DocRef = _Deposit.DocRef;
                _Data.IsDelete = _Deposit.IsDelete;
                _Data.ReasonCancel = _Deposit.ReasonCancel;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }

            return _Data.DepositID;
        }

        public long SaveDevelopment(DevelopmentTable _Development)
        {

            var _Data = DMPS.DevelopmentTables.SingleOrDefault(s => s.DevelopmentID == _Development.DevelopmentID && s.IsDelete != true);

            if (_Data == null)
            {
                _Data = new DevelopmentTable();
                _Data.DevelopmentCode = _Development.DevelopmentCode;
                _Data.DevelopmentName = _Development.DevelopmentName;
                _Data.DevelopmentStatus = _Development.DevelopmentStatus;
                _Data.Creator = _Development.Reviser;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = _Development.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.DevelopmentTables.Add(_Data);
                DMPS.SaveChanges();
            }
            else
            {
                _Data.DevelopmentCode = _Development.DevelopmentCode;
                _Data.DevelopmentName = _Development.DevelopmentName;
                _Data.DevelopmentStatus = _Development.DevelopmentStatus;
                _Data.Reviser = _Development.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }

            return _Data.DevelopmentID;
        }

        //public long SaveProject(ProjectTable _Project)
        //{

        //    var _Data = DMPS.ProjectTables.SingleOrDefault(s => s.ProjectID == _Project.ProjectID && s.IsDelete != true);

        //    if (_Data == null)
        //    {
        //        _Data = new ProjectTable();
        //        _Data.DevelopmentID = _Project.DevelopmentID;
        //        _Data.ProjectStatus = _Project.ProjectStatus;
        //        _Data.ProjectCode = _Project.ProjectCode;
        //        _Data.ProjectName = _Project.ProjectName;
        //        _Data.Creator = _Project.Creator;
        //        _Data.CreateDateTime = DateTime.Now;
        //        _Data.Reviser = _Project.Reviser;
        //        _Data.ReviseDateTime = DateTime.Now;
        //        DMPS.ProjectTables.Add(_Data);
        //        DMPS.SaveChanges();
        //    }
        //    else
        //    {
        //        _Data.DevelopmentID = _Project.DevelopmentID;
        //        _Data.ProjectStatus = _Project.ProjectStatus;
        //        _Data.ProjectCode = _Project.ProjectCode;
        //        _Data.ProjectName = _Project.ProjectName;
        //        _Data.Reviser = _Project.Reviser;
        //        _Data.ReviseDateTime = DateTime.Now;
        //        DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
        //        DMPS.SaveChanges();
        //    }

        //    return _Data.ProjectID;
        //}

        public long SaveDevelopments(DevelopmentTable _Developments)
        {

            var _Data = DMPS.DevelopmentTables.SingleOrDefault(s => s.DevelopmentID == _Developments.DevelopmentID && s.IsDelete != true);

            if (_Data == null)
            {
                _Data = new DevelopmentTable();
                _Data.DevelopmentName = _Developments.DevelopmentName;
                _Data.Creator = _Developments.Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = _Developments.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.DevelopmentTables.Add(_Data);
                DMPS.SaveChanges();
            }
            else
            {
                _Data.DevelopmentName = _Developments.DevelopmentName;
                _Data.Reviser = _Developments.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }

            return _Data.DevelopmentID;
        }

        public long SaveContactBank(ContactBankTable _BankContact )
        {
            var _Data = DMPS.ContactBankTables.SingleOrDefault(s => s.DepositID == _BankContact.DepositID );

            if (_Data == null)
            {
                _Data = new ContactBankTable();
                _Data.BankID = _BankContact.BankID;
                _Data.BankTypeID = _BankContact.BankTypeID;
                _Data.BankNo = _BankContact.BankNo;
                _Data.BankName = _BankContact.BankName;
                _Data.ContactsID = _BankContact.ContactsID;
                _Data.DepositID = _BankContact.DepositID;
                _Data.Creator = _BankContact.Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = _BankContact.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.ContactBankTables.Add(_Data);
                DMPS.SaveChanges();
            }
            else
            {
                _Data.BankID = _BankContact.BankID;
                _Data.BankTypeID = _BankContact.BankTypeID;
                _Data.BankNo = _BankContact.BankNo;
                _Data.BankName = _BankContact.BankName;
                _Data.Reviser = _BankContact.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }

            return _Data.ContactBankID;

        }

        //public long SaveCheckRoomMaster(CheckRoomMasterTable _CheckRoomMS)
        //{

        //    var _Data = DMPS.CheckRoomMasterTables.SingleOrDefault(s => s.CheckRoomID == _CheckRoomMS.CheckRoomID && s.IsDelete != true);

        //    if (_Data == null)
        //    {
        //        _Data = new ProjectTable();
        //        _Data.ProjectID = _Project.ProjectID;
        //        _Data.ProjectCode = _Project.ProjectCode;
        //        _Data.ProjectName = _Project.ProjectName;
        //        _Data.Creator = _Project.Creator;
        //        _Data.CreateDateTime = DateTime.Now;
        //        _Data.Reviser = _Project.Reviser;
        //        _Data.ReviseDateTime = DateTime.Now;
        //        DMPS.ProjectTables.Add(_Data);
        //        DMPS.SaveChanges();
        //    }
        //    else
        //    {
        //        _Data.ProjectID = _Project.ProjectID;
        //        _Data.ProjectCode = _Project.ProjectCode;
        //        _Data.ProjectName = _Project.ProjectName;
        //        _Data.Reviser = _Project.Reviser;
        //        _Data.ReviseDateTime = DateTime.Now;
        //        DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
        //        DMPS.SaveChanges();
        //    }

        //    return _Data.ProjectID;
        //}


        public FileStreamResult GetFile(string filename)
        {
            string resultType = Path.GetExtension(filename);
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            return File(fs, ExtenMethod.GetMimeType(resultType));
        }



        public string GetImageTable(long DepositID,string Type) {

            var _Data = DMPS.ImageTables.Where(s => s.DepositID == DepositID && s.Type == Type).OrderBy(s=>s.Running).ToList();

            if (_Data.Count > 0)
            {
                var Result = _Data.ToObj2Json();
                return Result;
            }
            else {
                return "";
            }
        }

        public void FileUploadHandler()
        {
            //string setIDDoc = Session["SetID"].ToString().Substring(0, Session["SetID"].ToString().IndexOf("("));
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    long _DepositID = Request["DepositID"].ToString().ToLong();
                    long _Running = Request["Running"].ToString().ToLong();
                    string _Type = Request["Type"].ToString();


                    if (files.Count > 0)
                    {
                        var _data = DMPS.DepositTables.Where(s => s.DepositID == _DepositID).SingleOrDefault();

                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = files[i];
                            string fname;

                            // Checking for Internet Explorer  
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = file.FileName;
                            }
                            //string path = Server.MapPath("~/AttachFile/CashAdvance/" + avp.AdvancePaymentNO + "/" + DateTime.Now.ToString("yyyy-MM-dd"));
                            string path = Server.MapPath("~/AttachFile/PM/" + DateTime.Now.ToString("yyyy-MM-dd"));

                            if (Directory.Exists(path))
                            {
                                fname = Path.Combine(path, fname);
                            }
                            else
                            {
                                Directory.CreateDirectory(path);
                                fname = Path.Combine(path, fname);
                            }
                            file.SaveAs(fname);
                            string extension = Path.GetExtension(fname);
                            string SaveFilename = "AttachFile/PM/" + DateTime.Now.ToString("yyyy-MM-dd") + "\\" + _data.DepositNo  + extension;
                            string newFilename = path + "\\" + _data.DepositNo + "_" + _Running + extension;

                            if (System.IO.File.Exists(newFilename))
                            {
                                System.IO.File.Move(newFilename, path + "\\" + _data.DepositNo + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                            }
                            System.IO.File.Move(fname, newFilename);



                            var _ful = DMPS.ImageTables.Where(s => s.DepositID == _DepositID && s.Running == _Running && s.Type== _Type).SingleOrDefault();
                            if (_ful == null)
                            {
                                _ful = new Inspinia_MVC5.Models.DMPS.ImageTable();
                                _ful.DepositID = _DepositID;
                                _ful.Path = newFilename;
                                _ful.Running = _Running;
                                _ful.Type = _Type;
                                DMPS.ImageTables.Add(_ful);
                            }
                            else
                            {
                                _ful.Path = newFilename;
                                DMPS.Entry(_ful).State = System.Data.Entity.EntityState.Modified;
                            }

                            DMPS.SaveChanges();

                        }
                    }
                }
                catch (Exception ex)
                {
                    //return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                //return Json("No files selected.");
            }
        }

        public int RemoveFile(string FileName)
        {
            string path = Server.MapPath("files//" + FileName);
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
            }

            return 1;
        }

        public int CheckExistDataRental(long CustID = 0)
        {
            var _data = 0;
            var _lstData = DMPS.vw_UnitsDetails.Where(s => s.ContactsID == CustID && s.DepositStatus == 1 && s.IsDelete != true).ToList();

            if (_lstData.Count() == 0) {
                _data = 0; }
            else { _data = 1; }
            return _data;
        }
         

        private string GenerateDocuments(int CompanyID = 0, int AppID = 0)
        {
            string result = string.Empty;
            //var _CompanyCode = GetCompanyCode(CompanyID);
            var _DocumentCode = "SUP";
            //result += _CompanyCode + '-';
            result += _DocumentCode;
            result += DateTime.Now.ToString("yyyyMMdd-");
            int Year = DateTime.Now.Year;
            int Month = DateTime.Now.Month;
            DateTime StartofMonth = new DateTime(Year, Month, 1, 0, 0, 0);
            DateTime EndofMonth = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month), 23, 59, 59);

            int CountRec;
            //CountRec = DMPS.DepositTables.Where(s => s.CreateDateTime >= StartofMonth && s.CreateDateTime <= EndofMonth && s.CompanyID == CompanyID && s.WorkNO != "").Count() + 1;
            string _DepositNo;
            _DepositNo = DMPS.DepositTables.Where(s => s.IsDelete != true && s.DepositNo.Contains(result)).Max(x=>x.DepositNo);

            if (_DepositNo == null)
            { CountRec = 1; }
            else { CountRec = int.Parse(_DepositNo.Split('-')[1]) + 1; }

            result += CountRec.ToString("000#");
            return result;
        }

        //private string GetCompanyCode(int CompanyID = 0)
        //{
        //    string result = string.Empty;
        //    var data = MASDB.CompanyTables.Where(s => s.CompanyId == CompanyID).SingleOrDefault();

        //    if (data != null)
        //    {
        //        result = data.CompanyNameCode.ToString();
        //    }
        //    else
        //    {
        //        result = "XXX";
        //    }

        //    return result;
        //}

        private string GetDocumentCode(int appid = 0)
        {
            string result = string.Empty;
            var data = cApi.apiGetApplicationList().Where(s => s.ApplicationId == appid).SingleOrDefault();

            if (data != null)
            {
                result = data.ApplicationPrefixChar.ToString();
            }
            else
            {
                result = "XXX";
            }

            return result;
        }

        public int  GetRoleAdminMenu(int EmpID,string RoleCode)
        {

            var data = DMPS.RoleAdminMenus.Where(s => s.RoleCode == RoleCode && s.EmpCode == EmpID.ToString()).SingleOrDefault();
            int  _result = 0;

            if (data == null)
            {
                _result = 0;
            }
            else
            {
                _result = 1;
            };

            return _result;
        }

        public string LoadData2Upload(long DS)
        {
           var data2up = DMPS.DepositAttachments.Where(s => s.DepositID == DS && s.IsDelete == false).ToList();
            for (int i = 0; i < data2up.Count; i++)
            {
                data2up[i].RevisedBy = cApi.apiGetEmployeeDetailList().Where(s => s.EmpID == data2up[i].RevisedBy).SingleOrDefault().DisplayName;
            }

            return data2up.ToObj2Json();
        }

        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            string path = Request.Form["strPath"];
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;

                    for (int i = 0; i < files.Count; i++)
                    {

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
 
                        // Get the complete folder path and store the file inside it. 
                        string strPath = Server.MapPath("~/" + path);
                        bool isExists = Directory.Exists(strPath);
                        if (!isExists)
                            Directory.CreateDirectory(strPath);
                        if (System.IO.File.Exists(Path.Combine(strPath, fname)))
                        {
                            System.IO.File.Delete(Path.Combine(strPath, fname));
                        }

                        file.SaveAs(Path.Combine(strPath, fname));

                    }
                    // Returns message that successfully uploaded  
                    return Json("True");
                }
                catch (Exception ex)
                {
                   
                    return Json("False");
                }
            }
            return Json("True");
        }
        
        public string AddDepositAttachments(string emp_id, long dpsid, string dpspath )
        {
            var EmpID = emp_id;
            var DepositID = dpsid;
            var DepositPath = dpspath;
            var AddDPSATTID = 0;

            try
            {
                    DepositAttachment dps = new DepositAttachment();
                    dps.DepositID = DepositID;
                    dps.FilePath = DepositPath;
                    dps.RevisedBy = EmpID;
                    dps.RevisedDateTime = DateTime.Now;
                    dps.IsDelete = false;

                    DMPS.DepositAttachments.Add(dps);
                    DMPS.SaveChanges();
                    AddDPSATTID = dps.DepositAttachmentID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            
            return AddDPSATTID.ToObj2Json(); 

        }

        public ActionResult DeleteDepositAttachments(int dpsAtid)
        {
           
            var DepositAttDelRec = DMPS.DepositAttachments.SingleOrDefault(s => s.DepositAttachmentID == dpsAtid);

            try
            {

                DepositAttDelRec.IsDelete = true;  

                DMPS.Entry(DepositAttDelRec).State = EntityState.Modified;
                DMPS.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json("False");

            }

            return Json("True");

        }

        //string strPath = Server.MapPath("~/" + path);
        //System.IO.File.Delete(strPath);

        public ActionResult DeleteDepositFiltPath(string dpsFilePath)
        {
            
            try
            {

                string path = dpsFilePath;

                string strPath = Server.MapPath( path);
                System.IO.File.Delete(strPath);
                           
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json("False");

            }

            return Json("True");

        }

        public FileResult LoadDraftDocument(string Filename)
        {
            Filename = Uri.UnescapeDataString(Filename);
            //string pathSource = Server.MapPath("../Report/eBrokerage/Draft/" + Filename + );
            string pathSource = Server.MapPath("~/" + Filename);

            FileStream fsSource = new FileStream(pathSource, FileMode.Open, FileAccess.Read);

            var typefile = Filename.Substring(Filename.LastIndexOf('.') + 1);
            if (typefile == "pdf")
            {
                return new FileStreamResult(fsSource, "application/pdf");
            }
            else
            {
                return new FileStreamResult(fsSource, "image/png, image/jpeg");
            }


        }

        public string LoadDataHistory (long DS)
        {
            var DataHistory2up = DMPS.vw_DepositAttachmentHistory.Where(s => s.DepositID == DS).ToList();
            return DataHistory2up.ToObj2Json();
        }
        
        public ActionResult DepositAttachmentHistory( string emp_id, int staID, int dpsAHID)
        {
            var AttachmentStatusTypeID = staID;
            var EmpID = emp_id;
            var DepositAttHisID = dpsAHID;

            try
            {

                DepositAttachmentHistory DAH = new DepositAttachmentHistory();
                DAH.DepositAttachmentID = DepositAttHisID;
                DAH.AttachmentStatusTypeID = AttachmentStatusTypeID;
                DAH.RevisedBy = EmpID;
                DAH.RevisedDateTime = DateTime.Now;

                DMPS.DepositAttachmentHistories.Add(DAH);
                DMPS.SaveChanges();
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
               return Json("False");
            }

               return Json("True");
        }


        }

}
