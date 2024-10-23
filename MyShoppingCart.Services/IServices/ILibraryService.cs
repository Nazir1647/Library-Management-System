using LMS.Models.Models;
using LMS.Tables.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.IServices
{
    public interface ILibraryService
    {
        Task<dynamic> OrderBook(int userId, int bookId);
        Task<dynamic> GetOrdersOFUser(int userId);
        Task<dynamic> AddCategory(BookCategoryModel bookCategory);
        Task<dynamic> GetCategories();
        Task<dynamic> ReturnBook(int userId, int bookId, int fine);
        Task<dynamic> ApproveRequest(int userId);
        Task<dynamic> GetOrders();
        Task<dynamic> BlockFineOverdueUsers(int id);
        Task<dynamic> Unblock(int userId);
    }
}
