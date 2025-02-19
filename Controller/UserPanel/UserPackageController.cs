using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
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

        // دریافت پکیج های فعال کاربر 

        [HttpGet("GetActivePackages/{userId}")]
        public async Task<IActionResult> GetActivePackages(int userId)
        {
            var activePackages = await _context.UserPackages
                .Where(up => up.UserId == userId && up.IsActive)
                .Include(up => up.Package)
                .Select(up => new UserPackageDto
                {
                    PackageId = up.Package.Id,
                    PackageName = up.Package.Name,
                    Price = up.Package.Price,
                    PurchaseDate = up.PurchaseDate,
                    ExpiryDate = up.ExpiryDate,
                    Status = "فعال",
                    UsedImageCount = up.UsedImageCount,
                    UsedVideoCount = up.UsedVideoCount,
                    UsedNotificationCount = up.UsedNotificationCount,
                    UsedAudioFileCount = up.UsedAudioFileCount
                })
                .ToListAsync();

            if (!activePackages.Any())
                return NotFound(new { StatusCode = 404, Message = "هیچ پکیج فعالی برای این کاربر یافت نشد." });

            return Ok(new { StatusCode = 200, Data = activePackages });
        }


        // دریافت تمام پکیج های خریداری شده کاربر 

        [HttpGet("GetAllPackages/{userId}")]
        public async Task<IActionResult> GetAllPackages(int userId)
        {
            var userPackages = await _context.UserPackages
                .Where(up => up.UserId == userId)
                .Include(up => up.Package)
                .OrderByDescending(up => up.PurchaseDate)
                .Select(up => new UserPackageDto
                {
                    PackageId = up.Package.Id,
                    PackageName = up.Package.Name,
                    Price = up.Package.Price,
                    PurchaseDate = up.PurchaseDate,
                    ExpiryDate = up.ExpiryDate,
                    Status = up.IsActive ? "فعال" : "منقضی‌شده"
                })
                .ToListAsync();

            if (!userPackages.Any())
                return NotFound(new { StatusCode = 404, Message = "هیچ پکیجی برای این کاربر یافت نشد." });

            return Ok(new { StatusCode = 200, Data = userPackages });
        }

        // دریافت جزییات یک پکیج خاص 

        [HttpGet("GetPackageDetails/{userId}/{packageId}")]
        public async Task<IActionResult> GetPackageDetails(int userId, int packageId)
        {
            var package = await _context.UserPackages
                .Where(up => up.UserId == userId && up.PackageId == packageId)
                .Include(up => up.Package)
                .Select(up => new UserPackageDto
                {
                    PackageId = up.Package.Id,
                    PackageName = up.Package.Name,
                    Price = up.Package.Price,
                    PurchaseDate = up.PurchaseDate,
                    ExpiryDate = up.ExpiryDate,
                    Status = up.IsActive ? "فعال" : "منقضی‌شده",
                    UsedImageCount = up.UsedImageCount,
                    UsedVideoCount = up.UsedVideoCount,
                    UsedNotificationCount = up.UsedNotificationCount,
                    UsedAudioFileCount = up.UsedAudioFileCount
                })
                .FirstOrDefaultAsync();

            if (package == null)
                return NotFound(new { StatusCode = 404, Message = "پکیج موردنظر برای این کاربر یافت نشد." });

            return Ok(new { StatusCode = 200, Data = package });
        }


        // دریافت وضعیت پکیج فعال کاربر

        [HttpGet("HasActivePackage/{userId}")]
        public async Task<IActionResult> HasActivePackage(int userId)
        {
            bool hasActive = await _context.UserPackages.AnyAsync(up => up.UserId == userId && up.IsActive);
            return Ok(new { StatusCode = 200, HasActivePackage = hasActive });
        }

    }
}