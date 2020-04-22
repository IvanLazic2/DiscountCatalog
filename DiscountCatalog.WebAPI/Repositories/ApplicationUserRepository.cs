using DiscountCatalog.WebAPI.Models;
using DiscountCatalog.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AbatementHelper.WebAPI.Repositories
{
    public class ApplicationUserRepository : IRepository<ApplicationUser>
    {
        protected readonly DbContext Context;

        public ApplicationUserRepository(DbContext context)
        {
            Context = context;
        }

        public void Add(ApplicationUser entity)
        {
            Context.Set<ApplicationUser>().Add(entity);
        }

        public void AddRange(IEnumerable<ApplicationUser> entities)
        {
            Context.Set<ApplicationUser>().AddRange(entities);
        }

        public IQueryable<ApplicationUser> Find(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return Context.Set<ApplicationUser>().Where(predicate).AsQueryable();
        }

        public ApplicationUser Get(string id)
        {
            return Context.Set<ApplicationUser>().Find(id);
        }

        public IQueryable<ApplicationUser> GetAll()
        {
            return Context.Set<ApplicationUser>().AsQueryable();
        }

        public void Remove(ApplicationUser entity)
        {
            Context.Set<ApplicationUser>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<ApplicationUser> entities)
        {
            Context.Set<ApplicationUser>().RemoveRange(entities);
        }
    }
}