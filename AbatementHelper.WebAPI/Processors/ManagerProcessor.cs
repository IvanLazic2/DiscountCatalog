using AbatementHelper.CommonModels.CreateModels;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Repositories;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.WebAPI.Processors
{
    public static class ManagerProcessor
    {
        public static async Task<ApplicationUser> WebApiUserToApplicationUserAsync(WebApiUser user)
        {
            var applicationUser = new ApplicationUser();

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<WebApiUser, ApplicationUser>()
                    .ForMember(m => m.Roles, act => act.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                applicationUser = mapper.Map<WebApiUser, ApplicationUser>(user);

                if (applicationUser != null)
                {
                    using (var userManager = new UserManager())
                    {
                        await userManager.AddToRoleAsync(user.Id, "Manager");
                    }
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return applicationUser;

            //return new ApplicationUser
            //{
            //    UserName = user.UserName,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    Email = user.Email,
            //    PhoneNumber = user.PhoneNumber,
            //    Country = user.Country,
            //    City = user.City,
            //    PostalCode = user.PostalCode,
            //    Street = user.Street,

            //    TwoFactorEnabled = user.TwoFactorEnabled,
            //};
        }

        public static ApplicationUser CreateManagerModelToApplicationUser(CreateManagerModel manager)
        {
            var user = new ApplicationUser();

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<CreateManagerModel, ApplicationUser>()
                    .ForMember(m => m.UserName, act => act.Ignore())
                    .ForMember(m => m.Id, act => act.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                user = mapper.Map<CreateManagerModel, ApplicationUser>(manager);

                if (user != null)
                {
                    user.Id = Guid.NewGuid().ToString();
                    user.UserName = manager.Email.Split('@')[0];
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return user;
        }

        public static async Task<WebApiManager> ManagerEntityToWebApiManagerAsync(ManagerEntity manager)
        {
            StoreAdminRepository storeAdminRepository = new StoreAdminRepository();

            WebApiManager webApiManager = new WebApiManager();

            IList<string> roles = new List<string>();

            using (UserManager userManager = new UserManager())
            {
                roles = userManager.GetRoles(manager.User.Id);
            }

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<ApplicationUser, WebApiManager>()
                    .ForMember(m => m.Stores, act => act.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                webApiManager = mapper.Map<ApplicationUser, WebApiManager>(manager.User);

                if (manager.Stores != null)
                {
                    webApiManager.Stores = await storeAdminRepository.GetManagerStoresAsync(manager.Id);
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return webApiManager;
        }


    }
}