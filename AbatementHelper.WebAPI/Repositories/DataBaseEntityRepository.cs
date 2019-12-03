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
                var result = context.Entry(UserProcessor.WebApiUserToApplicationUser(user)).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteUser(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = context.Users.Find(id);
                user.Deleted = true;
                context.SaveChanges();
            }
        }

        public void RestoreUser(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = context.Users.Find(id);
                user.Deleted = false;
                context.SaveChanges();
            }
        }
    }


}