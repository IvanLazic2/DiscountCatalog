using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.CommonModels.Models
{
    public class AuthenticatedUser
    {
        public string Id { get; set; }
        public string Access_Token { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
