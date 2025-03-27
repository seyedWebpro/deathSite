using api.Context;
using api.Model;
using api.Services;
using api.View.Blog;
using deathSite.View.Blog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly apiContext _context;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<BlogController> _logger;

        public BlogController(apiContext context, IFileUploadService fileUploadService, ILogger<BlogController> logger)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        // ایجاد بلاگ جدید همراه با آپلود یک فایل
        [HttpPost("create")]
        public async Task<IActionResult> CreateBlogWithFile([FromForm] BlogDto blogDto, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "داده‌های بلاگ نامعتبر است.", StatusCode = 400 });
            }

            var blog = new Blog
            {
                Title = blogDto.Title,
                Content = blogDto.Content,
                PublishedDate = DateTime.UtcNow,
                Description = blogDto.Description
            };

            _context.blogs.Add(blog);
            await _context.SaveChangesAsync();

            if (file != null)
            {
                blog.FilePath = await _fileUploadService.UploadFileAsync(file, "blogs", blog.Id);
            }

            _context.blogs.Update(blog);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "بلاگ با موفقیت ایجاد شد.",
                Blog = blog,
                StatusCode = 200
            });
        }

        // دریافت همه بلاگ‌ها
        [HttpGet("all")]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await _context.blogs
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    b.Content,
                    b.Description,
                    b.PublishedDate,
                    FilePath = b.FilePath
                })
                .ToListAsync();

            return Ok(new
            {
                Message = "لیست بلاگ‌ها بازیابی شد.",
                Data = blogs,
                StatusCode = 200
            });
        }

        // دریافت فایل مرتبط با یک بلاگ
        [HttpGet("{id}/file")]
        public async Task<IActionResult> GetBlogFile(int id)
        {
            var blog = await _context.blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound(new { Message = "بلاگ یافت نشد.", StatusCode = 404 });
            }

            return Ok(new
            {
                Message = "فایل بلاگ بازیابی شد.",
                Data = blog.FilePath,
                StatusCode = 200
            });
        }

        // حذف بلاگ و فایل‌ها و پوشه‌های مرتبط
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            try
            {
                var blog = await _context.blogs.FindAsync(id);
                if (blog == null)
                {
                    _logger.LogWarning("Blog with id {BlogId} not found.", id);
                    return NotFound(new { Message = "بلاگ یافت نشد.", StatusCode = 404 });
                }

                // حذف فایل مرتبط
                if (!string.IsNullOrEmpty(blog.FilePath))
                {
                    try
                    {
                        string fileName = Path.GetFileName(blog.FilePath);
                        await _fileUploadService.DeleteFileAsync(fileName, "blogs", id);
                    }
                    catch (FileNotFoundException ex)
                    {
                        _logger.LogWarning(ex, "File not found for deletion: {FilePath}", blog.FilePath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error deleting file: {FilePath}", blog.FilePath);
                    }
                }

                // حذف پوشه بلاگ اگر فایل‌ها حذف شدند
                string blogFolderPath = Path.Combine("wwwroot", "uploads", "blogs", id.ToString());
                if (Directory.Exists(blogFolderPath))
                {
                    try
                    {
                        Directory.Delete(blogFolderPath, true);
                        _logger.LogInformation("پوشه {BlogFolderPath} با موفقیت حذف شد.", blogFolderPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "خطا در حذف پوشه {BlogFolderPath}", blogFolderPath);
                    }
                }

                // حذف بلاگ از دیتابیس
                _context.blogs.Remove(blog);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Blog with id {BlogId} and associated file was successfully deleted.", id);
                return Ok(new { Message = "بلاگ و فایل مربوطه حذف شدند.", StatusCode = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting blog with id {BlogId}.", id);
                return StatusCode(500, new { Message = "خطای داخلی سرور.", Error = ex.Message, StatusCode = 500 });
            }
        }

        // دریافت بلاگ بر اساس شناسه
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogById(int id)
        {
            var blog = await _context.blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound(new { Message = "بلاگ یافت نشد.", StatusCode = 404 });
            }

            return Ok(new
            {
                Message = "بلاگ با موفقیت دریافت شد.",
                Data = blog,
                StatusCode = 200
            });
        }

        // ویرایش بلاگ موجود با فیلدهای اختیاری
[HttpPut("update/{id}")]
public async Task<IActionResult> UpdateBlog(int id, [FromForm] UpdateBlogDto updateBlogDto, IFormFile? file)
{
    var blog = await _context.blogs.FindAsync(id);
    if (blog == null)
    {
        return NotFound(new { Message = "بلاگ یافت نشد.", StatusCode = 404 });
    }

    // به روز رسانی فیلدها تنها در صورتی که مقدار داشته باشند
    if (!string.IsNullOrEmpty(updateBlogDto.Title))
    {
        blog.Title = updateBlogDto.Title;
    }
    if (!string.IsNullOrEmpty(updateBlogDto.Description))
    {
        blog.Description = updateBlogDto.Description;
    }
    if (!string.IsNullOrEmpty(updateBlogDto.Content))
    {
        blog.Content = updateBlogDto.Content;
    }

    // به روز رسانی تاریخ انتشار
    blog.PublishedDate = DateTime.UtcNow;

    // آپلود فایل جدید در صورت ارسال
    if (file != null)
    {
        // در صورت وجود فایل قبلی، ابتدا آن را حذف می‌کنیم
        if (!string.IsNullOrEmpty(blog.FilePath))
        {
            string oldFileName = Path.GetFileName(blog.FilePath);
            try
            {
                await _fileUploadService.DeleteFileAsync(oldFileName, "blogs", id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "خطا در حذف فایل قبلی {FilePath}", blog.FilePath);
            }
        }
        blog.FilePath = await _fileUploadService.UploadFileAsync(file, "blogs", blog.Id);
    }

    _context.blogs.Update(blog);
    await _context.SaveChangesAsync();

    return Ok(new { Message = "بلاگ با موفقیت به روز شد.", Data = blog, StatusCode = 200 });
}


    }
}
