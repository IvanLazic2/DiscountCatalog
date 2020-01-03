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
        [Route("GetAllProductsAsync/{id}")]
        public async Task<List<WebApiProduct>> GetAllProductsAsync(string id)
        {
            return await storeRepository.GetAllProductsAsync(id);
        }

        [HttpPost]
        [Route("CreateProductAsync")]
        public async Task<Response> CreateProductAsync(WebApiProduct product)
        {
            return await storeRepository.CreateProductAsync(product);
        }

        [HttpPost]
        [Route("EditProductAsync")]
        public async Task<Response> EditProductAsync(WebApiProduct product)
        {
            return await storeRepository.EditProductAsync(product);
        }

        [HttpGet]
        [Route("ProductDetailsAsync/{id}")]
        public async Task<WebApiProduct> ProductDetailsAsync(string id)
        {
            ProductEntity product = await storeRepository.ReadProductByIdAsync(id);

            return await ProductProcessor.ProductEntityToWebApiProductAsync(product);
        } 

        [HttpPut]
        [Route("DeleteProductAsync/{id}")]
        public async Task<IHttpActionResult> DeleteProductAsync(string id)
        {
            await storeRepository.DeleteProductAsync(id);

            return Ok();
        }

        [HttpGet]
        [Route("GetAllDeletedProductsAsync/{id}")]
        public async Task<List<WebApiProduct>> GetAllDeletedProductsAsync(string id)
        {
            return await storeRepository.GetAllDeletedProductsAsync(id);
        }

        [HttpPut]
        [Route("RestoreProductAsync/{id}")]
        public async Task<IHttpActionResult> RestoreProductAsync(string id)
        {
            await storeRepository.RestoreProductAsync(id);

            return Ok();
        }

        [HttpGet]
        [Route("GetAllExpiredProductsAsync/{id}")]
        public async Task<List<WebApiProduct>> GetAllExpiredProductsAsync(string id)
        {
            return await storeRepository.GetAllExpiredProductsAsync(id);
        }

        [HttpPut]
        [Route("PostProductImageAsync")]
        public async Task<Response> PostProductImageAsync(WebApiPostImage product)
        {
            Response response = await storeRepository.PostProductImageAsync(product);

            return response;
        }

        [HttpGet]
        [Route("GetProductImageAsync/{id}")]
        public async Task<byte[]> GetProductImageAsync(string id)
        {
            byte[] byteArray = await storeRepository.GetProductImageAsync(id);

            return ImageProcessor.CreateThumbnail(byteArray);
        }
    }
}