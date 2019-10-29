using AbatementHelper.CommonModels.Models;
using AbatementHelper.MVC.Models;
using AbatementHelper.MVC.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AbatementHelper.MVC.Controllers
{
    public class ProductManagerController : Controller
    {
        public async Task<ActionResult> ProductSaved()
        {
            var product = new Product()
            {
                ProductName = "test1",
                CompanyName = "test2",
                StoreName = "test3",
                ProductOldPrice = "test4",
                ProductNewPrice = "test5",
                ProductAbatementDateBegin = DateTime.Now,
                ProductAbatementDateEnd = DateTime.Now,
                ProductNote = "test6"
            };

            var success = await ProductManagerProcessor.ProcessProduct(product);

            if (success)
                ViewBag.Message = "Product Saved";
            else
                ViewBag.Message = "Product Not Saved";

            return View();
        }
    }
}