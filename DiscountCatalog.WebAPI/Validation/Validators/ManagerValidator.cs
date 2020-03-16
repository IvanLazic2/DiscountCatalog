using DiscountCatalog.WebAPI.Models.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Validation.Validators
{
    public class ManagerValidator : AbstractValidator<ManagerEntity>
    {
        public ManagerValidator()
        {
            RuleFor(m => m.Identity)
                .NotNull()
                .WithMessage("Failed to create user.")
                .OverridePropertyName(string.Empty);

            RuleFor(m => m.Administrator)
                .NotNull()
                .WithMessage("Store administrator does not exist.")
                .OverridePropertyName(string.Empty);
        }
    }
}