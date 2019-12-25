using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.MVC.Models;
using Hanssens.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Script.Serialization;

namespace AbatementHelper.MVC.Repositories
{
    public class AccountRepository
    {
        private HttpClient apiClient;
        public bool LoginSuccessful;
        public bool RegisterSuccessful;
        public string ResponseMessageText = null;
        Response responseModel = new Response();

        public AccountRepository()
        {
            InitializeClient();
        }

        public void AddTokenToHeader()
        {
            var token = HttpContext.Current.Request.Cookies["Access_Token"];

            if (token != null)
            {
                apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value.ToString());
            }
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Response> Login(string email, string userName, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Email", email),
                new KeyValuePair<string, string>("UserName", userName),
                new KeyValuePair<string, string>("Password", password)
            });

            var request = await apiClient.PostAsync("/api/Login/Authenticate", data);

            var response = request.Content.ReadAsStringAsync();

            string responseString = response.Result;

            responseModel = JsonConvert.DeserializeObject<Response>(responseString);

            ResponseMessageText = responseModel.ResponseMessage;

            if (responseModel.ResponseCode == (int)HttpStatusCode.OK)
            {

                LoginSuccessful = true;
            }
            else
            {
                LoginSuccessful = false;
            }

            return responseModel;
        }

        public async Task<string> Register(User user)
        {
            var jsonContent = JsonConvert.SerializeObject(user);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            response = await apiClient.PostAsync("api/Account/Register", httpContent);

            RegisterSuccessful = response.IsSuccessStatusCode;

            var result = response.Content.ReadAsStringAsync();

            return await result;
        }

        public async Task<Response> Login(AuthenticationModel authentication)
        {
            var jsonContent = JsonConvert.SerializeObject(authentication);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var request = await apiClient.PostAsync("api/Account/Authenticate", httpContent);

            string response = await request.Content.ReadAsStringAsync();

            LoginSuccessful = request.IsSuccessStatusCode;

            var result = request.Content.ReadAsAsync<AuthenticatedUser>();

            Response resultModel = JsonConvert.DeserializeObject<Response>(response);

            return resultModel;
        }

        public WebApiUser Details()
        {
            WebApiUser user = new WebApiUser();

            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            if (id != null)
            {
                var response = apiClient.GetAsync("api/Account/Details/" + id);
                response.Wait();

                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var resultContent = result.Content.ReadAsAsync<WebApiUser>();
                    resultContent.Wait();

                    user = resultContent.Result;
                }
            }

            return user;
        }

        public WebApiUser Edit()
        {
            WebApiUser user = new WebApiUser();

            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            if (id != null)
            {
                var response = apiClient.GetAsync("api/Account/Edit/" + id);
                response.Wait();

                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var resultContent = result.Content.ReadAsAsync<WebApiUser>();
                    resultContent.Wait();

                    user = resultContent.Result;
                }
            }

            return user;

        }

        public Response Edit(WebApiUser user)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsJsonAsync("api/Account/Edit", user);
            response.Wait();

            var result = response.Result;

            var resultContent = result.Content.ReadAsAsync<Response>();

            responseModel.ResponseMessage = resultContent.Result.ResponseMessage;
            responseModel.Success = resultContent.Result.Success;

            return responseModel;
        }

        public bool Delete(WebApiUser user)
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            if (id != null)
            {
                var response = apiClient.PutAsync("api/Account/Delete/" + id, null);
                response.Wait();

                var result = response.Result;


                var resultString = result.ToString();


                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
            }

            return false;
        }

    }
}