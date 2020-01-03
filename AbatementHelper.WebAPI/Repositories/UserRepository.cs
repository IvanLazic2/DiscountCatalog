using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.WebAPI.Repositories
{
    public class UserRepository
    {
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

        public async Task<ApplicationUser> ReadUserByIdAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = await context.Users.FindAsync(id);

                return user;
            }
        }

        public async Task<Response> Edit(WebApiUser user)
        {
            var response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser userEntity = context.Users.Find(user.Id);

                    context.Users.Attach(userEntity);

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

                    await context.SaveChangesAsync();

                    response.Message = "Successfully edited.";
                    response.Success = true;
                }
            }
            catch (DbUpdateException exception)
            {
                response.Message = exception.InnerException.InnerException.Message;
                response.Success = false;
            }

            return response;

        }

        public async Task<Response> PostUserImageAsync(WebApiPostImage user)
        {
            var response = new Response();

            byte[] image = user.Image;

            if (ImageProcessor.IsValid(image))
            {
                try
                {
                    using (var context = new ApplicationUserDbContext())
                    {
                        ApplicationUser applicationUser = await context.Users.FindAsync(user.Id);

                        context.Users.Attach(applicationUser);
                        applicationUser.UserImage = image;

                        context.SaveChanges();
                    }
                }
                catch (Exception exception)
                {
                    response.Message = exception.InnerException.InnerException.Message;
                    response.Success = false;
                }

                response.Message = "Successfully uploaded.";
                response.Success = true;
            }
            else
            {
                response.Message = "Invalid image type";
                response.Success = false;
            }


            return response;
        }

        public async Task<byte[]> GetUserImageAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = await context.Users.FindAsync(id);

                return user.UserImage;
            }

        }

        public async Task<ApplicationUser> ReturnUser(string userName)
        {
            using (var userManager = new UserManager())
            {
                ApplicationUser user = await userManager.FindByNameAsync(userName);

                return user;
            }
        }

        public async Task DeleteAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = await context.Users.FindAsync(id);

                context.Users.Attach(user);
                user.Deleted = true;

                await context.SaveChangesAsync();
            }
        }

        public async Task RestoreAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = await context.Users.FindAsync(id);

                context.Users.Attach(user);
                user.Deleted = false;

                await context.SaveChangesAsync();
            }
        }
    }
}