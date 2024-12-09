using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Middleware;
using api.Model;
using api.Services;
using api.View.ShahidManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShahidController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly apiContext _context;

        public ShahidController(IFileUploadService fileUploadService, apiContext context)
        {
            _fileUploadService = fileUploadService;
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ShahidView shahidView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { StatusCode = 400, Message = "خطا در اعتبارسنجی داده‌ها." });
            }

            var shahid = new Shahid
            {
                FullName = shahidView.FullName,
                FatherName = shahidView.FatherName,
                BirthBorn = shahidView.BirthBorn,
                BirthDead = shahidView.BirthDead,
                PlaceDead = shahidView.PlaceDead,
                Responsibilities = shahidView.Responsibilities,
                Operations = shahidView.Operations,
                Biography = shahidView.Biography,
                Will = shahidView.Will,
                Memories = shahidView.Memories,
                CauseOfMartyrdom = shahidView.CauseOfMartyrdom,
                LastResponsibility = shahidView.LastResponsibility,
                Gorooh = shahidView.Gorooh,
                Yegan = shahidView.Yegan,
                Niru = shahidView.Niru,
                Ghesmat = shahidView.Ghesmat,
                Poem = shahidView.Poem,
                // فیلدهای URL خالی هستند
                PhotoUrls = new List<string>(), // خالی
                VideoUrls = new List<string>(), // خالی
                VoiceUrls = new List<string>()   // خالی
            };

            await _context.shahids.AddAsync(shahid);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = shahid.Id }, new
            {
                StatusCode = 201,
                Message = "شهادت با موفقیت ایجاد شد.",
                Data = shahidView
            });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ShahidView shahidView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { StatusCode = 400, Message = "خطا در اعتبارسنجی داده‌ها." });
            }

            var shahid = await _context.shahids.FindAsync(id);
            if (shahid == null)
            {
                return NotFound(new { StatusCode = 404, Message = "شهید پیدا نشد." });
            }

            // به‌روزرسانی اطلاعات
            shahid.FullName = shahidView.FullName;
            shahid.FatherName = shahidView.FatherName;
            shahid.BirthBorn = shahidView.BirthBorn;
            shahid.BirthDead = shahidView.BirthDead;
            shahid.PlaceDead = shahidView.PlaceDead;
            shahid.Responsibilities = shahidView.Responsibilities;
            shahid.Operations = shahidView.Operations;
            shahid.Biography = shahidView.Biography;
            shahid.Will = shahidView.Will;
            shahid.Memories = shahidView.Memories;
            shahid.CauseOfMartyrdom = shahidView.CauseOfMartyrdom;
            shahid.LastResponsibility = shahidView.LastResponsibility;
            shahid.Gorooh = shahidView.Gorooh;
            shahid.Yegan = shahidView.Yegan;
            shahid.Niru = shahidView.Niru;
            shahid.Ghesmat = shahidView.Ghesmat;
            shahid.Poem = shahidView.Poem;

            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "اطلاعات شهید با موفقیت به‌روزرسانی شد." });
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var shahids = await _context.shahids.ToListAsync();
            return Ok(new
            {
                StatusCode = 200,
                Data = shahids
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shahid = await _context.shahids.FindAsync(id);
            if (shahid == null)
            {
                return NotFound(new { StatusCode = 404, Message = "شهید پیدا نشد." });
            }

            return Ok(new
            {
                StatusCode = 200,
                Data = shahid
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var shahid = await _context.shahids.FindAsync(id);
            if (shahid == null)
            {
                return NotFound(new { StatusCode = 404, Message = "شهید پیدا نشد." });
            }

            _context.shahids.Remove(shahid);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "شهید با موفقیت حذف شد." });
        }

        [HttpPost("{id}/upload-files")]
public async Task<IActionResult> UploadFiles(int id, List<IFormFile> files)
{
    var shahid = await _context.shahids.FindAsync(id);
    if (shahid == null)
    {
        return NotFound(new { StatusCode = 404, Message = "شهید پیدا نشد." });
    }

    // بارگذاری فایل‌ها
    foreach (var file in files)
    {
        if (file != null && file.Length > 0)
        {
            var fileUrl = await _fileUploadService.UploadFileAsync(file);
            if (file.ContentType.StartsWith("image/"))
            {
                // اضافه کردن URL عکس به لیست
                shahid.PhotoUrls.Add(fileUrl);
            }
            else if (file.ContentType.StartsWith("video/"))
            {
                // اضافه کردن URL ویدیو به لیست
                shahid.VideoUrls.Add(fileUrl);
            }
            else if (file.ContentType.StartsWith("audio/"))
            {
                // اضافه کردن URL صدا به لیست
                shahid.VoiceUrls.Add(fileUrl);
            }
        }
    }

    await _context.SaveChangesAsync();

    return Ok(new { StatusCode = 200, Message = "فایل‌ها با موفقیت بارگذاری شدند." });
}


        [HttpGet("{id}/files")]
        public async Task<IActionResult> GetFilesByShahidId(int id)
        {
            var shahid = await _context.shahids.FindAsync(id);
            if (shahid == null)
            {
                return NotFound(new { StatusCode = 404, Message = "شهید پیدا نشد." });
            }

            return Ok(new
            {
                StatusCode = 200,
                Data = new
                {
                    Photos = shahid.PhotoUrls,
                    Videos = shahid.VideoUrls,
                    Voices = shahid.VoiceUrls
                }
            });
        }

        [HttpDelete("{id}/clear-files")]
        public async Task<IActionResult> ClearFiles(int id)
        {
            var shahid = await _context.shahids.FindAsync(id);
            if (shahid == null)
            {
                return NotFound(new { StatusCode = 404, Message = "شهید پیدا نشد." });
            }

            // حذف فایل‌ها از سرور
            var allFileUrls = shahid.PhotoUrls.Concat(shahid.VideoUrls).Concat(shahid.VoiceUrls).ToList();
            foreach (var fileUrl in allFileUrls)
            {
                await _fileUploadService.DeleteFileAsync(Path.GetFileName(fileUrl));
            }

            // پاک کردن لیست فایل‌ها
            shahid.PhotoUrls.Clear();
            shahid.VideoUrls.Clear();
            shahid.VoiceUrls.Clear();

            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "تمامی فایل‌ها با موفقیت حذف شدند." });
        }

    }
}