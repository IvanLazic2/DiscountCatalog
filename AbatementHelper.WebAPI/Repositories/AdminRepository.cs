using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public async Task<List<ApplicationUser>> ReadAllUsersAsync()
        {
            using (var context = new ApplicationUserDbContext())
            {
                List<ApplicationUser> users = await context.Users.ToListAsync();

                return users;
            }
        }

        public async Task<Response> EditAsync(WebApiUser user)
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
                        using (var userManager = new UserManager())
                        {
                            IList<string> roles = await userManager.GetRolesAsync(user.Id);

                            List<IdentityRole> identityRoles = await context.Roles.ToListAsync();

                            IdentityRole existingRole = identityRoles.FirstOrDefault(r => r.Name == user.Role);

                            if (roles.FirstOrDefault() != user.Role && existingRole != null)
                            {
                                await userManager.RemoveFromRoleAsync(user.Id, roles.FirstOrDefault());

                                await userManager.AddToRoleAsync(user.Id, user.Role);
                            }
                        }
                    }

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

        public async Task<ApplicationUser> ReadUserById(string id)
        {
            using (var userManager = new UserManager())
            {
                ApplicationUser user = await userManager.FindByIdAsync(id);

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