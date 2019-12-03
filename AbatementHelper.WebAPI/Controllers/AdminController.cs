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
        [Route("EditUser/{id}")]
        public WebApiUser EditUser(string id)
        {
            //WebApiUser user = new WebApiUser();

            var user = entityReader.ReadUserById(id);

            return UserProcessor.ApplicationUserToWebApiUser(user);
        }

        [HttpPut]
        [Route("EditUser")]
        public IHttpActionResult EditUser(WebApiUser user)
        {
            entityReader.EditUser(user);

            return Ok();
        }

        [HttpGet]
        [Route("DetailsUser/{id}")]
        public WebApiUser DetailsUser(string id)
        {
            var user = entityReader.ReadUserById(id);

            return UserProcessor.ApplicationUserToWebApiUser(user);
        }

        [HttpPut]
        [Route("DeleteUser")]
        public IHttpActionResult DeleteUser(WebApiUser user)
        {
            entityReader.DeleteUser(user.Id);

            return Ok();
        }

        [HttpPut]
        [Route("RestoreUser/{id}")]
        public IHttpActionResult RestoreUser(string id)
        {
            entityReader.RestoreUser(id);

            return Ok();
        }




        //[HttpPut]
        //[Route("Delete/{role}/{id}")]
        //public IHttpActionResult Delete(string role, string id)
        //{
        //    if (role == "User" || role == "StoreAdmin" || role == "Admin")
        //    {
        //        DataBaseReader.DeleteUser(id);
        //        return Ok("User deleted");
        //    }
        //    else if (role == "Store")
        //    {
        //        DataBaseReader.DeleteStore(id);
        //        return Ok("Store deleted");
        //    }
        //    else
        //    {
        //        return BadRequest("User does not exist");
        //    }
        //}

        //[HttpPut]
        //[Route("Restore/{role}/{id}")]
        //public IHttpActionResult Restore(string role, string id)
        //{
        //    if (role == "User" || role == "StoreAdmin" || role == "Admin")
        //    {
        //        DataBaseReader.RestoreUser(id);
        //        return Ok("User restored");
        //    }
        //    else if (role == "Store")
        //    {
        //        DataBaseReader.RestoreStore(id);
        //        return Ok("Store restored");
        //    }
        //    else
        //    {
        //        return BadRequest("User does not exist");
        //    }
        //}










    }
}
