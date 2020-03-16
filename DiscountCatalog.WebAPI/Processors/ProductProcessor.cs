//using DiscountCatalog.Common.Models;
//using DiscountCatalog.Common.WebApiModels;
//using DiscountCatalog.WebAPI.DataBaseModels;
//using DiscountCatalog.WebAPI.Models;
//using DiscountCatalog.WebAPI.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using AutoMapper;
//using System.Threading.Tasks;

//namespace DiscountCatalog.WebAPI.Processors
//{
//    public static class ProductProcessor
//    {
//        public static async Task<WebApiProduct> ProductEntityToWebApiProductAsync(DataBaseModels.Product product)
//        {
//            WebApiProduct webApiProduct = new WebApiProduct();

//            StoreRepository storeRepository = new StoreRepository();

//            var config = new MapperConfiguration(c =>
//            {
//                c.CreateMap<DataBaseModels.Product, WebApiProduct>()
//                    .ForMember(s => s.Store, act => act.Ignore());
//            });

//            IMapper mapper = config.CreateMapper();

//            try
//            {
//                webApiProduct = mapper.Map<DataBaseModels.Product, WebApiProduct>(product);

//                if (product.Store != null)
//                {
//                    webApiProduct.Store = await StoreProcessor.StoreEntityToWebApiStoreAsync(product.Store);
//                }

                
//            }
//            catch (Exception exception)
//            {

//                throw;
//            }

//            return webApiProduct;
//        }

//        public static async Task<DataBaseModels.Product> WebApiProductToProductEntityAsync(WebApiProduct product)
//        {
//            DataBaseModels.Product productEntity = new DataBaseModels.Product();

//            StoreRepository storeRepository = new StoreRepository();

//        var config = new MapperConfiguration(c =>
//            {
//                c.CreateMap<WebApiProduct, DataBaseModels.Product>()
//                    .ForMember(s => s.Store, act => act.Ignore())
//                    .ForMember(s => s.Id, act => act.Ignore());
//            });

//            IMapper mapper = config.CreateMapper();

//            try
//            {
//                productEntity = mapper.Map<WebApiProduct, DataBaseModels.Product>(product);

//                if (productEntity != null)
//                {
//                    productEntity.Store = await storeRepository.GetStoreAsync(product.Store.Id);
//                }
//            }
//            catch (Exception exception)
//            {

//                throw;
//            }

//            return productEntity;
//        }
//    }
//}