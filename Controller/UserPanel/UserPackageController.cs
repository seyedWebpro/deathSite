using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using deathSite.View.Dead;
using deathSite.View.Packages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace deathSite.Controller.UserPanel
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPackageController : ControllerBase
    {
        private readonly apiContext _context;

        public UserPackageController(apiContext context)
        {
            _context = context;
        }

        // دریافت پکیج‌های فعال کاربر (همراه با اطلاعات متوفی)
      [HttpGet("GetActivePackages/{userId}")]
public async Task<IActionResult> GetActivePackages(int userId)
{
    var activePackages = await _context.DeceasedPackages
        .Where(dp => dp.Deceased.UserId == userId && dp.IsActive)
        .Include(dp => dp.Package)
        .Select(dp => new UserPackageDto
        {
            PackageId = dp.Package.Id,
            PackageName = dp.Package.Name,
            Price = dp.Package.Price,
            PurchaseDate = dp.ActivationDate,
            ExpiryDate = dp.ExpirationDate,
            Status = "فعال",
            UsedImageCount = 0,  // مقداردهی اولیه (نیاز به منبع داده مشخص دارد)
            UsedVideoCount = 0,
            UsedNotificationCount = 0,
            UsedAudioFileCount = 0,
            Deceaseds = new List<DeceasedDetailsDto>
            {
                new DeceasedDetailsDto
                {
                    Id = dp.Deceased.Id,
                    FullName = dp.Deceased.FullName
                }
            }
        })
        .ToListAsync();

    if (!activePackages.Any())
        return NotFound(new { StatusCode = 404, Message = "هیچ پکیج فعالی برای این کاربر یافت نشد." });

    return Ok(new { StatusCode = 200, Data = activePackages });
}


        // دریافت تمام پکیج‌های خریداری شده کاربر (همراه با اطلاعات متوفی)
        [HttpGet("GetAllPackages/{userId}")]
public async Task<IActionResult> GetAllPackages(int userId)
{
    var userPackages = await _context.DeceasedPackages
        .Where(dp => dp.Deceased.UserId == userId)
        .Include(dp => dp.Package)
        .OrderByDescending(dp => dp.ActivationDate)
        .Select(dp => new UserPackageDto
        {
            PackageId = dp.Package.Id,
            PackageName = dp.Package.Name,
            Price = dp.Package.Price,
            PurchaseDate = dp.ActivationDate,
            ExpiryDate = dp.ExpirationDate,
            Status = dp.IsActive ? "فعال" : "منقضی‌شده",
            UsedImageCount = 0,
            UsedVideoCount = 0,
            UsedNotificationCount = 0,
            UsedAudioFileCount = 0,
            Deceaseds = new List<DeceasedDetailsDto>
            {
                new DeceasedDetailsDto
                {
                    Id = dp.Deceased.Id,
                    FullName = dp.Deceased.FullName
                }
            }
        })
        .ToListAsync();

    if (!userPackages.Any())
        return NotFound(new { StatusCode = 404, Message = "هیچ پکیجی برای این کاربر یافت نشد." });

    return Ok(new { StatusCode = 200, Data = userPackages });
}


        // دریافت جزییات یک پکیج خاص (همراه با اطلاعات متوفی)
        [HttpGet("GetPackageDetails/{userId}/{packageId}")]
public async Task<IActionResult> GetPackageDetails(int userId, int packageId)
{
    var package = await _context.DeceasedPackages
        .Where(dp => dp.Deceased.UserId == userId && dp.PackageId == packageId)
        .Include(dp => dp.Package)
        .Select(dp => new UserPackageDto
        {
            PackageId = dp.Package.Id,
            PackageName = dp.Package.Name,
            Price = dp.Package.Price,
            PurchaseDate = dp.ActivationDate,
            ExpiryDate = dp.ExpirationDate,
            Status = dp.IsActive ? "فعال" : "منقضی‌شده",
            UsedImageCount = 0,
            UsedVideoCount = 0,
            UsedNotificationCount = 0,
            UsedAudioFileCount = 0,
            Deceaseds = new List<DeceasedDetailsDto>
            {
                new DeceasedDetailsDto
                {
                    Id = dp.Deceased.Id,
                    FullName = dp.Deceased.FullName
                }
            }
        })
        .FirstOrDefaultAsync();

    if (package == null)
        return NotFound(new { StatusCode = 404, Message = "پکیج موردنظر برای این کاربر یافت نشد." });

    return Ok(new { StatusCode = 200, Data = package });
}


        // دریافت جزییات یک پکیج خاص برای یک متوفی مشخص (در صورت نیاز)
       [HttpGet("HasActivePackage/{userId}")]
public async Task<IActionResult> HasActivePackage(int userId)
{
    bool hasActive = await _context.DeceasedPackages.AnyAsync(dp => dp.Deceased.UserId == userId && dp.IsActive);
    return Ok(new { StatusCode = 200, HasActivePackage = hasActive });
}

    }
}
