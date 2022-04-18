using Data.Entities;
using Dtos;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace IdentityProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISendMailProvider _sendMail;
        private readonly IConfiguration _configuration;
        public UsersController(UserManager<ApplicationUser> userManager, ISendMailProvider sendMail, IConfiguration configuration)
        {
            _userManager = userManager;
            _sendMail = sendMail;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var useExist = await _userManager.FindByNameAsync(model.UserName);

            if (useExist != null)
            {
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.BadRequest,
                    Message = "User already exist"
                });
            }

            var user = new ApplicationUser() { Email = model.Email, UserName = model.UserName };

            await _userManager.AddToRoleAsync(user, "Owner");

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await SendEmailConfirm(new EmailDto { Email = model.Email });
            }

            return Ok(new BaseModelResponseDto
            {
                Code = Infrastructure.Enums.ApiResponseCode.Success,
                Message = "Register Successfully"
            });
        }

        [AllowAnonymous]
        [HttpPost("SendConfirmEmail")]
        public async Task<IActionResult> SendEmailConfirm(EmailDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) {
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.BadRequest,
                    Message = "User is not exist"
                });
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            UriBuilder uri = new UriBuilder(_configuration["EmaiConfirmUrl"]);
            var query = HttpUtility.ParseQueryString(uri.Query);
            query["email"] = model.Email;
            query["token"] = token;
            uri.Query = query.ToString();
            var urlString = uri.ToString();

            await _sendMail.SendTemplateEmailAsync(model.Email, "ConfirmEmail", "Confirm Registration", new { Name = user.UserName, Token = token, ConfirmationLink = urlString });

            return Ok(); 
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmRegistration(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) {
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.BadRequest,
                    Message = "User is not exist"
                });
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.BadRequest,
                    Message = "can't confirm your email"
                });
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserDto model)
        {
                if (!ModelState.IsValid) { 
                return Ok(new BaseModelResponseDto
                {
                    Code = ApiResponseCode.InvalidModel,
                    Message = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)))
                });
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return Ok(new BaseModelResponseDto
                {
                    Code = ApiResponseCode.BadRequest,
                    Message = "tài khoản chưa được tạo bằng Email này"
                });

            var isValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isValid)
                return Ok(new BaseModelResponseDto
                {
                    Code = ApiResponseCode.BadRequest,
                    Message = "mật khẩu sai "
                });

            if (!user.EmailConfirmed)
                return Ok(new BaseModelResponseDto
                {
                    Code = ApiResponseCode.BadRequest,
                    Message = "Chưa xác thực Email cho tài khoản"
                });

            var token = await CreateToken(user);
            return Ok(new BaseModelResponseDto<string>
            {
                Code = ApiResponseCode.Success,
                Message = "Login successfully",
                Data = token
            });


        }

        private async Task<string> CreateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var config = _configuration.GetSection("Jwt").Get<JwtSettingModel>();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecretKey));
            var token = new JwtSecurityToken(
                issuer: config.Issuer,
                audience: config.Issuer,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(config.ExpiryMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
