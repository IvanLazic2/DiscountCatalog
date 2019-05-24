using AbatementHelper.Classes.Models;
using AbatementHelper.Classes.Repositories;
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
            //procesiranje, validacija, formatiranje i ostala sranja

            return ProductRepository.AddProductToDataBase(product);
        }
    }
}