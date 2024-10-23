using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Models.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public float Price { get; set; }
        public bool? Ordered { get; set; }
        public int BookCategoryId { get; set; }
        public string? BookCategoryName { get; set; } = string.Empty;
        public string? SubCategoryName { get; set; } = string.Empty;
    }
}
