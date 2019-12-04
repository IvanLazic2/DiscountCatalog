using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AbatementHelper.MVC.Repositeories
{
    public class AdminRepository
    {
        private HttpClient apiClient;

        public AdminRepository()
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

        public void AddTokenToHeader()
        {
            var token = HttpContext.Current.Request.Cookies["Access_Token"];

            if (token != null)
            {
                apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value.ToString());
            }
        }

        public WebApiListOfUsersResult GetAllUsers()
        {
            WebApiListOfUsersResult users = new WebApiListOfUsersResult();

            AddTokenToHeader();

            var response = apiClient.GetAsync("api/Admin/GetAllUsers/");
            response.Wait();

            var result = response.Result;

            var resultString = result.ToString();

            if (result.IsSuccessStatusCode) //ovdje dobiva server error
            {
                var resultContent = result.Content.ReadAsAsync<WebApiListOfUsersResult>();
                resultContent.Wait();

                users = resultContent.Result;
            }

            return users;
        }

        public WebApiUser EditUser(string id)
        {
            WebApiUser user = new WebApiUser();

            AddTokenToHeader();

            var response = apiClient.GetAsync("api/Admin/EditUser/" + id);
            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<WebApiUser>();
                resultContent.Wait();

                user = resultContent.Result;
            }

            return user;

        }

        public bool EditUser(WebApiUser user)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsJsonAsync("api/Admin/EditUser", user);
            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public WebApiUser DetailsUser(string id)
        {
            WebApiUser user = new WebApiUser();

            AddTokenToHeader();

            var response = apiClient.GetAsync("api/Admin/DetailsUser/" + id);
            response.Wait();

            var result = response.Result;

            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<WebApiUser>();
                resultContent.Wait();

                user = resultContent.Result;
            }

            return user;
        }

        public bool DeleteUser(WebApiUser user)
        {
            AddTokenToHeader();

            var jsonContent = JsonConvert.SerializeObject(user);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = apiClient.PutAsync("api/Admin/DeleteUser/", httpContent);
            response.Wait();

            var result = response.Result;

            
            var resultString = result.ToString();

            
            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public bool RestoreUser(string id)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsync("api/Admin/RestoreUser/" + id, null);
            response.Wait();

            var result = response.Result;


            var resultString = result.ToString();


            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

    }
}