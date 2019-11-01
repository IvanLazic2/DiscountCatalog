using AbatementHelper.CommonModels.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

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

        public DataBaseResultListOfUsers GetAllUsers()
        {
            DataBaseResultListOfUsers users = new DataBaseResultListOfUsers();

            AddTokenToHeader();

            var response = apiClient.GetAsync("api/Admin/GetAllUsers");
            response.Wait();

            var result = response.Result;

            if (result.IsSuccessStatusCode)
            {
                var resultContent = result.Content.ReadAsAsync<DataBaseResultListOfUsers>();
                resultContent.Wait();

                users = resultContent.Result;
            }

            return users;
        }

        public DataBaseUser Edit(string id)
        {
            DataBaseUser user = new DataBaseUser();

            AddTokenToHeader();

            var response = apiClient.GetAsync("api/Admin/Edit/" + id);
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

        public bool Edit(DataBaseUser user)
        {
            AddTokenToHeader();

            var response = apiClient.PutAsJsonAsync<DataBaseUser>("api/Admin/Edit", user);
            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public DataBaseUser GetDelete(string id)
        {
            DataBaseUser user = new DataBaseUser();

            AddTokenToHeader();

            var response = apiClient.GetAsync("api/Admin/GetDelete/" + id);
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

        public bool Delete(string id)
        { 
            AddTokenToHeader();

            var response = apiClient.DeleteAsync("api/Admin/Delete/" + id);
            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}