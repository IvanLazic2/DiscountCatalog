using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.MVC.Models;
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

namespace DiscountCatalog.MVC.Repositories
{
    public class StoreRepository : MVCRepository
    {


        //private async Task<WebApiResult> ReadResponseAsync(HttpResponseMessage request)
        //{
        //    var modelState = new WebApiResult();

        //    if (!request.IsSuccessStatusCode)
        //    {
        //        var resultContent = await request.Content.ReadAsStringAsync();

        //        var obj = new { message = "", ModelState = new Dictionary<string, string[]>() };
        //        var modelStateResult = JsonConvert.DeserializeAnonymousType(resultContent, obj);

        //        if (modelStateResult.ModelState != null)
        //        {
        //            foreach (var error in modelStateResult.ModelState)
        //            {
        //                modelState.ModelState.Add(error.Key, error.Value.First());
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var successMessage = await request.Content.ReadAsAsync<string>();

        //        modelState.Message = successMessage;
        //    }

        //    return modelState;
        //}

        //public async Task<WebApiResult> CreateProductAsync(WebApiProduct product)
        //{
        //    product.Store = new WebApiStore
        //    {
        //        Id = HttpContext.Current.Request.Cookies["StoreID"].Value,
        //        StoreName = HttpContext.Current.Request.Cookies["StoreName"].Value
        //    };

        //    AddTokenToHeader();

        //    var jsonContent = JsonConvert.SerializeObject(product);

        //    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //    HttpResponseMessage request = await apiClient.PostAsync("api/Store/CreateProductAsync", httpContent);

        //    WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //    return result;
        //}

        //public async Task<WebApiListOfProductsResult> GetAllProductsAsync()
        //{
        //    string storeId = HttpContext.Current.Request.Cookies["StoreID"].Value;

        //    AddTokenToHeader();

        //    HttpResponseMessage request = await apiClient.GetAsync("api/Store/GetAllProductsAsync/" + storeId);

        //    var result = request.Content;

        //    return result;

        //}

        //public async Task<WebApiProductResult> ProductDetailsAsync(string id)
        //{
        //    AddTokenToHeader();

        //    HttpResponseMessage request = await apiClient.GetAsync("api/Store/ProductDetailsAsync/" + id);

        //    WebApiProductResult result = await request.Content.ReadAsAsync<WebApiProductResult>();

        //    return result;
        //}

        //public async Task<WebApiResult> PostProductImageAsync(WebApiPostImage image)
        //{
        //    AddTokenToHeader();

        //    HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Store/PostProductImageAsync", image);

        //    WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //    return result;
        //}

        //public async Task<byte[]> GetProductImageAsync(string id)
        //{
        //    AddTokenToHeader();

        //    HttpResponseMessage request = await apiClient.GetAsync("api/Store/GetProductImageAsync/" + id);

        //    var resultContent = await request.Content.ReadAsAsync<byte[]>();

        //    return resultContent;
        //}

        //public async Task<WebApiProductResult> EditProductAsync(WebApiProduct product)
        //{
        //    product.Store = new WebApiStore
        //    {
        //        Id = HttpContext.Current.Request.Cookies["StoreID"].Value
        //    };

        //    AddTokenToHeader();

        //    HttpResponseMessage request = await apiClient.PutAsJsonAsync("api/Store/EditProductAsync", product);

        //    WebApiProductResult result = await request.Content.ReadAsAsync<WebApiProductResult>();

        //    return result;

        //}

        //public async Task<WebApiResult> DeleteProductAsync(string id)
        //{
        //    AddTokenToHeader();

        //    HttpResponseMessage request = await apiClient.PutAsync("api/Store/DeleteProductAsync/" + id, null);

        //    WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //    return result;
        //}

        //public async Task<WebApiResult> RestoreProductAsync(string id)
        //{
        //    AddTokenToHeader();

        //    HttpResponseMessage request = await apiClient.PutAsync("api/Store/RestoreProductAsync/" + id, null);

        //    WebApiResult result = await request.Content.ReadAsAsync<WebApiResult>();

        //    return result;
        //}

        //public async Task<WebApiListOfProductsResult> GetAllDeletedProductsAsync()
        //{
        //    string storeId = HttpContext.Current.Request.Cookies["StoreID"].Value;

        //    AddTokenToHeader();

        //    HttpResponseMessage request = await apiClient.GetAsync("api/Store/GetAllDeletedProductsAsync/" + storeId);

        //    WebApiListOfProductsResult result = await request.Content.ReadAsAsync<WebApiListOfProductsResult>();

        //    return result;
        //}

        //public async Task<WebApiListOfProductsResult> GetAllExpiredProductsAsync()
        //{
        //    string storeId = HttpContext.Current.Request.Cookies["StoreID"].Value;

        //    AddTokenToHeader();

        //    HttpResponseMessage request = await apiClient.GetAsync("api/Store/GetAllExpiredProductsAsync/" + storeId);

        //    WebApiListOfProductsResult result = await request.Content.ReadAsAsync<WebApiListOfProductsResult>();

        //    return result;
        //}
    }
}