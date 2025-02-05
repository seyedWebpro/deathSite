using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Model.AdminModel;
using api.Services;
using api.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class SiteSettingController : ControllerBase
    {
        private readonly apiContext _context;
        private readonly IFileUploadService _fileUploadService;
                private readonly ILogger<NewsController> _logger;


        public SiteSettingController(apiContext context, IFileUploadService fileUploadService, ILogger<NewsController> logger)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }


        // متد برای آپلود یا جایگزینی لوگو

        [HttpPost("upload-logo")]
        public async Task<IActionResult> UploadLogo(IFormFile logoImage)
        {
            if (logoImage == null || logoImage.Length == 0)
            {
                return BadRequest(new { Message = "لطفاً لوگو را بارگذاری کنید.", StatusCode = 400 });
            }

            try
            {
                // تعیین دسته بندی فایل
                string category = logoImage.ContentType.Split('/')[0];

                // تعیین یک entityId برای لوگو. اگر لوگو متعلق به یک موجودیت خاص است، این مقدار باید از آن موجودیت گرفته شود.
                int entityId = 0; // این مقدار را می‌توانید به دلخواه تغییر دهید

                // آپلود فایل لوگو
                string uploadedFilePath = await _fileUploadService.UploadFileAsync(logoImage, "logos", entityId);

                // بررسی وجود لوگو در پایگاه داده
                var existingLogo = await _context.logoSiteSettings.FirstOrDefaultAsync();

                if (existingLogo != null)
                {
                    // فقط مسیر لوگو را به روز می‌کنیم
                    existingLogo.LogoImagePath = uploadedFilePath;
                    _context.logoSiteSettings.Update(existingLogo);
                    await _context.SaveChangesAsync();

                    return Ok(new { Message = "لوگوی موجود با موفقیت جایگزین شد.", LogoPath = uploadedFilePath, StatusCode = 200 });
                }
                else
                {
                    // در صورتی که لوگو قبلاً موجود نباشد، لوگوی جدید را اضافه می‌کنیم
                    var newLogo = new LogoSiteSettings
                    {
                        LogoImagePath = uploadedFilePath
                    };

                    _context.logoSiteSettings.Add(newLogo);
                    await _context.SaveChangesAsync();

                    return Ok(new { Message = "لوگو با موفقیت آپلود شد.", LogoPath = uploadedFilePath, StatusCode = 200 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "خطای داخلی سرور هنگام آپلود لوگو.", Error = ex.Message, StatusCode = 500 });
            }
        }


        [HttpDelete("delete-all-logos")]
public async Task<IActionResult> DeleteAllLogos()
{
    try
    {
        // گرفتن تمامی لوگوها از پایگاه داده
        var allLogos = await _context.logoSiteSettings.ToListAsync();

        if (allLogos.Count == 0)
        {
            return NotFound(new { Message = "هیچ لوگویی برای حذف وجود ندارد.", StatusCode = 404 });
        }

        bool anyFileDeleted = false;

        // حذف فایل‌های مربوط به لوگوها
        foreach (var logo in allLogos)
        {
            if (!string.IsNullOrEmpty(logo.LogoImagePath))
            {
                string fileName = Path.GetFileName(logo.LogoImagePath);
                try
                {
                    // حذف فایل لوگو
                    await _fileUploadService.DeleteFileAsync(fileName, "logos", 0); // entityId را می‌توان به دلخواه تنظیم کرد
                    anyFileDeleted = true;
                }
                catch (FileNotFoundException ex)
                {
                    _logger.LogWarning(ex, "File not found for deletion: {FilePath}", logo.LogoImagePath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting file: {FilePath}", logo.LogoImagePath);
                }
            }
        }

        // حذف پوشه مربوط به لوگوها اگر فایلی حذف شد
        if (anyFileDeleted)
        {
            string logosFolderPath = Path.Combine("wwwroot", "uploads", "logos");
            if (Directory.Exists(logosFolderPath))
            {
                try
                {
                    Directory.Delete(logosFolderPath, true);
                    _logger.LogInformation("Folder {LogosFolderPath} deleted successfully.", logosFolderPath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting folder {LogosFolderPath}", logosFolderPath);
                }
            }
        }

        // حذف تمامی لوگوها از پایگاه داده
        _context.logoSiteSettings.RemoveRange(allLogos);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "تمامی لوگوها با موفقیت حذف شدند.", StatusCode = 200 });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An error occurred while deleting logos.");
        return StatusCode(500, new { Message = "خطای داخلی سرور هنگام حذف لوگوها.", Error = ex.Message, StatusCode = 500 });
    }
}



        // متد برای دریافت لوگو فعلی
        [HttpGet("current-logo")]
        public async Task<IActionResult> GetCurrentLogo()
        {
            var logo = await _context.logoSiteSettings.FirstOrDefaultAsync();

            if (logo == null)
            {
                return NotFound(new { Message = "لوگویی برای سایت پیدا نشد.", StatusCode = 404 });
            }

            return Ok(new { LogoPath = logo.LogoImagePath, StatusCode = 200 });
        }

        // متد برای ویرایش یا اضافه کردن منو
        [HttpPut("update-menu/{id}")]
        public async Task<IActionResult> UpdateMenu(int id, [FromBody] MenuSiteSettingsDto updatedMenuDto)
        {
            var existingMenu = await _context.MenuSiteSettings.FindAsync(id);
            if (existingMenu != null)
            {
                // اگر منو موجود است، آن را ویرایش می‌کنیم
                existingMenu.Title = updatedMenuDto.Title;
                existingMenu.Link = updatedMenuDto.Link;

                // اگر مقدار Order موجود باشد، به روز رسانی می‌کنیم
                if (updatedMenuDto.Order.HasValue)
                {
                    existingMenu.Order = updatedMenuDto.Order.Value;
                }

                _context.MenuSiteSettings.Update(existingMenu);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "منو با موفقیت ویرایش شد.", StatusCode = 200 });
            }
            else
            {
                // اگر منو موجود نیست، منو جدیدی ایجاد می‌کنیم
                var newMenu = new MenuSiteSettings
                {
                    Title = updatedMenuDto.Title,
                    Link = updatedMenuDto.Link,
                    Order = updatedMenuDto.Order
                };

                _context.MenuSiteSettings.Add(newMenu);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "منو جدید با موفقیت اضافه شد.", StatusCode = 200 });
            }
        }

        // متد برای دریافت همه منوها
        [HttpGet("all-menus")]
        public async Task<IActionResult> GetAllMenus()
        {
            var menus = await _context.MenuSiteSettings
                .OrderBy(m => m.Order ?? int.MaxValue) // اگر Order نداشته باشد، در انتها قرار می‌گیرد
                .Select(m => new MenuSiteSettingsDto
                {
                    Title = m.Title,
                    Link = m.Link,
                    Order = m.Order
                })
                .ToListAsync();

            return Ok(new { Message = "منوها با موفقیت بازیابی شدند.", menus, StatusCode = 200 });
        }

        // متد برای حذف منو
        [HttpDelete("delete-menu/{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var menu = await _context.MenuSiteSettings.FindAsync(id);
            if (menu == null)
            {
                return NotFound(new { Message = "منو یافت نشد.", StatusCode = 404 });
            }

            _context.MenuSiteSettings.Remove(menu);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "منو با موفقیت حذف شد.", StatusCode = 200 });
        }
    }
}
