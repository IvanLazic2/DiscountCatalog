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
        public IHttpActionResult GetAllProducts(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            IPagingList<ProductREST> list = productService.GetAll(storeId, sortOrder, searchString, pageIndex, pageSize);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetAllDeletedProducts/{storeId}")]
        public IHttpActionResult GetAllDeletedProducts(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            IPagingList<ProductREST> list = productService.GetAllDeleted(storeId, sortOrder, searchString, pageIndex, pageSize);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetAllExpiredProducts/{storeId}")]
        public IHttpActionResult GetAllExpiredProducts(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            IPagingList<ProductREST> list = productService.GetAllExpired(storeId, sortOrder, searchString, pageIndex, pageSize);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetProduct/{storeId}")]
        public IHttpActionResult GetProduct(string storeId, string productId)
        {
            ProductREST product = productService.Get(storeId, productId);

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


        //[HttpGet]
        //[Route("GetAllActiveProductsAsync/{id}")]
        //public async Task<WebApiListOfProductsResult> GetAllActiveProductsAsync(string id)
        //{
        //    WebApiListOfProductsResult result = await storeRepository.GetAllActiveProductsAsync(id);

        //    return result;
        //}

        //[HttpPost]
        //[Route("CreateProductAsync")]
        //public async Task<WebApiResult> CreateProductAsync(WebApiProduct product)
        //{
        //    //SimulateValidation(product);

        //    var result = new WebApiResult();

        //    var priceValidator = new PriceValidator();
        //    var discountValidator = new DiscountValidator();

        //    var priceValidatorResult = priceValidator.GetErrors(product);

        //    if (!priceValidatorResult.Success)
        //    {
        //        foreach (var error in priceValidatorResult.Errors)
        //        {
        //            result.AddModelError(error.Key, error.Value);
        //        }

        //        return result;
        //    }

        //    product.OldPrice = priceValidatorResult.OldPrice;
        //    product.NewPrice = priceValidatorResult.NewPrice;
        //    product.Discount = priceValidatorResult.Discount;

        //    FluentValidation.Results.ValidationResult discountValidatorResult = discountValidator.Validate(product);

        //    if (!discountValidatorResult.IsValid)
        //    {
        //        foreach (ValidationFailure failure in discountValidatorResult.Errors)
        //        {
        //            result.AddModelError(failure.PropertyName, failure.ErrorMessage);
        //        }

        //        return result;
        //    }

        //    result = await storeRepository.CreateProductAsync(product);

        //    return result;
        //}

        //[HttpPut]
        //[Route("EditProductAsync")]
        //public async Task<WebApiResult> EditProductAsync(WebApiProduct product)
        //{
        //    //SimulateValidation(product);

        //    var result = new WebApiResult();

        //    var priceValidator = new PriceValidator();
        //    var discountValidator = new DiscountValidator();

        //    var priceValidatorResult = priceValidator.GetErrors(product);

        //    if (!priceValidatorResult.Success)
        //    {
        //        foreach (var error in priceValidatorResult.Errors)
        //        {
        //            result.AddModelError(error.Key, error.Value);
        //        }

        //        return result;
        //    }

        //    product.OldPrice = priceValidatorResult.OldPrice;
        //    product.NewPrice = priceValidatorResult.NewPrice;
        //    product.Discount = priceValidatorResult.Discount;

        //    FluentValidation.Results.ValidationResult discountValidatorResult = discountValidator.Validate(product);

        //    if (!discountValidatorResult.IsValid)
        //    {
        //        foreach (ValidationFailure failure in discountValidatorResult.Errors)
        //        {
        //            result.AddModelError(failure.PropertyName, failure.ErrorMessage);
        //        }

        //        return result;
        //    }

        //    result = await storeRepository.EditProductAsync(product);

        //    return result;
        //}

        //[HttpGet]
        //[Route("ProductDetailsAsync/{id}")]
        //public async Task<WebApiProductResult> ProductDetailsAsync(string id)
        //{
        //    WebApiProductResult result = await storeRepository.ReadProductByIdAsync(id);

        //    return result;
        //}

        //[HttpPut]
        //[Route("DeleteProductAsync/{id}")]
        //public async Task<WebApiResult> DeleteProductAsync(string id)
        //{
        //    WebApiResult result = await storeRepository.DeleteProductAsync(id);

        //    return result;
        //}

        //[HttpGet]
        //[Route("GetAllDeletedProductsAsync/{id}")]
        //public async Task<WebApiListOfProductsResult> GetAllDeletedProductsAsync(string id)
        //{
        //    WebApiListOfProductsResult result = await storeRepository.GetAllDeletedProductsAsync(id);

        //    return result;
        //}

        //[HttpPut]
        //[Route("RestoreProductAsync/{id}")]
        //public async Task<WebApiResult> RestoreProductAsync(string id)
        //{
        //    WebApiResult result = await storeRepository.RestoreProductAsync(id);

        //    return result;
        //}

        //[HttpGet]
        //[Route("GetAllExpiredProductsAsync/{id}")]
        //public async Task<WebApiListOfProductsResult> GetAllExpiredProductsAsync(string id)
        //{
        //    WebApiListOfProductsResult result = await storeRepository.GetAllExpiredProductsAsync(id);

        //    return result;
        //}

        //[HttpPut]
        //[Route("PostProductImageAsync")]
        //public async Task<WebApiResult> PostProductImageAsync(WebApiPostImage product)
        //{
        //    WebApiResult result = await storeRepository.PostProductImageAsync(product);

        //    return result;
        //}

        //[HttpGet]
        //[Route("GetProductImageAsync/{id}")]
        //public async Task<byte[]> GetProductImageAsync(string id)
        //{
        //    byte[] byteArray = await storeRepository.GetProductImageAsync(id);

        //    return ImageProcessor.CreateThumbnail(byteArray);

        //    //return byteArray;
        //}
    }
}