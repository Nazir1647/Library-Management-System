using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using LMS.Abstractions.Interfaces;
using LMS.Models.Common;
using LMS.Models.Models;
using LMS.Services.IServices;
using LMS.Tables.Table;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace LMS.Services.Services
{
    public class AuthService : IAuthService
    {
        private IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(IConfiguration config, IUnitOfWork unitOfWork)
        {
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public async Task<dynamic> Login(UserCred user)
        {
            try
            {
                var isUserNameExist = await _unitOfWork.GenericRepository<User>().GetAllByIdAsync(x => x.Email.Equals(user.Email)).ConfigureAwait(false);
                var userData = isUserNameExist.FirstOrDefault(x => x.Password.Equals(user.Password));
                if (userData == null)
                {
                    return new ApiResponse<string>(Status.FAIL, AlertMessages.LoginFailed, AlertMessages.LoginFailed);
                }else if (userData.AccountStatus == AccountStatus.UNAPROOVED)
                {
                    return new ApiResponse<string>(Status.FAIL, AlertMessages.LoginFailed, "unapproved");
                }else if (userData.AccountStatus == AccountStatus.BLOCKED)
                {
                    return new ApiResponse<string>(Status.FAIL, AlertMessages.LoginFailed, "blocked");
                }
                var token = GenerateToken(userData);
                return new ApiResponse<string>(Status.OK, AlertMessages.Success, token);

            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(Status.ERROR, AlertMessages.Error, ex.ToString());
            }
        }

        public async Task<dynamic> Registration(UserModel user)
        {
            // NAzir
            var bookCategoryData = await _unitOfWork.GenericRepository<User>().GetAllAsync().ConfigureAwait(false);
            if (bookCategoryData.Any(x => x.Email == user.Email))
            {
                return new ApiResponse<string>(Status.FAIL, string.Format(AlertMessages.AlreadyExist, user.Email), "");
            }
            var data = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                AccountStatus = AccountStatus.UNAPROOVED,
                CreatedOn = DateTime.Now,
                MobileNumber = user.MobileNumber,
                Password = user.Password,
                UserType = UserType.NONE,
            };
            await _unitOfWork.GenericRepository<User>().SaveAsync(data);
            await _unitOfWork.CompleteAsync();
            return new ApiResponse<User>(Status.OK, AlertMessages.SaveSuccessful, data);
        }

            private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>{
                new Claim("id", user.Id.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("email", user.Email),
                new Claim("mobileNumber", user.MobileNumber),
                new Claim("userType", user.UserType.ToString()),
                new Claim("accountStatus", user.AccountStatus.ToString()),
                new Claim("createdOn", user.CreatedOn.ToString())
            };

            var token = new JwtSecurityToken(
                _config["JwtSettings:Issuer"],
                _config["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
