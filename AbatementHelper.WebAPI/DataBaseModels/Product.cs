using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.DataBaseModels
{
    public class ProductEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public virtual StoreEntity Store { get; set; }
        public string ProductName { get; set; }
    }
}