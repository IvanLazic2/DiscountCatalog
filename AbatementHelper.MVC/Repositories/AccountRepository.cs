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

        //public async Task<Response> Login(string email, string userName, string password)
        //{
        //    var data = new FormUrlEncodedContent(new[]
        //    {
        //        new KeyValuePair<string, string>("Email", email),
        //        new KeyValuePair<string, string>("UserName", userName),
        //        new KeyValuePair<string, string>("Password", password)
        //    });

        //    var request = await apiClient.PostAsync("/api/Login/AuthenticateAsync", data);

        //    var response = request.Content.ReadAsStringAsync();

        //    string responseString = response.Result;

        //    responseModel = JsonConvert.DeserializeObject<Response>(responseString);

        //    ResponseMessageText = responseModel.ResponseMessage;

        //    if (responseModel.ResponseCode == (int)HttpStatusCode.OK)
        //    {

        //        LoginSuccessful = true;
        //    }
        //    else
        //    {
        //        LoginSuccessful = false;
        //    }

        //    return responseModel;
        //}

        public async Task<Response> RegisterAsync(User user)
        {
            var jsonContent = JsonConvert.SerializeObject(user);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PostAsync("api/Account/RegisterAsync", httpContent);

            Response result = await request.Content.ReadAsAsync<Response>();

            return result;
        }

        public async Task<Response> LoginAsync(AuthenticationModel authentication)
        {
            var jsonContent = JsonConvert.SerializeObject(authentication);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage request = await apiClient.PostAsync("api/Account/AuthenticateAsync", httpContent);

            if (request.IsSuccessStatusCode)
            {
                var result = await request.Content.ReadAsAsync<Response>();

                return result;
            }

            return null;
        }

        public async Task<WebApiUser> DetailsAsync()
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            if (id != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/Account/DetailsAsync/" + id);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<WebApiUser>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<Response> PostUserImageAsync(WebApiPostImage image)
        {
            if (image.Id != null && image.Image != null)
            {
                AddTokenToHeader();

                HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Account/PostUserImageAsync", image);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<Response>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<byte[]> GetUserImageAsync(string id)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.GetAsync("api/Account/GetUserImageAsync/" + id);

            var resultContent = await request.Content.ReadAsAsync<byte[]>();

            return resultContent;
        }

        public async Task<WebApiUser> EditAsync()
        {
            WebApiUser user = new WebApiUser();

            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            if (id != null)
            {
                HttpResponseMessage request = await apiClient.GetAsync("api/Account/EditAsync/" + id);

                if (request.IsSuccessStatusCode)
                {
                    var resultContent = await request.Content.ReadAsAsync<WebApiUser>();

                    return resultContent;
                }
            }

            return null;
        }

        public async Task<Response> EditAsync(WebApiUser user)
        {
            AddTokenToHeader();

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Account/EditAsync", user);

            if (request.IsSuccessStatusCode)
            {
                Response resultContent = await request.Content.ReadAsAsync<Response>();

                return resultContent;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(WebApiUser user)
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            if (id != null)
            {
                HttpResponseMessage request = await apiClient.PutAsync("api/Account/DeleteAsync/" + id, null);

                if (request.IsSuccessStatusCode)
                {
                    return true;
                }
            }

            return false;
        }


        public async Task<bool> RestoreAsync(WebApiUser user)
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            if (id != null)
            {
                HttpResponseMessage request = await apiClient.PutAsync("api/Account/RestoreAsync/" + id, null);

                if (request.IsSuccessStatusCode)
                {
                    return true;
                }
            }

            return false;
        }
    }
}