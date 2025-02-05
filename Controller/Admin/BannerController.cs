using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Model.AdminModel;
using api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("api/banner")]
    public class BannerController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<BannerController> _logger;
        private readonly apiContext _context;

        public BannerController(IFileUploadService fileUploadService, ILogger<BannerController> logger, apiContext context)
        {
            _fileUploadService = fileUploadService;
            _logger = logger;
            _context = context;
        }
        [HttpPost("upload-mobile")]
        public async Task<IActionResult> UploadMobileImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { StatusCode = 400, Message = "فایل معتبر نیست." });
            }

            // ایجاد بنر جدید در دیتابیس
            var banner = new Banner { PublishedDate = DateTime.UtcNow };
            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();

            // آپلود فایل
            string uploadedFilePath = await _fileUploadService.UploadFileAsync(file, "banners", banner.Id);

            // ذخیره آدرس تصویر موبایل در دیتابیس
            banner.MobileImagePath = uploadedFilePath;
            _context.Banners.Update(banner);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "تصویر موبایل با موفقیت بارگذاری شد.", FilePath = uploadedFilePath, BannerId = banner.Id });
        }

        [HttpPost("upload-desktop")]
        public async Task<IActionResult> UploadDesktopImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { StatusCode = 400, Message = "فایل معتبر نیست." });
            }

            // ایجاد بنر جدید در دیتابیس
            var banner = new Banner { PublishedDate = DateTime.UtcNow };
            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();

            // آپلود فایل
            string uploadedFilePath = await _fileUploadService.UploadFileAsync(file, "banners", banner.Id);

            // ذخیره آدرس تصویر دسکتاپ در دیتابیس
            banner.DesktopImagePath = uploadedFilePath;
            _context.Banners.Update(banner);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "تصویر دسکتاپ با موفقیت بارگذاری شد.", FilePath = uploadedFilePath, BannerId = banner.Id });
        }
        // دریافت همه بنرهای موبایل
        [HttpGet("mobile")]
        public async Task<IActionResult> GetAllMobileBanners()
        {
            var mobileBanners = await _context.Banners
                                               .Where(b => b.MobileImagePath != null)
                                               .ToListAsync();

            if (mobileBanners == null || mobileBanners.Count == 0)
            {
                return NotFound(new { StatusCode = 404, Message = "هیچ بنر موبایلی پیدا نشد." });
            }

            return Ok(new { StatusCode = 200, Banners = mobileBanners });
        }

        // دریافت همه بنرهای دسکتاپ
        [HttpGet("desktop")]
        public async Task<IActionResult> GetAllDesktopBanners()
        {
            var desktopBanners = await _context.Banners
                                                .Where(b => b.DesktopImagePath != null)
                                                .ToListAsync();

            if (desktopBanners == null || desktopBanners.Count == 0)
            {
                return NotFound(new { StatusCode = 404, Message = "هیچ بنر دسکتاپی پیدا نشد." });
            }

            return Ok(new { StatusCode = 200, Banners = desktopBanners });
        }

        // دریافت بنر با ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBannerById(int id)
        {
            var banner = await _context.Banners.FindAsync(id);

            if (banner == null)
            {
                return NotFound(new { StatusCode = 404, Message = "بنر مورد نظر یافت نشد." });
            }

            return Ok(new { StatusCode = 200, Banner = banner });
        }

        // حذف همه بنرهای موبایل و فایل‌های آن‌ها به همراه پوشه
        [HttpDelete("mobile/delete-all")]
        public async Task<IActionResult> DeleteAllMobileBanners()
        {
            try
            {
                var mobileBanners = await _context.Banners.Where(b => b.MobileImagePath != null).ToListAsync();

                if (mobileBanners.Count == 0)
                {
                    return NotFound(new { StatusCode = 404, Message = "هیچ بنر موبایلی پیدا نشد." });
                }

                bool anyFileDeleted = false;
                foreach (var banner in mobileBanners)
                {
                    try
                    {
                        string fileName = Path.GetFileName(banner.MobileImagePath);
                        await _fileUploadService.DeleteFileAsync(fileName, "banners", banner.Id);
                        anyFileDeleted = true;
                    }
                    catch (FileNotFoundException ex)
                    {
                        _logger.LogWarning(ex, "فایل مربوط به بنر موبایل با آدرس {FilePath} پیدا نشد.", banner.MobileImagePath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "خطا در حذف فایل بنر موبایل با آدرس {FilePath}.", banner.MobileImagePath);
                    }
                }

                // حذف پوشه بنرهای موبایل اگر فایل‌ها حذف شدند
                if (anyFileDeleted)
                {
                    foreach (var banner in mobileBanners) // Iterate again to delete folder for each banner
                    {
                        string bannerFolderPath = Path.Combine("wwwroot", "uploads", "banners", banner.Id.ToString());
                        if (Directory.Exists(bannerFolderPath))
                        {
                            try
                            {
                                Directory.Delete(bannerFolderPath, true);
                                _logger.LogInformation("پوشه {BannerFolderPath} با موفقیت حذف شد.", bannerFolderPath);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "خطا در حذف پوشه {BannerFolderPath}", bannerFolderPath);
                            }
                        }
                    }
                }

                _context.Banners.RemoveRange(mobileBanners);
                await _context.SaveChangesAsync();

                return Ok(new { StatusCode = 200, Message = "بنرهای موبایل و فایل‌های آن‌ها حذف شدند." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در حذف بنرهای موبایل.");
                return StatusCode(500, new { StatusCode = 500, Message = "خطای داخلی سرور.", Error = ex.Message });
            }
        }

        // حذف همه بنرهای دسکتاپ و فایل‌های آن‌ها به همراه پوشه
        [HttpDelete("desktop/delete-all")]
        public async Task<IActionResult> DeleteAllDesktopBanners()
        {
            try
            {
                var desktopBanners = await _context.Banners.Where(b => b.DesktopImagePath != null).ToListAsync();

                if (desktopBanners.Count == 0)
                {
                    return NotFound(new { StatusCode = 404, Message = "هیچ بنر دسکتاپی پیدا نشد." });
                }

                bool anyFileDeleted = false;
                foreach (var banner in desktopBanners)
                {
                    try
                    {
                        string fileName = Path.GetFileName(banner.DesktopImagePath);
                        await _fileUploadService.DeleteFileAsync(fileName, "banners", banner.Id);
                        anyFileDeleted = true;
                    }
                    catch (FileNotFoundException ex)
                    {
                        _logger.LogWarning(ex, "فایل مربوط به بنر دسکتاپ با آدرس {FilePath} پیدا نشد.", banner.DesktopImagePath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "خطا در حذف فایل بنر دسکتاپ با آدرس {FilePath}.", banner.DesktopImagePath);
                    }
                }

                // حذف پوشه بنرهای دسکتاپ اگر فایل‌ها حذف شدند
                if (anyFileDeleted)
                {
                    foreach (var banner in desktopBanners) // Iterate again to delete folder for each banner
                    {
                        string bannerFolderPath = Path.Combine("wwwroot", "uploads", "banners", banner.Id.ToString());
                        if (Directory.Exists(bannerFolderPath))
                        {
                            try
                            {
                                Directory.Delete(bannerFolderPath, true);
                                _logger.LogInformation("پوشه {BannerFolderPath} با موفقیت حذف شد.", bannerFolderPath);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "خطا در حذف پوشه {BannerFolderPath}", bannerFolderPath);
                            }
                        }
                    }
                }

                _context.Banners.RemoveRange(desktopBanners);
                await _context.SaveChangesAsync();

                return Ok(new { StatusCode = 200, Message = "بنرهای دسکتاپ و فایل‌های آن‌ها حذف شدند." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در حذف بنرهای دسکتاپ.");
                return StatusCode(500, new { StatusCode = 500, Message = "خطای داخلی سرور.", Error = ex.Message });
            }
        }

        // حذف بنر بر اساس ID
[HttpDelete("delete/{id}")]
public async Task<IActionResult> DeleteBannerById(int id)
{
    try
    {
        var banner = await _context.Banners.FindAsync(id);
        if (banner == null)
        {
            return NotFound(new { StatusCode = 404, Message = "بنر مورد نظر یافت نشد." });
        }

        // حذف فایل‌های مرتبط
        bool anyFileDeleted = false;

        if (!string.IsNullOrEmpty(banner.MobileImagePath))
        {
            try
            {
                string fileName = Path.GetFileName(banner.MobileImagePath);
                await _fileUploadService.DeleteFileAsync(fileName, "banners", id);
                anyFileDeleted = true;
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning(ex, "فایل مربوط به بنر موبایل با آدرس {FilePath} پیدا نشد.", banner.MobileImagePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در حذف فایل بنر موبایل با آدرس {FilePath}.", banner.MobileImagePath);
            }
        }

        if (!string.IsNullOrEmpty(banner.DesktopImagePath))
        {
            try
            {
                string fileName = Path.GetFileName(banner.DesktopImagePath);
                await _fileUploadService.DeleteFileAsync(fileName, "banners", id);
                anyFileDeleted = true;
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning(ex, "فایل مربوط به بنر دسکتاپ با آدرس {FilePath} پیدا نشد.", banner.DesktopImagePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در حذف فایل بنر دسکتاپ با آدرس {FilePath}.", banner.DesktopImagePath);
            }
        }

        // حذف پوشه بنر اگر فایل‌ها حذف شدند
        string bannerFolderPath = Path.Combine("wwwroot", "uploads", "banners", id.ToString());
        if (anyFileDeleted && Directory.Exists(bannerFolderPath))
        {
            try
            {
                Directory.Delete(bannerFolderPath, true);
                _logger.LogInformation("پوشه {BannerFolderPath} با موفقیت حذف شد.", bannerFolderPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در حذف پوشه {BannerFolderPath}", bannerFolderPath);
            }
        }

        // حذف رکورد از دیتابیس
        _context.Banners.Remove(banner);
        await _context.SaveChangesAsync();

        return Ok(new { StatusCode = 200, Message = "بنر و فایل‌های مربوطه حذف شدند." });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "خطا در حذف بنر با ID {BannerId}.", id);
        return StatusCode(500, new { StatusCode = 500, Message = "خطای داخلی سرور.", Error = ex.Message });
    }
}


    }
}
