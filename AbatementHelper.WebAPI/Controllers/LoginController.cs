using AbatementHelper.WebApi.Repositeories;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace AbatementHelper.WebAPI.Controllers
{

    [System.Web.Http.RoutePrefix("api/Login")]
    public class LoginController : System.Web.Http.ApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("EmailAuthentication")]
        public HttpResponseMessage EmailAuthentication(AuthenticationModel model)
        {
            if (DataBaseReader.ReadEmail(model.Email))
            {
                AuthenticationManagerRepository authenticate = new AuthenticationManagerRepository();
                var result = Task.Run(() => authenticate.Authenticate(model.Email, DataBaseReader.ReadUsername(model.Email), model.Password));
                result.Wait();


                var readuser = DataBaseReader.ReadUser(model.Email);


                if (authenticate.LoginSuccessful)
                {
                    return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Authentication successfull!");
                }
                else
                {
                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, "Incorrect password!");
                }
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, "Email address does not exist!");

        }
    }
}