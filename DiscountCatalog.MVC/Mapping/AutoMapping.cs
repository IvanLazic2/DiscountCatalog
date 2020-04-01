using AutoMapper;
using DiscountCatalog.Common.Models;
using DiscountCatalog.MVC.REST.Account;
using DiscountCatalog.MVC.REST.Manager;
using DiscountCatalog.MVC.REST.Product;
using DiscountCatalog.MVC.REST.Store;
using DiscountCatalog.MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.MVC.Mapping
{
    public class AutoMapping
    {
        public static IMapper Initialise()
        {
            var config = new MapperConfiguration(c =>
            {
                //VIEWMODELS

                c.CreateMap<User, UserViewModel>();

                //REST

                c.CreateMap<AccountRESTPut, AccountREST>();
                c.CreateMap<AccountRESTPost, AccountREST>();

                c.CreateMap<StoreRESTPut, StoreREST>();
                c.CreateMap<StoreRESTPost, StoreREST>();

                c.CreateMap<ManagerRESTPut, ManagerREST>();
                c.CreateMap<ManagerRESTPost, ManagerREST>();

                c.CreateMap<ProductRESTPut, ProductREST>();
                c.CreateMap<ProductRESTPost, ProductREST>();

            });

            return config.CreateMapper();
        }
    }
}