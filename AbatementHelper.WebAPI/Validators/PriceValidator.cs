using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.Extentions;
using AbatementHelper.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AbatementHelper.WebAPI.Validators
{
    public class PriceValidator
    {
        public PriceValidatorResult GetErrors(WebApiProduct product)
        {
            var result = new PriceValidatorResult();

            if (product.ProductOldPrice != null)
            {
                if (!decimal.TryParse(product.ProductOldPrice, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out decimal oldPrice))
                {
                    result.Errors.Add(ObjectExtensions.GetPropertyName(() => product.ProductOldPrice), "Old price has to be a number.");
                }
                else
                {
                    result.OldPrice = oldPrice;
                }
            }
            else
            {
                product.OldPrice = null;
            }

            if (product.ProductNewPrice != null)
            {
                if (!decimal.TryParse(product.ProductNewPrice, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out decimal newPrice))
                {
                    result.Errors.Add(ObjectExtensions.GetPropertyName(() => product.ProductNewPrice), "New price has to be a number.");
                }
                else
                {
                    result.NewPrice = newPrice;
                }
            }
            else
            {
                product.NewPrice = null;
            }

            if (product.DiscountPercentage != null)
            {
                if (!decimal.TryParse(product.DiscountPercentage, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out decimal discount))
                {
                    result.Errors.Add(ObjectExtensions.GetPropertyName(() => product.DiscountPercentage), "Discount has to be a number.");
                }
                else
                {
                    result.Discount = discount;
                }
            }
            else
            {
                product.Discount = null;
            }

            if (result.Errors.Count > 0)
            {
                result.Success = false;
            }
            else
            {
                result.Success = true;
            }

            return result;
        }
    }
}