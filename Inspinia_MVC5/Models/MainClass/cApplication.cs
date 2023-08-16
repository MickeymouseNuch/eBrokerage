using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5.Models.MainClass
{
    public class cApplication
    {
        public long ApplicationId { get; set; }
        public string ApplicationNameTH { get; set; }
        public string ApplicationNameEN { get; set; }
        public string ApplicationUrl { get; set; }
        public string ApplicationUrlPublic { get; set; }
        public string ApplicationCode { get; set; }
        public Nullable<long> OrgId { get; set; }
        public Nullable<long> VersionNo { get; set; }
        public string ApplicationPrefixChar { get; set; }
        public string URL_Create { get; set; }
        public string URL_Edit { get; set; }
        public string URL_Delete { get; set; }
        public string URL_Report { get; set; }
        public string URL_Setting { get; set; }
        public string URL_Login { get; set; }
        public string AppicationImage { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> SortOrder { get; set; }
        public Nullable<long> ImageID { get; set; }
        public string ServerName { get; set; }
        public string UserDB { get; set; }
        public string PasswordDB { get; set; }
        public Nullable<long> CompanyID { get; set; }
        public string ColorICON { get; set; }
        public Nullable<bool> IsAuthenWebApp { get; set; }
        public string URL_Project { get; set; }
        public string Description { get; set; }
        public string ImageIcon { get; set; }
    }
}