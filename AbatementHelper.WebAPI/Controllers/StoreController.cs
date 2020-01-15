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
        public async Task<WebApiListOfProductsResult> GetAllProductsAsync(string id)
        {
            WebApiListOfProductsResult result = await storeRepository.GetAllProductsAsync(id);

            return result;
        }

        [HttpGet]
        [Route("GetAllActiveProductsAsync/{id}")]
        public async Task<WebApiListOfProductsResult> GetAllActiveProductsAsync(string id)
        {
            WebApiListOfProductsResult result = await storeRepository.GetAllActiveProductsAsync(id);

            return result;
        }

        [HttpPost]
        [Route("CreateProductAsync")]
        public async Task<WebApiResult> CreateProductAsync(WebApiProduct product)
        {
            //SimulateValidation(product);

            var result = new WebApiResult();

            var priceValidator = new PriceValidator();
            var discountValidator = new DiscountValidator();

            var priceValidatorResult = priceValidator.GetErrors(product);

            if (!priceValidatorResult.Success)
            {
                foreach (var error in priceValidatorResult.Errors)
                {
                    result.ModelState.Add(error.Key, error.Value);
                }

                return result;
            }

            product.OldPrice = priceValidatorResult.OldPrice;
            product.NewPrice = priceValidatorResult.NewPrice;
            product.Discount = priceValidatorResult.Discount;

            FluentValidation.Results.ValidationResult discountValidatorResult = discountValidator.Validate(product);

            if (!discountValidatorResult.IsValid)
            {
                foreach (ValidationFailure failure in discountValidatorResult.Errors)
                {
                    result.ModelState.Add(failure.PropertyName, failure.ErrorMessage);
                }

                return result;
            }

            result = await storeRepository.CreateProductAsync(product);

            return result;
        }

        [HttpPut]
        [Route("EditProductAsync")]
        public async Task<WebApiResult> EditProductAsync(WebApiProduct product)
        {
            //SimulateValidation(product);

            var result = new WebApiResult();
            
            var priceValidator = new PriceValidator();
            var discountValidator = new DiscountValidator();

            var priceValidatorResult = priceValidator.GetErrors(product);

            if (!priceValidatorResult.Success)
            {
                foreach (var error in priceValidatorResult.Errors)
                {
                    result.ModelState.Add(error.Key, error.Value);
                }

                return result;
            }

            product.OldPrice = priceValidatorResult.OldPrice;
            product.NewPrice = priceValidatorResult.NewPrice;
            product.Discount = priceValidatorResult.Discount;

            FluentValidation.Results.ValidationResult discountValidatorResult = discountValidator.Validate(product);

            if (!discountValidatorResult.IsValid)
            {
                foreach (ValidationFailure failure in discountValidatorResult.Errors)
                {
                    result.ModelState.Add(failure.PropertyName, failure.ErrorMessage);
                }

                return result;
            }

            result = await storeRepository.EditProductAsync(product);

            return result;
        }

        [HttpGet]
        [Route("ProductDetailsAsync/{id}")]
        public async Task<WebApiProductResult> ProductDetailsAsync(string id)
        {
            WebApiProductResult result = await storeRepository.ReadProductByIdAsync(id);

            return result;
        }

        [HttpPut]
        [Route("DeleteProductAsync/{id}")]
        public async Task<WebApiResult> DeleteProductAsync(string id)
        {
            WebApiResult result = await storeRepository.DeleteProductAsync(id);

            return result;
        }

        [HttpGet]
        [Route("GetAllDeletedProductsAsync/{id}")]
        public async Task<WebApiListOfProductsResult> GetAllDeletedProductsAsync(string id)
        {
            WebApiListOfProductsResult result = await storeRepository.GetAllDeletedProductsAsync(id);

            return result;
        }

        [HttpPut]
        [Route("RestoreProductAsync/{id}")]
        public async Task<WebApiResult> RestoreProductAsync(string id)
        {
            WebApiResult result = await storeRepository.RestoreProductAsync(id);

            return result;
        }

        [HttpGet]
        [Route("GetAllExpiredProductsAsync/{id}")]
        public async Task<WebApiListOfProductsResult> GetAllExpiredProductsAsync(string id)
        {
            WebApiListOfProductsResult result = await storeRepository.GetAllExpiredProductsAsync(id);

            return result;
        }

        [HttpPut]
        [Route("PostProductImageAsync")]
        public async Task<WebApiResult> PostProductImageAsync(WebApiPostImage product)
        {
            WebApiResult result = await storeRepository.PostProductImageAsync(product);

            return result;
        }

        [HttpGet]
        [Route("GetProductImageAsync/{id}")]
        public async Task<byte[]> GetProductImageAsync(string id)
        {
            byte[] byteArray = await storeRepository.GetProductImageAsync(id);

            //return ImageProcessor.CreateThumbnail(byteArray);

            return byteArray;
        }
    }
}