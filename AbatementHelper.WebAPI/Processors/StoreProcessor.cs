using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Processors
{
    public static class StoreProcessor
    {
        public static WebApiStore StoreEntityToWebApiStore(StoreEntity store)
        {
            return new WebApiStore
            {
                Id = store.Id,
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