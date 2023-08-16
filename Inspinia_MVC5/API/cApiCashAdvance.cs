using Inspinia_MVC5.Models.CashAdvance;
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

namespace Inspinia_MVC5.API
{
    public class cApiCashAdvance
    {
        string apihost = ConfigurationSettings.AppSettings["APIHost"].ToString();
        string token = ConfigurationSettings.AppSettings["APIKey"].ToString();

        public List<cBudgetAccountTable> apiGetBudgetAccountList()
        {
            List<cBudgetAccountTable> KeyReuslt = new List<cBudgetAccountTable>();

            string URL = apihost + "GetBudgetAccount";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    budgetaccountid = "",
                    flag = "A"
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
                        KeyReuslt = JsonResult.data.ToJson2List<cBudgetAccountTable>();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }
        public cBudgetAccountTable apiGetBudgetAccount(string BudgetAccountID)
        {
            cBudgetAccountTable KeyReuslt = new cBudgetAccountTable();

            string URL = apihost + "GetBudgetAccount";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    budgetaccountid = BudgetAccountID,
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
                        List<cBudgetAccountTable> temp = JsonResult.data.ToJson2List<cBudgetAccountTable>();
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

        public List<cBudgetModelTable> apiGetBudgetModelList()
        {
            List<cBudgetModelTable> KeyReuslt = new List<cBudgetModelTable>();

            string URL = apihost + "GetBudgetModel";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    budgetmodel = "",
                    flag = "A"
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
                        KeyReuslt = JsonResult.data.ToJson2List<cBudgetModelTable>();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return KeyReuslt;
        }
        public cBudgetModelTable apiGetBudgetModel(string BudgetModelID)
        {
            cBudgetModelTable KeyReuslt = new cBudgetModelTable();

            string URL = apihost + "GetBudgetModel";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string dataResult = string.Empty;

            try
            {
                string apiUrl = URL;

                var input = new
                {
                    token = token,
                    budgetmodelid = BudgetModelID,
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
                        List<cBudgetModelTable> temp = JsonResult.data.ToJson2List<cBudgetModelTable>();
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