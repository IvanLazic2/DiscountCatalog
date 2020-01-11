using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.CommonModels.WebApiModels
{
    public class WorkingHoursModel
    {
        public DateTime WeekBegin { get; set; }
        public DateTime WeekEnd { get; set; }
        public DateTime WeekendsBegin { get; set; }
        public DateTime WeekendsEnd { get; set; }
        public DateTime HolidaysBegin { get; set; }
        public DateTime HolidaysEnd { get; set; }
    }
}
