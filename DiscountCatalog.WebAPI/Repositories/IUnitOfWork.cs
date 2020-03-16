using DiscountCatalog.WebAPI.Repositories.EntityRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.WebAPI.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IStoreAdminRepository StoreAdmins { get; }
        IManagerRepository Managers { get; }
        IStoreRepository Stores { get; }
        IProductRepository Products { get; }
        IAccountRepository Accounts { get;  }
        IRoleRepository Roles { get; }
        int Complete();
    }
}
