using LMS.Models.Models;
using LMS.Services.IServices;
using LMS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _iCategoyService;

        public CategoryController(ICategoryService iCategoyService)
        {
            _iCategoyService = iCategoyService;
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(BookCategoryModel bookCategory)
        {
            var res = await _iCategoyService.AddCategory(bookCategory);
            return Ok(res);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _iCategoyService.GetCategories();
            return Ok(res);
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _iCategoyService.DeleteCategory(id);
            return Ok(res);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(BookCategoryModel categoryModel)
        {
            var res = await _iCategoyService.UpdateCategory(categoryModel);
            return Ok(res);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _iCategoyService.GetCategoryById(id);
            return Ok(res);
        }
    }
}
