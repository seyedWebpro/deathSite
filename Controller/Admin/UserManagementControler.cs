using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Middleware;
using api.Services;
using api.View.UserManagement;
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

        public UserManagementControler(apiContext context, ICalculatorService calculatorService)
        {
            _context = context;
            _calculatorService = calculatorService;
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

        // به‌روزرسانی کاربر
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            var user = await _context.users.FindAsync(id);
            if (user == null) return NotFound(new { StatusCode = 404, Message = "کاربر پیدا نشد." });

            user.firstName = userDto.FirstName;
            user.lastName = userDto.LastName;
            user.phoneNumber = userDto.PhoneNumber;
            user.role = userDto.Role;

            // هش کردن رمز عبور در صورت نیاز
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.password = _calculatorService.HashPassword(userDto.Password);
            }

            await _context.SaveChangesAsync();
            return Ok(new { StatusCode = 200, Message = "کاربر با موفقیت به‌روزرسانی شد.", Data = user });
        }

        // حذف کاربر
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null) return NotFound(new { StatusCode = 404, Message = "کاربر پیدا نشد." });

            // پیدا کردن توکن‌های مربوط به کاربر
            var userTokens = _context.userTokens.Where(ut => ut.UserId == user.Id).ToList();

            // منقضی کردن توکن‌ها
            foreach (var userToken in userTokens)
            {
                userToken.IsExpired = true; // یا می‌توانید توکن‌ها را حذف کنید
            }

            // حذف کاربر
            _context.users.Remove(user);
            await _context.SaveChangesAsync(); // ذخیره تغییرات در پایگاه داده

            return Ok(new { StatusCode = 204, Message = "کاربر با موفقیت حذف شد." });
        }

    }
}