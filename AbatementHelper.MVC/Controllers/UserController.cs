using AbatementHelper.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbatementHelper.MVC.Repositories;
using System.Threading.Tasks;
using System.Web.Security;
using AbatementHelper.CommonModels.Models;
using System.Web.Routing;
using AbatementHelper.CommonModels.WebApiModels;

namespace AbatementHelper.MVC.Controllers
{
    public class UserController : Controller
    {
        private AccountRepository account = new AccountRepository();
        private Response response = new Response();

        [HttpGet]
        public ActionResult AccountTypeSelection()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AccountTypeSelection(string accountType)
        {
            Session["accountType"] = accountType;
            return RedirectToAction("Registration", "User", new { accountType });
        }

        //Registration action
        [HttpGet]
        public ActionResult Registration(string accountType)
        {
            if (accountType != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccountTypeSelection", "User");
            }
        }
        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(User user)
        {
            user.Role = Session["accountType"].ToString();

            var result = await account.Register(user);

            if (account.RegisterSuccessful)
            {
                TempData["Success"] = "Registration Successful, please log in!";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Message = "Register Unsuccessful";
                return RedirectToAction("Registration");
            }

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(AuthenticationModel user)
        {
            var result = await account.Login(user);

            if (account.LoginSuccessful)
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
                Response.Cookies.Add(new HttpCookie("UserID")
                {
                    Value = result.User.Id,
                    HttpOnly = true
                });
                Response.Cookies.Add(new HttpCookie("Email")
                {
                    Value = result.User.Email,
                    HttpOnly = true
                });

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login");
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


        //Details

        [HttpGet]
        [Route("Details")]
        public ActionResult Details()
        {
            WebApiUser user = account.Details();

            if (TempData["Message"] != null && TempData["Success"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                ViewBag.Success = (bool)TempData["Success"];
            }

            return View(user);
        }

        //Edit GET

        [HttpGet]
        [Route("Edit")]
        public ActionResult Edit()
        {
            WebApiUser user = new WebApiUser();

            user = account.Edit();

            return View(user);
        }

        //Edit POST

        [HttpPost]
        [Route("Edit")]
        public ActionResult Edit(WebApiUser user)
        {
            Response editResponse = account.Edit(user);

            TempData["Message"] = editResponse.ResponseMessage;
            TempData["Success"] = editResponse.Success;

            return RedirectToAction("Details");
        }

        //Delete

        [HttpGet]
        [Route("Delete")]
        public ActionResult Delete()
        {
            WebApiUser user = account.Details();

            return View(user);
        }

        [HttpPost]
        [Route("Delete")]
        public ActionResult Delete(WebApiUser user)
        {
            account.Delete(user);

            return RedirectToAction("Index", "Home");
        }


    }
}