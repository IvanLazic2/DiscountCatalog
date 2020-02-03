using AbatementHelper.CommonModels.CreateModels;
using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;
using AbatementHelper.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AbatementHelper.WebAPI.Controllers
{ 
    //[Authorize(Roles = "Admin")]
    [RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        private AdminRepository adminRepository = new AdminRepository();

        [HttpGet]
        [Route("GetAllUsersAsync")]
        public async Task<WebApiListOfUsersResult> GetAllUsersAsync()
        {
            WebApiListOfUsersResult result = await adminRepository.GetAllUsersAsync();

            return result;
        }

        [HttpGet]
        [Route("GetAllStoresAsync")]
        public async Task<WebApiListOfStoresResult> GetAllStoresAsync()
        {
            WebApiListOfStoresResult result = await adminRepository.GetAllStoresAsync();

            return result;
        }

        [HttpPost]
        [Route("CreateUserAsync")]
        public async Task<WebApiResult> CreateUserAsync(CreateUserModel user)
        {
            WebApiResult result = await adminRepository.CreateUserAsync(user, user.Password);

            return result;
        }

        [HttpPost]
        [Route("CreateStoreAsync")]
        public async Task<WebApiResult> CreateStoreAsync(WebApiStore store)
        {
            WebApiResult result = await adminRepository.CreateStoreAsync(store);

            return result;
        }

        [HttpPost]
        [Route("CreateManagerAsync")]
        public async Task<WebApiResult> CreateManagerAsync(CreateManagerModel manager)
        {
            WebApiResult result = await adminRepository.CreateManagerAsync(manager, manager.Password);

            return result;
        }

        [HttpGet]
        [Route("EditUserAsync/{id}")]
        public async Task<WebApiUserResult> EditUserAsync(string id)
        {
            WebApiUserResult result = await adminRepository.ReadUserByIdAsync(id);

            return result;
        }

        [HttpPut]
        [Route("EditUserAsync")]
        public async Task<WebApiResult> EditUserAsync(WebApiUser user)
        {
            WebApiResult result = await adminRepository.EditUserAsync(user);

            return result;
        }

        [HttpGet]
        [Route("EditStoreAsync/{id}")]
        public async Task<WebApiStoreResult> EditStoreAsync(string id)
        {
            WebApiStoreResult result = await adminRepository.ReadStoreByIdAsync(id);

            return result;
        }

        [HttpPut]
        [Route("EditStoreAsync")]
        public async Task<WebApiResult> EditStoreAsync(WebApiStore store)
        {
            WebApiResult result = await adminRepository.EditStoreAsync(store);

            return result;
        }

        [HttpGet]
        [Route("UserDetailsAsync/{id}")]
        public async Task<WebApiUserResult> UserDetailsAsync(string id)
        {
            WebApiUserResult result = await adminRepository.ReadUserByIdAsync(id);

            return result;
        }

        [HttpPut]
        [Route("DeleteUserAsync")]
        public async Task<WebApiResult> DeleteUserAsync(WebApiUser user)
        {
            WebApiResult result = await adminRepository.DeleteUserAsync(user.Id);

            return result;
        }

        [HttpPut]
        [Route("RestoreUserAsync/{id}")]
        public async Task<WebApiResult> RestoreUserAsync(string id)
        {
            WebApiResult result = await adminRepository.RestoreUserAsync(id);

            return result;
        }

        [HttpGet]
        [Route("StoreDetailsAsync/{id}")]
        public async Task<WebApiStoreResult> StoreDetailsAsync(string id)
        {
            WebApiStoreResult result = await adminRepository.ReadStoreByIdAsync(id);

            return result;
        }

        [HttpPut]
        [Route("DeleteStoreAsync")]
        public async Task<WebApiResult> DeleteStoreAsync(WebApiUser user)
        {
            WebApiResult result = await adminRepository.DeleteStoreAsync(user.Id);

            return result;
        }

        [HttpPut]
        [Route("RestoreStoreAsync/{id}")]
        public async Task<WebApiResult> RestoreStoreAsync(string id)
        {
            WebApiResult result = await adminRepository.RestoreStoreAsync(id);

            return result;
        }

        //Approve / Refuse 
    }
}
