using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.DataBaseModels
{
    public class ProductEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public StoreEntity Store { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        [Required]
        public double ProductOldPrice { get; set; }
        [Required]
        public double ProductNewPrice { get; set; }
        public double DiscountPercentage { get; set; }
        [Required, Column(TypeName = "datetime2")]
        public DateTime DiscountDateBegin { get; set; }
        [Required, Column(TypeName = "datetime2")]
        public DateTime DiscountDateEnd { get; set; }
        public string Quantity { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public bool Expired { get; set; }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public byte[] ProductImage { get; set; }
    }
}