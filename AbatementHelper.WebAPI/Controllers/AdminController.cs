using AbatementHelper.CommonModels.Models;
using AbatementHelper.WebApi.Repositeories;
using AbatementHelper.WebAPI.Models;
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

    //[HttpPost]
    //[Route("Approve")]
    ////[Authorize(Roles = "Admin")]
    //public static async void Approve()
    //{
    //    DataBaseReader.UpdateDataBaseApproved(false, "aaa@aaa.aaa");
    //}

    //[Authorize(Roles = "Admin")]
    //public static void Disapprove()
    //{

    //}
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        [HttpGet]
        [Route("GetAllUsers")]
        public DataBaseResultListOfUsers GetAllUsers()
        {
            var users = DataBaseReader.ReadAllUsers();

            return users;
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
        [Route("EditGet/{id}")]
        public DataBaseUser EditGet(string id)
        {
            DataBaseUser user = new DataBaseUser();

            user = DataBaseReader.ReadUserById(id).Value;

            return user;
        }

        [HttpPut]
        [Route("EditPost")]
        public IHttpActionResult EditPost(DataBaseUser user)
        {
            DataBaseReader.EditUser(user);

            return Ok();
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IHttpActionResult Delete(string id)
        {
            DataBaseReader.Delete(id);

            return Ok();
        }

    }
}
