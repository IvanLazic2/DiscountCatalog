using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Repositories;
using AutoMapper;
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
            StoreAdminRepository storeAdminRepository = new StoreAdminRepository();
            
            WebApiStore webApiStore = new WebApiStore();

            var config = new MapperConfiguration(c => {
                c.CreateMap<StoreEntity, WebApiStore>()
                    .ForMember(s => s.Managers, act => act.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                webApiStore = mapper.Map<StoreEntity, WebApiStore>(store);
                webApiStore.Managers = storeAdminRepository.GetStoreManagers(store.Id);
            }
            catch (Exception exception)
            {
                throw;
            }

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