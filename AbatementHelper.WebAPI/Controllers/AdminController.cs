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
        public void GetAllUsers()
        {

        }

        [HttpPost]
        [Route("Approve")]
        public IHttpActionResult Approve(string email)
        {
            if (DataBaseReader.UpdateDataBaseApproved(email, true))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Refuse")]
        public IHttpActionResult Refuse(string email)
        {
            if (DataBaseReader.UpdateDataBaseApproved(email, false))
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}
