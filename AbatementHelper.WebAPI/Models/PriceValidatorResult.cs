using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Models
{
    public class PriceValidatorResult
    {
        public Dictionary<string, string> Errors { get; set; }
        public bool Success { get; set; }
        public decimal? OldPrice { get; set; }
        public decimal? NewPrice { get; set; }
        public decimal? Discount { get; set; }

        public PriceValidatorResult()
        {
            Errors = new Dictionary<string, string>();
        }
    }
}