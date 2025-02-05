
using api.Context;
using api.Model;
using api.Model.AdminModel;
using api.View.Tags;
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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] TagCreateDTO tagDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { StatusCode = 400, Message = "خطا در اعتبارسنجی داده‌ها." });
            }

            var tag = new Tag
            {
                Title = tagDto.Title,
                Type = tagDto.Type,
                Description = tagDto.Description
            };

            await _context.tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "برچسب با موفقیت ایجاد شد.", Data = tag });
        }

        [HttpGet("getByTitleAndType")]
        public async Task<IActionResult> GetByTitleAndType([FromQuery] string title, [FromQuery] string type)
        {
            var tag = await _context.tags.FirstOrDefaultAsync(t => t.Title == title && t.Type == type);
            if (tag == null)
            {
                return NotFound(new { StatusCode = 404, Message = "برچسب مورد نظر یافت نشد." });
            }

            return Ok(new { StatusCode = 200, Message = "برچسب دریافت شد.", Data = tag });
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _context.tags.ToListAsync();
            return Ok(new { StatusCode = 200, Message = "لیست برچسب‌ها دریافت شد.", Data = tags });
        }

        [HttpGet("getByType/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            var tags = await _context.tags.Where(t => t.Type == type).ToListAsync();
            return Ok(new { StatusCode = 200, Message = "برچسب‌های فیلتر شده دریافت شد.", Data = tags });
        }

        [HttpGet("getByTitle/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var tag = await _context.tags.FirstOrDefaultAsync(t => t.Title == title);
            if (tag == null)
            {
                return NotFound(new { StatusCode = 404, Message = "برچسب مورد نظر یافت نشد." });
            }

            return Ok(new { StatusCode = 200, Message = "برچسب دریافت شد.", Data = tag });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TagUpdateDTO tagDto)
        {
            var tag = await _context.tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound(new { StatusCode = 404, Message = "برچسب مورد نظر یافت نشد." });
            }

            if (!string.IsNullOrWhiteSpace(tagDto.Title))
            {
                tag.Title = tagDto.Title;
            }
            if (!string.IsNullOrWhiteSpace(tagDto.Type))
            {
                tag.Type = tagDto.Type;
            }
            if (!string.IsNullOrWhiteSpace(tagDto.Description))
            {
                tag.Description = tagDto.Description;
            }

            _context.tags.Update(tag);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "برچسب با موفقیت به‌روزرسانی شد.", Data = tag });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _context.tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound(new { StatusCode = 404, Message = "برچسب مورد نظر یافت نشد." });
            }

            _context.tags.Remove(tag);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "برچسب با موفقیت حذف شد." });
        }

        [HttpDelete("deleteByName/{tagName}")]
        public async Task<IActionResult> DeleteByName(string tagName)
        {
            // جستجوی تگ بر اساس نام
            var tag = await _context.tags
                .FirstOrDefaultAsync(t => t.Title == tagName);

            if (tag == null)
            {
                return NotFound(new { StatusCode = 404, Message = "برچسب مورد نظر یافت نشد." });
            }

            // حذف تگ
            _context.tags.Remove(tag);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "برچسب با موفقیت حذف شد." });
        }

        [HttpGet("getTagWithShahids/{title}")]
        public async Task<IActionResult> GetTagWithShahids(string title)
        {
            // جستجوی تگ با نام مشخص شده
            var tag = await _context.tags
                .Include(t => t.ShahidTags)
                .ThenInclude(st => st.Shahid)
                .FirstOrDefaultAsync(t => t.Title == title);

            if (tag == null)
            {
                return NotFound(new { StatusCode = 404, Message = "تگ مورد نظر یافت نشد." });
            }

            // آماده‌سازی نتیجه برای ارسال
            var result = new
            {
                tag.Id,
                tag.Title,
                tag.Type,
                tag.Description,
                Shahids = tag.ShahidTags.Select(st => new
                {
                    st.Shahid.Id,
                    st.Shahid.FullName,
                    st.Shahid.LastResponsibility,
                    st.Shahid.Biography,
                    st.Shahid.PhotoUrls
                }).ToList()
            };

            return Ok(new { StatusCode = 200, Message = "اطلاعات تگ و شهدای مرتبط دریافت شد.", Data = result });
        }

        [HttpPost("addTagToShahid")]
        public async Task<IActionResult> AddTagToShahid(int shahidId, int tagId)
        {
            // بررسی وجود شهید
            var shahid = await _context.shahids.FindAsync(shahidId);
            if (shahid == null)
            {
                return NotFound(new { StatusCode = 404, Message = "شهید مورد نظر یافت نشد." });
            }

            // بررسی وجود تگ
            var tag = await _context.tags.FindAsync(tagId);
            if (tag == null)
            {
                return NotFound(new { StatusCode = 404, Message = "تگ مورد نظر یافت نشد." });
            }

            // بررسی وجود رابطه تکراری
            var existingRelation = await _context.ShahidTags
                .FirstOrDefaultAsync(st => st.ShahidId == shahidId && st.TagId == tagId);
            if (existingRelation != null)
            {
                return BadRequest(new { StatusCode = 400, Message = "این تگ قبلاً به این شهید اضافه شده است." });
            }

            // ایجاد رابطه
            var shahidTag = new ShahidTag
            {
                ShahidId = shahidId,
                TagId = tagId
            };

            _context.ShahidTags.Add(shahidTag);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "تگ با موفقیت به شهید اضافه شد." });
        }

        [HttpDelete("removeTagFromShahid")]
        public async Task<IActionResult> RemoveTagFromShahid(int shahidId, int tagId)
        {
            var shahidTag = await _context.ShahidTags
                .FirstOrDefaultAsync(st => st.ShahidId == shahidId && st.TagId == tagId);

            if (shahidTag == null)
            {
                return NotFound(new { StatusCode = 404, Message = "رابطه تگ و شهید یافت نشد." });
            }

            _context.ShahidTags.Remove(shahidTag);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "تگ با موفقیت از شهید حذف شد." });
        }

        [HttpDelete("removeTagFromShahidByName")]
        public async Task<IActionResult> RemoveTagFromShahidByName(int shahidId, string tagName)
        {
            // جستجوی تگ بر اساس نام
            var tag = await _context.tags
                .FirstOrDefaultAsync(t => t.Title == tagName);

            if (tag == null)
            {
                return NotFound(new { StatusCode = 404, Message = "تگ یافت نشد." });
            }

            // جستجوی رابطه‌ی بین تگ و شهید
            var shahidTag = await _context.ShahidTags
                .FirstOrDefaultAsync(st => st.ShahidId == shahidId && st.TagId == tag.Id);

            if (shahidTag == null)
            {
                return NotFound(new { StatusCode = 404, Message = "رابطه تگ و شهید یافت نشد." });
            }

            // حذف رابطه‌ی بین تگ و شهید
            _context.ShahidTags.Remove(shahidTag);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "تگ با موفقیت از شهید حذف شد." });
        }

        [HttpGet("getTagsForShahid/{shahidId}")]
        public async Task<IActionResult> GetTagsForShahid(int shahidId)
        {
            var shahid = await _context.shahids
                .Include(s => s.ShahidTags)
                .ThenInclude(st => st.Tag)
                .FirstOrDefaultAsync(s => s.Id == shahidId);

            if (shahid == null)
            {
                return NotFound(new { StatusCode = 404, Message = "شهید مورد نظر یافت نشد." });
            }

            var tags = shahid.ShahidTags.Select(st => new
            {
                st.Tag.Id,
                st.Tag.Title,
                st.Tag.Description
            }).ToList();

            return Ok(new { StatusCode = 200, Message = "تگ‌های شهید دریافت شد.", Data = tags });
        }

        [HttpDelete("removeAllTagsFromShahid/{shahidId}")]
        public async Task<IActionResult> RemoveAllTagsFromShahid(int shahidId)
        {
            var shahidTags = _context.ShahidTags.Where(st => st.ShahidId == shahidId);

            if (!shahidTags.Any())
            {
                return NotFound(new { StatusCode = 404, Message = "هیچ تگی برای این شهید یافت نشد." });
            }

            _context.ShahidTags.RemoveRange(shahidTags);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "تمام تگ‌ها با موفقیت از شهید حذف شدند." });
        }

    }
}
