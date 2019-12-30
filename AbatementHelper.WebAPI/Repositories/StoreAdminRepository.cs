using AbatementHelper.CommonModels.CreateModels;
using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace AbatementHelper.WebAPI.Repositories
{
    public class StoreAdminRepository
    {

        public async Task<ApplicationUser> ReadUserById(string id)
        {
            using (var userManager = new UserManager())
            {
                return await userManager.FindByIdAsync(id);
            }
            //using (var context = new ApplicationUserDbContext())
            //{
            //    var user = (from u in context.Users
            //                where u.Id == id
            //                select u).FirstOrDefault();

            //    return user;
            //}
        }

        public async Task<StoreEntity> ReadStoreById(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                var store = context.Stores.Find(id);
                context.Stores.Include(s => s.Managers).ToList();

                return store;
            }
        }

        public ApplicationUser FindUserByManagerId(string id)
        {
            ApplicationUser user = new ApplicationUser();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ManagerEntity manager = context.Managers.Find(id);

                    context.Entry(manager).Reference(m => m.User).Load();

                    user = manager.User;
                }
            }
            catch (Exception exception)
            {

            }

            return user;
        }

        public ManagerEntity FindManagerByUserId(string id)
        {
            ManagerEntity manager = new ManagerEntity();
            List<StoreEntity> stores = new List<StoreEntity>();

            using (var context = new ApplicationUserDbContext())
            {
                manager = context.Managers.Include(m => m.Stores)
                                          .Include(m => m.StoreAdmin).FirstOrDefault(m => m.User.Id == id);

            }

            return manager;
        }

        public List<WebApiManager> GetStoreManagers(string id)
        {
            List<WebApiManager> webApiManagers = new List<WebApiManager>();

            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity store = context.Stores.Find(id);
                context.Stores.Include(s => s.Managers).ToList();

                foreach (var manager in store.Managers)
                {
                    context.Entry(manager).Reference(m => m.User).Load();

                    webApiManagers.Add
                    (
                        new WebApiManager
                        {
                            Id = manager.User.Id,
                            UserName = manager.User.UserName
                        }
                    );
                }
            }

            return webApiManagers;
        }

        public List<WebApiStore> GetManagerStores(string id)
        {
            List<WebApiStore> webApiStores = new List<WebApiStore>();

            using (var context = new ApplicationUserDbContext())
            {
                ManagerEntity manager = context.Managers.Find(id);
                context.Managers.Include(m => m.Stores).ToList();

                foreach (var store in manager.Stores)
                {
                    webApiStores.Add
                    (
                        new WebApiStore
                        {
                            Id = store.Id,
                            StoreName = store.StoreName
                        }
                    );
                }
            }

            return webApiStores;
        }

        public List<WebApiStore> GetAllStores(string storeAdminId)
        {
            List<WebApiStore> stores = new List<WebApiStore>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {

                    foreach (var store in context.Stores.Where(s => s.StoreAdmin.Id == storeAdminId).Include(s => s.Managers).ToList())
                    {
                        //context.Stores.Include(s => s.Managers);

                        if (!store.Deleted && !store.Approved)
                        {
                            store.StoreAdmin = context.Users.Find(storeAdminId);

                            stores.Add(StoreProcessor.StoreEntityToWebApiStore(store));
                        }
                    }

                }
            }
            catch (Exception exception)
            {

            }

            return stores;
        }

        public Response CreateStore(WebApiStore store)
        {
            Response response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {

                    StoreEntity processedStore = StoreProcessor.WebApiStoreToStoreEntity(store);
                    processedStore.StoreAdmin = context.Users.Find(store.StoreAdminId);
                    context.Stores.Add(processedStore);

                    context.SaveChanges();

                    response.ResponseMessage = "Successfully created";
                    response.Success = true;

                    //try
                    //{
                    //    context.SaveChanges();
                    //}
                    //catch (DbEntityValidationException ex)
                    //{
                    //    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    //    {
                    //        // Get entry

                    //        DbEntityEntry entry = item.Entry;
                    //        string entityTypeName = entry.Entity.GetType().Name;

                    //        // Display or log error messages

                    //        foreach (DbValidationError subItem in item.ValidationErrors)
                    //        {
                    //            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                    //                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                    //            Debug.WriteLine(message);
                    //        }
                    //    }
                    //}


                }
            }
            catch (DbUpdateException exception)
            {
                response.ResponseMessage = exception.InnerException.InnerException.Message;
                response.Success = false;
            }

            return response;
        }

        public Response EditStore(WebApiStore store)
        {
            Response response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    StoreEntity storeEntity = context.Stores.Find(store.Id);
                    context.Stores.Attach(storeEntity);

                    storeEntity.StoreName = store.StoreName;
                    storeEntity.WorkingHoursWeek = store.WorkingHoursWeek;
                    storeEntity.WorkingHoursWeekends = store.WorkingHoursWeekends;
                    storeEntity.WorkingHoursHolidays = store.WorkingHoursHolidays;
                    storeEntity.Country = store.Country;
                    storeEntity.City = store.City;
                    storeEntity.PostalCode = store.PostalCode;
                    storeEntity.Street = store.Street;

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

        public Response PostStoreImage(WebApiStore store)
        {
            Response response = new Response();

            byte[] image = store.StoreImage;

            if (ImageProcessor.IsValid(image))
            {
                try
                {
                    using (var context = new ApplicationUserDbContext())
                    {
                        StoreEntity storeEntity = context.Stores.Find(store.Id);

                        context.Stores.Attach(storeEntity);
                        storeEntity.StoreImage = image;

                        context.SaveChanges();
                    }
                }
                catch (Exception exception)
                {
                    response.ResponseMessage = exception.InnerException.InnerException.Message;
                    response.Success = false;
                }

                response.ResponseMessage = "Successfully uploaded.";
                response.Success = true;
            }
            else
            {
                response.ResponseMessage = "Invalid image type";
                response.Success = false;
            }


            return response;
        }

        public byte[] GetStoreImage(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity store = context.Stores.Find(id);

                return store.StoreImage;
            }

        }

        public void DeleteStore(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity store = context.Stores.Find(id);
                context.Stores.Attach(store);
                store.Deleted = true;
                context.SaveChanges();
            }
        }

        public List<WebApiStore> GetAllDeletedStores(string storeAdminId)
        {
            using (var context = new ApplicationUserDbContext())
            {
                List<WebApiStore> stores = new List<WebApiStore>();

                foreach (var store in context.Stores.Where(s => s.StoreAdmin.Id == storeAdminId && s.Deleted).ToList())
                {
                    store.StoreAdmin = context.Users.Find(storeAdminId);

                    stores.Add(StoreProcessor.StoreEntityToWebApiStore(store));

                }

                return stores;
            }
        }

        public void RestoreStore(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity store = context.Stores.Find(id);
                context.Stores.Attach(store);
                store.Deleted = false;
                context.SaveChanges();
            }
        }

        public SelectedStore SelectStore(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                var store = context.Stores.Find(id);

                return new SelectedStore
                {
                    Id = store.Id,
                    StoreName = store.StoreName
                };
            }
        }

        public List<WebApiManager> GetAllManagers(string storeAdminId)
        {
            using (var context = new ApplicationUserDbContext())
            {
                List<WebApiManager> managers = new List<WebApiManager>();

                List<ManagerEntity> managerEntites = context.Managers.Where(s => s.StoreAdmin.Id == storeAdminId).ToList();

                foreach (var manager in managerEntites)
                {
                    context.Entry(manager).Reference(m => m.User).Load();
                    context.Entry(manager).Collection(m => m.Stores).Load();

                    if (!manager.User.Deleted && !manager.User.Approved)
                    {
                        //context.Entry(manager).Reference(m => m.User).Load();
                        managers.Add(ManagerProcessor.ManagerEntityToWebApiManager(manager));
                    }
                }

                return managers;
            }
        }

        public Response CreateManager(CreateManagerModel user, string password)
        {
            Response response = new Response();

            ApplicationUser processedUser = ManagerProcessor.CreateManagerModelToApplicationUser(user);

            try
            {
                using (var userManager = new UserManager())
                {
                    userManager.Create(processedUser, password);
                    userManager.AddToRole(processedUser.Id, "Manager");
                }

                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser applicationUser = context.Users.Find(processedUser.Id);
                    ApplicationUser storeAdmin = context.Users.Find(user.StoreAdminId);

                    context.Managers.Add(new ManagerEntity
                    {
                        User = applicationUser,
                        StoreAdmin = storeAdmin
                    });

                    context.SaveChanges();
                }

                response.ResponseMessage = "Created successfully";
                response.Success = true;
            }
            catch (DbEntityValidationException exception)
            {
                var ex = ExceptionProcessor.processException(exception);
                response.ResponseMessage = ex.Message;
                response.Success = false;
            }

            return response;
        }



        public async Task<Response> EditManager(WebApiManager user)
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
            catch (DbEntityValidationException exception)
            {
                var ex = ExceptionProcessor.processException(exception);
                response.ResponseMessage = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public void DeleteManager(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = context.Users.Find(id);
                context.Users.Attach(user);
                user.Deleted = true;
                context.SaveChanges();
            }
        }

        public List<WebApiManager> GetAllDeletedManagers(string storeAdminId)
        {
            using (var context = new ApplicationUserDbContext())
            {
                List<WebApiManager> managers = new List<WebApiManager>();

                List<ManagerEntity> managerEntityList = context.Managers.Where(m => m.StoreAdmin.Id == storeAdminId).ToList();

                foreach (var manager in managerEntityList)
                {
                    context.Entry(manager).Reference(m => m.User).Load();

                    if (manager.User.Deleted)
                    {
                        managers.Add(ManagerProcessor.ManagerEntityToWebApiManager(manager));
                    }
                }

                return managers;
            }
        }

        public void RestoreManager(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = context.Users.Find(id);
                context.Users.Attach(user);
                user.Deleted = false;
                context.SaveChanges();
            }
        }

        public List<WebApiManagerStore> GetAllManagerStores(string managerId)
        {
            List<WebApiManagerStore> managerStores = new List<WebApiManagerStore>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = context.Users.Find(managerId);

                    ManagerEntity managerEntity = context.Managers.Where(m => m.User.Id == user.Id).FirstOrDefault();
                    context.Entry(managerEntity).Collection(m => m.Stores).Load();
                    context.Entry(managerEntity).Reference(m => m.StoreAdmin).Load();

                    List<StoreEntity> storeEntities = context.Stores.ToList();

                    foreach (var store in storeEntities)
                    {
                        context.Entry(store).Reference(s => s.StoreAdmin).Load();

                        StoreEntity storeEntity = managerEntity.Stores.Where(s => s.Id == store.Id).FirstOrDefault();

                        if (store.StoreAdmin.Id == managerEntity.StoreAdmin.Id)
                        {
                            if (storeEntity != null)
                            {
                                managerStores.Add(new WebApiManagerStore
                                {
                                    Store = StoreProcessor.StoreEntityToWebApiStore(storeEntity),
                                    Manager = ManagerProcessor.ManagerEntityToWebApiManager(managerEntity),
                                    Assigned = true
                                });
                            }
                            else
                            {
                                managerStores.Add(new WebApiManagerStore
                                {
                                    Store = StoreProcessor.StoreEntityToWebApiStore(store),
                                    Manager = ManagerProcessor.ManagerEntityToWebApiManager(managerEntity),
                                    Assigned = false
                                });
                            }
                        }

                    }
                }

            }
            catch (Exception exception)
            {

            }

            return managerStores;
        }

        public Response AssignStore(WebApiStoreAssign storeAssign)
        {
            Response response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = context.Users.Find(storeAssign.ManagerId);
                    ManagerEntity manager = context.Managers.FirstOrDefault(m => m.User.Id == storeAssign.ManagerId);

                    context.Entry(manager).Collection(m => m.Stores).Load();

                    StoreEntity store = context.Stores.Find(storeAssign.StoreId);

                    context.Managers.Attach(manager);
                    manager.Stores.Add(store);

                    context.SaveChanges();

                    response.ResponseMessage = "Store assigned successfully";
                    response.Success = true;
                }
            }
            catch (Exception exception)
            {

            }

            return response;
        }

        public Response UnassignStore(WebApiStoreAssign storeUnassign)
        {
            Response response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = context.Users.Find(storeUnassign.ManagerId);
                    ManagerEntity manager = context.Managers.FirstOrDefault(m => m.User.Id == storeUnassign.ManagerId);

                    context.Entry(manager).Collection(m => m.Stores).Load();

                    StoreEntity store = context.Stores.Find(storeUnassign.StoreId);

                    context.Managers.Attach(manager);
                    manager.Stores.Remove(store);
                    //manager.Stores.Add(store);

                    context.SaveChanges();

                    response.ResponseMessage = "Store unassigned successfully";
                    response.Success = true;
                }
            }
            catch (Exception exception)
            {

            }

            return response;
        }
    }
}