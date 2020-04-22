using DiscountCatalog.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Models.Entities
{
    public class StoreEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Index(IsUnique = false), Required, StringLength(200)]
        public string StoreName { get; set; }

        public ICollection<ManagerEntity> Managers { get; set; }
        public string WorkingHoursWeekBegin { get; set; }
        public string WorkingHoursWeekEnd { get; set; }
        public string WorkingHoursWeekendsBegin { get; set; }
        public string WorkingHoursWeekendsEnd { get; set; }
        public string WorkingHoursHolidaysBegin { get; set; }
        public string WorkingHoursHolidaysEnd { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }

        [Required]
        public StoreAdminEntity Administrator { get; set; }
        public ICollection<ProductEntity> Products { get; set; }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }

        public ImageEntity StoreImage { get; set; }
    }
}