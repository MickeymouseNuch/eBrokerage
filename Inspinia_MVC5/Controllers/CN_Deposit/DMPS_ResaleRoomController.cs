using Inspinia_MVC5;
using Inspinia_MVC5.API;
using Inspinia_MVC5.Models;
using Inspinia_MVC5.Models.DMPS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UVG_Main.Controllers.CN_Deposit
{
    public class DMPS_ResaleRoomController : Controller
    {
        PMdbEntities1 DMPS = new PMdbEntities1();
        cApiPortal cApi = new cApiPortal();
        MASDBEntities MASDB = new MASDBEntities();

        // GET: DMPS_ResaleRoom
        public ActionResult Index()
        {
            var lstDSBankDS = (from t1 in MASDB.BankTables.Where(s => s.isNonBank == false && s.isDelete == false) select new { BankID = t1.BankID, DisplayName = t1.BankID + " : " + t1.BankName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddl_Bank = new SelectList(lstDSBankDS, "BankID", "DisplayName");
            ViewBag.ddl_RSCashierBank = new SelectList(lstDSBankDS, "BankID", "DisplayName");
            
            //var lstDSAccountTypeDS = (from t1 in MASDB.BankTypeTables.Where(s => s.IsDelete == false) select new { BankTypeID = t1.BankTypeID, DisplayName = t1.BankTypeNameTH }).OrderBy(s => s.DisplayName).ToList();
            //ViewBag.ddl_AccountType = new SelectList(lstDSAccountTypeDS, "BankTypeID", "DisplayName");

            ViewBag.ddl_Developer = new SelectList(DMPS.DevelopmentTables.OrderBy(s => s.DevelopmentName).ToList(), "DevelopmentID", "DevelopmentName");
            ViewBag.ddl_RentType = new SelectList(DMPS.RentTypeMasterTables.OrderBy(s => s.RentTypeID).ToList(), "RentTypeID", "RentTypeNameTH");


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


            ViewBag.ddl_Project = new SelectList(DMPS.ProjectTables.Where(s => s.IsDelete == false).OrderBy(s => s.ProjectName).ToList(), "ProjectID", "ProjectName");
            return View();
        }

        public ActionResult LoadDataListView(int IsAdmin,long EmpCode,long ResaleID)
        {

            //var query = DMPS.vw_Resale;
            var query = DMPS.vw_Resale.Where(s => (s.DepositStatus == 0 || s.DepositStatus == 2) && s.IsDelete != true);

            if (IsAdmin != 1 ) {
                query = query.Where(s => s.InChargeBy == EmpCode);
            }

            //if ( ResaleID != 0)
            //{
            //    query = query.Where(s => s.ResaleID == ResaleID);
            //}



            ViewBag.lstDataview = query.OrderByDescending(S => S.ReviseDateTime).ToList();

            return PartialView();
        }

        public ActionResult LoadModal()
        {
            return PartialView();
        }

        public string GetResaleData(long DepositID, long DSContactID, long UnitID, long ResaleID)
        {
            var result = "";
            var _Deposit = DMPS.DepositTables.SingleOrDefault(s => s.DepositID == DepositID && s.IsDelete == false);
            var _Contract = DMPS.vw_CRM_Contract.SingleOrDefault(s => s.ContactsID == DSContactID);
            var _Unit = DMPS.UnitsTables.SingleOrDefault(s => s.UnitsID == UnitID && s.IsDelete != true);
            var _UnitDetails = DMPS.UnitsDetialTables.SingleOrDefault(s => s.UnitsID == UnitID && s.IsDelete != true);
            var _Resale = DMPS.ResaleTables.SingleOrDefault(s => s.ResaleID == ResaleID && s.IsDelete == false);
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

                _Resale.Pay_Cashier = 0;
                _Resale.Pay_CashierBank = "";
                _Resale.Pay_CashierNo = "";
                _Resale.Pay_CashierDate = DateTime.Now;

                _Resale.Pay_TransferCash = 0;
                _Resale.Pay_TransferBankNo = "";
                _Resale.Pay_TransferBankName = "";
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
            string _ResaleStatusDesc = DMPS.UnitStatusMasterTables.Where(s=>s.StatusID == _Resale.ResaleStatus).SingleOrDefault().DescriptionTH;
            
            List<Object> obj = new List<object>();
            obj.Add(_Deposit);
            obj.Add(_Contract);
            obj.Add(_Unit);
            obj.Add(_UnitDetails);
            obj.Add(_Resale);
            obj.Add(_BankContact);
            obj.Add(_ResaleContact);
            obj.Add(_ResaleStatusDesc);
            result = obj.ToObj2Json();

            return result;
        }

        public string SaveResaleTable(CRM_Contacts _RentCust, ResaleTable _Resale)
        {
            var _custID = SaveCustomer(_RentCust);
            var _ResaleID = SaveResale(_Resale, _custID);

            if (_Resale.ResaleStatus == -1)
            { var _dsID = UpdateDepositStatus(_Resale.DepositID.GetValueOrDefault(), 0); }
            else { var _dsID = UpdateDepositStatus(_Resale.DepositID.GetValueOrDefault(), _Resale.ResaleStatus.GetValueOrDefault()); }

            return _custID.ToString() + '|' + _ResaleID.ToString();
        }

        public long UpdateDepositStatus(long _DepositID, long _ResaleStatus)
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

        public long SaveCustomer(CRM_Contacts _Contacts)
        {
            var _Data = DMPS.CRM_Contacts.SingleOrDefault(s => s.CitizenID == _Contacts.CitizenID.Replace(" ", "").Replace("-", "").ToLower() && s.CustomerTypeId == _Contacts.CustomerTypeId && s.IsDelete != true);
            //var _Data = DMPS.CRM_Contacts.SingleOrDefault(s => s.CitizenID == _Contacts.CitizenID && s.IsDelete == false);

            if (_Data == null)
            {
                _Data = new CRM_Contacts();
                _Data.CustomerTypeId = 3;
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
                DMPS.Entry(_Data).State = System.Data.Entity.EntityState.Modified;
                DMPS.SaveChanges();
            }

            return _Data.ContactsID;
        }

        public long SaveResale(ResaleTable _Resale, long _custID)
        {
            var _Data = DMPS.ResaleTables.SingleOrDefault(s => s.ResaleID == _Resale.ResaleID && s.IsDelete != true);
            var _DocNo = GenerateDocuments(0, 0);

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
                _Data.Creator = _Resale.Creator;
                _Data.CreateDateTime = DateTime.Now;
                _Data.Reviser = _Resale.Reviser;
                _Data.ReviseDateTime = DateTime.Now;
                _Data.DocRef = _Resale.DocRef;
                _Data.DepositID = _Resale.DepositID;
                _Data.IsDelete = _Resale.IsDelete;
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
                _Data.IsDelete = _Resale.IsDelete;
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

        private string GenerateDocuments(int CompanyID = 0, int AppID = 0)
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

        public string getSellerDetail(string IDCard = "")
        {
            string result = string.Empty;

            vw_CRM_Contract IsContact = new vw_CRM_Contract();
            IsContact.ContactsID = 0;

            var _Is = DMPS.vw_CRM_Contract.Where(s => s.CitizenID == IDCard).ToList();
            if (_Is.Count >= 1) { IsContact = _Is[0]; }
            result = IsContact.ToObj2Json();

            //vw_CRM_Contract IsContact = DMPS.vw_CRM_Contract.SingleOrDefault(s => s.CitizenID == IDCard);
            //if(IsContact == null) { IsContact = new vw_CRM_Contract(); IsContact.ContactsID = 0; }
            //result = IsContact.ToObj2Json();

            return result;
        }

        //public ActionResult PrintReport()
        //{         
        //    //ReportClass rptMemo = new ReportClass();
        //    //rptMemo.FileName = Server.MapPath("~/Report/CN_Deposit/rpt_withdraw.rpt");
        //    //rptMemo.Load();

        //    //Stream st = rptMemo.ExportToStream(ExportFormatType.PortableDocFormat);
        //    //return File(st, "application/pdf");
        //}

       

    }
}