using LMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.IServices
{
    public interface ICategoryService
    {
        Task<dynamic> AddCategory(BookCategoryModel bookCategory);
        Task<dynamic> GetCategories();
        Task<dynamic> GetCategoryById(int id);
        Task<dynamic> DeleteCategory(int id);
        Task<dynamic> UpdateCategory(BookCategoryModel book);
    }
}
