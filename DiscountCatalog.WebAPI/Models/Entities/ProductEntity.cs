using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Models.Entities
{
    public class ProductEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public StoreEntity Store { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        [Required]
        public decimal? OldPrice { get; set; }
        [Required]
        public decimal? NewPrice { get; set; }
        public string Currency { get; set; }
        [Required]
        public decimal? DiscountPercentage { get; set; }
        [Required]
        public string DiscountDateBegin { get; set; }
        [Required]
        public string DiscountDateEnd { get; set; }
        public string Quantity { get; set; }
        public string MeasuringUnit { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public bool Expired { get; set; }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public byte[] ProductImage { get; set; }
    }
}