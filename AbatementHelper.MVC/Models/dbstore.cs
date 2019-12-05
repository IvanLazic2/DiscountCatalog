﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.MVC.Models
{
    public class dbstore
    {
        public string Id { get; set; }
        public string StoreName { get; set; }
        public string WorkingHoursWeek { get; set; }
        public string WorkingHoursWeekends { get; set; }
        public string WorkingHoursHolidays { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string StoreAdminId { get; set; }
        public List<string> Managers { get; set; }
        public bool Approved { get; set; }
        public bool Deleted { get; set; }
    }
}
