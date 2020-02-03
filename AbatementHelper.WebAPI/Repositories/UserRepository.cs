using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.Extentions;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.WebAPI.Repositories
{
    public class UserRepository
    {
        private HttpClient apiClient;

        public UserRepository()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<WebApiAuthenticatedUserResult> AuthenticateAsync(string username, string password)
        {
            var result = new WebApiAuthenticatedUserResult();

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            HttpResponseMessage response = await apiClient.PostAsync("/token", data);

            AuthenticatedUser authenticatedUser = await response.Content.ReadAsAsync<AuthenticatedUser>();

            if (authenticatedUser == null || !response.IsSuccessStatusCode)
            {
                result.AddModelError(string.Empty, "Incorrect password."); //////////
            }
            else
            {
                result.User = authenticatedUser;
            }

            return result;
        }

        public async Task<string> ReturnUserNameAsync(string usernameOrEmail)
        {
            string username = usernameOrEmail;

            if (usernameOrEmail.Contains("@"))
            {
                using (var userManager = new UserManager())
                {
                    ApplicationUser userForEmail = await userManager.FindByEmailAsync(usernameOrEmail);

                    if (userForEmail != null)
                    {
                        username = userForEmail.UserName;
                    }
                }
            }

            return username;
        }

        public async Task<WebApiUserResult> ReadUserByIdAsync(string id)
        {
            var result = new WebApiUserResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = await context.Users.FindAsync(id);

                    if (user != null)
                    {
                        WebApiUser processedUser = await UserProcessor.ApplicationUserToWebApiUser(user);

                        result.User = processedUser;
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "User does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiResult> EditAsync(WebApiUser user)
        {
            var result = new WebApiResult();

            try
            {
                using (var userManager = new UserManager())
                {
                    ApplicationUser userEntity = await userManager.FindByIdAsync(user.Id);

                    userEntity.UserName = user.UserName;
                    userEntity.FirstName = user.FirstName;
                    userEntity.LastName = user.LastName;
                    userEntity.Email = user.Email;
                    userEntity.PhoneNumber = user.PhoneNumber;
                    userEntity.Country = user.Country;
                    userEntity.City = user.City;
                    userEntity.PostalCode = user.PostalCode;
                    userEntity.Street = user.Street;
                    userEntity.TwoFactorEnabled = user.TwoFactorEnabled;

                    IdentityResult updateResult = await userManager.UpdateAsync(userEntity);

                    if (!updateResult.Succeeded)
                    {
                        foreach (var error in updateResult.Errors)
                        {
                            result.AddModelError(string.Empty, error);
                        }
                    }

                    result.Message = "Account info updated.";
                }
            }
            catch (Exception exception)
            {
                string type = exception.GetType().ToString();

                if (exception is DbUpdateException)
                {
                    DbUpdateException ex = exception as DbUpdateException;

                    var entities = ex.Entries;

                    foreach (var entity in entities)
                    {
                        ApplicationUser userEntitiy = entity.Entity as ApplicationUser;

                        using (var userManager = new UserManager())
                        {
                            ApplicationUser originalEntity = await userManager.FindByIdAsync(userEntitiy.Id);

                            if (originalEntity.Email != userEntitiy.Email)
                            {
                                result.AddModelError(ObjectExtensions.GetPropertyName(() => userEntitiy.Email), "Email is already taken.");
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task<WebApiResult> PostUserImageAsync(WebApiPostImage user)
        {
            var result = new WebApiResult();

            byte[] image = user.Image;

            if (image != null)
            {
                if (ImageProcessor.IsValid(image))
                {
                    try
                    {
                        using (var context = new ApplicationUserDbContext())
                        {
                            ApplicationUser applicationUser = await context.Users.FindAsync(user.Id);

                            if (applicationUser != null)
                            {
                                context.Users.Attach(applicationUser);
                                applicationUser.UserImage = image;

                                await context.SaveChangesAsync();

                                result.Message = "Image saved.";
                            }
                            else
                            {
                                result.AddModelError(string.Empty, "User does not exist.");
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        result.Exception = exception;
                        result.AddModelError(string.Empty, "An exception has occured.");
                    }
                }
                else
                {
                    result.AddModelError(string.Empty, "Invalid image type.");
                }
            }
            else
            {
                result.AddModelError(string.Empty, "Image is empty.");
            }

            return result;
        }

        public async Task<byte[]> GetUserImageAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = await context.Users.FindAsync(id);

                return user.UserImage;
            }

        }

        public async Task<ApplicationUser> ReturnUserByUserNameAsync(string userName)
        {
            using (var userManager = new UserManager())
            {
                ApplicationUser user = await userManager.FindByNameAsync(userName);

                return user;
            }
        }

        public async Task<WebApiResult> DeleteAsync(string id)
        {
            var result = new WebApiResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = await context.Users.FindAsync(id);

                    if (user != null)
                    {
                        context.Users.Attach(user);
                        user.Deleted = true;

                        await context.SaveChangesAsync();

                        result.Message = "Account deleted.";
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "User does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiResult> RestoreAsync(string id)
        {
            var result = new WebApiResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = await context.Users.FindAsync(id);

                    if (user != null)
                    {
                        context.Users.Attach(user);
                        user.Deleted = false;

                        await context.SaveChangesAsync();

                        result.Message = "Account restored.";
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "User does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }
    }
}