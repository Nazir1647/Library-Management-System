using LMS.Models.Models;
using LMS.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _iBookService;

        public BookController(IBookService iBookService)
        {
            _iBookService = iBookService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _iBookService.GetBooks();
            return Ok(res);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _iBookService.GetBookById(id);
            return Ok(res);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(BookModel book)
        {
            var res = await _iBookService.AddBook(book);
            return Ok(res);
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _iBookService.DeleteBook(id);
            return Ok(res);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(BookModel book)
        {
            var res = await _iBookService.UpdateBook(book);
            return Ok(res);
        }

    }
}
