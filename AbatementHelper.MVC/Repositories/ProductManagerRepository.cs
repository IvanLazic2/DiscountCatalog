using AbatementHelper.Classes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.MVC.Repositories
{
    public class ProductManagerRepository
    {
        public static async Task<bool> SaveProduct(Product product)
        {
            var client = new HttpClient();

            var jsonContent = JsonConvert.SerializeObject(product);

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://localhost:51188/SaveProduct", httpContent);

            var responseString = await response.Content.ReadAsStringAsync();

            if (responseString == "false")
                return false;
            else
                return true;
        }
    }
}