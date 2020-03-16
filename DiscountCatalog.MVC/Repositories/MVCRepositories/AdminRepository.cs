using DiscountCatalog.Common.CreateModels;
using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.MVC.Models.Paging;
using DiscountCatalog.MVC.Paging;
using DiscountCatalog.MVC.Repositories;
using DiscountCatalog.MVC.ViewModels;
using Newtonsoft.Json;
using PagedList;
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

namespace DiscountCatalog.MVC.Repositories
{
    public class AdminRepository : MVCRepository
    {
        #region Create

        //CREATE

        public async Task<Result> CreateUser(UserViewModel user)
        {
            AddTokenToHeader();

            var request = await apiClient.PostAsJsonAsync("api/Admin/CreateUser", user);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> CreateStoreAdmin(UserViewModel storeAdmin)
        {
            AddTokenToHeader();

            var request = await apiClient.PostAsJsonAsync("api/Admin/CreateStoreAdmin", storeAdmin);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> CreateManager(ManagerViewModel manager)
        {
            AddTokenToHeader();

            var request = await apiClient.PostAsJsonAsync("api/Admin/CreateManager", manager);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> CreateStore(StoreViewModel store)
        {
            AddTokenToHeader();

            var request = await apiClient.PostAsJsonAsync("api/Admin/CreateStore", store);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }
        #endregion

        #region Read

        //READ

        public async Task<PagingEntity<User>> GetAllUsers(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync($"api/Admin/GetAllUsers?sortOrder={sortOrder}&searchString={searchString}&pageIndex={pageIndex}&pageSize={pageSize}");

            var result = await request.Content.ReadAsAsync<PagingEntity<User>>();

            return result;
        }

        public async Task<PagingEntity<User>> GetAllDeletedUsers(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync($"api/Admin/GetAllDeletedUsers?sortOrder={sortOrder}&searchString={searchString}&pageIndex={pageIndex}&pageSize={pageSize}");

            var result = await request.Content.ReadAsAsync<PagingEntity<User>>();

            return result;
        }

        public async Task<User> GetUser(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/GetUser/" + id);

            var result = await request.Content.ReadAsAsync<User>();

            return result;
        }


        public async Task<PagingEntity<StoreAdmin>> GetAllStoreAdmins(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync($"api/Admin/GetAllStoreAdmins?sortOrder={sortOrder}&searchString={searchString}&pageIndex={pageIndex}&pageSize={pageSize}");

            var result = await request.Content.ReadAsAsync<PagingEntity<StoreAdmin>>();

            return result;
        }

        public async Task<List<StoreAdmin>> GetAllDeletedStoreAdmins() //temp
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/GetAllDeletedStoreAdmins/");

            var result = await request.Content.ReadAsAsync<List<StoreAdmin>>();

            return result;
        }

        public async Task<StoreAdmin> GetStoreAdmin(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/GetStoreAdmin/" + id);

            var result = await request.Content.ReadAsAsync<StoreAdmin>();

            return result;
        }


        public async Task<PagingEntity<Manager>> GetAllManagers(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync($"api/Admin/GetAllManagers?sortOrder={sortOrder}&searchString={searchString}&pageIndex={pageIndex}&pageSize={pageSize}");

            var result = await request.Content.ReadAsAsync<PagingEntity<Manager>>();

            return result;
        }

        public async Task<List<Manager>> GetAllDeletedManagers() //temp
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/GetAllDeletedManagers/");

            var result = await request.Content.ReadAsAsync<List<Manager>>();

            return result;
        }

        public async Task<Manager> GetManager(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/GetManager/" + id);

            var result = await request.Content.ReadAsAsync<Manager>();

            return result;
        }


        public async Task<List<Store>> GetAllStores()
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/GetAllStores/");

            var result = await request.Content.ReadAsAsync<List<Store>>();

            return result;
        }

        public async Task<List<Store>> GetAllDeletedStores()
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/GetAllDeletedStores/");

            var result = await request.Content.ReadAsAsync<List<Store>>();

            return result;
        }

        public async Task<Store> GetStore(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/GetStore/" + id);

            var result = await request.Content.ReadAsAsync<Store>();

            return result;
        }
        #endregion

        #region Update

        //UPDATE

        public async Task<Result> EditUser(User user)
        {
            AddTokenToHeader();

            var request = await apiClient.PutAsJsonAsync("api/Admin/EditUser/" + user.Id, user);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> EditStoreAdmin(StoreAdmin storeAdmin)
        {
            AddTokenToHeader();

            var request = await apiClient.PutAsJsonAsync("api/Admin/EditStoreAdmin/" + storeAdmin.Identity.Id, storeAdmin);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> EditManager(Manager manager)
        {
            AddTokenToHeader();

            var request = await apiClient.PutAsJsonAsync("api/Admin/EditManager", manager);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> EditStore(Store store)
        {
            AddTokenToHeader();

            var request = await apiClient.PutAsJsonAsync("api/Admin/EditStore", store);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }
        #endregion

        #region Delete

        //DELETE

        public async Task<Result> DeleteUser(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/DeleteUser/" + id);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> DeleteStoreAdmin(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/DeleteStoreAdmin/" + id);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> DeleteManager(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/DeleteManager/" + id);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> DeleteStore(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/DeleteStore/" + id);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        #endregion

        #region Restore

        //RESTORE

        public async Task<Result> RestoreUser(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/RestoreUser/" + id);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> RestoreStoreAdmin(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/RestoreStoreAdmin/" + id);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> RestoreManager(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/RestoreManager/" + id);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> RestoreStore(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync("api/Admin/RestoreStore/" + id);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }
        #endregion

        
        ~AdminRepository()
        {
            apiClient.Dispose();
        }
    }
}