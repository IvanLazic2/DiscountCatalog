using AbatementHelper.CommonModels.CreateModels;
using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Models.Result;
using AbatementHelper.WebAPI.Processors;
using AbatementHelper.WebAPI.Repositories;
using AbatementHelper.WebAPI.Validators;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        private void SimulateValidation(object model)
        {
            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }

        [HttpGet]
        [Route("GetAllStoresAsync/{storeAdminId}")]
        public async Task<WebApiListOfStoresResult> GetAllStoresAsync(string storeAdminId)
        {
            WebApiListOfStoresResult result = await storeAdminRepository.GetAllStoresAsync(storeAdminId);

            return result;
        }

        [HttpPost]
        [Route("CreateStoreAsync")]
        public async Task<WebApiResult> CreateStoreAsync(WebApiStore store)
        {
            //SimulateValidation(store);

            WebApiResult result = await storeAdminRepository.CreateStoreAsync(store);

            return result;
        }

        [HttpGet]
        [Route("EditStoreAsync/{id}")]
        public async Task<WebApiStoreResult> EditStore(string id)
        {
            WebApiStoreResult result = await storeAdminRepository.ReadStoreByIdAsync(id);

            return result;
        }

        [HttpPut]
        [Route("EditStoreAsync")]
        public async Task<WebApiResult> EditStoreAsync(WebApiStore store)
        {
            WebApiResult result = await storeAdminRepository.EditStoreAsync(store);

            return result;
        }

        [HttpPut]
        [Route("PostStoreImageAsync")]
        public async Task<WebApiResult> PostStoreImageAsync(WebApiPostImage store)
        {
            WebApiResult result = await storeAdminRepository.PostStoreImageAsync(store);

            return result;
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
        public async Task<WebApiStoreResult> DetailsStore(string id)
        {
            WebApiStoreResult result = await storeAdminRepository.ReadStoreByIdAsync(id);

            return result;
        }

        [HttpPut]
        [Route("DeleteStoreAsync/{id}")]
        public async Task<WebApiResult> DeleteStoreAsync(string id)
        {
            WebApiResult result = await storeAdminRepository.DeleteStoreAsync(id);

            return result;
        }

        [HttpGet]
        [Route("GetAllDeletedStoresAsync/{storeAdminId}")]
        public async Task<WebApiListOfStoresResult> GetAllDeletedStoresAsync(string storeAdminId)
        {
            WebApiListOfStoresResult result = await storeAdminRepository.GetAllDeletedStoresAsync(storeAdminId);

            return result;
        }

        [HttpPut]
        [Route("RestoreStoreAsync/{id}")]
        public async Task<WebApiResult> RestoreStoreAsync(string id)
        {
            WebApiResult result = await storeAdminRepository.RestoreStoreAsync(id);

            return result;
        }

        [HttpGet]
        [Route("SelectAsync/{id}")]
        public async Task<SelectedStore> SelectAsync(string id)
        {
            return await storeAdminRepository.SelectStoreAsync(id);
        }

        [HttpGet]
        [Route("GetAllManagersAsync/{storeAdminId}")]
        public async Task<WebApiListOfManagersResult> GetAllManagersAsync(string storeAdminId)
        {
            WebApiListOfManagersResult result = await storeAdminRepository.GetAllManagersAsync(storeAdminId);

            return result;
        }


        [HttpPost]
        [Route("CreateManagerAsync")]
        public async Task<WebApiResult> CreateManagerAsync(CreateManagerModel manager)
        {
            //SimulateValidation(manager);

            WebApiResult result = await storeAdminRepository.CreateManagerAsync(manager, manager.Password);

            return result;
        }

        [HttpGet]
        [Route("DetailsManagerAsync/{id}")]
        public async Task<WebApiUserResult> DetailsManager(string id)
        {
            WebApiUserResult result = await storeAdminRepository.ReadUserById(id);

            return result;
        }

        [HttpGet]
        [Route("EditManagerAsync/{id}")]
        public async Task<WebApiUserResult> EditManager(string id)
        {
            WebApiUserResult result = await storeAdminRepository.ReadUserById(id);

            return result;
        }

        [HttpPut]
        [Route("EditManagerAsync")]
        public async Task<WebApiResult> EditManager(WebApiManager manager)
        {
            //SimulateValidation(manager);

            WebApiResult result = await storeAdminRepository.EditManager(manager);

            return result;
        }

        [HttpPut]
        [Route("PostManagerImageAsync")]
        public async Task<WebApiResult> PostManagerImageAsync(WebApiPostImage manager)
        {
            WebApiResult result = await storeAdminRepository.PostManagerImageAsync(manager);

            return result;
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
        public async Task<WebApiResult> DeleteManagerAsync(string id)
        {
            WebApiResult result = await storeAdminRepository.DeleteManagerAsync(id);

            return result;
        }

        [HttpGet]
        [Route("GetAllDeletedManagersAsync/{storeAdminId}")]
        public async Task<WebApiListOfManagersResult> GetAllDeletedManagersAsync(string storeAdminId)
        {
            WebApiListOfManagersResult result = await storeAdminRepository.GetAllDeletedManagersAsync(storeAdminId);

            return result;
        }

        [HttpPut]
        [Route("RestoreManagerAsync/{id}")]
        public async Task<WebApiResult> RestoreManagerAsync(string id)
        {
            WebApiResult result = await storeAdminRepository.RestoreManagerAsync(id);

            return result;
        }

        [HttpGet]
        [Route("GetAllManagerStoresAsync/{id}")]
        public async Task<List<WebApiManagerStore>> GetAllAssignedStoresAsync(string id)
        {
            return await storeAdminRepository.GetAllManagerStoresAsync(id);
        }

        [HttpPost]
        [Route("AssignStoreAsync")]
        public async Task<WebApiResult> AssignStoreAsync(WebApiStoreAssign storeAssign)
        {
            WebApiResult result = await storeAdminRepository.AssignStoreAsync(storeAssign);

            return result;
        }

        [HttpPost]
        [Route("UnassignStoreAsync")]
        public async Task<WebApiResult> UnassignStoreAsync(WebApiStoreAssign storeUnassign)
        {
            WebApiResult result = await storeAdminRepository.UnassignStoreAsync(storeUnassign);

            return result;
        }
    }
}