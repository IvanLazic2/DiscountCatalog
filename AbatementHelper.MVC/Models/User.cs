using AbatementHelper.MVC.Models.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.MVC.Models
{
    public partial class User
    {
        public string Id { get; set; }
        //public int UserId { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        //public Address Address { get; set; }
        public string Password { get; set; }
        //public bool EmailConfirmed { get; set; }
        //public Guid ConfirmationCode { get; set; }
        public string Role { get; set; }
    }
}
