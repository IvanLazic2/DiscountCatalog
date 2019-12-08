using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.Processors;
using AbatementHelper.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AbatementHelper.WebAPI.Controllers
{
    [Authorize(Roles = "StoreAdmin")]
    [RoutePrefix("api/StoreAdmin")]
    public class StoreAdminController : ApiController
    {
        private DataBaseEntityRepository entityReader = new DataBaseEntityRepository();

        [HttpGet]
        [Route("GetAllStores/{storeAdminId}")]
        public List<WebApiStore> GetAllStores(string storeAdminId)
        {
            var stores = entityReader.GetAllStores(storeAdminId);

            return stores;
        }

        [HttpPost]
        [Route("CreateStore")]
        public Response CreateStore(WebApiStore store)
        {
            return entityReader.CreateStore(store); ;
        }

        [HttpGet]
        [Route("EditStore/{id}")]
        public async Task<WebApiStore> EditStore(string id)
        {
            var store = await entityReader.ReadStoreById(id);

            return StoreProcessor.StoreEntityToWebApiStore(store);
        }

        [HttpPut]
        [Route("EditStore")]
        public Response EditStore(WebApiStore store)
        {
            return entityReader.EditStore(store);
        }

        [HttpGet]
        [Route("DetailsStore/{id}")]
        public async Task<WebApiStore> DetailsStore(string id)
        {
            WebApiStore store = StoreProcessor.StoreEntityToWebApiStore(await entityReader.ReadStoreById(id));

            return store;
        }

        [HttpPut]
        [Route("DeleteStore/{id}")]
        public IHttpActionResult DeleteStore(string id)
        {
            entityReader.DeleteStore(id);

            return Ok();
        }

        [HttpGet]
        [Route("GetAllDeletedStores/{storeAdminId}")]
        public List<WebApiStore> GetAllDeletedStores(string storeAdminId)
        {
            var stores = entityReader.GetAllDeletedStores(storeAdminId);

            return stores;
        }

        [HttpPut]
        [Route("RestoreStore/{id}")]
        public IHttpActionResult RestoreStore(string id)
        {
            entityReader.RestoreStore(id);

            return Ok();
        }

        [HttpGet]
        [Route("Select/{id}")]
        public SelectedStore Select(string id)
        {
            return entityReader.SelectStore(id);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("CreateManager")]
        public IHttpActionResult CreateManager()
        {
            entityReader.CreateManager();

            return Ok();
        }
    }
}