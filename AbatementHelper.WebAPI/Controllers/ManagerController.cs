using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private ManagerRepository managerRepository = new ManagerRepository();

        private void SimulateValidation(object model)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }

        [HttpGet]
        [Route("GetAllStoresAsync/{managerId}")]
        public async Task<List<WebApiStore>> GetAllStoresAsync(string managerId)
        {
            var stores = await managerRepository.GetAllStoresAsync(managerId);

            return stores;
        }

        [HttpGet]
        [Route("SelectAsync/{id}")]
        public async Task<SelectedStore> SelectAsync(string id)
        {
            return await managerRepository.SelectStoreAsync(id);
        }

        [HttpGet]
        [Route("EditStoreAsync/{id}")]
        public async Task<WebApiStore> EditStoreAsync(string id)
        {
            var store = await managerRepository.ReadStoreByIdAsync(id);

            return store;
        }

        [HttpPut]
        [Route("EditStoreAsync")]
        public async Task<IHttpActionResult> EditStoreAsync(WebApiStore store)
        {
            SimulateValidation(store);

            ModelStateResponse response = await managerRepository.EditStoreAsync(store);

            if (!response.IsValid)
            {
                foreach (var error in response.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpGet]
        [Route("DetailsStoreAsync/{id}")]
        public async Task<WebApiStore> DetailsStoreAsync(string id)
        {
            WebApiStore store = await managerRepository.ReadStoreByIdAsync(id);

            return store;
        }

        [HttpPost]
        [Route("AbandonStoreAsync")]
        public async Task<Response> AbandonStoreAsync(WebApiStoreAssign storeUnassign)
        {
            Response response = await managerRepository.UnassignStoreAsync(storeUnassign);

            return response;
        }
    }
}