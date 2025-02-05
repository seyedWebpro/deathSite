using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using api.Context;
using api.Middleware;
using api.Model;
using api.Services;
using api.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class userController : ControllerBase
    {
        private readonly apiContext _context;
        private readonly ISmsService _smsService;
        private readonly ICalculatorService _calculatorService;
        private readonly ICacheService _cacheService;
        public userController(apiContext context, ISmsService smsService, ICalculatorService calculatorService, IConfiguration configuration, ICacheService cacheService)
        {
            _context = context;
            _smsService = smsService;
            _calculatorService = calculatorService;
            _cacheService = cacheService;
        }

        private void InvalidateUserTokens(int userId)
        {
            var userTokens = _context.userTokens.Where(ut => ut.UserId == userId).ToList();
            foreach (var userToken in userTokens)
            {
                userToken.IsExpired = true; // منقضی کردن توکن
            }
            _context.SaveChanges(); // ذخیره تغییرات
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CheckPhoneNumber([FromBody] CheckPhoneNumberVew request)
        {
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            var existingUser = await _context.users.FirstOrDefaultAsync(u => u.phoneNumber == request.PhoneNumber);

            if (existingUser != null)
            {
                // منقضی کردن توکن‌های قبلی
                InvalidateUserTokens(existingUser.Id);

                // تولید توکن JWT برای کاربر موجود
                var token = _calculatorService.GenerateJwtToken(existingUser);

                // ارسال پیامک برای کاربر موجود
                await _smsService.SendAuthenticationSmsAsync(request.PhoneNumber);

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "شما با این شماره تلفن قبلاً ثبت نام کرده‌اید!",
                    Token = token
                });
            }
            else
            {
                // اگر کاربر ثبت‌نام نکرده باشد، هیچ پیامکی ارسال نمی‌شود
                return Ok(new { StatusCode = 200, Message = "این شماره تلفن ثبت نشده است." });
            }
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordView request)
        {
            // بررسی اعتبارسنجی مدل
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }
            var existingUser = await _context.users.FirstOrDefaultAsync(u => u.phoneNumber == request.PhoneNumber);

            if (existingUser != null)
            {
                // شماره تلفن در دیتابیس وجود دارد
                await _smsService.SendPasswordResetSmsAsync(request.PhoneNumber);
                return Ok(new { StatusCode = 200, Message = "کد تایید بازنشانی رمز عبور به شماره تلفن شما ارسال شد." });
            }
            else
            {
                // شماره تلفن در دیتابیس وجود ندارد
                return Ok(new { StatusCode = 200, Message = "این شماره تلفن در سیستم ثبت نشده است." });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeView request)
        {
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            var storedCode = _cacheService.GetVerificationCode(request.PhoneNumber);

            if (storedCode == null)
            {
                return BadRequest(new { StatusCode = 400, Message = "کدی برای این شماره تلفن ارسال نشده است." });
            }

            if (storedCode == request.Code)
            {
                var existingUser = await _context.users.FirstOrDefaultAsync(u => u.phoneNumber == request.PhoneNumber);

                if (existingUser != null)
                {
                    // منقضی کردن توکن‌های قبلی
                    InvalidateUserTokens(existingUser.Id);

                    // تولید توکن JWT برای کاربر
                    var token = _calculatorService.GenerateJwtToken(existingUser);
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "کد تأیید صحیح است.",
                        Token = token
                    });
                }
                else
                {
                    return BadRequest(new { StatusCode = 400, Message = "کاربر با این شماره تلفن یافت نشد." });
                }
            }
            else
            {
                return BadRequest(new { StatusCode = 400, Message = "کد تأیید نادرست است." });
            }
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterUser([FromBody] registerView model)
        {
            // بررسی اعتبارسنجی مدل
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            var existingUser = await _context.users.FirstOrDefaultAsync(u => u.phoneNumber == model.phoneNumber);

            if (existingUser != null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "این شماره تلفن قبلاً ثبت‌نام شده است."
                });
            }

            // هش کردن رمز عبور
            var hashedPassword = _calculatorService.HashPassword(model.password);
            if (string.IsNullOrEmpty(hashedPassword))
            {
                throw new Exception("خطا در هش کردن رمز عبور.");
            }

            // ایجاد کاربر جدید
            var newUser = new User
            {
                firstName = model.firstName,
                lastName = model.lastName,
                password = hashedPassword,
                phoneNumber = model.phoneNumber,
                role = "User"
            };

            await _context.users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            // تولید توکن
            var token = _calculatorService.GenerateJwtToken(newUser);
            if (string.IsNullOrEmpty(token))
            {
                return StatusCode(500, new { Message = "خطا در تولید توکن." });
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "ثبت‌نام با موفقیت انجام شد.",
                Token = token
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordView model)
        {
            // بررسی اعتبارسنجی مدل
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            var existingUser = await _context.users.FirstOrDefaultAsync(u => u.phoneNumber == model.PhoneNumber);

            if (existingUser == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "این شماره تلفن در سیستم ثبت نشده است."
                });
            }

            // هش کردن رمز عبور جدید
            var hashedPassword = _calculatorService.HashPassword(model.NewPassword);
            if (string.IsNullOrEmpty(hashedPassword))
            {
                return StatusCode(500, new { Message = "خطا در هش کردن رمز عبور." });
            }

            // به‌روزرسانی رمز عبور کاربر
            existingUser.password = hashedPassword;

            // ذخیره تغییرات در پایگاه داده
            _context.users.Update(existingUser);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "رمز عبور با موفقیت تغییر یافت."
            });
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> LoginUser([FromBody] LoginView model)
        {
            // بررسی اعتبارسنجی مدل
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            var existingUser = await _context.users.FirstOrDefaultAsync(u => u.phoneNumber == model.phoneNumber);

            if (existingUser == null)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "کاربری با این شماره تلفن پیدا نشد."
                });
            }

            // هش کردن رمز عبور وارد شده
            var hashedPassword = _calculatorService.HashPassword(model.password);

            // بررسی همخوانی رمز عبور
            if (existingUser.password != hashedPassword)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "رمز عبور نادرست است."
                });
            }

            // تولید توکن با استفاده از AuthService
            var token = _calculatorService.GenerateJwtToken(existingUser);

            return Ok(new
            {
                StatusCode = 200,
                Message = "ورود با موفقیت انجام شد.",
                Token = token
            });
        }

        [HttpGet("[action]")]
        public IActionResult GetUserInfo()
        {
            // دریافت توکن از هدر Authorization
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { StatusCode = 400, Message = "توکن معتبر نیست." });
            }

            // تجزیه توکن و استخراج اطلاعات کاربر
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // استخراج اطلاعات کاربر از توکن
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var firstName = jwtToken.Claims.FirstOrDefault(c => c.Type == "firstName")?.Value;
            var lastName = jwtToken.Claims.FirstOrDefault(c => c.Type == "lastName")?.Value;
            var phoneNumber = jwtToken.Claims.FirstOrDefault(c => c.Type == "phoneNumber")?.Value;
            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            var packageId = jwtToken.Claims.FirstOrDefault(c => c.Type == "packageId")?.Value;

            if (firstName == null || lastName == null)
            {
                return BadRequest(new { StatusCode = 400, Message = "اطلاعات کاربر در توکن موجود نیست." });
            }

            // بررسی وضعیت توکن در پایگاه داده
            var userToken = _context.userTokens.FirstOrDefault(ut => ut.Token == token && ut.IsExpired == false);
            if (userToken == null)
            {
                return Unauthorized(new { StatusCode = 401, Message = "توکن منقضی شده است." });
            }

            // بازگشت اطلاعات کاربر
            return Ok(new
            {
                StatusCode = 200,
                Message = "اطلاعات کاربر با موفقیت دریافت شد.",
                UserInfo = new
                {
                    UserId = userToken.UserId, // افزودن UserId به پاسخ
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    Role = role,
                    PackageId = packageId
                }
            });
        }



    }
}