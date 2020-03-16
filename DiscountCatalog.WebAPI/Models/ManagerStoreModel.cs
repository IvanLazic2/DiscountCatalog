using DiscountCatalog.WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Models
{
    public class ManagerStoreModel
    {
        public ManagerEntity Manager { get; set; }
        public StoreEntity Store { get; set; }

        public ManagerStoreModel(ManagerEntity manager, StoreEntity store)
        {
            Manager = manager;
            Store = store;
        }
    }
}