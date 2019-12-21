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
    public class ManagerRepository
    {
        private HttpClient apiClient;
        private Response responseModel = new Response();

        public ManagerRepository()
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

        public List<WebApiStore> GetAllStores()
        {
            AddTokenToHeader();

            string managerId = HttpContext.Current.Request.Cookies["UserID"].Value;

            List<WebApiStore> stores = new List<WebApiStore>();

            var response = apiClient.GetAsync("api/Manager/GetAllStores/" + managerId);
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

        public SelectedStore Select(string id)
        {
            AddTokenToHeader();

            SelectedStore store = new SelectedStore();

            var response = apiClient.GetAsync("api/Manager/Select/" + id);
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

        public WebApiStore EditStore(string id)
        {
            WebApiStore store = new WebApiStore();

            AddTokenToHeader();

            var response = apiClient.GetAsync("api/Manager/EditStore/" + id);
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

            var response = apiClient.PutAsJsonAsync("api/Manager/EditStore", store);
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

                var response = apiClient.GetAsync("api/Manager/DetailsStore/" + id);
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

        public Response AbandonStore(string id)
        {
            Response response = new Response();

            WebApiStoreAssign storeUnassign = new WebApiStoreAssign();

            storeUnassign.ManagerId = HttpContext.Current.Request.Cookies["UserID"].Value;
            storeUnassign.StoreId = id;

            if (storeUnassign.ManagerId != null && storeUnassign.StoreId != null)
            {
                AddTokenToHeader();

                var jsonContent = JsonConvert.SerializeObject(storeUnassign);

                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var apiResponse = apiClient.PostAsync("api/Manager/AbandonStore/", httpContent);
                apiResponse.Wait();

                var result = apiResponse.Result;
                if (result.IsSuccessStatusCode)
                {
                    var resultContent = result.Content.ReadAsAsync<Response>();
                    resultContent.Wait();

                    response = resultContent.Result;
                }
            }

            return response;
        }
    }
}