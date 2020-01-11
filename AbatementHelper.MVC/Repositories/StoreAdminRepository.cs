using AbatementHelper.CommonModels.CreateModels;
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

        public async Task<ModelStateResponse> CreateStoreAsync(WebApiStore store)
        {
            AddTokenToHeader();

            var modelState = new ModelStateResponse();

            store.StoreAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            var jsonContent = JsonConvert.SerializeObject(store);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PostAsync("api/StoreAdmin/CreateStoreAsync", httpContent);

            var resultContent = await request.Content.ReadAsStringAsync();

            if (!request.IsSuccessStatusCode)
            {
                var obj = new { message = "", ModelState = new Dictionary<string, string[]>() };
                var modelStateResult = JsonConvert.DeserializeAnonymousType(resultContent, obj);

                modelState.Message = modelStateResult.message;

                if (modelStateResult.ModelState != null)
                {
                    modelState.ModelSate = modelStateResult.ModelState;
                }
            }

            return modelState;
        }

        public async Task<List<WebApiStore>> GetAllStoresAsync()
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllStoresAsync/" + storeAdminId);


            if (request.IsSuccessStatusCode)
            {
                var resultContent = await request.Content.ReadAsAsync<List<WebApiStore>>();

                return resultContent;
            }
            else
            {
                return null;
            }
        }

        public async Task<WebApiStore> EditStoreAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/EditStoreAsync/" + id);

            if (request.IsSuccessStatusCode)
            {
                var resultContent = await request.Content.ReadAsAsync<WebApiStore>();

                return resultContent;
            }
            else
            {
                return null;
            }
        }

        public async Task<ModelStateResponse> EditStoreAsync(WebApiStore store)
        {
            AddTokenToHeader();

            var modelState = new ModelStateResponse();

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/StoreAdmin/EditStoreAsync", store);

            var resultContent = await request.Content.ReadAsStringAsync();

            if (!request.IsSuccessStatusCode)
            {
                var obj = new { message = "", ModelState = new Dictionary<string, string[]>() };
                var modelStateResult = JsonConvert.DeserializeAnonymousType(resultContent, obj);

                modelState.Message = modelStateResult.message;

                if (modelStateResult.ModelState != null)
                {
                    modelState.ModelSate = modelStateResult.ModelState;
                }
            }

            return modelState;
        }

        public async Task<Response> PostStoreImageAsync(WebApiPostImage image)
        {
            if (image.Image != null && image.Id != null)
            {
                AddTokenToHeader();

                HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/StoreAdmin/PostStoreImageAsync", image);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<Response>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<byte[]> GetStoreImageAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetStoreImageAsync/" + id);

            if (request.IsSuccessStatusCode)
            {
                var resultContent = await request.Content.ReadAsAsync<byte[]>();

                return resultContent;
            }
            else
            {
                return null;
            }
        }

        public async Task<WebApiStore> DetailsStoreAsync(string id)
        {
            if (id != null)
            {
                AddTokenToHeader();

                HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/DetailsStoreAsync/" + id);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<WebApiStore>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<bool> DeleteStoreAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsync("api/StoreAdmin/DeleteStoreAsync/" + id, null);

            if (request.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RestoreStoreAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsync("api/StoreAdmin/RestoreStoreAsync/" + id, null);

            if (request.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<WebApiStore>> GetAllDeletedStoresAsync()
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            if (storeAdminId != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllDeletedStoresAsync/" + storeAdminId);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<List<WebApiStore>>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<SelectedStore> SelectAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/SelectAsync/" + id);

            if (request.IsSuccessStatusCode)
            {
                var resultContent = await request.Content.ReadAsAsync<SelectedStore>();

                return resultContent;
            }
            else
            {
                return null;
            }
        }

        //Manager

        public async Task<List<WebApiManager>> GetAllManagersAsync()
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            if (storeAdminId != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllManagersAsync/" + storeAdminId);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<List<WebApiManager>>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<ModelStateResponse> CreateManagerAsync(CreateManagerModel user)
        {
            AddTokenToHeader();

            var modelState = new ModelStateResponse();

            user.StoreAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            if (user.StoreAdminId != null)
            {
                var jsonContent = JsonConvert.SerializeObject(user);

                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage request = await apiClient.PostAsync("api/StoreAdmin/CreateManagerAsync", httpContent);

                var resultContent = await request.Content.ReadAsStringAsync();

                if (!request.IsSuccessStatusCode)
                {
                    var obj = new { message = "", ModelState = new Dictionary<string, string[]>() };
                    var modelStateResult = JsonConvert.DeserializeAnonymousType(resultContent, obj);

                    modelState.Message = modelStateResult.message;

                    if (modelStateResult.ModelState != null)
                    {
                        modelState.ModelSate = modelStateResult.ModelState;
                    }
                }
            }

            return modelState;
        }

        public async Task<WebApiManager> EditManagerAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/EditManagerAsync/" + id);

            if (request.IsSuccessStatusCode)
            {
                var resultContent = await request.Content.ReadAsAsync<WebApiManager>();

                return resultContent;
            }
            else
            {
                return null;
            }

        }

        public async Task<ModelStateResponse> EditManagerAsync(WebApiManager user)
        {
            AddTokenToHeader();

            var modelState = new ModelStateResponse();

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/StoreAdmin/EditManagerAsync", user);

            var resultContent = await request.Content.ReadAsStringAsync();

            if (!request.IsSuccessStatusCode)
            {
                var obj = new { message = "", ModelState = new Dictionary<string, string[]>() };
                var modelStateResult = JsonConvert.DeserializeAnonymousType(resultContent, obj);

                modelState.Message = modelStateResult.message;

                if (modelStateResult.ModelState != null)
                {
                    modelState.ModelSate = modelStateResult.ModelState;
                }
            }

            return modelState;
        }

        public async Task<Response> PostManagerImageAsync(WebApiPostImage image)
        {
            if (image.Image != null && image.Id != null)
            {
                AddTokenToHeader();

                HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/StoreAdmin/PostManagerImageAsync", image);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<Response>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<byte[]> GetManagerImageAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetManagerImageAsync/" + id);

            if (request.IsSuccessStatusCode)
            {
                var resultContent = await request.Content.ReadAsAsync<byte[]>();

                return resultContent;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteManagerAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsync("api/StoreAdmin/DeleteManagerAsync/" + id, null);

            if (request.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RestoreManagerAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsync("api/StoreAdmin/RestoreManagerAsync/" + id, null);

            if (request.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<WebApiManager>> GetAllDeletedManagers()
        {
            AddTokenToHeader();

            string storeAdminId = HttpContext.Current.Request.Cookies["UserID"].Value;

            if (storeAdminId != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllDeletedManagersAsync/" + storeAdminId);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<List<WebApiManager>>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<WebApiManager> DetailsManagerAsync(string id)
        {
            if (id != null)
            {
                AddTokenToHeader();

                HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/DetailsManagerAsync/" + id);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<WebApiManager>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<List<WebApiManagerStore>> GetAllManagerStoresAsync(string id)
        {
            if (id != null)
            {
                AddTokenToHeader();

                HttpResponseMessage request = await apiClient.GetAsync("api/StoreAdmin/GetAllManagerStoresAsync/" + id);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<List<WebApiManagerStore>>();

                    return resultContent;
                }
            }

            return null;
        }

        //public List<WebApiStore> GetAllNotAssignedStores(string id)
        //{
        //    List<WebApiStore> stores = new List<WebApiStore>();

        //    if (id != null)
        //    {
        //        AddTokenToHeader();

        //        var response = apiClient.GetAsync("api/StoreAdmin/GetAllNotAssignedStores/" + id);
        //        response.Wait();

        //        var result = response.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var resultContent = result.Content.ReadAsAsync<List<WebApiStore>>();
        //            resultContent.Wait();

        //            stores = resultContent.Result;
        //        }
        //    }

        //    return stores;
        //}

        public async Task<Response> AssignStoreAsync(WebApiStoreAssign storeAssign)
        {
            if (storeAssign.ManagerId != null && storeAssign.StoreId != null)
            {
                AddTokenToHeader();

                var jsonContent = JsonConvert.SerializeObject(storeAssign);

                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage request = await apiClient.PostAsync("api/StoreAdmin/AssignStoreAsync/", httpContent);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<Response>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<Response> UnassignStoreAsync(WebApiStoreAssign storeUnassign)
        {
            if (storeUnassign.ManagerId != null && storeUnassign.StoreId != null)
            {
                AddTokenToHeader();

                var jsonContent = JsonConvert.SerializeObject(storeUnassign);

                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage request = await apiClient.PostAsync("api/StoreAdmin/UnassignStoreAsync/", httpContent);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<Response>();

                    return resultContent;
                }
            }

            return null;
        }
    }
}