using AbatementHelper.CommonModels.Models;
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

        public async Task<string> RegisterStore(Store store)
        {
            //
            AddTokenToHeader();

            var jsonContent = JsonConvert.SerializeObject(store);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            response = await apiClient.PostAsync("api/Account/RegisterStore", httpContent);

            RegisterSuccessful = response.IsSuccessStatusCode;

            var result = response.Content.ReadAsStringAsync();

            return await result;
        }

        //public DataBaseResultListOfStores GetAllStores(string masterStoreID)
        //{
        //    DataBaseResultListOfStores stores = new DataBaseResultListOfStores();

        //    AddTokenToHeader();

        //    var response = apiClient.GetAsync("api/StoreAdmin/GetAllStores/"+masterStoreID);
        //    response.Wait();

        //    var result = response.Result;

        //    var resultString = response.Result.ToString();

        //    if (result.IsSuccessStatusCode)
        //    {
        //        var resultContent = result.Content.ReadAsAsync<DataBaseResultListOfStores>();
        //        resultContent.Wait();

        //        stores = resultContent.Result;
        //    }

        //    return stores;

        //}

        //public DataBaseResultListOfStores GetAllDeletedStores(string masterStoreID)
        //{
        //    DataBaseResultListOfStores stores = new DataBaseResultListOfStores();

        //    AddTokenToHeader();

        //    var response = apiClient.GetAsync("api/StoreAdmin/GetAllDeletedStores/" + masterStoreID);
        //    response.Wait();

        //    var result = response.Result;

        //    var resultString = response.Result.ToString();

        //    if (result.IsSuccessStatusCode)
        //    {
        //        var resultContent = result.Content.ReadAsAsync<DataBaseResultListOfStores>();
        //        resultContent.Wait();

        //        stores = resultContent.Result;
        //    }

        //    return stores;

        //}

        //public DataBaseStore Edit(string id)
        //{
        //    DataBaseStore store = new DataBaseStore();

        //    AddTokenToHeader();

        //    var response = apiClient.GetAsync("api/StoreAdmin/Edit/" + id);
        //    response.Wait();

        //    var result = response.Result;
        //    if (result.IsSuccessStatusCode)
        //    {
        //        var resultContent = result.Content.ReadAsAsync<DataBaseStore>();
        //        resultContent.Wait();

        //        store = resultContent.Result;
        //    }

        //    return store;
        //}

        //public bool Edit(DataBaseStore store)
        //{
        //    AddTokenToHeader();

        //    var response = apiClient.PutAsJsonAsync<DataBaseStore>("api/StoreAdmin/Edit", store);
        //    response.Wait();

        //    var result = response.Result;
        //    if (result.IsSuccessStatusCode)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public bool DeleteStore(string id)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsync("api/StoreAdmin/Delete/" + id, null);
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

            var response = apiClient.PutAsync("api/StoreAdmin/Restore/" + id, null);
            response.Wait();

            var result = response.Result;


            var resultString = result.ToString();


            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}