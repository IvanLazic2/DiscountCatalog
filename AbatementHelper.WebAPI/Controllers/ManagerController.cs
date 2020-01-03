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
        [Route("GetAllStoresAsync/{managerId}")]
        public async Task<List<WebApiStore>> GetAllStoresAsync(string managerId)
        {
            var stores = await manager.GetAllStoresAsync(managerId);

            return stores;
        }

        [HttpGet]
        [Route("SelectAsync/{id}")]
        public async Task<SelectedStore> SelectAsync(string id)
        {
            return await manager.SelectStoreAsync(id);
        }

        [HttpGet]
        [Route("EditStoreAsync/{id}")]
        public async Task<WebApiStore> EditStoreAsync(string id)
        {
            var store = await manager.ReadStoreByIdAsync(id);

            return store;
        }

        [HttpPut]
        [Route("EditStoreAsync")]
        public async Task<Response> EditStoreAsync(WebApiStore store)
        {
            return await manager.EditStoreAsync(store);
        }

        [HttpGet]
        [Route("DetailsStoreAsync/{id}")]
        public async Task<WebApiStore> DetailsStoreAsync(string id)
        {
            WebApiStore store = await manager.ReadStoreByIdAsync(id);

            return store;
        }

        [HttpPost]
        [Route("AbandonStoreAsync")]
        public async Task<Response> AbandonStoreAsync(WebApiStoreAssign storeUnassign)
        {
            Response response = await manager.UnassignStoreAsync(storeUnassign);

            return response;
        }
    }
}