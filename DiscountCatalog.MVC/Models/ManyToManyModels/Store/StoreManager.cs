﻿using DiscountCatalog.MVC.REST.Manager;
using DiscountCatalog.MVC.REST.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Models.ManyToManyModels.Store
{
    public class StoreManager
    {
        public ManagerREST Manager { get; set; }
        public bool Assigned { get; set; }
    }
}