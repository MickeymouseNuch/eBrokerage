using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Inspinia_MVC5.Models.DMPS;
using Inspinia_MVC5;
using Inspinia_MVC5.API;
using Inspinia_MVC5.Models;
using System.Data.Entity;
using System.Web;

namespace UVG_Main.Controllers.CN_Deposit
{
    public class DMPS_ReleaseRoomController : Controller
    {
        PMdbEntities1 DMPS = new PMdbEntities1();
        MASDBEntities MASDB = new MASDBEntities();
        cApiPortal cApi = new cApiPortal();

        // GET: DMPS_ReleaseRoom
        public ActionResult Index()
        {

       

            var lstDSBankDS = (from t1 in MASDB.BankTables.Where(s => s.isNonBank == false && s.isDelete == false) select new { BankID = t1.BankID, DisplayName = t1.BankID + " : " + t1.BankName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddl_Bank = new SelectList(lstDSBankDS, "BankID", "DisplayName");
            ViewBag.ddl_RNCashierBank = new SelectList(lstDSBankDS, "BankID", "DisplayName");
            ViewBag.ddl_ComRNCashierBank = new SelectList(lstDSBankDS, "BankID", "DisplayName");

            var lstDSAccountTypeDS = (from t1 in MASDB.BankTypeTables.Where(s => s.IsDelete == false) select new { BankTypeID = t1.BankTypeID, DisplayName = t1.BankTypeNameTH }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddl_AccountType = new SelectList(lstDSAccountTypeDS, "BankTypeID", "DisplayName");

            ViewBag.ddl_Developer = new SelectList(DMPS.DevelopmentTables.OrderBy(s => s.DevelopmentName).ToList(), "DevelopmentID", "DevelopmentName");
            ViewBag.ddl_RentType = new SelectList(DMPS.RentTypeMasterTables.OrderBy(s => s.RentTypeID).ToList(), "RentTypeID", "RentTypeNameTH");


            ViewBag.ddl_Project = new SelectList(DMPS.ProjectTables.Where(s => s.IsDelete == false).OrderBy(s => s.ProjectName).ToList(), "ProjectID", "ProjectName");

            var lstProvice = (from t1 in MASDB.ProvinceTables.Where(s => s.PROVINCE_ID != 0) select new { ProviceID = t1.PROVINCE_ID, DisplayName = t1.PROVINCE_NAME }).OrderBy(s => s.ProviceID).ToList();
            ViewBag.ddlProvince = new SelectList(lstProvice, "ProviceID", "DisplayName");

            var lstCardFrom = (from t1 in MASDB.ProvinceTables.Where(s => s.PROVINCE_ID != 0) select new { ProviceID = t1.PROVINCE_ID, DisplayName = t1.PROVINCE_NAME }).OrderBy(s => s.ProviceID).ToList();
            ViewBag.ddlCardFrom = new SelectList(lstCardFrom, "ProviceID", "DisplayName");
            
            var lstDistinct = (from t1 in MASDB.AmphurTables.Where(s => s.PROVINCE_ID == 0) select new { DistinctID = t1.AMPHUR_ID, DisplayName = t1.AMPHUR_NAME }).OrderBy(s => s.DistinctID).ToList();
            ViewBag.ddlDistrict = new SelectList(lstDistinct, "DistinctID", "DisplayName");

            var lstSubDistinct = (from t1 in MASDB.DistrictTables.Where(s => s.PROVINCE_ID == 0 && s.AMPHUR_ID == 0) select new { SubDistinctID = t1.DISTRICT_ID, DisplayName = t1.DISTRICT_NAME }).OrderBy(s => s.SubDistinctID).ToList();
            ViewBag.ddlSubDistrict = new SelectList(lstSubDistinct, "SubDistinctID", "DisplayName");

            var lstUnitType = (from t1 in DMPS.DMPS_UnitTypeTable.Where(s => s.IsDelete == false) select new { UnitTypeID = t1.ID, DisplayName = t1.UnitType }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlDS_UnitType = new SelectList(lstUnitType, "UnitTypeID", "DisplayName");
            ViewBag.ddlUnitType = new SelectList(lstUnitType, "UnitTypeID", "DisplayName");

            ViewBag.ddlUnitStatus = new SelectList(DMPS.UnitStatusMasterTables.OrderBy(s => s.ID).ToList(), "ID", "DescriptionTH");

            var lstReasonCancel = (from t1 in DMPS.MasReasonCancelForms.Where(s => s.IsDelete == false) select new { ReasonCancelFormID = t1.ReasonCancelFormID, ReasonCancelFormName = t1.ReasonCancelFormName }).OrderBy(s => s.ReasonCancelFormID).ToList();
            ViewBag.ddl_ReasonCancel = new SelectList(lstReasonCancel, "ReasonCancelFormID", "ReasonCancelFormName");


            var qCheckRoomMaster = DMPS.CheckRoomMasterTables;
            ViewBag.lstCheckRoomMaster = qCheckRoomMaster.OrderBy(s => s.Sort).ToList();

            return View();
        }

        public ActionResult LoadDataListView(string IsshowTab, int IsAdmin, long Empcode, long RentID, DateTime? SDate = null, DateTime? EDate = null ,/* bool? _isChkR = false,*/ int _ddlUnitType = 0, string _ddlUnitStatus = "" , bool? _chkDate = false)
        {
            ViewBag.IsshowTab = IsshowTab;
            if (IsshowTab == "Lead")
            {
                var _SQL = "";

            if (IsAdmin == 1 && RentID != 0)
            {
                if (SDate != null)
                {
                    _SQL = "select * from  vw_Rental where  RentID = " + RentID + "  and  ( DepositStatus = 0 or DepositStatus = 1 ) and IsRent = 1 and IsDelete <> 1 and ReviseDateTime >= '" + SDate + "' and  ReviseDateTime <= '" + EDate + "'  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                }
                else
                {
                    _SQL = "select * from  vw_Rental where  RentID = " + RentID + "  and  ( DepositStatus = 0 or DepositStatus = 1 ) and IsRent = 1 and IsDelete <> 1    order by  ReviseDateTime desc ,CreateDateTime desc  ";

                }
            }

            if (IsAdmin == 1 && RentID == 0)
            {

                if (SDate != null)
                {
                    _SQL = "select * from  vw_Rental where ( DepositStatus = 0 or DepositStatus = 1  or DepositStatus = 3  ) and IsRent = 1 and IsDelete <> 1   and ReviseDateTime >= '" + SDate + "' and  ReviseDateTime <= '" + EDate + "'   order by  ReviseDateTime desc ,CreateDateTime  desc ";
                }
                else
                {
                    _SQL = "select * from  vw_Rental where ( DepositStatus = 0 or DepositStatus = 1  or DepositStatus = 3  ) and IsRent = 1 and IsDelete <> 1  order by  ReviseDateTime desc ,CreateDateTime  desc ";

                }
            }

            if (IsAdmin != 1 && RentID == 0)
            {
                if (SDate != null)
                {
                    _SQL = "select * from  vw_Rental where InChargeBy = '" + Empcode + "'   and ( DepositStatus = 0 or DepositStatus = 1  or DepositStatus = 3  ) and IsRent = 1 and IsDelete <> 1  and ReviseDateTime >= '" + SDate + "' and  ReviseDateTime <= '" + EDate + "'  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                }
                else
                {
                    _SQL = "select * from  vw_Rental where InChargeBy = '" + Empcode + "'   and ( DepositStatus = 0 or DepositStatus = 1  or DepositStatus = 3  ) and IsRent = 1 and IsDelete <> 1  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                }
            }
            if (IsAdmin != 1 && RentID != 0)
            {

                if (SDate != null)
                {
                    _SQL = "select * from  vw_Rental where  RentID = " + RentID + "  and  ( DepositStatus = 0 or DepositStatus = 1 ) and IsRent = 1 and IsDelete <> 1   and ReviseDateTime >= '" + SDate + "' and  ReviseDateTime <= '" + EDate + "'  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                }
                else
                {
                    _SQL = "select * from  vw_Rental where  RentID = " + RentID + "  and  ( DepositStatus = 0 or DepositStatus = 1 ) and IsRent = 1 and IsDelete <> 1  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                }
            }


            //if (RentID != 0){ _SQL = "select * from  vw_Rental where  RentID = "+ RentID + "  and  ( DepositStatus = 0 or DepositStatus = 1 ) and IsRent = 1 and IsDelete <> 1  order by RentID desc, ReviseDateTime desc "; }
            //else { _SQL = "select * from  vw_Rental where ( DepositStatus = 0 or DepositStatus = 1  or DepositStatus = 3  ) and IsRent = 1 and IsDelete <> 1  order by RentID desc, ReviseDateTime desc "; }


            var q = DMPS.Database.SqlQuery<vw_Rental>(_SQL);
                //if (RentID != 0) {
                //    query = query.Where(s => s.RentID == RentID);
                //}
                
                /*if (_isChkR != false)
                {

                    var query = DMPS.vw_Rental.Where(s => s.UnitTypeID == 8);
                    if (SDate != null && EDate != null)
                    {
                        query = DMPS.vw_Rental.Where(s => s.UnitTypeID == 8 && (s.RentStartDate >= SDate && s.RentEndDate <= EDate));

                    }
                    ViewBag.lstRental = query;

                    return PartialView();

                }*/
                if (_chkDate != false)
                {
                    if (_ddlUnitStatus != "Please Select")
                    {

                        if (_ddlUnitStatus != "" && _ddlUnitType == 0)
                        {
                            var query = DMPS.vw_Rental.Where(s => s.DescriptionTH == _ddlUnitStatus);
                            if (SDate != null && EDate != null)
                            {
                                query = DMPS.vw_Rental.Where(s => s.DescriptionTH == _ddlUnitStatus && (s.RentStartDate >= SDate && s.RentEndDate <= EDate));

                            }

                            ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                            return PartialView();
                        }

                        if (_ddlUnitType != 0 && _ddlUnitStatus != "")
                        {
                            var query = DMPS.vw_Rental.Where(s => s.DescriptionTH == _ddlUnitStatus && (s.UnitTypeID == _ddlUnitType));
                            if (SDate != null && EDate != null)
                            {
                                query = DMPS.vw_Rental.Where(s => s.DescriptionTH == _ddlUnitStatus && (s.UnitTypeID == _ddlUnitType) && (s.RentStartDate >= SDate && s.RentEndDate <= EDate));

                            }

                            ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                            return PartialView();
                        }
                    }
                    if (_ddlUnitType != 0 && _ddlUnitStatus == "Please Select")
                    {
                        var query = DMPS.vw_Rental.Where(s => s.UnitTypeID == _ddlUnitType);
                        if (SDate != null && EDate != null)
                        {
                            query = DMPS.vw_Rental.Where(s => s.UnitTypeID == _ddlUnitType && (s.RentStartDate >= SDate && s.RentEndDate <= EDate));

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
                            var query = DMPS.vw_Rental.Where(s => s.DescriptionTH == _ddlUnitStatus);

                            ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                            return PartialView();
                        }

                        if (_ddlUnitType != 0 && _ddlUnitStatus != "")
                        {
                            var query = DMPS.vw_Rental.Where(s => s.DescriptionTH == _ddlUnitStatus && (s.UnitTypeID == _ddlUnitType));

                            ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                            return PartialView();
                        }
                    }
                    if (_ddlUnitType != 0 && _ddlUnitStatus == "Please Select")
                    {
                        var query = DMPS.vw_Rental.Where(s => s.UnitTypeID == _ddlUnitType);

                        ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                        return PartialView();
                    }
                    if (_ddlUnitType == 0 && _ddlUnitStatus == "Please Select")
                    {
                        var query = DMPS.vw_Rental.Where(s => s.RentID != 0);

                        ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                        return PartialView();
                    }
                }

                ViewBag.lstRental = q;
            }
            //not use
            if (IsshowTab == "Cancel")
            {
                /*var _SQL = "";

                if (IsAdmin == 1 && RentID != 0)
                {
                    if (SDate != null)
                    {
                        _SQL = "select * from  vw_Rental where  RentID = " + RentID + "  and  ( DepositStatus = 0 or DepositStatus = 1 ) and IsRent = 1 and IsDelete <> 1 and ReviseDateTime >= '" + SDate + "' and  ReviseDateTime <= '" + EDate + "'  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                    }
                    else
                    {
                        _SQL = "select * from  vw_Rental where  RentID = " + RentID + "  and  ( DepositStatus = 0 or DepositStatus = 1 ) and IsRent = 1 and IsDelete <> 1    order by  ReviseDateTime desc ,CreateDateTime desc  ";

                    }
                }

                if (IsAdmin == 1 && RentID == 0)
                {

                    if (SDate != null)
                    {
                        _SQL = "select * from  vw_Rental where ( DepositStatus = 0 or DepositStatus = 1  or DepositStatus = 3  ) and IsRent = 1 and IsDelete <> 1   and ReviseDateTime >= '" + SDate + "' and  ReviseDateTime <= '" + EDate + "'   order by  ReviseDateTime desc ,CreateDateTime  desc ";
                    }
                    else
                    {
                        _SQL = "select * from  vw_Rental where ( DepositStatus = 0 or DepositStatus = 1  or DepositStatus = 3  ) and IsRent = 1 and IsDelete <> 1  order by  ReviseDateTime desc ,CreateDateTime  desc ";

                    }
                }

                if (IsAdmin != 1 && RentID == 0)
                {
                    if (SDate != null)
                    {
                        _SQL = "select * from  vw_Rental where InChargeBy = '" + Empcode + "'   and ( DepositStatus = 0 or DepositStatus = 1  or DepositStatus = 3  ) and IsRent = 1 and IsDelete <> 1  and ReviseDateTime >= '" + SDate + "' and  ReviseDateTime <= '" + EDate + "'  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                    }
                    else
                    {
                        _SQL = "select * from  vw_Rental where InChargeBy = '" + Empcode + "'   and ( DepositStatus = 0 or DepositStatus = 1  or DepositStatus = 3  ) and IsRent = 1 and IsDelete <> 1  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                    }
                }
                if (IsAdmin != 1 && RentID != 0)
                {

                    if (SDate != null)
                    {
                        _SQL = "select * from  vw_Rental where  RentID = " + RentID + "  and  ( DepositStatus = 0 or DepositStatus = 1 ) and IsRent = 1 and IsDelete <> 1   and ReviseDateTime >= '" + SDate + "' and  ReviseDateTime <= '" + EDate + "'  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                    }
                    else
                    {
                        _SQL = "select * from  vw_Rental where  RentID = " + RentID + "  and  ( DepositStatus = 0 or DepositStatus = 1 ) and IsRent = 1 and IsDelete <> 1  order by  ReviseDateTime desc ,CreateDateTime desc  ";
                    }
                }
                

                //if (RentID != 0){ _SQL = "select * from  vw_Rental where  RentID = "+ RentID + "  and  ( DepositStatus = 0 or DepositStatus = 1 ) and IsRent = 1 and IsDelete <> 1  order by RentID desc, ReviseDateTime desc "; }
                //else { _SQL = "select * from  vw_Rental where ( DepositStatus = 0 or DepositStatus = 1  or DepositStatus = 3  ) and IsRent = 1 and IsDelete <> 1  order by RentID desc, ReviseDateTime desc "; }
                var q = DMPS.Database.SqlQuery<vw_Rental>(_SQL);
                //if (RentID != 0) {
                //    query = query.Where(s => s.RentID == RentID);
                //}
                

                var q = DMPS.vw_CancelRent.Where(s => s.RentStatus == 5).OrderByDescending(s => s.ReviseDateTime);
                if (SDate != null && EDate != null)
                {
                    q = DMPS.vw_CancelRent.Where(s => s.RentStatus == 5 && (s.RentStartDate >= SDate && s.RentEndDate <= EDate)).OrderByDescending(s => s.ReviseDateTime);

                }


                if (_isChkR != false)
                {

                    var query = DMPS.vw_CancelRent.Where(s => s.UnitTypeID == 8);
                    if (SDate != null && EDate != null)
                    {
                        query = DMPS.vw_CancelRent.Where(s => s.RentStatus == 5 && s.UnitTypeID == 8 && (s.RentStartDate >= SDate && s.RentEndDate <= EDate)).OrderByDescending(s => s.ReviseDateTime);

                    }
                    ViewBag.lstCancelRental = query;

                    return PartialView();

                }
                if (_ddlUnitType != 0 && _ddlUnitStatus == 0)
                {
                    var query = DMPS.vw_Reservations.Where(s => s.UnitTypeID == _ddlUnitType);
                    if (SDate != null && EDate != null)
                    {
                        query = DMPS.vw_Reservations.Where(s => s.UnitTypeID == _ddlUnitType && (s.RentStartDate >= SDate && s.RentEndDate <= EDate));

                    }

                    ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                    return PartialView();
                }

                if (_ddlUnitStatus != 0 && _ddlUnitType == 0)
                {
                    var query = DMPS.vw_Reservations.Where(s => s.UnitSatatusID == _ddlUnitStatus);
                    if (SDate != null && EDate != null)
                    {
                        query = DMPS.vw_Reservations.Where(s => s.UnitSatatusID == _ddlUnitStatus && (s.RentStartDate >= SDate && s.RentEndDate <= EDate));

                    }

                    ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                    return PartialView();
                }

                if (_ddlUnitType != 0 && _ddlUnitStatus != 0)
                {
                    var query = DMPS.vw_Reservations.Where(s => s.UnitSatatusID == _ddlUnitStatus && (s.UnitTypeID == _ddlUnitType));
                    if (SDate != null && EDate != null)
                    {
                        query = DMPS.vw_Reservations.Where(s => s.UnitSatatusID == _ddlUnitStatus && (s.UnitTypeID == _ddlUnitType) && (s.RentStartDate >= SDate && s.RentEndDate <= EDate));

                    }

                    ViewBag.lstRental = query.OrderByDescending(s => s.ReviseDateTime);
                    return PartialView();
                }

                ViewBag.lstCancelRental = q;*/
            }

             return PartialView();
        }



        public ActionResult LoadDataListViewBrokenTrans(long RentID)
        {

            var q = DMPS.CheckRoomBrokenTrans.Where(s => s.RentID == RentID);
            ViewBag.DataListView = q.OrderBy(s => s.BrokenID).ToList();

            return PartialView();
        }

        public ActionResult LoadModal()
        {
            return PartialView();
        }

        public class BrokenTrtans
        {
            //public int? BrokenID;
            public Boolean? SelectBroken;
            public string Descriptions;
            //public string slug;
            //public string imageUrl;
        }

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

        public string getSellerDetail(string IDCard = "")
        {
            string result = string.Empty;

            vw_CRM_Contract IsContact = DMPS.vw_CRM_Contract.SingleOrDefault(s => s.CitizenID == IDCard && s.CustomerTypeId == 2);
            if (IsContact == null) { IsContact = new vw_CRM_Contract(); }
            result = IsContact.ToObj2Json();

            return result;
        }



        public string GetRentalData(long ?DepositID = 0, long ?DSContactID = 0, long ?UnitID=0, long ?RentID=0) {
            var result = "";

            var lstDeposit = DMPS.DepositTables.Where(s => s.DepositID == DepositID && s.IsDelete != true).ToList();
            var lstContract = DMPS.vw_CRM_Contract.Where(s => s.ContactsID == DSContactID && s.CustomerTypeId == 1).ToList();
            var lstUnit = DMPS.UnitsTables.Where(s => s.UnitsID == UnitID && s.IsDelete != true).ToList();
            var lstUnitDetails = DMPS.UnitsDetialTables.Where(s => s.UnitsID == UnitID && s.IsDelete != true).ToList();
            var lstRent = DMPS.RentTables.Where(s => s.RentID == RentID && s.IsDelete != true).ToList();
            var lstBankContact = DMPS.ContactBankTables.Where(s => s.DepositID == DepositID).ToList();
            var lstBrokenPayment = DMPS.CheckRoomBrokenPayments.Where(s => s.RentID == RentID).ToList();


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


            //if (lstBrokenPayment.Count == 0)
            //{

            //    List<CheckRoomBrokenPayment> lslstBrokenPayment = new List<CheckRoomBrokenPayment>();
            //    CheckRoomBrokenPayment qData = new CheckRoomBrokenPayment();
            //    qData.ID = 0;


            //    lslstBrokenPayment.Add(qData);
            //    lstBrokenPayment = lslstBrokenPayment;
            //}


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


                qData.Pay_Cash = 0;

                qData.Pay_Cashier = 0;
                qData.Pay_CashierBank = "";
                qData.Pay_CashierNo = "";
                qData.Pay_CashierDate = DateTime.Now;

                qData.Pay_TransferCash = 0;
                qData.Pay_TransferBankNo = "";
                qData.Pay_TransferBankName = "";
                qData.Pay_TransferCashDate = DateTime.Now;


                lsRent.Add(qData);
                lstRent = lsRent;
            }

            #endregion

            var _RTcontract = lstRent[0].RTContractID;
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
            obj.Add(lstBrokenPayment);
            
            result = obj.ToObj2Json();

            return result;


        }

        public long SaveBrokenTrans(string strBrokenTrans, int RentID, int CreateBy)
        {
            var RootObjects = JsonConvert.DeserializeObject<List<BrokenTrtans>>(strBrokenTrans);

            DMPS.Database.ExecuteSqlCommand("DELETE FROM CheckRoomBrokenTrans WHERE RentID = " + RentID + " ");

            foreach (var rootObject in RootObjects)
            {
                if (rootObject.Descriptions.TrimStart().TrimEnd() != "" )
                {
                    var _Data = DMPS.CheckRoomBrokenTrans.SingleOrDefault(s => s.BrokenID == 0 );

                    if (_Data == null)
                    {
                        _Data = new CheckRoomBrokenTran();
                        _Data.RentID = RentID;
                        _Data.TypeID = 1;
                        _Data.Description = rootObject.Descriptions;
                        _Data.Creator = CreateBy;
                        _Data.CreateDateTime = DateTime.Now;
                        _Data.Reviser = CreateBy;
                        _Data.ReviseDateTime = DateTime.Now;
                        DMPS.CheckRoomBrokenTrans.Add(_Data);
                    }
                    else
                    {
                        _Data.Description = rootObject.Descriptions;
                        _Data.Reviser = CreateBy;
                        _Data.ReviseDateTime = DateTime.Now;
                        DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;

                    }
                }
            }

            DMPS.SaveChanges();

            return RentID;
        }

        public long SaveCheckRoomTrans(string strCheckRoom, long DepositID, int CreateBy, long _CheckType)
        {
            var RootObjects = JsonConvert.DeserializeObject<List<CheckRoomTransAdd>>(strCheckRoom);

            DMPS.Database.ExecuteSqlCommand("DELETE FROM CheckRoomTrans WHERE ID = " + DepositID + " AND CheckType = " + _CheckType + " ");

            // CASE CHECK ROOM BEFORE
            if (_CheckType == 2)
            {
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
                            _Data.BrokenRoomQty = "0";
                            _Data.IsBroken = false;
                            _Data.Creator = CreateBy;
                            _Data.CreateDateTime = DateTime.Now;
                            _Data.Reviser = CreateBy;
                            _Data.ReviseDateTime = DateTime.Now;
                            _Data.IsDelete = false;
                            DMPS.CheckRoomTrans.Add(_Data);
                        }
                    }
                }
                DMPS.SaveChanges();
            }


            // CASE CHECK ROOM AFTER
            if (_CheckType == 3)
            {
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
                            _Data.CheckRoomQty = "0";
                            _Data.CheckType = _CheckType;
                            _Data.BrokenRoomQty = rootObject.CheckQTY.ToString();
                            _Data.IsBroken = rootObject.CheckCheckRoom;
                            _Data.Creator = CreateBy;
                            _Data.CreateDateTime = DateTime.Now;
                            _Data.Reviser = CreateBy;
                            _Data.ReviseDateTime = DateTime.Now;
                            _Data.IsDelete = false;
                            DMPS.CheckRoomTrans.Add(_Data);
                        }
                    }
                }
                DMPS.SaveChanges();
            }

            return DepositID;
        }

        //public ActionResult PrintReport(long ReportID)
        //{
        //    ReportClass rptMemo = new ReportClass();




        //    if (ReportID == 1) { rptMemo.FileName = Server.MapPath("~/Report/CN_Deposit/rptContractRent_12M.rpt"); }
        //    else if (ReportID == 2) { rptMemo.FileName = Server.MapPath("~/Report/CN_Deposit/rptContractRent_Condo.rpt"); }
        //    else if (ReportID == 3) { rptMemo.FileName = Server.MapPath("~/Report/CN_Deposit/rptContractRent_Restaurants.rpt"); }
        //    rptMemo.Load();

        //    Stream st = rptMemo.ExportToStream(ExportFormatType.PortableDocFormat);
        //    return File(st, "application/pdf");




        //}



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
                rptMemo.SetDataSource(GetDataRental(ReportID));


                Stream st = rptMemo.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(st, "application/pdf");


            }
            catch (Exception ex)
            {
                Console.Error.Write(ex);
                throw;
            }

        }

