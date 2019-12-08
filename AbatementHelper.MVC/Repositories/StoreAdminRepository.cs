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
        public bool RegisterSuccessful;
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

        public Response CreateStore(WebApiStore store)
        {
            AddTokenToHeader();

            store.StoreAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var jsonContent = JsonConvert.SerializeObject(store);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = apiClient.PostAsync("api/StoreAdmin/CreateStore", httpContent);
            response.Wait();

            var result = response.Result;

            var resultContent = result.Content.ReadAsAsync<Response>();

            responseModel.ResponseMessage = resultContent.Result.ResponseMessage;
            responseModel.Success = resultContent.Result.Success;

            return responseModel;
        }

        public async Task<List<WebApiStore>> GetAllStores()
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            List<WebApiStore> stores = new List<WebApiStore>();

            var response = apiClient.GetAsync("api/StoreAdmin/GetAllStores/" + storeAdminId);
            response.Wait();

            var result = response.Result;

            var resultString = response.Result.ToString();


            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<List<WebApiStore>>();
                resultContent.Wait();

                stores = resultContent.Result;
            }

            return stores;

        }

        public WebApiStore EditStore(string id)
        {
            WebApiStore store = new WebApiStore();

            AddTokenToHeader();

            var response = apiClient.GetAsync("api/StoreAdmin/EditStore/" + id);
            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<WebApiStore>();
                resultContent.Wait();

                store = resultContent.Result;
            }

            return store;
        }

        public Response EditStore(WebApiStore store)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsJsonAsync("api/StoreAdmin/EditStore", store);
            response.Wait();

            var result = response.Result;

            var resultContent = result.Content.ReadAsAsync<Response>();

            responseModel.ResponseMessage = resultContent.Result.ResponseMessage;
            responseModel.Success = resultContent.Result.Success;

            return responseModel;
        }

        public WebApiStore DetailsStore(string id)
        {
            WebApiStore store = new WebApiStore();

            if (id != null)
            {
                AddTokenToHeader();

                var response = apiClient.GetAsync("api/StoreAdmin/DetailsStore/" + id);
                response.Wait();

                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var resultContent = result.Content.ReadAsAsync<WebApiStore>();
                    resultContent.Wait();

                    store = resultContent.Result;
                } 
            }

            return store;
        }

        public bool DeleteStore(string id)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsync("api/StoreAdmin/DeleteStore/" + id, null);
            response.Wait();

            var result = response.Result;


            var resultString = result.ToString();


            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public bool RestoreStore(string id)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsync("api/StoreAdmin/RestoreStore/" + id, null);
            response.Wait();

            var result = response.Result;


            var resultString = result.ToString();


            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<List<WebApiStore>> GetAllDeletedStores()
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            List<WebApiStore> stores = new List<WebApiStore>();

            var response = apiClient.GetAsync("api/StoreAdmin/GetAllDeletedStores/" + storeAdminId);
            response.Wait();

            var result = response.Result;

            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<List<WebApiStore>>();
                resultContent.Wait();

                stores = resultContent.Result;
            }

            return stores;

        }

        public SelectedStore Select(string id)
        {
            AddTokenToHeader();

            SelectedStore store = new SelectedStore();

            var response = apiClient.GetAsync("api/StoreAdmin/Select/" + id);
            response.Wait();

            var result = response.Result;

            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<SelectedStore>();
                resultContent.Wait();

                store = resultContent.Result;
            }

            return store;
        }
    }
}