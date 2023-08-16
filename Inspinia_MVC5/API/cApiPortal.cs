using Inspinia_MVC5.Models.MainClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Script.Serialization;
using Inspinia_MVC5.Models;

namespace Inspinia_MVC5.API
{
    public class cApiPortal
    {
        string apihost = ConfigurationSettings.AppSettings["APIHost"].ToString();
        string token = ConfigurationSettings.AppSettings["APIKey"].ToString();
        MASDBEntities masdb = new MASDBEntities();

        public List<cApplication> apiGetApplicationList()
        {
            List<cApplication> KeyReuslt = new List<cApplication>();

            string URL = apihost + "GetApplication/getApplicationList";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    applicationid = "",
                    flag = "A"
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        KeyReuslt = JsonResult.data.ToJson2List<cApplication>();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }
        public cApplication apiGetApplication(long ApplicationID)
        {
            cApplication KeyReuslt = new cApplication();

            string URL = apihost + "GetApplication/getApplicationList";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    applicationid = ApplicationID,
                    flag = "P"
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        List<cApplication> temp = JsonResult.data.ToJson2List<cApplication>();
                        if (temp.Count != 0)
                        {
                            KeyReuslt = temp[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }
        public cImageTable apiGetImageApplication(long ApplicationID)
        {
            cImageTable KeyReuslt = new cImageTable();

            string URL = apihost + "GetApplication/getApplicationImage";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    applicationid = ApplicationID
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        List<cImageTable> temp = JsonResult.data.ToJson2List<cImageTable>();
                        if (temp.Count != 0)
                        {
                            KeyReuslt = temp[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }

        public List<MenuList> apiGetMainMenuList(string EmployeeID, long ApplicationID)
        {
            List<MenuList> KeyReuslt = new List<MenuList>();

            string URL = apihost + "GetMenu/getMainMenu";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    empid = EmployeeID,
                    applicationid = ApplicationID.ToString()
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        KeyReuslt = JsonResult.data.ToJson2List<MenuList>();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }
        public List<MenuList> apiGetSubMenuList(string EmployeeID, long ApplicationID)
        {
            List<MenuList> KeyReuslt = new List<MenuList>();

            string URL = apihost + "GetMenu/getSubMenu";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    empid = EmployeeID,
                    applicationid = ApplicationID.ToString()
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        KeyReuslt = JsonResult.data.ToJson2List<MenuList>();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }

        public List<cEmployeeDetail> apiGetEmployeeDetailList()
        {
            List<cEmployeeDetail> KeyReuslt = new List<cEmployeeDetail>();

            string URL = apihost + "GetEmployee/getEmployeeDetail";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    empid = "",
                    flag = "A"
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        KeyReuslt = JsonResult.data.ToJson2List<cEmployeeDetail>();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }
        public cEmployeeDetail apiGetEmployeeDetail(string EmpID)
        {
            cEmployeeDetail KeyReuslt = new cEmployeeDetail();

            string URL = apihost + "GetEmployee/getEmployeeDetail";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            //masdb.Database.ExecuteSqlCommand("Insert into LogExceptionTable (MessageLog,CreateDate) values ('"+ URL +"',getdate())");

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    empid = EmpID,
                    flag = "P"
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                //masdb.Database.ExecuteSqlCommand("Insert into LogExceptionTable (MessageLog,CreateDate) values ('" + response.IsSuccessStatusCode.ToString() + "',getdate())");
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    //masdb.Database.ExecuteSqlCommand("Insert into LogExceptionTable (MessageLog,CreateDate) values ('" + dataResult.ToString().Replace("'", "") + "',getdate())");
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        List<cEmployeeDetail> temp = JsonResult.data.ToJson2List<cEmployeeDetail>();
                        if (temp.Count != 0)
                        {
                            KeyReuslt = temp[0];
                        }
                    }
                }
                //else { masdb.Database.ExecuteSqlCommand("Insert into LogExceptionTable (MessageLog,CreateDate) values ('IsSuccessStatusCode : fasle',getdate())"); }
            }
            catch (Exception ex)
            {
                //masdb.Database.ExecuteSqlCommand("Insert into LogExceptionTable (MessageLog,CreateDate) values ('" + ex.ToString() + "',getdate())");
            }
            return KeyReuslt;
        }

        public cRoleAdminApp apiGetRoleAdminApp(string EmpID, long ApplicationID)
        {
            cRoleAdminApp KeyReuslt = new cRoleAdminApp();

            string URL = apihost + "GetEmployee/getAuthenRowAdmin";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    empid = EmpID,
                    applicationid = ApplicationID
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        List<cRoleAdminApp> temp = JsonResult.data.ToJson2List<cRoleAdminApp>();
                        if (temp != null && temp.Count != 0)
                        {
                            KeyReuslt = temp[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }

        public List<cCompany> apiGetCompanyList()
        {
            List<cCompany> KeyReuslt = new List<cCompany>();

            string URL = apihost + "GetCompany/getCompanyList";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    companyid = "",
                    flag = "A"
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        KeyReuslt = JsonResult.data.ToJson2List<cCompany>();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }
        public cCompany apiGetCompanyByCompanyID(long CompanyID)
        {
            cCompany KeyReuslt = new cCompany();

            string URL = apihost + "GetCompany/getCompanyList";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    companyid = CompanyID,
                    flag = "P"
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        List<cCompany> temp = JsonResult.data.ToJson2List<cCompany>();
                        if (temp.Count != 0)
                        {
                            KeyReuslt = temp[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }

        public List<cDepartment> apiGetDepartmentList()
        {
            List<cDepartment> KeyReuslt = new List<cDepartment>();

            string URL = apihost + "GetDepartment/GetDepartmentList";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    company = "",
                    flag = "A"
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        KeyReuslt = JsonResult.data.ToJson2List<cDepartment>();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }
        public cDepartment apiGetDepartment(string CompanyCode)
        {
            cDepartment KeyReuslt = new cDepartment();

            string URL = apihost + "GetDepartment/GetDepartmentList";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    company = CompanyCode,
                    flag = "P"
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        List<cDepartment> temp = JsonResult.data.ToJson2List<cDepartment>();
                        if (temp.Count != 0)
                        {
                            KeyReuslt = temp[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }

        public List<cCostCenter> apiGetCostCenterByCompanyID(long CompanyID)
        {
            List<cCostCenter> KeyReuslt = new List<cCostCenter>();

            string URL = apihost + "GetDepartment/GetCostCenterListByCompanyID";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    company = CompanyID,
                    flag = "P"
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        KeyReuslt = JsonResult.data.ToJson2List<cCostCenter>();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            if (KeyReuslt == null) { KeyReuslt = new List<cCostCenter>(); }
            return KeyReuslt;
        }

        public List<cPosition> apiGetPositionList()
        {
            List<cPosition> KeyReuslt = new List<cPosition>();

            string URL = apihost + "GetPosition/GetPositionList";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    position = "",
                    flag = "A"
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        KeyReuslt = JsonResult.data.ToJson2List<cPosition>();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }
        public cPosition apiGetPosition(string PositionCode)
        {
            cPosition KeyReuslt = new cPosition();

            string URL = apihost + "GetPosition/GetPositionList";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    position = PositionCode,
                    flag = "P"
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        List<cPosition> temp = JsonResult.data.ToJson2List<cPosition>();
                        if (temp.Count != 0)
                        {
                            KeyReuslt = temp[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }

        public List<cProject> apiGetProjectList()
        {
            List<cProject> KeyReuslt = new List<cProject>();

            string URL = apihost + "GetProject/GetProjectList";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    projectid = "",
                    flag = "A"
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PostAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        KeyReuslt = JsonResult.data.ToJson2List<cProject>();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }
        public cProject apiGetProject(string ProjectID)
        {
            cProject KeyReuslt = new cProject();

            string URL = apihost + "GetProject";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    projectid = ProjectID,
                    flag = "P"
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent inputContent = new StringContent(inputJson);
                inputContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PutAsync(apiUrl, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    dataResult = response.Content.ReadAsStringAsync().Result;
                    var JsonResult = dataResult.ToJson2Class<cApiResult>();
                    if (JsonResult.status == 1)
                    {
                        List<cProject> temp = JsonResult.data.ToJson2List<cProject>();
                        if (temp.Count != 0)
                        {
                            KeyReuslt = temp[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }





    }


}