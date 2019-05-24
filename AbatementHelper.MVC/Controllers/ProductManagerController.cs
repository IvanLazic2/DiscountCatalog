using AbatementHelper.Classes.Models;
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
                ProductOldPrice = "test3",
                ProductNewPrice = "test4",
                ProductAbatementDateBegin = "2000-01-01",
                ProductAbatementDateEnd = "2000-01-01",
                ProductNote = "test7"
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