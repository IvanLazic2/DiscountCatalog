using DiscountCatalog.WebAPI.Models.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Validation.Validators
{
    public class StoreValidator : AbstractValidator<StoreEntity>
    {
        public StoreValidator()
        {
            RuleFor(s => s.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id not set");

            RuleFor(s => s.StoreName)
                .NotNull()
                .WithMessage("Store name should not be empty.");

            RuleFor(s => s.Administrator)
                .NotNull()
                .WithMessage("Store admin does not exist.")
                .OverridePropertyName(string.Empty);

            /////////////////////////////////////

            RuleFor(s => s.WorkingHoursWeekBegin)
                .NotNull()
                .WithMessage("Working hours beginning should not be empty.")
                .When(s => s.WorkingHoursWeekEnd != null);

            RuleFor(s => s.WorkingHoursWeekEnd)
                .NotNull()
                .WithMessage("Working hours end should not be empty.")
                .When(s => s.WorkingHoursWeekBegin != null);

            RuleFor(s => s.WorkingHoursWeekEnd)
                .GreaterThan(s => s.WorkingHoursWeekBegin)
                .WithMessage("Working hours end should be later than beginning.")
                .When(s => s.WorkingHoursWeekBegin != null);

            RuleFor(s => s.WorkingHoursWeekendsBegin)
                .NotNull()
                .WithMessage("Working hours beginning should not be empty.")
                .When(s => s.WorkingHoursWeekendsEnd != null);

            RuleFor(s => s.WorkingHoursWeekendsEnd)
                .NotNull()
                .WithMessage("Working hours end should not be empty.")
                .When(s => s.WorkingHoursWeekendsBegin != null);

            RuleFor(s => s.WorkingHoursWeekendsEnd)
                .GreaterThan(s => s.WorkingHoursWeekendsBegin)
                .WithMessage("Working hours end should be later than beginning.")
                .When(s => s.WorkingHoursWeekendsBegin != null);

            RuleFor(s => s.WorkingHoursHolidaysBegin)
                .NotNull()
                .WithMessage("Working hours beginning should not be empty.")
                .When(s => s.WorkingHoursHolidaysEnd != null);

            RuleFor(s => s.WorkingHoursHolidaysEnd)
                .NotNull()
                .WithMessage("Working hours end should not be empty.")
                .When(s => s.WorkingHoursHolidaysBegin != null);

            RuleFor(s => s.WorkingHoursHolidaysEnd)
                .GreaterThan(s => s.WorkingHoursHolidaysBegin)
                .WithMessage("Working hours end should be later than beginning.")
                .When(s => s.WorkingHoursHolidaysBegin != null);
        }
    }
}