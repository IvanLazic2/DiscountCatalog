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
        private StoreRepository storeRepository = new StoreRepository();

        [HttpGet]
        [Route("GetAllProducts/{id}")]
        public List<WebApiProduct> GetAllProducts(string id)
        {
            return storeRepository.GetAllProducts(id);
        }

        [HttpPost]
        [Route("CreateProduct")]
        public Response CreateProduct(WebApiProduct product)
        {
            return storeRepository.CreateProduct(product);
        }

        [HttpPost]
        [Route("EditProduct")]
        public Response EditProduct(WebApiProduct product)
        {
            return storeRepository.EditProduct(product);
        }

        [HttpGet]
        [Route("ProductDetails/{id}")]
        public WebApiProduct ProductDetails(string id)
        {
            return ProductProcessor.ProductEntityToWebApiProduct(storeRepository.ReadProductById(id).Result);
        } 

        [HttpPut]
        [Route("DeleteProduct/{id}")]
        public IHttpActionResult DeleteProduct(string id)
        {
            storeRepository.DeleteProduct(id);

            return Ok();
        }

        [HttpGet]
        [Route("GetAllDeletedProducts/{id}")]
        public List<WebApiProduct> GetAllDeletedProducts(string id)
        {
            return storeRepository.GetAllDeletedProducts(id);
        }

        [HttpPut]
        [Route("RestoreProduct/{id}")]
        public IHttpActionResult RestoreProduct(string id)
        {
            storeRepository.RestoreProduct(id);

            return Ok();
        }

        [HttpGet]
        [Route("GetAllExpiredProducts/{id}")]
        public List<WebApiProduct> GetAllExpiredProducts(string id)
        {
            return storeRepository.GetAllExpiredProducts(id);
        }

        [HttpPut]
        [Route("PostProductImage")]
        public Response PostProductImage(WebApiProduct product)
        {
            Response response = storeRepository.PostProductImage(product);

            return response;
        }

        [HttpGet]
        [Route("GetProductImage/{id}")]
        public byte[] GetProductImage(string id)
        {
            byte[] byteArray = storeRepository.GetProductImage(id);

            return ImageProcessor.CreateThumbnail(byteArray);
        }
    }
}