        public ActionResult PrintReport(long ReportID)
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
                rptMemo.SetDataSource(GetDataRental(ReportID));

               
                Stream st = rptMemo.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(st, "application/pdf");


            }
            catch (Exception ex)
            {
                Console.Error.Write(ex);
                throw;
            }

        }

        public DataTable GetDataRental(long id)
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


      public int CompareContactDate(long ?UnitID ,DateTime ?RentStartDate ,DateTime ?RentEndDate , long ? RentID)
        {
            DataTable dt = new DataTable();
            string Conn = System.Configuration.ConfigurationSettings.AppSettings["Cnn_PM"];
            var connection = new SqlConnection(Conn);

            var command = new SqlCommand(" exec CompareContactDate " + UnitID + ",'" + RentStartDate.GetValueOrDefault().ToString("yyyy-MM-dd") + "','"+ RentEndDate.GetValueOrDefault().ToString("yyyy-MM-dd") + "',"+ RentID + "", connection);
                       connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            dt.Load(dr);
            dr.Close();

            if (dt.Rows.Count > 0) { return 1; }
            else { return 0; }
        }

        public long SaveEndRent(string lstCheckRoom, long Reviser, long RentID, DateTime EndDateTime, long DepositID, long ReservationsID = 0 , string _ReasonCancelID = "" , string _ReasonCancelOther = "", string Remark_Cancel = "")
        {

            var _RentID = EndContactRental(Reviser, RentID, EndDateTime , Remark_Cancel);
            //var ContactCancel = ContactReasonCancel( ReservationsID, RentID, _ReasonCancelID, _ReasonCancelOther , Reviser);
            var _DepositID = UpdateDepositStatus(DepositID, 0);
            var _DSID = SaveCheckRoomTrans(lstCheckRoom, _RentID, Convert.ToInt32(Reviser), 2);
            if (ReservationsID != 0) { var _RVID = CancalReservations(ReservationsID, Reviser, 7, "จบสัญญา"); }

            return _RentID;
        }

        public long EndContactRental(long Reviser, long RentID ,DateTime EndDateTime , string Remark_Cancel)
        {
            var _Data = DMPS.RentTables.SingleOrDefault(s => s.RentID == RentID && s.IsDelete != true);

            if (_Data != null)
            {
                _Data.RentStatus = 3;
                _Data.Reviser = Reviser;
                _Data.Remark_Cancel = Remark_Cancel;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.IsEnd = true;
                _Data.EndDateTime = EndDateTime;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }
            return _Data.RentID;

        }

        // cancelform add to Contact
        public string ContactReasonCancel(long ReservationsID, long RentID, string _ReasonCancelID, string _ReasonCancelOther, long? Reviser)
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
                        _data.RentID = int.Parse(RentID.ToString());
                        if (ReservationsID.ToString() == "")
                        {
                            _data.ReservationsID = null;
                        }

                        _data.ReasonCancelFormID = int.Parse(arrReasonID[i]);
                        _data.Other = (arrReasonOther[i]);
                        _data.Reviser = Reviser;
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

        public string SaveRentTable(CRM_Contacts _RentCust, RentTable _Rent, string BrokenTrans, decimal BrokenInsuranceAmt, decimal BrokenAmt, decimal BrokenRemainAmt, string BrokentOther, string CheckRoomChekBeforeTB, string CheckRoomChekAfterTB)
        {

            var _custID = SaveCustomer(_RentCust);
            var _RentID = SaveRent(_Rent, _custID);
            ////var _DSID = SaveCheckRoomTrans(CheckRoomChekBeforeTB, _RentID, Convert.ToInt32(_Rent.Creator.GetValueOrDefault()), 2);

            var _DSID_B = SaveCheckRoomTrans(CheckRoomChekBeforeTB, _RentID, Convert.ToInt32(_Rent.Creator.GetValueOrDefault()), 2);
            var _DSID_A = SaveCheckRoomTrans(CheckRoomChekAfterTB, _RentID, Convert.ToInt32(_Rent.Creator.GetValueOrDefault()), 3);
            var _BrokenTrans = SaveBrokenTrans(BrokenTrans, Convert.ToInt32(_RentID), Convert.ToInt32(_Rent.Creator.GetValueOrDefault()));

            var _IDBrokenInsuranceAmt = SaveBrokenInsuranceAmt(1, "เงินค้ำประกัน", Convert.ToInt32(_RentID), BrokenInsuranceAmt, _Rent.Creator.GetValueOrDefault(), _Rent.Reviser.GetValueOrDefault());
            var _IDSaveBrokenAmt = SaveBrokenAmt(2, "หักค่าเสียหาย", Convert.ToInt32(_RentID), BrokenAmt, _Rent.Creator.GetValueOrDefault(), _Rent.Reviser.GetValueOrDefault());
            var _IDBrokenRemainAmt = SaveBrokenRemainAmt(3, "คงเหลือเงินคืน", Convert.ToInt32(_RentID), BrokenRemainAmt, _Rent.Creator.GetValueOrDefault(), _Rent.Reviser.GetValueOrDefault());
            var _BrokentOther = SaveBrokentOther(99, BrokentOther, Convert.ToInt32(_RentID), 0, _Rent.Creator.GetValueOrDefault(), _Rent.Reviser.GetValueOrDefault());


            if (_Rent.RentStatus == -1 || _Rent.RentStatus == 5)
            {
                var _dsID = UpdateDepositStatus(_Rent.DepositID.GetValueOrDefault(), 0);
                var _RVID = CancalReservations(_Rent.ReservationsID.GetValueOrDefault(), _Rent.Reviser, 5, "ยกเลิกจากหน้าทำสัญญาเช่า");
            }
            else
            {
                var _dsID = UpdateDepositStatus(_Rent.DepositID.GetValueOrDefault(), _Rent.RentStatus.GetValueOrDefault());
            }


            return _custID.ToString() + '|' + _RentID.ToString();
        }


        public long CancalReservations(long ReservationsID, long? Reviser, int StatusDoc, string Remark_Cancel)
        {

            var _Data = DMPS.ReservationsTables.SingleOrDefault(s => s.ReservationsID == ReservationsID);
            if (_Data != null)
            {
                _Data.Reviser = Reviser.ToString();
                _Data.ReviseDateTime = DateTime.Now;
                _Data.ReservationsStatus = StatusDoc;

                if (StatusDoc == 5)
                {
                    _Data.IsDelete = false;
                    _Data.IsCancel = true;
                }

                _Data.Remark_Cancel = Remark_Cancel;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }

            return _Data.ReservationsID;
        }

       


        public long SaveBrokentOther(int TypeID, string PaymentDesc, int RentID, decimal PaymentAmt, long Creator, long Reviser)
        {

            var _Data = DMPS.CheckRoomBrokenPayments.SingleOrDefault(s => s.RentID == RentID && s.TypeID == TypeID);

            if (_Data == null)
            {
                _Data = new CheckRoomBrokenPayment();
                _Data.TypeID = TypeID;
                _Data.RentID = RentID;
                _Data.PaymentDesc = PaymentDesc;
                _Data.PaymentAmt = PaymentAmt;
                _Data.Creator = Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.CheckRoomBrokenPayments.Add(_Data);
            }
            else

            {
                _Data.PaymentDesc = PaymentDesc;
                _Data.Reviser = Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;

            }

            DMPS.SaveChanges();
            return _Data.ID;

        }

        public long SaveBrokenRemainAmt(int TypeID, string PaymentDesc, int RentID, decimal PaymentAmt, long Creator, long Reviser)
        {

            var _Data = DMPS.CheckRoomBrokenPayments.SingleOrDefault(s => s.RentID == RentID && s.TypeID == TypeID);

            if (_Data == null)
            {
                _Data = new CheckRoomBrokenPayment();
                _Data.TypeID = TypeID;
                _Data.RentID = RentID;
                _Data.PaymentDesc = PaymentDesc;
                _Data.PaymentAmt = PaymentAmt;
                _Data.Creator = Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.CheckRoomBrokenPayments.Add(_Data);
            }
            else

            {
                _Data.PaymentAmt = PaymentAmt;
                _Data.Reviser = Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;

            }

            DMPS.SaveChanges();
            return _Data.ID;

        }

        public long SaveBrokenAmt(int TypeID, string PaymentDesc, int RentID, decimal PaymentAmt, long Creator, long Reviser)
        {

            var _Data = DMPS.CheckRoomBrokenPayments.SingleOrDefault(s => s.RentID == RentID && s.TypeID == TypeID);

            if (_Data == null)
            {
                _Data = new CheckRoomBrokenPayment();
                _Data.TypeID = TypeID;
                _Data.RentID = RentID;
                _Data.PaymentDesc = PaymentDesc;
                _Data.PaymentAmt = PaymentAmt;
                _Data.Creator = Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.CheckRoomBrokenPayments.Add(_Data);
            }
            else

            {
                _Data.PaymentAmt = PaymentAmt;
                _Data.Reviser = Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;

            }

            DMPS.SaveChanges();
            return _Data.ID;

        }

        public long SaveBrokenInsuranceAmt(int TypeID, string PaymentDesc ,int RentID, decimal PaymentAmt, long Creator, long Reviser)
        {
          
            var _Data = DMPS.CheckRoomBrokenPayments.SingleOrDefault(s => s.RentID == RentID && s.TypeID == TypeID);

            if (_Data == null)
            {
                _Data = new CheckRoomBrokenPayment();
                _Data.TypeID = TypeID;
                _Data.RentID = RentID;
                _Data.PaymentDesc = PaymentDesc;
                _Data.PaymentAmt = PaymentAmt;
                _Data.Creator = Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.CheckRoomBrokenPayments.Add(_Data);
            }
            else

           {
                _Data.PaymentAmt = PaymentAmt;
                _Data.Reviser = Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
               
            }

            DMPS.SaveChanges();
            return _Data.ID;

        }

        public long UpdateDepositStatus(long _DepositID, long _RentStatus) {

            var _Data = DMPS.DepositTables.SingleOrDefault(s => s.DepositID == _DepositID && s.IsDelete != true);

            if (_Data != null) { 
            _Data.DepositStatus = Convert.ToInt32(_RentStatus);
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
                _Data.CitizenID = _Contacts.CitizenID.Replace(" ","").Replace("-","").ToLower();
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
                _Data.JuristicPerson = _Contacts.JuristicPerson;

                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }

            return _Data.ContactsID;
        }

        public long SaveRent(RentTable _Rent, long _custID)
        {

            var _Data = DMPS.RentTables.SingleOrDefault(s => s.RentID == _Rent.RentID && s.IsDelete != true);
            var _DocNo = GenerateDocuments(0, 0);
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
                _Data.Pay_Cash = _Rent.Pay_Cash;
                _Data.Pay_Cashier = _Rent.Pay_Cashier;
                _Data.Pay_CashierBank = _Rent.Pay_CashierBank;
                _Data.Pay_CashierNo = _Rent.Pay_CashierNo;
                _Data.Pay_CashierDate = _Rent.Pay_CashierDate;
                _Data.Pay_TransferCash = _Rent.Pay_TransferCash;
                _Data.Pay_TransferBankNo = _Rent.Pay_TransferBankNo;
                _Data.Pay_TransferBankName = _Rent.Pay_TransferBankName;
                _Data.Pay_TransferCashDate = _Rent.Pay_TransferCashDate;
                _Data.CommissionChecked = _Rent.CommissionChecked;
                _Data.CommissionPrice = _Rent.CommissionPrice;

 
                _Data.Com_Pay_Cash = _Rent.Com_Pay_Cash;
                _Data.Com_Pay_Cashier = _Rent.Com_Pay_Cashier;
                _Data.Com_Pay_CashierBank = _Rent.Com_Pay_CashierBank;
                _Data.Com_Pay_CashierNo = _Rent.Com_Pay_CashierNo;
                _Data.Com_Pay_CashierDate = _Rent.Com_Pay_CashierDate;
                _Data.Com_Pay_TransferCash = _Rent.Com_Pay_TransferCash;
                _Data.Com_Pay_TransferBankNo = _Rent.Com_Pay_TransferBankNo;
                _Data.Com_Pay_TransferBankName = _Rent.Com_Pay_TransferBankName;
                _Data.Com_Pay_TransferCashDate = _Rent.Com_Pay_TransferCashDate;
                _Data.CopyRentID = _Rent.CopyRentID;
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
                _Data.CommissionChecked = _Rent.CommissionChecked;
                _Data.CommissionPrice = _Rent.CommissionPrice;
                _Data.Com_Pay_Cash = _Rent.Com_Pay_Cash;
                _Data.Com_Pay_Cashier = _Rent.Com_Pay_Cashier;
                _Data.Com_Pay_CashierBank = _Rent.Com_Pay_CashierBank;
                _Data.Com_Pay_CashierNo = _Rent.Com_Pay_CashierNo;
                _Data.Com_Pay_CashierDate = _Rent.Com_Pay_CashierDate;
                _Data.Com_Pay_TransferCash = _Rent.Com_Pay_TransferCash;
                _Data.Com_Pay_TransferBankNo = _Rent.Com_Pay_TransferBankNo;
                _Data.Com_Pay_TransferBankName = _Rent.Com_Pay_TransferBankName;
                _Data.Com_Pay_TransferCashDate = _Rent.Com_Pay_TransferCashDate;
                _Data.CopyRentID = _Rent.CopyRentID;

                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }

            return _Data.RentID;

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

        private string GenerateDocuments(int CompanyID = 0, int AppID = 0)
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

        public long UpdateOldContact(long _RentID, long Creator, long Reviser)
        {

            var _Data = DMPS.RentTables.SingleOrDefault(s => s.RentID == _RentID);

            if (_Data != null)
            {
                _Data.RentStatus = 3;
                _Data.Reviser = Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }
            return _Data.RentID;
        }


        public long UpdateRateCopyContact(long _RentID,Decimal _Rate = 0, long Creator =0, long Reviser = 0)
        {

            var _Data = DMPS.RentTables.SingleOrDefault(s => s.RentID == _RentID);
            var _Data2 = DMPS.DepositTables.SingleOrDefault(s => s.DepositID == _Data.DepositID);

            if (_Data2 != null)
            {
                _Data2.RateComCopyContact = _Rate;
                _Data2.Reviser = Reviser;
                _Data2.ReviseDateTime = DateTime.Now;
                DMPS.Entry(_Data2).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }
            return _Data.RentID;
        }

        public string LoadData2UploadRent(long _RentID)
        {
            var RentDaTa = DMPS.RentAttachments.Where(s => s.RentID == _RentID && s.IsDelete == false).ToList();
            for (int i = 0; i < RentDaTa.Count; i++)
            {
                RentDaTa[i].RevisedBy = cApi.apiGetEmployeeDetailList().Where(s => s.EmpID == RentDaTa[i].RevisedBy).SingleOrDefault().DisplayName;
            }

            return RentDaTa.ToObj2Json();
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

        public string AddRentAttachments(string emp_id, long Rentid, string Rentpath)
        {
            var EmpID = emp_id;
            var RentID = Rentid;
            var RentPath = Rentpath;
            var AddRentATTID = 0;


            try
            {
                RentAttachment rn = new RentAttachment();
                rn.RentID = RentID;
                rn.FilePath = RentPath;
                rn.RevisedBy = EmpID;
                rn.RevisedDateTime = DateTime.Now;
                rn.IsDelete = false;

                DMPS.RentAttachments.Add(rn);
                DMPS.SaveChanges();
                AddRentATTID = rn.RentAttachmentID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return AddRentATTID.ToObj2Json();

        }

        public ActionResult DeleteRentAttachments(int RentAtid)
        {

            var RentAttDelRec = DMPS.RentAttachments.SingleOrDefault(s => s.RentAttachmentID == RentAtid);

            try
            {
                RentAttDelRec.IsDelete = true;

                DMPS.Entry(RentAttDelRec).State = EntityState.Modified;
                DMPS.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json("False");

            }

            return Json("True");

        }


        public ActionResult DeleteRentFiltPath(string RentFilePath)
        {

            try
            {

                string path = RentFilePath;

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

        public string LoadDataHistory(long _RentID)
        {
            var DataHistory2up = DMPS.vw_RentAttachmentHistory.Where(s => s.RentID == _RentID).ToList();
            return DataHistory2up.ToObj2Json();
        }

        public ActionResult RentAttachmentHistory( string emp_id, int staID, int RentAHID)
        {
            var AttachmentStatusTypeID = staID;
            var EmpID = emp_id;
            var RentAttHisID = RentAHID;

            try
            {

                RentAttachmentHistory RNAH = new RentAttachmentHistory();
                RNAH.RentAttachmentID = RentAttHisID;
                RNAH.AttachmentStatusTypeID = AttachmentStatusTypeID;
                RNAH.RevisedBy = EmpID;
                RNAH.RevisedDateTime = DateTime.Now;

                DMPS.RentAttachmentHistories.Add(RNAH);
                DMPS.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json("False");
            }

            return Json("True");
        }

        public string LoadShopDateils(int ReservationsID)
        {
            var loadShop = DMPS.ReservationsShops.Where(s => s.ReservationsID == ReservationsID).ToList();
            if (loadShop.Count != 0)
            {

                return loadShop[0].ToObj2Json();

            }
            return "false";
        }


    }
}