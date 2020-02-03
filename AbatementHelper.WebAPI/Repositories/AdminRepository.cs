using AbatementHelper.CommonModels.CreateModels;
using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.EntityValidation;
using AbatementHelper.WebAPI.Extentions;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.WebAPI.Repositories
{
    public class AdminRepository
    {
        public async Task<WebApiListOfUsersResult> GetAllUsersAsync()
        {
            var result = new WebApiListOfUsersResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var tasks = new List<Task<WebApiUser>>();

                    List<ApplicationUser> users = context.Users.ToList();

                    foreach (var user in users)
                    {
                        tasks.Add(Task.Run(() => UserProcessor.ApplicationUserToWebApiUser(user)));
                    }

                    var results = await Task.WhenAll(tasks);

                    result.Users = results.ToList();
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiListOfStoresResult> GetAllStoresAsync()
        {
            var result = new WebApiListOfStoresResult();

            var stores = new List<WebApiStore>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var tasks = new List<Task<WebApiStore>>();

                    List<StoreEntity> storeEntities = context.Stores.ToList();

                    if (storeEntities != null)
                    {
                        foreach (var store in storeEntities)
                        {
                            context.Entry(store).Collection(s => s.Managers).Load();
                            context.Entry(store).Reference(s => s.StoreAdmin).Load();


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

        public async Task<WebApiListOfManagersResult> GetAllManagersAsync()
        {
            var result = new WebApiListOfManagersResult();

            var managers = new List<WebApiManager>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var tasks = new List<Task<WebApiManager>>();

                    List<ManagerEntity> managerEntites = context.Managers.ToList();

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

        //create user

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
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

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
                            using (var context = new ApplicationUserDbContext())
                            {
                                ApplicationUser storeAdmin = context.Users.Find(user.StoreAdminId);
                                ApplicationUser applicationUser = context.Users.Find(processedUser.Id);

                                if (storeAdmin != null)
                                {
                                    ManagerEntity manager = new ManagerEntity
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        StoreAdmin = storeAdmin,
                                        User = applicationUser,
                                    };

                                    context.Managers.Add(manager);

                                    context.SaveChanges();

                                    result.Message = "Manager created.";
                                }
                                else
                                {
                                    result.AddModelError(string.Empty, "Store admin does not exist.");
                                }
                            }
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

        public async Task<WebApiResult> EditUserAsync(WebApiUser user)
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

                        var updateResult = await userManager.UpdateAsync(userEntity);

                        using (var context = new ApplicationUserDbContext())
                        {
                            IList<string> roles = await userManager.GetRolesAsync(user.Id);

                            List<IdentityRole> identityRoles = await context.Roles.ToListAsync();

                            IdentityRole existingRole = identityRoles.FirstOrDefault(r => r.Name == user.Role);

                            if (roles.FirstOrDefault() != user.Role && existingRole != null)
                            {
                                var removeResult = await userManager.RemoveFromRoleAsync(user.Id, roles.FirstOrDefault());

                                if (!removeResult.Succeeded)
                                {
                                    foreach (var error in removeResult.Errors)
                                    {
                                        result.AddModelError(string.Empty, error);
                                    }
                                }

                                var addResult = await userManager.AddToRoleAsync(user.Id, user.Role);

                                if (!addResult.Succeeded)
                                {
                                    foreach (var error in addResult.Errors)
                                    {
                                        result.AddModelError(string.Empty, error);
                                    }
                                }
                            }

                            await context.SaveChangesAsync();
                        }

                        if (!updateResult.Succeeded)
                        {
                            foreach (var error in updateResult.Errors)
                            {
                                result.AddModelError(string.Empty, error);
                            }
                        }
                        else
                        {
                            result.Message = "User updated.";
                        }
                    }
                    else
                    {
                        result.AddModelError(string.Empty, "User does not exist.");
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

        public async Task<WebApiResult> EditManagerAsync(WebApiManager user)
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

        public async Task<WebApiUserResult> ReadUserByIdAsync(string id)
        {
            var result = new WebApiUserResult();

            try
            {
                using (var userManager = new UserManager())
                {
                    ApplicationUser user = await userManager.FindByIdAsync(id);

                    result.User = await UserProcessor.ApplicationUserToWebApiUser(user);
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

        public async Task<WebApiResult> DeleteUserAsync(string id)
        {
            var result = new WebApiResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = await context.Users.FindAsync(id);

                    context.Users.Attach(user);
                    user.Deleted = true;

                    await context.SaveChangesAsync();

                    result.Message = "User deleted.";
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
        }

        public async Task<WebApiResult> RestoreUserAsync(string id)
        {
            var result = new WebApiResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = await context.Users.FindAsync(id);

                    context.Users.Attach(user);
                    user.Deleted = false;

                    await context.SaveChangesAsync();

                    result.Message = "User restored.";
                }
            }
            catch (Exception exception)
            {
                result.Exception = exception;
                result.AddModelError(string.Empty, "An exception has occured.");
            }

            return result;
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
    }
}