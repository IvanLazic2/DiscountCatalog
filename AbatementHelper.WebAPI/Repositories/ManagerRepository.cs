using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.EntityValidation;
using AbatementHelper.WebAPI.Extentions;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.WebAPI.Repositories
{
    public class ManagerRepository
    {
        public async Task<WebApiListOfStoresResult> GetAllStoresAsync(string id)
        {
            var result = new WebApiListOfStoresResult();

            var stores = new List<WebApiStore>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    var tasks = new List<Task<WebApiStore>>();

                    ApplicationUser user = await context.Users.FindAsync(id);

                    if (user != null)
                    {
                        ManagerEntity manager = await context.Managers.FirstOrDefaultAsync(m => m.User.Id == user.Id);

                        if (manager != null)
                        {
                            await context.Entry(manager).Collection(m => m.Stores).LoadAsync();

                            List<StoreEntity> storeEntities = manager.Stores.ToList();

                            if (storeEntities != null)
                            {
                                foreach (var store in storeEntities)
                                {
                                    if (store.Approved && !store.Deleted)
                                    {
                                        tasks.Add(Task.Run(() => StoreProcessor.StoreEntityToWebApiStoreAsync(store)));
                                    }
                                }
                            }
                        }
                        else
                        {
                            result.AddModelError(string.Empty, "Manager does not exist.");
                        }

                        var results = await Task.WhenAll(tasks);

                        stores = results.ToList();
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

            result.Stores = stores;

            return result;
        }

        public async Task<WebApiSelectedStoreResult> SelectStoreAsync(string id)
        {
            var result = new WebApiSelectedStoreResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    StoreEntity store = await context.Stores.FindAsync(id);

                    if (store != null)
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

        public async Task<WebApiStoreResult> ReadStoreByIdAsync(string id)
        {
            var result = new WebApiStoreResult();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    StoreEntity storeEntity = await context.Stores.FindAsync(id);

                    if (storeEntity != null)
                    {
                        WebApiStore store = await StoreProcessor.StoreEntityToWebApiStoreAsync(storeEntity);

                        if (store != null)
                        {
                            result.Store = store;
                        }
                        else
                        {
                            result.AddModelError(string.Empty, "An error has occured.");
                        }
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

        public async Task<WebApiResult> AbandonStoreAsync(WebApiStoreAssign storeUnassign)
        {
            var result = new WebApiResult();

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

                    result.Message = "Store abandoned.";
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