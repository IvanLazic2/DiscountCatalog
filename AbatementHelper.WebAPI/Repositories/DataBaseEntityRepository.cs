using AbatementHelper.CommonModels.Models;
using AbatementHelper.WebAPI.Models;
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
        public IdentityUser ReadUserById(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                var user = (from u in context.Users
                             where u.Id == id
                             select u).FirstOrDefault();

                return user;
            }
        }

        public async Task<string> ReadRole(string id)
        {
            //using (var context = new ApplicationUserDbContext())
            //{
            //    ApplicationUser user = (ApplicationUser)context.Users.Where(u => u.Email == email).SingleOrDefault();

            //    return new DataBaseResult()
            //    {
            //        Value = user.Role,
            //        Message = "Querry successful",
            //        Success = true
            //    };
            //}


            using (var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationUserDbContext())))
            {
                var roles = await userManager.GetRolesAsync(id);
                
                var rolesArray = new List<string>(roles).ToArray();

                if (roles.Count != 1)
                {
                    return "Error";
                }
                else
                {
                    return roles.First();
                }
            }

        }

        public static DataBaseResult ReadEmail(string email)
        {

            using (var context = new ApplicationUserDbContext())
            {
                var user = (from u in context.Users where u.Email == email select u).FirstOrDefault();

                return new DataBaseResult()
                {
                    Value = user.Email,
                    Message = "Query successful",
                    Success = true
                };
            }
        }

        public static DataBaseResult ReadUserName(string email)
        {
            using (var context = new ApplicationUserDbContext())
            {
                var user = (from u in context.Users where u.Email == email select u).FirstOrDefault();

                return new DataBaseResult()
                {
                    Value = user.UserName,
                    Message = "Query successful",
                    Success = true
                };
            }
        }

        public static List<IdentityUser> ReadUsers(string email)
        {
            using (var context = new ApplicationUserDbContext())
            {
                var users = (from u in context.Users
                             where u.Email == email
                             select u).ToList();

                return users;
            }
        }
    }


}