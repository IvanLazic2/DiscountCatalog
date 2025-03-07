﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.Common.WebApiModels
{
    public class WebApiStore
    {
        public string Id { get; set; }
        [Required]
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
        public bool Deleted { get; set; }
        public List<WebApiManager> Managers { get; set; } = new List<WebApiManager>();

        public DateTime? _WorkingHoursWeekBegin { get; set; }
        public DateTime? _WorkingHoursWeekEnd { get; set; }
        public DateTime? _WorkingHoursWeekendsBegin { get; set; }
        public DateTime? _WorkingHoursWeekendsEnd { get; set; }
        public DateTime? _WorkingHoursHolidaysBegin { get; set; }
        public DateTime? _WorkingHoursHolidaysEnd { get; set; }
    }
}
