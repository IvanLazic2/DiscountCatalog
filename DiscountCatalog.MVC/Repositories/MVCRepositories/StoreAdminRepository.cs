using DiscountCatalog.Common.CreateModels;
using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.MVC.Models;
using DiscountCatalog.MVC.Models.Paging;
using DiscountCatalog.MVC.ViewModels;
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

namespace DiscountCatalog.MVC.Repositories
{
    public class StoreAdminRepository : MVCRepository
    {
        #region Manager

        public async Task<Result> CreateManager(ManagerViewModel manager)
        {
            AddTokenToHeader();

            manager.StoreAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.PostAsJsonAsync("api/StoreAdmin/CreateManager", manager);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<PagingEntity<Manager>> GetAllManagers(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/Admin/GetAllManagers/{id}?sortOrder={sortOrder}&searchString={searchString}&pageIndex={pageIndex}&pageSize={pageSize}");

            var result = await request.Content.ReadAsAsync<PagingEntity<Manager>>();

            return result;
        }

        public async Task<PagingEntity<Manager>> GetAllDeletedManagers(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/Admin/GetAllDeleted" +
                $"Managers/{id}?sortOrder={sortOrder}&searchString={searchString}&pageIndex={pageIndex}&pageSize={pageSize}");

            var result = await request.Content.ReadAsAsync<PagingEntity<Manager>>();

            return result;
        }

        public async Task<Manager> GetManager(string id)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/Admin/GetManager/{storeAdminId}?managerId={id}");

            var result = await request.Content.ReadAsAsync<Manager>();

            return result;
        }

        public async Task<Result> EditManager(Manager manager)
        {
            AddTokenToHeader();

            var request = await apiClient.PutAsJsonAsync("api/StoreAdmin/EditManager", manager);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> DeleteManager(string id)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/DeleteManager/{storeAdminId}?managerId={id}");

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> RestoreManager(string id)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/RestoreManager/{storeAdminId}?managerId={id}");

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> PostManagerImage(string id, byte[] image)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.PutAsJsonAsync($"api/StoreAdmin/PostManagerImage/{storeAdminId}?managerId={id}", image);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<byte[]> GetManagerImage(string id)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/GetManagerImage/{storeAdminId}?managerId={id}");

            var result = await request.Content.ReadAsAsync<byte[]>();

            return result;
        }

        public async Task<PagingEntity<ManagerStore>> GetManagerStores(string id, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/GetManagerStores/{storeAdminId}?managerId={id}&sortOrder={sortOrder}&searchString={searchString}&pageIndex={pageIndex}&pageSize={pageSize}");

            var result = await request.Content.ReadAsAsync<PagingEntity<ManagerStore>>();

            return result;
        }

        public async Task<Result> Assign(string managerId, string storeId)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/Assign/{storeAdminId}?managerId={managerId}&storeId={storeId}");

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> Unassign(string managerId, string storeId)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/Unassign/{storeAdminId}?managerId={managerId}&storeId={storeId}");

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        #endregion

        //        //Store

        //        public async Task<WebApiResult> CreateStoreAsync(WebApiStore store)
        //        {
        //            store.StoreAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

        //            AddTokenToHeader();

        //            var jsonContent = JsonConvert.SerializeObject(store);

        //            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //            HttpResponseMessage request = await apiClient.PostAsync("api/StoreAdmin/CreateStoreAsync", httpContent);

        //            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //            return result;
        //        }

        //        public async Task<WebApiListOfStoresResult> GetAllStoresAsync()
        //        {
        //            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllStoresAsync/" + storeAdminId);

        //            WebApiListOfStoresResult result = await request.Content.ReadAsAsync<WebApiListOfStoresResult>();

        //            return result;
        //        }

        //        public async Task<WebApiStoreResult> EditStoreAsync(string id)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/EditStoreAsync/" + id);

        //            WebApiStoreResult result = await request.Content.ReadAsAsync<WebApiStoreResult>();

        //            return result;
        //        }

        //        public async Task<WebApiResult> EditStoreAsync(WebApiStore store)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/StoreAdmin/EditStoreAsync", store);

        //            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //            return result;
        //        }

        //        public async Task<WebApiResult> PostStoreImageAsync(WebApiPostImage image)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/StoreAdmin/PostStoreImageAsync", image);

        //            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //            return result;
        //        }

        //        public async Task<byte[]> GetStoreImageAsync(string id)
        //        {
        //            if (id != null)
        //            {
        //                AddTokenToHeader();

        //                HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetStoreImageAsync/" + id);

        //                if (request.IsSuccessStatusCode)
        //                {
        //                    var resultContent = await request.Content.ReadAsAsync<byte[]>();

        //                    return resultContent;
        //                }
        //            }

        //            return null;
        //        }

        //        public async Task<WebApiStoreResult> DetailsStoreAsync(string id)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/DetailsStoreAsync/" + id);

        //            WebApiStoreResult result = await request.Content.ReadAsAsync<WebApiStoreResult>();

        //            return result;
        //        }

        //        public async Task<WebApiResult> DeleteStoreAsync(string id)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.PutAsync("api/StoreAdmin/DeleteStoreAsync/" + id, null);

        //            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //            return result;
        //        }

        //        public async Task<WebApiResult> RestoreStoreAsync(string id)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.PutAsync("api/StoreAdmin/RestoreStoreAsync/" + id, null);

