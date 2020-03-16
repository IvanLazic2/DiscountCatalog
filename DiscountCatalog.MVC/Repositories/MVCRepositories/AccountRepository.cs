using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.Models.Extended;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.MVC.Models;
using DiscountCatalog.MVC.ViewModels;
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

namespace DiscountCatalog.MVC.Repositories
{
    public class AccountRepository : MVCRepository
    {
        public async Task<Result> RegisterAsync(UserViewModel user)
        {
            HttpResponseMessage request = await apiClient.PostAsJsonAsync("api/Account/Register", user);

            Result result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<AuthenticatedUserResult> Login(AuthenticationModel authentication)
        {
            HttpResponseMessage request = await apiClient.PostAsJsonAsync("api/Account/Authenticate", authentication);

            AuthenticatedUserResult result = await request.Content.ReadAsAsync<AuthenticatedUserResult>();

            return result;
        }

        public async Task<User> Details()
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            HttpResponseMessage request = await apiClient.GetAsync("api/Account/Details/" + id);

            User result = await request.Content.ReadAsAsync<User>();

            return result;
        }

        public async Task<Result> Edit(User user)
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Account/Edit/" + id, user);

            Result result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> Delete()
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            HttpResponseMessage request = await apiClient.PutAsync("api/Account/Delete/" + id, null);

            Result result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> Restore()
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            HttpResponseMessage request = await apiClient.PutAsync("api/Account/Restore/" + id, null);

            Result result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<Result> PostUserImage(byte[] image)
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Account/PostUserImage/" + id, image);

            Result result = await request.Content.ReadAsAsync<Result>();

            return result;
        }

        public async Task<byte[]> GetUserImage()
        {
            AddTokenToHeader();

            string id = HttpContext.Current.Request.Cookies["UserID"].Value;

            HttpResponseMessage request = await apiClient.GetAsync("api/Account/GetUserImage/" + id);

            var result = await request.Content.ReadAsAsync<byte[]>();

            return result;
        }
    }
}