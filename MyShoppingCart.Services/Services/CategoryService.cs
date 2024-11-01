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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public async Task<dynamic> GetCategoryById(int id)
        {
            var categoryData = await _unitOfWork.GenericRepository<BookCategory>().GetByIdAsync(x => x.Id == id).ConfigureAwait(false);

            var data = new BookCategoryModel
            {
                Id = categoryData.Id,
                Category = categoryData.Category,
                SubCategory = categoryData.SubCategory,
            };

            return new ApiResponse<BookCategoryModel>(Status.OK, AlertMessages.Success, data);
        }

        public async Task<dynamic> DeleteCategory(int id)
        {
            var categoryData = await _unitOfWork.GenericRepository<BookCategory>().GetByIdAsync(x => x.Id == id).ConfigureAwait(false);
            if (categoryData == null)
            {
                return new ApiResponse<string>(Status.FAIL, AlertMessages.NotFound, "");
            }
            _unitOfWork.GenericRepository<BookCategory>().Delete(categoryData);
            await _unitOfWork.CompleteAsync();
            return new ApiResponse<string>(Status.OK, AlertMessages.DeleteSuccessful, "");
        }

        public async Task<dynamic> UpdateCategory(BookCategoryModel categoryModel)
        {
            var categoryData = await _unitOfWork.GenericRepository<BookCategory>().GetByIdAsync(x => x.Id == categoryModel.Id).ConfigureAwait(false);
            if (categoryData.SubCategory == categoryModel.SubCategory && categoryData.Id != categoryModel.Id)
            {
                return new ApiResponse<string>(Status.FAIL, string.Format(AlertMessages.AlreadyExist, categoryModel.SubCategory), "");
            }

            categoryData.Category = categoryModel.Category;
            categoryData.SubCategory = categoryModel.SubCategory;

            await _unitOfWork.GenericRepository<BookCategory>().UpdateAsync(categoryData);
            await _unitOfWork.CompleteAsync();
            return new ApiResponse<BookCategory>(Status.OK, AlertMessages.SaveSuccessful, categoryData);
        }
    }
}
