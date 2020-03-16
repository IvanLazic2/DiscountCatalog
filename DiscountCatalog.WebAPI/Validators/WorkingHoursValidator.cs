using DiscountCatalog.Common.WebApiModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Validators
{
    public class WorkingHoursValidator : AbstractValidator<WebApiStore>
    {
        public WorkingHoursValidator()
        {
            RuleFor(s => s._WorkingHoursWeekBegin)
                .NotNull().WithMessage("Working hours beginning should not be empty.")
                .When(s => s._WorkingHoursWeekEnd != null);

            RuleFor(s => s._WorkingHoursWeekEnd)
                .NotNull().WithMessage("Working hours end should not be empty.")
                .When(s => s._WorkingHoursWeekBegin != null);

            RuleFor(s => s._WorkingHoursWeekEnd)
                .GreaterThan(s => s._WorkingHoursWeekBegin).WithMessage("Working hours end should be later than beginning.")
                .When(s => s._WorkingHoursWeekBegin != null);

            RuleFor(s => s._WorkingHoursWeekendsBegin)
                .NotNull().WithMessage("Working hours beginning should not be empty.")
                .When(s => s._WorkingHoursWeekendsEnd != null);

            RuleFor(s => s._WorkingHoursWeekendsEnd)
                .NotNull().WithMessage("Working hours end should not be empty.")
                .When(s => s._WorkingHoursWeekendsBegin != null);

            RuleFor(s => s._WorkingHoursWeekendsEnd)
                .GreaterThan(s => s._WorkingHoursWeekendsBegin).WithMessage("Working hours end should be later than beginning.")
                .When(s => s._WorkingHoursWeekendsBegin != null);

            RuleFor(s => s._WorkingHoursHolidaysBegin)
                .NotNull().WithMessage("Working hours beginning should not be empty.")
                .When(s => s._WorkingHoursHolidaysEnd != null);

            RuleFor(s => s._WorkingHoursHolidaysEnd)
                .NotNull().WithMessage("Working hours end should not be empty.")
                .When(s => s._WorkingHoursHolidaysBegin != null);

            RuleFor(s => s._WorkingHoursHolidaysEnd)
                .GreaterThan(s => s._WorkingHoursHolidaysBegin).WithMessage("Working hours end should be later than beginning.")
                .When(s => s._WorkingHoursHolidaysBegin != null);
        }
    }
}