using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Processors
{
    public static class DiscountProcessor
    {
        public static ProductEntity CalculateDiscount(ProductEntity product)
        {
            decimal?[] valueArray = new decimal?[]
            {
                product.OldPrice,
                product.NewPrice,
                product.DiscountPercentage
            };

            IEnumerable<decimal?> emptyValues = valueArray.Where(p => !p.HasValue);

            if (emptyValues.Count() < 2)
            {
                if (!product.OldPrice.HasValue)
                {
                    product.OldPrice = Math.Round(product.NewPrice.Value / (1 - product.DiscountPercentage.Value / 100), 2);
                }
                if (!product.NewPrice.HasValue)
                {
                    product.NewPrice = Math.Round(product.OldPrice.Value - (product.DiscountPercentage.Value / 100 * product.OldPrice.Value), 2);
                }
                if (!product.DiscountPercentage.HasValue)
                {
                    product.DiscountPercentage = Math.Round(100 - (product.NewPrice.Value / product.OldPrice.Value) * 100, 0);
                }
            }
            if (emptyValues.Count() == 0)
            {
                product.NewPrice = Math.Round(product.OldPrice.Value - (product.DiscountPercentage.Value / 100 * product.OldPrice.Value), 2);
            }

            return product;
        }
    }
}