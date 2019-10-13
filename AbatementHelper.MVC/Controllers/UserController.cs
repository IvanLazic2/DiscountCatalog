using AbatementHelper.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbatementHelper.MVC.Repositories;
using System.Threading.Tasks;
using AbatementHelper.Classes.Models;

namespace AbatementHelper.MVC.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult AccountTypeSelection()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AccountTypeSelection(bool accountType)
        {
            Session["accountType"] = accountType;
            return RedirectToAction("Registration");
        }

        //Registration action
        [HttpGet]
        public ActionResult Registration()
        {
            if ((bool)Session["accountType"])
            {
                return View("~/Views/User/StoreRegistration.cshtml"); //treba dodat taj view
            }

            return View("~/Views/User/UserRegistration.cshtml");
        }
        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration([Bind(Exclude = "EmailConfirmed, ActivationCode")] User user)
        {
            user.IsStore = (bool)Session["accountType"];

            ApiManagerRepository register = new ApiManagerRepository();

            var result = await register.Register(user);

            if (register.RegisterSuccessful)
            {
                TempData["Success"] = "Registration Successful, please log in!";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Message = "Register Unsuccessful";
                return View("~/Views/User/Registration.cshtml", user); //treba mijenjat
            }

            //return View("~/Views/User/RegistrationVerification.cshtml", user);
        }
        //verify Email

        //verify Email LINK

        //Login
        [HttpGet]
        public ActionResult Login()
        {
            if (TempData["Success"] != null)
            {
                ViewBag.Message = (string)TempData["Success"];
            }

            return View();
        }
        //Login POST
        [HttpPost]
        public async Task<ActionResult> Login([Bind(Exclude = "FirstName, LastName, PhoneNumber, BirthDate, EmailConfirmed, ConfirmationCode")] User user)
        {
            ApiManagerRepository authenticate = new ApiManagerRepository();
            var result = await authenticate.Authenticate(user.Email, user.Password);


            if (authenticate.LoginSuccessful && authenticate.ResponseMessageText != null)
            {
                //Response.Cookies.Add(new HttpCookie("Token")
                //{
                //    Value = result.User.Access_Token,
                //    HttpOnly = true
                //});

                ViewBag.Message = authenticate.ResponseMessageText;
                return View("~/Views/Home/Index.cshtml", user);
            }
            else
            {
                ViewBag.Message = authenticate.ResponseMessageText;
                return View("~/Views/User/Login.cshtml", user);
            }
        }
        //Logout
    }
}