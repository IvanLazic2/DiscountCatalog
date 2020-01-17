using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.MVC.Models;
using Hanssens.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Script.Serialization;

namespace AbatementHelper.MVC.Repositories
{
    public class AccountRepository
    {
        private HttpClient apiClient;

        public AccountRepository()
        {
            InitializeClient();
        }

        public void AddTokenToHeader()
        {
            var token = HttpContext.Current.Request.Cookies["Access_Token"];

            if (token != null)
            {
                apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value.ToString());
            }
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<WebApiResult> RegisterAsync(User user)
        {
            var jsonContent = JsonConvert.SerializeObject(user);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PostAsync("api/Account/RegisterAsync", httpContent);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiAuthenticatedUserResult> LoginAsync(AuthenticationModel authentication)
        {
            var jsonContent = JsonConvert.SerializeObject(authentication);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PostAsync("api/Account/AuthenticateAsync", httpContent);

            WebApiAuthenticatedUserResult result = await request.Content.ReadAsAsync<WebApiAuthenticatedUserResult>();

            return result;

        }

        public async Task<WebApiUserResult> DetailsAsync()
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            HttpResponseMessage request = await apiClient.GetAsync("api/Account/DetailsAsync/" + id);

            WebApiUserResult result = await request.Content.ReadAsAsync<WebApiUserResult>();

            return result;
        }

        public async Task<WebApiResult> PostUserImageAsync(WebApiPostImage image)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Account/PostUserImageAsync", image);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<byte[]> GetUserImageAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/Account/GetUserImageAsync/" + id);

            var result = await request.Content.ReadAsAsync<byte[]>();

            return result;
        }

        public async Task<WebApiUserResult> EditAsync()
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            HttpResponseMessage request = await apiClient.GetAsync("api/Account/EditAsync/" + id);

            WebApiUserResult result = await request.Content.ReadAsAsync<WebApiUserResult>();

            return result;
        }

        public async Task<WebApiResult> EditAsync(WebApiUser user)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Account/EditAsync", user);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiResult> DeleteAsync(WebApiUser user)
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            HttpResponseMessage request = await apiClient.PutAsync("api/Account/DeleteAsync/" + id, null);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }

        public async Task<WebApiResult> RestoreAsync(WebApiUser user)
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            HttpResponseMessage request = await apiClient.PutAsync("api/Account/RestoreAsync/" + id, null);

            WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

            return result;
        }
    }
}