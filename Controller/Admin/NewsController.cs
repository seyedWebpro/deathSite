using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Model;
using api.Services;
using api.View.News;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace api.Controller.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly apiContext _context;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<NewsController> _logger;

        public NewsController(apiContext context, IFileUploadService fileUploadService, ILogger<NewsController> logger)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        // ایجاد خبر جدید همراه با آپلود فایل‌ها
        [HttpPost("create")]
        public async Task<IActionResult> CreateNewsWithFile([FromForm] NewsDto newsDto, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "داده‌های خبر نامعتبر است.", StatusCode = 400 });
            }

            var news = new News
            {
                Title = newsDto.Title,
                Content = newsDto.Content,
                PublishedDate = DateTime.UtcNow
            };

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            if (file != null)
            {
                news.FilePath = await _fileUploadService.UploadFileAsync(file, "news", news.Id);
                _context.News.Update(news);
                await _context.SaveChangesAsync();
            }

            return Ok(new
            {
                Message = "خبر با موفقیت ایجاد شد.",
                News = news,
                StatusCode = 200
            });
        }

        // دریافت فایل‌های مرتبط با یک خبر
        [HttpGet("{id}/files")]
        public async Task<IActionResult> GetNewsFiles(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound(new { Message = "خبر یافت نشد.", StatusCode = 404 });
            }

            return Ok(new
            {
                Message = "فایل‌های خبر بازیابی شدند.",
                Data = new { File = news.FilePath },
                StatusCode = 200
            });
        }

        // دریافت همه اخبار
        [HttpGet("all")]
        public async Task<IActionResult> GetAllNews()
        {
            var newsList = await _context.News
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();

            return Ok(new
            {
                Message = "لیست اخبار دریافت شد.",
                Data = newsList,
                StatusCode = 200
            });
        }

        // حذف خبر و تمامی فایل‌های مرتبط
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            try
            {
                var news = await _context.News.FindAsync(id);
                if (news == null)
                {
                    _logger.LogWarning("News with id {NewsId} not found.", id);
                    return NotFound(new { Message = "خبر یافت نشد.", StatusCode = 404 });
                }

                // حذف فایل مرتبط
                if (!string.IsNullOrEmpty(news.FilePath))
                {
                    try
                    {
                        string fileName = Path.GetFileName(news.FilePath);
                        await _fileUploadService.DeleteFileAsync(fileName, "news", id);
                    }
                    catch (FileNotFoundException ex)
                    {
                        _logger.LogWarning(ex, "File not found for deletion: {FilePath}", news.FilePath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error deleting file: {FilePath}", news.FilePath);
                    }
                }

                // حذف پوشه خبر اگر فایل وجود داشته باشد
                string newsFolderPath = Path.Combine("wwwroot", "uploads", "news", id.ToString());
                if (Directory.Exists(newsFolderPath))
                {
                    try
                    {
                        Directory.Delete(newsFolderPath, true);
                        _logger.LogInformation("پوشه {NewsFolderPath} با موفقیت حذف شد.", newsFolderPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "خطا در حذف پوشه {NewsFolderPath}", newsFolderPath);
                    }
                }

                // حذف خبر از دیتابیس
                _context.News.Remove(news);
                await _context.SaveChangesAsync();

                _logger.LogInformation("News with id {NewsId} and associated file was successfully deleted.", id);
                return Ok(new { Message = "خبر و فایل مربوطه حذف شدند.", StatusCode = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting news with id {NewsId}.", id);
                return StatusCode(500, new { Message = "خطای داخلی سرور.", Error = ex.Message, StatusCode = 500 });
            }
        }


    }
}
