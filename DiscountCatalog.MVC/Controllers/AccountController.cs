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
using DiscountCatalog.MVC.REST.Account;
using DiscountCatalog.MVC.Extensions;
using AutoMapper;
using AbatementHelper.MVC.Mapping;
using DiscountCatalog.MVC.Cookies.Contractor;
using DiscountCatalog.MVC.Cookies.Implementation;

namespace DiscountCatalog.MVC.Controllers
{
    public class AccountController : Controller
    {
        #region Fields

        private readonly IMapper mapper;
        private readonly ICookieHandler cookieHandler;
        private readonly UserCookieHandler userCookieHandler;
        private AccountRepository accountRepository = new AccountRepository();

        #endregion

        #region Constructors

        public AccountController()
        {
            mapper = AutoMapping.Initialise();
            cookieHandler = new CookieHandler();
            userCookieHandler = new UserCookieHandler();

        }

        #endregion

        #region PrivateMethods

        private bool areUserCookiesValid()
        {
            return userCookieHandler.IsValid(userCookieHandler.Get(System.Web.HttpContext.Current));
        }

        private ActionResult RedirectToLogin()
        {
            cookieHandler.ClearAll(System.Web.HttpContext.Current);
            return RedirectToAction("Login").Error("An error has occurred, please log in.");
        }

        #endregion

        #region Methods

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

                cookieHandler.Set("Access_Token", result.Access_Token, true, System.Web.HttpContext.Current);
                cookieHandler.Set("UserID", result.Id, true, System.Web.HttpContext.Current);
                cookieHandler.Set("UserName", result.Access_Token, true, System.Web.HttpContext.Current);
                cookieHandler.Set("Email", result.Email, true, System.Web.HttpContext.Current);
                cookieHandler.Set("Role", result.Role, true, System.Web.HttpContext.Current);

                if (!userCookieHandler.IsValid(userCookieHandler.Get(System.Web.HttpContext.Current)))
                {
                    cookieHandler.ClearAll(System.Web.HttpContext.Current);
                    return View().Error("An error has occurred, please try again.");
                }

                return RedirectToAction("Index", "Home").Success(result.SuccessMessage);
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
            cookieHandler.ClearAll(System.Web.HttpContext.Current);

            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("Details")]
        public async Task<ActionResult> Details()
        {
            if (!areUserCookiesValid())
            {
                return RedirectToLogin();
            }

            AccountREST user = await accountRepository.Details();

            return View(user);
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<ActionResult> Edit()
        {
            if (!areUserCookiesValid())
            {
                return RedirectToLogin();
            }

            AccountREST user = await accountRepository.Details();

            if (user != null)
            {
                if (user.Id != null)
                {
                    return View(user);
                }
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<ActionResult> Edit(AccountRESTPut user)
        {
            if (!areUserCookiesValid())
            {
                return RedirectToLogin();
            }

            Result result = await accountRepository.Edit(user);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(await accountRepository.Details());
            }

            return RedirectToAction("Details").Success(result.SuccessMessage);
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<ActionResult> Delete()
        {
            if (!areUserCookiesValid())
            {
                return RedirectToLogin();
            }

            AccountREST user = await accountRepository.Details();

            return View(user);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult> Delete(User user)
        {
            if (!areUserCookiesValid())
            {
                return RedirectToLogin();
            }

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
            if (!areUserCookiesValid())
            {
                return RedirectToLogin();
            }

            // ZA SPREMANJE NA SERVER
            //string image = System.IO.Path.GetFileName(file.FileName); // + Guid.NewGuid().ToString();  mozda
            //string path = System.IO.Path.Combine(Server.MapPath("sad tu napisem path npr. ~/images/user il tak nest"), image);
            //file.SaveAs(path);
            //i u bazu stavim adresu.

            //ZA SPREMANJE NA BAZU

            try
            {
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

                    return RedirectToAction("Details", ModelState);
                }
                else
                {
                    return RedirectToAction("Details").Success(result.SuccessMessage);
                }
            }
            catch (Exception exc)
            {
                Type type = exc.GetType();
                throw;
            }
        }

        [Route("GetUserImage/{id}")]
        public async Task<ActionResult> GetUserImage(string id)
        {
            if (!areUserCookiesValid())
            {
                return RedirectToLogin();
            }

            byte[] byteArray = await accountRepository.GetUserImage();

            return File(byteArray, "image/png");
        }

        #endregion
        
    }
}