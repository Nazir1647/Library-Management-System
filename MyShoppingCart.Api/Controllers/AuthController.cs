using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LMS.Models.Models;
using LMS.Services.IServices;
using LMS.Tables.Table;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserCred user)
        {
           var res = await _authService.Login(user);
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] UserModel user)
        {
            var res = await _authService.Registration(user);
            return Ok(res);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _authService.GetAll();
            return Ok(res);
        }

        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var res = await _authService.GetById(email);
            return Ok(res);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ForgotPasswordModel forgotPasswordModel)
        {
            var res = await _authService.ChangePassword(forgotPasswordModel);
            return Ok(res);
        }

    }
}
