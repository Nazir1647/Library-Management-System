using LMS.Abstractions.Interfaces;
using LMS.Models.Common;
using LMS.Models.Models;
using LMS.Services.IServices;
using LMS.Tables.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LMS.Services.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LibraryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<dynamic> AddCategory(BookCategoryModel bookCategory)
        {
            var bookCategoryData = await _unitOfWork.GenericRepository<BookCategory>().GetAllAsync().ConfigureAwait(false);
            if (bookCategoryData.Any(x => x.SubCategory == bookCategory.SubCategory))
            {
                return new ApiResponse<string>(Status.FAIL, string.Format(AlertMessages.AlreadyExist, bookCategory.SubCategory), "");
            }
            var data = new BookCategory
            {
                Category = bookCategory.Category,
                SubCategory = bookCategory.SubCategory
            };
            await _unitOfWork.GenericRepository<BookCategory>().SaveAsync(data);
            await _unitOfWork.CompleteAsync();
            return new ApiResponse<BookCategory>(Status.OK, AlertMessages.SaveSuccessful, data);
        }

        public async Task<dynamic> ApproveRequest(int userId)
        {
            var userData = await _unitOfWork.GenericRepository<User>().GetByIdAsync(x=>x.Id == userId).ConfigureAwait(false);
            if (userData == null)
            {
                return new ApiResponse<string>(Status.FAIL, AlertMessages.NotFound, "");
            }

            if(userData.AccountStatus != AccountStatus.BLOCKED)
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



        public async Task<dynamic> GetCategories()
        {
            var bookCategoryData = await _unitOfWork.GenericRepository<BookCategory>().GetAllAsync().ConfigureAwait(false);
            var data = bookCategoryData.Select(x => new BookCategoryModel
            {
                Id = x.Id,
                Category = x.Category,
                SubCategory = x.SubCategory
            });
            return new ApiResponse<IEnumerable<BookCategoryModel>>(Status.OK, AlertMessages.Success, data);
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
            var canOrder = UserOrderData.Count(x => !x.Returned) < 3;
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
                return new ApiResponse<string>(Status.OK, "cannot order", "");
            }
        }

        public async Task<dynamic> ReturnBook(int userId, int bookId, int fine)
        {
            var orderData = await _unitOfWork.GenericRepository<Order>().GetAllByIdAsync(x => x.Id == userId).ConfigureAwait(false);
            var order = orderData.FirstOrDefault(x => x.BookId == bookId);
            if (order != null)
            {
                order.Returned = true;
                order.ReturnDate = DateTime.Now;
                order.FinePaid = fine;

                var BookData = await _unitOfWork.GenericRepository<Book>().GetByIdAsync(x => x.Id == bookId).ConfigureAwait(false);
                BookData.Ordered = false;

                await _unitOfWork.GenericRepository<Order>().UpdateAsync(order);
                await _unitOfWork.GenericRepository<Book>().UpdateAsync(BookData);
                await _unitOfWork.CompleteAsync();
                return new ApiResponse<string>(Status.FAIL, "book returned", "");
            }
            return new ApiResponse<string>(Status.FAIL, "book not returned", "");
        }

        public async Task<dynamic> Unblock(int userId)
        {
            var userData = await _unitOfWork.GenericRepository<User>().GetByIdAsync(x=>x.Id == userId).ConfigureAwait(false);

            if(userData != null)
            {
                userData.AccountStatus = AccountStatus.ACTIVE;
                await _unitOfWork.GenericRepository<User>().UpdateAsync(userData);
                await _unitOfWork.CompleteAsync();
                return new ApiResponse<string>(Status.OK, "unblocked", "");
            }
            return new ApiResponse<string>(Status.FAIL, "not unblocked", "");
        }
    }
}
