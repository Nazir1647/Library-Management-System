using LMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.IServices
{
    public interface IBookService
    {
        Task<dynamic> GetBooks();
        Task<dynamic> GetBookById(int id);
        Task<dynamic> AddBook(BookModel book);
        Task<dynamic> DeleteBook(int id);
        Task<dynamic> UpdateBook(BookModel book);
    }
}
