using AbatementHelper.CommonModels.Models;
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
        public Response responseModel;

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

        public async Task<Response> InitialLogin(string email)
        {
            var data = new
            {
                Email = email
            };

            string jsonData = (new JavaScriptSerializer()).Serialize(data);

            HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var request = await apiClient.PostAsync("/api/Login/InitialLogin/", httpContent);

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

        public async Task<Response> GetUserById(string id)
        {
            var request = apiClient.GetAsync("api/Login/GetUserById/" + id);
            request.Wait();

            var result = request.Result;

            var response = await result.Content.ReadAsAsync<Response>();
            
            return response;
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








        //public async Task<Response> StoreLogin(User user)
        //{

        //    var jsonContent = JsonConvert.SerializeObject(user);

        //    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //    var request = await apiClient.PostAsync("api/Login/StoreLogin", httpContent);

        //    var response = request.Content.ReadAsStringAsync();

        //    return new Response();
        //}












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

        public DataBaseUser Edit()
        {
            DataBaseUser user = new DataBaseUser();

            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            var response = apiClient.GetAsync("api/Account/Edit/" + id);
            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<DataBaseUser>();
                resultContent.Wait();

                user = resultContent.Result;
            }

            return user;

        }

        public bool Edit(DataBaseUser user)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsJsonAsync("api/Account/Edit", user);
            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public bool Delete(string id)
        {
            AddTokenToHeader();

            var response = apiClient.DeleteAsync("api/Account/Delete/" + id);
            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

    }
}