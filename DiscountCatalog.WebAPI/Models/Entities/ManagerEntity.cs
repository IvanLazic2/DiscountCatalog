using DiscountCatalog.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Models.Entities
{
    public class ManagerEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public ApplicationUser Identity { get; set; }
        [Required]
        public StoreAdminEntity Administrator { get; set; }
        
        public ICollection<StoreEntity> Stores { get; set; }
    }


}