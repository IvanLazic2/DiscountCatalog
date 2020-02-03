using AbatementHelper.CommonModels.CreateModels;
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

            WebApiListOfUsersResult result = await request.Content.ReadAsAsync<WebApiListOfUsersResult>();

            return result;
        }

        public async Task<WebApiListOfStoresResult> GetAllStoresAsync()
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/Admin/GetAllStoresAsync/");

            WebApiListOfStoresResult result = await request.Content.ReadAsAsync<WebApiListOfStoresResult>();

            return result;
        }

        public async Task<WebApiResult> CreateUserAsync(CreateUserModel user)
        {
            AddTokenToHeader();

            var jsonContent = JsonConvert.SerializeObject(user);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PostAsync("api/Admin/CreateUserAsync", httpContent);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiResult> CreateStoreAsync(WebApiStore store)
        {
            AddTokenToHeader();

            var jsonContent = JsonConvert.SerializeObject(store);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PostAsync("api/Admin/CreateStoreAsync", httpContent);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiResult> CreateManagerAsync(CreateManagerModel manager)
        {
            AddTokenToHeader();

            var jsonContent = JsonConvert.SerializeObject(manager);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PostAsync("api/Admin/CreateManagerAsync", httpContent);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiStoreResult> EditStoreAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/Admin/EditStoreAsync/" + id);

            WebApiStoreResult result = await request.Content.ReadAsAsync<WebApiStoreResult>();

            return result;
        }

        public async Task<WebApiResult> EditStoreAsync(WebApiStore store)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Admin/EditStoreAsync", store);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiUserResult> EditUserAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/Admin/EditUserAsync/" + id);

            WebApiUserResult result = await request.Content.ReadAsAsync<WebApiUserResult>();

            return result;
        }

        public async Task<WebApiResult> EditUserAsync(WebApiUser user)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Admin/EditUserAsync", user);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiUserResult> UserDetailsAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/Admin/UserDetailsAsync/" + id);

            WebApiUserResult result = await request.Content.ReadAsAsync<WebApiUserResult>();

            return result;
        }

        public async Task<WebApiStoreResult> StoreDetailsAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/Admin/StoreDetailsAsync/" + id);

            WebApiStoreResult result = await request.Content.ReadAsAsync<WebApiStoreResult>();

            return result;
        }

        public async Task<WebApiResult> DeleteUserAsync(WebApiUser user)
        {
            AddTokenToHeader();

            var jsonContent = JsonConvert.SerializeObject(user);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PutAsync("api/Admin/DeleteUserAsync/", httpContent);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiResult> DeleteStoreAsync(WebApiStore store)
        {
            AddTokenToHeader();

            var jsonContent = JsonConvert.SerializeObject(store);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PutAsync("api/Admin/DeleteStoreAsync/", httpContent);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiResult> RestoreUserAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsync("api/Admin/RestoreUserAsync/" + id, null);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiResult> RestoreStoreAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsync("api/Admin/RestoreStoreAsync/" + id, null);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }
    }
}