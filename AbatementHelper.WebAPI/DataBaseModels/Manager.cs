using AbatementHelper.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.DataBaseModels
{
    public class ManagerEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public ApplicationUser User { get; set; }
        public virtual ICollection<StoreEntity> Stores { get; set; }
    }
}