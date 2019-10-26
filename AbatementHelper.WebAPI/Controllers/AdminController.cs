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

    [RoutePrefix("api/Admin")]
    public class AdminController : System.Web.Http.ApiController
    {
        [HttpGet]
        [Route("GetAllUsers")]
        public DataBaseResultListOfUsers GetAllUsers()
        {
            //List<DataBaseResultListOfUsers> users = new List<DataBaseResultListOfUsers>();

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

    }
}
