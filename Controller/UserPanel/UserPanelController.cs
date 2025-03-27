using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
 private readonly ILogger<UserPanelController> _logger;

        public UserPanelController(apiContext context, ICalculatorService calculatorService ,  ILogger<UserPanelController> logger)
        {
            _context = context;
            _calculatorService = calculatorService;
            _logger = logger;
        }

//   [HttpPut("update")]
// public async Task<IActionResult> UpdateUser([FromBody] UpdateUserPanelDto model)
// {
//     var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

//     if (string.IsNullOrEmpty(token))
//     {
//         return Unauthorized(new { StatusCode = 401, Message = "توکن ارسال نشده است. لطفاً وارد حساب خود شوید." });
//     }

//     try
//     {
//         var handler = new JwtSecurityTokenHandler();
//         var jwtToken = handler.ReadJwtToken(token);

//         var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
//         if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
//         {
//             return Unauthorized(new { StatusCode = 401, Message = "توکن نامعتبر است. لطفاً دوباره وارد شوید." });
//         }

//         var user = await _context.users.FindAsync(userId);
//         if (user == null)
//         {
//             return NotFound(new { StatusCode = 404, Message = "کاربر یافت نشد." });
//         }

//  // به‌روزرسانی اطلاعات کاربر
//     user.firstName = string.IsNullOrWhiteSpace(model.FirstName) ? user.firstName : model.FirstName;
//     user.lastName = string.IsNullOrWhiteSpace(model.LastName) ? user.lastName : model.LastName;
//     user.phoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? user.phoneNumber : model.PhoneNumber;

//     _context.users.Update(user);
//     await _context.SaveChangesAsync();

//         return Ok(new { StatusCode = 200, Message = "اطلاعات کاربر با موفقیت به‌روزرسانی شد." });
//     }
//     catch (SecurityTokenException)
//     {
//         return Unauthorized(new { StatusCode = 401, Message = "توکن منقضی شده یا نامعتبر است. لطفاً دوباره وارد شوید." });
//     }
//     catch
//     {
//         return StatusCode(500, new { StatusCode = 500, Message = "خطای داخلی سرور. لطفاً بعداً دوباره تلاش کنید." });
//     }
// }

// انقضای توکن قبلی و ساخت توکن جدید

[HttpPut("update")]
public async Task<IActionResult> UpdateUser([FromBody] UpdateUserPanelDto model)
{
    // دریافت توکن از هدر درخواست
    var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

    if (string.IsNullOrEmpty(token))
    {
        return Unauthorized(new { StatusCode = 401, Message = "توکن ارسال نشده است. لطفاً وارد حساب خود شوید." });
    }

    try
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized(new { StatusCode = 401, Message = "توکن نامعتبر است. لطفاً دوباره وارد شوید." });
        }

        var user = await _context.users.FindAsync(userId);
        if (user == null)
        {
            return NotFound(new { StatusCode = 404, Message = "کاربر یافت نشد." });
        }

        // منقضی کردن توکن جاری در دیتابیس
        var storedToken = await _context.userTokens.FirstOrDefaultAsync(ut => ut.Token == token && !ut.IsExpired);
        if (storedToken != null)
        {
            storedToken.IsExpired = true;
            _context.userTokens.Update(storedToken);
            await _context.SaveChangesAsync();
        }

        // به‌روزرسانی اطلاعات کاربر
        user.firstName = string.IsNullOrWhiteSpace(model.FirstName) ? user.firstName : model.FirstName;
        user.lastName = string.IsNullOrWhiteSpace(model.LastName) ? user.lastName : model.LastName;
        user.phoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? user.phoneNumber : model.PhoneNumber;

        _context.users.Update(user);
        await _context.SaveChangesAsync();

        // صدور توکن جدید با اطلاعات به‌روز
        var newToken = _calculatorService.GenerateJwtToken(user);

        return Ok(new 
        { 
            StatusCode = 200, 
            Message = "اطلاعات کاربر با موفقیت به‌روزرسانی شد.",
            Token = newToken 
        });
    }
    catch (SecurityTokenException)
    {
        return Unauthorized(new { StatusCode = 401, Message = "توکن منقضی شده یا نامعتبر است. لطفاً دوباره وارد شوید." });
    }
    catch
    {
        return StatusCode(500, new { StatusCode = 500, Message = "خطای داخلی سرور. لطفاً بعداً دوباره تلاش کنید." });
    }
}



//        [HttpPut("change-password")]
// public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
// {
//     if (!ModelState.IsValid)
//     {
//         return BadRequest(new { StatusCode = 400, Message = "خطا در اعتبارسنجی داده‌ها." });
//     }

//     var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

//     if (string.IsNullOrEmpty(token))
//     {
//         return Unauthorized(new { StatusCode = 401, Message = "توکن ارسال نشده است. لطفاً وارد حساب خود شوید." });
//     }

//     try
//     {
//         var handler = new JwtSecurityTokenHandler();
//         var jwtToken = handler.ReadJwtToken(token);

//         var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
//         if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
//         {
//             return Unauthorized(new { StatusCode = 401, Message = "توکن نامعتبر است. لطفاً دوباره وارد شوید." });
//         }

//         var user = await _context.users.FindAsync(userId);
//         if (user == null)
//         {
//             return NotFound(new { StatusCode = 404, Message = "کاربر یافت نشد." });
//         }

