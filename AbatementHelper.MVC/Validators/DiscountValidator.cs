using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.MVC.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.MVC.Validators
{
    public class DiscountValidator : AbstractValidator<ProductViewModel>
    {
        public DiscountValidator()
        {
            //RuleFor(p => p.ProductNewPrice)
            //    .LessThan(p => p.ProductOldPrice).WithMessage();

            RuleFor(d => d.Discount.NewPrice)
                .LessThan(d => d.Discount.OldPrice).WithMessage("New price has to be a discount!")
                .Unless(d => !d.Discount.OldPrice.HasValue);


            RuleFor(d => d.Discount.OldPrice)
                .NotNull()
                .When(d => d.Discount.NewPrice.HasValue && !d.Discount.Discount.HasValue || !d.Discount.NewPrice.HasValue && d.Discount.Discount.HasValue)
                .WithMessage("Fill in at least two properties");

            RuleFor(d => d.Discount.NewPrice)
                .NotNull()
                .When(d => d.Discount.OldPrice.HasValue && !d.Discount.Discount.HasValue || !d.Discount.OldPrice.HasValue && d.Discount.Discount.HasValue)
                .WithMessage("Fill in at least two properties");

            RuleFor(d => d.Discount.Discount)
                .NotNull()
                .When(d => d.Discount.NewPrice.HasValue && !d.Discount.OldPrice.HasValue || !d.Discount.NewPrice.HasValue && d.Discount.OldPrice.HasValue)
                .WithMessage("Fill in at least two properties");

            RuleFor(d => d.Discount.OldPrice)
                .NotNull()
                .When(d => !d.Discount.NewPrice.HasValue && !d.Discount.OldPrice.HasValue && !d.Discount.Discount.HasValue)
                .WithMessage("Fill in at least two properties");

            RuleFor(d => d.Discount.NewPrice)
                .NotNull()
                .When(d => !d.Discount.NewPrice.HasValue && !d.Discount.OldPrice.HasValue && !d.Discount.Discount.HasValue)
                .WithMessage("Fill in at least two properties");

            RuleFor(d => d.Discount.Discount)
                .NotNull()
                .When(d => !d.Discount.NewPrice.HasValue && !d.Discount.OldPrice.HasValue && !d.Discount.Discount.HasValue)
                .WithMessage("Fill in at least two properties");
        }
    }
}