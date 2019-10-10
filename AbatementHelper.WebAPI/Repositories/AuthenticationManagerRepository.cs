using AbatementHelper.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.WebAPI.Repositories
{
    public class AuthenticationManagerRepository
    {
        private HttpClient apiClient;
        public bool LoginSuccessful;

        public AuthenticationManagerRepository()
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

        public async Task<AuthenticatedUser> Authenticate(string email, string username, string password)
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

            LoginSuccessful = response.IsSuccessStatusCode;

            var result = await response.Content.ReadAsAsync<AuthenticatedUser>();



            return result;

        }



    }
}