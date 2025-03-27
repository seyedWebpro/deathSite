using System;
using System.IO;
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
    public class PosterController : ControllerBase
    {
        private readonly apiContext _context;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<PosterController> _logger;

        public PosterController(apiContext context, IFileUploadService fileUploadService, ILogger<PosterController> logger)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        // POST: api/Posters/People/upload
        [HttpPost("People/upload")]
        public async Task<IActionResult> UploadPeoplePoster(IFormFile file, [FromForm] string link)
        {
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
                // آپلود پوستر افراد
                string uploadedFilePath = await _fileUploadService.UploadFileAsync(file, "PeoplePoster", 0);
                // ایجاد شی پوستر و ذخیره در دیتابیس به همراه لینک
                var poster = new Poster
                {
                    FilePath = uploadedFilePath,
                    Category = "PeoplePoster",
                    Link = link // مقدار لینک دریافت شده از فرم
                };
                _context.posters.Add(poster);
                await _context.SaveChangesAsync();
                return Ok(new { StatusCode = 200, Message = "پوستر افراد با موفقیت آپلود شد.", Data = poster });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading poster for people");
                return StatusCode(500, new { StatusCode = 500, Message = "خطا در آپلود فایل." });
            }
        }

        // GET: api/Posters/People
        [HttpGet("People")]
        public async Task<IActionResult> GetPeoplePosters()
        {
            var posters = await _context.posters
                .Where(p => p.Category == "PeoplePoster")
                .Select(p => new { p.Id, p.FilePath, p.Link })  // اضافه کردن p.Link
                .ToListAsync();

            return Ok(new { Message = "لیست پوسترهای افراد بازیابی شد.", Data = posters, StatusCode = 200 });
        }


        // GET: api/Posters/People/{id}/file
        [HttpGet("People/{id}/file")]
        public async Task<IActionResult> GetPeoplePosterFile(int id)
        {
            var poster = await _context.posters.FindAsync(id);
            if (poster == null)
            {
                return NotFound(new { Message = "پوستر یافت نشد.", StatusCode = 404 });
            }
            return Ok(new { Message = "آدرس فایل پوستر بازیابی شد.", Data = poster.FilePath, StatusCode = 200 });
        }

        // DELETE: api/Posters/People/delete/{id}
        [HttpDelete("People/delete/{id}")]
        public async Task<IActionResult> DeletePeoplePoster(int id)
        {
            try
            {
                var poster = await _context.posters.FindAsync(id);
                if (poster == null)
                {
                    _logger.LogWarning("Poster with id {PosterId} not found.", id);
                    return NotFound(new { Message = "پوستر یافت نشد.", StatusCode = 404 });
                }

                // حذف فایل مرتبط
                if (!string.IsNullOrEmpty(poster.FilePath))
                {
                    try
                    {
                        string fileName = Path.GetFileName(poster.FilePath);
                        await _fileUploadService.DeleteFileAsync(fileName, "PeoplePoster", 0);
                    }
                    catch (FileNotFoundException ex)
                    {
                        _logger.LogWarning(ex, "File not found for deletion: {FilePath}", poster.FilePath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error deleting file: {FilePath}", poster.FilePath);
                    }
                }

                // حذف رکورد از دیتابیس
                _context.posters.Remove(poster);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Poster with id {PosterId} and associated file was successfully deleted.", id);
                return Ok(new { Message = "پوستر و فایل مربوطه حذف شدند.", StatusCode = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting poster with id {PosterId}.", id);
                return StatusCode(500, new { Message = "خطای داخلی سرور.", Error = ex.Message, StatusCode = 500 });
            }
        }

        // POST: api/Posters/Shahid/upload
        [HttpPost("Shahid/upload")]
        public async Task<IActionResult> UploadShahidPoster(IFormFile file, [FromForm] string link)
        {
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
                // آپلود پوستر شهید
                string uploadedFilePath = await _fileUploadService.UploadFileAsync(file, "ShahidPoster", 0);
                // ایجاد شی پوستر و ذخیره در دیتابیس به همراه لینک
                var poster = new Poster
                {
                    FilePath = uploadedFilePath,
                    Category = "ShahidPoster",
                    Link = link
                };
                _context.posters.Add(poster);
                await _context.SaveChangesAsync();
                return Ok(new { StatusCode = 200, Message = "پوستر شهید با موفقیت آپلود شد.", Data = poster });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading poster for shahid");
                return StatusCode(500, new { StatusCode = 500, Message = "خطا در آپلود فایل." });
            }
        }



        // GET: api/Posters/Shahid
        [HttpGet("Shahid")]
        public async Task<IActionResult> GetShahidPosters()
        {
            var posters = await _context.posters
                .Where(p => p.Category == "ShahidPoster")
                .Select(p => new { p.Id, p.FilePath, p.Link })  // اضافه کردن p.Link
                .ToListAsync();

            return Ok(new { Message = "لیست پوسترهای شهید بازیابی شد.", Data = posters, StatusCode = 200 });
        }


        // GET: api/Posters/Shahid/{id}/file
        [HttpGet("Shahid/{id}/file")]
        public async Task<IActionResult> GetShahidPosterFile(int id)
        {
            var poster = await _context.posters.FindAsync(id);
            if (poster == null)
            {
                return NotFound(new { Message = "پوستر یافت نشد.", StatusCode = 404 });
            }
            return Ok(new { Message = "آدرس فایل پوستر بازیابی شد.", Data = poster.FilePath, StatusCode = 200 });
        }

        // DELETE: api/Posters/Shahid/delete/{id}
        [HttpDelete("Shahid/delete/{id}")]
        public async Task<IActionResult> DeleteShahidPoster(int id)
        {
            try
            {
                var poster = await _context.posters.FindAsync(id);
                if (poster == null)
                {
                    _logger.LogWarning("Poster with id {PosterId} not found.", id);
                    return NotFound(new { Message = "پوستر یافت نشد.", StatusCode = 404 });
                }

                // حذف فایل مرتبط
                if (!string.IsNullOrEmpty(poster.FilePath))
                {
                    try
                    {
                        string fileName = Path.GetFileName(poster.FilePath);
                        await _fileUploadService.DeleteFileAsync(fileName, "ShahidPoster", 0);
                    }
                    catch (FileNotFoundException ex)
                    {
                        _logger.LogWarning(ex, "File not found for deletion: {FilePath}", poster.FilePath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error deleting file: {FilePath}", poster.FilePath);
                    }
                }

                // حذف رکورد از دیتابیس
                _context.posters.Remove(poster);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Poster with id {PosterId} and associated file was successfully deleted.", id);
                return Ok(new { Message = "پوستر و فایل مربوطه حذف شدند.", StatusCode = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting poster with id {PosterId}.", id);
                return StatusCode(500, new { Message = "خطای داخلی سرور.", Error = ex.Message, StatusCode = 500 });
            }
        }



    }
}
