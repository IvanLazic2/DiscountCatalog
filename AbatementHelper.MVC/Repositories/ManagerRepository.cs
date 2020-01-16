using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.MVC.Models;
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

namespace AbatementHelper.MVC.Repositories
{
    public class ManagerRepository
    {
        private HttpClient apiClient;

        public ManagerRepository()
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

        public async Task<WebApiListOfStoresResult> GetAllStoresAsync()
        {
            string managerId = HttpContext.Current.Request.Cookies["UserID"].Value;

            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/Manager/GetAllStoresAsync/" + managerId);

            WebApiListOfStoresResult result = await request.Content.ReadAsAsync<WebApiListOfStoresResult>();

            return result;
        }

        public async Task<WebApiSelectedStoreResult> SelectAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/Manager/SelectAsync/" + id);

            WebApiSelectedStoreResult result = await request.Content.ReadAsAsync<WebApiSelectedStoreResult>();

            return result;
        }

        public async Task<WebApiStoreResult> EditStoreAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/Manager/EditStoreAsync/" + id);

            WebApiStoreResult result = await request.Content.ReadAsAsync<WebApiStoreResult>();

            return result;
        }

        public async Task<WebApiResult> EditStoreAsync(WebApiStore store)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Manager/EditStoreAsync", store);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiStoreResult> DetailsStoreAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/Manager/DetailsStoreAsync/" + id);

            WebApiStoreResult result = await request.Content.ReadAsAsync<WebApiStoreResult>();

            return result;
        }

        public async Task<WebApiResult> AbandonStoreAsync(WebApiStoreAssign storeUnassign)
        {
            AddTokenToHeader();

            storeUnassign.ManagerId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var jsonContent = JsonConvert.SerializeObject(storeUnassign);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PostAsync("api/Manager/AbandonStoreAsync/", httpContent);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }
    }
}