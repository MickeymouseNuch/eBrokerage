using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using Inspinia_MVC5.Models.DMPS;
using Inspinia_MVC5;
using Inspinia_MVC5.API;

namespace UVG_Main.Controllers.CN_Deposit
{
    public class DMPS_CRMLeadController : Controller
    {
        PMdbEntities1 DMPS = new PMdbEntities1();
        cApiPortal cApi = new cApiPortal();

        // GET: DMPS_CRMLead
        public ActionResult Index()
        {
            var lstPerfix = (from t1 in DMPS.CRM_Master_NamePrefix.Where(s => s.IsDelete != false) select new { ID = t1.ID, DisplayName = t1.FullText}).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlPerfix = new SelectList(lstPerfix, "ID", "DisplayName");

            var lstProject = (from t1 in DMPS.ProjectTables.Where(s => s.IsDelete != false) select new { ID = t1.ProjectID, DisplayName = t1.ProjectCode + ':' + t1.ProjectName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlProject = new SelectList(lstProject, "ID", "DisplayName");

            var lstUnits = (from t1 in DMPS.UnitsTables.Where(s => s.IsDelete != false) select new { ID = t1.UnitsID, DisplayName = t1.UnitsCode + ':'+ t1.UnitsName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlUnits = new SelectList(lstUnits, "ID", "DisplayName");

            var lstModel = (from t1 in DMPS.ModelTables.Where(s => s.IsDelete != false) select new { ID = t1.ModelID, DisplayName = t1.ModelCode + ':' + t1.ModelName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlModel = new SelectList(lstModel, "ID", "DisplayName");

            return View();
        }

        public ActionResult LoadDataListView(long TransID)
        {

            var qtrans = DMPS.vw_CRM_LeadData.Where(s => s.IsDelete != true);

            if (TransID != 0)
            {
                qtrans = qtrans.Where(s => s.TransID == TransID);
            }

            ViewBag.lst_CRMLead = qtrans.OrderBy(S => S.TransID).ToList();

            return PartialView();
                       
        }

        public ActionResult LoadModal()
        {
            return PartialView();
        }

        public string LoadData2Modal(long TransID)
        {
            var data = DMPS.vw_CRM_LeadData.Where(s => s.TransID == TransID).SingleOrDefault();
            if (data == null)
            {
                data = new vw_CRM_LeadData();
                data.TransID = 0;
                //data.DocumentStatusTH = "";
                //data.DocumentStatusEN = "";
                //data.IsDelete = false;
            }
            else
            {

            }

            var result = data.ToObj2Json();
            return result;
        }

        public long SaveCRM_Lead(CRM_Lead data) {
            long result = 0;
            var _CRM_LeadTB = DMPS.CRM_Lead.SingleOrDefault(s => s.TransID == data.TransID);

            try
            {
                if (_CRM_LeadTB == null)
                {
                    _CRM_LeadTB = new CRM_Lead();
                    _CRM_LeadTB.Prefix = data.Prefix;
                    _CRM_LeadTB.FirstName = data.FirstName;
                    _CRM_LeadTB.LastName = data.LastName;
                    _CRM_LeadTB.ProjectID = data.ProjectID;
                    _CRM_LeadTB.Tel1 = data.Tel1;
                    _CRM_LeadTB.Email = data.Email;
                    _CRM_LeadTB.Fax = data.Fax;
                    _CRM_LeadTB.Gender = data.Gender;
                    _CRM_LeadTB.Datasource = data.Datasource;
                    _CRM_LeadTB.JobTitle = data.JobTitle;
                    _CRM_LeadTB.JobLocation = data.JobLocation;
                    _CRM_LeadTB.AddressNo = data.AddressNo;
                    _CRM_LeadTB.Moo = data.Moo;
                    _CRM_LeadTB.Soi = data.Soi;
                    _CRM_LeadTB.Village = data.Village;
                    _CRM_LeadTB.Road = data.Road;
                    _CRM_LeadTB.SubDistrict = data.SubDistrict;
                    _CRM_LeadTB.District = data.District;
                    _CRM_LeadTB.Province = data.Province;
                    _CRM_LeadTB.ZipCode = data.ZipCode;
                    _CRM_LeadTB.Country = data.Country;
                    _CRM_LeadTB.Remark = data.Remark;
                    _CRM_LeadTB.VisitID = data.VisitID;
                    _CRM_LeadTB.UnitsID = data.UnitsID;
                    _CRM_LeadTB.Village = data.Village;

                    DMPS.CRM_Lead.Add(_CRM_LeadTB);
                    DMPS.SaveChanges();
                }
                else
                {
                    _CRM_LeadTB.Prefix = data.Prefix;
                    _CRM_LeadTB.FirstName = data.FirstName;
                    _CRM_LeadTB.LastName = data.LastName;
                    _CRM_LeadTB.ProjectID = data.ProjectID;
                    _CRM_LeadTB.Tel1 = data.Tel1;
                    _CRM_LeadTB.Email = data.Email;
                    _CRM_LeadTB.Fax = data.Fax;
                    _CRM_LeadTB.Gender = data.Gender;
                    _CRM_LeadTB.Datasource = data.Datasource;
                    _CRM_LeadTB.JobTitle = data.JobTitle;
                    _CRM_LeadTB.JobLocation = data.JobLocation;
                    _CRM_LeadTB.AddressNo = data.AddressNo;
                    _CRM_LeadTB.Moo = data.Moo;
                    _CRM_LeadTB.Soi = data.Soi;
                    _CRM_LeadTB.Village = data.Village;
                    _CRM_LeadTB.Road = data.Road;
                    _CRM_LeadTB.SubDistrict = data.SubDistrict;
                    _CRM_LeadTB.District = data.District;
                    _CRM_LeadTB.Province = data.Province;
                    _CRM_LeadTB.ZipCode = data.ZipCode;
                    _CRM_LeadTB.Country = data.Country;
                    _CRM_LeadTB.Remark = data.Remark;
                    _CRM_LeadTB.VisitID = data.VisitID;
                    _CRM_LeadTB.UnitsID = data.UnitsID;
                    _CRM_LeadTB.Village = data.Village;

                    DMPS.Entry(_CRM_LeadTB).State = EntityState.Modified;
                    DMPS.SaveChanges();
                }
                result = _CRM_LeadTB.TransID;
            }
            catch (Exception ex)
            {
                var errorEX = ex.ToString();
                result = 0;
            }
            //result = true;
            return result;
        }

    }
}