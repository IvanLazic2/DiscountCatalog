using DiscountCatalog.WebAPI.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor
{
    public interface IRoleRepository : IRepository<IdentityRole>
    {
        Task<IdentityRole> FindByNameAsync(string name);
    }
}
