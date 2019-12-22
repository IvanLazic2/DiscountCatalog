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

namespace AbatementHelper.WebAPI.Processors
{
    public static class ProductProcessor
    {
        public static WebApiProduct ProductEntityToWebApiProduct(ProductEntity product)
        {
            WebApiProduct webApiProduct = new WebApiProduct();

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<ProductEntity, WebApiProduct>()
                    .ForMember(s => s.Store, act => act.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                webApiProduct = mapper.Map<ProductEntity, WebApiProduct>(product);
            }
            catch (Exception exception)
            {

                throw;
            }

            return webApiProduct;
        }

        public static ProductEntity WebApiProductToProductEntity(WebApiProduct product)
        {
            ProductEntity productEntity = new ProductEntity();

            StoreRepository storeRepository = new StoreRepository();

        var config = new MapperConfiguration(c =>
            {
                c.CreateMap<WebApiProduct, ProductEntity > ()
                    .ForMember(s => s.Store, act => act.Ignore());
            });

            IMapper mapper = config.CreateMapper();

            try
            {
                productEntity = mapper.Map<WebApiProduct, ProductEntity>(product);

                if (productEntity != null)
                {
                    productEntity.Store = storeRepository.GetStore(product.Store.Id);
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