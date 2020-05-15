using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace DiscountCatalog.MVC.Repositories.MVCRepositories
{
    public class ProductRepository : MVCRepository
    {
        public async Task<decimal> GetMinPrice(string storeId)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync($"api/Product/GetMinPrice/{storeId}");

            decimal result = await request.Content.ReadAsAsync<decimal>();

            return result;
        }

        public async Task<decimal> GetMaxPrice(string storeId)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync($"api/Product/GetMaxPrice/{storeId}");

            decimal result = await request.Content.ReadAsAsync<decimal>();

            return result;
        }

        public async Task<IEnumerable<string>> GetAllCurrencies()
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync($"api/Product/GetAllCurrencies");

            IEnumerable<string> result = await request.Content.ReadAsAsync<IEnumerable<string>>();

            return result;
        }

        public async Task<IEnumerable<string>> GetAllMeasuringUnits()
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync($"api/Product/GetAllMeasuringUnits");

            IEnumerable<string> result = await request.Content.ReadAsAsync<IEnumerable<string>>();

            return result;
        }
    }
}