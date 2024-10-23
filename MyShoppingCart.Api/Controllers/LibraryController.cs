using LMS.Models.Models;
using LMS.Services.IServices;
using LMS.Tables.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _iLibraryService;

        public LibraryController(ILibraryService iLibraryService)
        {
            _iLibraryService = iLibraryService;
        }

        [HttpGet("OrderBook")]
        public async Task<IActionResult> OrderBook(int userId, int bookId)
        {
            var res = await _iLibraryService.OrderBook(userId, bookId);
            return Ok(res);
        }

        [HttpGet("GetOrdersOFUser")]
        public async Task<IActionResult> GetOrdersOFUser(int userId)
        {
            var res = await _iLibraryService.GetOrdersOFUser(userId);
            return Ok(res);
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(BookCategoryModel bookCategory)
        {
            var res = await _iLibraryService.AddCategory(bookCategory);
            return Ok(res);
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var res = await _iLibraryService.GetCategories();
            return Ok(res);
        }

        [HttpGet("ReturnBook")]
        public async Task<IActionResult> ReturnBook(int userId, int bookId, int fine)
        {
            var res = await _iLibraryService.ReturnBook(userId, bookId, fine);
            return Ok(res);
        }
             
        [HttpGet("ApproveRequest")]
        public async Task<IActionResult> ApproveRequest(int userId)
        {
            var res = await _iLibraryService.ApproveRequest(userId);
            return Ok(res);
        }

        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var res = await _iLibraryService.GetOrders();
            return Ok(res);
        }

        [HttpGet("BlockFineOverdueUsers")]
        public async Task<IActionResult> BlockFineOverdueUsers(int userId)
        {
            var res = await _iLibraryService.BlockFineOverdueUsers(userId);
            return Ok(res);
        }

        [HttpGet("Unblock")]
        public async Task<IActionResult> Unblock(int userId)
        {
            var res = await _iLibraryService.Unblock(userId);
            return Ok(res);
        }



    }
}
