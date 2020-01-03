using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.WebAPI.Processors
{
    public static class StoreProcessor
    {
        public static async Task<WebApiStore> StoreEntityToWebApiStoreAsync(StoreEntity store)
        {
            StoreAdminRepository storeAdminRepository = new StoreAdminRepository();

            WebApiStore webApiStore = new WebApiStore();

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<StoreEntity, WebApiStore>()
                    .ForMember(s => s.Managers, act => act.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                webApiStore = mapper.Map<StoreEntity, WebApiStore>(store);

                if (store.Managers != null)
                {
                    webApiStore.Managers = await storeAdminRepository.GetStoreManagersAsync(store.Id);
                }
            }
            catch (Exception exception)
            {
                throw;
            }

            return webApiStore;
        }

        public static StoreEntity WebApiStoreToStoreEntity(WebApiStore store)
        {
            var storeEntity = new StoreEntity();

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<WebApiStore, StoreEntity>()
                    .ForMember(s => s.Managers, act => act.Ignore())
                    .ForMember(s => s.StoreAdmin, act => act.Ignore())
                    .ForMember(s => s.Products, act => act.Ignore())
                    .ForMember(s => s.StoreImage, act => act.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                storeEntity = mapper.Map<WebApiStore, StoreEntity>(store);
            }
            catch (Exception exception)
            {

                throw;
            }

            return storeEntity;
        }
    }
}