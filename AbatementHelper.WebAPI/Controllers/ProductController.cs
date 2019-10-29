using AbatementHelper.CommonModels.Models;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AbatementHelper.WebAPI.Controllers
{
    public class ProductController : ApiController
    {
        // POST: api/Product
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("SaveProduct")]
        [Authorize/*(Roles = "Manager")*/]
        public async Task<IHttpActionResult> SaveProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest("Product info is empty!");
            }

            bool success = ProductProcessor.ProcessProduct(product);

            if (success)
            {
                return Ok("Product saved to database.");
            }

            return BadRequest("Product not saved to database.");

        }

        public async Task<IHttpActionResult> GetProducts(bool isExpired = false, int userId = 0)
        {

            // kod
            return Ok();
        }
    }
}
