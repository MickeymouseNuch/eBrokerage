using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inspinia_MVC5.Models;
using Inspinia_MVC5.Models.MainClass;
using Inspinia_MVC5.Models.CashAdvance;
//using Inspinia_MVC5.ws_SyncMAS;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Inspinia_MVC5.API;

namespace Inspinia_MVC5.Controllers.CashAdvance
{
    public class CashAdvance_ManageController : Controller
    {
        //MASDBEntities MASDB = new MASDBEntities();
        CADEntities CashAdvanceDB = new CADEntities();
        Email email = new Email();
        cApiPortal cApi = new cApiPortal();
        cApiCashAdvance cApiCA = new cApiCashAdvance();

        string KeyOfWs = "UVG";

        #region ----------Page List Data (index)------------
        public ActionResult Index()
        {
            var lstEmployees = (from t1 in cApi.apiGetEmployeeDetailList() where t1.IsActive != false select new { EmpID = t1.EmpID, DisplayName = t1.EmpID + " : " + t1.DisplayName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlPaymentOwner = new SelectList(lstEmployees, "EmpID", "DisplayName");

            var lstCompany = cApi.apiGetCompanyList();
            ViewBag.ddlCompany = new SelectList(lstCompany.OrderBy(s => s.CompanyName).ToList(), "CompanyId", "CompanyName");
            ViewBag.ddlCompanyOfEmp = new SelectList(lstCompany.OrderBy(s => s.CompanyName).ToList(), "CompanyId", "CompanyName");
            
            var lstDepartmentsTables = (from t1 in cApi.apiGetDepartmentList() select new { DeptId = t1.DeptId, CostCenter = t1.CostCenter + " : " + t1.DeptDescription }).OrderBy(s => s.CostCenter).ToList();
            ViewBag.ddlCostCenter = new SelectList(lstDepartmentsTables, "DeptId", "CostCenter");

            ViewBag.ddlDepartment = new SelectList(cApi.apiGetDepartmentList(), "DeptCode", "DeptDescription");
            ViewBag.ddlPosition = new SelectList(cApi.apiGetPositionList().OrderBy(s => s.PositionName).ToList(), "PositionID", "PositionName");
            
            var lstProject = (from t1 in cApi.apiGetProjectList() where t1.isDelete != true select new { ProjectID = t1.ProjectID, ProjectName = t1.ProjectID + " : " + t1.ProjectName }).OrderBy(s => s.ProjectID).ToList();
            ViewBag.ddlProject = new SelectList(lstProject, "ProjectID", "ProjectName");

            ViewBag.ddlPaymentType = new SelectList(CashAdvanceDB.PaymentTypeTables.OrderBy(s => s.PaymentNameTH).ToList(), "PaymentTypeID", "PaymentNameTH");

            var lstBudgetAccountTables = (from t1 in cApiCA.apiGetBudgetAccountList() where t1.IsDelete != true select new { BudgetAccountID = t1.BudgetAccountID, BudgetAccountNameTH = t1.BudgetAccountCode + " : " + t1.BudgetAccountNameTH }).OrderBy(s => s.BudgetAccountNameTH).ToList();
            ViewBag.ddlBudgetAccount = new SelectList(lstBudgetAccountTables, "BudgetAccountID", "BudgetAccountNameTH");
            
            var lstBudgetModelTables = (from t1 in cApiCA.apiGetBudgetModelList() where t1.IsDelete != true select new { BudgetModelID = t1.BudgetModelID, BudgetModelNameTH = t1.BudgetModelCode + " : " + t1.BudgetModelNameTH }).OrderBy(s => s.BudgetModelNameTH).ToList();
            ViewBag.ddlBudgetModel = new SelectList(lstBudgetModelTables, "BudgetModelID", "BudgetModelNameTH");

            ViewBag.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ViewBag.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            return View();
        }

        public ActionResult LoadDataListView(string EmployeeID, string ParentDeptID, DateTime StartDate, DateTime EndDate, string ParentDeptCode)
        {
            long _emp = EmployeeID.ToLong();
            string _DeptID = ParentDeptID;
            
            ConfigCAD _config = CashAdvanceDB.ConfigCADs.Where(s => s.DeptCode == ParentDeptCode && s.isDelete == false).SingleOrDefault();
            List<vw_AdvancePaymentList> LstCashAdvance = new List<vw_AdvancePaymentList>();
            if (_config == null)
            {
                LstCashAdvance = CashAdvanceDB.vw_AdvancePaymentList.Where(s => (s.PaymentOwner == EmployeeID || s.Creator == _emp) && s.DocumentDate >= StartDate && s.DocumentDate <= EndDate).OrderByDescending(s => s.DocumentDate).ToList();
            }
            else if (_config.IsFN == true)
            {
                LstCashAdvance = CashAdvanceDB.vw_AdvancePaymentList.Where(s => s.DocumentDate >= StartDate && s.DocumentDate <= EndDate && (s.DocumentStatusID != 0 || s.PaymentOwner == EmployeeID || s.Creator == _emp)).OrderByDescending(s => s.DocumentDate).ToList();
            }
            else
            {
                LstCashAdvance = CashAdvanceDB.vw_AdvancePaymentList.Where(s => (s.PaymentOwner == EmployeeID || s.Creator == _emp) && s.DocumentDate >= StartDate && s.DocumentDate <= EndDate).OrderByDescending(s => s.DocumentDate).ToList();
            }
            ViewBag.LstCashAdvance = LstCashAdvance;
            return PartialView();
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
            var EmpID = Request["EmpID"].ToString();
            var ComID = Request["ComID"].ToString();
            var ParentDeptID = Request["ParentDeptID"].ToString();
            var ParentDeptCode = Request["ParentDeptCode"].ToString();
            var DeptID = Request["DeptID"].ToString().ToLong();
            var PositionID = Request["PositionID"].ToString();

            #region Load AdvancePayment Data---------------------------------------------------
            AdvancePayment dataAvp = CashAdvanceDB.AdvancePayments.Where(s => s.AdvancePaymentID == id).SingleOrDefault();
            //new ticket data = null only
            if (dataAvp == null)
            {
                dataAvp = new AdvancePayment();
                dataAvp.AdvancePaymentID = 0;
                dataAvp.AdvancePaymentNO = "-";
                dataAvp.DocumentDate = DateTime.Now;
                dataAvp.PaymentOwner = EmpID;
                dataAvp.CompanyId = ComID.ToLong();

                dataAvp.DeptofCost = ParentDeptID.ToLong();
                dataAvp.DeptofCostCode = ParentDeptCode;
                dataAvp.PositionTransID = PositionID.ToLong();
                dataAvp.ReceiveCashDate = DateTime.Now;
                dataAvp.ClearCashDate = DateTime.Now.AddDays(7);
                dataAvp.DocumentStatusID = 0;
                dataAvp.Creator = EmpID.ToLong();
            }
            //old ticket data = data on ef select
            else
            {

            }
            #endregion

            #region Load Refund Data---------------------------------------------------
            RefundTable dataRft = CashAdvanceDB.RefundTables.Where(s => s.AdvancePaymentID == id && s.IsDelete != true).SingleOrDefault();
            if (dataRft == null)
            {
                dataRft = new RefundTable();
            }
            else
            {

            }


            #endregion

            #region Load Refund Data---------------------------------------------------
            List<RefundTran> lstRft = CashAdvanceDB.RefundTrans.Where(s => s.RefundID == dataRft.RefundID && s.IsDelete != true).ToList();
            if (lstRft.Count() == 0)
            {
                RefundTran foo = new RefundTran();
                foo.RefundDetail = "";
                foo.Amount = 0;
                foo.Withholding_Tax = 0;
                foo.CreateDate = DateTime.Now;
                lstRft.Add(foo);
            }
            #endregion

            List<Object> obj = new List<object>();
            obj.Add(dataAvp);
            obj.Add(dataRft);
            obj.Add(lstRft);

            result = obj.ToObj2Json();

            return result;
        }
        #endregion

        #region Save Data-------------------------------------------------------
        //Save รายละเอียดผู้ขอเบิก
        public long SaveAdvancePayment(AdvancePayment data)
        {
            long _IsAdvancePaymentID = 0;
            AdvancePayment avp = CashAdvanceDB.AdvancePayments.Where(s => s.AdvancePaymentID == data.AdvancePaymentID).SingleOrDefault();
            if (avp == null)
            {
                avp = new AdvancePayment();

                if (data.DocumentStatusID == 0) { avp.AdvancePaymentNO = ""; }
                else { avp.AdvancePaymentNO = GenerateAdvancePaymentNO(data.CompanyId.GetValueOrDefault()); }
                //avp.AdvancePaymentNO = GenerateAdvancePaymentNO();
                avp.DocumentDate = DateTime.Now;
                //avp.DeptofCost = data.DeptofCost;
                avp.PaymentOwner = data.PaymentOwner;
                avp.CompanyId = data.CompanyId;
                avp.PositionTransID = data.PositionTransID;
                //avp.DeptId = data.DeptId;
                avp.ProjectID = data.ProjectID;
                avp.BudgetAccountID = data.BudgetAccountID;
                avp.BudgetModelID = data.BudgetModelID;
                avp.Amount = data.Amount;
                avp.PaymentRemark = data.PaymentRemark;
                avp.ReceiveCashDate = data.ReceiveCashDate;
                avp.PaymentTypeID = data.PaymentTypeID;
                avp.ClearCashDate = data.ClearCashDate;
                avp.Creator = data.Creator;
                avp.CreateDate = DateTime.Now;
                avp.Reviser = data.Reviser;
                avp.ReviserDate = DateTime.Now;
                avp.IsDelete = false;
                avp.DeptCode = data.DeptCode;
                avp.DeptofCostCode = data.DeptofCostCode;
                avp.DocumentStatusID = data.DocumentStatusID;
                CashAdvanceDB.AdvancePayments.Add(avp);
                CashAdvanceDB.SaveChanges();
                _IsAdvancePaymentID = CashAdvanceDB.AdvancePayments.Max(s => s.AdvancePaymentID);
            }
            else
            {
                if (avp.AdvancePaymentNO == "") { avp.AdvancePaymentNO = GenerateAdvancePaymentNO(data.CompanyId.GetValueOrDefault()); }

                //avp.DeptofCost = data.DeptofCost;
                avp.PaymentOwner = data.PaymentOwner;
                avp.CompanyId = data.CompanyId;
                avp.PositionTransID = data.PositionTransID;
                //avp.DeptId = data.DeptId;
                avp.ProjectID = data.ProjectID;
                avp.BudgetAccountID = data.BudgetAccountID;
                avp.BudgetModelID = data.BudgetModelID;
                avp.Amount = data.Amount;
                avp.PaymentRemark = data.PaymentRemark;
                avp.ReceiveCashDate = data.ReceiveCashDate;
                avp.PaymentTypeID = data.PaymentTypeID;
                avp.ClearCashDate = data.ClearCashDate;
                avp.Reviser = data.Reviser;
                avp.ReviserDate = DateTime.Now;
                avp.DocumentStatusID = data.DocumentStatusID;
                avp.DeptCode = data.DeptCode;
                avp.DeptofCostCode = data.DeptofCostCode;
                CashAdvanceDB.SaveChanges();
                _IsAdvancePaymentID = avp.AdvancePaymentID;
            }

            if (avp.DocumentStatusID == 1)
            {
                //SendMail(avp);
            }
            return _IsAdvancePaymentID;
        }

        //Save ข้อมูลการอนุมัติการขอเบิก
        public void SavePV(AdvancePayment data)
        {
            AdvancePayment avp = CashAdvanceDB.AdvancePayments.Where(s => s.AdvancePaymentID == data.AdvancePaymentID).SingleOrDefault();
            avp.PVNo = data.PVNo;
            avp.ChequeDateIn = data.ChequeDateIn;
            avp.ChequeDate = data.ChequeDate;
            avp.Reviser = data.Reviser;
            avp.ReviserDate = DateTime.Now;
            avp.DocumentStatusID = 2;
            CashAdvanceDB.SaveChanges();
            //SendMail(avp);
        }

        //Save RefundTable
        public void SaveRefundTable(RefundTable data)
        {
            RefundTable rft = CashAdvanceDB.RefundTables.Where(s => s.RefundID == data.RefundID).SingleOrDefault();
            if (rft == null)
            {
                rft = new RefundTable();
                rft.RefundNO = "";//generate
                rft.AdvancePaymentID = data.AdvancePaymentID;
                rft.Creator = data.Creator;
                rft.CreateDate = DateTime.Now;
                rft.IsDelete = false;
                CashAdvanceDB.RefundTables.Add(rft);
                CashAdvanceDB.SaveChanges();
            }
            else
            {
                rft.Reviser = data.Reviser;
                rft.ReviserDate = DateTime.Now;
                CashAdvanceDB.SaveChanges();
            }
        }

        //Save การมารับเงินของผู้ขอเบิก
        public void SaveReceiveCheque(string LstAdvancePaymentID, long? Revizer)
        {
            var _LstAdvancePaymentID = LstAdvancePaymentID.Split('|');
            for (int i = 0; i < (_LstAdvancePaymentID.Count() - 1); i++)
            {
                long _FooAdvancePaymentID = _LstAdvancePaymentID[i].ToLong();
                AdvancePayment avp = CashAdvanceDB.AdvancePayments.Where(s => s.AdvancePaymentID == _FooAdvancePaymentID).SingleOrDefault();
                avp.Reviser = Revizer;
                avp.ReviserDate = DateTime.Now;
                avp.DocumentStatusID = 3;
                CashAdvanceDB.SaveChanges();
                SendMail(avp);
            }
        }

        //Save รายการเคลียร์เงินทดรองจ่าย
        public long SaveClearDetail(RefundTable data, string LstRefundDetail, string LstRefundDetailAmount, string LstWithholding_Tax)
        {
            //Save RefundTable
            RefundTable rft = CashAdvanceDB.RefundTables.Where(s => s.RefundID == data.RefundID).SingleOrDefault();
            long? _RefundID = 0;
            if (rft == null)
            {
                rft = new RefundTable();
                rft.RefundNO = "";//generate
                rft.AdvancePaymentID = data.AdvancePaymentID;
                rft.Creator = data.Creator;
                rft.CreateDate = DateTime.Now;
                rft.IsDelete = false;
                CashAdvanceDB.RefundTables.Add(rft);
                CashAdvanceDB.SaveChanges();
                _RefundID = CashAdvanceDB.RefundTables.Max(s => s.RefundID);
            }
            else
            {
                rft.Reviser = data.Reviser;
                rft.ReviserDate = DateTime.Now;
                CashAdvanceDB.SaveChanges();
                _RefundID = rft.RefundID;
            }

            //Save RefundTrans
            CashAdvanceDB.RefundTrans.RemoveRange(CashAdvanceDB.RefundTrans.Where(s => s.RefundID == _RefundID));
            CashAdvanceDB.SaveChanges();

            var _LstRefundDetail = LstRefundDetail.Split('|');
            var _LstRefundDetailAmount = LstRefundDetailAmount.Split('|');
            var _LstWithholding_Tax = LstWithholding_Tax.Split('|');

            for (int i = 0; i < _LstRefundDetail.Count() - 1; i++)
            {
                RefundTran FooRefundTrans = new RefundTran();
                FooRefundTrans.RefundID = _RefundID;
                FooRefundTrans.RefundDetail = _LstRefundDetail[i].ToString();
                FooRefundTrans.Amount = _LstRefundDetailAmount[i].ToString().ToDecimal();
                FooRefundTrans.Withholding_Tax = _LstWithholding_Tax[i].ToString().ToDecimal();
                FooRefundTrans.Creator = data.Creator;
                FooRefundTrans.CreateDate = DateTime.Now;
                FooRefundTrans.IsDelete = false;
                CashAdvanceDB.RefundTrans.Add(FooRefundTrans);
            }
            CashAdvanceDB.SaveChanges();

            AdvancePayment avp = CashAdvanceDB.AdvancePayments.Where(s => s.AdvancePaymentID == data.AdvancePaymentID).SingleOrDefault();
            avp.Reviser = data.Reviser;
            avp.ReviserDate = DateTime.Now;
            avp.DocumentStatusID = 4;
            CashAdvanceDB.SaveChanges();
            //SendMail(avp);
            return _RefundID.GetValueOrDefault();
        }

        public void IsSendMail(long AdvancePaymentID)
        {
            AdvancePayment avp = CashAdvanceDB.AdvancePayments.Where(s => s.AdvancePaymentID == AdvancePaymentID).SingleOrDefault();
            if (avp != null)
            {
                SendMail(avp);
            }
        }

        public void FileUploadHandler()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    long _AdvancePaymentID = Request["AdvancePaymentID"].ToString().ToLong();
                    long _RefundID = Request["RefundID"].ToString().ToLong();

                    if (files.Count > 0)
                    {
                        AdvancePayment avp = CashAdvanceDB.AdvancePayments.Where(s => s.AdvancePaymentID == _AdvancePaymentID).SingleOrDefault();
                        RefundTable rft = CashAdvanceDB.RefundTables.Where(s => s.RefundID == _RefundID).SingleOrDefault();

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
                            string path = Server.MapPath("~/AttachFile/CashAdvance/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + avp.AdvancePaymentNO);

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
                            string newFilename = path + "\\" + avp.AdvancePaymentNO + extension;

                            System.IO.File.Move(fname, newFilename);

                            rft.AttachFile = newFilename;
                            CashAdvanceDB.SaveChanges();
                        }
                    }
                    else
                    {
                        //wbERMS.UpdateCheckUploadJD(0);
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

        public FileStreamResult GetFile(string filename)
        {
            string resultType = Path.GetExtension(filename);
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            return File(fs, ExtenMethod.GetMimeType(resultType));
        }

        //Save ข้อมูลการเคลียร์เงิน
        public void SaveJV(RefundTable data)
        {
            RefundTable rft = CashAdvanceDB.RefundTables.Where(s => s.RefundID == data.RefundID).SingleOrDefault();
            rft.JVNo = data.JVNo;
            rft.RefundDate = data.RefundDate;
            rft.Reviser = data.Reviser;
            rft.ReviserDate = DateTime.Now;

            AdvancePayment avp = CashAdvanceDB.AdvancePayments.Where(s => s.AdvancePaymentID == data.AdvancePaymentID).SingleOrDefault();
            avp.Reviser = data.Reviser;
            avp.ReviserDate = DateTime.Now;
            avp.DocumentStatusID = 5;//End Document
            CashAdvanceDB.SaveChanges();
            //SendMail(avp);
        }

        //Reject เอกสารให้ย้อนกลับไป DocumentStatusID = 3 ให้ผู้ขอเบิกคีรายละเอียดใหม่
        public void RejectDocument(AdvancePayment data)
        {
            AdvancePayment avp = CashAdvanceDB.AdvancePayments.Where(s => s.AdvancePaymentID == data.AdvancePaymentID).SingleOrDefault();
            avp.Reviser = data.Reviser;
            avp.ReviserDate = DateTime.Now;
            if (data.DocumentStatusID == 1) { avp.DocumentStatusID = 0; } else { avp.DocumentStatusID = 3; }
            //avp.DocumentStatusID = 3;
            CashAdvanceDB.SaveChanges();
            //SendMail(avp);
        }

        #endregion

        public string GenerateAdvancePaymentNO(long CompanyId)
        {
            string result = string.Empty;
            string CompanyCode = "";
            //CompanyTable cm = MASDB.CompanyTables.Where(s => s.CompanyId == CompanyId).SingleOrDefault();
            cCompany cm = cApi.apiGetCompanyByCompanyID(CompanyId);
            if (cm != null) { CompanyCode = cm.CompanyCode; }

            result += CompanyCode + "-";
            result += DateTime.Now.ToString("yyMM-");
            int Year = DateTime.Now.Year;
            int Month = DateTime.Now.Month;
            DateTime StartofMonth = new DateTime(Year, Month, 1, 0, 0, 0);
            DateTime EndofMonth = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month), 23, 59, 59);
            int CountRec = 0;

            string maxNo = CashAdvanceDB.AdvancePayments.Where(s => s.DocumentDate >= StartofMonth && s.DocumentDate <= EndofMonth && s.AdvancePaymentNO != "" && s.AdvancePaymentNO.Contains(result)).Max(s => s.AdvancePaymentNO);
            if (maxNo == "" || maxNo == null) { CountRec = 1; }
            else
            {
                CountRec = maxNo.Split('-')[2].ToString().ToInt32() + 1;
            }

            result += CountRec.ToString("000");

            return result;
        }

        public JsonResult getListDepartment(long ComID)
        {
            object[] list;
            var a = cApi.apiGetDepartmentList().ToList();
            list = new object[a.Count];

            for (int i = 0; i < a.Count; i++)
            {
                list[i] = new { value = a[i].DeptId, name = a[i].DeptDescription };
            }
            return Json(list);
        }

        public string SendMail(AdvancePayment avp)
        {
            //Employee emp = MASDB.Employees.Where(s => s.EmpId == avp.PaymentOwner).SingleOrDefault();
            cEmployeeDetail emp = cApi.apiGetEmployeeDetail(avp.PaymentOwner);
            string Subject = "กรุณาอนุมัติเอกสารเลขที่ " + avp.AdvancePaymentNO + " ของ " + emp.DisplayName;
            if (avp.DocumentStatusID == 5) { Subject = "อนุมัติจบเอกสารเลขที่ " + avp.AdvancePaymentNO + " ของ " + emp.DisplayName; }
            else if (avp.IsDelete == true) { Subject = "ยกเลิกเอกสาร " + avp.AdvancePaymentNO + " ของ " + emp.DisplayName; }
            else if (avp.DocumentStatusID == 0) { Subject = "ย้อนกลับเอกสาร " + avp.AdvancePaymentNO + " ของ " + emp.DisplayName + " เนื่องจากเอกสารผิดพลาด"; }

            long TaskID = email.SaveTask(99, "", 0, 0, avp.AdvancePaymentNO, 0, 0, avp.PaymentOwner, avp.DeptId.GetValueOrDefault(), avp.PaymentOwner.ToLong(), "", "S", DateTime.Now, 0, avp.PaymentOwner, false, DateTime.Now, false, "");
            long WorkflowEmailHeadID = email.SaveEmail(email.getWorkflowEmail(99, avp.AdvancePaymentNO, "", Subject, "เอกสารเลขที่ " + avp.AdvancePaymentNO + " (" + CashAdvanceDB.DocumentStatusTables.Where(s => s.DocumentStatusID == avp.DocumentStatusID).SingleOrDefault().DocumentStatusName + ")", avp.AdvancePaymentNO, avp.DocumentStatusID.ToString(), TaskID), GenerateWorkflowEmailTable(0, avp));

            List<string> Add = new List<string>();
            Add.Add("Pisarn.s@univentures.co.th");
            //Add.Add("ekapong.w@univentures.co.th");

            #region add mail paymentowner creator financeList for send mail---------------------------------------------------
            Add.Add(getEmail(avp.PaymentOwner));
            if (avp.PaymentOwner != avp.Creator.ToString()) { Add.Add(getEmail(avp.Creator.ToString())); }
            var LstFinance = CashAdvanceDB.Config_Employee.Where(s => s.IsDelete != true).ToList();
            foreach (var items in LstFinance)
            {
                Add.Add(getEmail(items.EmployeeID));
            }
            #endregion

            email.SendEmail(Subject, Add, Add, Add, email.getHTML(TaskID));
            return "True";
        }

        private string getEmail(string EmployeeID)
        {
            string result = string.Empty;
            //result = MASDB.Employees.Where(s => s.EmpId == EmployeeID).SingleOrDefault().Email;
            return result;
        }

        public List<WorkflowEmailTable> GenerateWorkflowEmailTable(long HeadMailID, AdvancePayment avp)
        {
            //string EmpID = avp.PaymentOwner.ToString();
            //Employee emp = MASDB.Employees.Where(s => s.EmpId == EmpID).SingleOrDefault();
            List<WorkflowEmailTable> em = new List<WorkflowEmailTable>();
            //em.Add(email.getEmailLine(HeadMailID, 1, "เอกสารเลขที่ : ", avp.AdvancePaymentNO, "", 0));
            //em.Add(email.getEmailLine(HeadMailID, 2, "ชื่อผู้เบิก : ", emp.DisplayName, "", 0));

            //var qDept = MASDB.DepartmentsTables.Where(s => s.DeptId == avp.DeptofCost).ToList();
            //var _costcenter = "";
            //if (qDept.Count > 0) { _costcenter = qDept.Take(1).SingleOrDefault().CostCenter; }
            //em.Add(email.getEmailLine(HeadMailID, 3, "Cost Center : ", _costcenter, "", 0));
            ////em.Add(email.getEmailLine(HeadMailID, 3, "Cost Center : ", MASDB.DepartmentsTables.Where(s => s.DeptId == avp.DeptofCost).SingleOrDefault().CostCenter, "", 0));

            //var qProject = MASDB.Projects.Where(s => s.ProjectID == avp.ProjectID).ToList();
            //var _project = "";
            //if (qProject.Count > 0) { _project = qProject.Take(1).SingleOrDefault().ProjectName; }
            //em.Add(email.getEmailLine(HeadMailID, 4, "โครงการ : ", _project, "", 0));
            ////em.Add(email.getEmailLine(HeadMailID, 4, "โครงการ : ", MASDB.Projects.Where(s => s.ProjectID == avp.ProjectID).SingleOrDefault().ProjectName, "", 0));


            //var qBudgetAccount = MASDB.BudgetAccountTables.Where(s => s.BudgetAccountID == avp.BudgetAccountID).ToList();
            //var _BudgetAccount = "";
            //if (qBudgetAccount.Count > 0) { _BudgetAccount = qBudgetAccount.Take(1).SingleOrDefault().BudgetAccountNameTH; }
            //em.Add(email.getEmailLine(HeadMailID, 5, "งบประมาณ : ", _BudgetAccount, "", 0));
            ////em.Add(email.getEmailLine(HeadMailID, 5, "งบประมาณ : ", MASDB.BudgetAccountTables.Where(s => s.BudgetAccountID == avp.BudgetAccountID).SingleOrDefault().BudgetAccountNameTH, "", 0));

            //var qBudgetModel = MASDB.BudgetModelTables.Where(s => s.BudgetModelID == avp.BudgetModelID).ToList();
            //var _BudgetModel = "";
            //if (qBudgetModel.Count > 0) { _BudgetModel = qBudgetModel.Take(1).SingleOrDefault().BudgetModelNameTH; }
            //em.Add(email.getEmailLine(HeadMailID, 6, "รหัสงบประมาณ : ", _BudgetModel, "", 0));
            ////em.Add(email.getEmailLine(HeadMailID, 6, "รหัสงบประมาณ : ", MASDB.BudgetModelTables.Where(s => s.BudgetModelID == avp.BudgetModelID).SingleOrDefault().BudgetModelNameTH, "", 0));


            //em.Add(email.getEmailLine(HeadMailID, 7, "วันที่ต้องการรับเงิน : ", avp.ReceiveCashDate.GetValueOrDefault().ToString("dd/MM/yyyy"), "", 0));
            //em.Add(email.getEmailLine(HeadMailID, 8, "ยอดขอเบิก : ", avp.Amount.ToString("#,##0.00"), "บาท", 0));
            //em.Add(email.getEmailLine(HeadMailID, 9, "เพื่อ : ", avp.PaymentRemark, "", 0));


            //var qPaymentType = CashAdvanceDB.PaymentTypeTables.Where(s => s.PaymentTypeID == avp.PaymentTypeID).ToList();
            //var _PaymentType = "";
            //if (qPaymentType.Count > 0) { _PaymentType = qPaymentType.Take(1).SingleOrDefault().PaymentNameTH; }
            //em.Add(email.getEmailLine(HeadMailID, 10, "ประเภทการรับเงิน : ", _PaymentType, "", 0));

            //if (avp.DocumentStatusID > 1)
            //{
            //    em.Add(email.getEmailLine(HeadMailID, 11, "-", "-", "-", 0));
            //    em.Add(email.getEmailLine(HeadMailID, 12, "PV No. : ", avp.PVNo, "", 0));
            //    em.Add(email.getEmailLine(HeadMailID, 13, "วันที่รับเช็ค : ", avp.ChequeDate.GetValueOrDefault().ToString("dd/MM/yyyy"), "", 0));
            //}

            //RefundTable rt = CashAdvanceDB.RefundTables.Where(s => s.AdvancePaymentID == avp.AdvancePaymentID).SingleOrDefault();
            //if (rt == null) { return em; }
            //var q = CashAdvanceDB.RefundTrans.Where(s => s.RefundID == rt.RefundID);

            //if (avp.DocumentStatusID > 3)
            //{
            //    em.Add(email.getEmailLine(HeadMailID, 14, "-", "-", "-", 0));
            //    em.Add(email.getEmailLine(HeadMailID, 15, "วันที่เคลียร์ : ", q.Take(1).SingleOrDefault().CreateDate.GetValueOrDefault().ToString("dd/MM/yyyy"), "", 0));
            //    em.Add(email.getEmailLine(HeadMailID, 16, "ยอดรวมค่าใช้จ่าย : ", q.Sum(s => s.Amount).ToString("#,##0.00"), "บาท", 0));
            //}
            //if (avp.DocumentStatusID > 4)
            //{
            //    em.Add(email.getEmailLine(HeadMailID, 17, "-", "-", "-", 0));
            //    em.Add(email.getEmailLine(HeadMailID, 18, "JV No. : ", rt.JVNo, "", 0));
            //    em.Add(email.getEmailLine(HeadMailID, 19, "วันที่อนุมัติ : ", rt.RefundDate.GetValueOrDefault().ToString("dd/MM/yyyy"), "", 0));
            //}

            return em;
        }

        public string ClearDate(DateTime IsDate)
        {
            string result = string.Empty;
            result = IsDate.AddDays(7).ToObj2Json();
            return result;
        }

        public void CancelDocument(AdvancePayment data)
        {
            AdvancePayment avp = CashAdvanceDB.AdvancePayments.Where(s => s.AdvancePaymentID == data.AdvancePaymentID).SingleOrDefault();
            if (avp != null)
            {
                avp.Reviser = data.Reviser;
                avp.ReviserDate = DateTime.Now;
                avp.IsDelete = true;
                CashAdvanceDB.SaveChanges();
                //SendMail(avp);
            }
        }

        public bool chkAuthenAdvance(string PaymentOwner, string ParentDeptID, string ParentDeptCode)
        {
            bool result = false;
            //string _DeptID = (from empPosition in MASDB.EmployeePositionTables where empPosition.EmpID == PaymentOwner join poTrns in MASDB.PositionTrans on empPosition.PositionTransID equals poTrns.PositionTransID join orgdept in MASDB.OrganizationDepts on poTrns.OrganizationDeptsID equals orgdept.OrganizationDeptsID join dept in MASDB.DepartmentsTables on orgdept.DeptId equals dept.DeptId select new { _DeptID = orgdept.Parent_DeptId }).SingleOrDefault()._DeptID.ToString().ToLong().ToString();
            //string _DeptID = getMainDept(PaymentOwner);
            string _DeptID = ParentDeptID;

            //ConfigCAD _config = CashAdvanceDB.ConfigCADs.Where(s => s.DeptID == _DeptID).SingleOrDefault();
            ConfigCAD _config = CashAdvanceDB.ConfigCADs.Where(s => s.DeptCode == ParentDeptCode && s.isDelete == false).SingleOrDefault();
            var _advance = CashAdvanceDB.AdvancePayments.Where(s => s.IsDelete != true && s.DocumentStatusID != 5 && s.DocumentStatusID != 0 && s.PaymentOwner == PaymentOwner).ToList();

            if (_config != null)
            {
                if (_config.IsGA == true)//GA Advance unlimit
                {
                    result = true;
                }
                else
                {
                    if (_advance.Count() <= 0)
                    {
                        result = true;
                    }
                }
            }
            else
            {
                if (_advance.Count() <= 0)
                {
                    result = true;
                }
            }

            return result;
        }

        //public bool chkAuthenFinance(string DeptID)
        public bool chkAuthenFinance(string ParentDeptID)
        {
            bool result = false;
            //long fooDeptID = DeptID.ToLong();
            //string _DeptID = string.Empty;
            //try
            //{
            //    _DeptID = MASDB.OrganizationDepts.Where(s => s.DeptId == fooDeptID).SingleOrDefault().Parent_DeptId.ToString().ToLong().ToString();
            //}
            //catch { _DeptID = "0"; }
            ConfigCAD _config = CashAdvanceDB.ConfigCADs.Where(s => s.DeptID == ParentDeptID).SingleOrDefault();

            if (_config != null)
            {
                if (_config.IsFN == true)//FN Authen Approve
                {
                    result = true;
                }
            }

            return result;
        }

        public string getDepartment(long CompanyID = 0)
        {
            string result = string.Empty;
            //var lstDepartment = (from dept in MASDB.DepartmentsTables where (dept.DeptTypeId == "4" || dept.DeptTypeId == "2") select new { id = dept.DeptId, text = dept.DeptDescription }).ToList();
            //long aa = 0;
            //lstDepartment.Add(new { id = aa, text = "Please Select" });
            //result = lstDepartment.ToObj2Json();
            return result;
        }

        public string getEmployee(long CompanyID = 0)
        {
            string result = string.Empty;
            //var lstEmployee = (from emp in MASDB.Employees.Where(s => s.IsActive == true).OrderBy(s => s.DisplayName) join emp_po in MASDB.EmployeePositionTables on emp.EmpId equals emp_po.EmpID join potrns in MASDB.PositionTrans on emp_po.PositionTransID equals potrns.PositionTransID join orgdept in MASDB.OrganizationDepts on potrns.OrganizationDeptsID equals orgdept.OrganizationDeptsID join com in MASDB.CompanyTables on orgdept.CompanyID equals com.CompanyId select new { id = emp.EmpId, text = emp.EmpId + " : " + emp.DisplayName }).ToList();
            ////var lstEmployee = (from emp in MASDB.Employees.Where(s => s.IsActive == true).OrderBy(s => s.DisplayName) join emp_po in MASDB.EmployeePositionTables on emp.EmpId equals emp_po.EmpID join potrns in MASDB.PositionTrans on emp_po.PositionTransID equals potrns.PositionTransID join orgdept in MASDB.OrganizationDepts on potrns.OrganizationDeptsID equals orgdept.OrganizationDeptsID join com in MASDB.CompanyTables.Where(s => s.CompanyId == CompanyID) on orgdept.CompanyID equals com.CompanyId select new { id = emp.EmpId, text = emp.EmpId + " : " + emp.DisplayName }).ToList();
            //lstEmployee.Add(new { id = "0", text = "Please Select" });
            //result = lstEmployee.ToObj2Json();
            return result;
        }

        public string getCostCenter(long CompanyID = 0)
        {
            string result = string.Empty;
            ////var lstDepartment = (from dept in MASDB.DepartmentsTables.OrderBy(s=>s.CostCenter) where dept.CompanyId == CompanyID select new { id = dept.DeptId, text = dept.CostCenter + " : " + dept.DeptDescription }).ToList();
            //var lstDepartment = (from CostCenterT in cEmp.getCostCenter(CompanyID) select new { id = CostCenterT.DeptCode, text = CostCenterT.CostCenterCode }).OrderBy(s => s.text).ToList();
            //string aa = "0";
            //lstDepartment.Add(new { id = aa, text = "Please Select" });
            //result = lstDepartment.ToObj2Json();

            return result;
        }

        public string getDetailEmployee(string EmployeeID = "")
        {
            string result = string.Empty;

            if (EmployeeID == "0" || EmployeeID == "") { result = "0|0"; }
            else
            {
                ////cEmployee cEmp = new cEmployee();
                //var OrganizeEmployee = MASDB.Database.SqlQuery<cOrganizeEmployee>("Select * from vw_OrganizeEmployee where EmpID = '" + EmployeeID + "'").FirstOrDefault();
                //OrganizeEmployee.MainDeptID = cEmp.getMainDept(OrganizeEmployee);
                //OrganizeEmployee.MainDeptCode = cEmp.getMainDeptCode(OrganizeEmployee);

                //result = OrganizeEmployee.MainDeptCode.ToString() + "|" + OrganizeEmployee.PositionID.ToString() + "|" + OrganizeEmployee.CompanyId;
            }

            return result;
        }

        public string getPostion(long DepartmentID = 0)
        {
            string result = string.Empty;

            //var lstDept = MASDB.OrganizationDepts.Where(s => s.Parent_DeptId == DepartmentID).Select(s => s.DeptId).ToList();
            //var lstPosition = (from orgdept in MASDB.OrganizationDepts.Where(s => lstDept.Contains(s.DeptId)) join potrns in MASDB.PositionTrans on orgdept.OrganizationDeptsID equals potrns.OrganizationDeptsID join po in MASDB.PositionTables on potrns.PositionID equals po.PositionID select new { id = po.PositionID, text = po.PositionName }).ToList();


            ////var lstDepartment = (from dept in MASDB.DepartmentsTables where dept.DeptTypeId == "4" && dept.CompanyId == CompanyID select new { id = dept.DeptId, text = dept.DeptDescription }).ToList();
            //long aa = 0;
            //lstPosition.Add(new { id = aa, text = "Please Select" });
            //result = lstPosition.ToObj2Json();
            return result;
        }

        public ActionResult PrintReport(long AdvancePaymentID = 0)
        {
            DataTable dt = getDataForPrint(AdvancePaymentID);

            string CompanyName = string.Empty;
            var _avp = CashAdvanceDB.AdvancePayments.Where(s => s.AdvancePaymentID == AdvancePaymentID).SingleOrDefault();
            if (_avp != null)
            {
                long comid = _avp.CompanyId.GetValueOrDefault();
                //CompanyName = MASDB.CompanyTables.Where(s => s.CompanyId == comid).SingleOrDefault().CompanyName;
            }

            ReportClass rptMemo = new ReportClass();
            rptMemo.FileName = Server.MapPath("~/Report/CashAdvance/rpt_withdraw.rpt");
            rptMemo.Load();
            rptMemo.SetDataSource(dt);
            rptMemo.SetParameterValue("Company", CompanyName);

            Stream st = rptMemo.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(st, "application/pdf");
        }

        public DataTable getDataForPrint(long AdvancePaymentID = 0)
        {
            DataTable dt = new DataTable();
            //var connection = new SqlConnection("Server=10.40.3.11\\sql2008r2;DataBase=CAD; User Id=sa;Password=p@ssw0rd");
            var connection = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["CADDBSqlConnection"]);
            var command = new SqlCommand("Select * from vw_ReportAdvancePayment where AdvancePaymentID = " + AdvancePaymentID, connection);
            connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            dt.Load(dr);
            dr.Close();
            return dt;
        }

    }
}