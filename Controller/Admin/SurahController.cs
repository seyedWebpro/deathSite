using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Model.AdminModel;
using api.Services;
using api.View.Surah;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class SurahController : ControllerBase
    {
        private readonly apiContext _context;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<SurahController> _logger;

        public SurahController(apiContext context, IFileUploadService fileUploadService, ILogger<SurahController> logger)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        // ثبت سوره جدید به همراه فایل
        [HttpPost("create")]
        public async Task<IActionResult> CreateSurahWithFiles([FromForm] SurahDto surahDto, List<IFormFile> files)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "داده‌های سوره نامعتبر است.", StatusCode = 400 });
            }

            var surah = new Surah
            {
                Name = surahDto.Name,
                FilePath = string.Empty // مقدار اولیه
            };

            // ابتدا سوره را در دیتابیس ذخیره می‌کنیم تا شناسه آن مشخص شود
            _context.Surahs.Add(surah);
            await _context.SaveChangesAsync();

            // بررسی وجود فایل‌ها
            if (files != null && files.Count > 0)
            {
                // در اینجا فقط یک فایل داریم که مسیر آن به FilePath اضافه می‌شود
                var file = files.FirstOrDefault();
                if (file != null && file.Length > 0)
                {
                    string uploadedFilePath = await _fileUploadService.UploadFileAsync(file, "surahs", surah.Id);
                    surah.FilePath = uploadedFilePath;
                }

                // سوره را با مسیر فایل به‌روزرسانی می‌کنیم
                _context.Surahs.Update(surah);
                await _context.SaveChangesAsync();
            }

            return Ok(new
            {
                Message = "سوره با موفقیت ایجاد شد.",
                Surah = surah,
                StatusCode = 200
            });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSurah(int id)
        {
            try
            {
                var surah = await _context.Surahs.FindAsync(id);
                if (surah == null)
                {
                    _logger.LogWarning("Surah with id {SurahId} not found.", id);
                    return NotFound(new { Message = "سوره یافت نشد.", StatusCode = 404 });
                }

                string fileUrl = surah.FilePath;

                bool fileDeleted = false;

                // حذف فایل مربوط به سوره
                if (!string.IsNullOrEmpty(fileUrl))
                {
                    try
                    {
                        string fileName = Path.GetFileName(fileUrl);
                        await _fileUploadService.DeleteFileAsync(fileName, "surahs", id);
                        fileDeleted = true;
                    }
                    catch (FileNotFoundException ex)
                    {
                        _logger.LogWarning(ex, "File not found for deletion: {FileUrl}", fileUrl);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error deleting file: {FileUrl}", fileUrl);
                    }
                }

                // حذف پوشه سوره اگر فایل‌ها حذف شده‌اند
                if (fileDeleted)
                {
                    string surahFolderPath = Path.Combine("wwwroot", "uploads", "surahs", id.ToString());
                    if (Directory.Exists(surahFolderPath))
                    {
                        try
                        {
                            Directory.Delete(surahFolderPath, true);
                            _logger.LogInformation("Folder {SurahFolderPath} deleted successfully.", surahFolderPath);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error deleting folder {SurahFolderPath}", surahFolderPath);
                        }
                    }
                }

                // حذف سوره از دیتابیس
                _context.Surahs.Remove(surah);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Surah with id {SurahId} and associated files were successfully deleted.", id);
                return Ok(new { Message = "سوره و فایل‌های مربوطه حذف شدند.", StatusCode = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting surah with id {SurahId}.", id);
                return StatusCode(500, new { Message = "خطای داخلی سرور.", Error = ex.Message, StatusCode = 500 });
            }
        }

        // دریافت همه سوره‌ها
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSurahs()
        {
            var surahs = await _context.Surahs.ToListAsync();
            return Ok(new { Message = "سوره‌ها با موفقیت بازیابی شدند.", Surahs = surahs });
        }

        // دریافت سوره با شناسه
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSurahById(int id)
        {
            var surah = await _context.Surahs.FindAsync(id);
            if (surah == null)
            {
                return NotFound(new { Message = "سوره یافت نشد." });
            }

            return Ok(new { Message = "سوره با موفقیت بازیابی شد.", Surah = surah });
        }

    }
}