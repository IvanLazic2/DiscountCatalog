using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.MVC.Models;
using DiscountCatalog.MVC.Models.Paging;
using DiscountCatalog.MVC.REST.Store;
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
    public class ManagerRepository : MVCRepository
    {
        public async Task<PagingEntity<StoreREST>> GetAllStores(string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            AddTokenToHeader();

            string managerIdentityId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/Manager/GetAllStores/{managerIdentityId}?&sortOrder={sortOrder}&searchString={searchString}&pageIndex={pageIndex}&pageSize={pageSize}");

            var result = await request.Content.ReadAsAsync<PagingEntity<StoreREST>>();

            return result;
        }

        public async Task<StoreREST> GetStore(string id)
        {
            AddTokenToHeader();

            string managerIdentityId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.GetAsync($"api/Manager/GetStore/{managerIdentityId}?storeId={id}");

            var result = await request.Content.ReadAsAsync<StoreREST>();

            return result;
        }

        public async Task<Result> EditStore(StoreRESTPut store)
        {
            AddTokenToHeader();

            string managerIdentityId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.PutAsJsonAsync($"api/Manager/EditStore/{managerIdentityId}", store);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> PostStoreImage(string id, byte[] image)
        {
            AddTokenToHeader();

            string managerIdentityId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var request = await apiClient.PutAsJsonAsync($"api/Manager/PostStoreImage/{managerIdentityId}?storeId={id}", image);

            var result = await request.Content.ReadAsAsync<Result>();

            return result;
        }
    }
}