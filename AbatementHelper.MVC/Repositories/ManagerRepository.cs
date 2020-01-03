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

        public async Task<List<WebApiStore>> GetAllStoresAsync()
        {
            AddTokenToHeader();

            string managerId = HttpContext.Current.Request.Cookies["UserID"].Value;

            if (managerId != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/Manager/GetAllStoresAsync/" + managerId);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<List<WebApiStore>>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<SelectedStore> SelectAsync(string id)
        {
            AddTokenToHeader();

            if (id != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/Manager/SelectAsync/" + id);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<SelectedStore>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<WebApiStore> EditStoreAsync(string id)
        {
            AddTokenToHeader();

            if (id != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/Manager/EditStoreAsync/" + id);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<WebApiStore>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<Response> EditStoreAsync(WebApiStore store)
        {
            AddTokenToHeader();

            if (store != null)
            {
                HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Manager/EditStoreAsync", store);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<Response>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<WebApiStore> DetailsStoreAsync(string id)
        {
            if (id != null)
            {
                AddTokenToHeader();

                HttpResponseMessage request = await apiClient.GetAsync("api/Manager/DetailsStoreAsync/" + id);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<WebApiStore>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<Response> AbandonStoreAsync(string id)
        {
            WebApiStoreAssign storeUnassign = new WebApiStoreAssign();

            storeUnassign.ManagerId = HttpContext.Current.Request.Cookies["UserID"].Value;
            storeUnassign.StoreId = id;

            if (storeUnassign.ManagerId != null && storeUnassign.StoreId != null)
            {
                AddTokenToHeader();

                var jsonContent = JsonConvert.SerializeObject(storeUnassign);

                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage request = await apiClient.PostAsync("api/Manager/AbandonStoreAsync/", httpContent);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<Response>();

                    return resultContent;
                }
            }

            return null;
        }
    }
}