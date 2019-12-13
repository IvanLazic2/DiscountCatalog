using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.WebAPI.Repositories
{
    public class AdminRepository
    {
        private UserManager userManager = new UserManager();

        public List<ApplicationUser> ReadAllUsers()
        {
            return new UserManager().Users.ToList();
        }

        public Response Edit(WebApiUser user)
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
                    userEntity.EmailConfirmed = user.EmailConfirmed;
                    userEntity.PhoneNumber = user.PhoneNumber;
                    userEntity.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                    userEntity.Country = user.Country;
                    userEntity.City = user.City;
                    userEntity.PostalCode = user.PostalCode;
                    userEntity.Street = user.Street;
                    userEntity.TwoFactorEnabled = user.TwoFactorEnabled;
                    userEntity.Approved = user.Approved;
                    userEntity.Deleted = user.Deleted;

                    if (user.Role != null)
                    {
                        var roles = new UserManager().GetRoles(user.Id);
                        if (roles[0] != user.Role)
                        {
                            new UserManager().RemoveFromRole(user.Id, roles[0]);
                            new UserManager().AddToRole(user.Id, user.Role);
                        }
                    }

                    context.SaveChanges();

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

        public async Task<ApplicationUser> ReadUserById(string id)
        {
            return await userManager.FindByIdAsync(id);

            //using (var context = new ApplicationUserDbContext())
            //{
            //    var user = (from u in context.Users
            //                where u.Id == id
            //                select u).FirstOrDefault();

            //    return user;
            //}
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