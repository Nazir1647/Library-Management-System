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

namespace LMS.Services.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<dynamic> GetBookById(int id)
        {
            var bookData = await _unitOfWork.GenericRepository<Book>().GetByIdAsync(x => x.Id == id).ConfigureAwait(false);

            var data = new BookModel
            {
                Id = bookData.Id,
                Title = bookData.Title,
                Author = bookData.Author,
                Price = bookData.Price,
                Ordered = bookData.Ordered,
                BookCategoryId = bookData.BookCategoryId,
            };

            return new ApiResponse<BookModel>(Status.OK, AlertMessages.Success, data);
        }

        public async Task<dynamic> AddBook(BookModel book)
        {
            var bookCategoryData = await _unitOfWork.GenericRepository<Book>().GetAllAsync().ConfigureAwait(false);
            if (bookCategoryData.Any(x => x.Title == book.Title))
            {
                return new ApiResponse<string>(Status.FAIL, string.Format(AlertMessages.AlreadyExist, book.Title), "");
            }
            var data = new Book
            {
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                Ordered = false,
                BookCategoryId = book.BookCategoryId
            };
            await _unitOfWork.GenericRepository<Book>().SaveAsync(data);
            await _unitOfWork.CompleteAsync();
            return new ApiResponse<Book>(Status.OK, AlertMessages.SaveSuccessful, data);
        }

        public async Task<dynamic> DeleteBook(int id)
        {
            var bookData = await _unitOfWork.GenericRepository<Book>().GetByIdAsync(x => x.Id == id).ConfigureAwait(false);
            if (bookData == null)
            {
                return new ApiResponse<string>(Status.FAIL, AlertMessages.NotFound, "");
            }
            _unitOfWork.GenericRepository<Book>().Delete(bookData);
            await _unitOfWork.CompleteAsync();
            return new ApiResponse<string>(Status.OK, AlertMessages.DeleteSuccessful, "");
        }

        public async Task<dynamic> GetBooks()
        {
            var bookData = await _unitOfWork.GenericRepository<Book>().GetAllAsync().ConfigureAwait(false);
            var categoryData = await _unitOfWork.GenericRepository<BookCategory>().GetAllAsync().ConfigureAwait(false);

            var data = from bd in bookData
                       join
                       cd in categoryData on bd.BookCategoryId equals cd.Id
                       select new BookModel
                       {
                           Id = bd.Id,
                           Title = bd.Title,
                           Author = bd.Author,
                           Price = bd.Price,
                           Ordered = bd.Ordered,
                           BookCategoryId = bd.BookCategoryId,
                           BookCategoryName = cd.Category,
                           SubCategoryName = cd.SubCategory
                       };

            return new ApiResponse<IEnumerable<BookModel>>(Status.OK, AlertMessages.Success, data);
        }

        public async Task<dynamic> UpdateBook(BookModel book)
        {
            var bookData = await _unitOfWork.GenericRepository<Book>().GetByIdAsync(x => x.Id == book.Id).ConfigureAwait(false);
            if (bookData.Title == book.Title && bookData.Id != book.Id)
            {
                return new ApiResponse<string>(Status.FAIL, string.Format(AlertMessages.AlreadyExist, book.Title), "");
            }

            bookData.Title = book.Title;
            bookData.Author = book.Author;
            bookData.Price = book.Price;
            bookData.Ordered = false;
            bookData.BookCategoryId = book.BookCategoryId;
           
            await _unitOfWork.GenericRepository<Book>().UpdateAsync(bookData);
            await _unitOfWork.CompleteAsync();
            return new ApiResponse<Book>(Status.OK, AlertMessages.SaveSuccessful, bookData);
        }

    }
}
