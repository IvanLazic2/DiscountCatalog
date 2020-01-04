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
using System.Diagnostics;

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
        }

        public async Task<StoreEntity> ReadStoreByIdAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity store = await context.Stores.FindAsync(id);
                context.Stores.Include(s => s.Managers).ToList();

                return store;
            }
        }

        public async Task<ApplicationUser> FindUserByManagerIdAsync(string id)
        {
            var user = new ApplicationUser();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ManagerEntity manager = await context.Managers.FindAsync(id);

                    await context.Entry(manager).Reference(m => m.User).LoadAsync();

                    user = manager.User;
                }
            }
            catch (Exception exception)
            {

            }

            return user;
        }

        //public async Task<ManagerEntity> FindManagerByUserIdAsync(string id)
        //{
        //    //var manager = new ManagerEntity();
        //    //var stores = new List<StoreEntity>();

        //    using (var context = new ApplicationUserDbContext())
        //    {
        //        //manager = context.Managers.Include(m => m.Stores)
        //        //                          .Include(m => m.StoreAdmin).FirstOrDefault(m => m.User.Id == id);

        //        //ApplicationUser user = await context.Users.FindAsync(id);

        //        ManagerEntity manager = await context.Managers.FirstOrDefaultAsync(m => m.User.Id == id);

        //        await context.Entry(manager).Collection(m => m.Stores).LoadAsync();
        //        await context.Entry(manager).Reference(m => m.User).LoadAsync();

        //        return manager;
        //    }
        //}

        public async Task<List<WebApiManager>> GetStoreManagersAsync(string id)
        {
            var webApiManagers = new List<WebApiManager>();

            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity store = context.Stores.Find(id);
                context.Stores.Include(s => s.Managers).ToList();

                foreach (var manager in store.Managers)
                {
                    await context.Entry(manager).Reference(m => m.User).LoadAsync();

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

        public async Task<List<WebApiStore>> GetManagerStoresAsync(string id)
        {
            var webApiStores = new List<WebApiStore>();

            using (var context = new ApplicationUserDbContext())
            {
                ManagerEntity manager = context.Managers.Find(id);

                await context.Entry(manager).Collection(m => m.Stores).LoadAsync();

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

        public async Task<List<WebApiStore>> GetAllStoresAsync(string storeAdminId)
        {
            var stores = new List<WebApiStore>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var storeEntities = context.Stores.Where(s => s.StoreAdmin.Id == storeAdminId).ToList();

                    var tasks = new List<Task<WebApiStore>>();

                    foreach (var store in storeEntities)
                    {
                        context.Entry(store).Collection(s => s.Managers).Load();

                        if (!store.Deleted && !store.Approved) //OVO PROMIJENIT U store.Approved (bez !)
                        {
                            store.StoreAdmin = await context.Users.FindAsync(storeAdminId);

                            tasks.Add(Task.Run(() => StoreProcessor.StoreEntityToWebApiStoreAsync(store)));
                        }
                    }

                    var results = await Task.WhenAll(tasks);

                    stores = results.ToList();
                }
            }
            catch (Exception exception)
            {

            }

            return stores;
        }

        public async Task<Response> CreateStoreAsync(WebApiStore store)
        {
            var response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {

                    StoreEntity processedStore = StoreProcessor.WebApiStoreToStoreEntity(store);

                    processedStore.StoreAdmin = context.Users.Find(store.StoreAdminId);
                    processedStore.Approved = true;
                    processedStore.Deleted = false;

                    context.Stores.Add(processedStore);

                    await context.SaveChangesAsync();



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
                response.Message = exception.InnerException.InnerException.Message;
                response.Success = false;
            }

            response.Message = "Successfully created";
            response.Success = true;

            return response;
        }

        public async Task<Response> EditStoreAsync(WebApiStore store)
        {
            var response = new Response();

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

                    await context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException exception)
            {
                response.Message = exception.InnerException.InnerException.Message;
                response.Success = false;
            }

            response.Message = "Successfully edited.";
            response.Success = true;

            return response;
        }

        public async Task<Response> PostStoreImageAsync(WebApiPostImage store)
        {
            var response = new Response();

            byte[] image = store.Image;

            if (ImageProcessor.IsValid(image))
            {
                try
                {
                    using (var context = new ApplicationUserDbContext())
                    {
                        StoreEntity storeEntity = context.Stores.Find(store.Id);

                        context.Stores.Attach(storeEntity);

                        storeEntity.StoreImage = image;

                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception exception)
                {
                    response.Message = exception.InnerException.InnerException.Message;
                    response.Success = false;
                }

                response.Message = "Successfully uploaded.";
                response.Success = true;
            }
            else
            {
                response.Message = "Invalid image type";
                response.Success = false;
            }


            return response;
        }

        public async Task<byte[]> GetStoreImageAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity store = await context.Stores.FindAsync(id);

                return store.StoreImage;
            }

        }

        public async Task DeleteStoreAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity store = await context.Stores.FindAsync(id);

                context.Stores.Attach(store);

                store.Deleted = true;

                await context.SaveChangesAsync();
            }
        }

        public async Task<List<WebApiStore>> GetAllDeletedStoresAsync(string storeAdminId)
        {
            var stores = new List<WebApiStore>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {


                    var tasks = new List<Task<WebApiStore>>();

                    List<StoreEntity> storeEntities = context.Stores.Where(s => s.StoreAdmin.Id == storeAdminId && s.Deleted).ToList();

                    foreach (var store in storeEntities)
                    {
                        store.StoreAdmin = await context.Users.FindAsync(storeAdminId);

                        tasks.Add(Task.Run(() => StoreProcessor.StoreEntityToWebApiStoreAsync(store)));
                    }

                    var results = await Task.WhenAll(tasks);

                    stores = results.ToList();
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return stores;

        }

        public async Task RestoreStoreAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity store = await context.Stores.FindAsync(id);

                context.Stores.Attach(store);

                store.Deleted = false;

                await context.SaveChangesAsync();
            }
        }

        public async Task<SelectedStore> SelectStoreAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity store = await context.Stores.FindAsync(id);

                return new SelectedStore
                {
                    Id = store.Id,
                    StoreName = store.StoreName
                };
            }
        }

        public async Task<List<WebApiManager>> GetAllManagersAsync(string storeAdminId)
        {
            var managers = new List<WebApiManager>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var tasks = new List<Task<WebApiManager>>();

                    List<ManagerEntity> managerEntites = context.Managers.Where(s => s.StoreAdmin.Id == storeAdminId).ToList();

                    foreach (var manager in managerEntites)
                    {
                        context.Entry(manager).Reference(m => m.User).Load();
                        context.Entry(manager).Collection(m => m.Stores).Load();

                        if (!manager.User.Deleted && !manager.User.Approved) //MAKNIT ! SA Approved
                        {
                            tasks.Add(Task.Run(() => ManagerProcessor.ManagerEntityToWebApiManagerAsync(manager)));
                        }
                    }

                    var results = await Task.WhenAll(tasks);

                    managers = results.ToList();
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return managers;
        }

        public async Task<Response> CreateManagerAsync(CreateManagerModel user, string password)
        {
            var response = new Response();

            ApplicationUser processedUser = ManagerProcessor.CreateManagerModelToApplicationUser(user);

            try
            {
                using (var userManager = new UserManager())
                {
                    await userManager.CreateAsync(processedUser, password);
                    await userManager.AddToRoleAsync(processedUser.Id, "Manager");
                }

                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser applicationUser = await context.Users.FindAsync(processedUser.Id);
                    ApplicationUser storeAdmin = await context.Users.FindAsync(user.StoreAdminId);

                    context.Managers.Add(new ManagerEntity
                    {
                        User = applicationUser,
                        StoreAdmin = storeAdmin
                    });

                    await context.SaveChangesAsync();
                }

                response.Message = "Created successfully";
                response.Success = true;
            }
            catch (DbEntityValidationException exception)
            {
                var ex = ExceptionProcessor.processException(exception);
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }



        public async Task<Response> EditManager(WebApiManager user)
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
                    userEntity.PhoneNumber = user.PhoneNumber;
                    userEntity.Country = user.Country;
                    userEntity.City = user.City;
                    userEntity.PostalCode = user.PostalCode;
                    userEntity.Street = user.Street;
                    userEntity.TwoFactorEnabled = user.TwoFactorEnabled;

                    await context.SaveChangesAsync();

                    response.Message = "Successfully edited.";
                    response.Success = true;
                }
            }
            catch (DbEntityValidationException exception)
            {
                var ex = ExceptionProcessor.processException(exception);
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<Response> PostManagerImageAsync(WebApiPostImage manager)
        {
            var response = new Response();

            byte[] image = manager.Image;

            if (ImageProcessor.IsValid(image))
            {
                try
                {
                    using (var context = new ApplicationUserDbContext())
                    {
                        ApplicationUser user = await context.Users.FindAsync(manager.Id);

                        context.Users.Attach(user);

                        user.UserImage = image;

                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception exception)
                {
                    response.Message = exception.InnerException.InnerException.Message;
                    response.Success = false;
                }

                response.Message = "Successfully uploaded.";
                response.Success = true;
            }
            else
            {
                response.Message = "Invalid image type";
                response.Success = false;
            }

            return response;
        }

        public async Task<byte[]> GetManagerImageAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = await context.Users.FindAsync(id);

                return user.UserImage;
            }

        }

        public async Task DeleteManagerAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = await context.Users.FindAsync(id);

                context.Users.Attach(user);

                user.Deleted = true;

                await context.SaveChangesAsync();
            }
        }

        public async Task<List<WebApiManager>> GetAllDeletedManagersAsync(string storeAdminId)
        {
            var managers = new List<WebApiManager>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var tasks = new List<Task<WebApiManager>>();

                    List<ManagerEntity> managerEntityList = context.Managers.Where(m => m.StoreAdmin.Id == storeAdminId).ToList();

                    foreach (var manager in managerEntityList)
                    {
                        await context.Entry(manager).Reference(m => m.User).LoadAsync();

                        if (manager.User.Deleted)
                        {
                            tasks.Add(Task.Run(() => ManagerProcessor.ManagerEntityToWebApiManagerAsync(manager)));
                        }
                    }

                    var results = await Task.WhenAll(tasks);

                    managers = results.ToList();
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return managers;
        }

        public async Task RestoreManagerAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = await context.Users.FindAsync(id);

                context.Users.Attach(user);

                user.Deleted = false;

                await context.SaveChangesAsync();
            }
        }

        public async Task<List<WebApiManagerStore>> GetAllManagerStoresAsync(string managerId)
        {
            var watch = Stopwatch.StartNew();

            var managerStores = new List<WebApiManagerStore>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var tasks = new List<Task<WebApiManagerStore>>();

                    ApplicationUser user = await context.Users.FindAsync(managerId);

                    ManagerEntity managerEntity = await context.Managers.Where(m => m.User.Id == user.Id).FirstOrDefaultAsync();

                    await context.Entry(managerEntity).Collection(m => m.Stores).LoadAsync();
                    await context.Entry(managerEntity).Reference(m => m.StoreAdmin).LoadAsync();

                    List<StoreEntity> storeEntities = context.Stores.ToList();

                    foreach (var store in storeEntities)
                    {
                        bool assigned;

                        await context.Entry(store).Reference(s => s.StoreAdmin).LoadAsync();

                        StoreEntity storeEntity = managerEntity.Stores.Where(s => s.Id == store.Id).FirstOrDefault();

                        if (store.StoreAdmin.Id == managerEntity.StoreAdmin.Id)
                        {
                            if (storeEntity != null)
                            {
                                assigned = true;
                            }
                            else
                            {
                                assigned = false;
                            }

                            tasks.Add(Task.Run(() => ManagerProcessor.CreateWebApiManagerStoreAsync(store, managerEntity, assigned)));
                        }

                    }

                    var results = await Task.WhenAll(tasks);

                    managerStores = results.ToList();
                }

            }
            catch (Exception exception)
            {

            }

            watch.Stop();

            var elapsed = watch.ElapsedMilliseconds;

            return managerStores;
        }

        public async Task<Response> AssignStoreAsync(WebApiStoreAssign storeAssign)
        {
            var response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = await context.Users.FindAsync(storeAssign.ManagerId);
                    ManagerEntity manager = await context.Managers.FirstOrDefaultAsync(m => m.User.Id == storeAssign.ManagerId);

                    await context.Entry(manager).Collection(m => m.Stores).LoadAsync();

                    StoreEntity store = await context.Stores.FindAsync(storeAssign.StoreId);

                    context.Managers.Attach(manager);

                    manager.Stores.Add(store);

                    await context.SaveChangesAsync();


                }
            }
            catch (Exception exception)
            {

            }

            response.Message = "Store assigned successfully";
            response.Success = true;

            return response;
        }

        public async Task<Response> UnassignStoreAsync(WebApiStoreAssign storeUnassign)
        {
            var response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = await context.Users.FindAsync(storeUnassign.ManagerId);
                    ManagerEntity manager = await context.Managers.FirstOrDefaultAsync(m => m.User.Id == storeUnassign.ManagerId);

                    await context.Entry(manager).Collection(m => m.Stores).LoadAsync();

                    StoreEntity store = await context.Stores.FindAsync(storeUnassign.StoreId);

                    context.Managers.Attach(manager);

                    manager.Stores.Remove(store);

                    await context.SaveChangesAsync();


                }
            }
            catch (Exception exception)
            {

            }

            response.Message = "Store unassigned successfully";
            response.Success = true;

            return response;
        }
    }
}