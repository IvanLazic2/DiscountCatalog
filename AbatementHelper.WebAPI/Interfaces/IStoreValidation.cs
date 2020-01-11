using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.WebAPI.Interfaces
{
    interface IStoreValidation
    {
        ModelStateResponse Validate(WebApiStore store);
    }
}
