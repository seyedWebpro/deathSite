using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Model.AdminModel;
using api.Services;
using deathSite.View.Menue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace deathSite.Controller.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        
        private readonly apiContext _context;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<MenuController> _logger;


        public MenuController(apiContext context, IFileUploadService fileUploadService, ILogger<MenuController> logger)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }



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

    // Update only if the field is not null and not empty string
    if (!string.IsNullOrWhiteSpace(menuDTO.Title))
    {
        menu.Title = menuDTO.Title;
    }
    
    if (!string.IsNullOrWhiteSpace(menuDTO.Link))
    {
        menu.Link = menuDTO.Link;
    }
    
    if (menuDTO.Order.HasValue)
    {
        menu.Order = menuDTO.Order;
    }

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
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error updating menu {MenuId}", id);
        return StatusCode(500, new { StatusCode = 500, Message = "خطای داخلی سرور.", Error = ex.Message });
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