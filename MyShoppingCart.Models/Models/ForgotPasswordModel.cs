using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Models.Models
{
    public class ForgotPasswordModel
    {
        public int userId { get; set; }
        public int Otp { get; set; }
        public string? Password { get; set; }
    }
}
