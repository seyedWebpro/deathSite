using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Services;
using deathSite.Model.AdminModel;
using deathSite.View.About;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace deathSite.Controller.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class AboutUsController : ControllerBase
    {
        private readonly apiContext _context;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<AboutUsController> _logger;

        public AboutUsController(apiContext context, IFileUploadService fileUploadService, ILogger<AboutUsController> logger)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAboutUs([FromForm] AboutUsDto aboutUsDto, IFormFile? file)
        {
            // همیشه فقط یک رکورد "درباره ما" باید وجود داشته باشد
            var aboutUs = await _context.aboutUs.FirstOrDefaultAsync();

            if (aboutUs == null)
            {
                aboutUs = new AboutUs();
                _context.aboutUs.Add(aboutUs);
            }

            // به‌روزرسانی مقادیر
            aboutUs.Title = aboutUsDto.Title ?? aboutUs.Title;
            aboutUs.Description = aboutUsDto.Description ?? aboutUs.Description;
            aboutUs.Poshtiban = aboutUsDto.Poshtiban ?? aboutUs.Poshtiban;
            aboutUs.Sabeghe = aboutUsDto.Sabeghe ?? aboutUs.Sabeghe;
            aboutUs.ShahidSabt = aboutUsDto.ShahidSabt ?? aboutUs.ShahidSabt;

            // اگر فایلی برای آپلود وجود داشته باشد
            if (file != null)
            {
                if (!string.IsNullOrEmpty(aboutUs.ImageUrl))
                {
                    string oldFileName = Path.GetFileName(aboutUs.ImageUrl);
                    await _fileUploadService.DeleteFileAsync(oldFileName, "aboutus", 0);
                }

                aboutUs.ImageUrl = await _fileUploadService.UploadFileAsync(file, "aboutus", 0);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "اطلاعات 'درباره ما' با موفقیت به‌روزرسانی شد.",
                AboutUs = aboutUs,
                StatusCode = 200
            });
        }


        [HttpGet("get-about-us")]
        public async Task<IActionResult> GetAboutUs()
        {
            var aboutUs = await _context.aboutUs.FirstOrDefaultAsync();

            if (aboutUs == null)
            {
                return NotFound(new { Message = "رکورد 'درباره ما' یافت نشد.", StatusCode = 404 });
            }

            return Ok(new
            {
                Message = "اطلاعات 'درباره ما' با موفقیت دریافت شد.",
                AboutUs = aboutUs,
                StatusCode = 200
            });
        }


        [HttpDelete("delete-all-about-us-files")]
        public async Task<IActionResult> DeleteAllAboutUsFiles()
        {
            try
            {
                // گرفتن رکوردهای "درباره ما" از پایگاه داده
                var allAboutUs = await _context.aboutUs.ToListAsync();

                if (allAboutUs.Count == 0)
                {
                    return NotFound(new { Message = "هیچ رکورد 'درباره ما' برای حذف وجود ندارد.", StatusCode = 404 });
                }

                bool anyFileDeleted = false;

                // حذف فایل‌های مربوط به "درباره ما"
                foreach (var aboutUs in allAboutUs)
                {
                    if (!string.IsNullOrEmpty(aboutUs.ImageUrl))
                    {
                        string fileName = Path.GetFileName(aboutUs.ImageUrl);
                        try
                        {
                            // حذف فایل "درباره ما"
                            await _fileUploadService.DeleteFileAsync(fileName, "aboutus", aboutUs.Id);
                            anyFileDeleted = true;
                        }
                        catch (FileNotFoundException ex)
                        {
                            _logger.LogWarning(ex, "File not found for deletion: {FilePath}", aboutUs.ImageUrl);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error deleting file: {FilePath}", aboutUs.ImageUrl);
                        }
                    }
                }

                // حذف پوشه مربوط به "درباره ما" اگر فایلی حذف شد
                if (anyFileDeleted)
                {
                    string aboutUsFolderPath = Path.Combine("wwwroot", "uploads", "aboutus");
                    if (Directory.Exists(aboutUsFolderPath))
                    {
                        try
                        {
                            Directory.Delete(aboutUsFolderPath, true);
                            _logger.LogInformation("Folder {AboutUsFolderPath} deleted successfully.", aboutUsFolderPath);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error deleting folder {AboutUsFolderPath}", aboutUsFolderPath);
                        }
                    }
                }

                // حذف تمامی رکوردهای "درباره ما" از پایگاه داده
                _context.aboutUs.RemoveRange(allAboutUs);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "تمامی فایل‌ها و رکوردهای 'درباره ما' با موفقیت حذف شدند.", StatusCode = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting 'درباره ما' files and records.");
                return StatusCode(500, new { Message = "خطای داخلی سرور هنگام حذف فایل‌ها و رکوردهای 'درباره ما'.", Error = ex.Message, StatusCode = 500 });
            }
        }


    }
}