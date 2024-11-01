using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Models.Models
{
    public class DashboardModel
    {
        public int orders { get; set; }
        public int books { get; set; }
        public int users { get; set; }
        public BookOrder bookOrder { get; set; }
    }

    public class BookOrder
    {
        public int Total { get; set; }
        public int Pending { get; set; }
        public int Completed { get; set; }
    }

}
