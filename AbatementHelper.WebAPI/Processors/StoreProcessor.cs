using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Processors
{
    public static class StoreProcessor
    {
        public static WebApiStore StoreEntityToWebApiStoreParameters(StoreEntity store)
        {
            StoreAdminRepository storeAdminRepository = new StoreAdminRepository();
            List<WebApiManager> managers = new List<WebApiManager>();

            foreach (var manager in store.Managers)
            {
                managers.Add(ManagerProcessor.ApplicationUserToWebApiManager(storeAdminRepository.FindUserByManagerId(manager.Id)));
            }


            WebApiStoreParameters webApiStore = new WebApiStoreParameters
            (
                store.Id,
                store.StoreName,
                store.WorkingHoursWeek,
                store.WorkingHoursWeekends,
                store.WorkingHoursHolidays,
                store.Country,
                store.City,
                store.PostalCode,
                store.Street,
                store.StoreAdmin.Id,
                store.Approved,
                store.Deleted,
                managers
            );

            return webApiStore;
        }

        public static WebApiStore StoreEntityToWebApiStore(StoreEntity store)
        {
            StoreAdminRepository storeAdminRepository = new StoreAdminRepository();

            WebApiStore webApiStore = new WebApiStore
            {
                Id = store.Id,
                StoreName = store.StoreName,
                WorkingHoursWeek = store.WorkingHoursWeek,
                WorkingHoursWeekends = store.WorkingHoursWeekends,
                Country = store.Country,
                City = store.City,
                PostalCode = store.PostalCode,
                Street = store.Street,
                Approved = store.Approved,
                Deleted = store.Deleted
            };

            return webApiStore;
        }

        public static StoreEntity WebApiStoreToStoreEntity(WebApiStore store)
        {
            return new StoreEntity
            {
                StoreName = store.StoreName,
                WorkingHoursWeek = store.WorkingHoursWeek,
                WorkingHoursWeekends = store.WorkingHoursWeekends,
                WorkingHoursHolidays = store.WorkingHoursHolidays,
                Country = store.Country,
                City = store.City,
                PostalCode = store.PostalCode,
                Street = store.Street,

                Approved = store.Approved,
                Deleted = store.Deleted
            };
        }
    }
}