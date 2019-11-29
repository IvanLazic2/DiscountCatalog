using AbatementHelper.CommonModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Models
{
    public class WebApiUserInfo : UserInfo
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        //public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Country { get; set; }
        //public string City { get; set; }
        //public string PostalCode { get; set; }
        //public string Street { get; set; }
    }

    public class WebApiStoreInfo : StoreInfo
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        //public string StoreId { get; set; }
        public ApplicationUser Store { get; set; }
        //public string WorkingHoursWeek { get; set; }
        //public string WorkingHoursWeekends { get; set; }
        //public string WorkingHoursHolidays { get; set; }
        //public string MasterStoreID { get; set; }
    }

    public class WebApiStoreAdminInfo : StoreAdminInfo
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        //public string StoreAdminId { get; set; }
        public ApplicationUser StoreAdmin { get; set; }
    }

    public class WebApiAdminInfo : AdminInfo
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        //public string AdminId { get; set; }
        public ApplicationUser Admin { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
    }
}