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
    public class ApiManagerRepository
    {
        private HttpClient apiClient;
        public bool LoginSuccessful;
        public bool RegisterSuccessful;
        public string ResponseMessageText = null;
        public Response responseModel;

        public ApiManagerRepository()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Response> Authenticate(string email, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("password", password)
            });

            var request = await apiClient.PostAsync("/api/Login/EmailAuthentication", data);

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
            //
            var jsonContent = JsonConvert.SerializeObject(user);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            response = await apiClient.PostAsync("api/Account/Register", httpContent);

            RegisterSuccessful = response.IsSuccessStatusCode;

            var result = response.Content.ReadAsStringAsync();

            return await result;

            //if (response.IsSuccessStatusCode)
            //{
            //    return result;
            //}
            //else
            //{
            //    return result;
            //}
        }

    }
}