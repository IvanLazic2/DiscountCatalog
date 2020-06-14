using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.WebAPI.Processors;
using DiscountCatalog.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using FluentValidation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Service.Contractor;
using DiscountCatalog.WebAPI.Service.Implementation;
using DiscountCatalog.WebAPI.REST.Product;
using System.Net;
using DiscountCatalog.WebAPI.Paging.Contractor;

namespace DiscountCatalog.WebAPI.Controllers
{
    [RoutePrefix("api/Store")]
    public class StoreController : ApiController
    {
        #region Fields

        IProductService productService;

        #endregion

        #region Constructors

        public StoreController()
        {
            productService = new ProductService();
        }

        #endregion

        #region Methods

        [HttpPost]
        [Route("CreateProduct")]
        public IHttpActionResult CreateProduct(ProductRESTPost model)
        {
            Result result = productService.Create(model);

            if (result.Success)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }


        }

        [HttpGet]
        [Route("GetAllProducts/{storeId}")]
        public IHttpActionResult GetAllProducts(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter, bool includeUpcoming)
        {
            IPagingList<ProductREST> list = productService.GetAll(storeId, sortOrder, searchString, pageIndex, pageSize, priceFilter, dateFilter, includeUpcoming);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetAllDeletedProducts/{storeId}")]
        public IHttpActionResult GetAllDeletedProducts(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter, bool includeUpcoming)
        {
            IPagingList<ProductREST> list = productService.GetAllDeleted(storeId, sortOrder, searchString, pageIndex, pageSize, priceFilter, dateFilter, includeUpcoming);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetAllExpiredProducts/{storeId}")]
        public IHttpActionResult GetAllExpiredProducts(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter, bool includeUpcoming)
        {
            IPagingList<ProductREST> list = productService.GetAllExpired(storeId, sortOrder, searchString, pageIndex, pageSize, priceFilter, dateFilter, includeUpcoming);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetProduct/{storeId}")]
        public IHttpActionResult GetProduct(string storeId, string productId)
        {
            ProductREST product = productService.Get(storeId, productId);

            return Ok(product);
        }

        [HttpGet]
        [Route("GetExpiredProduct/{storeId}")]
        public IHttpActionResult GetExpiredProduct(string storeId, string productId)
        {
            ProductREST product = productService.GetExpired(storeId, productId);

            return Ok(product);
        }

        [HttpPut]
        [Route("EditProduct/{storeId}")]
        public async Task<IHttpActionResult> EditProduct(string storeId, ProductRESTPut model)
        {
            Result result = await productService.UpdateAsync(storeId, model);

            if (result.Success)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpGet]
        [Route("DeleteProduct/{storeId}")]
        public IHttpActionResult DeleteProduct(string storeId, string productId)
        {
            Result result = productService.Delete(storeId, productId);

            if (result.Success)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpGet]
        [Route("RestoreProduct/{storeId}")]
        public IHttpActionResult RestoreProduct(string storeId, string productId)
        {
            Result result = productService.Restore(storeId, productId);

            if (result.Success)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPut]
        [Route("PostProductImage/{storeId}")]
        public IHttpActionResult PostProductImage(string storeId, string productId, byte[] image)
        {
            Result result = productService.PostProductImage(storeId, productId, image);

            if (result.Success)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpGet]
        [Route("GetProductImage/{storeId}")]
        public byte[] GeStoreImage(string storeId, string productId)
        {
            byte[] image = productService.GetImage(productId);

            return image;
        }

        #endregion

    }
}