using AutoMapper;
using DiscountCatalog.WebAPI.BackendServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.App_Start
{
    public class Startup
    {
        public Refresher Refresher { get; set; }

        public Startup()
        {
            Refresher.Refresh();
        }

        public void MapperConfiguration()
        {
        }

        public void Testing()
        {
        }

        public void Refresh()
        {
            
        }
    }
}