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
        [Route("GetAllUsers")]
        public WebApiListOfUsersResult GetAllUsers()
        {
            var users = adminRepository.ReadAllUsers();

            List<WebApiUser> processedUsers = new List<WebApiUser>();

            foreach (var user in users)
            {
                processedUsers.Add(UserProcessor.ApplicationUserToWebApiUser(user));
            }

            return new WebApiListOfUsersResult
            {
                Value = processedUsers,
                Success = true
            };
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public async Task<WebApiUser> Edit(string id)
        {
            //WebApiUser user = new WebApiUser();

            var user = await adminRepository.ReadUserById(id);

            return UserProcessor.ApplicationUserToWebApiUser(user);
        }

        [HttpPut]
        [Route("Edit")]
        public Response Edit(WebApiUser user)
        {
            return adminRepository.Edit(user);
        }

        [HttpGet]
        [Route("Details/{id}")]
        public async Task<WebApiUser> Details(string id)
        {
            var user = await adminRepository.ReadUserById(id);

            return UserProcessor.ApplicationUserToWebApiUser(user);
        }

        [HttpPut]
        [Route("Delete")]
        public IHttpActionResult Delete(WebApiUser user)
        {
            adminRepository.Delete(user.Id);

            return Ok();
        }

        [HttpPut]
        [Route("Restore/{id}")]
        public IHttpActionResult Restore(string id)
        {
            adminRepository.Restore(id);

            return Ok();
        }
    }
}
