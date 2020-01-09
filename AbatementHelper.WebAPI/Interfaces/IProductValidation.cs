using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.WebAPI
{
    interface IProductValidation
    {
        ModelStateResponse Validate(ProductEntity product);
    }
}
