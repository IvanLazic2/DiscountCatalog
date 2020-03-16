//using DiscountCatalog.Common.WebApiModels;
//using DiscountCatalog.WebAPI.Interfaces;
//using DiscountCatalog.WebAPI.Models;
//using DiscountCatalog.WebAPI.Models.Result;
//using DiscountCatalog.WebAPI.Validators;
//using FluentValidation.Results;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace DiscountCatalog.WebAPI.EntityValidation
//{
//    public class StoreValidation : IStoreValidation
//    {
//        public ModelStateResponse Validate(WebApiStore store)
//        {
//            var response = new ModelStateResponse();

//            var workingHoursModelValidator = new WorkingHoursModelValidator();

//            WorkingHoursValidatorResult workingHoursModelValidatorResult = workingHoursModelValidator.GetErrors(store);

//            if (!workingHoursModelValidatorResult.Success)
//            {
//                foreach (var error in workingHoursModelValidatorResult.Errors)
//                {
//                    response.ModelState.Add(error.Key, error.Value);
//                }
//            }

//            store._WorkingHoursWeekBegin = workingHoursModelValidatorResult.WeekBegin;
//            store._WorkingHoursWeekEnd = workingHoursModelValidatorResult.WeekEnd;

//            store._WorkingHoursWeekendsBegin = workingHoursModelValidatorResult.WeekendsBegin;
//            store._WorkingHoursWeekendsEnd = workingHoursModelValidatorResult.WeekendsEnd;

//            store._WorkingHoursHolidaysBegin = workingHoursModelValidatorResult.HolidaysBegin;
//            store._WorkingHoursHolidaysEnd = workingHoursModelValidatorResult.HolidaysEnd;

//            var workingHoursValidator = new WorkingHoursValidator();

//            ValidationResult workingHoursValidatorResult = workingHoursValidator.Validate(store);

//            if (!workingHoursValidatorResult.IsValid)
//            {
//                foreach (ValidationFailure failure in workingHoursValidatorResult.Errors)
//                {
//                    response.ModelState.Add(failure.PropertyName, failure.ErrorMessage);
//                }
//            }

//            return response;
//        }
//    }
//}