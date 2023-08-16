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
    public class DMPS_CallVisitController : Controller
    {
        PMdbEntities1 DMPS = new PMdbEntities1();
        MASDBEntities MASDB = new MASDBEntities();
        cApiPortal cApi = new cApiPortal();

        public ActionResult Index()
        {
            var lstCountry = (from t1 in DMPS.MasCountries select new { CountryCode = t1.CountryCode, DisplayName = t1.CountryNameTH }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlRealAddress2Country = new SelectList(lstCountry, "CountryCode", "DisplayName");
            ViewBag.ddlWorkAdressCountry = new SelectList(lstCountry, "CountryCode", "DisplayName");

            /*var lstProjectName = (from t1 in DMPS.ProjectTables select new { ProjectID = t1.ProjectID, DisplayName = t1.ProjectName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlProjectName = new SelectList(lstProjectName, "ProjectID", "DisplayName");*/
            ViewBag.ddlProjectName = new SelectList(DMPS.ProjectTables.Where(s => s.IsDelete == false).OrderBy(s => s.ProjectName).ToList(), "ProjectID", "ProjectName");

            var lstProvice = (from t1 in MASDB.ProvinceTables.Where(s => s.PROVINCE_ID != 0) select new { ProviceID = t1.PROVINCE_ID, DisplayName = t1.PROVINCE_NAME }).OrderBy(s => s.ProviceID).ToList();
            ViewBag.ddlRealAdressProvince = new SelectList(lstProvice, "ProviceID", "DisplayName");
            ViewBag.ddlWorkAdressProvince = new SelectList(lstProvice, "ProviceID", "DisplayName");

            //var lstDistinct = (from t1 in MASDB.AmphurTables.Where(s => s.PROVINCE_ID == _ProvinceID) select new { DistinctID = t1.AMPHUR_ID, DisplayName = t1.AMPHUR_NAME }).OrderBy(s => s.DistinctID).ToList();
            //ViewBag.ddlRealAddressDistrict = new SelectList(lstDistinct, "DistinctID", "DisplayName");
            //ViewBag.ddlWorkAdressDistrict = new SelectList(lstDistinct, "DistinctID", "DisplayName");

            //var lstSubDistinct = (from t1 in MASDB.DistrictTables.Where(s => s.PROVINCE_ID == _ProvinceID && s.AMPHUR_ID == _DistinctID) select new { SubDistinctID = t1.DISTRICT_ID, DisplayName = t1.DISTRICT_NAME }).OrderBy(s => s.SubDistinctID).ToList();
            //ViewBag.ddlRealSubDistrict = new SelectList(lstSubDistinct, "SubDistinctID", "DisplayName");
            //ViewBag.ddlWorkAdressSubDistrict = new SelectList(lstSubDistinct, "SubDistinctID", "DisplayName");

            var lstMediaName = (from t1 in DMPS.MasMedias select new { MediaID = t1.MediaID, DisplayName = t1.MediaName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlMediaName = new SelectList(lstMediaName, "MediaID", "DisplayName");

            var lstMaritalStatus = (from t1 in DMPS.MasMaritalStatus select new { MaritalStatusID = t1.MaritalStatusID, DisplayName = t1.MaritalStatusName }).OrderBy(s => s.MaritalStatusID).ToList();
            ViewBag.ddlMaritalStatus = new SelectList(lstMaritalStatus, "MaritalStatusID", "DisplayName");

            var lstAges = (from t1 in DMPS.MasAges select new { AgeID = t1.AgeID, DisplayName = t1.AgeRange }).OrderBy(s => s.AgeID).ToList();
            ViewBag.ddlAges = new SelectList(lstAges, "AgeID", "DisplayName");

            var lstBudgets = (from t1 in DMPS.MasBudgets select new { BudgetID = t1.BudgetID, DisplayName = t1.BudgetRange }).OrderBy(s => s.BudgetID).ToList();
            ViewBag.ddlBudgets = new SelectList(lstBudgets, "BudgetID", "DisplayName");

            var lstOccupations = (from t1 in DMPS.MasOccupations select new { OccupationID = t1.OccupationID, DisplayName = t1.OccupationName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlOccupations = new SelectList(lstOccupations, "OccupationID", "DisplayName");

            var lstPurposes = (from t1 in DMPS.MasPurposes select new { PurposeID = t1.PurposeID, DisplayName = t1.PurposeName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlPurposes = new SelectList(lstPurposes, "PurposeID", "DisplayName");

            var lstSalaries = (from t1 in DMPS.MasSalaries select new { SalaryID = t1.SalaryID, DisplayName = t1.SalaryRange }).OrderBy(s => s.SalaryID).ToList();
            ViewBag.ddlSalaries = new SelectList(lstSalaries, "SalaryID", "DisplayName");

            var lstReasonName = (from t1 in DMPS.MasReasons select new { ReasonID = t1.ReasonID, DisplayName = t1.ReasonName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlReasonName = new SelectList(lstReasonName, "ReasonID", "DisplayName");

            var lstActivity = (from t1 in DMPS.MasAcitivities select new { AcitivityDetailsID = t1.AcitivityID, DisplayName = t1.AcitivityName}).OrderBy(s => s.AcitivityDetailsID).ToList();
            ViewBag.ddlActivity = new SelectList(lstActivity, "AcitivityDetailsID", "DisplayName");
            
            var lstTopic = (from t1 in DMPS.MasTopics select new { TopicID = t1.TopicID, DisplayName = t1.TopicName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlTopic = new SelectList(lstTopic, "TopicID", "DisplayName");

            //ViewBag.ddlDeveloper = new SelectList(DMPS.DevelopmentTables.OrderBy(s => s.DevelopmentName).ToList(), "DevelopmentID", "DevelopmentName");
            ViewBag.ddlDeveloper = new SelectList(DMPS.DevelopmentTables.Where(s => s.IsDelete == false).OrderBy(s => s.DevelopmentName).ToList(), "DevelopmentID", "DevelopmentName");
            //ViewBag.ddlProject = new SelectList(DMPS.ProjectTables.Where(s => s.IsDelete == false).OrderBy(s => s.ProjectName).ToList(), "ProjectID", "ProjectName");

            var lstSaleName = (from t1 in DMPS.MasSales select new { SaleCode = t1.SaleCode, DisplayName = t1.SaleName }).OrderBy(s => s.DisplayName).ToList();
            ViewBag.ddlSaleName = new SelectList(lstSaleName, "SaleCode", "DisplayName");

            return View();
        }

        public string getDeveloper(int ThisID)
        {

            string result = string.Empty;
            //var lstData = (from data in MASDB.AmphurTables where (data.AMPHUR_ID != 0) select new { id = data.AMPHUR_ID, text = data.AMPHUR_NAME }).ToList(); ;
            var lstData = (from data in DMPS.DevelopmentTables where (data.IsDelete == false) select new { id = data.DevelopmentID, text = data.DevelopmentName }).ToList(); ;
            if (ThisID != 0)
            {
                lstData = (from data in DMPS.DevelopmentTables select new { id = data.DevelopmentID, text = data.DevelopmentName }).OrderBy(data => data.text).ToList(); ;

            }

            long a = 0;
            lstData.Add(new { id = a, text = "Please Select" });
            result = lstData.ToObj2Json();
            return result;
        }
        public string SetProjectName(long _ddlDeveloper)
        {

            string result = string.Empty;
            var lstProject = (from dept in DMPS.ProjectTables where (dept.IsDelete != true) select new { id = dept.ProjectID, text = dept.ProjectName }).ToList(); ;
            if (_ddlDeveloper != 0)
            {
                lstProject = (from dept in DMPS.ProjectTables where (dept.DevelopmentID == _ddlDeveloper && dept.IsDelete != true) select new { id = dept.ProjectID, text = dept.ProjectName }).ToList(); ;

            }
            long aa = 0;
            lstProject.Add(new { id = aa, text = "Please Select" });
            result = lstProject.ToObj2Json();
            return result;
        }

        public string getDistinct(long ProviceID)
        {

            string result = string.Empty;
            var lstData = (from data in MASDB.AmphurTables where (data.PROVINCE_ID == ProviceID) select new { id = data.AMPHUR_ID, text = data.AMPHUR_NAME }).ToList(); ;

            lstData.Insert(0, new { id = 0, text = "Please Select" });
            result = lstData.ToObj2Json();
            return result;
        }

        public string getSubDistinct(long ProviceID, long DistinctID)
        {

            string result = string.Empty;
            var    lstData = (from data in MASDB.DistrictTables where (data.PROVINCE_ID == ProviceID && data.AMPHUR_ID == DistinctID) select new { id = data.DISTRICT_ID, text = data.DISTRICT_NAME }).ToList(); ;

            lstData.Insert(0, new { id = 0, text = "Please Select" });
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


        public string SetZipCodeWork(string _Province, string _Distinct, string _SubDistinct)
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

        public string AddQuestionnaireCustomer(long _CVID ,bool _IsForeign, string _CustomerSurName, string _CustomerName, string _CustomerTel,string _IDCard , string _CustomerEmail, string _CustomerLineID, string _AddressDetails,
             string _Country, string _Address, string _Building, string _Floor, string _Moo, string _Road, string _Soi, int _Province, int _District, int _SubDistrict, string _ZipCode,
             string _Company, string _CompanyPosition, string _CompanyAddressDetails, string _CompanyCountry, string _CompanyAddress, string _CompanyBuilding, string _CompanyFloor,
             string _CompanyMoo, string _CompanyRoad, string _CompanySoi, int _CompanyProvince, int _CompanyDistrict, int _CompanySubDistrict,string _CompanyZipCode,
             int _MaritalStatus, int _Ages, int _Salaries, int _Budgets,int _Occupations, string _OccupationsOther,string _EmpID, string _SaleName, string _MediaID, string _MediaOther,
             string _PurposesID, string _PurposesOther, string _ReasonID, string _ReasonOther, string _CompareProject)
        {
            var CV = 0;
            var CVID = DMPS.CallVisits.SingleOrDefault(s => s.CallVisitID == _CVID);
                
            try
            {

                if (_CVID != 0)
                {
                    
                    CVID.IsForeign = _IsForeign;
                    CVID.Name = _CustomerName;
                    CVID.SurName = _CustomerSurName;
                    CVID.Tel = _CustomerTel;
                    CVID.IDCard = _IDCard;
                    CVID.Email = _CustomerEmail;
                    CVID.LineID = _CustomerLineID;

                    if (_Address != "")
                    {
                        CVID.Address = _Address;

                    }
                    else
                    {
                        CVID.Address = _AddressDetails;
                    }

                    CVID.Building = _Building;
                    CVID.Floor = _Floor;
                    CVID.Moo = _Moo;
                    CVID.Road = _Road;
                    CVID.Soi = _Soi;

                    if (_Province != 0)
                    {
                        CVID.ProvinceID = _Province;
                        if (_District != 0)
                        {
                            CVID.DistrictID = _District;
                            if (_SubDistrict != 0)
                            {
                                CVID.SubDistrictID = _SubDistrict;
                            }
                        }
                    }
                    else
                    {
                        CVID.ProvinceID = null;
                        CVID.DistrictID = null;
                        CVID.SubDistrictID = null;
                    }



                    CVID.ZipCode = _ZipCode;

                    CVID.CountryCode = _Country;

                    CVID.CompanyName = _Company;
                    CVID.CompanyPosition = _CompanyPosition;

                    if (_CompanyAddress != "")
                    {
                        CVID.CompanyAddress = _CompanyAddress;
                    }
                    else
                    {
                        CVID.CompanyAddress = _CompanyAddressDetails;
                    }

                    CVID.CompanyBuilding = _CompanyBuilding;
                    CVID.CompanyFloor = _CompanyFloor;
                    CVID.CompanyMoo = _CompanyMoo;
                    CVID.CompanyRoad = _CompanyRoad;
                    CVID.CompanySoi = _CompanySoi;

                    if (_CompanyProvince != 0)
                    {
                        CVID.CompanyProvinceID = _CompanyProvince;
                        if (_CompanyDistrict != 0)
                        {
                            CVID.CompanyDistrictID = _CompanyDistrict;

                            if (_CompanySubDistrict != 0)
                            {
                                CVID.CompanySubDistrictID = _CompanySubDistrict;
                            }
                        }
                    }
                    else
                    {
                        CVID.CompanyProvinceID = null;
                        CVID.CompanyDistrictID = null;
                        CVID.CompanySubDistrictID = null;
                    }


                    CVID.CompanyZipCode = _CompanyZipCode;

                    CVID.CompanyCountryCode = _CompanyCountry;


                    if (_MaritalStatus != 0)
                    {
                        CVID.MaritalStatusID = _MaritalStatus;
                    }
                    if (_Ages != 0)
                    {
                        CVID.AgeID = _Ages;
                    }
                    if (_Salaries != 0)
                    {
                        CVID.SalaryID = _Salaries;
                    }
                    if (_Budgets != 0)
                    {
                        CVID.BudgetID = _Budgets;
                    }
                    if (_Occupations != 0)
                    {
                        CVID.OccupationID = _Occupations;
                        CVID.OtherOccupation = _OccupationsOther;
                    }

                    //CVID.Creator = _EmpID;
                    //CVID.CreateDateTime = DateTime.Now;
                    CVID.Reviser = _EmpID;
                    CVID.ReviseDateTime = DateTime.Now;
                    if (_SaleName != "")
                    {
                        CVID.SaleCode = int.Parse(_SaleName);
                    }
                    else
                    {
                        CVID.SaleCode = null;
                    }
                    CVID.IsDelete = false;
                    DMPS.Entry(CVID).State = EntityState.Modified;
                    DMPS.SaveChanges();
                    CV = CVID.CallVisitID;
                    AddCallVisitMedia(_EmpID, CV, _MediaID, _MediaOther, false);
                    AddCallVisitPurpose(_EmpID, CV, _PurposesID, _PurposesOther, false);
                    AddCallVisitReason(_EmpID, CV, _ReasonID, _ReasonOther, false);
                    AddCallVisitCompareProjrct(_EmpID, CV, _CompareProject, false);

                } else {

                    CallVisit cv = new CallVisit();
                    cv.IsForeign = _IsForeign;
                    cv.Name = _CustomerName;
                    cv.SurName = _CustomerSurName;
                    cv.Tel = _CustomerTel;
                    cv.IDCard = _IDCard;
                    cv.Email = _CustomerEmail;
                    cv.LineID = _CustomerLineID;

                    if (_Address != "")
                    {
                        cv.Address = _Address;

                    }
                    else
                    {
                        cv.Address = _AddressDetails;
                    }

                    cv.Building = _Building;
                    cv.Floor = _Floor;
                    cv.Moo = _Moo;
                    cv.Road = _Road;
                    cv.Soi = _Soi;

                    if (_Province != 0)
                    {
                        cv.ProvinceID = _Province;
                        if (_District != 0)
                        {
                            cv.DistrictID = _District;
                            if (_SubDistrict != 0)
                            {
                                cv.SubDistrictID = _SubDistrict;
                            }
                        }
                    }


                    cv.ZipCode = _ZipCode;

                    cv.CountryCode = _Country;

                    cv.CompanyName = _Company;
                    cv.CompanyPosition = _CompanyPosition;

                    if (_CompanyAddress != "")
                    {
                        cv.CompanyAddress = _CompanyAddress;
                    }
                    else
                    {
                        cv.CompanyAddress = _CompanyAddressDetails;
                    }

                    cv.CompanyBuilding = _CompanyBuilding;
                    cv.CompanyFloor = _CompanyFloor;
                    cv.CompanyMoo = _CompanyMoo;
                    cv.CompanyRoad = _CompanyRoad;
                    cv.CompanySoi = _CompanySoi;

                    if (_CompanyProvince != 0)
                    {
                        cv.CompanyProvinceID = _CompanyProvince;
                        if (_CompanyDistrict != 0)
                        {
                            cv.CompanyDistrictID = _CompanyDistrict;

                            if (_CompanySubDistrict != 0)
                            {
                                cv.CompanySubDistrictID = _CompanySubDistrict;
                            }
                        }
                    }
                    

                    cv.CompanyZipCode = _CompanyZipCode;

                    cv.CompanyCountryCode = _CompanyCountry;


                    if (_MaritalStatus != 0)
                    {
                        cv.MaritalStatusID = _MaritalStatus;
                    }
                    if (_Ages != 0)
                    {
                        cv.AgeID = _Ages;
                    }
                    if (_Salaries != 0)
                    {
                        cv.SalaryID = _Salaries;
                    }
                    if (_Budgets != 0)
                    {
                        cv.BudgetID = _Budgets;
                    }
                    if (_Occupations != 0)
                    {
                        cv.OccupationID = _Occupations;
                        cv.OtherOccupation = _OccupationsOther;
                    }

                    cv.Creator = _EmpID;
                    cv.CreateDateTime = DateTime.Now;
                    cv.Reviser = _EmpID;
                    cv.ReviseDateTime = DateTime.Now;
                    if(_SaleName != "")
                    {
                        cv.SaleCode = int.Parse(_SaleName);
                    }
                    else
                    {
                        cv.SaleCode = null;
                    }
                    cv.IsDelete = false;
                    DMPS.CallVisits.Add(cv);
                    DMPS.SaveChanges();
                    CV = cv.CallVisitID;                  
                    AddCallVisitMedia(_EmpID, CV, _MediaID, _MediaOther, true);
                    AddCallVisitPurpose(_EmpID, CV, _PurposesID, _PurposesOther, true);
                    AddCallVisitReason(_EmpID, CV, _ReasonID, _ReasonOther, true);
                    AddCallVisitCompareProjrct(_EmpID, CV, _CompareProject, true);
                    AddCallVisitCompareProjrct(_EmpID, CV, _CompareProject, true);

                }
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return CV.ToObj2Json();

        }

       
        public string AddCallVisitMedia(string _EmpID, int _CallVisitID, string _MediaID, string _MediaOther, bool IsNew)
        {
  
            try
            {
                if (IsNew == false)
                {
                    //delete
                    var CVM = DMPS.CallVisitMedias.Where(s => s.CallVisitID == _CallVisitID).ToList();

                    for (int i = 0; i < CVM.Count; i++)
                    {
                        DMPS.Entry(CVM[i]).State = EntityState.Deleted;
                    }
                    DMPS.SaveChanges();
                }

                var arrMediaID = _MediaID.Split(',');
                var arrMediaOther = _MediaOther.Split(',');
                if (_MediaID != "")
                {
                    for (var i = 0; i < arrMediaID.Length; i++)
                    {
                        CallVisitMedia cvm = new CallVisitMedia();
                        cvm.MediaID = int.Parse(arrMediaID[i]);
                        cvm.Other = arrMediaOther[i];
                        cvm.Reviser = long.Parse( _EmpID);
                        cvm.ReviseDateTime = DateTime.Now;
                        cvm.CallVisitID = _CallVisitID;
                        DMPS.CallVisitMedias.Add(cvm);

                    }
                    DMPS.SaveChanges();
                    
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "False";
            }

            return "True";

        }


        public string AddCallVisitPurpose(string _EmpID, int _CallVisitID, string _PurposesID, string _PurposesOther, bool IsNew)
        {

            try
            {
                if (IsNew == false)
                {
                    //delete
                    var CVP = DMPS.CallVisitPurposes.Where(s => s.CallVisitID == _CallVisitID).ToList();

                    for (int i = 0; i < CVP.Count; i++)
                    {
                        DMPS.Entry(CVP[i]).State = EntityState.Deleted;
                    }
                    DMPS.SaveChanges();
                }

                var arrPurposesID = _PurposesID.Split(',');
                var arrPurposesOther = _PurposesOther.Split(',');
                if (_PurposesID != "")
                {
                    for (var i = 0; i < arrPurposesID.Length; i++)
                    {
 
                        CallVisitPurpose cvp = new CallVisitPurpose();
                        cvp.CallVisitID = _CallVisitID;
                        cvp.PurposeID = int.Parse(arrPurposesID[i]);
                        cvp.Other = arrPurposesOther[i];
                        cvp.Reviser = long.Parse(_EmpID);
                        cvp.ReviseDateTime = DateTime.Now;
                        DMPS.CallVisitPurposes.Add(cvp);
                    }
                    DMPS.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "False";
            }

            return "True";


        }

        public string AddCallVisitReason(string _EmpID, int _CallVisitID, string _ReasonID, string _ReasonOther, bool IsNew)
        {

            try
            {
                if (IsNew == false)
                {
                    //delete
                    var CVR = DMPS.CallVisitReasons.Where(s => s.CallVisitID == _CallVisitID).ToList();

                    for (int i = 0; i < CVR.Count; i++)
                    {
                        DMPS.Entry(CVR[i]).State = EntityState.Deleted;
                    }
                    DMPS.SaveChanges();
                }

                var arrReasonID = _ReasonID.Split(',');
                var arrReasonOther = _ReasonOther.Split(',');
                if (_ReasonID != "")
                {
                    for (var i = 0; i < arrReasonID.Length; i++)
                    {

                        CallVisitReason cvr = new CallVisitReason();
                        cvr.CallVisitID = _CallVisitID;
                        cvr.ReasonID = int.Parse(arrReasonID[i]);
                        cvr.Other = arrReasonOther[i];
                        cvr.Reviser = long.Parse(_EmpID);
                        cvr.ReviseDateTime = DateTime.Now;

                        DMPS.CallVisitReasons.Add(cvr);
                    }
                    DMPS.SaveChanges();
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "False";
            }

            return "True";

        }

        public string AddCallVisitCompareProjrct(string _EmpID, int _CallVisitID, string _CompareProject, bool IsNew)
        {
            try
            {
                if (IsNew == false)
                {
                    //delete
                    var CVCP = DMPS.CallVisitCompareProjects.Where(s => s.CallVisitID == _CallVisitID).ToList();

                    for (int i = 0; i < CVCP.Count; i++)
                    {
                        DMPS.Entry(CVCP[i]).State = EntityState.Deleted;
                    }
                    DMPS.SaveChanges();
                }

                var splitCompareProject = _CompareProject.Split(',');
                if (_CompareProject != "")
                {
                    for (var i = 0; i < splitCompareProject.Length; i++)
                    {
                        if (splitCompareProject[i] != "")
                        {
                            CallVisitCompareProject cvcp = new CallVisitCompareProject();
                            cvcp.CallVisitID = _CallVisitID;
                            cvcp.CompareProjectName = splitCompareProject[i];
                            cvcp.Reviser = long.Parse(_EmpID);
                            cvcp.ReviseDateTime = DateTime.Now;

                            DMPS.CallVisitCompareProjects.Add(cvcp);
                        }
                                         
                    }
                    DMPS.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "False";
            }

            return "True";

        }

        public string AddCallVisitActivity(long _EmpID,int _CallVisitActivity, int _CallVisitID, int _ProjectID, int _DeveloperID, int _ActivityID, int _TopicID,string _TopicOther,DateTime _ActivityDate, string _ActivityDetails)
        {
            try
            {
                var CallVisitActivityID = DMPS.CallVisitActivities.SingleOrDefault(s => s.CallVisitActivityID == _CallVisitActivity);

                if (_CallVisitActivity != 0)
                {

                    CallVisitActivityID.DevelopmentID = _DeveloperID;
                    CallVisitActivityID.ProjectID = _ProjectID;
                    CallVisitActivityID.ActivityID = _ActivityID;
                    CallVisitActivityID.ActivityDate = _ActivityDate;
                    CallVisitActivityID.ActivityDetails = _ActivityDetails;
                    if (_TopicID != 0)
                    {
                        CallVisitActivityID.TopicID = _TopicID;
                        CallVisitActivityID.TopicOther = _TopicOther;
                    }

                    CallVisitActivityID.Creator = _EmpID;
                    CallVisitActivityID.CreateDateTime = DateTime.Now;
                    CallVisitActivityID.Reviser = _EmpID;
                    CallVisitActivityID.ReviseDateTime = DateTime.Now;
                    CallVisitActivityID.IsDelete = false;
                    DMPS.Entry(CallVisitActivityID).State = EntityState.Modified;
                    DMPS.SaveChanges();
                    
                }else{
                    CallVisitActivity cva = new CallVisitActivity();
                    cva.CallVisitID = _CallVisitID;
                    cva.DevelopmentID = _DeveloperID;
                    cva.ProjectID = _ProjectID;
                    cva.ActivityID = _ActivityID;
                    cva.ActivityDate = _ActivityDate;
                    cva.ActivityDetails = _ActivityDetails;
                    if (_TopicID != 0)
                    {
                        cva.TopicID = _TopicID;
                        cva.TopicOther = _TopicOther;
                    }

                    cva.Creator = _EmpID;
                    cva.CreateDateTime = DateTime.Now;
                    cva.Reviser = _EmpID;
                    cva.ReviseDateTime = DateTime.Now;
                    cva.IsDelete = false;
                    DMPS.CallVisitActivities.Add(cva);                                                                           
                    DMPS.SaveChanges();                    
                    
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "False";
            }

            
            return "True";

        }

        public string LoadDataActivityDetails(long _CallVisitID)
        {
           
            var ActivityDetails = DMPS.vw_CallVisitActivity.Where(s => s.CallVisitID == _CallVisitID && s.IsDelete == false).OrderByDescending(s => s.ReviseDateTime).ToList();
            return ActivityDetails.ToObj2Json();
        }

        public ActionResult DeleteActivityDetails(string _CallVisitActivityID)
        {
            //var splitCallVisitActivityID = _CallVisitActivityID.Split(',');
            var ids = _CallVisitActivityID.Split(',').Select(id => Convert.ToInt32(id)).ToArray();

            //var cards = _context.Cards.Where(c => ids.Contains(c.ID));

            try
            {
                var CallVisitActivityIDRec = DMPS.CallVisitActivities.Where(s => ids.Contains(s.CallVisitActivityID)).ToList();

                for (int i = 0; i < CallVisitActivityIDRec.Count; i++)
                {

                    CallVisitActivityIDRec[i].IsDelete = true;
                    DMPS.Entry(CallVisitActivityIDRec[i]).State = EntityState.Modified;
                    
                }
                
                DMPS.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json("False");

            }

            return Json("True");

        }

        public string DisplayInfo(long CallVisitID)
        {

            var _CallVisitID = DMPS.CallVisits.Where(s => s.CallVisitID == CallVisitID && s.IsDelete == false).OrderByDescending(s => s.ReviseDateTime).ToList();


            _CallVisitID[0].Creator = cApi.apiGetEmployeeDetailList().Where(s => s.EmpID == _CallVisitID[0].Creator).SingleOrDefault().DisplayName;


            return _CallVisitID[0].ToObj2Json();
        }
        
        public string DisplayActivityDetail(long CallVisitActivityID)
        {

            var _CallVisitActivityID = DMPS.CallVisitActivities.Where(s => s.CallVisitActivityID == CallVisitActivityID && s.IsDelete == false).OrderByDescending(s => s.ReviseDateTime).ToList();


            return _CallVisitActivityID[0].ToObj2Json();
        }

        public string DisplayCallVisitCompareProject(long CallVisitID)
        {
            var _CompareProjectID = "";
            var _CallVisitCompareProjectID = DMPS.CallVisitCompareProjects.Where(s => s.CallVisitID == CallVisitID).ToList();
            for (int i = 0; i < _CallVisitCompareProjectID.Count; i++)
            {
                _CompareProjectID += _CallVisitCompareProjectID[i].CompareProjectName + ",";

            }
            return _CompareProjectID.ToObj2Json();
            //return _CallVisitCompareProjectID.ToObj2Json();
        }
        public string DisplayCallVisitMedia(long CallVisitID)
        {
            var _CallVisitMediaID = DMPS.CallVisitMedias.Where(s => s.CallVisitID == CallVisitID).OrderByDescending(s => s.ReviseDateTime).ToList();
            return _CallVisitMediaID.ToObj2Json();
        }
        public string DisplayCallVisitPurpose(long CallVisitID)
        {
            var _CallVisitPurposeID = DMPS.CallVisitPurposes.Where(s => s.CallVisitID == CallVisitID).OrderByDescending(s => s.ReviseDateTime).ToList();
            return _CallVisitPurposeID.ToObj2Json();
        }
        public string DisplayCallVisitReason(long CallVisitID)
        {
            var _CallVisitReasonID = DMPS.CallVisitReasons.Where(s => s.CallVisitID == CallVisitID).OrderByDescending(s => s.ReviseDateTime).ToList();
            return _CallVisitReasonID.ToObj2Json();
        } 
        
        public string DisplayCallVisitPDPA(long CallVisitID)
        {
            var _CallVisitPDPAID = DMPS.CallVisitPDPAs.Where(s => s.CallVisitID == CallVisitID).OrderByDescending(s => s.ReviseDateTime).ToList();
            return _CallVisitPDPAID.ToObj2Json();
        }

        public ActionResult LoadDataListView(string IsshowTab, DateTime? SDate = null, DateTime? EDate = null, string NameSeach = "", string SurNameSeach = "", string TelSeach = "" , string IDCardSeach = "")
        {
            ViewBag.IsshowTab = IsshowTab;
            if (IsshowTab == "Lead")
            {
                var qLead = DMPS.vw_LeadCallVisit.Where(s => s.IsDelete == false).ToList();

                if (SDate != null)
                {

                    EDate = ((DateTime)EDate).AddDays(1);

                    qLead = qLead.Where(s => (s.CreateDateTime >= SDate && s.CreateDateTime <= EDate)).ToList();
                }

                if (NameSeach != "")
                {
                    NameSeach = NameSeach.ToUpper();
                    qLead = qLead.Where(s => ((s.Name.ToUpper()).Contains(NameSeach)) ).ToList();

                }
                if (SurNameSeach != "")
                {
                    SurNameSeach = SurNameSeach.ToUpper();
                    qLead = qLead.Where(s => ((s.SurName.ToUpper()).Contains(SurNameSeach))).ToList();

                }
                if (TelSeach != "")
                {

                    qLead = qLead.Where(s => s.Tel.Contains(TelSeach)).ToList();

                }
                if (IDCardSeach != "")
                {
                    IDCardSeach = IDCardSeach.ToUpper();
                    qLead = qLead.Where(s => (s.IDCard.ToUpper()).Contains(IDCardSeach)).ToList();

                }


                ViewBag.lstLeads = qLead.Where(s => s.IsDelete == false).OrderByDescending(S => S.CreateDateTime).ToList();

            }

            if (IsshowTab == "Call")
            {
                var qCall = DMPS.sp_MainCallVisit().Where(s => s.Call1 != null || s.Call2 != null || s.Call4 != null || s.Call3 != null ).ToList();

                if (SDate != null)
                {

                    EDate = ((DateTime)EDate).AddDays(1);

                    qCall = qCall.Where(s => (s.Call1  >= SDate && s.Call1 <= EDate) || (s.Call2 >= SDate && s.Call2 <= EDate) || (s.Call3 >= SDate && s.Call3 <= EDate) || (s.Call4 >= SDate && s.Call4 <= EDate) || (s.Colsed1 >= SDate && s.Colsed1 <= EDate) || (s.Colsed2 >= SDate && s.Colsed2 <= EDate)).ToList();
                }

                if (NameSeach != ""/* || SurNameSeach != "" || TelSeach != ""*/)
                {
                    NameSeach = NameSeach.ToUpper();
                    qCall = qCall.Where(s => ((s.Name.ToUpper()).Contains(NameSeach)) /*|| (s.SurName.Contains(SurNameSeach)) || (s.Tel.Contains(TelSeach))*/).ToList();

                }
                if (SurNameSeach != "" )
                {
                    SurNameSeach = SurNameSeach.ToUpper();
                    qCall = qCall.Where(s => ((s.SurName.ToUpper()).Contains(SurNameSeach))).ToList();

                }
                if (TelSeach != "")
                {

                    qCall = qCall.Where(s => s.Tel.Contains(TelSeach)).ToList();

                }
                if (IDCardSeach != "")
                {
                    IDCardSeach = IDCardSeach.ToUpper();
                    qCall = qCall.Where(s => (s.C_IDCard.ToUpper()).Contains(IDCardSeach)).ToList();

                }


                ViewBag.lstCalls = qCall.Where(s => s.IsDelete == false).OrderByDescending(S => S.CreateDateTime).ToList();

            }

            if (IsshowTab == "Walk")
            {
                var qWalk = DMPS.sp_MainCallVisit().Where(s => s.Visit1 != null || s.Visit2 != null).ToList();

                if (SDate != null)
                {

                    EDate = ((DateTime)EDate).AddDays(1);

                    qWalk = qWalk.Where(s => (s.Visit1 >= SDate && s.Visit1 <= EDate) || (s.Visit2 >= SDate && s.Visit2 <= EDate) || (s.Colsed1 >= SDate && s.Colsed1 <= EDate) || (s.Colsed2 >= SDate && s.Colsed2 <= EDate)).ToList();
                }

                if (NameSeach != "" /*|| SurNameSeach != "" || TelSeach != ""*/)
                {
                    NameSeach = NameSeach.ToUpper();
                    qWalk = qWalk.Where(s => ((s.Name.ToUpper()).Contains(NameSeach)) /*|| (s.SurName.Contains(SurNameSeach)) || (s.Tel.Contains(TelSeach))*/).ToList();
                }
                if (SurNameSeach != "")
                {
                    SurNameSeach = SurNameSeach.ToUpper();
                    qWalk = qWalk.Where(s => ((s.SurName.ToUpper()).Contains(SurNameSeach))).ToList();

                }
                if (TelSeach != "")
                {

                    qWalk = qWalk.Where(s => s.Tel.Contains(TelSeach)).ToList();

                }
                if (IDCardSeach != "")
                {
                    IDCardSeach = IDCardSeach.ToUpper();
                    qWalk = qWalk.Where(s => (s.C_IDCard.ToUpper()).Contains(IDCardSeach)).ToList();

                }


                ViewBag.lstWalks = qWalk.Where(s => s.IsDelete == false).OrderByDescending(S => S.CreateDateTime).ToList();

            }

            return PartialView();
        }

        public string PDPADisplayInfo()
        {

            var PDPAInfo = DMPS.MasPDPAs.Where(s => s.IsDelete == false).ToList();
            return PDPAInfo.ToObj2Json();
        }
        
        public string AddPDPA(int _EmpID, int _CallVisitID, string _PDPAID, string _RDO, string _Title, bool isUpdate)
        {

            try
            {
                //var CallVisitPDPAsID = DMPS.CallVisitPDPAs.SingleOrDefault(s => s.CallVisitID == _CallVisitID);

                var arrPDPAID = _PDPAID.Split(',');
                var arrRDO = _RDO.Split(',');

                if (_CallVisitID != 0)
                {
                    //delete
                    var CallVisitPDPAsID = DMPS.CallVisitPDPAs.Where(s => s.CallVisitID == _CallVisitID).ToList();

                    for (int i = 0; i < CallVisitPDPAsID.Count; i++)
                    {
                        DMPS.Entry(CallVisitPDPAsID[i]).State = EntityState.Deleted;
                    }

                    DMPS.SaveChanges();
                }

                if (_PDPAID != "")
                {
                    for (var i = 0; i < arrPDPAID.Length; i++)
                    {
                       
                         
                            CallVisitPDPA cvPDPA = new CallVisitPDPA();
                            cvPDPA.CallVisitID = _CallVisitID;
                            cvPDPA.PDPAID = int.Parse(arrPDPAID[i]);
                            if (arrRDO[i] == "0")
                            {
                                cvPDPA.Status = false;
                            }
                            if (arrRDO[i] == "1")
                            {
                                cvPDPA.Status = true;
                            }
                            if (arrRDO[i] == "")
                            {
                                cvPDPA.Status = null;
                            }

                            cvPDPA.Creator = _EmpID;
                            cvPDPA.CreateDateTime = DateTime.Now;
                            cvPDPA.Reviser = _EmpID;
                            cvPDPA.ReviseDateTime = DateTime.Now;
                            cvPDPA.IsDelete = false;
                            DMPS.CallVisitPDPAs.Add(cvPDPA);
                            

                    }

                DMPS.SaveChanges();
                    //if(!isUpdate)
                    //    InsertPDPALog(_EmpID.ToString(), _CallVisitID, _PDPAID, _RDO, _Title);

                }
            }                
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "False";
            }

          
            return "True";
        }


        public string InsertPDPALog(string _EmpID, int _CallVisitID, string _PDPAID, string _RDO, string _Title)
        {
            try
            {
                var arrPDPAID = _PDPAID.Split(',');
                var arrRDO = _RDO.Split(',');
                var arrTitle = _Title.Split(',');

               
                if (_PDPAID != "")
                {
                    for (var i = 0; i < arrPDPAID.Length; i++)
                    {

                        PDPALog _Log = new PDPALog();
                        _Log.CallVisitID = _CallVisitID;
                        _Log.PDPAID = int.Parse(arrPDPAID[i]);
                        if (arrRDO[i] == "0")
                        {
                            _Log.Status = false;
                        }
                        if (arrRDO[i] == "1")
                        {
                            _Log.Status = true;
                        }
                        if (arrRDO[i] == "")
                        {
                            _Log.Status = null;
                        }

                        _Log.Activity = "INSERT";
                        _Log.PDPATitle = arrTitle[i];
                        _Log.Reviser = _EmpID.ToString();
                        _Log.ReviseDateTime = DateTime.Now;
                        DMPS.PDPALogs.Add(_Log);


                    }

                    DMPS.SaveChanges();

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "False";
            }

            return "True";
        }

        public string UpdatePDPALog(string _EmpID, int _CallVisitID, string _PDPAID, string _RDO, string _Title)
        {

            try
            {

                var arrPDPAID = _PDPAID.Split(',');
                var arrRDO = _RDO.Split(',');
                var arrTitle = _Title.Split(',');

                if (_PDPAID != "")
                {
                    for (var i = 0; i < arrPDPAID.Length; i++)
                    {
                        PDPALog PDPAsID = new PDPALog();
                        PDPAsID.CallVisitID = _CallVisitID;
                        PDPAsID.PDPAID = int.Parse(arrPDPAID[i]);
                        if (arrRDO[i] == "0")
                        {
                            PDPAsID.Status = false;
                        }
                        if (arrRDO[i] == "1")
                        {
                            PDPAsID.Status = true;
                        }
                        if (arrRDO[i] == "")
                        {
                            PDPAsID.Status = null;
                        }

                        PDPAsID.Activity = "UPDATE";
                        PDPAsID.PDPATitle = arrTitle[i]; 
                        PDPAsID.Reviser = _EmpID;
                        PDPAsID.ReviseDateTime = DateTime.Now;
                        DMPS.PDPALogs.Add(PDPAsID);


                    }

                    DMPS.SaveChanges();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "False";
            }

            return "True";
        }

        public string CheckDuplicateByName(string _Name, string _SurName)
        {
            if (_Name != "")
            {

                var NameCheck = DMPS.CallVisits.Where(s => s.Name.ToUpper() == _Name.ToUpper() && s.SurName.ToUpper() == _SurName.ToUpper()).ToList();
                return NameCheck.ToObj2Json();
            }

            return "false";
        }
        public string CheckDuplicateByTel(string _Tel )
        {
            
            if (_Tel != "")
            {
                var chkTel = DMPS.CallVisits.Where(s => s.Tel == _Tel).ToList();
                return chkTel.ToObj2Json();
            }

           return "false"; 
        }
        public string CheckDuplicateByIDCard(string _IDCard)
        {

            if (_IDCard != "")
            {
                var chkIDCard = DMPS.CallVisits.Where(s => s.IDCard.ToUpper() == _IDCard.ToUpper()).ToList();
                return chkIDCard.ToObj2Json();
            }

            return "false";
        } 
    }
}