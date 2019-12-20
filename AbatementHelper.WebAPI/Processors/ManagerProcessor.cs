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
using System.Web;

namespace AbatementHelper.WebAPI.Processors
{
    public static class ManagerProcessor
    {
        public static ApplicationUser WebApiUserToApplicationUser(WebApiUser user)
        {
            UserManager userManager = new UserManager();

            try
            {
                userManager.AddToRole(user.Id, "Manager");
            }
            finally
            {
                userManager.Dispose();
            }

            return new ApplicationUser
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Country = user.Country,
                City = user.City,
                PostalCode = user.PostalCode,
                Street = user.Street,

                TwoFactorEnabled = user.TwoFactorEnabled,
            };
        }

        public static ApplicationUser CreateManagerModelToApplicationUser(CreateManagerModel manager)
        {
            return new ApplicationUser
            {
                UserName = manager.UserName,
                FirstName = manager.FirstName,
                LastName = manager.LastName,
                Email = manager.Email,
                PhoneNumber = manager.PhoneNumber,
                Country = manager.Country,
                City = manager.City,
                PostalCode = manager.PostalCode,
                Street = manager.Street,
                TwoFactorEnabled = manager.TwoFactorEnabled
            };
        }

        public static WebApiManager ManagerEntityToWebApiManager(ManagerEntity manager)
        {
            StoreAdminRepository storeAdminRepository = new StoreAdminRepository();

            WebApiManager webApiManager = new WebApiManager();

            IList<string> roles = new List<string>();

            UserManager userManager = new UserManager();

            try
            {
                roles = userManager.GetRoles(manager.User.Id);
            }
            finally
            {
                userManager.Dispose();
            }

            var config = new MapperConfiguration(c => {
                c.CreateMap<ApplicationUser, WebApiManager>()
                    .ForMember(m => m.Stores, act => act.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                webApiManager = mapper.Map<ApplicationUser, WebApiManager>(manager.User);
                webApiManager.Stores = storeAdminRepository.GetManagerStores(manager.Id);
            }
            catch (Exception exception)
            {

                throw;
            }
            
            return webApiManager;
        }


    }
}