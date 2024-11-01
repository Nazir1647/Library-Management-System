using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Tables.Table
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public UserType UserType { get; set; } = UserType.STUDENT;
        public AccountStatus AccountStatus { get; set; } = AccountStatus.UNAPROOVED;
    }

    public enum UserType
    {
        STUDENT, ADMIN
    }

    public enum AccountStatus
    {
        UNAPROOVED, ACTIVE, BLOCKED
    }
}
