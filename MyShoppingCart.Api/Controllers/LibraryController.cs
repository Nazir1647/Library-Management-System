using LMS.Models.Models;
using LMS.Services.IServices;
using LMS.Tables.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using System.Text.Json;
using System;
using LMS.Models.Common;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _iLibraryService;

        public LibraryController(ILibraryService iLibraryService)
        {
            _iLibraryService = iLibraryService;
        }

        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var res = await _iLibraryService.Dashboard();
            return Ok(res);
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

        [HttpGet("ReturnBook")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var res = await _iLibraryService.ReturnBook(id);
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

        [HttpGet("OrderReport")]
        public async Task<IActionResult> OrderReport()
        {
            var res = await _iLibraryService.OrderReport();

            var document = new PdfDocument();
            string HtmlContent = "<h1>Hello World!</h1>";
            PdfGenerator.AddPdfPages(document, res, PageSize.A4);
            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            string Filename = "Invoice_" + 1 + ".pdf";
            return File(response, "application/pdf", Filename);
        }


    }
}
