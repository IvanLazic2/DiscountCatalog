using AutoMapper;
using DiscountCatalog.Common.Models;
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

            });

            return config.CreateMapper();
        }
    }
}