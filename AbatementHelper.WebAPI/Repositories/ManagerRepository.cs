using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
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
        public async Task<List<WebApiStore>> GetAllStoresAsync(string id)
        {
            var stores = new List<WebApiStore>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
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
                                    stores.Add(await StoreProcessor.StoreEntityToWebApiStoreAsync(store));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return stores;
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

        public async Task<Response> EditStoreAsync(WebApiStore store)
        {
            var response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    StoreEntity storeEntity = await context.Stores.FindAsync(store.Id);

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

                    response.Message = "Successfully edited.";
                    response.Success = true;
                }
            }
            catch (DbUpdateException exception)
            {
                response.Message = exception.InnerException.InnerException.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<WebApiStore> ReadStoreByIdAsync(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity storeEntity = await context.Stores.FindAsync(id);

                WebApiStore store = await StoreProcessor.StoreEntityToWebApiStoreAsync(storeEntity);

                return store;
            }
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

                    response.Message = "Store abandoned";
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