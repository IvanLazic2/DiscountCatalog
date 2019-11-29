using AbatementHelper.CommonModels.Models;
using AbatementHelper.WebApi.Repositeories;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;
using AbatementHelper.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [HttpGet]
        [Route("GetAllUsers/{role}")]
        public DataBaseResultListOfUsers GetAllUsers(string role)
        {

            var users = DataBaseReader.ReadAllUsers(role);
            return users;
        }

        [HttpGet]
        [Route("GetAllStores")]
        public DataBaseResultListOfStores GetAllStores()
        {
            var stores = DataBaseReader.ReadAllStores();
            return stores;
        }


        [HttpPost]
        [Route("Approve")]
        public IHttpActionResult Approve(string email)
        {
            var querry = DataBaseReader.UpdateDataBaseApproved(email, true);

            if (querry.Success)
            {
                return Ok(querry.Message);
            }
            return BadRequest(querry.Message);
        }

        [HttpPost]
        [Route("Refuse")]
        public IHttpActionResult Refuse(string email)
        {
            var querry = DataBaseReader.UpdateDataBaseApproved(email, false);

            if (querry.Success)
            {
                return Ok(querry.Message);
            }
            return BadRequest(querry.Message);
        }

        [HttpGet]
        [Route("EditUser/{id}")]
        public DataBaseUser EditUser(string id)
        {
            DataBaseUser user = new DataBaseUser();

            user = DataBaseReader.ReadUserById(id).Value;

            return user;
        }

        [HttpPut]
        [Route("EditUser")]
        public IHttpActionResult EditUser(DataBaseUser user)
        {
            DataBaseReader.EditUser(user);

            return Ok();
        }




        [HttpGet]
        [Route("EditStore/{id}")]
        public DataBaseStore EditStore(string id)
        {
            DataBaseStore store = new DataBaseStore();

            store = DataBaseReader.ReadStoreById(id).Value;

            return store;
        }

        [HttpPut]
        [Route("EditStore")]
        public IHttpActionResult EditStore(DataBaseStore store)
        {
            DataBaseReader.EditStore(store);

            return Ok();
        }

        [HttpPut]
        [Route("Delete/{role}/{id}")]
        public IHttpActionResult Delete(string role, string id)
        {
            if (role == "User" || role == "StoreAdmin" || role == "Admin")
            {
                DataBaseReader.DeleteUser(id);
                return Ok("User deleted");
            }
            else if (role == "Store")
            {
                DataBaseReader.DeleteStore(id);
                return Ok("Store deleted");
            }
            else
            {
                return BadRequest("User does not exist");
            }
        }

        [HttpPut]
        [Route("Restore/{role}/{id}")]
        public IHttpActionResult Restore(string role, string id)
        {
            if (role == "User" || role == "StoreAdmin" || role == "Admin")
            {
                DataBaseReader.RestoreUser(id);
                return Ok("User restored");
            }
            else if (role == "Store")
            {
                DataBaseReader.RestoreStore(id);
                return Ok("Store restored");
            }
            else
            {
                return BadRequest("User does not exist");
            }
        }









        [Route("MethodTesting")]
        public IHttpActionResult ReadEmail(string email)
        {
            DataBaseEntityRepository.ReadUsers(email);

            return Ok();
        }
    }
}
