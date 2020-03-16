using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCatalog.Common.Models.Extended
{
    public class AuthenticatedUserResult : Result
    {
        public string Access_Token { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
