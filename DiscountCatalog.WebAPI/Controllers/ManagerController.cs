using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Paging.Contractor;
using DiscountCatalog.WebAPI.Repositories;
using DiscountCatalog.WebAPI.REST.Manager;
using DiscountCatalog.WebAPI.REST.Store;
using DiscountCatalog.WebAPI.Service.Contractor;
using DiscountCatalog.WebAPI.Service.Implementation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DiscountCatalog.WebAPI.Controllers
{
    [Authorize(Roles = "Manager")]
    [RoutePrefix("api/Manager")]
    public class ManagerController : ApiController
    {
        private readonly IStoreService storeService;

        public ManagerController()
        {
            storeService = new StoreService();
        }

        [HttpGet]
        [Route("GetAllStores/{managerIdentityId}")]
        public  IHttpActionResult GetAllStores(string managerIdentityId, string sortOrder, string searchString, int pageIndex, int pageSize)
        {
            IPagingList<StoreREST> stores = storeService.GetAll(managerIdentityId, string.Empty, sortOrder, searchString, pageIndex, pageSize);

            return Ok(stores);
        }

        [HttpPut]
        [Route("EditStore/{managerIdentityId}")]
        public async Task<IHttpActionResult> EditStore(string managerIdentityId, StoreRESTPut store)
        {
            Result result = await storeService.UpdateAsync(managerIdentityId, string.Empty, store);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetStore/{managerIdentityId}")]
        public IHttpActionResult GetStore(string managerIdentityId, string storeId)
        {
            StoreREST store = storeService.Get(managerIdentityId, string.Empty, storeId);

            return Ok(store);
        }

        [HttpPut]
        [Route("PostStoreImage/{managerIdentityId}")]
        public IHttpActionResult PostStoreImage(string managerIdentityId, string storeId, byte[] image)
        {
            Result result = storeService.PostImage(managerIdentityId, string.Empty, storeId, image);

            if (result.Success)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPost]
        [Route("AbandonStore/{managerId}")]
        public IHttpActionResult AbandonStore(string managerIdentityId, string storeId)
        {
            return Ok();
        } 
    }
}