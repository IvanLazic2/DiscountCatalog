using AbatementHelper.CommonModels.Models;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
    public class LoginController : ApiController
    {
        //private DataBaseEntityRepository entityReader = new DataBaseEntityRepository();
        private AuthenticationManagerRepository authenticate = new AuthenticationManagerRepository();
        private Response response = new Response();

        //[System.Web.Http.HttpPost]
        //[System.Web.Http.Route("InitialLogin")]
        //public Response InitialLogin(AuthenticationModel user)
        //{
        //    Response response = new Response();

        //    var emailReader = DataBaseEntityRepository.ReadEmail(user.Email);
        //    var userReader = DataBaseEntityRepository.ReadUsers(user.Email);

        //    //Task<string> role = Task.Run(() => entityReader.ReadRole("6d1eb4ab-1c16-4c74-a527-cfade928da79"));
        //    //role.Wait();

        //    if (emailReader.Success)
        //    {
        //        response.Users = userReader.ConvertAll(u => new AuthenticatedUser
        //        {
        //            Id = u.Id,
        //            Role = returnAsyncRole(u.Id),
        //            Email = u.Email,
        //            UserName = u.UserName
        //        });

        //        response.ResponseCode = (int)System.Net.HttpStatusCode.OK;

        //        if (response.Users.Count == 1)
        //        {
        //            response.User = response.Users.First();
        //            response.Users = null;
        //        }

        //        response.ResponseMessage = ""; // ovo cu jos vidit
        //    }
        //    else
        //    {
        //        response.ResponseCode = (int)System.Net.HttpStatusCode.BadRequest;
        //        response.ResponseMessage = "Email does not exist";
        //    }

        //    return response;
        //}

        //private string returnAsyncRole(string id)
        //{
        //    Task<string> role = Task.Run(() => entityReader.ReadRole(id));
        //    role.Wait();

        //    return role.Result;
        //}


        //[System.Web.Http.HttpGet]
        //[System.Web.Http.Route("GetUserById/{id}")]
        //public Response GetUserById(string id)
        //{
        //    Response response = new Response();

        //    var userReader = entityReader.ReadUserById(id);

        //    response.User = new AuthenticatedUser
        //    {
        //        Id = userReader.Id,
        //        UserName = userReader.UserName,
        //        Email = userReader.Email,
        //        Role = returnAsyncRole(userReader.Id)
        //    };

        //    response.ResponseMessage = "";
        //    response.ResponseCode = (int)System.Net.HttpStatusCode.OK;

        //    return response;
        //}

        public static async Task<ApplicationUser> ReturnUserName
            ( UserManager<ApplicationUser> userManager, string usernameOrEmail, string password)
        {
            var username = usernameOrEmail;
            if (usernameOrEmail.Contains("@"))
            {
                var userForEmail = await userManager.FindByEmailAsync(usernameOrEmail);
                if (userForEmail != null)
                {
                    username = userForEmail.UserName;
                }
            }
            return await userManager.FindAsync(username, password);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Authenticate")]
        public async Task<Response> Authenticate(AuthenticationModel model)
        {

            var user = await ReturnUserName(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationUserDbContext())), model.EmailOrUserName, model.Password);


            var result = Task.Run(() => authenticate.Authenticate(user.UserName, model.Password));
            result.Wait();

            response.User = result.Result;

            //var readuser = DataBaseReader.ReadUser(model.Email);  ovo treba

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


            //user.ResponseMessage = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, "Email address does not exist!");

            response.ResponseCode = (int)System.Net.HttpStatusCode.BadRequest;
            response.ResponseMessage = "Email address does not exist!";

            return response;

        }

    }

}