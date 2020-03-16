using DiscountCatalog.Common.WebApiModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace DiscountCatalog.MVC.Repositories
{
    public class HomeRepository
    {
        private HttpClient apiClient;

        public HomeRepository()
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

        private void AddTokenToHeader()
        {
            var token = HttpContext.Current.Request.Cookies["Access_Token"];

            if (token != null)
            {
                apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value.ToString());
            }
        }

        //public async Task<WebApiListOfProductsResult> GetAllProductsAsync()
        //{
        //    AddTokenToHeader();

        //    HttpResponseMessage request = await apiClient.GetAsync("api/Home/GetAllProductsAsync/");

        //    var result = await request.Content.ReadAsAsync<WebApiListOfProductsResult>();

        //    return result;

        //}
    }
}