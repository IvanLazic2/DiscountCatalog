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

        private void AddTokenToHeader()
        {
            var token = HttpContext.Current.Request.Cookies["Access_Token"];

            if (token != null)
            {
                apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value.ToString());
            }
        }

        //private async Task<WebApiResult> ReadResponseAsync(HttpResponseMessage request)
        //{
        //    var modelState = new WebApiResult();

        //    if (!request.IsSuccessStatusCode)
        //    {
        //        var resultContent = await request.Content.ReadAsStringAsync();

        //        var obj = new { message = "", ModelState = new Dictionary<string, string[]>() };
        //        var modelStateResult = JsonConvert.DeserializeAnonymousType(resultContent, obj);

        //        if (modelStateResult.ModelState != null)
        //        {
        //            foreach (var error in modelStateResult.ModelState)
        //            {
        //                modelState.ModelState.Add(error.Key, error.Value.First());
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var successMessage = await request.Content.ReadAsAsync<string>();

        //        modelState.Message = successMessage;
        //    }

        //    return modelState;
        //}

        public async Task<WebApiResult> CreateProductAsync(WebApiProduct product)
        {
            AddTokenToHeader();

            product.Store = new WebApiStore
            {
                Id = HttpContext.Current.Request.Cookies["StoreID"].Value,
                StoreName = HttpContext.Current.Request.Cookies["StoreName"].Value
            };

            if (product.Store.Id != null)
            {
                var jsonContent = JsonConvert.SerializeObject(product);

                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage request = await apiClient.PostAsync("api/Store/CreateProductAsync", httpContent);

                WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

                return result;
            }

            return null;
        }

        public async Task<WebApiListOfProductsResult> GetAllProductsAsync()
        {
            AddTokenToHeader();

            string storeId = HttpContext.Current.Request.Cookies["StoreID"].Value;

            if (storeId != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/Store/GetAllProductsAsync/" + storeId);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<WebApiListOfProductsResult>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<WebApiProductResult> ProductDetailsAsync(string id)
        {
            if (id != null)
            {
                AddTokenToHeader();

                HttpResponseMessage request = await apiClient.GetAsync("api/Store/ProductDetailsAsync/" + id);

                if (request.IsSuccessStatusCode)
                {
                    WebApiProductResult resultContent = await request.Content.ReadAsAsync<WebApiProductResult>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<WebApiResult> PostProductImageAsync(WebApiPostImage image)
        {
            if (image.Image != null && image.Id != null)
            {
                AddTokenToHeader();

                HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Store/PostProductImageAsync", image);

                if (request.IsSuccessStatusCode)
                {
                    WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

                    return result;
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

        public async Task<WebApiProductResult> EditProductAsync(WebApiProduct product)
        {
            AddTokenToHeader();

            if (product.Id != null)
            {
                product.Store = new WebApiStore
                {
                    Id = HttpContext.Current.Request.Cookies["StoreID"].Value
                };

                if (product.Store.Id != null)
                {
                    HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Store/EditProductAsync", product);

                    WebApiProductResult result = await request.Content.ReadAsAsync<WebApiProductResult>();

                    return result;
                }
            }

            return null;
        }

        public async Task<WebApiResult> DeleteProductAsync(string id)
        {
            AddTokenToHeader();

            if (id != null)
            {
                HttpResponseMessage request = await apiClient.PutAsync("api/Store/DeleteProductAsync/" + id, null);


                WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

                return result;

            }

            return null;
        }

        public async Task<WebApiResult> RestoreProductAsync(string id)
        {
            AddTokenToHeader();

            if (id != null)
            {
                HttpResponseMessage request = await apiClient.PutAsync("api/Store/RestoreProductAsync/" + id, null);

                WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

                return result;
            }

            return null;
        }

        public async Task<WebApiListOfProductsResult> GetAllDeletedProductsAsync()
        {
            AddTokenToHeader();

            string storeId = HttpContext.Current.Request.Cookies["StoreID"].Value;

            if (storeId != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/Store/GetAllDeletedProductsAsync/" + storeId);

                WebApiListOfProductsResult result = await request.Content.ReadAsAsync<WebApiListOfProductsResult>();

                return result;
            }

            return null;
        }

        public async Task<WebApiListOfProductsResult> GetAllExpiredProductsAsync()
        {
            AddTokenToHeader();

            string storeId = HttpContext.Current.Request.Cookies["StoreID"].Value;

            if (storeId != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/Store/GetAllExpiredProductsAsync/" + storeId);

                if (request.IsSuccessStatusCode)
                {
                    WebApiListOfProductsResult result = await request.Content.ReadAsAsync<WebApiListOfProductsResult>();

                    return result;
                }
            }

            return null;
        }
    }
}