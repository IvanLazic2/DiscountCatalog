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
        public Response EmailAuthentication(AuthenticationModel model)
        {
            Response response = new Response();

            if (DataBaseReader.ReadEmail(model.Email))
            {
                AuthenticationManagerRepository authenticate = new AuthenticationManagerRepository();

                var result = Task.Run(() => authenticate.Authenticate(model.Email, DataBaseReader.ReadUsername(model.Email), model.Password));
                result.Wait();

                response.User = result.Result;
                
                var readuser = DataBaseReader.ReadUser(model.Email);

                if (authenticate.LoginSuccessful)
                {
                    //user.ResponseMessage =  Request.CreateResponse(System.Net.HttpStatusCode.OK, "Authentication successfull!");

                    response.ResponseCode = (int)System.Net.HttpStatusCode.OK;
                    response.ResponseMessage = "Authentication Succesfull";

                    return response;
                }
                else
                {
                    //user.ResponseMessage =  Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, "Incorrect password!");

                    response.ResponseCode = (int)System.Net.HttpStatusCode.BadRequest;
                    response.ResponseMessage = "Incorrect password!";

                    return response;
                }
            }

            //user.ResponseMessage = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, "Email address does not exist!");

            response.ResponseCode = (int)System.Net.HttpStatusCode.BadRequest;
            response.ResponseMessage = "Email address does not exist!";

            return response;

        }

    }
}