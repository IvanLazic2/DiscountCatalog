using AbatementHelper.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbatementHelper.MVC.Repositories;
using System.Threading.Tasks;
using System.Web.Security;

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
        public ActionResult AccountTypeSelection(string accountType)
        {
            Session["accountType"] = accountType;
            return RedirectToAction("Registration");
        }

        //Registration action
        [HttpGet]
        public ActionResult Registration()
        {
            if ((string)Session["accountType"] == "Store")
            {
                return View("~/Views/User/StoreRegistration.cshtml");
            }
            else if ((string)Session["accountType"] == "User")
            {
                return View("~/Views/User/UserRegistration.cshtml");
            }
            return View("~/Views/Shared/Error.cshtml");
        }
        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration([Bind(Exclude = "EmailConfirmed, ActivationCode")] User user)
        {
            user.Role = Session["accountType"].ToString();
            if (user.Role == "Store")
            {
                user.Approved = false;
            }
            else
            {
                user.Approved = true;
            }

            AccountRepository register = new AccountRepository();

            var result = await register.Register(user);

            if (register.RegisterSuccessful)
            {
                TempData["Success"] = "Registration Successful, please log in!";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Message = "Register Unsuccessful";
                return View("~/Views/Shared/Error.cshtml", user); //treba mijenjat
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
            //Response.Cookies.Add(new HttpCookie("Access_Token")
            //{
            //    Value = null,
            //    HttpOnly = true
            //});

            AccountRepository authenticate = new AccountRepository();
            var result = await authenticate.Login(user.Email, user.Password);


            


            if (authenticate.LoginSuccessful && authenticate.ResponseMessageText != null)
            {
                Response.Cookies.Add(new HttpCookie("Access_Token")
                {
                    Value = result.User.Access_Token,
                    HttpOnly = true
                });

                Response.Cookies.Add(new HttpCookie("Role")
                {
                    Value = result.User.Role,
                    HttpOnly = true
                });

                Response.Cookies.Add(new HttpCookie("UserName")
                {
                    Value = result.User.UserName,
                    HttpOnly = true
                });



                ViewBag.Message = authenticate.ResponseMessageText;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = authenticate.ResponseMessageText;
                return View("~/Views/User/Login.cshtml", user);
            }
        }
        //Logout

        public ActionResult Logout()
        {
            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }

            return RedirectToAction("Login");
        }

    }
}