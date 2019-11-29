﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.CommonModels.Models
{
    public class DataBaseStore
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string WorkingHoursWeek { get; set; }
        public string WorkingHoursWeekends { get; set; }
        public string WorkingHoursHolidays { get; set; }
        public string Role { get; set; }
        public bool Approved { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string MasterStoreID { get; set; }
        public bool Deleted { get; set; }
    }
}
