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
using System.IO;
using System.Drawing;
using AbatementHelper.MVC.Processors;

namespace AbatementHelper.MVC.Controllers
{
    public class UserController : Controller //promjenit u accountcontroller
    {
        private AccountRepository account = new AccountRepository();
        
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(User user)
        {
            user.Role = Session["accountType"].ToString();

            WebApiResult result = await account.RegisterAsync(user);

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
            WebApiAuthenticatedUserResult result = await account.LoginAsync(user);

            if (result.Success)
            {
                string[] myCookies = Request.Cookies.AllKeys;
                foreach (string cookie in myCookies)
                {
                    Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                }

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
            WebApiUserResult result = await account.DetailsAsync();

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return RedirectToAction("Index", "Home");
            }

            WebApiUser user = result.User;

            return View(user);
        }

        [Route("PostUserImage")]
        public async Task<ActionResult> PostUserImage(PostImage image)
        {
            var result = new WebApiResult();

            if (image.File != null)
            {
                // ZA SPREMANJE NA SERVER
                //string image = System.IO.Path.GetFileName(file.FileName);
                //string path = System.IO.Path.Combine(Server.MapPath("sad tu napisem path npr. ~/images/user il tak nest"), image);
                //file.SaveAs(path);
                //i u bazu stavim adresu.

                //ZA SPREMANJE NA BAZU
                using (MemoryStream ms = new MemoryStream())
                {
                    image.File.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();

                    float mb = (array.Length / 1024f) / 1024f; 

                    if (mb < 1)
                    {
                        WebApiPostImage webApiImage = new WebApiPostImage
                        {
                            Id = image.Id,
                            Image = array

                        };

                        result = await account.PostUserImageAsync(webApiImage);
                    }
                    else
                    {
                        byte[] arrayScaled = ImageProcessor.To1MB(array);
                        float mbScaled = (arrayScaled.Length / 1024f) / 1024f;

                        if (arrayScaled != null)
                        {
                            WebApiPostImage webApiImage = new WebApiPostImage
                            {
                                Id = image.Id,
                                Image = arrayScaled

                            };

                            result = await account.PostUserImageAsync(webApiImage);
                        }
                    }
                }
            }

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [Route("GetUserImage/{id}")]
        public async Task<ActionResult> GetUserImage(string id)
        {
            byte[] byteArray = await account.GetUserImageAsync(id);

            return File(byteArray, "image/png");
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<ActionResult> Edit()
        {
            var result = new WebApiUserResult();

            result = await account.EditAsync();

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return RedirectToAction("Index", "Home");
            }

            WebApiUser user = result.User;

            return View(user);
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<ActionResult> Edit(WebApiUser user)
        {
            WebApiResult result = await account.EditAsync(user);

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
            WebApiUserResult result = await account.DetailsAsync();

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return RedirectToAction("Details");
            }

            WebApiUser user = result.User;

            return View(user);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult> Delete(WebApiUser user)
        {
            WebApiResult result = await account.DeleteAsync(user);

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
    }
}