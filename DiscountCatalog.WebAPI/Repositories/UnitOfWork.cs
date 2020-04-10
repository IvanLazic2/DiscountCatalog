using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Repositories.EntityRepositories;
using DiscountCatalog.WebAPI.Repositories.EntityRepositories.Contractor;
using DiscountCatalog.WebAPI.Repositories.EntityRepositories.Implementation;

namespace DiscountCatalog.WebAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationUserDbContext _context;

        public IStoreAdminRepository StoreAdmins { get; private set; }
        public IManagerRepository Managers { get; private set; }
        public IStoreRepository Stores { get; private set; }
        public IProductRepository Products { get; private set; }
        public IAccountRepository Accounts { get; private set; }
        public IRoleRepository Roles { get; private set; }

        public UnitOfWork(ApplicationUserDbContext context)
        {
            _context = context;

            StoreAdmins = new StoreAdminRepository(_context);
            Managers = new ManagerRepository(_context);
            Stores = new StoreRepository(_context);
            Products = new ProductRepository(_context);
            Accounts = new AccountRepository(_context);
            Roles = new RoleRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}