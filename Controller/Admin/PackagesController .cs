using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Middleware;
using api.Model;
using api.View.Packages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class packagesController : ControllerBase
    {
        private readonly apiContext _context;

        public packagesController(apiContext context)
        {
            _context = context;
        }

        // متد برای اضافه کردن پکیج
        [HttpPost]
        public async Task<IActionResult> CreatePackage([FromBody] PackageCreateView packageDto)
        {
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            var package = new Package
            {
                Name = packageDto.Name,
                Duration = packageDto.Duration,
                Price = packageDto.Price,
                RenewalFee = packageDto.RenewalFee,
                ValidityPeriod = packageDto.ValidityPeriod,
                ImageCount = packageDto.ImageCount,
                VideoCount = packageDto.VideoCount,
                NotificationCount = packageDto.NotificationCount,
                AudioFileLimit = packageDto.AudioFileLimit,
                BarcodeEnabled = packageDto.BarcodeEnabled,
                DisplayEnabled = packageDto.DisplayEnabled,
                TemplateSelectionEnabled = packageDto.TemplateSelectionEnabled,
                CondolenceMessageEnabled = packageDto.CondolenceMessageEnabled,
                VisitorContentSubmissionEnabled = packageDto.VisitorContentSubmissionEnabled,
                LocationAndNavigationEnabled = packageDto.LocationAndNavigationEnabled,
                SharingEnabled = packageDto.SharingEnabled,
                File360DegreeEnabled = packageDto.File360DegreeEnabled,
                UpdateCapabilityEnabled = packageDto.UpdateCapabilityEnabled,

                // استفاده از ویژگی رایگان بودن
                IsFreePackage = packageDto.IsFreePackage
            };

            _context.packages.Add(package);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 201, Message = "پکیج با موفقیت ایجاد شد.", Data = package });
        }


        // متد برای ویرایش پکیج
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(int id, [FromBody] PackageUpdateView packageDto)
        {
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            var package = await _context.packages.FindAsync(id);
            if (package == null)
            {
                return NotFound(new { StatusCode = 404, Message = "پکیج پیدا نشد." });
            }

            package.Name = packageDto.Name;
            package.Duration = packageDto.Duration;
            package.Price = packageDto.Price;
            package.RenewalFee = packageDto.RenewalFee;
            package.ValidityPeriod = packageDto.ValidityPeriod;
            package.ImageCount = packageDto.ImageCount;
            package.VideoCount = packageDto.VideoCount;
            package.NotificationCount = packageDto.NotificationCount;
            package.AudioFileLimit = packageDto.AudioFileLimit;
            package.BarcodeEnabled = packageDto.BarcodeEnabled;
            package.DisplayEnabled = packageDto.DisplayEnabled;
            package.TemplateSelectionEnabled = packageDto.TemplateSelectionEnabled;
            package.CondolenceMessageEnabled = packageDto.CondolenceMessageEnabled;
            package.VisitorContentSubmissionEnabled = packageDto.VisitorContentSubmissionEnabled;
            package.LocationAndNavigationEnabled = packageDto.LocationAndNavigationEnabled;
            package.SharingEnabled = packageDto.SharingEnabled;
            package.File360DegreeEnabled = packageDto.File360DegreeEnabled;
            package.UpdateCapabilityEnabled = packageDto.UpdateCapabilityEnabled;
            package.IsFreePackage = packageDto.IsFreePackage;

            _context.packages.Update(package);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "پکیج با موفقیت به‌روزرسانی شد.", Data = package });
        }

        // متد برای حذف پکیج
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var package = await _context.packages.FindAsync(id);
            if (package == null)
            {
                return NotFound(new { StatusCode = 404, Message = "پکیج پیدا نشد." });
            }

            _context.packages.Remove(package);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }

        // متد برای دریافت پکیج بر اساس شناسه
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageById(int id)
        {
            var package = await _context.packages.FindAsync(id);
            if (package == null)
            {
                return NotFound(new { StatusCode = 404, Message = "پکیج پیدا نشد." });
            }

            return Ok(new { StatusCode = 200, Message = "پکیج با موفقیت دریافت شد.", Data = package });
        }

        // متد برای دریافت همه پکیج‌ها
        [HttpGet]
        public async Task<IActionResult> GetAllPackages()
        {
            var packages = await _context.packages.ToListAsync();
            return Ok(new { StatusCode = 200, Message = "پکیج‌ها با موفقیت دریافت شدند.", Data = packages });
        }
    }
}