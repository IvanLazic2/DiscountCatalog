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
        [Route("GetAllStoresAsync/{storeAdminId}")]
        public async Task<List<WebApiStore>> GetAllStoresAsync(string storeAdminId)
        {
            var stores = await storeAdminRepository.GetAllStoresAsync(storeAdminId);

            return stores;
        }

        [HttpPost]
        [Route("CreateStoreAsync")]
        public async Task<Response> CreateStoreAsync(WebApiStore store)
        {
            return await storeAdminRepository.CreateStoreAsync(store); ;
        }

        [HttpGet]
        [Route("EditStoreAsync/{id}")]
        public async Task<WebApiStore> EditStore(string id)
        {
            var store = await storeAdminRepository.ReadStoreByIdAsync(id);

            var processedStore = await StoreProcessor.StoreEntityToWebApiStoreAsync(store);

            return processedStore;
        }

        [HttpPut]
        [Route("EditStoreAsync")]
        public async Task<Response> EditStoreAsync(WebApiStore store)
        {
            return await storeAdminRepository.EditStoreAsync(store);
        }

        [HttpPut]
        [Route("PostStoreImageAsync")]
        public async Task<Response> PostStoreImageAsync(WebApiPostImage store)
        {
            Response response = await storeAdminRepository.PostStoreImageAsync(store);

            return response;
        }

        [HttpGet]
        [Route("GetStoreImageAsync/{id}")]
        public async Task<byte[]> GetStoreImageAsync(string id)
        {
            byte[] byteArray = await storeAdminRepository.GetStoreImageAsync(id);

            return ImageProcessor.CreateThumbnail(byteArray);
        }

        [HttpGet]
        [Route("DetailsStoreAsync/{id}")]
        public async Task<WebApiStore> DetailsStore(string id)
        {
            StoreEntity storeEntity = await storeAdminRepository.ReadStoreByIdAsync(id);

            WebApiStore store = await StoreProcessor.StoreEntityToWebApiStoreAsync(storeEntity);

            return store;
        }

        [HttpPut]
        [Route("DeleteStoreAsync/{id}")]
        public async Task<IHttpActionResult> DeleteStoreAsync(string id)
        {
            await storeAdminRepository.DeleteStoreAsync(id);

            return Ok();
        }

        [HttpGet]
        [Route("GetAllDeletedStoresAsync/{storeAdminId}")]
        public async Task<List<WebApiStore>> GetAllDeletedStoresAsync(string storeAdminId)
        {
            var stores = await storeAdminRepository.GetAllDeletedStoresAsync(storeAdminId);

            return stores;
        }

        [HttpPut]
        [Route("RestoreStoreAsync/{id}")]
        public async Task<IHttpActionResult> RestoreStoreAsync(string id)
        {
            await storeAdminRepository.RestoreStoreAsync(id);

            return Ok();
        }

        [HttpGet]
        [Route("SelectAsync/{id}")]
        public async Task<SelectedStore> SelectAsync(string id)
        {
            return await storeAdminRepository.SelectStoreAsync(id);
        }

        [HttpGet]
        [Route("GetAllManagersAsync/{storeAdminId}")]
        public async Task<List<WebApiManager>> GetAllManagersAsync(string storeAdminId)
        {
            List<WebApiManager> managers = await storeAdminRepository.GetAllManagersAsync(storeAdminId);

            return managers;
        }


        [HttpPost]
        [Route("CreateManagerAsync")]
        public async Task<Response> CreateManagerAsync(CreateManagerModel manager)
        {
            return await storeAdminRepository.CreateManagerAsync(manager, manager.Password);
        }

        [HttpGet]
        [Route("DetailsManagerAsync/{id}")]
        public async Task<WebApiUser> DetailsManager(string id)
        {
            ApplicationUser user = await storeAdminRepository.ReadUserById(id);

            WebApiUser webApiUser = await UserProcessor.ApplicationUserToWebApiUser(user);

            return webApiUser;
        }

        [HttpGet]
        [Route("EditManagerAsync/{id}")]
        public async Task<WebApiUser> EditManager(string id)
        {
            ApplicationUser user = await storeAdminRepository.ReadUserById(id);

            WebApiUser webApiUser = await UserProcessor.ApplicationUserToWebApiUser(user);

            return webApiUser;
        }

        [HttpPut]
        [Route("EditManagerAsync")]
        public async Task<Response> EditManager(WebApiManager manager)
        {
            return await storeAdminRepository.EditManager(manager);
        }

        [HttpPut]
        [Route("PostManagerImageAsync")]
        public async Task<Response> PostManagerImageAsync(WebApiPostImage manager)
        {
            Response response = await storeAdminRepository.PostManagerImageAsync(manager);

            return response;
        }

        [HttpGet]
        [Route("GetManagerImageAsync/{id}")]
        public async Task<byte[]> GetManagerImageAsync(string id)
        {
            byte[] byteArray = await storeAdminRepository.GetManagerImageAsync(id);

            return ImageProcessor.CreateThumbnail(byteArray);
        }

        [HttpPut]
        [Route("DeleteManagerAsync/{id}")]
        public async Task<IHttpActionResult> DeleteManagerAsync(string id)
        {
            await storeAdminRepository.DeleteManagerAsync(id);

            return Ok();
        }

        [HttpGet]
        [Route("GetAllDeletedManagersAsync/{storeAdminId}")]
        public async Task<List<WebApiManager>> GetAllDeletedManagersAsync(string storeAdminId)
        {
            return await storeAdminRepository.GetAllDeletedManagersAsync(storeAdminId);
        }

        [HttpPut]
        [Route("RestoreManagerAsync/{id}")]
        public async Task<IHttpActionResult> RestoreManagerAsync(string id)
        {
            await storeAdminRepository.RestoreManagerAsync(id);

            return Ok();
        }

        [HttpGet]
        [Route("GetAllManagerStoresAsync/{id}")]
        public async Task<List<WebApiManagerStore>> GetAllAssignedStoresAsync(string id)
        {
            return await storeAdminRepository.GetAllManagerStoresAsync(id);
        }

        [HttpPost]
        [Route("AssignStoreAsync")]
        public async Task<Response> AssignStoreAsync(WebApiStoreAssign storeAssign)
        {
            Response response = await storeAdminRepository.AssignStoreAsync(storeAssign);

            return response;
        }

        [HttpPost]
        [Route("UnassignStoreAsync")]
        public async Task<Response> UnassignStoreAsync(WebApiStoreAssign storeUnassign)
        {
            Response response = await storeAdminRepository.UnassignStoreAsync(storeUnassign);

            return response;
        }
    }
}