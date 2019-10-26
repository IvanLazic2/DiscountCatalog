using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Models
{
    public class DataBaseUser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string Role { get; set; }
        public bool Approved { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }


        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }


        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }

    }
}