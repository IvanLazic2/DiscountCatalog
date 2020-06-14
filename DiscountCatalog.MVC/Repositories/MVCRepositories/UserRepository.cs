using DiscountCatalog.MVC.Models.Paging;
using DiscountCatalog.MVC.REST.Product;
using DiscountCatalog.MVC.REST.Store;
using DiscountCatalog.MVC.REST.StoreAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace DiscountCatalog.MVC.Repositories.MVCRepositories
{
    public class UserRepository : MVCRepository
    {
        public async Task<PagingEntity<ProductREST>> GetAllProducts(string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter, bool includeUpcoming)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync($"api/User/GetAllProducts?sortOrder={sortOrder}&searchString={searchString}&pageIndex={pageIndex}&pageSize={pageSize}&priceFilter={priceFilter}&dateFilter={dateFilter}&includeUpcoming={includeUpcoming}");

            var result = await request.Content.ReadAsAsync<PagingEntity<ProductREST>>();

            return result;
        }

        public async Task<ProductREST> GetProduct(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync($"api/User/GetProduct/{id}");

            var result = await request.Content.ReadAsAsync<ProductREST>();

            return result;
        }

        public async Task<StoreREST> GetStore(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync($"api/User/GetStore/{id}");

            var result = await request.Content.ReadAsAsync<StoreREST>();

            return result;
        }

        public async Task<PagingEntity<ProductREST>> GetStoreProducts(string id, string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter, bool includeUpcoming)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync($"api/User/GetStoreProducts/{id}?sortOrder={sortOrder}&searchString={searchString}&pageIndex={pageIndex}&pageSize={pageSize}&priceFilter={priceFilter}&dateFilter={dateFilter}&includeUpcoming={includeUpcoming}");

            var result = await request.Content.ReadAsAsync<PagingEntity<ProductREST>>();

            return result;
        }

        public async Task<StoreAdminREST> GetStoreAdmin(string id)
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync($"api/User/GetStoreAdmin/{id}");

            var result = await request.Content.ReadAsAsync<StoreAdminREST>();

            return result;
        }

        public async Task<decimal> GetMinPrice()
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync($"api/User/GetMinPrice");

            var result = await request.Content.ReadAsAsync<decimal>();

            return Math.Floor(result);
        }

        public async Task<decimal> GetMaxPrice()
        {
            AddTokenToHeader();

            var request = await apiClient.GetAsync($"api/User/GetMaxPrice");

            var result = await request.Content.ReadAsAsync<decimal>();

            return result;
        }
    }
}