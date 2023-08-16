using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Inspinia_MVC5.Models.DMPS;
using Inspinia_MVC5;
using Inspinia_MVC5.API;
using Inspinia_MVC5.Models;
using System.Web;
using System.Data.Entity;

namespace UVG_Main.Controllers.CN_Deposit
{
    public class DMPS_ReservationsRoomController : Controller
    {
        #region -------------Data Connection----------------
        PMdbEntities1 DMPS = new PMdbEntities1();
        cApiPortal cApi = new cApiPortal();
        MASDBEntities MASDB = new MASDBEntities();
        //CADEntities CashAdvanceDB = new CADEntities();
        #endregion

        public class CheckRoomTransAdd
        {
            public int CheckRoomID;
            public string CheckRoomDesc;
            public int CheckQTY;
            public Boolean CheckCheckRoom;
            public int CheckBrokenQTY;
            public Boolean CheckBrokenRoom;
            //public string slug;
            //public string imageUrl;
        }

        #region ----------Page List Data (index)------------
        public ActionResult Index()
        {
            var lstDSBankDS = (from t1 in MASDB.BankTables.Where(s => s.isNonBank == false && s.isDelete == false) select new { BankID = t1.BankID, DisplayName = t1.BankID + " : " + t1.BankName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddl_Bank = new SelectList(lstDSBankDS, "BankID", "DisplayName");
            ViewBag.ddl_RNBank = new SelectList(lstDSBankDS, "BankID", "DisplayName");
            ViewBag.ddl_RSBank = new SelectList(lstDSBankDS, "BankID", "DisplayName");

            ViewBag.ddl_CashierBank = new SelectList(lstDSBankDS, "BankID", "DisplayName");
            ViewBag.ddl_RNCashierBank = new SelectList(lstDSBankDS, "BankID", "DisplayName");
            ViewBag.ddl_RSCashierBank = new SelectList(lstDSBankDS, "BankID", "DisplayName");

            //var lstDSAccountTypeDS = (from t1 in MASDB.BankTypeTables.Where(s => s.IsDelete == false) select new { BankTypeID = t1.BankTypeID, DisplayName = t1.BankTypeNameTH }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddl_AccountType = new SelectList(lstDSAccountTypeDS, "BankTypeID", "DisplayName");

            ViewBag.ddl_Developer = new SelectList(DMPS.DevelopmentTables.OrderBy(s => s.DevelopmentName).ToList(), "DevelopmentID", "DevelopmentName");
            ViewBag.ddl_RNDeveloper = new SelectList(DMPS.DevelopmentTables.OrderBy(s => s.DevelopmentName).ToList(), "DevelopmentID", "DevelopmentName");
            ViewBag.ddl_RSDeveloper = new SelectList(DMPS.DevelopmentTables.OrderBy(s => s.DevelopmentName).ToList(), "DevelopmentID", "DevelopmentName");

            ViewBag.ddl_RentType = new SelectList(DMPS.RentTypeMasterTables.OrderBy(s => s.RentTypeID).ToList(), "RentTypeID", "RentTypeNameTH");
            ViewBag.ddl_RNRentType = new SelectList(DMPS.RentTypeMasterTables.OrderBy(s => s.RentTypeID).ToList(), "RentTypeID", "RentTypeNameTH");
            ViewBag.ddl_RSRentType = new SelectList(DMPS.RentTypeMasterTables.OrderBy(s => s.RentTypeID).ToList(), "RentTypeID", "RentTypeNameTH");


            ViewBag.ddl_Project = new SelectList(DMPS.ProjectTables.Where(s => s.IsDelete == false).OrderBy(s => s.ProjectName).ToList(), "ProjectID", "ProjectName");
            ViewBag.ddl_RNProject = new SelectList(DMPS.ProjectTables.Where(s => s.IsDelete == false).OrderBy(s => s.ProjectName).ToList(), "ProjectID", "ProjectName");
            ViewBag.ddl_RSProject = new SelectList(DMPS.ProjectTables.Where(s => s.IsDelete == false).OrderBy(s => s.ProjectName).ToList(), "ProjectID", "ProjectName");

            var lstProvice = (from t1 in MASDB.ProvinceTables.Where(s => s.PROVINCE_ID != 0) select new { ProviceID = t1.PROVINCE_ID, DisplayName = t1.PROVINCE_NAME }).OrderBy(s => s.ProviceID).ToList();
            ViewBag.ddlProvince = new SelectList(lstProvice, "ProviceID", "DisplayName");
            ViewBag.ddl_RNProvince = new SelectList(lstProvice, "ProviceID", "DisplayName");
            ViewBag.ddl_RSProvince = new SelectList(lstProvice, "ProviceID", "DisplayName");

            var lstCardFrom = (from t1 in MASDB.ProvinceTables.Where(s => s.PROVINCE_ID != 0) select new { ProviceID = t1.PROVINCE_ID, DisplayName = t1.PROVINCE_NAME }).OrderBy(s => s.ProviceID).ToList();
            ViewBag.ddlCardFrom = new SelectList(lstCardFrom, "ProviceID", "DisplayName");
            ViewBag.ddl_RNCardFrom = new SelectList(lstCardFrom, "ProviceID", "DisplayName");
            ViewBag.ddl_RSCardFrom = new SelectList(lstCardFrom, "ProviceID", "DisplayName");

            var lstDistinct = (from t1 in MASDB.AmphurTables.Where(s => s.PROVINCE_ID == 0) select new { DistinctID = t1.AMPHUR_ID, DisplayName = t1.AMPHUR_NAME }).OrderBy(s => s.DistinctID).ToList();
            ViewBag.ddlDistrict = new SelectList(lstDistinct, "DistinctID", "DisplayName");
            ViewBag.ddl_RSDistrict = new SelectList(lstDistinct, "DistinctID", "DisplayName");
            ViewBag.ddl_RNDistrict = new SelectList(lstDistinct, "DistinctID", "DisplayName");

            var lstSubDistinct = (from t1 in MASDB.DistrictTables.Where(s => s.PROVINCE_ID == 0 && s.AMPHUR_ID == 0) select new { SubDistinctID = t1.DISTRICT_ID, DisplayName = t1.DISTRICT_NAME }).OrderBy(s => s.SubDistinctID).ToList();
            ViewBag.ddlSubDistrict = new SelectList(lstSubDistinct, "SubDistinctID", "DisplayName");
            ViewBag.ddl_RNSubDistrict = new SelectList(lstSubDistinct, "SubDistinctID", "DisplayName");
            ViewBag.ddl_RSSubDistrict = new SelectList(lstSubDistinct, "SubDistinctID", "DisplayName");

            var lstUnitType = (from t1 in DMPS.DMPS_UnitTypeTable.Where(s => s.IsDelete == false) select new { UnitTypeID = t1.ID, DisplayName = t1.UnitType }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlDS_UnitType = new SelectList(lstUnitType, "UnitTypeID", "DisplayName");
            ViewBag.ddl_RNDS_UnitType = new SelectList(lstUnitType, "UnitTypeID", "DisplayName");
            ViewBag.ddl_RSDS_UnitType = new SelectList(lstUnitType, "UnitTypeID", "DisplayName");
            ViewBag.ddlUnitType = new SelectList(lstUnitType, "UnitTypeID", "DisplayName");

            ViewBag.ddlUnitStatus = new SelectList(DMPS.UnitStatusMasterTables.OrderBy(s => s.ID).ToList(), "ID", "DescriptionTH");

            //var lstUnitStatus = (from t1 in DMPS.UnitStatusMasterTables select new { StatusID = t1.ID, DisplayName = t1.DescriptionTH }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddlUnitStatus = new SelectList(lstUnitStatus, "StatusID", "DescriptionTH");

            var lstReasonCancel = (from t1 in DMPS.MasReasonCancelForms.Where(s => s.IsDelete == false) select new { ReasonCancelFormID = t1.ReasonCancelFormID, ReasonCancelFormName = t1.ReasonCancelFormName }).OrderBy(s => s.ReasonCancelFormID).ToList();
            ViewBag.ddl_ReasonCancel = new SelectList(lstReasonCancel, "ReasonCancelFormID", "ReasonCancelFormName");

            var qCheckRoomMaster = DMPS.CheckRoomMasterTables;
            ViewBag.lstCheckRoomMaster = qCheckRoomMaster.OrderBy(s => s.Sort).ToList();

            var lstSaleName = (from t1 in DMPS.MasSales select new { SaleCode = t1.SaleCode, DisplayName = t1.SaleName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlSaleName = new SelectList(lstSaleName, "SaleCode", "DisplayName");

            return View();
        }

        public ActionResult LoadDataListView(int IsAdmin, long Empcode, long ReservationsID, DateTime? SDate = null, DateTime? EDate = null, /*bool? _isChkRR = false ,*/int _ddlUnitType = 0  , string _ddlUnitStatus = "" , bool? _chkDate = false)
        {
            var _SQL = "";

            if (IsAdmin == 1 && ReservationsID != 0)
            {
                if (SDate != null)
                {
                    _SQL = "select * from  vw_Reservations where  ReservationsID = " + ReservationsID + "  and  ( DepositStatus = 0 or DepositStatus = 4 or DepositStatus = 6 or DepositStatus = 5 or  DepositStatus = 7 ) and (IsRent = 1 or IsSale = 1) and IsDelete <> 1 and ReservationsDate >= '" + SDate + "' and  ReservationsDate <= '" + EDate + "'  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                }
                else
                {
                    _SQL = "select * from  vw_Reservations where  ReservationsID = " + ReservationsID + "  and  (  DepositStatus = 0 or DepositStatus = 4 or DepositStatus = 6 or DepositStatus = 5 or  DepositStatus = 7 ) and (IsRent = 1 or IsSale = 1) and IsDelete <> 1    order by  ReviseDateTime desc ,CreateDateTime desc  ";

                }
            }

            if (IsAdmin == 1 && ReservationsID == 0)
            {

                if (SDate != null)
                {
                    _SQL = "select * from  vw_Reservations where (  DepositStatus = 0 or DepositStatus = 4 or DepositStatus = 6 or DepositStatus = 5 or  DepositStatus = 7 ) and (IsRent = 1 or IsSale = 1) and IsDelete <> 1   and ReviseDateTime >= '" + SDate + "' and  ReviseDateTime <= '" + EDate + "'   order by  ReviseDateTime desc ,CreateDateTime  desc ";
                }
                else
                {
                    _SQL = "select * from  vw_Reservations where (  DepositStatus = 0 or DepositStatus = 4 or DepositStatus = 6 or DepositStatus = 5 or  DepositStatus = 7 ) and (IsRent = 1 or IsSale = 1) and IsRent = 1 and IsDelete <> 1  order by  ReviseDateTime desc ,CreateDateTime  desc ";

                }
            }

            if (IsAdmin != 1 && ReservationsID == 0)
            {
                if (SDate != null)
                {
                    _SQL = "select * from  vw_Reservations where InChargeBy = '" + Empcode + "'   and (  DepositStatus = 0 or DepositStatus = 4 or DepositStatus = 6 or DepositStatus = 5 or  DepositStatus = 7 ) and (IsRent = 1 or IsSale = 1) and IsDelete <> 1  and ReviseDateTime >= '" + SDate + "' and  ReviseDateTime <= '" + EDate + "'  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                }
                else
                {
                    _SQL = "select * from  vw_Reservations where InChargeBy = '" + Empcode + "'   and (  DepositStatus = 0 or DepositStatus = 4 or DepositStatus = 6 or DepositStatus = 5 or  DepositStatus = 7 ) and (IsRent = 1 or IsSale = 1) and IsDelete <> 1  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                }
            }
            if (IsAdmin != 1 && ReservationsID != 0)
            {

                if (SDate != null)
                {
                    _SQL = "select * from  vw_Reservations where  ReservationsID = " + ReservationsID + "  and  (  DepositStatus = 0 or DepositStatus = 4 or DepositStatus = 6 or DepositStatus = 5  or  DepositStatus = 7 ) and (IsRent = 1 or IsSale = 1) and IsDelete <> 1   and ReviseDateTime >= '" + SDate + "' and  ReviseDateTime <= '" + EDate + "'  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                }
                else
                {
                    _SQL = "select * from  vw_Reservations where  ReservationsID = " + ReservationsID + "  and  (  DepositStatus = 0 or DepositStatus = 4 or DepositStatus = 6 or DepositStatus = 5  or  DepositStatus = 7 ) and (IsRent = 1 or IsSale = 1) and IsDelete <> 1  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                }
            }


            //if (RentID != 0){ _SQL = "select * from  vw_Rental where  RentID = "+ RentID + "  and  ( DepositStatus = 0 or DepositStatus = 1 ) and IsRent = 1 and IsDelete <> 1  order by RentID desc, ReviseDateTime desc "; }
            //else { _SQL = "select * from  vw_Rental where ( DepositStatus = 0 or DepositStatus = 1  or DepositStatus = 3  ) and IsRent = 1 and IsDelete <> 1  order by RentID desc, ReviseDateTime desc "; }


            var q = DMPS.Database.SqlQuery<vw_Reservations>(_SQL);
            //var query = DMPS.vw_Rental.Where(s => ( s.DepositStatus == 0 || s.DepositStatus == 1) && s.IsRent == true && s.IsDelete != true);
            //if (RentID != 0) {
            //    query = query.Where(s => s.RentID == RentID);
            //}
            /*if (_isChkRR != false)
            {

                var query = DMPS.vw_Reservations.Where(s => s.UnitTypeID == 8 );
                if(SDate != null && EDate != null)
                {
                    query = DMPS.vw_Reservations.Where(s => s.UnitTypeID == 8 && (s.RentStartDate >= SDate && s.RentEndDate <= EDate));

                }

                ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                return PartialView();
            } */

            if (_chkDate != false)
            {
                if (_ddlUnitStatus != "Please Select")
                {

                    if (_ddlUnitStatus != "" && _ddlUnitType == 0)
                    {
                        var query = DMPS.vw_Reservations.Where(s => s.DescriptionTH == _ddlUnitStatus);
                        if (SDate != null && EDate != null)
                        {
                            query = DMPS.vw_Reservations.Where(s => s.DescriptionTH == _ddlUnitStatus && (s.RentStartDate >= SDate && s.RentEndDate <= EDate));

                        }

                        ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                        return PartialView();
                    }

                    if (_ddlUnitType != 0 && _ddlUnitStatus != "")
                    {
                        var query = DMPS.vw_Reservations.Where(s => s.DescriptionTH == _ddlUnitStatus && (s.UnitTypeID == _ddlUnitType));
                        if (SDate != null && EDate != null)
                        {
                            query = DMPS.vw_Reservations.Where(s => s.DescriptionTH == _ddlUnitStatus && (s.UnitTypeID == _ddlUnitType) && (s.RentStartDate >= SDate && s.RentEndDate <= EDate));

                        }

                        ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                        return PartialView();
                    }
                }
                if (_ddlUnitType != 0 && _ddlUnitStatus == "Please Select")
                {
                    var query = DMPS.vw_Reservations.Where(s => s.UnitTypeID == _ddlUnitType);
                    if (SDate != null && EDate != null)
                    {
                        query = DMPS.vw_Reservations.Where(s => s.UnitTypeID == _ddlUnitType && (s.RentStartDate >= SDate && s.RentEndDate <= EDate));

                    }

                    ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                    return PartialView();
                }
            }
            else
            {
                if (_ddlUnitStatus != "Please Select")
                {

                    if (_ddlUnitStatus != "" && _ddlUnitType == 0)
                    {
                        var query = DMPS.vw_Reservations.Where(s => s.DescriptionTH == _ddlUnitStatus);

                        ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                        return PartialView();
                    }

                    if (_ddlUnitType != 0 && _ddlUnitStatus != "")
                    {
                        var query = DMPS.vw_Reservations.Where(s => s.DescriptionTH == _ddlUnitStatus && (s.UnitTypeID == _ddlUnitType));

                        ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                        return PartialView();
                    }
                }
                if (_ddlUnitType != 0 && _ddlUnitStatus == "Please Select")
                {
                    var query = DMPS.vw_Reservations.Where(s => s.UnitTypeID == _ddlUnitType);

                    ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                    return PartialView();
                }
                if (_ddlUnitType == 0 && _ddlUnitStatus == "Please Select")
                {
                    var query = DMPS.vw_Reservations.Where(s => s.ReservationsID != 0);

                    ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                    return PartialView();
                }
            }
            
            ViewBag.lstRental = q;
            return PartialView();
        }


        public string GetReservationsData(long? DepositID = 0, long? DSContactID = 0, long? UnitID = 0, long? ReservationsID = 0)
        {
            var result = "";

            var lstDeposit = DMPS.DepositTables.Where(s => s.DepositID == DepositID && s.IsDelete != true).ToList();
            var lstContract = DMPS.vw_CRM_Contract.Where(s => s.ContactsID == DSContactID && s.CustomerTypeId == 1).ToList();
            var lstUnit = DMPS.UnitsTables.Where(s => s.UnitsID == UnitID && s.IsDelete != true).ToList();
            var lstUnitDetails = DMPS.UnitsDetialTables.Where(s => s.UnitsID == UnitID && s.IsDelete != true).ToList();
            var lstReservations = DMPS.ReservationsTables.Where(s => s.ReservationsID == ReservationsID && s.IsDelete != true).ToList();
            var lstBankContact = DMPS.ContactBankTables.Where(s => s.DepositID == DepositID).ToList();



            if (lstBankContact.Count == 0)
            {

                List<ContactBankTable> lsBankContact = new List<ContactBankTable>();
                ContactBankTable qData = new ContactBankTable();
                qData.ContactBankID = 0;
                qData.BankID = "";
                qData.BankTypeID = 1;
                qData.BankNo = "";
                qData.BankName = "";
                qData.ContactsID = DSContactID;
                qData.DepositID = DepositID;


                lsBankContact.Add(qData);
                lstBankContact = lsBankContact;
            }


            #region Case Rent Data is null 
            if (lstReservations.Count == 0)
            {

                List<ReservationsTable> lsReservations = new List<ReservationsTable>();
                ReservationsTable qData = new ReservationsTable();
                qData.ReservationsID = 0;
                qData.ReservationsNo = "New";
                qData.ReservationsDate = DateTime.Now;
                qData.ReservationsAt = "";
                qData.DocRef = "";
                qData.ReservationsStatus = 0;
                qData.DueDateSignContact = DateTime.Now.AddDays(10);
                qData.RVCustomerID = 0;
                qData.RentPeriod = 0;
                qData.RentPrice = 0;
                qData.DayofDue = "";
                qData.RoomInsuranceAmt = 0;
                qData.RoomDepositAmt = 0;
                qData.RentStartDate = DateTime.Now;
                qData.RentEndDate = DateTime.Now;
                qData.TransferDate = DateTime.Now;


                qData.ReservationsAmt = 0;
 
                qData.SaleAmt = 0;
                qData.SalePerSqm = 0;
                qData.CashAmount = 0;
                qData.RemainingAmount = 0;
                qData.ContactAmount = 0;
                qData.RoomInsuranceAmt = 0;
                qData.RoomInsuranceQty = 0;
                qData.RoomDepositAmt = 0;
                qData.RoomDepositQty = 0;
                qData.ReservationsAmt = 0;
  
                qData.Pay_Cash = 0;
                qData.Pay_Cashier = 0;
                qData.Pay_CashierBank = "";
                qData.Pay_CashierNo = "";
                qData.Pay_CashierDate = DateTime.Now;


                qData.Pay_TransferCash = 0;
                qData.Pay_TransferBankNo = "709-2-43146-5";
                qData.Pay_TransferBankName = "บจก. แกรนด์ ยูนิตี้ ดิเวลล็อปเมนท์";

                qData.Pay_TransferCashDate = DateTime.Now;

                qData.Remark = "";
                qData.Remark_Cancel = "";

             
                qData.DepositID = DepositID;


                lsReservations.Add(qData);
                lstReservations = lsReservations;
            }

            #endregion

            var _RTcontract = lstReservations[0].RVCustomerID;
            var lstRVContact = DMPS.vw_CRM_Contract.Where(s => s.ContactsID == _RTcontract).ToList();
            var contact = DMPS.vw_CRM_Contract.Where(s => s.ContactsID == _RTcontract).SingleOrDefault();

            var contact2 = DMPS.CRM_Contacts.Where(s => s.ContactsID == _RTcontract).SingleOrDefault();

            #region Case lstRentContact Data is null 
            if (lstRVContact.Count == 0)
            {

                List<vw_CRM_Contract> lsRentCust = new List<vw_CRM_Contract>();
                vw_CRM_Contract qData = new vw_CRM_Contract();
                qData.ContactsID = 0;

                qData.CitizenIssue = DateTime.Now;
                qData.CitizenExp = DateTime.Now;
                qData.NameFullPre = "";
                qData.CustomerTypeId = 2;
                qData.AddressNo_Work = "";
                qData.Province = "0";
                qData.District = "0";
                qData.SubDistrict = "0";
                qData.UnExpireIDCard = false;
                qData.IsForeign = false;
                lsRentCust.Add(qData);
                lstRVContact = lsRentCust;

            }

            #endregion

            //if (contact != null)
            //{
            //    var _ProvinceID = Convert.ToInt32(contact.Province);
            //    var _DistinctID = Convert.ToInt32(contact.District);
            //    var lstProvice = (from t1 in MASDB.ProvinceTables.Where(s => s.PROVINCE_ID != 0) select new { ProviceID = t1.PROVINCE_ID, DisplayName = t1.PROVINCE_NAME }).OrderBy(s => s.ProviceID).ToList();
            //    ViewBag.ddlProvince = new SelectList(lstProvice, "ProviceID", "DisplayName",0);

            //    var lstDistinct = (from t1 in MASDB.AmphurTables.Where(s => s.PROVINCE_ID == 0) select new { DistinctID = t1.AMPHUR_ID, DisplayName = t1.AMPHUR_NAME }).OrderBy(s => s.DistinctID).ToList();
            //    ViewBag.ddlDistrict = new SelectList(lstDistinct, "DistinctID", "DisplayName",0);

            //    var lstSubDistinct = (from t1 in MASDB.DistrictTables.Where(s => s.PROVINCE_ID == 0 && s.AMPHUR_ID == 0) select new { SubDistinctID = t1.DISTRICT_ID, DisplayName = t1.DISTRICT_NAME }).OrderBy(s => s.SubDistinctID).ToList();
            //    ViewBag.ddlSubDistrict = new SelectList(lstSubDistinct, "SubDistinctID", "DisplayName",0);
            //}


            List<Object> obj = new List<object>();
            obj.Add(lstDeposit);
            obj.Add(lstContract);
            obj.Add(lstUnit);
            obj.Add(lstUnitDetails);
            obj.Add(lstReservations);
            obj.Add(lstBankContact);
            obj.Add(lstRVContact);

            result = obj.ToObj2Json();

            return result;


        }

        public string ReservationsTable(CRM_Contacts _RVCust, ReservationsTable _RV)
        {
            var _custID = SaveCustomer(_RVCust);
            var _RVID = SaveReservations(_RV, _custID);


            if (_RV.IsDelete == true )
            {
                var _dsID = UpdateDepositStatus(_RV.DepositID.GetValueOrDefault(), 0);
            }
            else
            {
                var _dsID = UpdateDepositStatus(_RV.DepositID.GetValueOrDefault(), _RV.ReservationsStatus.GetValueOrDefault());
            }


            return _custID.ToString() + '|' + _RVID.ToString();
        }

        public long CancelReservationsTable(long ReservationsID, long DepositID, long Reviser, bool IsCancel, string Remark_Cancel, string RentID , string _ReasonCancelID, string _ReasonCancelOther ,string _SaleName)
        {
            var _RVID = CancalReservations(ReservationsID, DepositID, Reviser, IsCancel, Remark_Cancel, _SaleName);
            var ContactCancel = ContactReasonCancel(ReservationsID, RentID, _ReasonCancelID, _ReasonCancelOther,_SaleName);
            var _DSID = UpdateDepositStatus(DepositID,  0);

            return ReservationsID;
        }

        
        // cancelform
        public long CancalReservations(long ReservationsID, long DepositID, long Reviser, bool IsCancel, string Remark_Cancel ,string _SaleName)
        {
            
            var _Data = DMPS.ReservationsTables.SingleOrDefault(s => s.ReservationsID == ReservationsID);
            if (_Data != null)
            {
                _Data.Reviser = _SaleName;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.ReservationsStatus = 5;
                _Data.IsDelete = false;
                _Data.IsCancel = IsCancel;
                _Data.Remark_Cancel = Remark_Cancel;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
                //ReservationsID = _Data.ReservationsID;
                //ContactReasonCancel(ReservationsID);
            }

            return _Data.ReservationsID;

        }

        // cancelform add to Contact
        public string ContactReasonCancel(long ReservationsID, string RentID, string _ReasonCancelID, string _ReasonCancelOther, string _SaleName)
        {

            try
            {

                var arrReasonID = _ReasonCancelID.Split(',');
                var arrReasonOther = _ReasonCancelOther.Split(',');
                if (_ReasonCancelID != "")
                {
                    for (var i = 0; i < arrReasonID.Length; i++)
                    {
                        ContactReasonCancel _data = new ContactReasonCancel();
                        _data.ReservationsID = (int)(ReservationsID);
                        if (RentID != "")
                        {
                            _data.RentID = int.Parse(RentID);
                        }
                        else
                        {
                            _data.RentID = null;
                        }

                        _data.ReasonCancelFormID = int.Parse(arrReasonID[i]);
                        _data.Other = (arrReasonOther[i]);
                        _data.Reviser = long.Parse(_SaleName);
                        _data.ReviseDateTime = DateTime.Now;

                        DMPS.ContactReasonCancels.Add(_data);
                    }
                    DMPS.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return "true";

        }
        public long UpdateDepositStatus(long _DepositID, long _RentStatus)
        {

            var _Data = DMPS.DepositTables.SingleOrDefault(s => s.DepositID == _DepositID && s.IsDelete != true);

            if (_Data != null)
            {
                _Data.DepositStatus = Convert.ToInt32(_RentStatus);
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }
            return _Data.DepositID;

        }

        public long SaveCustomer(CRM_Contacts _Contacts)
        {

            var _Data = DMPS.CRM_Contacts.SingleOrDefault(s => s.CitizenID == _Contacts.CitizenID.Replace(" ", "").Replace("-", "").ToLower() && s.CustomerTypeId == _Contacts.CustomerTypeId && s.IsDelete != true);

            if (_Data == null)
            {
                _Data = new CRM_Contacts();
                _Data.CustomerTypeId = _Contacts.CustomerTypeId;
                _Data.Prefix = _Contacts.Prefix;
                _Data.FirstName = _Contacts.FirstName;
                _Data.LastName = _Contacts.LastName;
                _Data.Age = _Contacts.Age;
                _Data.CitizenID = _Contacts.CitizenID.Replace(" ", "").Replace("-", "").ToLower();
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
                _Data.JuristicPerson = _Contacts.JuristicPerson;
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
                _Data.CitizenID = _Contacts.CitizenID.Replace(" ", "").Replace("-", "").ToLower();
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
                _Data.AddressNo_Work = _Contacts.AddressNo_Work;
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
                _Data.ModifyDate = DateTime.Now;
                _Data.ModifyBy = _Contacts.ModifyBy;
                _Data.Remark = _Contacts.Remark;
                _Data.IsDelete = _Contacts.IsDelete;
                _Data.UnExpireIDCard = _Contacts.UnExpireIDCard;
                _Data.IsForeign = _Contacts.IsForeign;
                _Data.JuristicPerson = _Contacts.JuristicPerson;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }

            return _Data.ContactsID;
        }

        public long SaveReservations(ReservationsTable _RVTable, long _custID)
        {

            var _Data = DMPS.ReservationsTables.SingleOrDefault(s => s.ReservationsID == _RVTable.ReservationsID && s.IsDelete != true);
            var _DocNo = GenerateDocuments(0, 0);
 

            if (_Data == null)
            {
                _Data = new ReservationsTable();
                _Data.ReservationsNo = _DocNo;
                _Data.ReservationsDate = _RVTable.ReservationsDate;
                _Data.ReservationsAt = _RVTable.ReservationsAt;
                _Data.DocRef = _RVTable.DocRef;
                _Data.ReservationsType = _RVTable.ReservationsType;
                _Data.ReservationsStatus = _RVTable.ReservationsStatus;
                _Data.RVCustomerID = _custID;
                _Data.DueDateSignContact = _RVTable.DueDateSignContact;
                _Data.DSContactID = _RVTable.DSContactID;
                _Data.RentPeriod = _RVTable.RentPeriod;
                _Data.RentStartDate = _RVTable.RentStartDate;
                _Data.RentEndDate = _RVTable.RentEndDate;
                _Data.RentPrice = _RVTable.RentPrice;
                _Data.DayofDue = _RVTable.DayofDue;
                _Data.RoomInsuranceAmt = _RVTable.RoomInsuranceAmt;
                _Data.RoomDepositAmt = _RVTable.RoomDepositAmt;
                _Data.TransferDate = _RVTable.TransferDate;
                _Data.SaleAmt = _RVTable.SaleAmt;
                _Data.SalePerSqm = _RVTable.SalePerSqm;
                _Data.CashAmount = _RVTable.CashAmount;
                _Data.RemainingAmount = _RVTable.RemainingAmount;
                _Data.ReservationsAmt = _RVTable.ReservationsAmt;
                _Data.RoomInsuranceQty = _RVTable.RoomInsuranceQty;
                _Data.RoomDepositQty = _RVTable.RoomDepositQty;
                _Data.ContactAmount = _RVTable.ContactAmount;
                _Data.Pay_Cash = _RVTable.Pay_Cash;
                _Data.Pay_Cashier = _RVTable.Pay_Cashier;
                _Data.Pay_CashierBank = _RVTable.Pay_CashierBank;
                _Data.Pay_CashierNo = _RVTable.Pay_CashierNo;
                _Data.Pay_CashierDate = _RVTable.Pay_CashierDate;
                _Data.Pay_TransferCash = _RVTable.Pay_TransferCash;
                _Data.Pay_TransferBankNo = _RVTable.Pay_TransferBankNo;
                _Data.Pay_TransferBankName = _RVTable.Pay_TransferBankName;
                _Data.Pay_TransferCashDate = _RVTable.Pay_TransferCashDate;
                _Data.Remark = _RVTable.Remark;
                _Data.Remark_Cancel = _RVTable.Remark_Cancel;
                _Data.Creator = _RVTable.Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = _RVTable.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.IsDelete = _RVTable.IsDelete;
                _Data.DepositID = _RVTable.DepositID;
                _Data.IsContact = _RVTable.IsContact;
                _Data.IsCancel = _RVTable.IsCancel;
                DMPS.ReservationsTables.Add(_Data);
                DMPS.SaveChanges();
            }
            else
            {
                _Data.ReservationsDate = _RVTable.ReservationsDate;
                _Data.ReservationsAt = _RVTable.ReservationsAt;
                _Data.DocRef = _RVTable.DocRef;
                _Data.ReservationsType = _RVTable.ReservationsType;
                _Data.ReservationsStatus = _RVTable.ReservationsStatus;
                _Data.RVCustomerID = _custID;
                _Data.DueDateSignContact = _RVTable.DueDateSignContact;
                _Data.DSContactID = _RVTable.DSContactID;
                _Data.RentPeriod = _RVTable.RentPeriod;
                _Data.RentStartDate = _RVTable.RentStartDate;
                _Data.RentEndDate = _RVTable.RentEndDate;
                _Data.RentPrice = _RVTable.RentPrice;
                _Data.DayofDue = _RVTable.DayofDue;
                _Data.RoomInsuranceAmt = _RVTable.RoomInsuranceAmt;
                _Data.RoomDepositAmt = _RVTable.RoomDepositAmt;
                _Data.TransferDate = _RVTable.TransferDate;
                _Data.SaleAmt = _RVTable.SaleAmt;
                _Data.SalePerSqm = _RVTable.SalePerSqm;
                _Data.CashAmount = _RVTable.CashAmount;
                _Data.RemainingAmount = _RVTable.RemainingAmount;
                _Data.Pay_Cash = _RVTable.Pay_Cash;
                _Data.Pay_Cashier = _RVTable.Pay_Cashier;
                _Data.Pay_CashierBank = _RVTable.Pay_CashierBank;
                _Data.Pay_CashierNo = _RVTable.Pay_CashierNo;
                _Data.Pay_CashierDate = _RVTable.Pay_CashierDate;
                _Data.Pay_TransferCash = _RVTable.Pay_TransferCash;
                _Data.Pay_TransferBankNo = _RVTable.Pay_TransferBankNo;
                _Data.Pay_TransferCashDate = _RVTable.Pay_TransferCashDate;
                _Data.ReservationsAmt = _RVTable.ReservationsAmt;
                _Data.RoomInsuranceQty = _RVTable.RoomInsuranceQty;
                _Data.RoomDepositQty = _RVTable.RoomDepositQty;
                _Data.ContactAmount = _RVTable.ContactAmount;
                _Data.Remark = _RVTable.Remark;
                _Data.Remark_Cancel = _RVTable.Remark_Cancel;
                //_Data.Creator = _RVTable.Creator;
                //_Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = _RVTable.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.IsDelete = _RVTable.IsDelete;
                _Data.DepositID = _RVTable.DepositID;
                _Data.IsContact = _RVTable.IsContact;
                _Data.IsCancel = _RVTable.IsCancel;

                if (_RVTable.IsDelete == true || _RVTable.IsCancel == true)
                {
                    _Data.ReservationsStatus = 5;
                    _Data.Remark_Cancel = "ยกเลิกผ่านระบบ";
                }

                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }

            return _Data.ReservationsID;

        }

        private string GenerateDocuments(int CompanyID = 0, int AppID = 0)
        {
            string result = string.Empty;
            //var _CompanyCode = GetCompanyCode(CompanyID);
            //var _DocumentCode = GetDocumentCode(28);
            var _DocumentCode = "RV";
            //result += _CompanyCode + '-';
            result += _DocumentCode;
            result += DateTime.Now.ToString("yyyyMM-");
            int Year = DateTime.Now.Year;
            int Month = DateTime.Now.Month;
            DateTime StartofMonth = new DateTime(Year, Month, 1, 0, 0, 0);
            DateTime EndofMonth = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month), 23, 59, 59);

            int CountRec;
            //CountRec = DMPS.DepositTables.Where(s => s.CreateDateTime >= StartofMonth && s.CreateDateTime <= EndofMonth && s.CompanyID == CompanyID && s.WorkNO != "").Count() + 1;
            string _DocNo;
            _DocNo = DMPS.ReservationsTables.Where(s => s.IsDelete != true && s.ReservationsNo.Contains(result)).Max(x => x.ReservationsNo);

            if (_DocNo == null)
            { CountRec = 1; }
            else { CountRec = int.Parse(_DocNo.Split('-')[1]) + 1; }

            result += CountRec.ToString("000#");
            return result;
        }

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

        #endregion

        #region -------------Modal Popup------------
        public ActionResult LoadModal()
        {
            return PartialView();
        }

        //-----Blind Data to Modal Popup Json Style-----
        public string LoadData2Modal(long id = 0)
        {
            string result = string.Empty;
            //AdvancePayment data = CashAdvanceDB.AdvancePayments.Where(s => s.AdvancePaymentID == id).SingleOrDefault();

            ////new ticket data = null only
            //if (data == null)
            //{
            //    data = new AdvancePayment();            

            //}
            ////old ticket data = data on ef select
            //else
            //{


            //}

            //result = data.ToObj2Json();
            return result;
        }

        public ActionResult LoadModalRent()
        {
            return PartialView();
        }
        //-----Blind Data to Modal Popup Json Style-----
        public string GetRentalData(long? DepositID = 0, long? DSContactID = 0, long? UnitID = 0, long? RentID = 0 , long ReservationsID = 0,long RVCustomerID = 0)
        {
            var result = "";

            var lstDeposit = DMPS.DepositTables.Where(s => s.DepositID == DepositID && s.IsDelete != true).ToList();
            var lstContract = DMPS.vw_CRM_Contract.Where(s => s.ContactsID == DSContactID && s.CustomerTypeId == 1).ToList();
            var lstUnit = DMPS.UnitsTables.Where(s => s.UnitsID == UnitID && s.IsDelete != true).ToList();
            var lstUnitDetails = DMPS.UnitsDetialTables.Where(s => s.UnitsID == UnitID && s.IsDelete != true).ToList();
            var lstReservations = DMPS.ReservationsTables.Where(s => s.ReservationsID == ReservationsID && s.IsDelete == false).ToList();
            var lstRent = DMPS.RentTables.Where(s => s.RentID == RentID && s.IsDelete != true).ToList();
            var lstBankContact = DMPS.ContactBankTables.Where(s => s.DepositID == DepositID).ToList();



            if (lstBankContact.Count == 0)
            {

                List<ContactBankTable> lsBankContact = new List<ContactBankTable>();
                ContactBankTable qData = new ContactBankTable();
                qData.ContactBankID = 0;
                qData.BankID = "";
                qData.BankTypeID = 1;
                qData.BankNo = "";
                qData.BankName = "";
                qData.ContactsID = DSContactID;
                qData.DepositID = DepositID;


                lsBankContact.Add(qData);
                lstBankContact = lsBankContact;
            }


            #region Case Rent Data is null 
            if (lstRent.Count == 0)
            {

                List<RentTable> lsRent = new List<RentTable>();
                RentTable qData = new RentTable();
                qData.RentID = 0;
                qData.RentNo = "New";
                qData.RentDate = DateTime.Now;
                qData.RentEndDate = DateTime.Now;
                qData.RentStartDate = DateTime.Now;
                qData.DepositID = DepositID;
                qData.RTContractID = 0;
                qData.RoomInsuranceQty = 0;
                qData.RoomInsuranceAmt = 0;
                qData.RoomDepositAmt = 0;
                qData.RoomDepositQty = 0;
                qData.RentPeriod = 0;
                qData.RentPrice = 0;
                qData.RoomFurnitureAmt = 0;
                qData.MulctPrice = 1000;
                qData.RoomFurnitureQty = 0;
                qData.RoomFurnitureAmt = 0;
                qData.RoomFurnitureDepositQty = 0;
                qData.RoomFurnitureDepositAmt = 0;

                qData.Pay_Cash = 0;

                qData.Pay_Cashier = 0;
                qData.Pay_CashierBank = "";
                qData.Pay_CashierNo = "";
                qData.Pay_CashierDate = DateTime.Now;

                qData.Pay_TransferCash = 0;
                qData.Pay_TransferBankNo = "709-2-43146-5";
                qData.Pay_TransferBankName = "บจก. แกรนด์ ยูนิตี้ ดิเวลล็อปเมนท์";

                qData.Pay_TransferCashDate = DateTime.Now;

                lsRent.Add(qData);
                lstRent = lsRent;
            }

            #endregion

            var _RTcontract = lstReservations[0].RVCustomerID;
            var lstRentContact = DMPS.vw_CRM_Contract.Where(s => s.ContactsID == _RTcontract).ToList();
            var contact = DMPS.vw_CRM_Contract.Where(s => s.ContactsID == _RTcontract).SingleOrDefault();
            #region Case lstRentContact Data is null 
            if (lstRentContact.Count == 0)
            {

                List<vw_CRM_Contract> lsRentCust = new List<vw_CRM_Contract>();
                vw_CRM_Contract qData = new vw_CRM_Contract();
                qData.ContactsID = 0;

                qData.CitizenIssue = DateTime.Now;
                qData.CitizenExp = DateTime.Now;
                qData.NameFullPre = "";
                qData.CustomerTypeId = 2;
                qData.AddressNo_Work = "";
                qData.Province = "0";
                qData.District = "0";
                qData.SubDistrict = "0";
                lsRentCust.Add(qData);
                lstRentContact = lsRentCust;

            }

            #endregion

            //if (contact != null)
            //{
            //    var _ProvinceID = Convert.ToInt32(contact.Province);
            //    var _DistinctID = Convert.ToInt32(contact.District);
            //    var lstProvice = (from t1 in MASDB.ProvinceTables.Where(s => s.PROVINCE_ID != 0) select new { ProviceID = t1.PROVINCE_ID, DisplayName = t1.PROVINCE_NAME }).OrderBy(s => s.ProviceID).ToList();
            //    ViewBag.ddlProvince = new SelectList(lstProvice, "ProviceID", "DisplayName",0);

            //    var lstDistinct = (from t1 in MASDB.AmphurTables.Where(s => s.PROVINCE_ID == 0) select new { DistinctID = t1.AMPHUR_ID, DisplayName = t1.AMPHUR_NAME }).OrderBy(s => s.DistinctID).ToList();
            //    ViewBag.ddlDistrict = new SelectList(lstDistinct, "DistinctID", "DisplayName",0);

            //    var lstSubDistinct = (from t1 in MASDB.DistrictTables.Where(s => s.PROVINCE_ID == 0 && s.AMPHUR_ID == 0) select new { SubDistinctID = t1.DISTRICT_ID, DisplayName = t1.DISTRICT_NAME }).OrderBy(s => s.SubDistinctID).ToList();
            //    ViewBag.ddlSubDistrict = new SelectList(lstSubDistinct, "SubDistinctID", "DisplayName",0);
            //}


            List<Object> obj = new List<object>();
            obj.Add(lstDeposit);
            obj.Add(lstContract);
            obj.Add(lstUnit);
            obj.Add(lstUnitDetails);
            obj.Add(lstRent);
            obj.Add(lstBankContact);
            obj.Add(lstRentContact);
            obj.Add(lstReservations);
            
            result = obj.ToObj2Json();

            return result;


        }

        public string SaveRentTable(CRM_Contacts _RentCust, RentTable _Rent, string lstCheckRoom,long RVCustomerID)
        {
            //var _custID = SaveCustomer(_RentCust);
            var _RentID = SaveRent(_Rent, RVCustomerID);
            var _DSID = SaveCheckRoomTrans(lstCheckRoom, _RentID, Convert.ToInt32(_Rent.Creator.GetValueOrDefault()), 2);

            if (_Rent.RentStatus == -1)
            {
                var _dsID = UpdateDepositStatus(_Rent.DepositID.GetValueOrDefault(), 0);
                var _RVID = UpdateReservationsContact(_Rent.ReservationsID.GetValueOrDefault(), false);
            }
            else
            {
                var _RVID = UpdateReservationsContact(_Rent.ReservationsID.GetValueOrDefault(), true);

                var _dsID = UpdateDepositStatus(_Rent.DepositID.GetValueOrDefault(), _Rent.RentStatus.GetValueOrDefault());
            }


            return RVCustomerID.ToString() + '|' + _RentID.ToString();
        }


        public string UpdateReservationsContact(long ReservationsID, Boolean IsContact) {
            var _Data = DMPS.ReservationsTables.SingleOrDefault(s => s.ReservationsID == ReservationsID );

            if (_Data != null)
            {
                _Data.IsContact = IsContact;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }
            return _Data.ReservationsID.ToString();

        }

        public long SaveRent(RentTable _Rent, long _custID)
        {

            var _Data = DMPS.RentTables.SingleOrDefault(s => s.RentID == _Rent.RentID && s.IsDelete != true);
            var _DocNo = GenerateDocumentsRN(0, 0);
            var _RentFurnitureNo = GenerateRentFurnitureNo(0, 0);

            if (_Data == null)
            {
                _Data = new RentTable();
                _Data.RentNo = _DocNo;
                _Data.RentDate = _Rent.RentDate;
                _Data.RentAt = _Rent.RentAt;
                _Data.RentStatus = _Rent.RentStatus;
                _Data.RTContractID = _custID;
                _Data.RentPeriod = _Rent.RentPeriod;
                _Data.RentStartDate = _Rent.RentStartDate;
                _Data.RentEndDate = _Rent.RentEndDate;
                _Data.RentPrice = _Rent.RentPrice;
                _Data.DayofDue = _Rent.DayofDue;
                _Data.RoomInsuranceQty = _Rent.RoomInsuranceQty;
                _Data.RoomInsuranceAmt = _Rent.RoomInsuranceAmt;
                _Data.RoomDepositQty = _Rent.RoomDepositQty;
                _Data.RoomDepositAmt = _Rent.RoomDepositAmt;
                _Data.CheckRoomRemarkRent = _Rent.CheckRoomRemarkRent;
                _Data.RentRemark = _Rent.RentRemark;
                _Data.DocRef = _Rent.DocRef;
                _Data.MulctPrice = _Rent.MulctPrice;
                _Data.Creator = _Rent.Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = _Rent.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.DepositID = _Rent.DepositID;
                _Data.IsDelete = _Rent.IsDelete;
                _Data.RoomFurnitureAmt = _Rent.RoomFurnitureAmt;
                _Data.RoomFurnitureQty = _Rent.RoomFurnitureQty;
                _Data.RoomFurnitureDepositQty = _Rent.RoomFurnitureDepositQty;
                _Data.RoomFurnitureDepositAmt = _Rent.RoomFurnitureDepositAmt;
                _Data.ReservationsID = _Rent.ReservationsID;
                _Data.Pay_Cash = _Rent.Pay_Cash;
                _Data.Pay_Cashier = _Rent.Pay_Cashier;
                _Data.Pay_CashierBank = _Rent.Pay_CashierBank;
                _Data.Pay_CashierNo = _Rent.Pay_CashierNo;
                _Data.Pay_CashierDate = _Rent.Pay_CashierDate;
                _Data.Pay_TransferCash = _Rent.Pay_TransferCash;
                _Data.Pay_TransferBankNo = _Rent.Pay_TransferBankNo;
                _Data.Pay_TransferBankName = _Rent.Pay_TransferBankName;
                _Data.Pay_TransferCashDate = _Rent.Pay_TransferCashDate;
                _Data.CommissionChecked= _Rent.CommissionChecked;
                _Data.CommissionPrice= _Rent.CommissionPrice;

                _Data.Com_Pay_Cash = 0;
                _Data.Com_Pay_Cashier = 0;
                _Data.Com_Pay_CashierBank = "";
                _Data.Com_Pay_CashierNo = "";
                 _Data.Com_Pay_TransferCash = 0;
                _Data.Com_Pay_TransferBankNo = "";
                _Data.Com_Pay_TransferBankName = "";

                DMPS.RentTables.Add(_Data);
                DMPS.SaveChanges();
            }
            else
            {
                _Data.RentDate = _Rent.RentDate;
                _Data.RentAt = _Rent.RentAt;
                _Data.RentStatus = _Rent.RentStatus;
                _Data.RTContractID = _Rent.RTContractID;
                _Data.RentPeriod = _Rent.RentPeriod;
                _Data.RentStartDate = _Rent.RentStartDate;
                _Data.RentEndDate = _Rent.RentEndDate;
                _Data.RentPrice = _Rent.RentPrice;
                _Data.DayofDue = _Rent.DayofDue;
                _Data.RoomInsuranceQty = _Rent.RoomInsuranceQty;
                _Data.RoomInsuranceAmt = _Rent.RoomInsuranceAmt;
                _Data.RoomDepositQty = _Rent.RoomDepositQty;
                _Data.RoomDepositAmt = _Rent.RoomDepositAmt;
                _Data.CheckRoomRemarkRent = _Rent.CheckRoomRemarkRent;
                _Data.RentRemark = _Rent.RentRemark;
                _Data.MulctPrice = _Rent.MulctPrice;
                _Data.Reviser = _Rent.Reviser;
                _Data.DocRef = _Rent.DocRef;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.DepositID = _Rent.DepositID;
                _Data.IsDelete = _Rent.IsDelete;
                _Data.RoomFurnitureAmt = _Rent.RoomFurnitureAmt;
                _Data.RoomFurnitureQty = _Rent.RoomFurnitureQty;
                _Data.RoomFurnitureDepositQty = _Rent.RoomFurnitureDepositQty;
                _Data.RoomFurnitureDepositAmt = _Rent.RoomFurnitureDepositAmt;
                _Data.Pay_Cash = _Rent.Pay_Cash;
                _Data.Pay_Cashier = _Rent.Pay_Cashier;
                _Data.Pay_CashierBank = _Rent.Pay_CashierBank;
                _Data.Pay_CashierNo = _Rent.Pay_CashierNo;
                _Data.Pay_CashierDate = _Rent.Pay_CashierDate;
                _Data.Pay_TransferCash = _Rent.Pay_TransferCash;
                _Data.Pay_TransferBankNo = _Rent.Pay_TransferBankNo;
                _Data.Pay_TransferBankName = _Rent.Pay_TransferBankName;
                _Data.Pay_TransferCashDate = _Rent.Pay_TransferCashDate;
                _Data.ReservationsID = _Rent.ReservationsID;
                _Data.CommissionChecked = _Rent.CommissionChecked;
                _Data.CommissionPrice = _Rent.CommissionPrice;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }

            return _Data.RentID;

        }

        public long SaveCheckRoomTrans(string strCheckRoom, long DepositID, int CreateBy, long _CheckType)
        {
            var RootObjects = JsonConvert.DeserializeObject<List<CheckRoomTransAdd>>(strCheckRoom);

            DMPS.Database.ExecuteSqlCommand("DELETE FROM CheckRoomTrans WHERE ID = " + DepositID + " AND CheckType = " + _CheckType + " ");

            foreach (var rootObject in RootObjects)
            {
                if (rootObject.CheckCheckRoom == true)
                {
                    var _Data = DMPS.CheckRoomTrans.SingleOrDefault(s => s.ID == DepositID && s.CheckRoomID == rootObject.CheckRoomID && s.CheckType == _CheckType && s.IsDelete != true);

                    if (_Data == null)
                    {
                        _Data = new CheckRoomTran();
                        _Data.CheckRoomID = rootObject.CheckRoomID;
                        //_Data.CheckType = _CheckRoomTrans.CheckType;
                        _Data.ID = DepositID;
                        _Data.CheckRoomQty = rootObject.CheckQTY.ToString();
                        _Data.CheckType = _CheckType;
                        _Data.BrokenRoomQty = rootObject.CheckBrokenQTY.ToString();
                        _Data.IsBroken = rootObject.CheckBrokenRoom;
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
                        _Data.BrokenRoomQty = rootObject.CheckBrokenQTY.ToString();
                        _Data.IsBroken = rootObject.CheckBrokenRoom;
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

        private string GenerateRentFurnitureNo(int CompanyID = 0, int AppID = 0)
        {
            string result = string.Empty;
            //var _CompanyCode = GetCompanyCode(CompanyID);
            //var _DocumentCode = GetDocumentCode(28);
            var _DocumentCode = "RT(F)";
            //result += _CompanyCode + '-';
            result += _DocumentCode;
            result += DateTime.Now.ToString("yyyyMM-");
            int Year = DateTime.Now.Year;
            int Month = DateTime.Now.Month;
            DateTime StartofMonth = new DateTime(Year, Month, 1, 0, 0, 0);
            DateTime EndofMonth = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month), 23, 59, 59);

            int CountRec;
            //CountRec = DMPS.DepositTables.Where(s => s.CreateDateTime >= StartofMonth && s.CreateDateTime <= EndofMonth && s.CompanyID == CompanyID && s.WorkNO != "").Count() + 1;
            string _DocNo;
            _DocNo = DMPS.RentTables.Where(s => s.IsDelete != true && s.RentFurnitureNo.Contains(result)).Max(x => x.RentNo);

            if (_DocNo == null)
            { CountRec = 1; }
            else { CountRec = int.Parse(_DocNo.Split('-')[1]) + 1; }

            result += CountRec.ToString("000#");
            return result;
        }

        public ActionResult PrintReportRN(long ReportID)
        {
            try
            {
                ReportClass rptMemo = new ReportClass();
                ReportDocument rptDoc = new ReportDocument();

                //PrintDocument printDocument = new PrintDocument();
                //rptDoc.PrintOptions.PrinterName = printDocument.PrinterSettings.PrinterName;
                //rptDoc.FileName = Server.MapPath("~/Report/PM/rptContractRent_Condo.rpt");
                //rptDoc.SetDataSource(dt);
                //rptDoc.PrintOptions.PrinterName = printDocument.PrinterSettings.PrinterName;
                //rptDoc.PrintToPrinter(1, false, 0, 0);


                rptMemo.FileName = Server.MapPath("~/Report/PM/rptContractRent_12M.rpt");
                rptMemo.Load();
                rptMemo.SetDataSource(GetDataRentalRN(ReportID));


                Stream st = rptMemo.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(st, "application/pdf");


            }
            catch (Exception ex)
            {
                Console.Error.Write(ex);
                throw;
            }

        }

        public DataTable GetDataRentalRN(long id)
        {
            DataTable dt = new DataTable();
            string Conn = System.Configuration.ConfigurationSettings.AppSettings["Cnn_PM"];
            var connection = new SqlConnection(Conn);


            var command = new SqlCommand(" select * from vw_rpt_Rent where RentID = '" + id + "'", connection);
            connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            dt.Load(dr);
            dr.Close();
            return dt;
        }

        public ActionResult PrintReportFurniture(long ReportID)
        {
            try
            {
                ReportClass rptMemo = new ReportClass();
                ReportDocument rptDoc = new ReportDocument();

                //PrintDocument printDocument = new PrintDocument();
                //rptDoc.PrintOptions.PrinterName = printDocument.PrinterSettings.PrinterName;
                //rptDoc.FileName = Server.MapPath("~/Report/PM/rptContractRent_Condo.rpt");
                //rptDoc.SetDataSource(dt);
                //rptDoc.PrintOptions.PrinterName = printDocument.PrinterSettings.PrinterName;
                //rptDoc.PrintToPrinter(1, false, 0, 0);


                rptMemo.FileName = Server.MapPath("~/Report/PM/rptContactFurniture_Ordinary.rpt");
                rptMemo.Load();
                rptMemo.SetDataSource(GetDataRentalRN(ReportID));


                Stream st = rptMemo.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(st, "application/pdf");


            }
            catch (Exception ex)
            {
                Console.Error.Write(ex);
                throw;
            }

        }

        private string GenerateDocumentsRN(int CompanyID = 0, int AppID = 0)
        {
            string result = string.Empty;
            //var _CompanyCode = GetCompanyCode(CompanyID);
            //var _DocumentCode = GetDocumentCode(28);
            var _DocumentCode = "RT";
            //result += _CompanyCode + '-';
            result += _DocumentCode;
            result += DateTime.Now.ToString("yyyyMM-");
            int Year = DateTime.Now.Year;
            int Month = DateTime.Now.Month;
            DateTime StartofMonth = new DateTime(Year, Month, 1, 0, 0, 0);
            DateTime EndofMonth = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month), 23, 59, 59);

            int CountRec;
            //CountRec = DMPS.DepositTables.Where(s => s.CreateDateTime >= StartofMonth && s.CreateDateTime <= EndofMonth && s.CompanyID == CompanyID && s.WorkNO != "").Count() + 1;
            string _DocNo;
            _DocNo = DMPS.RentTables.Where(s => s.IsDelete != true && s.RentNo.Contains(result)).Max(x => x.RentNo);

            if (_DocNo == null)
            { CountRec = 1; }
            else { CountRec = int.Parse(_DocNo.Split('-')[1]) + 1; }

            result += CountRec.ToString("000#");
            return result;
        }

        public ActionResult LoadModalResale()
        {
            return PartialView();
        }

        public string GetResaleData(long DepositID, long DSContactID, long UnitID, long ResaleID, long ReservationsID)
        {
            var result = "";
            var _Deposit = DMPS.DepositTables.SingleOrDefault(s => s.DepositID == DepositID && s.IsDelete == false);
            var _Contract = DMPS.vw_CRM_Contract.SingleOrDefault(s => s.ContactsID == DSContactID);
            var _Unit = DMPS.UnitsTables.SingleOrDefault(s => s.UnitsID == UnitID && s.IsDelete != true);
            var _UnitDetails = DMPS.UnitsDetialTables.SingleOrDefault(s => s.UnitsID == UnitID && s.IsDelete != true);
            var _Reservations = DMPS.ReservationsTables.SingleOrDefault(s => s.ReservationsID == ReservationsID && s.IsDelete == false);
            var _Resale = DMPS.ResaleTables.SingleOrDefault(s => s.ResaleID == ResaleID && s.IsDelete == false);
            var _RVContract = DMPS.vw_CRM_Contract.SingleOrDefault(s => s.ContactsID == _Reservations.RVCustomerID);

            if (_Resale == null)
            {
                _Resale = new ResaleTable();
                _Resale.ResaleID = 0;
                _Resale.ResaleNo = "New";
                _Resale.ResaleDate = DateTime.Now;
                _Resale.DepositID = DepositID;
                _Resale.RSContractID = 0;
                _Resale.ResaleStatus = 0;
                _Resale.SaleAmt = 0;
                _Resale.ContractDiscount = 0;
                _Resale.TransferDiscount = 0;
                _Resale.SaleAmt = 0;
                _Resale.SalePerSqm = 0;
                _Resale.PriceBFVat = 0;
                _Resale.VatAmt = 0;
                _Resale.SalePrice = 0;
                _Resale.NetPrice = 0;

                _Resale.Pay_Cash = 0;

                _Resale.TransferFeeAmt = 0;
                _Resale.ContactAmt = 0;

                _Resale.Pay_Cashier = 0;
                _Resale.Pay_CashierBank = "";
                _Resale.Pay_CashierNo = "";
                _Resale.Pay_CashierDate = DateTime.Now;

                _Resale.Pay_TransferCash = 0;
                _Resale.Pay_TransferBankNo = "022-1-17385-6";
                _Resale.Pay_TransferBankName = "บจก. คอนเน็กซ์ชั่น";
                _Resale.Pay_TransferCashDate = DateTime.Now;

            }
            var _BankContact = DMPS.ContactBankTables.SingleOrDefault(s => s.DepositID == DepositID);
            var _ResaleContact = DMPS.vw_CRM_Contract.SingleOrDefault(s => s.ContactsID == _Resale.RSContractID);
            if (_ResaleContact == null)
            {
                _ResaleContact = new vw_CRM_Contract();
                _ResaleContact.ContactsID = 0;
                _ResaleContact.CitizenIssue = DateTime.Now;
                _ResaleContact.CitizenExp = DateTime.Now;
                _ResaleContact.NameFullPre = "";
                _ResaleContact.CustomerTypeId = 3;
                _ResaleContact.AddressNo_Work = "";
            }
            string _ResaleStatusDesc = DMPS.UnitStatusMasterTables.Where(s => s.StatusID == _Resale.ResaleStatus).SingleOrDefault().DescriptionTH;

            List<Object> obj = new List<object>();
            obj.Add(_Deposit);
            obj.Add(_Contract);
            obj.Add(_Unit);
            obj.Add(_UnitDetails);
            obj.Add(_Resale);
            obj.Add(_BankContact);
            obj.Add(_ResaleContact);
            obj.Add(_ResaleStatusDesc);
            obj.Add(_Reservations);
            obj.Add(_RVContract);

            result = obj.ToObj2Json();

            return result;
        }

        public string getSellerDetail(string IDCard = "" ,int CustomerTypeId = 0)
       {
            string result = string.Empty;

            vw_CRM_Contract IsContact = DMPS.vw_CRM_Contract.FirstOrDefault(s => s.CitizenID == IDCard && s.CustomerTypeId == CustomerTypeId );
            if (IsContact == null)
            {
                IsContact = new vw_CRM_Contract();

                IsContact.ContactsID = 0;
                IsContact.CustomerTypeId = CustomerTypeId;
                IsContact.NameFullPre = "";
                IsContact.Age = null;
                IsContact.CitizenIssue = DateTime.Now;
                IsContact.CitizenExp = DateTime.Now;
                IsContact.Nationality = "";
                IsContact.CitizenProvince = "";
                IsContact.AddressNo = "";
                IsContact.Moo = "";
                IsContact.Soi = "";
                IsContact.Road = "";
                IsContact.Province = "";
                IsContact.District = "";
                IsContact.SubDistrict = "";
                IsContact.ZipCode = "";
                IsContact.Tel1 = "";
                IsContact.AddressNo_Work = "";
                IsContact.Tel2 = "";
                IsContact.Email = "";

            }
            result = IsContact.ToObj2Json();

            return result; //debug
        }


        public string SaveResaleTable(CRM_Contacts _RentCust, ResaleTable _Resale , long RVCustomerID)
        {
           // var _custID = SaveCustomer(_RentCust);
            var _ResaleID = SaveResale(_Resale, RVCustomerID);

            if (_Resale.ResaleStatus == -1)
            {
                var _dsID = UpdateDepositStatusRS(_Resale.DepositID.GetValueOrDefault(), 0);
                var _RVID = UpdateReservationsContact(_Resale.ReservationsID.GetValueOrDefault(), false);
            }
            else { var _dsID = UpdateDepositStatusRS(_Resale.DepositID.GetValueOrDefault(), _Resale.ResaleStatus.GetValueOrDefault());
                var _RVID = UpdateReservationsContact(_Resale.ReservationsID.GetValueOrDefault(), true);
            }

            return RVCustomerID.ToString() + '|' + _ResaleID.ToString();
        }



        public long SaveResale(ResaleTable _Resale, long _custID)
        {
            var _Data = DMPS.ResaleTables.SingleOrDefault(s => s.ResaleID == _Resale.ResaleID && s.IsDelete != true);
            var _DocNo = GenerateDocumentsRS(0, 0);

            if (_Data == null)
            {
                _Data = new ResaleTable();
                _Data.ResaleNo = _DocNo;
                _Data.ResaleDate = _Resale.ResaleDate;
                _Data.ResaleAt = _Resale.ResaleAt;
                _Data.ResaleStatus = _Resale.ResaleStatus;
                _Data.RSContractID = _custID;
                _Data.SalePrice = _Resale.SalePrice;
                _Data.ContractDiscount = _Resale.ContractDiscount;
                _Data.TransferDiscount = _Resale.TransferDiscount;
                _Data.SaleAmt = _Resale.SaleAmt;
                _Data.SalePerSqm = _Resale.SalePerSqm;
                _Data.PriceBFVat = _Resale.PriceBFVat;
                _Data.VatAmt = _Resale.VatAmt;
                _Data.NetPrice = _Resale.NetPrice;
                _Data.ReservationsID = _Resale.ReservationsID;
                _Data.Creator = _Resale.Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = _Resale.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.DocRef = _Resale.DocRef;
                _Data.DepositID = _Resale.DepositID;
                _Data.IsDelete = _Resale.IsDelete;
                _Data.ReservationsID = _Resale.ReservationsID;
                _Data.Pay_Cash = _Resale.Pay_Cash;
                _Data.Pay_Cashier = _Resale.Pay_Cashier;
                _Data.Pay_CashierBank = _Resale.Pay_CashierBank;
                _Data.Pay_CashierNo = _Resale.Pay_CashierNo;
                _Data.Pay_CashierDate = _Resale.Pay_CashierDate;
                _Data.Pay_TransferCash = _Resale.Pay_TransferCash;
                _Data.Pay_TransferBankName = _Resale.Pay_TransferBankName;
                _Data.Pay_TransferBankNo = _Resale.Pay_TransferBankNo;
                _Data.Pay_TransferCashDate = _Resale.Pay_TransferCashDate;
                _Data.Pay_TransferCashDate = _Resale.Pay_TransferCashDate;
                _Data.TransferFeeAmt = _Resale.TransferFeeAmt;
                _Data.ContactAmt = _Resale.ContactAmt;
                DMPS.ResaleTables.Add(_Data);
                DMPS.SaveChanges();
            }
            else
            {
                _Data.ResaleDate = _Resale.ResaleDate;
                _Data.ResaleAt = _Resale.ResaleAt;
                _Data.ResaleStatus = _Resale.ResaleStatus;
                _Data.RSContractID = _custID;
                _Data.SalePrice = _Resale.SalePrice;
                _Data.ContractDiscount = _Resale.ContractDiscount;
                _Data.TransferDiscount = _Resale.TransferDiscount;
                _Data.SaleAmt = _Resale.SaleAmt;
                _Data.SalePerSqm = _Resale.SalePerSqm;
                _Data.PriceBFVat = _Resale.PriceBFVat;
                _Data.VatAmt = _Resale.VatAmt;
                _Data.NetPrice = _Resale.NetPrice;
                _Data.Reviser = _Resale.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.DocRef = _Resale.DocRef;
                _Data.DepositID = _Resale.DepositID;
                _Data.ReservationsID = _Resale.ReservationsID;
                _Data.IsDelete = _Resale.IsDelete;
                _Data.ReservationsID = _Resale.ReservationsID;
                _Data.Pay_Cash = _Resale.Pay_Cash;
                _Data.Pay_Cashier = _Resale.Pay_Cashier;
                _Data.Pay_CashierBank = _Resale.Pay_CashierBank;
                _Data.Pay_CashierNo = _Resale.Pay_CashierNo;
                _Data.Pay_CashierDate = _Resale.Pay_CashierDate;
                _Data.Pay_TransferCash = _Resale.Pay_TransferCash;
                _Data.Pay_TransferBankName = _Resale.Pay_TransferBankName;
                _Data.Pay_TransferBankNo = _Resale.Pay_TransferBankNo;
                _Data.Pay_TransferCashDate = _Resale.Pay_TransferCashDate;
                _Data.TransferFeeAmt = _Resale.TransferFeeAmt;
                _Data.ContactAmt = _Resale.ContactAmt;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }
            return _Data.ResaleID;
        }

        public long UpdateDepositStatusRS(long _DepositID, long _ResaleStatus)
        {
            var _Data = DMPS.DepositTables.SingleOrDefault(s => s.DepositID == _DepositID && s.IsDelete != true);
            if (_Data != null)
            {
                _Data.DepositStatus = Convert.ToInt32(_ResaleStatus);
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }
            return _Data.DepositID;
        }

        private string GenerateDocumentsRS(int CompanyID = 0, int AppID = 0)
        {
            string result = string.Empty;
            //var _CompanyCode = GetCompanyCode(CompanyID);
            //var _DocumentCode = GetDocumentCode(28);
            var _DocumentCode = "RS";
            //result += _CompanyCode + '-';
            result += _DocumentCode;
            result += DateTime.Now.ToString("yyyyMM-");
            int Year = DateTime.Now.Year;
            int Month = DateTime.Now.Month;
            DateTime StartofMonth = new DateTime(Year, Month, 1, 0, 0, 0);
            DateTime EndofMonth = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month), 23, 59, 59);

            int CountRec;
            //CountRec = DMPS.DepositTables.Where(s => s.CreateDateTime >= StartofMonth && s.CreateDateTime <= EndofMonth && s.CompanyID == CompanyID && s.WorkNO != "").Count() + 1;
            string _DocNo;
            _DocNo = DMPS.ResaleTables.Where(s => s.IsDelete != true && s.ResaleNo.Contains(result)).Max(x => x.ResaleNo);

            if (_DocNo == null)
            { CountRec = 1; }
            else { CountRec = int.Parse(_DocNo.Split('-')[1]) + 1; }

            result += CountRec.ToString("000#");
            return result;
        }
        #endregion

        public string LoadData2UploadReservations(long _ReservationsID)
        {
            var ReservationsData = DMPS.ReservationsAttachments.Where(s => s.ReservationsID == _ReservationsID && s.IsDelete == false).ToList();
            for (int i = 0; i < ReservationsData.Count; i++)
            {
                ReservationsData[i].RevisedBy = cApi.apiGetEmployeeDetailList().Where(s => s.EmpID == ReservationsData[i].RevisedBy).SingleOrDefault().DisplayName;
            }

            return ReservationsData.ToObj2Json();
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

        public string AddReservationsAttachments(string emp_id, long Reservationsid, string Reservationspath)
        {
            var EmpID = emp_id;
            var ReservationsID = Reservationsid;
            var ReservationsPath = Reservationspath;
            var AddReservationsATTID = 0;

            try
            {
                ReservationsAttachment rv = new ReservationsAttachment();
                rv.ReservationsID = ReservationsID;
                rv.FilePath = ReservationsPath;
                rv.RevisedBy = EmpID;
                rv.RevisedDateTime = DateTime.Now;
                rv.IsDelete = false;

                DMPS.ReservationsAttachments.Add(rv);
                DMPS.SaveChanges();
                AddReservationsATTID = rv.ReservationsAttachmentID;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }

            return AddReservationsATTID.ToObj2Json();

        }

        public ActionResult DeleteReservationsAttachments(int ReservationsAtid)
        {

            var ReservationsAttDelRec = DMPS.ReservationsAttachments.SingleOrDefault(s => s.ReservationsAttachmentID == ReservationsAtid);

            try
            {
                ReservationsAttDelRec.IsDelete = true;

                DMPS.Entry(ReservationsAttDelRec).State = EntityState.Modified;
                DMPS.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json("False");

            }

            return Json("True");

        }

        public ActionResult DeleteReservationsFiltPath(string ReservationsFilePath)
        {

            try
            {

                string path = ReservationsFilePath;

                string strPath = Server.MapPath(path);
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

        public string LoadDataHistory(long _ReservationsID)
        {
            var DataHistory2up = DMPS.vw_ReservationsAttachmentHistory.Where(s => s.ReservationsID == _ReservationsID).ToList();
            return DataHistory2up.ToObj2Json();
        }      

        public ActionResult ReservationsAttachmentHistory( string emp_id, int staID, int RVAHID)
        {
            var AttachmentStatusTypeID = staID;
            var EmpID = emp_id;
            var ReservationsAttHisID = RVAHID;

            try
            {

                ReservationsAttachmentHistory RVAH = new ReservationsAttachmentHistory();
                RVAH.ReservationsAttachmentID = ReservationsAttHisID;
                RVAH.AttachmentStatusTypeID = AttachmentStatusTypeID;
                RVAH.RevisedBy = EmpID;
                RVAH.RevisedDateTime = DateTime.Now;

                DMPS.ReservationsAttachmentHistories.Add(RVAH);
                DMPS.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json("False");
            }

            return Json("True");
        }

        public string SaveShopDateils(int _RVID, string _EmpID , string _ShopName ,string _ShopAim)
        {
            var SHOPID = DMPS.ReservationsShops.SingleOrDefault(s => s.ReservationsID == _RVID);

            try
            {

                if (SHOPID != null )
                {
                    SHOPID.ShopName = _ShopName;
                    SHOPID.ShopAim = _ShopAim;

                    SHOPID.Reviser = _EmpID;
                    SHOPID.ReviseDateTime = DateTime.Now;
                    SHOPID.IsDelete = false;

                    DMPS.Entry(SHOPID).State = EntityState.Modified;
                }
                else
                {
                    ReservationsShop RVShop = new ReservationsShop();
                    RVShop.ReservationsID = _RVID;
                    RVShop.ShopName = _ShopName;
                    RVShop.ShopAim = _ShopAim;
                    RVShop.Creator = _EmpID;
                    RVShop.CreateDateTime = DateTime.Now;
                    RVShop.Reviser = _EmpID;
                    RVShop.ReviseDateTime = DateTime.Now;
                    RVShop.IsDelete = false;

                    DMPS.ReservationsShops.Add(RVShop);
                }
                
                DMPS.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "False";
            }


            return "True";
        }

        public string LoadShopDateils(int _ReservationsID)
        {
            var loadShop = DMPS.ReservationsShops.Where(s => s.ReservationsID == _ReservationsID).ToList();
            if (loadShop.Count != 0)
            {
                
                return loadShop[0].ToObj2Json();

            }
            return "false";
        }

    }
}