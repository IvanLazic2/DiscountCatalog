using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.MVC.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
        public string Password { get; set; }
        public bool EmailConfirmed { get; set; }
        public System.Guid ConfirmationCode { get; set; }
        public bool IsStore { get; set; }
    }
}
