using AbatementHelper.WebAPI.Models.BindingModels;
using AutoMapper;
using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.Models.Extended;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Mapping
{
    public class AutoMapping
    {
        public static IMapper Initialise()
        {
            var config = new MapperConfiguration(c =>
            {
                //BINDING MODELS

                c.CreateMap<UserBindingModel, ApplicationUser>();

                c.CreateMap<StoreAdminBindingModel, ApplicationUser>();

                c.CreateMap<StoreBindingModel, StoreEntity>();

                c.CreateMap<ManagerBindingModel, ApplicationUser>();

                //COMMON MODELS

                c.CreateMap<ApplicationUser, User>();
                    //.ForMember(dst => dst.Id, act => act.Ignore());

                c.CreateMap<ApplicationUser, StoreAdmin>();

                c.CreateMap<ApplicationUser, Manager>();

                c.CreateMap<StoreEntity, Store>()
                    .ForMember(dst => dst.Products, opt => opt.MapFrom(src => src.Products));

                c.CreateMap<ProductEntity, Product>()
                    .ForMember(dst => dst.Store, opt => opt.MapFrom(src => src.Store));

                c.CreateMap<StoreAdminEntity, StoreAdmin>()
                    .ForMember(dst => dst.Identity, opt => opt.MapFrom(src => src.Identity))
                    .ForMember(dst => dst.Managers, opt => opt.MapFrom(src => src.Managers))
                    .ForMember(dst => dst.Stores, opt => opt.MapFrom(src => src.Stores));

                c.CreateMap<ManagerEntity, Manager>()
                    .ForMember(dst => dst.Identity, opt => opt.MapFrom(src => src.Identity))
                    .ForMember(dst => dst.Stores, opt => opt.MapFrom(src => src.Stores))
                    .ForMember(dst => dst.Administrator, opt => opt.MapFrom(src => src.Administrator));


                //c.CreateMap<AuthenticatedUser, AuthenticatedUserResult>();
            });

            return config.CreateMapper();
        }
    }
}