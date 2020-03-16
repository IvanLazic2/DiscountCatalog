using DiscountCatalog.Common.WebApiModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Validators
{
    public class DiscountValidator : AbstractValidator<WebApiProduct>
    {
        public DiscountValidator()
        {
            RuleFor(d => d.NewPrice)
                .LessThan(d => d.OldPrice).WithMessage("New price has to be a discount!")
                .Unless(d => !d.OldPrice.HasValue);


            RuleFor(d => d.OldPrice)
                .NotNull()
                .When(d => d.NewPrice.HasValue && !d.Discount.HasValue || !d.NewPrice.HasValue && d.Discount.HasValue)
                .WithMessage("Fill in at least two properties");

            RuleFor(d => d.NewPrice)
                .NotNull()
                .When(d => d.OldPrice.HasValue && !d.Discount.HasValue || !d.OldPrice.HasValue && d.Discount.HasValue)
                .WithMessage("Fill in at least two properties");

            RuleFor(d => d.Discount)
                .NotNull()
                .When(d => d.NewPrice.HasValue && !d.OldPrice.HasValue || !d.NewPrice.HasValue && d.OldPrice.HasValue)
                .WithMessage("Fill in at least two properties");

            RuleFor(d => d.OldPrice)
                .NotNull()
                .When(d => !d.NewPrice.HasValue && !d.OldPrice.HasValue && !d.Discount.HasValue)
                .WithMessage("Fill in at least two properties");

            RuleFor(d => d.NewPrice)
                .NotNull()
                .When(d => !d.NewPrice.HasValue && !d.OldPrice.HasValue && !d.Discount.HasValue)
                .WithMessage("Fill in at least two properties");

            RuleFor(d => d.Discount)
                .NotNull()
                .When(d => !d.NewPrice.HasValue && !d.OldPrice.HasValue && !d.Discount.HasValue)
                .WithMessage("Fill in at least two properties");
        }
    }
}