using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Model.AdminModel;
using api.Services;
using api.View;
using deathSite.View.Menue;
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
        private readonly ILogger<SiteSettingController> _logger;


        public SiteSettingController(apiContext context, IFileUploadService fileUploadService, ILogger<SiteSettingController> logger)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }


        // متد برای آپلود یا جایگزینی لوگو

        [HttpPost("upload-logo")]
public async Task<IActionResult> UploadLogo(IFormFile file)
{
    // دریافت اولین رکورد تنظیمات لوگو (در صورت وجود)
    var logoSetting = await _context.logoSiteSettings.FirstOrDefaultAsync();

    if (file == null || file.Length == 0)
    {
        return BadRequest(new { StatusCode = 400, Message = "فایلی برای آپلود انتخاب نشده است." });
    }

    if (!file.ContentType.StartsWith("image/"))
    {
        return BadRequest(new { StatusCode = 400, Message = "فقط فایل‌های تصویری مجاز هستند." });
    }

    try
    {
        // حذف لوگوی قبلی (در صورت وجود)
        if (logoSetting != null && !string.IsNullOrEmpty(logoSetting.LogoImagePath))
        {
            string oldFileName = Path.GetFileName(logoSetting.LogoImagePath);
            await _fileUploadService.DeleteFileAsync(oldFileName, "logos", 0);
        }

        // اگر تنظیمات لوگو وجود نداشت، ایجاد کنیم
        if (logoSetting == null)
        {
            logoSetting = new LogoSiteSettings();
            _context.logoSiteSettings.Add(logoSetting);
        }

        // آپلود لوگوی جدید
        string uploadedFilePath = await _fileUploadService.UploadFileAsync(file, "logos", 0);
        logoSetting.LogoImagePath = uploadedFilePath;

        // ذخیره تغییرات در دیتابیس
        await _context.SaveChangesAsync();

        return Ok(new { StatusCode = 200, Message = "لوگوی سایت با موفقیت آپلود شد.", Data = logoSetting });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error uploading site logo.");
        return StatusCode(500, new { StatusCode = 500, Message = "خطا در آپلود لوگو." });
    }
}


        [HttpGet("logo-path")]
        public async Task<IActionResult> GetLogoPath()
        {
            try
            {
                // دریافت اولین رکورد تنظیمات لوگو
                var logoSetting = await _context.logoSiteSettings.FirstOrDefaultAsync();

                if (logoSetting == null || string.IsNullOrEmpty(logoSetting.LogoImagePath))
                {
                    return NotFound(new { StatusCode = 404, Message = "لوگویی یافت نشد." });
                }

                // بازگرداندن مسیر لوگو
                return Ok(new
                {
                    Message = "آدرس فایل لوگو با موفقیت بازیابی شد.",
                    Data = logoSetting.LogoImagePath,
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت آدرس لوگو");
                return StatusCode(500, new { StatusCode = 500, Message = "خطا در دریافت آدرس لوگو.", Error = ex.Message });
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
    }
}
