//using DiscountCatalog.Common.Models;
//using DiscountCatalog.Common.WebApiModels;
//using DiscountCatalog.WebAPI.Extentions;
//using DiscountCatalog.WebAPI.Models;
//using DiscountCatalog.WebAPI.Models.Entities;
//using DiscountCatalog.WebAPI.ModelState;
//using DiscountCatalog.WebAPI.REST.Product;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Web;

//namespace DiscountCatalog.WebAPI.Validators
//{
//    public class PriceValidator //napravit decimal array i proci i vidit koji su prazni, ako je samo jedan prazan onda sam provjeravam koji, ako su svi popunjeni samo postotak promijenim ili new price to cu jos vidit.
//    {
//        public EntityModelState ModelState { get; private set; }

//        public PriceValidator()
//        {
//            ModelState = new EntityModelState();
//        }

//        public ProductEntity CalculatePrice(ProductEntity product)
//        {
//            decimal?[] valueArray = new decimal?[]
//            {
//                product.OldPrice,
//                product.OldPrice,
//                product.DiscountPercentage
//            };

//            IEnumerable<decimal?> emptyValues = valueArray.Where(p => !p.HasValue);

//            if (emptyValues.Count() < 2)
//            {
//                if (!product.OldPrice.HasValue)
//                {
//                    product.OldPrice = Math.Round(product.NewPrice.Value / (1 - product.DiscountPercentage.Value / 100), 2);
//                }
//                if (!product.NewPrice.HasValue)
//                {
//                    product.NewPrice = Math.Round(product.OldPrice.Value - (product.DiscountPercentage.Value / 100 * product.OldPrice.Value), 2);
//                }
//                if (!product.DiscountPercentage.HasValue)
//                {
//                    product.DiscountPercentage = Math.Round(100 - (product.NewPrice.Value / product.OldPrice.Value) * 100, 0);
//                }
//            }
//            if (emptyValues.Count() == 0)
//            {
//                product.NewPrice = Math.Round(product.OldPrice.Value - (product.DiscountPercentage.Value / 100 * product.OldPrice.Value), 2);
//            }

//            return product;
//        }






//        public PriceValidatorResult GetErrors(WebApiProduct product)
//        {
//            var result = new PriceValidatorResult();

//            if (product.ProductOldPrice != null)
//            {
//                if (!decimal.TryParse(product.ProductOldPrice, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out decimal oldPrice))
//                {
//                    result.Errors.Add(ObjectExtensions.GetPropertyName(() => product.ProductOldPrice), "Old price has to be a number.");
//                }
//                else
//                {
//                    result.OldPrice = oldPrice;
//                }
//            }
//            else
//            {
//                product.OldPrice = null;
//            }

//            if (product.ProductNewPrice != null)
//            {
//                if (!decimal.TryParse(product.ProductNewPrice, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out decimal newPrice))
//                {
//                    result.Errors.Add(ObjectExtensions.GetPropertyName(() => product.ProductNewPrice), "New price has to be a number.");
//                }
//                else
//                {
//                    result.NewPrice = newPrice;
//                }
//            }
//            else
//            {
//                product.NewPrice = null;
//            }

//            if (product.DiscountPercentage != null)
//            {
//                if (!decimal.TryParse(product.DiscountPercentage, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out decimal discount))
//                {
//                    result.Errors.Add(ObjectExtensions.GetPropertyName(() => product.DiscountPercentage), "Discount has to be a number.");
//                }
//                else
//                {
//                    result.Discount = discount;
//                }
//            }
//            else
//            {
//                product.Discount = null;
//            }

//            return result;
//        }
//    }
//}