using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Processors;
using AbatementHelper.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AbatementHelper.WebAPI.Controllers
{
    [RoutePrefix("api/Store")]
    public class StoreController : ApiController
    {
        private StoreRepository store = new StoreRepository();

        [HttpGet]
        [Route("GetAllProducts/{id}")]
        public List<WebApiProduct> GetAllProducts(string id)
        {
            return store.GetAllProducts(id);
        }

        [HttpPost]
        [Route("CreateProduct")]
        public Response CreateProduct(WebApiProduct product)
        {
            return store.CreateProduct(product);
        }

        [HttpPost]
        [Route("EditProduct")]
        public Response EditProduct(WebApiProduct product)
        {
            return store.EditProduct(product);
        }

        [HttpGet]
        [Route("ProductDetails/{id}")]
        public WebApiProduct ProductDetails(string id)
        {
            return ProductProcessor.ProductEntityToWebApiProduct(store.ReadProductById(id).Result);
        }

        [HttpPut]
        [Route("DeleteProduct/{id}")]
        public IHttpActionResult DeleteProduct(string id)
        {
            store.DeleteProduct(id);

            return Ok();
        }

        [HttpGet]
        [Route("GetAllDeletedProducts/{id}")]
        public List<WebApiProduct> GetAllDeletedProducts(string id)
        {
            return store.GetAllDeletedProducts(id);
        }

        [HttpPut]
        [Route("RestoreProduct/{id}")]
        public IHttpActionResult RestoreProduct(string id)
        {
            store.RestoreProduct(id);

            return Ok();
        }

        [HttpGet]
        [Route("GetAllExpiredProducts/{id}")]
        public List<WebApiProduct> GetAllExpiredProducts(string id)
        {
            return store.GetAllExpiredProducts(id);
        }
    }
}