//using DiscountCatalog.Common.WebApiModels;
//using DiscountCatalog.WebAPI.DataBaseModels;
//using DiscountCatalog.WebAPI.Models;
//using DiscountCatalog.WebAPI.Repositories;
//using AutoMapper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;

//namespace DiscountCatalog.WebAPI.Processors
//{
//    public static class StoreProcessor
//    {
//        public static async Task<WebApiStore> StoreEntityToWebApiStoreAsync(Store store)
//        {
//            StoreAdminRepository storeAdminRepository = new StoreAdminRepository();

//            WebApiStore webApiStore = new WebApiStore();

//            var config = new MapperConfiguration(c =>
//            {
//                c.CreateMap<Store, WebApiStore>()
//                    .ForMember(s => s.Managers, act => act.Ignore());
//            });

//            IMapper mapper = config.CreateMapper();

//            try
//            {
//                webApiStore = mapper.Map<Store, WebApiStore>(store);

//                if (store.Managers != null)
//                {
//                    webApiStore.Managers = await storeAdminRepository.GetStoreManagersAsync(store.Id);
//                }
//            }
//            catch (Exception exception)
//            {
//                throw;
//            }

//            return webApiStore;
//        }

//        public static Store WebApiStoreToStoreEntity(WebApiStore store)
//        {
//            var storeEntity = new Store();

//            var config = new MapperConfiguration(c =>
//            {
//                c.CreateMap<WebApiStore, Store>()
//                    .ForMember(s => s.Managers, act => act.Ignore())
//                    .ForMember(s => s.StoreAdmin, act => act.Ignore())
//                    .ForMember(s => s.Products, act => act.Ignore())
//                    .ForMember(s => s.StoreImage, act => act.Ignore())
//                    .ForMember(s => s.Id, act => act.Ignore());
//            });

//            IMapper mapper = config.CreateMapper();

//            try
//            {
//                storeEntity = mapper.Map<WebApiStore, Store>(store);
//            }
//            catch (Exception exception)
//            {

//                throw;
//            }

//            return storeEntity;
//        }
//    }
//}