using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.CommonModels.WebApiModels
{
    public class WebApiStoreParameters : WebApiStore
    {
        public WebApiStoreParameters()
        {
            Managers = new List<WebApiManager>();
            ManagersUserNames = new List<string>();
        }

        public WebApiStoreParameters(string id, string storeName, string workingHoursWeek, string workingHoursWeekends, string workingHoursHolidays, string country, string city, string postalCode, string street, string storeAdminId, bool approved, bool deleted, List<WebApiManager> managers) : this()
        {
            Id = id;
            StoreName = storeName;
            WorkingHoursWeek = workingHoursWeek;
            WorkingHoursWeekends = workingHoursWeekends;
            WorkingHoursHolidays = workingHoursHolidays;
            Country = country;
            City = city;
            PostalCode = postalCode;
            Street = street;
            StoreAdminId = storeAdminId;
            Approved = approved;
            Deleted = deleted;
            Managers = managers;

            foreach (var manager in Managers)
            {
                ManagersUserNames.Add(manager.UserName);
            }
        }
    }
}
