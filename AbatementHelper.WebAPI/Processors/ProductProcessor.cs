using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Processors
{
    public class ProductProcessor
    {
        public static bool ProcessProduct(Product product)
        {
            return ProductRepository.AddProductToDataBase(product);
        }

        //public static bool ProcessGetProduct(Product product)
        //{


        //    return ProductRepository.GetProductsFromDataBase(product);
        //}
    }
}