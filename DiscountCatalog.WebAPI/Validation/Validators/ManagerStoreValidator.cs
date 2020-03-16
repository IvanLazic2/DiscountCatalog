using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiscountCatalog.WebAPI.Models;

namespace DiscountCatalog.WebAPI.Validation.Validators
{
    public class ManagerStoreValidator : AbstractValidator<ManagerStoreModel>
    {
        public ManagerStoreValidator()
        {
            RuleFor(m => m.Manager)
                .NotNull()
                .WithMessage("Manager does not exist.");

            RuleFor(m => m.Store)
                .NotNull()
                .WithMessage("Store does not exist.");
        }
    }
}