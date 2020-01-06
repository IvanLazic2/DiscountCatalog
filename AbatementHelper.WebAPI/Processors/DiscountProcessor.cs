using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Processors
{
    public static class DiscountProcessor
    {
        public static DiscountResponseModel DiscountCalculator(DiscountModel discount)
        {
            var discountReturnModel = new DiscountResponseModel();

            if (discount.OldPrice != 0)
            {
                if (discount.NewPrice != 0)
                {
                    if (discount.Discount != 0)
                    {
                        //popunit novu cijenu
                        discountReturnModel.Discount.NewPrice = Math.Round(discount.OldPrice - (discount.Discount / 100 * discount.OldPrice), 2);
                        discountReturnModel.Discount.OldPrice = discount.OldPrice;
                        discountReturnModel.Discount.Discount = discount.Discount;
                        discountReturnModel.Message = "New price is changed";
                        discountReturnModel.Success = true;
                    }
                    else
                    {
                        //popunit postotak
                        discountReturnModel.Discount.Discount = Math.Round(100 - (discount.NewPrice / discount.OldPrice) * 100, 1);
                        discountReturnModel.Discount.OldPrice = discount.OldPrice;
                        discountReturnModel.Discount.NewPrice = discount.NewPrice;
                        discountReturnModel.Message = "Success";
                        discountReturnModel.Success = true;
                    }
                }
                else
                {
                    if (discount.Discount != 0)
                    {
                        //popunit novu cijenu
                        discountReturnModel.Discount.NewPrice = Math.Round(discount.OldPrice - (discount.Discount / 100 * discount.OldPrice), 2);
                        discountReturnModel.Discount.OldPrice = discount.OldPrice;
                        discountReturnModel.Discount.Discount = discount.Discount;
                        discountReturnModel.Message = "Success";
                        discountReturnModel.Success = true;
                    }
                    else
                    {
                        //error
                        discountReturnModel.Message = "Fill in at least two properties";
                        discountReturnModel.Success = false;
                    }
                }
            }
            else if (discount.NewPrice != 0)
            {
                if (discount.Discount != 0)
                {
                    //popunit staru cijenu
                    discountReturnModel.Discount.OldPrice = Math.Round(discount.NewPrice / discount.Discount * 100, 2);
                    discountReturnModel.Discount.NewPrice = discount.NewPrice;
                    discountReturnModel.Discount.Discount = discount.Discount;
                    discountReturnModel.Message = "Success";
                    discountReturnModel.Success = true;
                }
                else
                {
                    //error
                    discountReturnModel.Message = "Fill in at least two properties";
                    discountReturnModel.Success = false;
                }
            }
            else if (discount.Discount != 0)
            {
                //error
                discountReturnModel.Message = "Fill in at least two properties";
                discountReturnModel.Success = false;
            }

            return discountReturnModel;
        }
    }
}