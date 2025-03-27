using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
    public class ElamiehController : ControllerBase
    {

        private readonly apiContext _context;
        private readonly ILogger<ElamiehController> _logger;
        private readonly IFileUploadService _fileUploadService;

        public ElamiehController(apiContext context, ILogger<ElamiehController> logger, IFileUploadService fileUploadService)
        {
            _context = context;
            _logger = logger;
            _fileUploadService = fileUploadService;
        }


    //     [HttpPost("{deceasedId}/add")]
    //     public async Task<IActionResult> AddElamiehAsync(int deceasedId,
    // [FromForm] List<IFormFile>? photos)
    //     {
    //         // بررسی احراز هویت کاربر
    //         var currentUserId = GetCurrentUserId();
    //         if (currentUserId == 0)
    //         {
    //             _logger.LogWarning("Unauthorized access attempt detected.");
    //             return Unauthorized(new { message = "توکن معتبر نیست.", statusCode = 401 });
    //         }

    //         // بررسی وجود متوفی
    //         var deceased = await _context.Deceaseds.FindAsync(deceasedId);
    //         if (deceased == null)
    //         {
    //             return NotFound("متوفی یافت نشد.");
    //         }

    //         // بررسی مالکیت کاربر
    //         if (deceased.UserId != currentUserId)
    //         {
    //             _logger.LogWarning("User with id {UserId} is not authorized to add an Elamieh for this record.", currentUserId);
    //             return Unauthorized(new { Message = "شما مجاز به افزودن اعلامیه برای این رکورد نیستید." });
    //         }

    //         // آپلود عکس‌های اعلامیه
    //         List<string> uploadedPhotoUrls = new();
    //         if (photos != null && photos.Count > 0)
    //         {
    //             uploadedPhotoUrls.AddRange(await _fileUploadService.UploadMultipleFilesAsync(photos, "elamieh", deceasedId));
    //         }

    //         // ایجاد شی جدید از اعلامیه
    //         var elamieh = new Elamieh
    //         {
    //             DeceasedId = deceasedId,
    //             UserId = currentUserId,
    //             PhotoUrls = uploadedPhotoUrls
    //         };

    //         // افزودن به دیتابیس و ذخیره تغییرات
    //         _context.Elamiehs.Add(elamieh);
    //         await _context.SaveChangesAsync();

    //         return Ok(new { Message = "اعلامیه با موفقیت ایجاد شد.", ElamiehId = elamieh.Id, PhotoUrls = uploadedPhotoUrls });
    //     }

    // حذف مقادیر قبلی و جایگزینی مقادیر جدید 

    [HttpPost("{deceasedId}/add")]
public async Task<IActionResult> AddElamiehAsync(int deceasedId, [FromForm] List<IFormFile>? photos)
{
    // بررسی احراز هویت کاربر
    var currentUserId = GetCurrentUserId();
    if (currentUserId == 0)
    {
        _logger.LogWarning("Unauthorized access attempt detected.");
        return Unauthorized(new { message = "توکن معتبر نیست.", statusCode = 401 });
    }

    // بررسی وجود متوفی
    var deceased = await _context.Deceaseds.FindAsync(deceasedId);
    if (deceased == null)
    {
        return NotFound("متوفی یافت نشد.");
    }

    // بررسی مالکیت کاربر
    if (deceased.UserId != currentUserId)
    {
        _logger.LogWarning("User with id {UserId} is not authorized to add an Elamieh for this record.", currentUserId);
        return Unauthorized(new { Message = "شما مجاز به افزودن اعلامیه برای این رکورد نیستید." });
    }

    // حذف اعلامیه‌های قبلی و عکس‌های آن
    var previousElamiehs = await _context.Elamiehs.Where(e => e.DeceasedId == deceasedId).ToListAsync();
    foreach (var previousElamieh in previousElamiehs)
    {
        // حذف فایل‌های عکس اعلامیه قبلی
        if (previousElamieh.PhotoUrls != null)
        {
            foreach (var photoUrl in previousElamieh.PhotoUrls)
            {
                var fileName = Path.GetFileName(photoUrl);
                await _fileUploadService.DeleteFileAsync(fileName, "elamieh", deceasedId);
            }
        }

        // حذف اعلامیه از دیتابیس
        _context.Elamiehs.Remove(previousElamieh);
    }

    // آپلود عکس‌های جدید اعلامیه
    List<string> uploadedPhotoUrls = new();
    if (photos != null && photos.Count > 0)
    {
        uploadedPhotoUrls.AddRange(await _fileUploadService.UploadMultipleFilesAsync(photos, "elamieh", deceasedId));
    }

    // ایجاد شی جدید از اعلامیه
    var newElamieh = new Elamieh
    {
        DeceasedId = deceasedId,
        UserId = currentUserId,
        PhotoUrls = uploadedPhotoUrls
    };

    // افزودن به دیتابیس و ذخیره تغییرات
    _context.Elamiehs.Add(newElamieh);
    await _context.SaveChangesAsync();

    return Ok(new { Message = "اعلامیه با موفقیت ایجاد شد.", ElamiehId = newElamieh.Id, PhotoUrls = uploadedPhotoUrls });
}


        private int GetCurrentUserId()
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Authorization token is missing.");
                    return 0;
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (userIdClaim == null)
                {
                    _logger.LogWarning("UserId claim not found in token.");
                    return 0;
                }

                return int.Parse(userIdClaim);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while extracting user ID from token.");
                return 0;
            }
        }


       [HttpGet("{id}")]
