using AbatementHelper.Classes.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;

namespace AbatementHelper.MVC.Repositories
{
    public class ApiManagerRepository
    {
        private HttpClient apiClient;

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

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            //
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            HttpResponseMessage response;

            response = await apiClient.PostAsync("/token", data);
      
            var result = await response.Content.ReadAsAsync<AuthenticatedUser>();

            return result;
                
        }

        public async Task<string> Register(object user)
        {
            //
            var jsonContent = JsonConvert.SerializeObject(user);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            response = await apiClient.PostAsync("api/Account/Register", httpContent);


            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                return result;
            }
            else
            {
                var result = await response.Content.ReadAsAsync<ResponseError>();

                return "";
            }

            
            

        }
    }
}