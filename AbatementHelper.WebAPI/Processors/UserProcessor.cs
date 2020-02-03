using AbatementHelper.CommonModels.CreateModels;
using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.Models;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.WebAPI.Processors
{
    public static class UserProcessor
    {
        public static async Task<WebApiUser> ApplicationUserToWebApiUser(ApplicationUser user)
        {
            WebApiUser webApiUser = new WebApiUser();

            IList<string> roles = new List<string>();

            using (UserManager userManager = new UserManager())
            {
                roles = await userManager.GetRolesAsync(user.Id);
            }

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<ApplicationUser, WebApiUser>()
                    .ForMember(u => u.Role, act => act.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                webApiUser = mapper.Map<ApplicationUser, WebApiUser>(user);

                if (webApiUser != null)
                {
                    webApiUser.Role = roles.FirstOrDefault();
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return webApiUser;
        }

        public static async Task<ApplicationUser> WebApiUserToApplicationUser(WebApiUser user)
        {
            ApplicationUser applicationUser = new ApplicationUser();

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<WebApiUser, ApplicationUser>()
                    .ForMember(u => u.Roles, act => act.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                applicationUser = mapper.Map<WebApiUser, ApplicationUser>(user);

                if (applicationUser != null)
                {
                    using (UserManager userManager = new UserManager())
                    {
                        if (user.Role != null)
                        {
                            var roles = await userManager.GetRolesAsync(user.Id);
                            userManager.RemoveFromRole(user.Id, roles.FirstOrDefault());
                            userManager.AddToRole(user.Id, user.Role);
                        }
                    }
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return applicationUser;
        }

        public static async Task<ApplicationUser> CreateUserModelToApplicationUser(CreateUserModel user)
        {
            var applicationUser = new ApplicationUser();

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<CreateUserModel, ApplicationUser>()
                    .ForMember(m => m.UserName, act => act.Ignore())
                    .ForMember(m => m.Id, act => act.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                applicationUser = mapper.Map<CreateUserModel, ApplicationUser>(user);

                if (applicationUser != null && applicationUser.Email != null)
                {
                    applicationUser.Id = Guid.NewGuid().ToString();
                    applicationUser.UserName = user.Email.Split('@')[0];
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return applicationUser;
        }
    }
}