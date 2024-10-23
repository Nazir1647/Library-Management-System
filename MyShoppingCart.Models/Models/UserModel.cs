using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Models.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? MobileNumber { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; }
        public string? UserType { get; set; }
        public string? AccountStatus { get; set; }
    }
}
