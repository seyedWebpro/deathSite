using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Services;
using deathSite.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace deathSite.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShahidViewController : ControllerBase
    {

        private readonly ILogger<ShahidViewController> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly apiContext _context;
        private readonly IConfiguration _configuration;


        public ShahidViewController(ILogger<ShahidViewController> logger, IFileUploadService fileUploadService, apiContext context, IConfiguration configuration)
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
            _context = context;
            _configuration = configuration;
        }



        [HttpPost("add-view/{shahidId}")]
        public async Task<IActionResult> AddView(int shahidId)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            int? userId = null; // برای کاربران مهمان

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);

                    var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                    if (userIdClaim != null)
                    {
                        userId = int.Parse(userIdClaim);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Invalid token provided.");
                }
            }

            var shahid = await _context.shahids.FindAsync(shahidId);
            if (shahid == null)
            {
                return NotFound(new { StatusCode = 404, Message = "شهید موردنظر یافت نشد." });
            }

            // اگر کاربر لاگین کرده باشد
            if (userId.HasValue)
            {
                var existingView = await _context.ShahidViewCounts
                    .FirstOrDefaultAsync(sv => sv.UserId == userId.Value && sv.ShahidId == shahidId);

                if (existingView != null)
                {
                    return Ok(new { StatusCode = 200, Message = "شما قبلاً برای این شهید ویو ثبت کرده‌اید." });
                }

                // ثبت ویو جدید
                _context.ShahidViewCounts.Add(new ShahidViewCount
                {
                    UserId = userId.Value,
                    ShahidId = shahidId,
                    ViewDate = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }
            else
            {
                // اگر کاربر مهمان باشد، از IP استفاده می‌کنیم
                var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                var existingView = await _context.ShahidViewCounts
                    .FirstOrDefaultAsync(sv => sv.IPAddress == userIp && sv.ShahidId == shahidId);

                if (existingView != null)
                {
                    return Ok(new { StatusCode = 200, Message = "شما قبلاً برای این شهید ویو ثبت کرده‌اید." });
                }

                // ثبت ویو جدید برای کاربر مهمان
                _context.ShahidViewCounts.Add(new ShahidViewCount
                {
                    UserId = null, // برای کاربران مهمان
                    ShahidId = shahidId,
                    ViewDate = DateTime.UtcNow,
                    IPAddress = userIp
                });
                await _context.SaveChangesAsync();
            }

            return Ok(new { StatusCode = 200, Message = "ویو با موفقیت ثبت شد." });
        }


        [HttpGet("get-view-count/{shahidId}")]
        public async Task<IActionResult> GetViewCount(int shahidId)
        {
            // یافتن شهید بر اساس ID
            var shahid = await _context.shahids.FindAsync(shahidId);
            if (shahid == null)
            {
                return NotFound(new { StatusCode = 404, Message = "شهید موردنظر یافت نشد." });
            }

            // شمارش تعداد ویوها برای شهید خاص
            var viewCount = await _context.ShahidViewCounts
                .Where(sv => sv.ShahidId == shahidId)
                .CountAsync();

            return Ok(new { StatusCode = 200, Message = "تعداد ویوها با موفقیت بازیابی شد.", Data = new { ShahidId = shahidId, ViewCount = viewCount } });
        }

    }
}