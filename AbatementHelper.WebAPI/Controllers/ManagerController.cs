using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AbatementHelper.WebAPI.Controllers
{
    //[Authorize(Roles = "Manager")]
    [RoutePrefix("api/Manager")]
    public class ManagerController : ApiController
    {
        ManagerRepository manager = new ManagerRepository();

        [HttpGet]
        [Route("GetAllStores/{managerId}")]
        public List<WebApiStore> GetAllStores(string managerId)
        {
            var stores = manager.GetAllStores(managerId);

            return stores;
        }

        [HttpGet]
        [Route("Select/{id}")]
        public SelectedStore Select(string id)
        {
            return manager.SelectStore(id);

        }

        [HttpGet]
        [Route("EditStore/{id}")]
        public async Task<WebApiStore> EditStore(string id)
        {
            var store = await manager.ReadStoreById(id);

            return store;
        }

        [HttpPut]
        [Route("EditStore")]
        public Response EditStore(WebApiStore store)
        {
            return manager.EditStore(store);
        }

        [HttpGet]
        [Route("DetailsStore/{id}")]
        public async Task<WebApiStore> DetailsStore(string id)
        {
            WebApiStore store = await manager.ReadStoreById(id);

            return store;
        }

        [HttpPost]
        [Route("AbandonStore")]
        public Response AbandonStore(WebApiStoreAssign storeUnassign)
        {
            Response response = manager.UnassignStore(storeUnassign);

            return response;
        }
    }
}