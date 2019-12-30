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
using System.Web;

namespace AbatementHelper.MVC.Repositories
{
    public class StoreRepository
    {
        private HttpClient apiClient;
        private Response responseModel = new Response();

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

        public Response CreateProduct(WebApiProduct product)
        {
            AddTokenToHeader();

            product.Store = new WebApiStore
            {
                Id = HttpContext.Current.Request.Cookies["StoreID"].Value
            };

            var jsonContent = JsonConvert.SerializeObject(product);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = apiClient.PostAsync("api/Store/CreateProduct", httpContent);
            response.Wait();

            var result = response.Result;

            var resultContent = result.Content.ReadAsAsync<Response>();

            responseModel.ResponseMessage = resultContent.Result.ResponseMessage;
            responseModel.Success = resultContent.Result.Success;

            return responseModel;
        }

        public List<WebApiProduct> GetAllProducts()
        {
            AddTokenToHeader();

            string storeId = HttpContext.Current.Request.Cookies["StoreID"].Value;

            List<WebApiProduct> products = new List<WebApiProduct>();

            var response = apiClient.GetAsync("api/Store/GetAllProducts/" + storeId);
            response.Wait();

            var result = response.Result;

            var resultString = response.Result.ToString();


            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<List<WebApiProduct>>();
                resultContent.Wait();

                products = resultContent.Result;
            }

            return products;

        }

        public WebApiProduct ProductDetails(string id)
        {
            WebApiProduct product = new WebApiProduct();

            if (id != null)
            {
                AddTokenToHeader();

                var response = apiClient.GetAsync("api/Store/ProductDetails/" + id);
                response.Wait();

                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var resultContent = result.Content.ReadAsAsync<WebApiProduct>();
                    resultContent.Wait();

                    product = resultContent.Result;
                }
            }

            return product;
        }

        public Response PostProductImage(byte[] array)
        {
            WebApiProduct product = new WebApiProduct();


            AddTokenToHeader();

            product.Id = HttpContext.Current.Request.Cookies["ProductID"].Value;
            product.ProductImage = array;

            var response = apiClient.PutAsJsonAsync("api/Store/PostProductImage", product);
            response.Wait();

            var result = response.Result;

            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<Response>();
                resultContent.Wait();

                responseModel = resultContent.Result;
            }


            return responseModel;
        }

        public byte[] GetProductImage(string id)
        {
            AddTokenToHeader();

            var response = apiClient.GetAsync("api/Store/GetProductImage/" + id);
            response.Wait();

            var result = response.Result;

            var resultContent = result.Content.ReadAsAsync<byte[]>();
            //resultContent.Wait();

            return resultContent.Result;
        }

        public Response EditProduct(WebApiProduct product)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsJsonAsync("api/Store/EditProduct", product);
            response.Wait();

            var result = response.Result;

            var resultContent = result.Content.ReadAsAsync<Response>();

            responseModel.ResponseMessage = resultContent.Result.ResponseMessage;
            responseModel.Success = resultContent.Result.Success;

            return responseModel;
        }

        public bool DeleteProduct(string id)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsync("api/Store/DeleteProduct/" + id, null);
            response.Wait();

            var result = response.Result;


            var resultString = result.ToString();


            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public bool RestoreProduct(string id)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsync("api/Store/RestoreProduct/" + id, null);
            response.Wait();

            var result = response.Result;


            var resultString = result.ToString();


            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public List<WebApiProduct> GetAllDeletedProducts()
        {
            AddTokenToHeader();

            string storeId = HttpContext.Current.Request.Cookies["StoreID"].Value;

            List<WebApiProduct> stores = new List<WebApiProduct>();

            var response = apiClient.GetAsync("api/Store/GetAllDeletedProducts/" + storeId);
            response.Wait();

            var result = response.Result;

            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<List<WebApiProduct>>();
                resultContent.Wait();

                stores = resultContent.Result;
            }

            return stores;

        }

        public List<WebApiProduct> GetAllExpiredProducts()
        {
            AddTokenToHeader();

            string storeId = HttpContext.Current.Request.Cookies["StoreID"].Value;

            List<WebApiProduct> stores = new List<WebApiProduct>();

            var response = apiClient.GetAsync("api/Store/GetAllExpiredProducts/" + storeId);
            response.Wait();

            var result = response.Result;

            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<List<WebApiProduct>>();
                resultContent.Wait();

                stores = resultContent.Result;
            }

            return stores;

        }
    }
}