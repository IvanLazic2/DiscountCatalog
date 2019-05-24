using AbatementHelper.Classes.Models;
using AbatementHelper.WebAPI.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AbatementHelper.WebAPI.Controllers
{
    public class ProductController : ApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("SaveProduct")]
        // POST: api/Product
        public bool SaveProduct(Product product)
        {
            if (product == null)
            {
                return false;
            }

            return ProductProcessor.ProcessProduct(product);
        }
    }
}
