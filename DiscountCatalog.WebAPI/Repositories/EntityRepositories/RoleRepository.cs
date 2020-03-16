using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories
{
    public class RoleRepository : Repository<IdentityRole>, IRoleRepository
    {
        private RoleStore<IdentityRole> _roleStore { get; set; }
        private RoleManager _roleManager { get; set; }

        public ApplicationUserDbContext DbContext
        {
            get
            {
                return Context as ApplicationUserDbContext;
            }
        }

        public RoleRepository(ApplicationUserDbContext context)
            :base(context)
        {
            _roleStore = new RoleStore<IdentityRole>(Context);
            _roleManager = new RoleManager(_roleStore);
        }

        public Task<IdentityRole> FindByNameAsync(string name)
        {
            return _roleManager.FindByNameAsync(name);
        }
    }
}