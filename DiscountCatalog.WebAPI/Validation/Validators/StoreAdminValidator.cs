using DiscountCatalog.WebAPI.Models.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Validation.Validators
{
    public class StoreAdminValidator : AbstractValidator<StoreAdminEntity>
    {
        public StoreAdminValidator()
        {
            RuleFor(s => s.Identity)
                .NotNull()
                .WithMessage("Failed to create user.")
                .OverridePropertyName(string.Empty); 
        }
    }
}