public async Task<IActionResult> GetElamieh(int id)
{
    var elamieh = await _context.Elamiehs.FindAsync(id);
    if (elamieh == null)
    {
        return NotFound(new { Message = "اعلامیه یافت نشد." });
    }

    return Ok(new
    {
        ElamiehId = elamieh.Id,
        DeceasedId = elamieh.DeceasedId,
        UserId = elamieh.UserId,
        CreatedAt = elamieh.CreatedAt,
        PhotoUrls = elamieh.PhotoUrls
    });
}

[HttpGet("deceased/{deceasedId}/elamiehs")]
public async Task<IActionResult> GetElamiehsByDeceasedId(int deceasedId)
{
    var elamiehs = await _context.Elamiehs
        .Where(e => e.DeceasedId == deceasedId)
        .Select(e => new
        {
            ElamiehId = e.Id,
            DeceasedId = e.DeceasedId,
            UserId = e.UserId,
            CreatedAt = e.CreatedAt,
            PhotoUrls = e.PhotoUrls
        })
        .ToListAsync();

    return Ok(elamiehs);
}

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteElamieh(int id)
        {
            try
            {
                var elamieh = await _context.Elamiehs.FindAsync(id);
                if (elamieh == null)
                {
                    return NotFound(new { Message = "اعلامیه یافت نشد." });
                }

                // حذف فایل‌های مربوطه از سرور
                bool anyFileDeleted = false;
                foreach (var fileUrl in elamieh.PhotoUrls)
                {
                    try
                    {
                        string fileName = Path.GetFileName(fileUrl);
                        await _fileUploadService.DeleteFileAsync(fileName, "elamieh", elamieh.DeceasedId);
                        anyFileDeleted = true;
                    }
                    catch (FileNotFoundException ex)
                    {
                        _logger.LogWarning(ex, "فایل برای حذف یافت نشد: {FileUrl}", fileUrl);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "خطا در حذف فایل: {FileUrl}", fileUrl);
                    }
                }

                // حذف پوشه مربوط به اعلامیه
                string elamiehFolderPath = Path.Combine("wwwroot", "uploads", "elamieh", elamieh.DeceasedId.ToString());
                if (Directory.Exists(elamiehFolderPath))
                {
                    try
                    {
                        Directory.Delete(elamiehFolderPath, true); // حذف پوشه به صورت بازگشتی
                        _logger.LogInformation("پوشه {ElamiehFolderPath} با موفقیت حذف شد.", elamiehFolderPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "خطا در حذف پوشه {ElamiehFolderPath}", elamiehFolderPath);
                    }
                }

                // حذف اعلامیه از دیتابیس
                _context.Elamiehs.Remove(elamieh);
                await _context.SaveChangesAsync();

                _logger.LogInformation("اعلامیه با شناسه {ElamiehId} و فایل‌های مرتبط حذف شد.", id);
                return Ok(new { Message = "اعلامیه و فایل‌های مربوطه حذف شدند." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در حذف اعلامیه با شناسه {ElamiehId}.", id);
                return StatusCode(500, new { Message = "خطای داخلی سرور.", Error = ex.Message });
            }
        }


    }
}