using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.CommonModels.Models
{
    public class UserInfo
    {
        //[Key]
        //public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        //public ApplicationUser User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
    }

    public class StoreInfo
    {
        //[Key]
        //public Guid Id { get; set; } = Guid.NewGuid();
        public string StoreId { get; set; }
        //public ApplicationUser Store { get; set; }
        public string WorkingHoursWeek { get; set; }
        public string WorkingHoursWeekends { get; set; }
        public string WorkingHoursHolidays { get; set; }
        public string MasterStoreID { get; set; }
    }

    public class StoreAdminInfo
    {
        //[Key]
        //public Guid Id { get; set; } = Guid.NewGuid();
        public string StoreAdminId { get; set; }
        //public ApplicationUser StoreAdmin { get; set; }
    }

    public class AdminInfo
    {
        //[Key]
        //public Guid Id { get; set; } = Guid.NewGuid();
        public string AdminId { get; set; }
        //public ApplicationUser Admin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}