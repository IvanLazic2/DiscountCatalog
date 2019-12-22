﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string ProductOldPrice { get; set; }
        [Required]
        public string ProductNewPrice { get; set; }
        public string DiscountPercentage { get; set; }
        [Required]
        public DateTime DiscountDateBegin { get; set; }
        [Required]
        public DateTime DiscountDateEnd { get; set; }
        public string Quantity { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public bool Expired { get; set; }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
    }
}