using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.WebAPI.Repositories
{
    public class UserRepository
    {
        private UserManager userManager = new UserManager();

        public async Task<string> ReturnUserName(UserManager userManager, string usernameOrEmail)
        {
            var username = usernameOrEmail;
            if (usernameOrEmail.Contains("@"))
            {
                var userForEmail = await userManager.FindByEmailAsync(usernameOrEmail);
                if (userForEmail != null)
                {
                    username = userForEmail.UserName;
                }
            }
            //return await userManager.FindAsync(username, password);
            return username;
        }

        public ApplicationUser ReadUserById(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                var user = (from u in context.Users
                            where u.Id == id
                            select u).FirstOrDefault();

                return user;
            }
        }

        public static WebApiResult ReadEmail(string email)
        {

            using (var context = new ApplicationUserDbContext())
            {
                var user = (from u in context.Users where u.Email == email select u).FirstOrDefault();

                return new WebApiResult()
                {
                    Value = user.Email,
                    Message = "Query successfull",
                    Success = true
                };
            }
        }

        public static WebApiResult ReadUserName(string email)
        {
            using (var context = new ApplicationUserDbContext())
            {
                var user = (from u in context.Users where u.Email == email select u).FirstOrDefault();

                return new WebApiResult()
                {
                    Value = user.UserName,
                    Message = "Query successfull",
                    Success = true
                };
            }
        }

        public async Task<Response> Edit(WebApiUser user)
        {
            Response response = new Response();
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

                    response.ResponseMessage = "Successfully edited.";
                    response.Success = true;
                }
            }
            catch (DbUpdateException exception)
            {
                response.ResponseMessage = exception.InnerException.InnerException.Message;
                response.Success = false;
            }

            return response;

        }

        public Response UploadUserImage(WebApiUser user)
        {
            Response response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser applicationUser = context.Users.Find(user.Id);

                    applicationUser.UserImage = user.Image;

                    context.Users.Attach(applicationUser);

                    context.SaveChanges();
                }

                response.ResponseMessage = "Successfully edited.";
                response.Success = true;
            }
            catch (Exception exception)
            {
                response.ResponseMessage = exception.InnerException.InnerException.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<ApplicationUser> ReturnUser(string userName)
        {
            var user = userManager.FindByNameAsync(userName);
            
            return await user;
        }

       

        public void Delete(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = context.Users.Find(id);
                context.Users.Attach(user);
                user.Deleted = true;
                context.SaveChanges();
            }
        }

        public void Restore(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = context.Users.Find(id);
                context.Users.Attach(user);
                user.Deleted = false;
                context.SaveChanges();
            }
        }
    }
}