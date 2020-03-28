using DiscountCatalog.Common.CreateModels;
using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.MVC.Models;
using DiscountCatalog.MVC.Models.Paging;
using DiscountCatalog.MVC.REST.Manager;
using DiscountCatalog.MVC.REST.Store;
using DiscountCatalog.MVC.ViewModels;
using DiscountCatalog.MVC.ViewModels.Manager;
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

        public async Task<Result> CreateManager(ManagerRESTPost manager)
        {
            AddTokenToHeader();

            manager.StoreAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.PostAsJsonAsync($"api/StoreAdmin/CreateManager", manager);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<PagingEntity<ManagerREST>> GetAllManagers(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/GetAllManagers/{storeAdminId}?sortOrder={sortOrder}&searchString={searchString}&pageIndex={pageIndex}&pageSize={pageSize}");

            var result = await request.Content.ReadAsAsync<PagingEntity<ManagerREST>>();

            return result;
        }

        public async Task<PagingEntity<ManagerREST>> GetAllDeletedManagers(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/GetAllDeletedManagers/{storeAdminId}?sortOrder={sortOrder}&searchString={searchString}&pageIndex={pageIndex}&pageSize={pageSize}");

            var result = await request.Content.ReadAsAsync<PagingEntity<ManagerREST>>();

            return result;
        }

        public async Task<ManagerREST> GetManager(string id)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/GetManager/{storeAdminId}?managerId={id}");

            var result = await request.Content.ReadAsAsync<ManagerREST>();

            return result;
        }

        public async Task<Result> EditManager(ManagerRESTPut manager)
        {
            AddTokenToHeader();

            var request = await apiClient.PutAsJsonAsync($"api/StoreAdmin/EditManager/{manager.Id}", manager);

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

        #region Store

        public async Task<Result> CreateStore(StoreRESTPost store)
        {
            AddTokenToHeader();

            store.StoreAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.PostAsJsonAsync("api/StoreAdmin/CreateStore", store);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<PagingEntity<StoreREST>> GetAllStores(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/GetAllStores/{storeAdminId}?sortOrder={sortOrder}&searchString={searchString}&pageIndex={pageIndex}&pageSize={pageSize}");

            var result = await request.Content.ReadAsAsync<PagingEntity<StoreREST>>();

            return result;
        }

        public async Task<PagingEntity<StoreREST>> GetAllDeletedStores(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/GetAllDeletedStores/{storeAdminId}?sortOrder={sortOrder}&searchString={searchString}&pageIndex={pageIndex}&pageSize={pageSize}");

            var result = await request.Content.ReadAsAsync<PagingEntity<StoreREST>>();

            return result;
        }

        public async Task<StoreREST> GetStore(string id)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/GetStore/{storeAdminId}?storeId={id}");

            var result = await request.Content.ReadAsAsync<StoreREST>();

            return result;
        }

        public async Task<Result> EditStore(StoreRESTPut store)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.PutAsJsonAsync($"api/StoreAdmin/EditStore/{storeAdminId}", store);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> DeleteStore(string id)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/DeleteStore/{storeAdminId}?storeId={id}");

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> RestoreStore(string id)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/RestoreStore/{storeAdminId}?storeId={id}");

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> PostStoreImage(string id, byte[] image)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.PutAsJsonAsync($"api/StoreAdmin/PostStoreImage/{storeAdminId}?storeId={id}", image);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<byte[]> GetStoreImage(string id)
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/StoreAdmin/GetStoreImage/{storeAdminId}?storeId={id}");

            var result = await request.Content.ReadAsAsync<byte[]>();

            return result;
        } 


        #endregion

    }
}