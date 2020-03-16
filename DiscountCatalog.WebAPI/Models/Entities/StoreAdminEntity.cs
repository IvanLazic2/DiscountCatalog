using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Models.Entities
{
    public class StoreAdminEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public ApplicationUser Identity { get; set; }
        public ICollection<StoreEntity> Stores { get; set; }
        public ICollection<ManagerEntity> Managers { get; set; }
    }
}