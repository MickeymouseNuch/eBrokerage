using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Inspinia_MVC5.Models.MainClass;
using System.Net.Mail;

namespace Inspinia_MVC5.Models.MainClass
{
    public class Email
    {
        MASDBEntities MASDB = new MASDBEntities();

        public long SaveTask(long ApplicationId,string WorkflowTaskNo,long WorkflowId,long VersionNo,string DocumentId,long WorkflowStateID, long StateId
            ,string EmpId,long DeptId,long UserId,string UserLogon,string Action,System.DateTime ActionDatetime,decimal Credit,string SubmitedBy,bool EmailSent
            ,System.DateTime EmailDatetime,bool UnRead,string Comment)
        {
            long result = 0;
            try
            {
                string sqlcmd = string.Empty;
                sqlcmd = string.Format("exec sp_ap_WorkflowTaskInsert {0},'{1}',{2},{3},'{4}',{5},{6},'{7}',{8},{9},'{10}','{11}','{12}',"
                    , ApplicationId
                    , WorkflowTaskNo
                    , WorkflowId
                    , VersionNo
                    , DocumentId
                    , WorkflowStateID
                    , StateId
                    , EmpId
                    , DeptId
                    , UserId
                    , UserLogon
                    , Action
                    , ActionDatetime
                    , Credit
                    , SubmitedBy
                    , EmailSent
                    , EmailDatetime
                    , UnRead
                    , Comment
                    );
                //WorkflowTask wft = new WorkflowTask();
                //wft.ApplicationId = ApplicationId;
                //wft.WorkflowTaskNo = WorkflowTaskNo;
                //wft.WorkflowId = WorkflowId;
                //wft.VersionNo = VersionNo;
                //wft.DocumentId = DocumentId;
                //wft.WorkflowStateID = WorkflowStateID;
                //wft.StateId = StateId;
                //wft.EmpId = EmpId;
                //wft.DeptId = DeptId;
                //wft.UserId = UserId;
                //wft.UserLogon = UserLogon;
                //wft.Action = Action;
                //wft.ActionDatetime = ActionDatetime;
                //wft.Credit = Credit;
                //wft.SubmitedBy = SubmitedBy;
                //wft.EmailSent = EmailSent;
                //wft.EmailDatetime = EmailDatetime;
                //wft.UnRead = UnRead;
                //wft.Comment = Comment;
                //MASDB.WorkflowTasks.Add(wft);
                //MASDB.SaveChanges();
                //result = MASDB.WorkflowTasks.Max(s => s.TaskId);
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

        public long SaveEmail(WorkflowEmail wfe, List<WorkflowEmailTable> LstWfe)
        {
            long result = 0;
            try
            {
                //MASDB.WorkflowEmails.Add(wfe);
                //MASDB.SaveChanges();
                //long wfeID = MASDB.WorkflowEmails.Max(s => s.WorkflowEmailHeadID);

                //foreach (var item in LstWfe)
                //{
                //    item.WorkflowEmailHeadID = wfeID;
                //    MASDB.WorkflowEmailTables.Add(item);
                //}
                //MASDB.SaveChanges();
                //result = wfeID;
            }
            catch (Exception ex)
            {
                result = 0;
            }

            return result;
        }

        public WorkflowEmail getWorkflowEmail(long ApplicationId,string DocumentId,string WorkflowEmail1,string WorkflowEmailSubject,string WorkflowEmailHeader,string WorkflowEmailFooter,string WorkflowEmailStattus,long TaskId)
        {
            WorkflowEmail wfe = new WorkflowEmail();
            wfe.ApplicationId = ApplicationId;
            wfe.DocumentId = DocumentId;
            wfe.WorkflowEmail1 = WorkflowEmail1;
            wfe.WorkflowEmailSubject = WorkflowEmailSubject;
            wfe.WorkflowEmailHeader = WorkflowEmailHeader;
            wfe.WorkflowEmailFooter = WorkflowEmailFooter;
            wfe.WorkflowEmailStattus = WorkflowEmailStattus;
            wfe.TaskId = TaskId;
            return wfe;
        }

        public WorkflowEmailTable getEmailLine(long WorkflowEmailHeadID, long NumberRecords, string EmailBodyLable, string EmailBodyValue, string EmailBodyEventually, long Line)
        {
            WorkflowEmailTable el = new WorkflowEmailTable();
            el.WorkflowEmailHeadID = WorkflowEmailHeadID;
            el.NumberRecords = NumberRecords;
            el.EmailBodyLable = EmailBodyLable;
            el.EmailBodyValue = EmailBodyValue;
            el.EmailBodyEventually = EmailBodyEventually;
            el.Line = Line;
            return el;
        }

        public void SendEmail(string Subject, List<string> Add, List<string> CC, List<string> BCC, string HtmlBody)
        //public void SendEmail()
        {
            string host = System.Configuration.ConfigurationSettings.AppSettings["Host"];
            int port = Convert.ToInt16(System.Configuration.ConfigurationSettings.AppSettings["Port"]);
            string SetfromMail = System.Configuration.ConfigurationSettings.AppSettings["_fromMail"];

            SmtpClient smtp = new SmtpClient(host, port);
            MailMessage msg = new MailMessage();

            msg.IsBodyHtml = true;
            //msg.Subject = Subject;
            msg.Subject = Subject;
            msg.From = new MailAddress(SetfromMail);

            msg.To.Add(new MailAddress("pisarn.s@univentures.co.th"));
            //msg.To.Add(new MailAddress("watchara.s@univentures.co.th"));
            foreach (var itemAdd in Add)
            {
                msg.To.Add(new MailAddress(itemAdd));
            }
            //foreach (var itemCC in CC)
            //{
            //    msg.CC.Add(new MailAddress(itemCC));
            //}
            //foreach (var itemBCC in BCC)
            //{
            //    msg.Bcc.Add(new MailAddress(itemBCC));
            //}

            string BodyFormat = HtmlBody;
            msg.Body = BodyFormat;
            smtp.Send(msg);
        }

        public string getHTML(long TaskID)
        {
            string resultGetHtml = string.Empty;
            //WorkflowEmail wf = MASDB.WorkflowEmails.Where(s => s.TaskId == TaskID).SingleOrDefault();
            //if (wf == null) { return ""; }
            //List<WorkflowEmailTable> lstWft = MASDB.WorkflowEmailTables.Where(s => s.WorkflowEmailHeadID == wf.WorkflowEmailHeadID).ToList();
            //int countLine = 1;

            //resultGetHtml += "<html><head><style type =\"text/css\">tbody tr:nth-child(even) {background-color:#f4f4f4; }tbody tr:nth-child(odd) {background-color:#FFFFFF;}</style></head><body>";
            //resultGetHtml += "<table style=\"width: 650px\">";
            //resultGetHtml += "<thead><tr><th colspan=\"3\" style=\"background-color: #003399; text-align: left\"><strong><font color=\"#FFFFFF\">" + wf.WorkflowEmailHeader.ToString() + "</font></strong></th></tr></thead>";
            //resultGetHtml += "<tbody>";

            //foreach (var item in lstWft)
            //{
            //    if (countLine % 2 == 0)
            //    {
            //        resultGetHtml += "<tr style=\"background-color:#dedede; \">";
            //    }
            //    else
            //    {
            //        resultGetHtml += "<tr style=\"background-color:#FFFFFF; \">";
            //    }

            //    resultGetHtml += "<td align='right'style=\"width: 200px;\"><strong>" + item.EmailBodyLable + "</strong></td>";

            //    if (item.Line == 0)
            //    {
            //        resultGetHtml += "<td align='right' style=\"width: 400px\">" + item.EmailBodyValue + "</td>";
            //    }
            //    else if (item.Line == 1)
            //    {
            //        resultGetHtml += "<td align='right' style=\"border-bottom: 1px solid black; width: 400px\">" + item.EmailBodyValue + "</td>";
            //    }
            //    else if (item.Line == 2)
            //    {
            //        resultGetHtml += "<td align='right' style=\"border-bottom: 4px double black; width: 400px\">" + item.EmailBodyValue + "</td>";
            //    }
            //    else
            //    {

            //    }
            //    resultGetHtml += "<td style=\"width: 50px\">" + item.EmailBodyEventually + "</td>";
            //    resultGetHtml += "</tr>";
            //    countLine++;
            //}

            //resultGetHtml += "</tbody>";
            //resultGetHtml += "</table>";
            //resultGetHtml += "<br>";

            ////resultGetHtml += getLink(DocumentNo, UserLogon) + "<br>";
            ////resultGetHtml += "<br><strong>*** Status " + BodyLine.Rows[0]["DocumentStatusName"].ToString() + "</strong><br>";

            //resultGetHtml += "</body></html>";
            return resultGetHtml;
        }

        public long SaveEmailLog(WorkflowEmailLog wfel)
        {
            long result = 0;
            //try
            //{
            //    MASDB.WorkflowEmailLogs.Add(wfel);
            //    MASDB.SaveChanges();
            //    result = MASDB.WorkflowEmailLogs.Max(s => s.WorkflowEmailLog_ID);
            //}
            //catch (Exception ex)
            //{
            //    result = 0;
            //}
            return result;
        }
    }
}