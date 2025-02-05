using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Services;
using api.View.UserManagement;
using api.View.UserView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller.UserPanel
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPanelController : ControllerBase
    {
        private readonly apiContext _context;
        private readonly ICalculatorService _calculatorService;


        public UserPanelController(apiContext context, ICalculatorService calculatorService)
        {
            _context = context;
            _calculatorService = calculatorService;

        }

        [HttpPut("update/{id}")]
public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserPanelDto model)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(new { StatusCode = 400, Message = "خطا در اعتبارسنجی داده‌ها.", Errors = ModelState });
    }

    var user = await _context.users.FindAsync(id);
    if (user == null)
    {
        return NotFound(new { StatusCode = 404, Message = "کاربر یافت نشد." });
    }

    // به‌روزرسانی نام و نام خانوادگی
    user.firstName = string.IsNullOrWhiteSpace(model.FirstName) ? user.firstName : model.FirstName;
    user.lastName = string.IsNullOrWhiteSpace(model.LastName) ? user.lastName : model.LastName;

    // شماره تلفن را اگر تغییر کرده باشد به‌روز می‌کنیم
    user.phoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? user.phoneNumber : model.PhoneNumber;

    _context.users.Update(user);
    await _context.SaveChangesAsync();

    return Ok(new { StatusCode = 200, Message = "اطلاعات کاربر با موفقیت به‌روز شد." });
}


        [HttpPut("change-password/{id}")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { StatusCode = 400, Message = "خطا در اعتبارسنجی داده‌ها." });
            }

            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { StatusCode = 404, Message = "کاربر یافت نشد." });
            }

            // هش کردن رمز عبور فعلی
            var hashedCurrentPassword = _calculatorService.HashPassword(model.CurrentPassword);

            // بررسی صحت رمز عبور فعلی
            if (user.password != hashedCurrentPassword)
            {
                return BadRequest(new { StatusCode = 400, Message = "رمز عبور فعلی نادرست است." });
            }

            // هش کردن رمز عبور جدید و ذخیره آن
            user.password = _calculatorService.HashPassword(model.NewPassword);

            _context.users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "رمز عبور با موفقیت تغییر یافت." });
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.firstName,
                    u.lastName,
                    u.phoneNumber,
                    u.role,
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { StatusCode = 404, Message = "کاربر یافت نشد." });
            }

            return Ok(new { StatusCode = 200, Message = "کاربر دریافت شد.", Data = user });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.users
                .Select(u => new
                {
                    u.Id,
                    u.firstName,
                    u.lastName,
                    u.phoneNumber,
                    u.role,
                })
                .ToListAsync();

            return Ok(new { StatusCode = 200, Message = "لیست کاربران دریافت شد.", Data = users });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { StatusCode = 404, Message = "کاربر یافت نشد." });
            }

            _context.users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "حساب کاربری با موفقیت حذف شد." });
        }


    }
}