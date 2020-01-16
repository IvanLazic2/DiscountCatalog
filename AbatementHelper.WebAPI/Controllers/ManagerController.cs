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
        [Route("GetAllStoresAsync/{id}")]
        public async Task<WebApiListOfStoresResult> GetAllStoresAsync(string id)
        {
            WebApiListOfStoresResult result = await managerRepository.GetAllStoresAsync(id);

            return result;
        }

        [HttpGet]
        [Route("SelectAsync/{id}")]
        public async Task<WebApiSelectedStoreResult> SelectAsync(string id)
        {
            WebApiSelectedStoreResult result = await managerRepository.SelectStoreAsync(id);

            return result;
        }

        [HttpGet]
        [Route("EditStoreAsync/{id}")]
        public async Task<WebApiStoreResult> EditStoreAsync(string id)
        {
            WebApiStoreResult result = await managerRepository.ReadStoreByIdAsync(id);

            return result;
        }

        [HttpPut]
        [Route("EditStoreAsync")]
        public async Task<WebApiResult> EditStoreAsync(WebApiStore store)
        {
            WebApiResult result = await managerRepository.EditStoreAsync(store);

            return result;
        }

        [HttpGet]
        [Route("DetailsStoreAsync/{id}")]
        public async Task<WebApiStoreResult> DetailsStoreAsync(string id)
        {
            WebApiStoreResult result = await managerRepository.ReadStoreByIdAsync(id);

            return result;
        }

        [HttpPost]
        [Route("AbandonStoreAsync")]
        public async Task<WebApiResult> AbandonStoreAsync(WebApiStoreAssign storeUnassign)
        {
            WebApiResult result = await managerRepository.AbandonStoreAsync(storeUnassign);

            return result;
        }
    }
}