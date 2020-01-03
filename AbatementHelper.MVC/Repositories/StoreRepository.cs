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
    public class StoreRepository
    {
        private HttpClient apiClient;

        public StoreRepository()
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

        public async Task<Response> CreateProductAsync(WebApiProduct product)
        {
            AddTokenToHeader();

            product.Store = new WebApiStore
            {
                Id = HttpContext.Current.Request.Cookies["StoreID"].Value
            };

            if (product.Store.Id != null)
            {
                var jsonContent = JsonConvert.SerializeObject(product);

                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage request = await apiClient.PostAsync("api/Store/CreateProductAsync", httpContent);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<Response>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<List<WebApiProduct>> GetAllProductsAsync()
        {
            AddTokenToHeader();

            string storeId = HttpContext.Current.Request.Cookies["StoreID"].Value;

            if (storeId != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/Store/GetAllProductsAsync/" + storeId);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<List<WebApiProduct>>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<WebApiProduct> ProductDetailsAsync(string id)
        {
            if (id != null)
            {
                AddTokenToHeader();

                HttpResponseMessage request = await apiClient.GetAsync("api/Store/ProductDetailsAsync/" + id);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<WebApiProduct>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<Response> PostProductImageAsync(WebApiPostImage image)
        {
            if (image.Image != null && image.Id != null)
            {
                AddTokenToHeader();

                HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Store/PostProductImageAsync", image);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<Response>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<byte[]> GetProductImageAsync(string id)
        {
            AddTokenToHeader();

            if (id != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/Store/GetProductImageAsync/" + id);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<byte[]>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<Response> EditProductAsync(WebApiProduct product)
        {
            AddTokenToHeader();

            if (product.Id != null)
            {
                HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Store/EditProductAsync", product);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<Response>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            AddTokenToHeader();

            if (id != null)
            {
                HttpResponseMessage request = await apiClient.PutAsync("api/Store/DeleteProductAsync/" + id, null);

                if (request.IsSuccessStatusCode)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> RestoreProductAsync(string id)
        {
            AddTokenToHeader();

            if (id != null)
            {
                HttpResponseMessage request = await apiClient.PutAsync("api/Store/RestoreProductAsync/" + id, null);

                if (request.IsSuccessStatusCode)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<List<WebApiProduct>> GetAllDeletedProductsAsync()
        {
            AddTokenToHeader();

            string storeId = HttpContext.Current.Request.Cookies["StoreID"].Value;

            if (storeId != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/Store/GetAllDeletedProductsAsync/" + storeId);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<List<WebApiProduct>>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<List<WebApiProduct>> GetAllExpiredProductsAsync()
        {
            AddTokenToHeader();

            string storeId = HttpContext.Current.Request.Cookies["StoreID"].Value;

            if (storeId != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/Store/GetAllExpiredProductsAsync/" + storeId);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<List<WebApiProduct>>();

                    return resultContent;
                }
            }

            return null;
        }
    }
}