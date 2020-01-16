using AbatementHelper.CommonModels.CreateModels;
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

namespace AbatementHelper.MVC.Repositeories
{
    public class StoreAdminRepository
    {
        private HttpClient apiClient;
        private Response responseModel = new Response();

        public StoreAdminRepository()
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

        //Store

        public async Task<WebApiResult> CreateStoreAsync(WebApiStore store)
        {
            store.StoreAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            AddTokenToHeader();

            var jsonContent = JsonConvert.SerializeObject(store);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PostAsync("api/StoreAdmin/CreateStoreAsync", httpContent);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiListOfStoresResult> GetAllStoresAsync()
        {
            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllStoresAsync/" + storeAdminId);

            WebApiListOfStoresResult result = await request.Content.ReadAsAsync<WebApiListOfStoresResult>();

            return result;
        }

        public async Task<WebApiStoreResult> EditStoreAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/EditStoreAsync/" + id);

            WebApiStoreResult result = await request.Content.ReadAsAsync<WebApiStoreResult>();

            return result;
        }

        public async Task<WebApiResult> EditStoreAsync(WebApiStore store)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/StoreAdmin/EditStoreAsync", store);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiResult> PostStoreImageAsync(WebApiPostImage image)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/StoreAdmin/PostStoreImageAsync", image);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<byte[]> GetStoreImageAsync(string id)
        {
            if (id != null)
            {
                AddTokenToHeader();

                HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetStoreImageAsync/" + id);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<byte[]>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<WebApiStoreResult> DetailsStoreAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/DetailsStoreAsync/" + id);

            WebApiStoreResult result = await request.Content.ReadAsAsync<WebApiStoreResult>();

            return result;
        }

        public async Task<WebApiResult> DeleteStoreAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsync("api/StoreAdmin/DeleteStoreAsync/" + id, null);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiResult> RestoreStoreAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsync("api/StoreAdmin/RestoreStoreAsync/" + id, null);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiListOfStoresResult> GetAllDeletedStoresAsync()
        {
            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllDeletedStoresAsync/" + storeAdminId);

            WebApiListOfStoresResult result = await request.Content.ReadAsAsync<WebApiListOfStoresResult>();

            return result;
        }

        public async Task<WebApiSelectedStoreResult> SelectAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/SelectAsync/" + id);

            WebApiSelectedStoreResult result = await request.Content.ReadAsAsync<WebApiSelectedStoreResult>();

            return result;
        }

        //Manager

        public async Task<WebApiListOfManagersResult> GetAllManagersAsync()
        {
            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllManagersAsync/" + storeAdminId);

            WebApiListOfManagersResult result = await request.Content.ReadAsAsync<WebApiListOfManagersResult>();

            return result;
        }

        public async Task<WebApiResult> CreateManagerAsync(CreateManagerModel user)
        {
            user.StoreAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            AddTokenToHeader();

            var jsonContent = JsonConvert.SerializeObject(user);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PostAsync("api/StoreAdmin/CreateManagerAsync", httpContent);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiManagerResult> EditManagerAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/EditManagerAsync/" + id);

            WebApiManagerResult result = await request.Content.ReadAsAsync<WebApiManagerResult>();

            return result;
        }

        public async Task<WebApiResult> EditManagerAsync(WebApiManager user)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/StoreAdmin/EditManagerAsync", user);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiResult> PostManagerImageAsync(WebApiPostImage image)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/StoreAdmin/PostManagerImageAsync", image);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<byte[]> GetManagerImageAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetManagerImageAsync/" + id);

            if (request.IsSuccessStatusCode)
            {
                var resultContent = await request.Content.ReadAsAsync<byte[]>();

                return resultContent;
            }
            else
            {
                return null;
            }
        }

        public async Task<WebApiResult> DeleteManagerAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsync("api/StoreAdmin/DeleteManagerAsync/" + id, null);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiResult> RestoreManagerAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsync("api/StoreAdmin/RestoreManagerAsync/" + id, null);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiListOfManagersResult> GetAllDeletedManagers()
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllDeletedManagersAsync/" + storeAdminId);

            WebApiListOfManagersResult result = await request.Content.ReadAsAsync<WebApiListOfManagersResult>();

            return result;
        }

        public async Task<WebApiManagerResult> DetailsManagerAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/DetailsManagerAsync/" + id);

            WebApiManagerResult result = await request.Content.ReadAsAsync<WebApiManagerResult>();

            return result;
        }

        public async Task<WebApiListOfManagerStoresResult> GetAllManagerStoresAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllManagerStoresAsync/" + id);

            WebApiListOfManagerStoresResult result = await request.Content.ReadAsAsync<WebApiListOfManagerStoresResult>();

            return result;
        }

        //public List<WebApiStore> GetAllNotAssignedStores(string id)
        //{
        //    List<WebApiStore> stores = new List<WebApiStore>();

        //    if (id != null)
        //    {
        //        AddTokenToHeader();

        //        var response = apiClient.GetAsync("api/StoreAdmin/GetAllNotAssignedStores/" + id);
        //        response.Wait();

        //        var result = response.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var resultContent = result.Content.ReadAsAsync<List<WebApiStore>>();
        //            resultContent.Wait();

        //            stores = resultContent.Result;
        //        }
        //    }

        //    return stores;
        //}

        public async Task<WebApiResult> AssignStoreAsync(WebApiStoreAssign storeAssign)
        {
            AddTokenToHeader();

            var jsonContent = JsonConvert.SerializeObject(storeAssign);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PostAsync("api/StoreAdmin/AssignStoreAsync/", httpContent);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiResult> UnassignStoreAsync(WebApiStoreAssign storeUnassign)
        {
            AddTokenToHeader();

            var jsonContent = JsonConvert.SerializeObject(storeUnassign);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PostAsync("api/StoreAdmin/UnassignStoreAsync/", httpContent);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }
    }
}