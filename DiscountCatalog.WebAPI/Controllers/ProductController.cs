using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Service.Contractor;
using DiscountCatalog.WebAPI.Service.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DiscountCatalog.WebAPI.Controllers
{
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {
        private IList<string> currencies = new List<string>()
        {
            "kn",
            "$",
            "€",
            "£"
        };

        private IList<string> measuringUnits = new List<string>()
        {
            "kg",
            "g",
            "l",
            "ml",
            "cm",
            "m"
        };

        private readonly IProductService productService;

        public ProductController()
        {
            productService = new ProductService();
        }

        [HttpGet]
        [Route("GetMinPrice/{storeId}")]
        public IHttpActionResult GetMinPrice(string storeId)
        {
            decimal min = productService.GetMinPrice(storeId);

            return Ok(min);
        }

        [HttpGet]
        [Route("GetMaxPrice/{storeId}")]
        public IHttpActionResult GetMaxPrice(string storeId)
        {
            decimal max = productService.GetMaxPrice(storeId);

            return Ok(max);
        }

        [HttpGet]
        [Route("GetAllCurrencies")]
        public IHttpActionResult GetAllCurrencies()
        {
            return Ok(currencies);
        }

        [HttpGet]
        [Route("GetAllMeasuringUnits")]
        public IHttpActionResult GetAllMeasuringUnits()
        {
            return Ok(measuringUnits);
        }
    }
}