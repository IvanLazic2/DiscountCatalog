using AbatementHelper.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.DataBaseModels
{
    public class ManagerEntity : ApplicationUser
    {
        public virtual ICollection<StoreEntity> Stores { get; set; }
    }
}