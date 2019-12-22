using AbatementHelper.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.DataBaseModels
{
    public class StoreEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Index(IsUnique = true), Required, StringLength(200)]
        public string StoreName { get; set; }
        public ICollection<ManagerEntity> Managers { get; set; }
        public string WorkingHoursWeek { get; set; }
        public string WorkingHoursWeekends { get; set; }
        public string WorkingHoursHolidays { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        [Required]
        public ApplicationUser StoreAdmin { get; set; }
        public ICollection<ProductEntity> Products { get; set; }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }

    }
}