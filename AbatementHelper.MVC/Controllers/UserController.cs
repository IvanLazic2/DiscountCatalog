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
            return RedirectToAction("Registration");
        }

        //Registration action
        [HttpGet]
        public ActionResult Registration()
        {
            if ((string)Session["accountType"] == "StoreAdmin")
            {
                return View("~/Views/User/StoreAdminRegistration.cshtml");
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
        public async Task<ActionResult> Registration(User user)
        {
            user.Role = Session["accountType"].ToString();

            //if (user.Role == "StoreAdmin")
            //{
            //    user.Approved = false;
            //}
            //else
            //{
            //    user.Approved = true;
            //}



            var result = await account.Register(user);

            if (account.RegisterSuccessful)
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
        public ActionResult InitialLogin()
        {
            if (TempData["Success"] != null)
            {
                ViewBag.Message = (string)TempData["Success"];
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> InitialLogin(AuthenticatedUser user)
        {
            //TempData["EmailSuccess"] = false;
            //TempData["Email"] = null;

            //AccountRepository authenticate = new AccountRepository();
            //Response result = await authenticate.InitialLogin(user.Email);

            //if (authenticate.LoginSuccessful && authenticate.ResponseMessageText != null)
            //{
            //    TempData["StoreSelection"] = false;

            //    user.Role = result.Users[0].Role;

            //    if (user.Role == "Store")
            //    {
            //        TempData["StoreSelection"] = true;
            //        //var storeResult = authenticate.StoreLogin(user);
            //        return View(result);
            //    }

            //    //TempData["Email"] = user.Email;
            //    ViewBag.Message = authenticate.ResponseMessageText;
            //    //return RedirectToAction("PasswordLogin", new { email = user.Email, role = user.Role });
            return RedirectToAction("AccountSelection", "User", new { email = user.Email });
            //}
            //else
            //{
            //    //TempData["Email"] = null;
            //    ViewBag.Message = authenticate.ResponseMessageText;
            //    return View("~/Views/User/InitialLogin.cshtml");
            //}
        }

        [HttpGet]
        public async Task<ActionResult> AccountSelection(string email)
        {
            response = await account.InitialLogin(email);

            if (response.Users != null)
            {
                List<User> users = response.Users.ConvertAll(u => new User
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserName = u.UserName,
                    Role = u.Role
                });

                return View(users);
            }
            else if (response.User != null)
            {
                if (response.User.Email != null)
                {
                    return RedirectToAction("PasswordLogin", "User", new { id = response.User.Id });
                }
                else
                {
                    return RedirectToAction("InitialLogin", "User");
                }
            }
            else
            {
                return RedirectToAction("InitialLogin", "User");
            }
        }

        //[HttpPost]
        //public async Task<ActionResult> AccountSelection(User user)
        //{


        //    return RedirectToAction("PasswordLogin", "User", new { id = user.Id });
        //}

        [HttpGet]
        public async Task<ActionResult> PasswordLogin(string id)
        {
            var response = await account.GetUserById(id);

            User user = new User
            {
                Id = response.User.Id,
                Email = response.User.Email,
                UserName = response.User.UserName,
                Role = response.User.Role
            };

            TempData["User"] = user;

            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> PasswordLogin(User user)
        {
            User userTemp = (User)TempData["User"];
            userTemp.Password = user.Password;

            var result = await account.Login(userTemp.Email, userTemp.UserName, userTemp.Password);

            if (account.LoginSuccessful && account.ResponseMessageText != null)
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

                ViewBag.Message = account.ResponseMessageText;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = account.ResponseMessageText;
                return RedirectToAction("InitialLogin", "User");
            }

            //return RedirectToAction("Index", "Home");
        }

        //public async Task<ActionResult> StoreSelect(string id)
        //{
        //    return View();
        //}




        //public ActionResult PasswordLogin(string email, string role)
        //{
        //    if(email != null && role != null)
        //    {
        //        if(role == "User")
        //        {
        //            return View("~/Views/User/UserPasswordLogin.cshtml");
        //        }
        //        else if(role == "Store")
        //        {
        //            return View();
        //        }
        //        else
        //        {
        //            return View("~/Views/Shared/Error.cshtml");
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("InitialLogin", "User");
        //    }


        //    //if (TempData["Email"] != null)
        //    //{
        //        //return View();
        //    //}

        //    //ViewBag.Message = "Please entera an e-mail address";
        //    //return RedirectToAction("InitialLogin", "User");
        //}

        //[HttpPost]
        //public async Task<ActionResult> PasswordLogin(User user)
        //{
        //    AccountRepository authenticate = new AccountRepository();
        //    var result = await authenticate.UserLogin(user.Email, user.Password);

        //    if (authenticate.LoginSuccessful && authenticate.ResponseMessageText != null)
        //    {
        //        Response.Cookies.Add(new HttpCookie("Access_Token")
        //        {
        //            Value = result.User.Access_Token,
        //            HttpOnly = true
        //        });

        //        Response.Cookies.Add(new HttpCookie("Role")
        //        {
        //            Value = result.User.Role,
        //            HttpOnly = true
        //        });

        //        Response.Cookies.Add(new HttpCookie("UserName")
        //        {
        //            Value = result.User.UserName,
        //            HttpOnly = true
        //        });
        //        Response.Cookies.Add(new HttpCookie("UserID")
        //        {
        //            Value = result.User.Id,
        //            HttpOnly = true
        //        });
        //        Response.Cookies.Add(new HttpCookie("Email")
        //        {
        //            Value = result.User.Email,
        //            HttpOnly = true
        //        });

        //        ViewBag.Message = authenticate.ResponseMessageText;
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        ViewBag.Message = authenticate.ResponseMessageText;
        //        return View("~/Views/User/Login.cshtml", user);
        //    }
        //}

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

        //Edit GET

        [HttpGet]
        public ActionResult Edit(string id)
        {
            DataBaseUser user = new DataBaseUser();

            user = account.Edit();

            return View(user);
        }

        //Edit POST

        [HttpPost]
        public ActionResult Edit(DataBaseUser user)
        {
            if (account.Edit(user))
            {
                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/Shared/Error.cshtml", user); //za sad
        }

        //Delete

        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            account.Delete(id);

            return View();
        }


    }
}