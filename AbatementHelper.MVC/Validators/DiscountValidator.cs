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

            //RuleFor(d => d.Discount.NewPrice)
            //    .LessThan(d => d.Discount.OldPrice).WithMessage("New price has to be a discount!")
            //    .Unless(d => d.Discount.OldPrice == 0);


            //RuleFor(d => d.Discount.OldPrice)
            //    .NotEmpty()
            //    .Unless(d => d.Discount.NewPrice != 0 && d.Discount.Discount != 0);
        }
    }
}