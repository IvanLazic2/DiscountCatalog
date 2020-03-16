//using DiscountCatalog.Common.CreateModels;
//using DiscountCatalog.Common.WebApiModels;
//using DiscountCatalog.WebAPI.DataBaseModels;
//using DiscountCatalog.WebAPI.Models;
//using DiscountCatalog.WebAPI.Repositories;
//using AutoMapper;
//using Microsoft.AspNet.Identity;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;

//namespace DiscountCatalog.WebAPI.Processors
//{
//    public static class ManagerProcessor
//    {
//        public static async Task<ApplicationUser> WebApiUserToApplicationUserAsync(WebApiUser user)
//        {
//            var applicationUser = new ApplicationUser();

//            var config = new MapperConfiguration(c =>
//            {
//                c.CreateMap<WebApiUser, ApplicationUser>()
//                    .ForMember(m => m.Roles, act => act.Ignore());
//            });

//            IMapper mapper = config.CreateMapper();

//            try
//            {
//                applicationUser = mapper.Map<WebApiUser, ApplicationUser>(user);

//                if (applicationUser != null)
//                {
//                    using (var userManager = new UserManager())
//                    {
//                        await userManager.AddToRoleAsync(user.Id, "Manager");
//                    }
//                }
//            }
//            catch (Exception exception)
//            {

//                throw;
//            }

//            return applicationUser;
//        }

//        public static ApplicationUser CreateManagerModelToApplicationUser(CreateManagerModel manager)
//        {
//            var user = new ApplicationUser();

//            var config = new MapperConfiguration(c =>
//            {
//                c.CreateMap<CreateManagerModel, ApplicationUser>()
//                    .ForMember(m => m.UserName, act => act.Ignore())
//                    .ForMember(m => m.Id, act => act.Ignore());
//            });

//            IMapper mapper = config.CreateMapper();

//            try
//            {
//                user = mapper.Map<CreateManagerModel, ApplicationUser>(manager);

//                if (user != null && user.Email != null)
//                {
//                    user.Id = Guid.NewGuid().ToString();
//                    user.UserName = manager.Email.Split('@')[0];
//                }
//            }
//            catch (Exception exception)
//            {

//                throw;
//            }

//            return user;
//        }

//        public static async Task<WebApiManager> ManagerEntityToWebApiManagerAsync(Manager manager)
//        {
//            StoreAdminRepository storeAdminRepository = new StoreAdminRepository();

//            WebApiManager webApiManager = new WebApiManager();

//            IList<string> roles = new List<string>();

//            using (UserManager userManager = new UserManager())
//            {
//                roles = userManager.GetRoles(manager.User.Id);
//            }

//            var config = new MapperConfiguration(c =>
//            {
//                c.CreateMap<ApplicationUser, WebApiManager>()
//                    .ForMember(m => m.Stores, act => act.Ignore());
//            });

//            IMapper mapper = config.CreateMapper();

//            try
//            {
//                webApiManager = mapper.Map<ApplicationUser, WebApiManager>(manager.User);

//                if (manager.Stores != null)
//                {
//                    webApiManager.Stores = await storeAdminRepository.GetManagerStoresAsync(manager.Id);
//                }
//            }
//            catch (Exception exception)
//            {

//                throw;
//            }

//            return webApiManager;
//        }

//        public static async Task<WebApiManagerStore> CreateWebApiManagerStoreAsync(Store store, Manager manager, bool assigned)
//        {
//            WebApiStore webApiStore = await StoreProcessor.StoreEntityToWebApiStoreAsync(store);

//            WebApiManager webApiManager = await ManagerEntityToWebApiManagerAsync(manager);

//            return new WebApiManagerStore
//            {
//                Store = webApiStore,
//                Manager = webApiManager,
//                Assigned = assigned
//            };
//        }  

//        public static async Task<WebApiManager> ApplicationUserToWebApiManagerAsync(ApplicationUser user)
//        {
//            var config = new MapperConfiguration(c =>
//            {
//                c.CreateMap<ApplicationUser, WebApiManager>()
//                    .ForMember(m => m.Stores, act => act.Ignore());
//            });

//            IMapper mapper = config.CreateMapper();

//            try
//            {
//                WebApiManager webApiManager = mapper.Map<ApplicationUser, WebApiManager>(user);

//                return webApiManager;
//            }
//            catch (Exception exception)
//            {

//                throw;
//            }
//        }
//    }
//}