using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.REST.Product;
using DiscountCatalog.WebAPI.REST.Store;
using DiscountCatalog.WebAPI.REST.StoreAdmin;
using DiscountCatalog.WebAPI.Service.Contractor;
using DiscountCatalog.WebAPI.Service.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DiscountCatalog.WebAPI.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        readonly IUserService userService;
        readonly IProductService productService;

        public UserController()
        {
            userService = new UserService();
            productService = new ProductService();
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public IHttpActionResult GetAllProducts(string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter, bool includeUpcoming)
        {
            IPagingList<ProductREST> products = userService.GetAllProducts(sortOrder, searchString, pageIndex, pageSize, priceFilter, dateFilter, includeUpcoming);

            return Ok(products);
        }

        [HttpGet]
        [Route("GetProduct/{productId}")]
        public IHttpActionResult GetProduct(string productId)
        {
            ProductREST product = userService.GetProduct(productId);

            return Ok(product);
        }

        [HttpGet]
        [Route("GetStore/{storeId}")]
        public IHttpActionResult GetStore(string storeId)
        {
            StoreREST store = userService.GetStore(storeId);

            return Ok(store);
        }

        [HttpGet]
        [Route("GetStoreProducts/{storeId}")]
        public IHttpActionResult GetStoreProducts(string storeId, string sortOrder, string searchString, int pageIndex, int pageSize, string priceFilter, string dateFilter, bool includeUpcoming)
        {
            IPagingList<ProductREST> products = productService.GetAll(storeId, sortOrder, searchString, pageIndex, pageSize, priceFilter, dateFilter, includeUpcoming);

            return Ok(products);
        }


        [HttpGet]
        [Route("GetStoreAdmin/{storeAdminIdentityId}")]
        public IHttpActionResult GetStoreAdmin(string storeAdminIdentityId)
        {
            StoreAdminREST storeAdmin = userService.GetStoreAdmin(storeAdminIdentityId);

            return Ok(storeAdmin);
        }

        [HttpGet]
        [Route("GetMinPrice")]
        public IHttpActionResult GetMinPrice()
        {
            decimal min = userService.GetMinPrice();

            return Ok(min);
        }

        [HttpGet]
        [Route("GetMaxPrice")]
        public IHttpActionResult GetMaxPrice()
        {
            decimal max = userService.GetMaxPrice();

            return Ok(max);
        }
    }
}