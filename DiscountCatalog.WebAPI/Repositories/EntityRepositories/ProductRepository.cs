using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace DiscountCatalog.WebAPI.Repositories.EntityRepositories
{
    public class ProductRepository : Repository<ProductEntity>, IProductRepository
    {
        public ApplicationUserDbContext DbContext
        {
            get
            {
                return Context as ApplicationUserDbContext;
            }
        }

        public ProductRepository(ApplicationUserDbContext context)
            :base(context)
        {
        }

        public IEnumerable<ProductEntity> GetProductsWithStore()
        {
            return DbContext.Products
                .Include(p => p.Store)
                .ToList();
        }
    }
}