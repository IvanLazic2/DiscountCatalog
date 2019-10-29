using AbatementHelper.CommonModels.Models;
using AbatementHelper.MVC.Models;
using AbatementHelper.MVC.Repositeories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.MVC.Processors
{
    public class ProductManagerProcessor
    {
        public static async Task<bool> ProcessProduct(Product product)
        {
            //procesiranje, kalkuliranje, formatiranje i ostala sranja

            return await ProductManagerRepository.SaveProduct(product);
        }
    }
}
