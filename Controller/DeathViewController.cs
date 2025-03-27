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
    public class DeathViewController : ControllerBase
    {
        private readonly ILogger<DeathViewController> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly apiContext _context;
        private readonly IConfiguration _configuration;


        public DeathViewController(ILogger<DeathViewController> logger, IFileUploadService fileUploadService, apiContext context, IConfiguration configuration)
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
            _context = context;
            _configuration = configuration;
        }



        [HttpPost("add-view/{deceasedId}")]
public async Task<IActionResult> AddView(int deceasedId)
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
            _logger.LogWarning("خطا در پردازش توکن: {Message}", ex.Message);
            userId = null;
        }
    }

    var deceased = await _context.Deceaseds.FindAsync(deceasedId);
    if (deceased == null)
    {
        return NotFound(new { StatusCode = 404, Message = "متوفی موردنظر یافت نشد." });
    }

    // بررسی برای کاربران لاگین شده
    if (userId.HasValue)
    {
        var existingView = await _context.DeathViewCounts
            .FirstOrDefaultAsync(dv => dv.UserId == userId.Value && dv.DeceasedId == deceasedId);

        if (existingView != null)
        {
            return Ok(new { StatusCode = 200, Message = "شما قبلاً برای این متوفی ویو ثبت کرده‌اید." });
        }

        _context.DeathViewCounts.Add(new DeathViewCount
        {
            UserId = userId.Value,
            DeceasedId = deceasedId,
            ViewDate = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return Ok(new { StatusCode = 200, Message = "ویو با موفقیت ثبت شد." });
    }

    // بررسی برای کاربران مهمان با IP
    var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
    if (string.IsNullOrEmpty(userIp))
    {
        _logger.LogWarning("امکان دریافت IP کاربر وجود ندارد.");
        return StatusCode(400, new { StatusCode = 400, Message = "خطا در دریافت IP کاربر." });
    }

    var existingIpView = await _context.DeathViewCounts
        .FirstOrDefaultAsync(dv => dv.IPAddress == userIp && dv.DeceasedId == deceasedId);

    if (existingIpView != null)
    {
        return Ok(new { StatusCode = 200, Message = "شما قبلاً برای این متوفی ویو ثبت کرده‌اید." });
    }

    _context.DeathViewCounts.Add(new DeathViewCount
    {
        UserId = null,
        DeceasedId = deceasedId,
        ViewDate = DateTime.UtcNow,
        IPAddress = userIp
    });

    await _context.SaveChangesAsync();
    return Ok(new { StatusCode = 200, Message = "ویو با موفقیت ثبت شد." });
}

        [HttpGet("get-view-count/{deceasedId}")]
        public async Task<IActionResult> GetViewCount(int deceasedId)
        {
            var deceased = await _context.Deceaseds.FindAsync(deceasedId);
            if (deceased == null)
            {
                return NotFound(new { StatusCode = 404, Message = "متوفی موردنظر یافت نشد." });
            }

            var viewCount = await _context.DeathViewCounts
                .Where(dv => dv.DeceasedId == deceasedId)
                .CountAsync();

            return Ok(new { StatusCode = 200, Message = "تعداد ویوها با موفقیت بازیابی شد.", Data = new { DeceasedId = deceasedId, ViewCount = viewCount } });
        }


    }
}