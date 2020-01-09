using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Extentions;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;

namespace AbatementHelper.WebAPI.DataBaseValidation
{
    public class ProductValidation : IProductValidation
    {
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public decimal Discount { get; set; }

        public ModelStateResponse Validate(ProductEntity product)
        {
            var response = new ModelStateResponse();

            var webApiProduct = new WebApiProduct();

            try
            {
                var discountModel = new DiscountModel
                {
                    OldPrice = product.ProductOldPrice,
                    NewPrice = product.ProductNewPrice,
                    Discount = product.DiscountPercentage
                };

                DiscountResponseModel discountResponse = DiscountProcessor.DiscountCalculator(discountModel);

                if (!discountResponse.Success)
                {
                    response.ModelState.Add(ObjectExtensions.GetPropertyName(() => webApiProduct.ProductOldPrice), discountResponse.Message);
                    response.ModelState.Add(ObjectExtensions.GetPropertyName(() => webApiProduct.ProductNewPrice), discountResponse.Message);
                    response.ModelState.Add(ObjectExtensions.GetPropertyName(() => webApiProduct.DiscountPercentage), discountResponse.Message);
                }

                if (discountResponse.Discount.OldPrice.HasValue && discountResponse.Discount.NewPrice.HasValue && discountResponse.Discount.Discount.HasValue)
                {
                    //product.ProductOldPrice = discountResponse.Discount.OldPrice;
                    //product.ProductNewPrice = discountResponse.Discount.NewPrice;
                    //product.DiscountPercentage = discountResponse.Discount.Discount;

                    OldPrice = discountResponse.Discount.OldPrice.Value;
                    NewPrice = discountResponse.Discount.NewPrice.Value;
                    Discount = discountResponse.Discount.Discount.Value;
                }
                else
                {
                    response.ModelState.Add(ObjectExtensions.GetPropertyName(() => webApiProduct.ProductOldPrice), "An error has occurred.");
                    response.ModelState.Add(ObjectExtensions.GetPropertyName(() => webApiProduct.ProductNewPrice), "An error has occurred.");
                    response.ModelState.Add(ObjectExtensions.GetPropertyName(() => webApiProduct.DiscountPercentage), "An error has occurred.");
                }

                if (product.ProductNewPrice > product.ProductOldPrice)
                {
                    response.ModelState.Add(ObjectExtensions.GetPropertyName(() => webApiProduct.ProductNewPrice), "New price has to be a discount.");
                }

                DateTime discountDateEnd = DateTime.Parse(product.DiscountDateEnd);
                DateTime discountDateBegin = DateTime.Parse(product.DiscountDateBegin);

                if (DateTime.Compare(discountDateBegin, discountDateEnd) >= 0)
                {
                    response.ModelState.Add(ObjectExtensions.GetPropertyName(() => webApiProduct.DiscountDateEnd), "Discount end date cannot be earlier or same as discount begin date!");
                }
            }
            catch (Exception)
            {

                throw;
            }

            return response;
        }
    }
}