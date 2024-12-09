using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Middleware;
using api.Model.AdminModel;
using api.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly apiContext _context;

        public TagController(apiContext context)
        {
            _context = context;
        }

        // متد برای اضافه کردن برچسب
        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] TagView tagDto)
        {
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            var tag = new Tag
            {
                Name = tagDto.Name
            };

            _context.tags.Add(tag);
            await _context.SaveChangesAsync();

            var tagDtoResponse = new TagView
            {
                Name = tag.Name
            };

            return Ok(new { StatusCode = 201, Message = "برچسب با موفقیت ایجاد شد.", Data = tagDtoResponse });
        }

        // متد برای دریافت همه تگ‌ها
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _context.tags.ToListAsync();
            return Ok(new { StatusCode = 200, Data = tags });
        }

        // متد برای دریافت تگ بر اساس ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById(int id)
        {
            var tag = await _context.tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound(new { StatusCode = 404, Message = "تگ مورد نظر پیدا نشد." });
            }

            return Ok(new { StatusCode = 200, Data = tag });
        }

        // متد برای حذف تگ
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound(new { StatusCode = 404, Message = "تگ مورد نظر پیدا نشد." });
            }

            _context.tags.Remove(tag);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "تگ با موفقیت حذف شد." });
        }

    }
}