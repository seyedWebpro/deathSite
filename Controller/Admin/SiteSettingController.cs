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

        // منو

         // GET: api/Menu
        [HttpGet]
        public async Task<ActionResult> GetMenus()
        {
            var menus = await _context.MenuSiteSettings
                .OrderBy(m => m.Order)
                .ToListAsync();

            return Ok(new { StatusCode = 200, Data = menus });
        }

        // GET: api/Menu/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetMenu(int id)
        {
            var menu = await _context.MenuSiteSettings.FindAsync(id);

            if (menu == null)
            {
                return NotFound(new { StatusCode = 404, Message = "منو پیدا نشد." });
            }

            return Ok(new { StatusCode = 200, Data = menu });
        }

        // POST: api/Menu
        [HttpPost]
        public async Task<ActionResult> CreateMenu([FromBody] MenuCreateDTO menuDTO)
        {
            var menu = new MenuSiteSettings
            {
                Title = menuDTO.Title,
                Link = menuDTO.Link,
                Order = menuDTO.Order
            };

            _context.MenuSiteSettings.Add(menu);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMenu), new { id = menu.Id }, 
                new { StatusCode = 201, Message = "منو با موفقیت ایجاد شد.", Data = menu });
        }

        // PUT: api/Menu/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu(int id, [FromBody] MenuUpdateDTO menuDTO)
        {
            var menu = await _context.MenuSiteSettings.FindAsync(id);
            if (menu == null)
            {
                return NotFound(new { StatusCode = 404, Message = "منو پیدا نشد." });
            }

            // Update only provided fields
            if (menuDTO.Title != null)
                menu.Title = menuDTO.Title;
            
            if (menuDTO.Link != null)
                menu.Link = menuDTO.Link;
            
            if (menuDTO.Order != null)
                menu.Order = menuDTO.Order;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { StatusCode = 200, Message = "منو با موفقیت به‌روزرسانی شد.", Data = menu });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(id))
                {
                    return NotFound(new { StatusCode = 404, Message = "منو پیدا نشد." });
                }
                throw;
            }
        }

        // POST: api/Menu/5/upload-icon
        [HttpPost("{id}/upload-icon")]
        public async Task<IActionResult> UploadIcon(int id, IFormFile file)
        {
            var menu = await _context.MenuSiteSettings.FindAsync(id);
            if (menu == null)
            {
                return NotFound(new { StatusCode = 404, Message = "منو پیدا نشد." });
            }

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
                // Delete old icon if exists
                if (!string.IsNullOrEmpty(menu.IconImagePath))
                {
                    string oldFileName = Path.GetFileName(menu.IconImagePath);
                    await _fileUploadService.DeleteFileAsync(oldFileName, "menus", id);
                }

                // Upload new icon
                string uploadedFilePath = await _fileUploadService.UploadFileAsync(file, "menus", id);
                menu.IconImagePath = uploadedFilePath;

                await _context.SaveChangesAsync();
                return Ok(new { StatusCode = 200, Message = "آیکون منو با موفقیت آپلود شد.", Data = menu });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading icon for menu {MenuId}", id);
                return StatusCode(500, new { StatusCode = 500, Message = "خطا در آپلود فایل." });
            }
        }

        // DELETE: api/Menu/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var menu = await _context.MenuSiteSettings.FindAsync(id);
            if (menu == null)
            {
                return NotFound(new { StatusCode = 404, Message = "منو پیدا نشد." });
            }

            try
            {
                // Delete icon file if exists
                if (!string.IsNullOrEmpty(menu.IconImagePath))
                {
                    try
                    {
                        string fileName = Path.GetFileName(menu.IconImagePath);
                        await _fileUploadService.DeleteFileAsync(fileName, "menus", id);

                        // Delete menu folder
                        string menuFolderPath = Path.Combine("wwwroot", "uploads", "menus", id.ToString());
                        if (Directory.Exists(menuFolderPath))
                        {
                            Directory.Delete(menuFolderPath, true);
                            _logger.LogInformation("Folder {MenuFolderPath} deleted successfully.", menuFolderPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error deleting icon file for menu {MenuId}", id);
                    }
                }

                // Delete menu from database
                _context.MenuSiteSettings.Remove(menu);
                await _context.SaveChangesAsync();

                return Ok(new { StatusCode = 200, Message = "منو و فایل‌های مربوطه با موفقیت حذف شدند." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu {MenuId}", id);
                return StatusCode(500, new { StatusCode = 500, Message = "خطای داخلی سرور.", Error = ex.Message });
            }
        }

        private bool MenuExists(int id)
        {
            return _context.MenuSiteSettings.Any(e => e.Id == id);
        }

    }
}
