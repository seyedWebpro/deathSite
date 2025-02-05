using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Middleware;
using api.Model;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShahidController : ControllerBase
    {
        private readonly ILogger<ShahidController> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly apiContext _context;

        public ShahidController(ILogger<ShahidController> logger, IFileUploadService fileUploadService, apiContext context)
        {
            _logger = logger;
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
                PlaceOfBurial = shahidView.PlaceOfBurial, // اضافه کردن محل دفن
                BurialSiteLink = shahidView.BurialSiteLink,
                MediaLink = shahidView.MediaLink,
                DeadPlaceLink = shahidView.DeadPlaceLink,
                virtualLink = shahidView.virtualLink,
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
                PoemVerseOne = shahidView.PoemVerseOne,
                PoemVerseTwo = shahidView.PoemVerseTwo
            };

            await _context.shahids.AddAsync(shahid);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = shahid.Id }, new
            {
                StatusCode = 200,
                Message = "شهادت با موفقیت ایجاد شد.",
                Data = shahid // بازگرداندن داده اصلی همراه با مقدار Id
            });
        }



        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var shahids = await _context.shahids
                .Select(shahid => new
                {
                    Id = shahid.Id,
                    FullName = shahid.FullName,
                    FatherName = shahid.FatherName,
                    BirthBorn = shahid.BirthBorn,
                    BirthDead = shahid.BirthDead,
                    PlaceDead = shahid.PlaceDead,
                    PlaceOfBurial = shahid.PlaceOfBurial,
                    BurialSiteLink = shahid.BurialSiteLink,
                    MediaLink = shahid.MediaLink,
                    DeadPlaceLink = shahid.DeadPlaceLink,
                    virtualLink = shahid.virtualLink,
                    Responsibilities = shahid.Responsibilities,
                    Operations = shahid.Operations,
                    Biography = shahid.Biography,
                    Will = shahid.Will,
                    Memories = shahid.Memories,
                    PhotoUrls = shahid.PhotoUrls,
                    VideoUrls = shahid.VideoUrls,
                    VoiceUrls = shahid.VoiceUrls,
                    CauseOfMartyrdom = shahid.CauseOfMartyrdom,
                    LastResponsibility = shahid.LastResponsibility,
                    Gorooh = shahid.Gorooh,
                    Yegan = shahid.Yegan,
                    Niru = shahid.Niru,
                    Ghesmat = shahid.Ghesmat,
                    PoemVerseOne = shahid.PoemVerseOne,
                    PoemVerseTwo = shahid.PoemVerseTwo
                })
                .ToListAsync();

            if (!shahids.Any())
            {
                return NotFound(new { StatusCode = 404, Message = "هیچ شهیدی پیدا نشد." });
            }

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

            var result = new
            {
                Id = shahid.Id,
                FullName = shahid.FullName,
                FatherName = shahid.FatherName,
                BirthBorn = shahid.BirthBorn,
                BirthDead = shahid.BirthDead,
                PlaceDead = shahid.PlaceDead,
                PlaceOfBurial = shahid.PlaceOfBurial,
                BurialSiteLink = shahid.BurialSiteLink,
                MediaLink = shahid.MediaLink,
                DeadPlaceLink = shahid.DeadPlaceLink,
                virtualLink = shahid.virtualLink,
                Responsibilities = shahid.Responsibilities,
                Operations = shahid.Operations,
                Biography = shahid.Biography,
                Will = shahid.Will,
                Memories = shahid.Memories,
                PhotoUrls = shahid.PhotoUrls,
                VideoUrls = shahid.VideoUrls,
                VoiceUrls = shahid.VoiceUrls,
                CauseOfMartyrdom = shahid.CauseOfMartyrdom,
                LastResponsibility = shahid.LastResponsibility,
                Gorooh = shahid.Gorooh,
                Yegan = shahid.Yegan,
                Niru = shahid.Niru,
                Ghesmat = shahid.Ghesmat,
                PoemVerseOne = shahid.PoemVerseOne,
                PoemVerseTwo = shahid.PoemVerseTwo
            };

            return Ok(new
            {
                StatusCode = 200,
                Data = result
            });
        }

        [HttpPost("{id}/upload-files")]
        public async Task<IActionResult> UploadFiles(int id, List<IFormFile> files)
        {
            var shahid = await _context.shahids.FindAsync(id);
            if (shahid == null)
            {
                return NotFound(new { StatusCode = 404, Message = "شهید پیدا نشد." });
            }

            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    // تعیین دسته بندی فایل
                    string category = file.ContentType.Split('/')[0];
                    string uploadedFilePath = await _fileUploadService.UploadFileAsync(file, "shahids", id);

                    // افزودن مسیر فایل به لیست مرتبط
                    switch (category)
                    {
                        case "image":
                            shahid.PhotoUrls.Add(uploadedFilePath);
                            break;
                        case "video":
                            shahid.VideoUrls.Add(uploadedFilePath);
                            break;
                        case "audio":
                            shahid.VoiceUrls.Add(uploadedFilePath);
                            break;
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

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var shahid = await _context.shahids.FindAsync(id);
                if (shahid == null)
                {
                    _logger.LogWarning("Shahid with id {ShahidId} not found.", id);
                    return NotFound(new { StatusCode = 404, Message = "شهید پیدا نشد." });
                }

                var allFileUrls = shahid.PhotoUrls.Concat(shahid.VideoUrls).Concat(shahid.VoiceUrls).ToList();
                bool anyFileDeleted = false;

                foreach (var fileUrl in allFileUrls)
                {
                    try
                    {
                        string fileName = Path.GetFileName(fileUrl);
                        await _fileUploadService.DeleteFileAsync(fileName, "shahids", id);
                        anyFileDeleted = true;
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

                if (!anyFileDeleted)
                {
                    _logger.LogWarning("No files were deleted for Shahid with id {ShahidId}. The shahid will not be deleted.", id);
                    return BadRequest(new { StatusCode = 400, Message = "هیچ فایلی حذف نشد. شهید حذف نخواهد شد." });
                }

                // حذف پوشه مربوط به شهید
                string shahidFolderPath = Path.Combine("wwwroot", "uploads", "shahids", id.ToString());
                if (Directory.Exists(shahidFolderPath))
                {
                    try
                    {
                        Directory.Delete(shahidFolderPath, true);
                        _logger.LogInformation("Folder {ShahidFolderPath} deleted successfully.", shahidFolderPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error deleting folder {ShahidFolderPath}", shahidFolderPath);
                    }
                }

                // حذف شهید از دیتابیس
                _context.shahids.Remove(shahid);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Shahid with id {ShahidId} and associated files were successfully deleted.", id);
                return Ok(new { StatusCode = 200, Message = "شهید و فایل‌های مربوطه حذف شدند." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting shahid with id {ShahidId}.", id);
                return StatusCode(500, new { StatusCode = 500, Message = "خطای داخلی سرور.", Error = ex.Message });
            }
        }

        


    }
}