using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Processors;
using AbatementHelper.WebAPI.Repositories;
using AbatementHelper.WebAPI.Validators;
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
using AbatementHelper.WebAPI.Models;

namespace AbatementHelper.WebAPI.Controllers
{
    [RoutePrefix("api/Store")]
    public class StoreController : ApiController
    {
        private StoreRepository storeRepository = new StoreRepository();

        private void SimulateValidation(object model)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }

        [HttpGet]
        [Route("GetAllProductsAsync/{id}")]
        public async Task<List<WebApiProduct>> GetAllProductsAsync(string id)
        {
            return await storeRepository.GetAllProductsAsync(id);
        }

        [HttpPost]
        [Route("CreateProductAsync")]
        public async Task<IHttpActionResult> CreateProductAsync(WebApiProduct product)
        {
            SimulateValidation(product);

            var priceValidator = new PriceValidator();
            var discountValidator = new DiscountValidator();

            var priceValidatorResult = priceValidator.GetErrors(product);

            if (!priceValidatorResult.Success)
            {
                foreach (var error in priceValidatorResult.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return BadRequest(ModelState);
            }

            product.OldPrice = priceValidatorResult.OldPrice;
            product.NewPrice = priceValidatorResult.NewPrice;
            product.Discount = priceValidatorResult.Discount;

            FluentValidation.Results.ValidationResult discountValidatorResult = discountValidator.Validate(product);

            if (!discountValidatorResult.IsValid)
            {
                foreach (ValidationFailure failure in discountValidatorResult.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            ModelStateResponse response = await storeRepository.CreateProductAsync(product);

            if (!response.IsValid)
            {
                foreach (var error in response.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpPut]
        [Route("EditProductAsync")]
        public async Task<IHttpActionResult> EditProductAsync(WebApiProduct product)
        {
            SimulateValidation(product);
            
            var priceValidator = new PriceValidator();
            var discountValidator = new DiscountValidator();

            var priceValidatorResult = priceValidator.GetErrors(product);

            if (!priceValidatorResult.Success)
            {
                foreach (var error in priceValidatorResult.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return BadRequest(ModelState);
            }

            product.OldPrice = priceValidatorResult.OldPrice;
            product.NewPrice = priceValidatorResult.NewPrice;
            product.Discount = priceValidatorResult.Discount;

            FluentValidation.Results.ValidationResult discountValidatorResult = discountValidator.Validate(product);

            if (!discountValidatorResult.IsValid)
            {
                foreach (ValidationFailure failure in discountValidatorResult.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            ModelStateResponse response = await storeRepository.EditProductAsync(product);

            if (!response.IsValid)
            {
                foreach (var error in response.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
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