using AbatementHelper.CommonModels.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AbatementHelper.MVC.Repositeories
{
    public class AdminRepository
    {
        private HttpClient apiClient;

        public AdminRepository()
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

        public DataBaseResultListOfUsers GetAllUsers(string role)
        {
            DataBaseResultListOfUsers users = new DataBaseResultListOfUsers();

            AddTokenToHeader();

            var response = apiClient.GetAsync("api/Admin/GetAllUsers/"+role);
            response.Wait();

            var result = response.Result;

            var resultString = result.ToString();

            if (result.IsSuccessStatusCode) //ovdje dobiva server error
            {
                var resultContent = result.Content.ReadAsAsync<DataBaseResultListOfUsers>();
                resultContent.Wait();

                users = resultContent.Result;
            }

            return users;
        }

        public DataBaseResultListOfStores GetAllStores()
        {
            DataBaseResultListOfStores users = new DataBaseResultListOfStores();

            AddTokenToHeader();

            var response = apiClient.GetAsync("api/Admin/GetAllStores");
            response.Wait();

            var result = response.Result;

            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<DataBaseResultListOfStores>();
                resultContent.Wait();

                users = resultContent.Result;
            }

            return users;
        }

        public DataBaseUser EditUser(string id)
        {
            DataBaseUser user = new DataBaseUser();

            AddTokenToHeader();

            var response = apiClient.GetAsync("api/Admin/EditUser/" + id);
            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<DataBaseUser>();
                resultContent.Wait();

                user = resultContent.Result;
            }

            return user;

        }

        public bool EditUser(DataBaseUser user)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsJsonAsync("api/Admin/EditUser", user);
            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public DataBaseStore EditStore(string id)
        {
            DataBaseStore store = new DataBaseStore();

            AddTokenToHeader();

            var response = apiClient.GetAsync("api/Admin/EditStore/" + id);
            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<DataBaseStore>();
                resultContent.Wait();

                store = resultContent.Result;
            }

            return store;

        }

        public bool EditStore(DataBaseStore store)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsJsonAsync("api/Admin/EditStore", store);
            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public bool Delete(string role, string id)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsync("api/Admin/Delete/"  + role + "/" + id, null);
            response.Wait();

            var result = response.Result;

            
            var resultString = result.ToString();

            
            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public bool Restore(string role, string id)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsync("api/Admin/Restore/" + role + "/" + id, null);
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