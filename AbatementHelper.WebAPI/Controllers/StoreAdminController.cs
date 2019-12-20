using AbatementHelper.CommonModels.CreateModels;
using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
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
    //[Authorize(Roles = "StoreAdmin")]
    [RoutePrefix("api/StoreAdmin")]
    public class StoreAdminController : ApiController
    {
        private StoreAdminRepository storeAdminRepository = new StoreAdminRepository();

        [HttpGet]
        [Route("GetAllStores/{storeAdminId}")]
        public List<WebApiStore> GetAllStores(string storeAdminId)
        {
            var stores = storeAdminRepository.GetAllStores(storeAdminId);

            return stores;
        }

        [HttpPost]
        [Route("CreateStore")]
        public Response CreateStore(WebApiStore store)
        {
            return storeAdminRepository.CreateStore(store); ;
        }

        [HttpGet]
        [Route("EditStore/{id}")]
        public async Task<WebApiStore> EditStore(string id)
        {
            var store = await storeAdminRepository.ReadStoreById(id);

            var processedStore = StoreProcessor.StoreEntityToWebApiStore(store);

            return processedStore;
        }

        [HttpPut]
        [Route("EditStore")]
        public Response EditStore(WebApiStore store)
        {
            return storeAdminRepository.EditStore(store);
        }

        [HttpGet]
        [Route("DetailsStore/{id}")]
        public async Task<WebApiStore> DetailsStore(string id)
        {
            WebApiStore store = StoreProcessor.StoreEntityToWebApiStore(await storeAdminRepository.ReadStoreById(id));

            return store;
        }

        [HttpPut]
        [Route("DeleteStore/{id}")]
        public IHttpActionResult DeleteStore(string id)
        {
            storeAdminRepository.DeleteStore(id);

            return Ok();
        }

        [HttpGet]
        [Route("GetAllDeletedStores/{storeAdminId}")]
        public List<WebApiStore> GetAllDeletedStores(string storeAdminId)
        {
            var stores = storeAdminRepository.GetAllDeletedStores(storeAdminId);

            return stores;
        }

        [HttpPut]
        [Route("RestoreStore/{id}")]
        public IHttpActionResult RestoreStore(string id)
        {
            storeAdminRepository.RestoreStore(id);

            return Ok();
        }

        [HttpGet]
        [Route("Select/{id}")]
        public SelectedStore Select(string id)
        {
            return storeAdminRepository.SelectStore(id);
        }

        [HttpGet]
        [Route("GetAllManagers/{storeAdminId}")]
        public List<WebApiManager> GetAllManagers(string storeAdminId)
        {
            List<WebApiManager> managers = storeAdminRepository.GetAllManagers(storeAdminId);

            return managers;
        }


        [HttpPost]
        [Route("CreateManager")]
        public Response CreateManager(CreateManagerModel manager)
        {
            return storeAdminRepository.CreateManager(manager, manager.Password);
        }

        [HttpGet]
        [Route("DetailsManager/{id}")]
        public async Task<WebApiUser> DetailsManager(string id)
        {
            return UserProcessor.ApplicationUserToWebApiUser(await storeAdminRepository.ReadUserById(id));
        }

        [HttpGet]
        [Route("EditManager/{id}")]
        public async Task<WebApiUser> EditManager(string id)
        {
            return UserProcessor.ApplicationUserToWebApiUser(await storeAdminRepository.ReadUserById(id));
        }

        [HttpPut]
        [Route("EditManager")]
        public async Task<Response> EditManager(WebApiManager manager)
        {
            return await storeAdminRepository.EditManager(manager);
        }

        [HttpPut]
        [Route("DeleteManager/{id}")]
        public IHttpActionResult DeleteManager(string id)
        {
            storeAdminRepository.DeleteManager(id);

            return Ok();
        }

        [HttpGet]
        [Route("GetAllDeletedManagers/{storeAdminId}")]
        public List<WebApiManager> GetAllDeletedManagers(string storeAdminId)
        {
            return storeAdminRepository.GetAllDeletedManagers(storeAdminId);
        }

        [HttpPut]
        [Route("RestoreManager/{id}")]
        public IHttpActionResult RestoreManager(string id)
        {
            storeAdminRepository.RestoreManager(id);

            return Ok();
        }

        [HttpGet]
        [Route("GetAllManagerStores/{id}")]
        public List<WebApiManagerStore> GetAllAssignedStores(string id)
        {
            return storeAdminRepository.GetAllManagerStores(id);
        }

        [HttpPost]
        [Route("AssignStore")]
        public Response AssignStore(WebApiStoreAssign storeAssign)
        {
            Response response = storeAdminRepository.AssignStore(storeAssign);

            return response;
        }

        [HttpPost]
        [Route("UnassignStore")]
        public Response UnassignStore(WebApiStoreAssign storeUnassign)
        {
            Response response = storeAdminRepository.UnassignStore(storeUnassign);

            return response;
        }
    }
}