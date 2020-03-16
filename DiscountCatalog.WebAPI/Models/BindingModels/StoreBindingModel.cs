﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Models.BindingModels
{
    public class StoreBindingModel
    {
        public string Id { get; set; }
        public string StoreName { get; set; }
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
        public string StoreAdminId { get; set; }
        public bool Approved { get; set; }
    }
}