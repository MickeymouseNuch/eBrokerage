using Inspinia_MVC5.API;
using Inspinia_MVC5.Models.DMPS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspinia_MVC5.Controllers.CN_Deposit
{
    public class DMPS_DraftDocumentController : Controller
    {
        PMdbEntities1 DMPS = new PMdbEntities1();
        cApiPortal cApi = new cApiPortal();
        // GET: DMPS_DraftDocument
        public ActionResult Index()
        {
            return View();
        }

        public FileResult LoadDraftDocument(string Filename)
        {
            Filename = Uri.UnescapeDataString(Filename);
            //string pathSource = Server.MapPath("../Report/eBrokerage/Draft/" + Filename + );
            string pathSource = Server.MapPath("~/" + Filename );

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


        public string LoadData2UploadDraftDocument(long DraftDocumentid)
        {
            var DraftDocumentData = DMPS.DraftDocumentAttachments.Where(s => s.DraftDocumentID == DraftDocumentid && s.IsDelete == false).ToList();
            for (int i = 0; i < DraftDocumentData.Count; i++)
            {
                DraftDocumentData[i].RevisedBy = cApi.apiGetEmployeeDetailList().Where(s => s.EmpID == DraftDocumentData[i].RevisedBy).SingleOrDefault().DisplayName;
            }

            return DraftDocumentData.ToObj2Json();
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

        public string AddDraftDocumentAttachments(string emp_id, long DraftDocumentid, string DraftDocumentpath)
        {
            var EmpID = emp_id;
            var DraftDocumentID = DraftDocumentid;
            var DraftDocumentPath = DraftDocumentpath;
            var AddDDATTID = 0;

            try
            {
                DraftDocumentAttachment dd = new DraftDocumentAttachment();
                dd.DraftDocumentID = DraftDocumentID;
                dd.FilePath = DraftDocumentPath;
                dd.RevisedBy = EmpID;
                dd.RevisedDateTime = DateTime.Now;
                dd.IsDelete = false;

                DMPS.DraftDocumentAttachments.Add(dd);
                DMPS.SaveChanges();
                AddDDATTID = dd.DraftDocumentAttachmentID;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return AddDDATTID.ToObj2Json();

        }

        public ActionResult DeleteDraftDocumentAttachments(int DraftDocumentAtid)
        {

            var DraftDocumentAttDelRec = DMPS.DraftDocumentAttachments.SingleOrDefault(s => s.DraftDocumentAttachmentID == DraftDocumentAtid);

            try
            {
                DraftDocumentAttDelRec.IsDelete = true;
                DMPS.Entry(DraftDocumentAttDelRec).State = EntityState.Modified;
                DMPS.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json("False");

            }

            return Json("True");

        }


        public ActionResult DeleteDraftDocumentFiltPath(string DraftDocumentFilePath)
        {

            try
            {

                string path = DraftDocumentFilePath;

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

        public string LoadFileNameMain(long DraftDocumentid)
        {
            var DraftDoclatest = DMPS.DraftDocumentAttachments.Where(s => s.DraftDocumentID == DraftDocumentid && s.IsDelete == false).OrderByDescending(s => s.RevisedDateTime).ToList();
            var FileNameMain = DraftDoclatest[0];
            return FileNameMain.ToObj2Json();
        }

        public string LoadDataHistory(long DraftDocumentid)
        {
            var DraftDocumentHistory2up = DMPS.vw_DraftDocumentAttachmentHistory.Where(s => s.DraftDocumentID == DraftDocumentid).ToList();
            return DraftDocumentHistory2up.ToObj2Json();
        }

        public ActionResult DraftDocumentAttachmentHistory(string emp_id, int staID, int DDAHID)
        {
            var AttachmentStatusTypeID = staID;
            var EmpID = emp_id;
            var DraftDocumentAttHisID = DDAHID;
 
            try
            {

                DraftDocumentAttachmentHistory DDAH = new DraftDocumentAttachmentHistory();
                DDAH.DraftDocumentAttachmentID = DraftDocumentAttHisID;
                DDAH.AttachmentStatusTypeID = AttachmentStatusTypeID;
                DDAH.RevisedBy = EmpID;
                DDAH.RevisedDateTime = DateTime.Now;

                DMPS.DraftDocumentAttachmentHistories.Add(DDAH);
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