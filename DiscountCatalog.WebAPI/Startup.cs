using System;
using System.Collections.Generic;
using System.Linq;
using DiscountCatalog.WebAPI.BackendServices;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DiscountCatalog.WebAPI.Startup))]

namespace DiscountCatalog.WebAPI
{
    public partial class Startup
    {
        public Refresher Refresher { get; set; }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            Refresher.Refresh();
        }

        
    }
}
