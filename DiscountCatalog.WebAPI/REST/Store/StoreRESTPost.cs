using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.REST.Store
{
    public class StoreRESTPost
    {
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
    }
}