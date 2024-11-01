using LMS.Abstractions.Interfaces;
using LMS.Models.Common;
using LMS.Models.Models;
using LMS.Services.IServices;
using LMS.Tables.Table;
using System.Text;

namespace LMS.Services.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LibraryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<dynamic> ApproveRequest(int userId)
        {
            var userData = await _unitOfWork.GenericRepository<User>().GetByIdAsync(x => x.Id == userId).ConfigureAwait(false);
            if (userData == null)
            {
                return new ApiResponse<string>(Status.FAIL, AlertMessages.NotFound, "");
            }

            if (userData.AccountStatus != AccountStatus.BLOCKED)
            {
                userData.AccountStatus = AccountStatus.ACTIVE;
                await _unitOfWork.GenericRepository<User>().UpdateAsync(userData);
                await _unitOfWork.CompleteAsync();
                return new ApiResponse<string>(Status.OK, "approved", "");
            }
            return new ApiResponse<string>(Status.OK, "not approved", "");
        }

        public Task<dynamic> BlockFineOverdueUsers(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<dynamic> GetOrders()
        {
            var orderData = await _unitOfWork.GenericRepository<Order>().GetAllAsync().ConfigureAwait(false);
            var bookData = await _unitOfWork.GenericRepository<Book>().GetAllAsync().ConfigureAwait(false);
            var userData = await _unitOfWork.GenericRepository<User>().GetAllAsync().ConfigureAwait(false);

            var data = from od in orderData
                       join
                       bd in bookData on od.BookId equals bd.Id
                       join
                       u in userData on od.UserId equals u.Id
                       select new OrderModel
                       {
                           UserId = od.UserId,
                           BookId = od.BookId,
                           OrderDate = od.OrderDate,
                           Returned = od.Returned,
                           ReturnDate = od.ReturnDate,
                           FinePaid = od.FinePaid,
                           Id = od.Id,
                           Username = u.FirstName + " " + u.LastName,
                           Bookname = bd.Title,
                       };
            return new ApiResponse<IEnumerable<OrderModel>>(Status.OK, AlertMessages.Success, data);
        }


        public async Task<dynamic> GetOrdersOFUser(int userId)
        {
            var orderData = await _unitOfWork.GenericRepository<Order>().GetAllAsync().ConfigureAwait(false);
            var bookData = await _unitOfWork.GenericRepository<Book>().GetAllAsync().ConfigureAwait(false);
            var userData = await _unitOfWork.GenericRepository<User>().GetAllAsync().ConfigureAwait(false);

            var data = from od in orderData
                       join
                       bd in bookData on od.BookId equals bd.Id
                       join
                       u in userData on od.UserId equals u.Id
                       where od.UserId == userId
                       select new OrderModel
                       {
                           UserId = userId,
                           BookId = od.BookId,
                           OrderDate = od.OrderDate,
                           Returned = od.Returned,
                           ReturnDate = od.ReturnDate,
                           FinePaid = od.FinePaid,
                           Id = od.Id,
                           Username = u.FirstName + " " + u.LastName,
                           Bookname = bd.Title,
                       };

            return new ApiResponse<IEnumerable<OrderModel>>(Status.OK, AlertMessages.Success, data);
        }

        public async Task<dynamic> OrderBook(int userId, int bookId)
        {
            var UserOrderData = await _unitOfWork.GenericRepository<Order>().GetAllAsync().ConfigureAwait(false);

            if(UserOrderData.Any(x=>x.UserId == userId && x.BookId == bookId && !x.Returned))
            {
                return new ApiResponse<string>(Status.FAIL, "You already have this book borrowed. Please select a different book.", "");
            }

            var canOrder = UserOrderData.Count(x => x.UserId == userId && !x.Returned) < 3;
            if (canOrder)
            {
                var order = new Order
                {
                    UserId = userId,
                    BookId = bookId,
                    OrderDate = DateTime.Now,
                    ReturnDate = null,
                    Returned = false,
                    FinePaid = 0
                };

                await _unitOfWork.GenericRepository<Order>().SaveAsync(order);
                await _unitOfWork.CompleteAsync();
                return new ApiResponse<string>(Status.OK, AlertMessages.SaveSuccessful, "");
            }
            else
            {
                return new ApiResponse<string>(Status.FAIL, "You have reached the limit of 3 books. Please return a book before borrowing more.", "");
            }
        }

        public async Task<dynamic> ReturnBook(int id)
        {
            var orderData = await _unitOfWork.GenericRepository<Order>().GetByIdAsync(x => x.Id == id).ConfigureAwait(false);
            if (orderData != null)
            {
                orderData.Returned = true;
                orderData.ReturnDate = DateTime.Now;
                orderData.FinePaid = 0;

                var BookData = await _unitOfWork.GenericRepository<Book>().GetByIdAsync(x => x.Id == orderData.BookId).ConfigureAwait(false);
                BookData.Ordered = false;

                await _unitOfWork.GenericRepository<Order>().UpdateAsync(orderData);
                await _unitOfWork.GenericRepository<Book>().UpdateAsync(BookData);
                await _unitOfWork.CompleteAsync();
                return new ApiResponse<string>(Status.OK, AlertMessages.BookReturned, "");
            }
            return new ApiResponse<string>(Status.FAIL, "book not returned", "");
        }

        public async Task<dynamic> Unblock(int userId)
        {
            var userData = await _unitOfWork.GenericRepository<User>().GetByIdAsync(x => x.Id == userId).ConfigureAwait(false);

            if (userData != null)
            {
                userData.AccountStatus = AccountStatus.ACTIVE;
                await _unitOfWork.GenericRepository<User>().UpdateAsync(userData);
                await _unitOfWork.CompleteAsync();
                return new ApiResponse<string>(Status.OK, "unblocked", "");
            }
            return new ApiResponse<string>(Status.FAIL, "not unblocked", "");
        }

        public async Task<string> OrderReport()
        {
            var orderData = await _unitOfWork.GenericRepository<Order>().GetAllAsync().ConfigureAwait(false);
            var bookData = await _unitOfWork.GenericRepository<Book>().GetAllAsync().ConfigureAwait(false);
            var userData = await _unitOfWork.GenericRepository<User>().GetAllAsync().ConfigureAwait(false);

            var data = from od in orderData
                       join
                       bd in bookData on od.BookId equals bd.Id
                       join
                       u in userData on od.UserId equals u.Id
                       select new OrderModel
                       {
                           UserId = od.UserId,
                           BookId = od.BookId,
                           OrderDate = od.OrderDate,
                           Returned = od.Returned,
                           ReturnDate = od.ReturnDate,
                           FinePaid = od.FinePaid,
                           Id = od.Id,
                           Username = u.FirstName + " " + u.LastName,
                           Bookname = bd.Title,
                           Email = u.Email
                           
                       };
            return htmlContent(data.ToList());
        }

        private string htmlContent(List<OrderModel> Order)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "Report.html");
          
            if (!File.Exists(templatePath))          
                throw new FileNotFoundException("The HTML template file was not found.", templatePath);          

            string htmlTemplate = File.ReadAllText(templatePath);

            StringBuilder tbody = new StringBuilder();
            int srNo = 1; 

            foreach (var item in Order)
            {
                tbody.Append("<tr>")
                     .Append($"<td>{srNo}</td>")
                     .Append($"<td>{item.Username}</td>")
                     .Append($"<td>{item.Bookname}</td>")
                     .Append($"<td>{item.Email}</td>")
                     .Append($"<td>{item.OrderDate:yyyy-MM-dd}</td>")
                     .Append($"<td>{item.ReturnDate?.ToString("yyyy-MM-dd") ?? ""}</td>")
                     .Append($"<td>{(item.Returned ? "Completed" : "Pending")}</td>")
                     .Append("</tr>");

                srNo++;
            }

            htmlTemplate = htmlTemplate.Replace("##TBody", tbody.ToString());
            return htmlTemplate;
        }

        public async Task<dynamic> Dashboard()
        {
            var orderData = await _unitOfWork.GenericRepository<Order>().GetAllAsync().ConfigureAwait(false);
            var userData = await _unitOfWork.GenericRepository<User>().GetAllAsync().ConfigureAwait(false);
            var bookData = await _unitOfWork.GenericRepository<Book>().GetAllAsync().ConfigureAwait(false);

            int totalOrder = orderData.Count();
            int Pending = orderData.Count(x => x.Returned == false);
            int completed = orderData.Count(x => x.Returned == true);

            DashboardModel dashboard = new DashboardModel()
            {
                orders = totalOrder,
                books = bookData.Count(),
                users = userData.Count(),
                bookOrder = new BookOrder
                {
                    Total = totalOrder,
                    Pending = Pending,
                    Completed = completed
                }
            };
            return new ApiResponse<DashboardModel>(Status.OK, AlertMessages.Success, dashboard);
        }

    }
}
