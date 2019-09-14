using AbatementHelper.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbatementHelper.MVC.Repositories;
using System.Threading.Tasks;

namespace AbatementHelper.MVC.Controllers
{
    public class UserController : Controller
    {
        //Registration action
        [HttpGet]
        public ActionResult Registration()
        {
            return View("~/Views/User/Registration.cshtml");
        }
        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration([Bind(Exclude = "EmailConfirmed, ActivationCode")] User user)
        {
            ApiManagerRepository register = new ApiManagerRepository();

            ViewBag.Status = "";

            var result = await register.Register(user);

            ViewBag.Status = result;

            return View("~/Views/User/RegistrationVerification.cshtml", user);
        }
        //verify Email

        //verify Email LINK

        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //Login POST
        [HttpPost]
        public async Task<ActionResult> Login([Bind(Exclude = "FirstName, LastName, PhoneNumber, BirthDate, EmailConfirmed, ConfirmationCode")] User user)
        {
            ApiManagerRepository authenticate = new ApiManagerRepository();

            var result = await authenticate.Authenticate(user.Email, user.Password);

            ViewBag.Message = result;

            return View(user);
        }
        //Logout
    }
}