        //            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //            return result;
        //        }

        //        public async Task<WebApiListOfStoresResult> GetAllDeletedStoresAsync()
        //        {
        //            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllDeletedStoresAsync/" + storeAdminId);

        //            WebApiListOfStoresResult result = await request.Content.ReadAsAsync<WebApiListOfStoresResult>();

        //            return result;
        //        }

        //        public async Task<WebApiSelectedStoreResult> SelectAsync(string id)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/SelectAsync/" + id);

        //            WebApiSelectedStoreResult result = await request.Content.ReadAsAsync<WebApiSelectedStoreResult>();

        //            return result;
        //        }

        //        //Manager

        //        public async Task<WebApiListOfManagersResult> GetAllManagersAsync()
        //        {
        //            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllManagersAsync/" + storeAdminId);

        //            WebApiListOfManagersResult result = await request.Content.ReadAsAsync<WebApiListOfManagersResult>();

        //            return result;
        //        }

        //        public async Task<WebApiResult> CreateManagerAsync(CreateManagerModel user)
        //        {
        //            user.StoreAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

        //            AddTokenToHeader();

        //            var jsonContent = JsonConvert.SerializeObject(user);

        //            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //            HttpResponseMessage request = await apiClient.PostAsync("api/StoreAdmin/CreateManagerAsync", httpContent);

        //            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //            return result;
        //        }

        //        public async Task<WebApiManagerResult> EditManagerAsync(string id)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/EditManagerAsync/" + id);

        //            WebApiManagerResult result = await request.Content.ReadAsAsync<WebApiManagerResult>();

        //            return result;
        //        }

        //        public async Task<WebApiResult> EditManagerAsync(WebApiManager user)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/StoreAdmin/EditManagerAsync", user);

        //            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //            return result;
        //        }

        //        public async Task<WebApiResult> PostManagerImageAsync(WebApiPostImage image)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/StoreAdmin/PostManagerImageAsync", image);

        //            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //            return result;
        //        }

        //        public async Task<byte[]> GetManagerImageAsync(string id)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetManagerImageAsync/" + id);

        //            if (request.IsSuccessStatusCode)
        //            {
        //                var resultContent = await request.Content.ReadAsAsync<byte[]>();

        //                return resultContent;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }

        //        public async Task<WebApiResult> DeleteManagerAsync(string id)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.PutAsync("api/StoreAdmin/DeleteManagerAsync/" + id, null);

        //            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //            return result;
        //        }

        //        public async Task<WebApiResult> RestoreManagerAsync(string id)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.PutAsync("api/StoreAdmin/RestoreManagerAsync/" + id, null);

        //            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //            return result;
        //        }

        //        public async Task<WebApiListOfManagersResult> GetAllDeletedManagers()
        //        {
        //            AddTokenToHeader();

        //            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

        //            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllDeletedManagersAsync/" + storeAdminId);

        //            WebApiListOfManagersResult result = await request.Content.ReadAsAsync<WebApiListOfManagersResult>();

        //            return result;
        //        }

        //        public async Task<WebApiManagerResult> DetailsManagerAsync(string id)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/DetailsManagerAsync/" + id);

        //            WebApiManagerResult result = await request.Content.ReadAsAsync<WebApiManagerResult>();

        //            return result;
        //        }

        //        public async Task<WebApiListOfManagerStoresResult> GetAllManagerStoresAsync(string id)
        //        {
        //            AddTokenToHeader();

        //            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllManagerStoresAsync/" + id);

        //            WebApiListOfManagerStoresResult result = await request.Content.ReadAsAsync<WebApiListOfManagerStoresResult>();

        //            return result;
        //        }

        //        //public List<WebApiStore> GetAllNotAssignedStores(string id)
        //        //{
        //        //    List<WebApiStore> stores = new List<WebApiStore>();

        //        //    if (id != null)
        //        //    {
        //        //        AddTokenToHeader();

        //        //        var response = apiClient.GetAsync("api/StoreAdmin/GetAllNotAssignedStores/" + id);
        //        //        response.Wait();

        //        //        var result = response.Result;
        //        //        if (result.IsSuccessStatusCode)
        //        //        {
        //        //            var resultContent = result.Content.ReadAsAsync<List<WebApiStore>>();
        //        //            resultContent.Wait();

        //        //            stores = resultContent.Result;
        //        //        }
        //        //    }

        //        //    return stores;
        //        //}

        //        public async Task<WebApiResult> AssignStoreAsync(WebApiStoreAssign storeAssign)
        //        {
        //            AddTokenToHeader();

        //            var jsonContent = JsonConvert.SerializeObject(storeAssign);

        //            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //            HttpResponseMessage request = await apiClient.PostAsync("api/StoreAdmin/AssignStoreAsync/", httpContent);

        //            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //            return result;
        //        }

        //        public async Task<WebApiResult> UnassignStoreAsync(WebApiStoreAssign storeUnassign)
        //        {
        //            AddTokenToHeader();

        //            var jsonContent = JsonConvert.SerializeObject(storeUnassign);

        //            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //            HttpResponseMessage request = await apiClient.PostAsync("api/StoreAdmin/UnassignStoreAsync/", httpContent);

        //            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //            return result;
        //        }
    }
}