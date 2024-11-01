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
        Task<dynamic> ReturnBook(int id);
        Task<dynamic> ApproveRequest(int userId);
        Task<dynamic> GetOrders();
        Task<dynamic> BlockFineOverdueUsers(int id);
        Task<dynamic> Unblock(int userId);
        Task<string> OrderReport();
        Task<dynamic> Dashboard();
    }
}
