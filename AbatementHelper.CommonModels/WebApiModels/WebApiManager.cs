﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.CommonModels.WebApiModels
{
    public class WebApiManager
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string StoreAdminName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string Role { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }
        public List<WebApiStore> Stores { get; set; } = new List<WebApiStore>();
    }
}
