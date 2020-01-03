using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using System.Threading.Tasks;

namespace AbatementHelper.WebAPI.Processors
{
    public static class ProductProcessor
    {
        public static async Task<WebApiProduct> ProductEntityToWebApiProductAsync(ProductEntity product)
        {
            WebApiProduct webApiProduct = new WebApiProduct();

            StoreRepository storeRepository = new StoreRepository();

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<ProductEntity, WebApiProduct>()
                    .ForMember(s => s.Store, act => act.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                webApiProduct = mapper.Map<ProductEntity, WebApiProduct>(product);

                if (product.Store != null)
                {
                    webApiProduct.Store = await StoreProcessor.StoreEntityToWebApiStoreAsync(product.Store);
                }

                
            }
            catch (Exception exception)
            {

                throw;
            }

            return webApiProduct;
        }

        public static async Task<ProductEntity> WebApiProductToProductEntityAsync(WebApiProduct product)
        {
            ProductEntity productEntity = new ProductEntity();

            StoreRepository storeRepository = new StoreRepository();

        var config = new MapperConfiguration(c =>
            {
                c.CreateMap<WebApiProduct, ProductEntity>()
                    .ForMember(s => s.Store, act => act.Ignore())
                    .ForMember(s => s.Id, act => act.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                productEntity = mapper.Map<WebApiProduct, ProductEntity>(product);

                if (productEntity != null)
                {
                    productEntity.Store = await storeRepository.GetStoreAsync(product.Store.Id);
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return productEntity;
        }
    }
}