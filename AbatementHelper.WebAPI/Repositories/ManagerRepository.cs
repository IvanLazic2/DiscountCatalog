using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.WebAPI.Repositories
{
    public class ManagerRepository
    {
        public List<WebApiStore> GetAllStores(string id)
        {
            List<WebApiStore> stores = new List<WebApiStore>();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser user = context.Users.Find(id);

                    if (user != null)
                    {
                        ManagerEntity manager = context.Managers.FirstOrDefault(m => m.User.Id == user.Id);

                        if (manager != null)
                        {
                            context.Entry(manager).Collection(m => m.Stores).Load();

                            List<StoreEntity> storeEntities = manager.Stores.ToList();

                            if (storeEntities != null)
                            {
                                foreach (var store in storeEntities)
                                {
                                    stores.Add(StoreProcessor.StoreEntityToWebApiStore(store));
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

        public async Task<WebApiStore> ReadStoreById(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity storeEntity = context.Stores.Find(id);

                WebApiStore store = StoreProcessor.StoreEntityToWebApiStore(storeEntity);

                return store;
            }
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

                    context.SaveChanges();

                    response.ResponseMessage = "Store abandoned";
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