//         // بررسی رمز عبور فعلی
//         if (user.password != _calculatorService.HashPassword(model.CurrentPassword))
//         {
//             return BadRequest(new { StatusCode = 400, Message = "رمز عبور فعلی نادرست است." });
//         }

//         // بررسی پیچیدگی رمز عبور جدید (مثلاً حداقل ۸ کاراکتر)
//         if (model.NewPassword.Length < 6)
//         {
//             return BadRequest(new { StatusCode = 400, Message = "رمز عبور جدید باید حداقل ۸ کاراکتر باشد." });
//         }

//         // به‌روزرسانی رمز عبور
//         user.password = _calculatorService.HashPassword(model.NewPassword);
//         _context.users.Update(user);
//         await _context.SaveChangesAsync();

//         return Ok(new { StatusCode = 200, Message = "رمز عبور با موفقیت تغییر یافت." });
//     }
//     catch (SecurityTokenException)
//     {
//         return Unauthorized(new { StatusCode = 401, Message = "توکن منقضی شده یا نامعتبر است. لطفاً دوباره وارد شوید." });
//     }
//     catch
//     {
//         return StatusCode(500, new { StatusCode = 500, Message = "خطای داخلی سرور. لطفاً بعداً دوباره تلاش کنید." });
//     }
// }

//  انقضای توکن قبلی و جایگزینی 

[HttpPut("change-password")]
public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(new { StatusCode = 400, Message = "خطا در اعتبارسنجی داده‌ها." });
    }

    var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

    if (string.IsNullOrEmpty(token))
    {
        return Unauthorized(new { StatusCode = 401, Message = "توکن ارسال نشده است. لطفاً وارد حساب خود شوید." });
    }

    try
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized(new { StatusCode = 401, Message = "توکن نامعتبر است. لطفاً دوباره وارد شوید." });
        }

        var user = await _context.users.FindAsync(userId);
        if (user == null)
        {
            return NotFound(new { StatusCode = 404, Message = "کاربر یافت نشد." });
        }

        // بررسی رمز عبور فعلی
        if (user.password != _calculatorService.HashPassword(model.CurrentPassword))
        {
            return BadRequest(new { StatusCode = 400, Message = "رمز عبور فعلی نادرست است." });
        }

        // بررسی پیچیدگی رمز عبور جدید
        if (model.NewPassword.Length < 6)
        {
            return BadRequest(new { StatusCode = 400, Message = "رمز عبور جدید باید حداقل ۶ کاراکتر باشد." });
        }

        // به‌روزرسانی رمز عبور
        user.password = _calculatorService.HashPassword(model.NewPassword);
        _context.users.Update(user);
        await _context.SaveChangesAsync();

        // منقضی کردن توکن قبلی
        var storedToken = await _context.userTokens.FirstOrDefaultAsync(ut => ut.Token == token && !ut.IsExpired);
        if (storedToken != null)
        {
            storedToken.IsExpired = true;
            _context.userTokens.Update(storedToken);
            await _context.SaveChangesAsync();
        }

        // صدور توکن جدید
        var newToken = _calculatorService.GenerateJwtToken(user);

        return Ok(new { StatusCode = 200, Message = "رمز عبور با موفقیت تغییر یافت.", Token = newToken });
    }
    catch (SecurityTokenException)
    {
        return Unauthorized(new { StatusCode = 401, Message = "توکن منقضی شده یا نامعتبر است. لطفاً دوباره وارد شوید." });
    }
    catch
    {
        return StatusCode(500, new { StatusCode = 500, Message = "خطای داخلی سرور. لطفاً بعداً دوباره تلاش کنید." });
    }
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
        

[HttpDelete("delete-account")]
public async Task<IActionResult> DeleteAccount()
{
    var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
    if (string.IsNullOrEmpty(token))
    {
        _logger.LogWarning("Authorization token is missing.");
        return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
    }

    try
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (userIdClaim == null)
        {
            _logger.LogWarning("Token does not contain UserId claim.");
            return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
        }

        var userId = int.Parse(userIdClaim);

        // پیدا کردن کاربر در دیتابیس
        var user = await _context.users.FindAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID {UserId}.", userId);
            return NotFound(new { StatusCode = 404, Message = "کاربر پیدا نشد." });
        }

        // حذف رکوردهای وابسته در جدول Deceaseds
        var deceasedRecords = await _context.Deceaseds.Where(d => d.UserId == user.Id).ToListAsync();
        if (deceasedRecords.Any())
        {
            _logger.LogInformation("Deleting {Count} related records from Deceaseds for user {UserId}.", deceasedRecords.Count, userId);
            _context.Deceaseds.RemoveRange(deceasedRecords);
        }

        _logger.LogInformation("User {UserId} found. Invalidating tokens.", userId);

        // پیدا کردن و منقضی کردن توکن‌های کاربر
        var userTokens = await _context.userTokens.Where(ut => ut.UserId == user.Id).ToListAsync();
        foreach (var userToken in userTokens)
        {
            userToken.IsExpired = true;
            _logger.LogDebug("Token {TokenId} invalidated for user {UserId}.", userToken.Id, userId);
        }

        // حذف کاربر
        _context.users.Remove(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User {UserId} deleted successfully.", userId);
        return Ok(new { StatusCode = 204, Message = "حساب کاربری شما با موفقیت حذف شد." });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error occurred while deleting user account.");
        return StatusCode(500, new { StatusCode = 500, Message = "خطای داخلی سرور.", Error = ex.Message });
    }
}



    }
}