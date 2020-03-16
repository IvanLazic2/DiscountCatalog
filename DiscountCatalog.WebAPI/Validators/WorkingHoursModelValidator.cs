using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.WebAPI.Extentions;
using DiscountCatalog.WebAPI.Models.Result;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Validators
{
    public class WorkingHoursModelValidator
    {
        public WorkingHoursValidatorResult GetErrors(WebApiStore store)
        {
            var result = new WorkingHoursValidatorResult();

            if (store.WorkingHoursWeekBegin != null)
            {
                if (!DateTime.TryParseExact(store.WorkingHoursWeekBegin, "hh:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime weekBegin))
                {
                    result.Errors.Add(ObjectExtensions.GetPropertyName(() => store.WorkingHoursWeekBegin), "Time has to be in format hh:mm");
                }
                else
                {
                    result.WeekBegin = weekBegin;
                }
            }
            else
            {
                result.WeekBegin = null;
            }

            if (store.WorkingHoursWeekEnd != null)
            {
                if (!DateTime.TryParseExact(store.WorkingHoursWeekEnd, "hh:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime weekEnd))
                {
                    result.Errors.Add(ObjectExtensions.GetPropertyName(() => store.WorkingHoursWeekEnd), "Time has to be in format hh:mm");
                }
                else
                {
                    result.WeekEnd = weekEnd;
                }
            }
            else
            {
                result.WeekEnd = null;
            }

            if (store.WorkingHoursWeekendsBegin != null)
            {
                if (!DateTime.TryParseExact(store.WorkingHoursWeekendsBegin, "hh:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime weekendsBegin))
                {
                    result.Errors.Add(ObjectExtensions.GetPropertyName(() => store.WorkingHoursWeekendsBegin), "Time has to be in format hh:mm");
                }
                else
                {
                    result.WeekendsBegin = weekendsBegin;
                }
            }
            else
            {
                result.WeekendsBegin = null;
            }

            if (store.WorkingHoursWeekendsEnd != null)
            {
                if (!DateTime.TryParseExact(store.WorkingHoursWeekendsEnd, "hh:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime weekendsEnd))
                {
                    result.Errors.Add(ObjectExtensions.GetPropertyName(() => store.WorkingHoursWeekendsEnd), "Time has to be in format hh:mm");
                }
                else
                {
                    result.WeekendsEnd = weekendsEnd;
                }
            }
            else
            {
                result.WeekendsEnd = null;
            }

            if (store.WorkingHoursHolidaysBegin != null)
            {
                if (!DateTime.TryParseExact(store.WorkingHoursHolidaysBegin, "hh:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime holidaysBegin))
                {
                    result.Errors.Add(ObjectExtensions.GetPropertyName(() => store.WorkingHoursHolidaysBegin), "Time has to be in format hh:mm");
                }
                else
                {
                    result.HolidaysBegin = holidaysBegin;
                }
            }
            else
            {
                result.HolidaysBegin = null;
            }

            if (store.WorkingHoursHolidaysEnd != null)
            {
                if (!DateTime.TryParseExact(store.WorkingHoursHolidaysEnd, "hh:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime holidaysEnd))
                {
                    result.Errors.Add(ObjectExtensions.GetPropertyName(() => store.WorkingHoursHolidaysEnd), "Time has to be in format hh:mm");
                }
                else
                {
                    result.HolidaysEnd = holidaysEnd;
                }
            }
            else
            {
                result.HolidaysEnd = null;
            }

            return result;
        }
    }
}