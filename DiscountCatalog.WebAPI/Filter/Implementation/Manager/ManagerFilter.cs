using DiscountCatalog.WebAPI.Filter.Contractor;
using DiscountCatalog.WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace DiscountCatalog.WebAPI.Filter.Implementation.Manager
{
    public class ManagerFilter : IFilter<ManagerEntity>
    {
        public IQueryable<ManagerEntity> Filter(IQueryable<ManagerEntity> managers)
        {
            return managers;
        }

        public ManagerEntity Filter(ManagerEntity manager)
        {
            return manager;
        }
    }
}