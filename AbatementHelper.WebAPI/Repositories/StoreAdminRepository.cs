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
using AbatementHelper.WebAPI.Extentions;
using AbatementHelper.WebAPI.EntityValidation;
using AbatementHelper.WebAPI.Models.Result;
using AbatementHelper.WebAPI.Validators;
using FluentValidation.Results;

namespace AbatementHelper.WebAPI.Repositories
{
    public class StoreAdminRepository
    {
        public async Task<WebApiManagerResult> ReadUserById(string id)
        {
            var result = new WebApiManagerResult();

            try
            {
                using (var userManager = new UserManager())
                {
                    ApplicationUser applicationUser = await userManager.FindByIdAsync(id);

                    if (applicationUser != null)
                    {
                        result.Manager = await ManagerProcessor.ApplicationUserToWebApiManagerAsync(applicationUser);
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "Manager does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiStoreResult> ReadStoreByIdAsync(string id)
        {
            var result = new WebApiStoreResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    StoreEntity store = await context.Stores.FindAsync(id);

                    if (store != null)
                    {
                        context.Stores.Include(s => s.Managers).ToList();

                        WebApiStore webApiStore = await StoreProcessor.StoreEntityToWebApiStoreAsync(store);

                        result.Store = webApiStore;
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "Store does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "Store does not exist.");
            }

            return result;

        }

        private async Task<ApplicationUser> FindUserByManagerIdAsync(string id)
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
            catch (Exception)
            {
                throw;
            }

            return user;
        }

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

        public async Task<WebApiListOfStoresResult> GetAllActiveStoresAsync(string storeAdminId)
        {
            var result = new WebApiListOfStoresResult();

            var stores = new List<WebApiStore>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var tasks = new List<Task<WebApiStore>>();

                    List<StoreEntity> storeEntities = context.Stores.Where(s => s.StoreAdmin.Id == storeAdminId).ToList();

                    if (storeEntities != null)
                    {
                        foreach (var store in storeEntities)
                        {
                            context.Entry(store).Collection(s => s.Managers).Load();

                            if (!store.Deleted && store.Approved)
                            {
                                store.StoreAdmin = await context.Users.FindAsync(storeAdminId);

                                if (store.StoreAdmin != null)
                                {
                                    tasks.Add(Task.Run(() => StoreProcessor.StoreEntityToWebApiStoreAsync(store)));
                                }
                                else
                                {
                                    result.AddModelError(string.Empty, "Store administrator does not exist.");
                                }
                            }
                        }

                        var results = await Task.WhenAll(tasks);

                        stores = results.ToList();
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            result.Stores = stores;

            return result;
        }

        public async Task<WebApiListOfStoresResult> GetAllStoresAsync(string storeAdminId)
        {
            var result = new WebApiListOfStoresResult();

            var stores = new List<WebApiStore>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var tasks = new List<Task<WebApiStore>>();

                    List<StoreEntity> storeEntities = context.Stores.Where(s => s.StoreAdmin.Id == storeAdminId).ToList();

                    if (storeEntities != null)
                    {
                        foreach (var store in storeEntities)
                        {
                            context.Entry(store).Collection(s => s.Managers).Load();

                            if (!store.Deleted)
                            {
                                store.StoreAdmin = await context.Users.FindAsync(storeAdminId);

                                tasks.Add(Task.Run(() => StoreProcessor.StoreEntityToWebApiStoreAsync(store)));
                            }
                        }

                        var results = await Task.WhenAll(tasks);

                        stores = results.ToList();
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            result.Stores = stores;

            return result;
        }

        public async Task<WebApiResult> CreateStoreAsync(WebApiStore store)
        {
            var result = new WebApiResult();

            try
            {
                var storeValidation = new StoreValidation();

                ModelStateResponse validationResponse = storeValidation.Validate(store);

                if (!validationResponse.IsValid)
                {
                    foreach (var error in validationResponse.ModelState)
                    {
                        result.AddModelError(error.Key, error.Value);
                    }
                }

                using (var context = new ApplicationUserDbContext())
                {
                    StoreEntity existingStore = context.Stores.FirstOrDefault(s => s.StoreName == store.StoreName);

                    if (existingStore == null)
                    {
                        StoreEntity processedStore = StoreProcessor.WebApiStoreToStoreEntity(store);

                        if (processedStore != null)
                        {
                            processedStore.StoreAdmin = context.Users.Find(store.StoreAdminId);

                            if (processedStore.StoreAdmin != null)
                            {
                                if (result.Success)
                                {
                                    processedStore.Approved = true;
                                    processedStore.Deleted = false;

                                    context.Stores.Add(processedStore);

                                    await context.SaveChangesAsync();

                                    result.Message = "Store created.";
                                }
                            }
                            else
                            {
                                result.AddModelError(string.Empty, "Store administrator does not exist.");
                            }
                        }
                    }
                    else
                    {
                        result.AddModelError(ObjectExtensions.GetPropertyName(() => store.StoreName), $"Store name is already taken");
                    }

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
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiResult> EditStoreAsync(WebApiStore store)
        {
            var result = new WebApiResult();

            try
            {
                var storeValidation = new StoreValidation();

                ModelStateResponse validationResponse = storeValidation.Validate(store);

                if (!validationResponse.IsValid)
                {
                    foreach (var error in validationResponse.ModelState)
                    {
                        result.AddModelError(error.Key, error.Value);
                    }
                }

                using (var context = new ApplicationUserDbContext())
                {
                    StoreEntity existingStore = context.Stores.FirstOrDefault(s => s.StoreName == store.StoreName);
                    StoreEntity originalStore = context.Stores.FirstOrDefault(s => s.Id == store.Id);

                    if (existingStore != null)
                    {
                        if (existingStore.StoreName == originalStore.StoreName)
                        {
                            StoreEntity storeEntity = context.Stores.Find(store.Id);

                            if (storeEntity != null)
                            {
                                if (result.Success)
                                {
                                    context.Stores.Attach(storeEntity);

                                    storeEntity.StoreName = store.StoreName;
                                    storeEntity.WorkingHoursWeekBegin = store.WorkingHoursWeekBegin;
                                    storeEntity.WorkingHoursWeekEnd = store.WorkingHoursWeekEnd;
                                    storeEntity.WorkingHoursWeekendsBegin = store.WorkingHoursWeekendsBegin;
                                    storeEntity.WorkingHoursWeekendsEnd = store.WorkingHoursWeekendsEnd;
                                    storeEntity.WorkingHoursHolidaysBegin = store.WorkingHoursHolidaysBegin;
                                    storeEntity.WorkingHoursHolidaysEnd = store.WorkingHoursHolidaysEnd;
                                    storeEntity.Country = store.Country;
                                    storeEntity.City = store.City;
                                    storeEntity.PostalCode = store.PostalCode;
                                    storeEntity.Street = store.Street;

                                    await context.SaveChangesAsync();

                                    result.Message = "Store updated.";
                                }
                            }
                            else
                            {
                                result.AddModelError(string.Empty, "Store does not exist.");
                            }
                        }
                        else
                        {
                            result.AddModelError(ObjectExtensions.GetPropertyName(() => store.StoreName), "Store name is already taken.");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiResult> PostStoreImageAsync(WebApiPostImage store)
        {
            var result = new WebApiResult();

            byte[] image = store.Image;

            if (image != null)
            {
                if (ImageProcessor.IsValid(image))
                {
                    try
                    {
                        using (var context = new ApplicationUserDbContext())
                        {
                            StoreEntity storeEntity = context.Stores.Find(store.Id);

                            if (storeEntity != null)
                            {
                                context.Stores.Attach(storeEntity);

                                storeEntity.StoreImage = image;

                                await context.SaveChangesAsync();

                                result.Message = "Image saved";
                            }
                            else
                            {
                                result.AddModelError(string.Empty, "Product does not exist.");
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        result.Exception = exception;
                        result.AddModelError(string.Empty, "An exception has occured.");
                    }
                }
                else
                {
                    result.AddModelError(string.Empty, "Invalid image type.");
                }
            }
            else
            {
                result.AddModelError(string.Empty, "Image is empty.");
            }

            return result;
        }

        public async Task<byte[]> GetStoreImageAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity store = await context.Stores.FindAsync(id);

                return store.StoreImage;
            }

        }

        public async Task<WebApiResult> DeleteStoreAsync(string id)
        {
            var result = new WebApiResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    StoreEntity store = await context.Stores.FindAsync(id);

                    if (store != null)
                    {
                        context.Stores.Attach(store);

                        store.Deleted = true;

                        await context.SaveChangesAsync();

                        result.Message = "Store deleted.";
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "Store does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiListOfStoresResult> GetAllDeletedStoresAsync(string storeAdminId)
        {
            var result = new WebApiListOfStoresResult();

            var stores = new List<WebApiStore>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var tasks = new List<Task<WebApiStore>>();

                    List<StoreEntity> storeEntities = context.Stores.Where(s => s.StoreAdmin.Id == storeAdminId && s.Deleted).ToList();

                    if (storeEntities != null)
                    {
                        foreach (var store in storeEntities)
                        {
                            store.StoreAdmin = await context.Users.FindAsync(storeAdminId);

                            tasks.Add(Task.Run(() => StoreProcessor.StoreEntityToWebApiStoreAsync(store)));
                        }

                        var results = await Task.WhenAll(tasks);

                        stores = results.ToList();
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            result.Stores = stores;

            return result;
        }

        public async Task<WebApiResult> RestoreStoreAsync(string id)
        {
            var result = new WebApiResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    StoreEntity store = await context.Stores.FindAsync(id);

                    if (store != null)
                    {
                        context.Stores.Attach(store);

                        store.Deleted = false;

                        await context.SaveChangesAsync();

                        result.Message = "Store restored.";
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "Store does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiSelectedStoreResult> SelectStoreAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                var result = new WebApiSelectedStoreResult();

                try
                {
                    StoreEntity store = await context.Stores.FindAsync(id);

                    if (store != null)
                    {
                        if (store.Approved)
                        {
                            SelectedStore selectedStore = new SelectedStore
                            {
                                Id = store.Id,
                                StoreName = store.StoreName
                            };

                            result.Store = selectedStore;

                            result.Message = $"Store {selectedStore.StoreName} selected.";
                        }
                        else
                        {
                            result.AddModelError(string.Empty, $"Store: {store.StoreName} has to be approved.");
                        }
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "Store does not exist.");
                    }
                }
                catch (Exception exception)
                {
                    result.Exception = exception;
                    result.AddModelError(string.Empty, "An exception has occured.");
                }

                return result;
            }
        }

        public async Task<WebApiListOfManagersResult> GetAllManagersAsync(string storeAdminId)
        {
            var result = new WebApiListOfManagersResult();

            var managers = new List<WebApiManager>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var tasks = new List<Task<WebApiManager>>();

                    List<ManagerEntity> managerEntites = context.Managers.Where(s => s.StoreAdmin.Id == storeAdminId).ToList();

                    if (managerEntites != null)
                    {
                        foreach (var manager in managerEntites)
                        {
                            context.Entry(manager).Reference(m => m.User).Load();
                            context.Entry(manager).Collection(m => m.Stores).Load();

                            if (!manager.User.Deleted && manager.User.Approved)
                            {
                                tasks.Add(Task.Run(() => ManagerProcessor.ManagerEntityToWebApiManagerAsync(manager)));
                            }
                        }

                        var results = await Task.WhenAll(tasks);

                        managers = results.ToList();
                    }

                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            result.Managers = managers;

            return result;
        }

        public async Task<WebApiResult> CreateManagerAsync(CreateManagerModel user, string password)
        {
            var result = new WebApiResult();

            try
            {
                ApplicationUser processedUser = ManagerProcessor.CreateManagerModelToApplicationUser(user);

                using (var userManager = new UserManager())
                {
                    processedUser.Approved = true; // za sad

                    var createResult = await userManager.CreateAsync(processedUser, password);

                    if (!createResult.Succeeded)
                    {
                        var managerValidation = new ManagerValidation();

                        ModelStateResponse managerValidationResult = await managerValidation.ValidateAsync(processedUser);

                        if (!managerValidationResult.IsValid)
                        {
                            foreach (var error in managerValidationResult.ModelState)
                            {
                                result.AddModelError(error.Key, error.Value);
                            }
                        }
                    }
                    else
                    {
                        var roleResult = await userManager.AddToRoleAsync(processedUser.Id, "Manager");

                        if (!roleResult.Succeeded)
                        {
                            foreach (var error in roleResult.Errors)
                            {
                                result.AddModelError(string.Empty, error);
                            }
                        }
                        else
                        {
                            result.Message = "Manager created.";
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiResult> EditManager(WebApiManager user)
        {
            var result = new WebApiResult();

            try
            {
                using (var userManager = new UserManager())
                {
                    ApplicationUser userEntity = await userManager.FindByIdAsync(user.Id);

                    if (userEntity != null)
                    {
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

                        var managerValidation = new ManagerValidation();

                        var updateResult = await userManager.UpdateAsync(userEntity);

                        if (!updateResult.Succeeded)
                        {
                            foreach (var error in updateResult.Errors)
                            {
                                result.AddModelError(string.Empty, error);
                            }
                        }
                        else
                        {
                            result.Message = "Manager updated.";
                        }
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "Manager does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                string type = exception.GetType().ToString();

                if (exception is DbUpdateException)
                {
                    DbUpdateException ex = exception as DbUpdateException;

                    var entities = ex.Entries;

                    foreach (var entity in entities)
                    {
                        ApplicationUser userEntitiy = entity.Entity as ApplicationUser;

                        using (var userManager = new UserManager())
                        {
                            ApplicationUser originalEntity = await userManager.FindByIdAsync(userEntitiy.Id);

                            if (originalEntity.Email != userEntitiy.Email)
                            {
                                result.AddModelError(ObjectExtensions.GetPropertyName(() => userEntitiy.Email), "Email is already taken.");
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task<WebApiResult> PostManagerImageAsync(WebApiPostImage manager)
        {
            var result = new WebApiResult();

            byte[] image = manager.Image;

            if (image != null)
            {
                if (ImageProcessor.IsValid(image))
                {
                    try
                    {
                        using (var context = new ApplicationUserDbContext())
                        {
                            ApplicationUser user = await context.Users.FindAsync(manager.Id);

                            if (user != null)
                            {
                                context.Users.Attach(user);

                                user.UserImage = image;

                                await context.SaveChangesAsync();

                                result.Message = "Image saved.";
                            }
                            else
                            {
                                result.AddModelError(string.Empty, "Manager does not exist.");
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        result.Exception = exception;
                        result.AddModelError(string.Empty, "An exception has occured.");
                    }
                }
                else
                {
                    result.AddModelError(string.Empty, "Invalid image type.");
                }
            }
            else
            {
                result.AddModelError(string.Empty, "Image is empty.");
            }

            return result;
        }

        public async Task<byte[]> GetManagerImageAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = await context.Users.FindAsync(id);

                return user.UserImage;
            }

        }

        public async Task<WebApiResult> DeleteManagerAsync(string id)
        {
            var result = new WebApiResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = await context.Users.FindAsync(id);

                    if (user != null)
                    {
                        context.Users.Attach(user);

                        user.Deleted = true;

                        await context.SaveChangesAsync();

                        result.Message = "Manager deleted.";
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "Manager does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiListOfManagersResult> GetAllDeletedManagersAsync(string storeAdminId)
        {
            var result = new WebApiListOfManagersResult();

            var managers = new List<WebApiManager>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var tasks = new List<Task<WebApiManager>>();

                    List<ManagerEntity> managerEntities = context.Managers.Where(m => m.StoreAdmin.Id == storeAdminId).ToList();

                    if (managerEntities != null)
                    {
                        foreach (var manager in managerEntities)
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
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            result.Managers = managers;

            return result;
        }

        public async Task<WebApiResult> RestoreManagerAsync(string id)
        {
            var result = new WebApiResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = await context.Users.FindAsync(id);

                    if (user != null)
                    {
                        context.Users.Attach(user);

                        user.Deleted = false;

                        await context.SaveChangesAsync();

                        result.Message = "Manager restored.";
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "Manager does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiListOfManagerStoresResult> GetAllManagerStoresAsync(string managerId)
        {
            var result = new WebApiListOfManagerStoresResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var tasks = new List<Task<WebApiManagerStore>>();

                    ApplicationUser user = await context.Users.FindAsync(managerId);

                    if (user != null)
                    {
                        ManagerEntity managerEntity = await context.Managers.Where(m => m.User.Id == user.Id).FirstOrDefaultAsync();

                        if (managerEntity != null)
                        {
                            await context.Entry(managerEntity).Collection(m => m.Stores).LoadAsync();

                            await context.Entry(managerEntity).Reference(m => m.StoreAdmin).LoadAsync();

                            List<StoreEntity> storeEntities = context.Stores.Where(s => s.Approved && !s.Deleted).ToList();

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

                            result.Stores = results.ToList();
                        }
                        else
                        {
                            result.AddModelError(string.Empty, "Manager does not exist.");
                        }
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "Manager does not exist.");
                    }
                }

            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiResult> AssignStoreAsync(WebApiStoreAssign storeAssign)
        {
            var result = new WebApiResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = await context.Users.FindAsync(storeAssign.ManagerId);

                    ManagerEntity manager = await context.Managers.FirstOrDefaultAsync(m => m.User.Id == storeAssign.ManagerId);

                    if (manager != null)
                    {
                        await context.Entry(manager).Collection(m => m.Stores).LoadAsync();

                        StoreEntity store = await context.Stores.FindAsync(storeAssign.StoreId);

                        if (store != null)
                        {
                            if (store.Approved)
                            {
                                context.Managers.Attach(manager);

                                manager.Stores.Add(store);

                                await context.SaveChangesAsync();

                                result.Message = "Store assigned.";
                            }
                            else
                            {
                                result.AddModelError(string.Empty, "Store has to be approved.");
                            }
                        }
                        else
                        {
                            result.AddModelError(string.Empty, "Store does not exist.");
                        }
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "Manager does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiResult> UnassignStoreAsync(WebApiStoreAssign storeUnassign)
        {
            var result = new WebApiResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = await context.Users.FindAsync(storeUnassign.ManagerId);

                    ManagerEntity manager = await context.Managers.FirstOrDefaultAsync(m => m.User.Id == storeUnassign.ManagerId);

                    if (manager != null)
                    {
                        await context.Entry(manager).Collection(m => m.Stores).LoadAsync();

                        StoreEntity store = await context.Stores.FindAsync(storeUnassign.StoreId);

                        if (store != null)
                        {
                            context.Managers.Attach(manager);

                            manager.Stores.Remove(store);

                            await context.SaveChangesAsync();

                            result.Message = "Store unassigned.";
                        }
                        else
                        {
                            result.AddModelError(string.Empty, "Store does not exist.");
                        }
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "Manager does not exist.");
                    }
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }
    }
}