using DiscountCatalog.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiscountCatalog.MVC.Repositories;
using System.Threading.Tasks;
using System.Web.Security;
using DiscountCatalog.Common.Models;
using System.Web.Routing;
using DiscountCatalog.Common.WebApiModels;
using System.IO;
using System.Drawing;
using DiscountCatalog.MVC.Processors;
using DiscountCatalog.MVC.ViewModels;
using DiscountCatalog.Common.Models.Extended;

namespace DiscountCatalog.MVC.Controllers
{
    public class AccountController : Controller //promjenit u accountcontroller
    {

        private AccountRepository accountRepository = new AccountRepository();

        [HttpGet]
        public ActionResult AccountTypeSelection()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AccountTypeSelection(string accountType)
        {
            Session["accountType"] = accountType;
            return RedirectToAction("Register", new { accountType });
        }

        [HttpGet]
        public ActionResult Register(string accountType)
        {
            if (accountType != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccountTypeSelection");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserViewModel user)
        {
            user.Role = Session["accountType"].ToString();

            Result result = await accountRepository.RegisterAsync(user);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(user);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(AuthenticationModel user)
        {
            AuthenticatedUserResult result = await accountRepository.Login(user);

            if (result.Success)
            {
                Logout();

                Response.Cookies.Add(new HttpCookie("Access_Token")
                {
                    Value = result.Access_Token,
                    HttpOnly = true
                });
                Response.Cookies.Add(new HttpCookie("Role")
                {
                    Value = result.Role,
                    HttpOnly = true
                });
                Response.Cookies.Add(new HttpCookie("UserName")
                {
                    Value = result.UserName,
                    HttpOnly = true
                });
                Response.Cookies.Add(new HttpCookie("UserID")
                {
                    Value = result.Id,
                    HttpOnly = true
                });
                Response.Cookies.Add(new HttpCookie("Email")
                {
                    Value = result.Email,
                    HttpOnly = true
                });

                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View();
            }
        }

        public ActionResult Logout()
        {
            string[] myCookies = Request.Cookies.AllKeys;

            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("Details")]
        public async Task<ActionResult> Details()
        {
            User user = await accountRepository.Details();

            return View(user);
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<ActionResult> Edit()
        {
            User user = await accountRepository.Details();

            return View(user);
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<ActionResult> Edit(User user)
        {
            Result result = await accountRepository.Edit(user);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(user);
            }

            return RedirectToAction("Details");
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<ActionResult> Delete()
        {
            User user = await accountRepository.Details();
            
            return View(user);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult> Delete(User user)
        {
            Result result = await accountRepository.Delete();

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(user);
            }

            Logout();

            return RedirectToAction("Index", "Home");
        }

        [Route("PostUserImage")]
        public async Task<ActionResult> PostUserImage(PostImage image)
        {
            // ZA SPREMANJE NA SERVER
            //string image = System.IO.Path.GetFileName(file.FileName); // + Guid.NewGuid().ToString();  mozda
            //string path = System.IO.Path.Combine(Server.MapPath("sad tu napisem path npr. ~/images/user il tak nest"), image);
            //file.SaveAs(path);
            //i u bazu stavim adresu.

            //ZA SPREMANJE NA BAZU

            byte[] array = ImageProcessor.GetBuffer(image.File);

            byte[] imageArray = array;

            float mb = (array.Length / 1024f) / 1024f;

            if (mb > 1)
            {
                byte[] arrayScaled = ImageProcessor.To1MB(array);
                //float mbScaled = (arrayScaled.Length / 1024f) / 1024f;

                imageArray = arrayScaled;
            }

            Result result = await accountRepository.PostUserImage(imageArray);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return RedirectToAction("Details");
        }

        [Route("GetUserImage/{id}")]
        public async Task<ActionResult> GetUserImage(string id)
        {
            byte[] byteArray = await accountRepository.GetUserImage();

            return File(byteArray, "image/png");
        }
    }
}