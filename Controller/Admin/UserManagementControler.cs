using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Middleware;
using api.Services;
using api.View.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserManagementControler : ControllerBase
    {
        private readonly apiContext _context;
        private readonly ICalculatorService _calculatorService;
        private readonly ILogger<UserManagementControler> _logger;

        public UserManagementControler(apiContext context, ICalculatorService calculatorService, ILogger<UserManagementControler> logger)
        {
            _context = context;
            _calculatorService = calculatorService;
            _logger = logger;
        }

        // دریافت تمام کاربران
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.users.ToListAsync();
            return Ok(new { StatusCode = 200, Message = "کاربران با موفقیت دریافت شدند.", Data = users });
        }

        // دریافت کاربر بر اساس ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null) return NotFound(new { StatusCode = 404, Message = "کاربر پیدا نشد." });

            return Ok(new { StatusCode = 200, Message = "کاربر با موفقیت دریافت شد.", Data = user });
        }

        // اضافه کردن تغییر توکن

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { StatusCode = 404, Message = "کاربر پیدا نشد." });
            }

            // به‌روزرسانی اطلاعات کاربر
            user.firstName = userDto.FirstName;
            user.lastName = userDto.LastName;
            user.phoneNumber = userDto.PhoneNumber;
            user.role = userDto.Role;

            // هش کردن رمز عبور در صورت نیاز
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.password = _calculatorService.HashPassword(userDto.Password);
            }

            // حذف تمام توکن‌های قبلی کاربر
            var oldTokens = await _context.userTokens.Where(ut => ut.UserId == id && !ut.IsExpired).ToListAsync();
            foreach (var token in oldTokens)
            {
                token.IsExpired = true;
            }
            _context.userTokens.UpdateRange(oldTokens);

            // ذخیره تغییرات در دیتابیس
            await _context.SaveChangesAsync();

            // صدور توکن جدید برای کاربر
            var newToken = _calculatorService.GenerateJwtToken(user);

            return Ok(new
            {
                StatusCode = 200,
                Message = "کاربر با موفقیت به‌روزرسانی شد و توکن جدید صادر شد.",
                Data = user,
                Token = newToken
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation("شروع فرآیند حذف کاربر با شناسه {UserId}.", id);
            try
            {
                var user = await _context.users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("حذف کاربر: کاربری با شناسه {UserId} پیدا نشد.", id);
                    return NotFound(new { StatusCode = 404, Message = "کاربر پیدا نشد." });
                }

                // حذف رکوردهای وابسته در جدول Deceaseds
                var deceasedRecords = await _context.Deceaseds.Where(d => d.UserId == user.Id).ToListAsync();
                if (deceasedRecords.Any())
                {
                    _logger.LogInformation("حذف {Count} رکورد وابسته از جدول Deceaseds برای کاربر {UserId}.", deceasedRecords.Count, user.Id);
                    _context.Deceaseds.RemoveRange(deceasedRecords);
                }

                _logger.LogInformation("کاربر {UserId} پیدا شد. شروع به منقضی کردن توکن‌ها.", id);
                // پیدا کردن توکن‌های مربوط به کاربر
                var userTokens = await _context.userTokens.Where(ut => ut.UserId == user.Id).ToListAsync();

                // منقضی کردن توکن‌ها
                foreach (var userToken in userTokens)
                {
                    userToken.IsExpired = true;
                    _logger.LogDebug("توکن با شناسه {TokenId} برای کاربر {UserId} منقضی شد.", userToken.Id, id);
                }

                // حذف کاربر
                _context.users.Remove(user);
                _logger.LogInformation("در حال ذخیره تغییرات حذف کاربر {UserId} در پایگاه داده.", id);
                await _context.SaveChangesAsync();
                _logger.LogInformation("کاربر با شناسه {UserId} با موفقیت حذف شد.", id);
                return Ok(new { StatusCode = 204, Message = "کاربر با موفقیت حذف شد." });
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException != null)
                {
                    _logger.LogError(dbEx, "خطا در حذف کاربر {UserId} به دلیل مشکل در به‌روزرسانی پایگاه داده. جزئیات: {InnerMessage}", id, dbEx.InnerException.Message);
                }
                else
                {
                    _logger.LogError(dbEx, "خطا در حذف کاربر {UserId} به دلیل مشکل در به‌روزرسانی پایگاه داده.", id);
                }
                return StatusCode(500, new { StatusCode = 500, Message = "خطای داخلی سرور.", Error = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در هنگام حذف کاربر با شناسه {UserId}.", id);
                return StatusCode(500, new { StatusCode = 500, Message = "خطای داخلی سرور.", Error = ex.Message });
            }
        }

        // اضافه کردن تغییر توکن

        [HttpPut("updateRole/{userId}")]
        public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] string newRole)
        {
            // جستجوی کاربر با استفاده از ID
            var user = await _context.users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new { StatusCode = 404, Message = "کاربر مورد نظر یافت نشد." });
            }

            // تغییر نقش کاربر
            user.role = newRole;

            try
            {
                // حذف تمام توکن‌های قبلی کاربر
                var oldTokens = await _context.userTokens.Where(ut => ut.UserId == userId && !ut.IsExpired).ToListAsync();
                foreach (var token in oldTokens)
                {
                    token.IsExpired = true;
                }
                _context.userTokens.UpdateRange(oldTokens);

                // ذخیره تغییرات نقش کاربر
                await _context.SaveChangesAsync();

                // تولید توکن جدید برای کاربر
                var newToken = _calculatorService.GenerateJwtToken(user);

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "نقش کاربر با موفقیت تغییر یافت و توکن جدید صادر شد.",
                    UserId = user.Id,
                    NewRole = user.role,
                    Token = newToken
                });
            }
            catch (Exception ex)
            {
                // در صورت بروز خطا
                return StatusCode(500, new { StatusCode = 500, Message = "خطا در تغییر نقش کاربر.", Error = ex.Message });
            }
        }

    }
}