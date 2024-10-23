using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Models.Models
{
    public  class OrderModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Returned { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int FinePaid { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Bookname { get; set; } = string.Empty;
    }
}
