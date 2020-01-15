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
            var result = new WebApiListOfUsersResult();

            List<WebApiUser> users = await adminRepository.ReadAllUsersAsync();

            result.Users = users;

            return result;
        }

        [HttpGet]
        [Route("GetAllStoresAsync")]
        public async Task<WebApiListOfStoresResult> GetAllStoresAsync()
        {
            var result = new WebApiListOfStoresResult();

            //repo

            return result;
        } 

        [HttpGet]
        [Route("EditAsync/{id}")]
        public async Task<WebApiUser> EditAsync(string id)
        {
            ApplicationUser user = await adminRepository.ReadUserById(id);

            WebApiUser webApiUser = await UserProcessor.ApplicationUserToWebApiUser(user);

            return webApiUser;
        }

        [HttpPut]
        [Route("EditAsync")]
        public async Task<Response> EditAsync(WebApiUser user)
        {
            return await adminRepository.EditAsync(user);
        }

        [HttpGet]
        [Route("DetailsAsync/{id}")]
        public async Task<WebApiUser> DetailsAsync(string id)
        {
            var user = await adminRepository.ReadUserById(id);

            WebApiUser webApiUser = await UserProcessor.ApplicationUserToWebApiUser(user);

            return webApiUser;
        }

        [HttpPut]
        [Route("DeleteAsync")]
        public async Task<IHttpActionResult> DeleteAsync(WebApiUser user)
        {
            await adminRepository.DeleteAsync(user.Id);

            return Ok();
        }

        [HttpPut]
        [Route("RestoreAsync/{id}")]
        public async Task<IHttpActionResult> RestoreAsync(string id)
        {
            await adminRepository.RestoreAsync(id);

            return Ok();
        }

        //Approve / Refuse 
    }
}
