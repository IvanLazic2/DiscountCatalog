using AbatementHelper.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.DataBaseModels
{
    public class ManagerEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        //[Required]
        public ApplicationUser User { get; set; }
        //[Required]
        public ApplicationUser StoreAdmin { get; set; }
        public ICollection<StoreEntity> Stores { get; set; }

    }


}