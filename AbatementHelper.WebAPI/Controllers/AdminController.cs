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
        private DataBaseEntityRepository entityReader = new DataBaseEntityRepository();

        [HttpGet]
        [Route("GetAllUsers")]
        public WebApiListOfUsersResult GetAllUsers()
        {
            var users = entityReader.ReadAllUsers();

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






        //[HttpGet]
        //[Route("GetAllStores")]
        //public DataBaseResultListOfStores GetAllStores()
        //{
        //    var stores = DataBaseReader.ReadAllStores();
        //    return stores;
        //}


        //[HttpPost]
        //[Route("Approve")]
        //public IHttpActionResult Approve(string email)
        //{
        //    var querry = DataBaseReader.UpdateDataBaseApproved(email, true);

        //    if (querry.Success)
        //    {
        //        return Ok(querry.Message);
        //    }
        //    return BadRequest(querry.Message);
        //}

        //[HttpPost]
        //[Route("Refuse")]
        //public IHttpActionResult Refuse(string email)
        //{
        //    var querry = DataBaseReader.UpdateDataBaseApproved(email, false);

        //    if (querry.Success)
        //    {
        //        return Ok(querry.Message);
        //    }
        //    return BadRequest(querry.Message);
        //}

        [HttpGet]
        [Route("Edit/{id}")]
        public WebApiUser Edit(string id)
        {
            //WebApiUser user = new WebApiUser();

            var user = entityReader.ReadUserById(id);

            return UserProcessor.ApplicationUserToWebApiUser(user);
        }

        [HttpPut]
        [Route("Edit")]
        public IHttpActionResult Edit(WebApiUser user)
        {
            entityReader.EditUser(user);

            return Ok();
        }

        [HttpGet]
        [Route("Details/{id}")]
        public WebApiUser Details(string id)
        {
            var user = entityReader.ReadUserById(id);

            return UserProcessor.ApplicationUserToWebApiUser(user);
        }

        [HttpPut]
        [Route("Delete")]
        public IHttpActionResult Delete(WebApiUser user)
        {
            entityReader.DeleteUser(user.Id);

            return Ok();
        }

        [HttpPut]
        [Route("Restore/{id}")]
        public IHttpActionResult Restore(string id)
        {
            entityReader.RestoreUser(id);

            return Ok();
        }
    }
}
