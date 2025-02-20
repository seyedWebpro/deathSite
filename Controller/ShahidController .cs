using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Context;
using api.Middleware;
using api.Model;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShahidController : ControllerBase
    {
        private readonly ILogger<ShahidController> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly apiContext _context;
        private readonly IConfiguration _configuration;


        public ShahidController(ILogger<ShahidController> logger, IFileUploadService fileUploadService, apiContext context, IConfiguration configuration)
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
            _context = context;
            _configuration = configuration;
        }



        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ShahidView shahidView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { StatusCode = 400, Message = "خطا در اعتبارسنجی داده‌ها." });
            }

            // استخراج توکن از هدر Authorization
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
            }

            try
            {
                // تجزیه توکن و استخراج اطلاعات کاربر
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // استخراج اطلاعات کاربر از توکن
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

                if (userIdClaim == null || roleClaim == null)
                {
                    return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
                }

                var userId = int.Parse(userIdClaim);
                var role = roleClaim;

                // ایجاد رکورد شهید و ارتباط آن با کاربر
                var shahid = new Shahid
                {
                    FullName = shahidView.FullName,
                    FatherName = shahidView.FatherName,
                    BirthBorn = shahidView.BirthBorn,
                    BirthDead = shahidView.BirthDead,
                    PlaceDead = shahidView.PlaceDead,
                    PlaceOfBurial = shahidView.PlaceOfBurial,
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
                    PoemVerseTwo = shahidView.PoemVerseTwo,
                    UserId = userId // استفاده از userId استخراج شده از توکن
                };

                await _context.shahids.AddAsync(shahid);
                await _context.SaveChangesAsync();

                // بازگشت داده‌های شهید به همراه اطلاعات کاربر (UserId و role)
                var result = new
                {
                    StatusCode = 200,
                    Message = "شهید با موفقیت ایجاد شد.",
                    Data = new
                    {
                        id = shahid.Id,
                        UserId = shahid.UserId,  // بازگرداندن UserId
                        Role = role  // بازگرداندن role
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
            }
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

        [HttpGet("{id}/user")]
        public async Task<IActionResult> GetByIdForUser(int id)
        {
            // استخراج توکن از هدر Authorization
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
            }

            try
            {
                // تجزیه توکن و استخراج اطلاعات کاربر
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // استخراج UserId از توکن
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var userId = int.Parse(userIdClaim);

                // پیدا کردن شهید با Id مربوطه
                var shahid = await _context.shahids.FindAsync(id);
                if (shahid == null)
                {
                    return NotFound(new { StatusCode = 404, Message = "شهید پیدا نشد." });
                }

                // بررسی اینکه آیا شهید متعلق به کاربر جاری است
                if (shahid.UserId != userId)
                {
                    return Unauthorized(new { StatusCode = 401, Message = "شما اجازه دسترسی به این شهید را ندارید." });
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
            catch (Exception)
            {
                return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllByUser(int userId)
        {
            // استخراج توکن از هدر Authorization
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
            }

            try
            {
                // تجزیه توکن و استخراج اطلاعات کاربر
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // استخراج UserId از توکن
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var extractedUserId = int.Parse(userIdClaim);

                // بررسی اینکه آیا UserId از توکن با userId که به عنوان پارامتر آمده یکسان است
                if (userId != extractedUserId)
                {
                    return Unauthorized(new { StatusCode = 401, Message = "شما اجازه دسترسی به این اطلاعات را ندارید." });
                }

                var shahids = await _context.shahids
                    .Where(s => s.UserId == userId)
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
                    return NotFound(new { StatusCode = 404, Message = "هیچ شهیدی برای این کاربر پیدا نشد." });
                }

                return Ok(new
                {
                    StatusCode = 200,
                    Data = shahids
                });
            }
            catch (Exception)
            {
                return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
            }
        }



        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetById(int id)
        // {
        //     var shahid = await _context.shahids.FindAsync(id);
        //     if (shahid == null)
        //     {
        //         return NotFound(new { StatusCode = 404, Message = "شهید پیدا نشد." });
        //     }

        //     var result = new
        //     {
        //         Id = shahid.Id,
        //         FullName = shahid.FullName,
        //         FatherName = shahid.FatherName,
        //         BirthBorn = shahid.BirthBorn,
        //         BirthDead = shahid.BirthDead,
        //         PlaceDead = shahid.PlaceDead,
        //         PlaceOfBurial = shahid.PlaceOfBurial,
        //         BurialSiteLink = shahid.BurialSiteLink,
        //         MediaLink = shahid.MediaLink,
        //         DeadPlaceLink = shahid.DeadPlaceLink,
        //         virtualLink = shahid.virtualLink,
        //         Responsibilities = shahid.Responsibilities,
        //         Operations = shahid.Operations,
        //         Biography = shahid.Biography,
        //         Will = shahid.Will,
        //         Memories = shahid.Memories,
        //         PhotoUrls = shahid.PhotoUrls,
        //         VideoUrls = shahid.VideoUrls,
        //         VoiceUrls = shahid.VoiceUrls,
        //         CauseOfMartyrdom = shahid.CauseOfMartyrdom,
        //         LastResponsibility = shahid.LastResponsibility,
        //         Gorooh = shahid.Gorooh,
        //         Yegan = shahid.Yegan,
        //         Niru = shahid.Niru,
        //         Ghesmat = shahid.Ghesmat,
        //         PoemVerseOne = shahid.PoemVerseOne,
        //         PoemVerseTwo = shahid.PoemVerseTwo
        //     };

        //     return Ok(new
        //     {
        //         StatusCode = 200,
        //         Data = result
        //     });
        // }

        [HttpPost("{id}/upload-files")]
        public async Task<IActionResult> UploadFiles(int id, List<IFormFile> files)
        {
            // استخراج توکن از هدر Authorization
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
            }

            try
            {
                // تجزیه توکن و استخراج اطلاعات کاربر
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // استخراج UserId از توکن
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var userId = int.Parse(userIdClaim);

                // پیدا کردن شهید با Id مربوطه
                var shahid = await _context.shahids.FindAsync(id);
                if (shahid == null)
                {
                    return NotFound(new { StatusCode = 404, Message = "شهید پیدا نشد." });
                }

                // بررسی اینکه آیا شهید متعلق به کاربر جاری است
                if (shahid.UserId != userId)
                {
                    return Unauthorized(new { StatusCode = 401, Message = "شما اجازه بارگذاری فایل برای این شهید را ندارید." });
                }

                // بارگذاری فایل‌ها
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
            catch (Exception ex)
            {
                return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
            }
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
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
            }

            try
            {
                // تجزیه توکن و استخراج اطلاعات کاربر
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (userIdClaim == null)
                {
                    return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
                }

                var userId = int.Parse(userIdClaim);

                var shahid = await _context.shahids.FindAsync(id);
                if (shahid == null)
                {
                    _logger.LogWarning("Shahid with id {ShahidId} not found.", id);
                    return NotFound(new { StatusCode = 404, Message = "شهید پیدا نشد." });
                }

                // بررسی اینکه آیا شهید متعلق به کاربر است
                if (shahid.UserId != userId)
                {
                    return Unauthorized(new { StatusCode = 401, Message = "شما مجاز به حذف این شهید نیستید." });
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
                        Directory.Delete(shahidFolderPath, true); // به صورت بازگشتی پوشه‌ها را حذف می‌کند
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

        /// <summary>
        /// دریافت شهدا بر اساس روز، ماه و سال شهادت از تاریخ UTC ورودی
        /// </summary>
        /// <param name="utcDate">تاریخ به صورت UTC (مثلاً 2025-02-16)</param>
        /// <returns>لیستی از شهدا شامل آی‌دی، نام و آدرس عکس (در صورت وجود)</returns>
        [HttpGet("martyrs-by-date")]
        public IActionResult GetMartyrsByDate([FromQuery] DateTime utcDate)
        {
            try
            {
                // تبدیل تاریخ ورودی به DateOnly جهت مقایسه روز، ماه و سال
                var targetDate = DateOnly.FromDateTime(utcDate);
                _logger.LogInformation("شروع جستجو برای شهدا با روز {Day}، ماه {Month} و سال {Year} از تاریخ ورودی {Date}.",
                    targetDate.Day, targetDate.Month, targetDate.Year, targetDate);

                // فیلتر شهدا بر اساس تطابق روز، ماه و سال در تاریخ شهادت (BirthDead)
                // ابتدا فیلتر در پایگاه داده و سپس اعمال عملیات مربوط به لیست‌های داخلی در حافظه
                var martyrs = _context.shahids
                    .Where(s => s.BirthDead.Day == targetDate.Day && s.BirthDead.Month == targetDate.Month && s.BirthDead.Year == targetDate.Year)
                    .AsEnumerable() // انتقال داده‌ها به حافظه برای انجام عملیات پیچیده‌تر روی لیست‌ها
                    .Select(s => new
                    {
                        s.Id,
                        s.FullName,
                        // اگر لیست عکس‌ها وجود داشته باشد و خالی نباشد، اولین آدرس عکس را برمی‌گردانیم؛ در غیر این صورت رشته خالی
                        PhotoUrl = (s.PhotoUrls != null && s.PhotoUrls.Any() ? s.PhotoUrls.First() : string.Empty)
                    })
                    .ToList();

                _logger.LogInformation("تعداد {Count} شهید برای تاریخ {Date} یافت شد.", martyrs.Count, targetDate);
                return Ok(martyrs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت شهدا برای تاریخ {Date}", utcDate);
                return StatusCode(500, "خطای سروری داخلی");
            }
        }

    }
}