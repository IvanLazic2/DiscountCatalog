using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.MVC.Extensions;
using DiscountCatalog.MVC.Repositories;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DiscountCatalog.MVC.Controllers
{
    public class HomeController : Controller
    {
        private HomeRepository HomeRepository = new HomeRepository();

        public ActionResult Index()
        {
            //TempData["Role"] = 

            return View();
        }

        //[Route("GetAllProducts")]
        //public async Task<ActionResult> GetAllProducts(string sortOrder, string CurrentFilter, string searchString, int? page)
        //{
        //    ViewBag.CurrentSort = sortOrder;

        //    ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";

        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = CurrentFilter;
        //    }

        //    ViewBag.CurrentFilter = searchString;

        //    WebApiListOfProductsResult result = await HomeRepository.GetAllProductsAsync();

        //    if (!result.Success)
        //    {
        //        foreach (var error in result.ModelState)
        //        {
        //            ModelState.AddModelError(error.Key, error.Value);
        //        }
        //    }

        //    List<WebApiProduct> products = result.Products;

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        products = products.Where(u => u.ProductName.Contains(searchString, StringComparer.OrdinalIgnoreCase) ||
        //                                       u.CompanyName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
        //    }

        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            products = products.OrderByDescending(p => p.ProductName).ToList();
        //            break;
        //        case "price":
        //            products = products.OrderBy(p => p.ProductNewPrice).ToList();
        //            break;
        //        case "price_desc":
        //            products = products.OrderByDescending(p => p.ProductNewPrice).ToList();
        //            break;
        //        case "percentage":
        //            products = products.OrderBy(p => p.DiscountPercentage).ToList();
        //            break;
        //        case "percentage_desc":
        //            products = products.OrderByDescending(p => p.DiscountPercentage).ToList();
        //            break;
        //        default:
        //            products = products.OrderBy(p => p.ProductName).ToList();
        //            break;
        //    }

        //    int pageSize = 15;
        //    int pageNumber = (page ?? 1);

        //    return View(products.ToPagedList(pageNumber, pageSize));
        //}

        //getproductimage

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}