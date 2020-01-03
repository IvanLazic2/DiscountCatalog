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
        private Response responseModel = new Response();

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

        public async Task<WebApiListOfUsersResult> GetAllUsersAsync()
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/Admin/GetAllUsersAsync/");

            if (request.IsSuccessStatusCode)
            {
                WebApiListOfUsersResult resultContent = await request.Content.ReadAsAsync<WebApiListOfUsersResult>();

                return resultContent;
            }
            else
            {
                return null;
            }

        }

        public async Task<WebApiUser> EditUserAsync(string id)
        {
            WebApiUser user = new WebApiUser();

            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/Admin/EditAsync/" + id);

            if (request.IsSuccessStatusCode)
            {
                WebApiUser resultContent = await request.Content.ReadAsAsync<WebApiUser>();

                return resultContent;
            }
            else
            {
                return null;
            }
        }

        public async Task<Response> EditUserAsync(WebApiUser user)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Admin/EditAsync", user);

            if (request.IsSuccessStatusCode)
            {
                Response resultContent = await request.Content.ReadAsAsync<Response>();

                return resultContent;
            }
            else
            {
                return null;
            }

        }

        public async Task<WebApiUser> DetailsUserAsync(string id)
        {
            WebApiUser user = new WebApiUser();

            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/Admin/DetailsAsync/" + id);

            if (request.IsSuccessStatusCode)
            {
                var resultContent = await request.Content.ReadAsAsync<WebApiUser>();

                return resultContent;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(WebApiUser user)
        {
            AddTokenToHeader();

            var jsonContent = JsonConvert.SerializeObject(user);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PutAsync("api/Admin/DeleteAsync/", httpContent);
            
            if (request.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }  
        }

        public async Task<bool> RestoreUserAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsync("api/Admin/RestoreAsync/" + id, null);

            if (request.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}