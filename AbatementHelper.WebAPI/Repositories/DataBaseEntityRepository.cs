using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.WebAPI.Repositories
{
    public class DataBaseEntityRepository
    {


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
                    Message = "Query successful",
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
                    Message = "Query successful",
                    Success = true
                };
            }
        }

        public List<ApplicationUser> ReadAllUsers()
        {
            return new UserManager().Users.ToList();
        }

        public void EditUser(WebApiUser user)
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
            }
        }

        public void DeleteUser(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = context.Users.Find(id);
                context.Users.Attach(user);
                user.Deleted = true;
                context.SaveChanges();
            }
        }

        public void RestoreUser(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = context.Users.Find(id);
                context.Users.Attach(user);
                user.Deleted = false;
                context.SaveChanges();
            }
        }

        //User

        public void Edit(WebApiUser user)
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

                context.SaveChanges();
            }
        }
    }


}