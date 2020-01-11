using AbatementHelper.CommonModels.WebApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Models.Response
{
    public class WorkingHoursResponseModel
    {
        public WorkingHoursModel WorkingHours { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }

        public WorkingHoursResponseModel()
        {
            WorkingHours = new WorkingHoursModel();
        }
    }
}