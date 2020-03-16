using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Models.Result
{
    public class WorkingHoursValidatorResult
    {
        public Dictionary<string, string> Errors { get; set; }
        public bool Success
        {
            get
            {
                if (Errors.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            private set
            {
            }
        }
        public DateTime? WeekBegin { get; set; }
        public DateTime? WeekEnd { get; set; }
        public DateTime? WeekendsBegin { get; set; }
        public DateTime? WeekendsEnd { get; set; }
        public DateTime? HolidaysBegin { get; set; }
        public DateTime? HolidaysEnd { get; set; }

        public WorkingHoursValidatorResult()
        {
            Errors = new Dictionary<string, string>();
        }
    }
}