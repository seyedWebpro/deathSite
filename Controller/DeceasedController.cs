using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using api.Context;
using api.Model;
using api.Services;
using deathSite.Model;
using deathSite.Services.QRCode;
using deathSite.View.Dead;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace deathSite.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeceasedController : ControllerBase
    {
        private readonly apiContext _context;
        private readonly ILogger<DeceasedController> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly IQRCodeService _qrCodeService;

        public DeceasedController(apiContext context, ILogger<DeceasedController> logger, IFileUploadService fileUploadService, IQRCodeService qrCodeService)
        {
            _context = context;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _qrCodeService = qrCodeService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveDeceased([FromBody] DeceasedDto deceasedDto)
        {
            try
            {
                // استخراج شناسه کاربر از توکن
                var currentUserId = GetCurrentUserId();
                if (currentUserId == 0)
                {
                    _logger.LogWarning("Unauthorized access attempt detected.");
                    return Unauthorized(new { message = "توکن معتبر نیست.", statusCode = 401 });
                }

                // پیدا کردن پکیج رایگان
                var freePackage = await _context.packages
                    .FirstOrDefaultAsync(p => p.IsFreePackage); // فرض می‌کنیم که یک پکیج رایگان وجود دارد

                if (freePackage == null)
                {
                    return BadRequest(new { message = "پکیج رایگان یافت نشد.", statusCode = 400 });
                }

                // ذخیره متوفی جدید
                var deceased = new Deceased
                {
                    FullName = deceasedDto.FullName,
                    DateOfMartyrdom = deceasedDto.DateOfMartyrdom,
                    Gender = deceasedDto.Gender,
                    PublishedDate = DateTime.Now,
                    BirthDate = DateTime.Now,
                    IsApproved = ApprovalStatus.Pending, // وضعیت پیش‌فرض
                    UserId = currentUserId // استفاده از شناسه کاربر استخراج شده از توکن
                };

                // افزودن پکیج رایگان به متوفی
                deceased.Packages.Add(new DeceasedPackage
                {
                    Package = freePackage,
                    Deceased = deceased,
                    ActivationDate = DateTime.Now,
                    IsActive = true,
                    IsFreePackage = true
                });

                _context.Deceaseds.Add(deceased);
                await _context.SaveChangesAsync();

                // ثبت QR Code
                string deceasedUrl = $"https://new.tarhimcode.ir/deceased/{deceased.Id}";
                byte[] qrCodeBytes = _qrCodeService.GenerateQRCode(deceasedUrl);
                string fileName = $"qrcode_{Guid.NewGuid()}.png";
                string qrCodeUrl = await _fileUploadService.SaveQRCodeAsync(qrCodeBytes, "deceased", deceased.Id, fileName);

                // ثبت QR Code در دیتابیس
                deceased.QRCodeUrl = qrCodeUrl;
                await _context.SaveChangesAsync();

                // ساختن DTO برای پاسخ
                var responseDto = new DeceasedResponseDto
                {
                    Id = deceased.Id,
                    FullName = deceased.FullName,
                    Gender = deceased.Gender,
                    DateOfMartyrdom = deceased.DateOfMartyrdom.Value,
                    PublishedDate = deceased.PublishedDate,
                    BirthDate = deceased.BirthDate,
                    IsApproved = deceased.IsApproved.ToString(),
                    QRCodeUrl = deceased.QRCodeUrl
                };

                return Ok(new
                {
                    message = "اطلاعات متوفی با موفقیت ذخیره شد.",
                    deceased = responseDto,
                    statusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving deceased.");
                return StatusCode(500, new { message = "خطا در ذخیره اطلاعات.", error = ex.Message, statusCode = 500 });
            }
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


        [HttpGet("user/{userId}/deceased")]
        public IActionResult GetDeceasedDetailsByUserId(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching all deceased details for user ID: {UserId}", userId);

                var deceasedDetails = _context.Deceaseds
                    .Where(d => d.UserId == userId)
                    .Include(d => d.DeceasedLocation)
                    .Include(d => d.DeathViews)
                    .Include(d => d.Elamiehs)
                    .Include(d => d.Packages)  // تغییر از UserPackages به Packages
                        .ThenInclude(dp => dp.Package)  // تغییر از UserPackages به DeceasedPackage
                    .Include(d => d.Sarbarg)
                    .Select(d => new
                    {
                        d.Id,
                        d.FullName,
                        d.PlaceOfMartyrdom,
                        d.DateOfMartyrdom,
                        d.DateOfBirth,
                        d.Gender,
                        d.Description,
                        d.Khaterat,
                        d.PhotoUrls,
                        d.VideoUrls,
                        d.VoiceUrls,
                        d.UserId,
                        d.PublishedDate,
                        d.BirthDate,
                        d.FatherName,
                        d.Job,
                        d.Tahsilat,
                        d.ChildrenNames,
                        d.HowDeath,
                        d.OstanMazar,
                        d.CityMazar,
                        d.Aramestan,
                        d.GhesteMazar,
                        d.RadifMazar,
                        d.NumberMazar,
                        d.CoverPhotoUrl,
                        d.Ghaleb,
                        d.PlaceBirth,
                        d.QRCodeUrl,
                        Status = d.IsApproved.ToString(), // نمایش وضعیت به‌صورت استرینگ
                        DeceasedLocation = d.DeceasedLocation != null ? new
                        {
                            d.DeceasedLocation.Id,
                            d.DeceasedLocation.Balad,
                            d.DeceasedLocation.Neshan,
                            d.DeceasedLocation.GoogleMap,
                            d.DeceasedLocation.Mokhtasat
                        } : null,
                        DeathViews = d.DeathViews.Count(),  // نمایش تعداد بازدیدها به عنوان عدد
                        Elamiehs = d.Elamiehs.Select(e => e.PhotoUrls).ToList(),
                        Packages = d.Packages.Select(dp => new  // تغییر از UserPackages به Packages
                        {
                            dp.Id,
                            dp.ActivationDate,
                            dp.ExpirationDate,
                            dp.IsActive,
                            dp.IsFreePackage,
                            Package = new
                            {
                                dp.Package.Id,
                                dp.Package.Name,
                                dp.Package.Duration,
                                dp.Package.Price,
                                dp.Package.RenewalFee,
                                dp.Package.ValidityPeriod,
                                dp.Package.ImageCount,
                                dp.Package.VideoCount,
                                dp.Package.NotificationCount,
                                dp.Package.AudioFileLimit,
                                dp.Package.BarcodeEnabled,
                                dp.Package.DisplayEnabled,
                                dp.Package.TemplateSelectionEnabled,
                                dp.Package.CondolenceMessageEnabled,
                                dp.Package.VisitorContentSubmissionEnabled,
                                dp.Package.LocationAndNavigationEnabled,
                                dp.Package.SharingEnabled,
                                dp.Package.File360DegreeEnabled,
                                dp.Package.UpdateCapabilityEnabled
                            }
                        }).ToList(),
                        Sarbarg = d.Sarbarg != null ? new
                        {
                            d.Sarbarg.Id,
                            d.Sarbarg.Title,
                            d.Sarbarg.Description
                        } : null
                    })
                    .ToList();

                if (!deceasedDetails.Any())
                {
                    _logger.LogWarning("No deceased records found for user ID: {UserId}", userId);
                    return NotFound(new { message = "هیچ متوفی برای این کاربر یافت نشد.", statusCode = 404 });
                }

                _logger.LogInformation("Successfully retrieved {Count} deceased records for user ID: {UserId}", deceasedDetails.Count, userId);
                return Ok(new { message = "تمامی اطلاعات متوفی‌ها بارگذاری شدند.", data = deceasedDetails, statusCode = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching deceased details for user ID: {UserId}. Error details: {ErrorDetails}", userId, ex.ToString());
                return StatusCode(500, new { message = "خطا در بارگذاری اطلاعات.", error = ex.Message, details = ex.ToString(), statusCode = 500 });
            }
        }


        // [HttpGet("deceased/{deceasedId}")]
        // public async Task<IActionResult> GetDeceasedDetailsByDeceasedId(int deceasedId)
        // {
        //     var deceased = await _context.Deceaseds
        //         .Include(d => d.User)  // بارگذاری اطلاعات کاربر
        //         .Include(d => d.Packages)  // بارگذاری پکیج‌های متوفی
        //         .ThenInclude(dp => dp.Package) // بارگذاری پکیج‌های مرتبط با متوفی
        //         .FirstOrDefaultAsync(d => d.Id == deceasedId);

        //     if (deceased == null)
        //     {
        //         return NotFound(new { message = "متوفی با شناسه وارد شده یافت نشد.", statusCode = 404 });
        //     }

        //     var result = new
        //     {
        //         deceased.FullName,
        //         deceased.PlaceOfMartyrdom,
        //         deceased.DateOfMartyrdom,
        //         deceased.Gender,
        //         deceased.Description,
        //         Packages = deceased.Packages.Select(dp => new
        //         {
        //             dp.Package.Name,
        //             dp.Package.Price,
        //             dp.Package.Duration,
        //             dp.ActivationDate,
        //             dp.IsActive,
        //             dp.IsFreePackage
        //         }).ToList()  // پکیج‌های متوفی
        //     };

        //     return Ok(result);
        // }

[HttpGet("deceased/{deceasedId}")]
public async Task<IActionResult> GetDeceasedDetailsByDeceasedId(int deceasedId)
{
    try
    {
        _logger.LogInformation("Fetching details for deceased ID: {DeceasedId}", deceasedId);

        var deceased = await _context.Deceaseds
            .Include(d => d.User)
            .Include(d => d.Packages).ThenInclude(dp => dp.Package)
            .Include(d => d.DeceasedLocation)
            .Include(d => d.DeathViews)
            .Include(d => d.Sarbarg)
            .FirstOrDefaultAsync(d => d.Id == deceasedId);

        if (deceased == null)
        {
            _logger.LogWarning("Deceased with ID {DeceasedId} not found.", deceasedId);
            return NotFound(new { message = "متوفی با شناسه وارد شده یافت نشد.", statusCode = 404 });
        }

        var result = new
        {
            deceased.FullName,
            deceased.PlaceOfMartyrdom,
            deceased.DateOfMartyrdom,
            deceased.DateOfBirth,
            deceased.Gender,
            deceased.Description,
            deceased.Khaterat,
            deceased.PhotoUrls,
            deceased.VideoUrls,
            deceased.VoiceUrls,
            deceased.PublishedDate,
            deceased.BirthDate,
            deceased.PlaceBirth,
            deceased.FatherName,
            deceased.Job,
            deceased.Tahsilat,
            deceased.ChildrenNames,
            deceased.HowDeath,
            deceased.OstanMazar,
            deceased.CityMazar,
            deceased.Aramestan,
            deceased.GhesteMazar,
            deceased.RadifMazar,
            deceased.NumberMazar,
            deceased.CoverPhotoUrl,
            deceased.Ghaleb,
            deceased.QRCodeUrl,
            Status = deceased.IsApproved.ToString(),

            // اطلاعات نویسنده متوفی
            User = new
            {
                deceased.User.Id,
                deceased.User.firstName,
                deceased.User.lastName,
                deceased.User.phoneNumber,
                deceased.User.Email,
                deceased.User.role
            },

            // اطلاعات مکان متوفی
            Location = deceased.DeceasedLocation != null ? new
            {
                deceased.DeceasedLocation.Balad,
                deceased.DeceasedLocation.Neshan,
                deceased.DeceasedLocation.GoogleMap,
                deceased.DeceasedLocation.Mokhtasat
            } : null,

            // اطلاعات بازدیدها
            ViewsCount = deceased.DeathViews.Count(),
            Views = deceased.DeathViews.Select(v => new
            {
                v.UserId,
                v.IPAddress,
                v.ViewDate
            }).ToList(),

            // اطلاعات پکیج‌های متوفی
            Packages = deceased.Packages.Select(dp => new
            {
                dp.Id,
                dp.ActivationDate,
                dp.ExpirationDate,
                dp.IsActive,
                dp.IsFreePackage,
                FactorId = dp.FactorId,
                Package = new
                {
                    dp.Package.Id,
                    dp.Package.Name,
                    dp.Package.Price,
                    dp.Package.Duration,
                    dp.Package.ValidityPeriod,
                    dp.Package.ImageCount,
                    dp.Package.VideoCount,
                    dp.Package.NotificationCount,
                    dp.Package.AudioFileLimit,
                    dp.Package.BarcodeEnabled,
                    dp.Package.DisplayEnabled,
                    dp.Package.TemplateSelectionEnabled,
                    dp.Package.CondolenceMessageEnabled,
                    dp.Package.VisitorContentSubmissionEnabled,
                    dp.Package.LocationAndNavigationEnabled,
                    dp.Package.SharingEnabled,
                    dp.Package.File360DegreeEnabled,
                    dp.Package.UpdateCapabilityEnabled
                }
            }).ToList(),

            // اطلاعات سربرگ
            Sarbarg = deceased.Sarbarg != null ? new
            {
                deceased.Sarbarg.Id,
                deceased.Sarbarg.Title,
                deceased.Sarbarg.Description
            } : null
        };

        _logger.LogInformation("Successfully retrieved details for deceased ID: {DeceasedId}", deceasedId);
        return Ok(new { message = "اطلاعات متوفی با موفقیت بارگذاری شد.", data = result, statusCode = 200 });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error occurred while fetching details for deceased ID: {DeceasedId}", deceasedId);
        return StatusCode(500, new { message = "خطا در بارگذاری اطلاعات.", error = ex.Message, details = ex.ToString(), statusCode = 500 });
    }
}



 [HttpGet("deceased")]
        public async Task<IActionResult> GetAllDeceased()
        {
            var deceasedList = await _context.Deceaseds
                .Include(d => d.User)  // بارگذاری اطلاعات کاربر
                .Include(d => d.Packages)  // بارگذاری پکیج‌های متوفی
                .ThenInclude(dp => dp.Package) // بارگذاری پکیج‌های مرتبط با متوفی
                .ToListAsync();

            if (deceasedList == null || !deceasedList.Any())
            {
                return NotFound();
            }

            var result = deceasedList.Select(d => new
            {
                d.Id,
                d.FullName,
                d.DateOfMartyrdom,
                d.Ghaleb,
                d.IsApproved,
                d.PublishedDate,
                Packages = d.Packages.Select(dp => new
                {
                    dp.Package.Name,
                    dp.Package.Price,
                    dp.Package.Duration,
                    dp.ActivationDate,
                    dp.IsActive,
                    dp.IsFreePackage
                }).ToList(),  // پکیج‌های متوفی
                User = new
                {
                    d.User.firstName,
                    d.User.lastName,
                    d.User.phoneNumber,
                    d.User.Email
                }
            }).ToList();

            return Ok(result);
        }

[HttpGet("deceasedAccepted")]
public async Task<IActionResult> GetAllDeceasedAccepted()
{
    var deceasedList = await _context.Deceaseds
        .Where(d => d.IsApproved == ApprovalStatus.Approved) // فقط متوفیان تایید شده
        .Select(d => new
        {
            d.Id,
            d.FullName,
            d.DateOfMartyrdom,
            d.Ghaleb,
            d.IsApproved,
            d.PublishedDate,
            d.CoverPhotoUrl,
            ViewCount = d.DeathViews.Count, // تعداد ویوها
            d.Description
        })
        .ToListAsync();

    if (deceasedList == null || !deceasedList.Any())
    {
        return NotFound();
    }

    return Ok(deceasedList);
}

        // تایید متوفی 

        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateApprovalStatus(int id, [FromBody] ApprovalStatus status)
        {
            var deceased = await _context.Deceaseds.FindAsync(id);
            if (deceased == null)
            {
                return NotFound(new { message = "متوفی مورد نظر یافت نشد.", statusCode = 404 });
            }

            deceased.IsApproved = status;
            _context.Deceaseds.Update(deceased);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "وضعیت تأیید متوفی با موفقیت تغییر یافت.",
                statusCode = 200,
                newStatus = deceased.IsApproved
            });
        }

        [HttpGet("status/{id}")]
        public async Task<IActionResult> GetApprovalStatus(int id)
        {
            var deceased = await _context.Deceaseds
                .Where(d => d.Id == id)
                .Select(d => new
                {
                    d.Id,
                    d.FullName,
                    Status = d.IsApproved.ToString() // تبدیل مقدار Enum به رشته
                })
                .FirstOrDefaultAsync();

            if (deceased == null)
            {
                return NotFound(new { message = "متوفی مورد نظر یافت نشد.", statusCode = 404 });
            }

            return Ok(new
            {
                message = "وضعیت تأیید متوفی دریافت شد.",
                statusCode = 200,
                deceased
            });
        }


        [HttpGet("qrcode/{id}")]
        public IActionResult GetQRCode(int id)
        {
            // پیدا کردن متوفی با استفاده از ID
            var deceased = _context.Deceaseds.Find(id);
            if (deceased == null || string.IsNullOrEmpty(deceased.QRCodeUrl))
                return NotFound();

            // ساخت مسیر فیزیکی فایل از روی آدرس نسبی ذخیره شده
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", deceased.QRCodeUrl.TrimStart('/'));
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "image/png");
        }






        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDeceased(int id, [FromBody] UpdateDeceasedDto deceasedDto)
        {
            _logger.LogInformation("Received request to update deceased with ID: {DeceasedId}", id);

            // استخراج و بررسی توکن
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
            {
                _logger.LogWarning("Unauthorized access attempt detected.");
                return Unauthorized(new { message = "توکن معتبر نیست.", statusCode = 401 });
            }

            var deceased = await _context.Deceaseds.FindAsync(id);
            if (deceased == null)
            {
                _logger.LogWarning("Deceased with ID: {DeceasedId} not found.", id);
                return NotFound(new { message = "متوفی یافت نشد.", statusCode = 404 });
            }

            // بررسی مالکیت (به فرض اینکه تنها مالک بتواند اطلاعات را به‌روزرسانی کند)
            if (deceased.UserId != currentUserId)
            {
                _logger.LogWarning("User with id {UserId} is not authorized to update this record.", currentUserId);
                return Forbid();
            }

            // به‌روزرسانی اطلاعات متوفی
            deceased.PlaceOfMartyrdom = deceasedDto.PlaceOfMartyrdom ?? deceased.PlaceOfMartyrdom;
            deceased.DateOfMartyrdom = deceasedDto.DateOfMartyrdom ?? deceased.DateOfMartyrdom;
            deceased.DateOfBirth = deceasedDto.DateOfBirth ?? deceased.DateOfBirth;
            deceased.PlaceBirth = deceasedDto.PlaceBirth ?? deceased.PlaceBirth;
            deceased.Description = deceasedDto.Description ?? deceased.Description;
            deceased.Khaterat = deceasedDto.Khaterat ?? deceased.Khaterat;

            // اطلاعات تکمیلی متوفی
            deceased.FatherName = deceasedDto.FatherName ?? deceased.FatherName;
            deceased.Job = deceasedDto.Job ?? deceased.Job;
            deceased.Tahsilat = deceasedDto.Tahsilat ?? deceased.Tahsilat;
            deceased.HowDeath = deceasedDto.HowDeath ?? deceased.HowDeath;

            // به‌روزرسانی اطلاعات مکان دفن
            deceased.OstanMazar = deceasedDto.OstanMazar ?? deceased.OstanMazar;
            deceased.CityMazar = deceasedDto.CityMazar ?? deceased.CityMazar;
            deceased.Aramestan = deceasedDto.Aramestan ?? deceased.Aramestan;
            deceased.GhesteMazar = deceasedDto.GhesteMazar ?? deceased.GhesteMazar;
            deceased.RadifMazar = deceasedDto.RadifMazar ?? deceased.RadifMazar;
            deceased.NumberMazar = deceasedDto.NumberMazar ?? deceased.NumberMazar;

            // به‌روزرسانی اسامی فرزندان (جایگزینی کامل لیست در صورت ارسال)
            if (deceasedDto.ChildrenNames != null)
            {
                deceased.ChildrenNames = deceasedDto.ChildrenNames;
            }

            // به‌روزرسانی سربرگ (Sarbarg)
            if (deceasedDto.Sarbarg != null)
            {
                if (deceased.Sarbarg == null)
                {
                    // اگر سربرگ قبلاً وجود نداشته باشد، یک موجودیت جدید ایجاد می‌کنیم
                    deceased.Sarbarg = new deathSite.Model.Sarbarg
                    {
                        Title = deceasedDto.Sarbarg.Title,
                        Description = deceasedDto.Sarbarg.Description
                    };
                }
                else
                {
                    // اگر سربرگ قبلاً وجود داشته باشد، آن را به‌روزرسانی می‌کنیم
                    deceased.Sarbarg.Title = deceasedDto.Sarbarg.Title;
                    deceased.Sarbarg.Description = deceasedDto.Sarbarg.Description;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deceased with ID: {DeceasedId} updated successfully.", id);
                return Ok(new { message = "اطلاعات متوفی با موفقیت به‌روزرسانی شد.", statusCode = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating deceased with ID: {DeceasedId}.", id);
                return StatusCode(500, new { message = "خطا در به‌روزرسانی اطلاعات.", error = ex.Message, statusCode = 500 });
            }
        }



        // POST api/deceased/{deceasedId}/uploadFiles
        [HttpPost("{deceasedId}/uploadFiles")]
        public async Task<IActionResult> UploadFilesAsync(int deceasedId,
            [FromForm] List<IFormFile>? photos,
            [FromForm] List<IFormFile>? videos,
            [FromForm] List<IFormFile>? audios)
        {
            // بررسی توکن
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
            {
                _logger.LogWarning("Unauthorized access attempt detected.");
                return Unauthorized(new { message = "توکن معتبر نیست.", statusCode = 401 });
            }

            // یافتن متوفی و بررسی مالکیت
            var deceased = await _context.Deceaseds.FindAsync(deceasedId);
            if (deceased == null)
            {
                return NotFound("متوفی یافت نشد.");
            }
            if (deceased.UserId != currentUserId)
            {
                _logger.LogWarning("User with id {UserId} is not authorized to upload files for this record.", currentUserId);
                return Forbid();
            }

            if (photos == null && videos == null && audios == null)
            {
                return BadRequest("هیچ فایلی ارسال نشده است.");
            }

            try
            {
                List<string> uploadedFileUrls = new List<string>();

                // آپلود فایل‌های عکس
                if (photos != null && photos.Count > 0)
                {
                    uploadedFileUrls.AddRange(await _fileUploadService.UploadMultipleFilesAsync(photos, "deceased", deceasedId));
                }

                // آپلود فایل‌های ویدیو
                if (videos != null && videos.Count > 0)
                {
                    uploadedFileUrls.AddRange(await _fileUploadService.UploadMultipleFilesAsync(videos, "deceased", deceasedId));
                }

                // آپلود فایل‌های صدا
                if (audios != null && audios.Count > 0)
                {
                    uploadedFileUrls.AddRange(await _fileUploadService.UploadMultipleFilesAsync(audios, "deceased", deceasedId));
                }

                // جدا کردن فایل‌ها به نوع‌های مختلف و ذخیره کردن URL ها
                foreach (var fileUrl in uploadedFileUrls)
                {
                    if (fileUrl.Contains("images"))
                    {
                        deceased.PhotoUrls.Add(fileUrl);
                    }
                    else if (fileUrl.Contains("videos"))
                    {
                        deceased.VideoUrls.Add(fileUrl);
                    }
                    else if (fileUrl.Contains("audios"))
                    {
                        deceased.VoiceUrls.Add(fileUrl);
                    }
                }

                // ذخیره تغییرات در دیتابیس
                await _context.SaveChangesAsync();

                return Ok(new { Message = "فایل‌ها با موفقیت آپلود شدند.", FileUrls = uploadedFileUrls });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "خطا در آپلود فایل‌ها.", Error = ex.Message });
            }
        }

        // DELETE api/deceased/{deceasedId}/deleteFile/{fileId}
        [HttpDelete("{deceasedId}/deleteFile/{fileId}")]
        public async Task<IActionResult> DeleteFileAsync(int deceasedId, string fileId)
        {
            // بررسی توکن
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
            {
                _logger.LogWarning("Unauthorized access attempt detected.");
                return Unauthorized(new { message = "توکن معتبر نیست.", statusCode = 401 });
            }

            // یافتن متوفی و بررسی مالکیت
            var deceased = await _context.Deceaseds.FindAsync(deceasedId);
            if (deceased == null)
            {
                return NotFound("متوفی یافت نشد.");
            }
            if (deceased.UserId != currentUserId)
            {
                _logger.LogWarning("User with id {UserId} is not authorized to delete files for this record.", currentUserId);
                return Forbid();
            }

            try
            {
                // پیدا کردن URL فایل در لیست‌ها
                string? fileUrl = deceased.PhotoUrls?.FirstOrDefault(url => url.Contains(fileId)) ??
                                  deceased.VideoUrls?.FirstOrDefault(url => url.Contains(fileId)) ??
                                  deceased.VoiceUrls?.FirstOrDefault(url => url.Contains(fileId));

                if (fileUrl == null)
                {
                    return NotFound("فایل مورد نظر یافت نشد.");
                }

                // تبدیل URL به مسیر فیزیکی فایل
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                else
                {
                    return NotFound("فایل در سیستم پیدا نشد.");
                }

                // حذف URL از لیست مناسب
                if (deceased.PhotoUrls.Contains(fileUrl))
                {
                    deceased.PhotoUrls.Remove(fileUrl);
                }
                else if (deceased.VideoUrls.Contains(fileUrl))
                {
                    deceased.VideoUrls.Remove(fileUrl);
                }
                else if (deceased.VoiceUrls.Contains(fileUrl))
                {
                    deceased.VoiceUrls.Remove(fileUrl);
                }

                await _context.SaveChangesAsync();

                return Ok(new { Message = "فایل با موفقیت حذف شد." });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while deleting file: {Error}", ex.Message);
                return StatusCode(500, new { Message = "خطا در حذف فایل.", Error = ex.Message });
            }
        }


        [HttpPost("{deceasedId}/uploadCover")]
        public async Task<IActionResult> UploadCoverPhotoAsync(int deceasedId, IFormFile coverPhoto)
        {
            // بررسی توکن
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
            {
                _logger.LogWarning("Unauthorized access attempt detected.");
                return Unauthorized(new { message = "توکن معتبر نیست.", statusCode = 401 });
            }

            // یافتن متوفی و بررسی مالکیت
            var deceased = await _context.Deceaseds.FindAsync(deceasedId);
            if (deceased == null)
            {
                return NotFound("متوفی یافت نشد.");
            }
            if (deceased.UserId != currentUserId)
            {
                _logger.LogWarning("User with id {UserId} is not authorized to upload files for this record.", currentUserId);
                return Forbid();
            }

            if (coverPhoto == null)
            {
                return BadRequest("هیچ فایلی ارسال نشده است.");
            }

            try
            {
                // حذف عکس کاور قبلی از سرور
                if (!string.IsNullOrEmpty(deceased.CoverPhotoUrl))
                {
                    string previousFileName = Path.GetFileName(deceased.CoverPhotoUrl);
                    await _fileUploadService.DeleteFileAsync(previousFileName, "deceasedCover", deceasedId);
                }

                // آپلود عکس کاور جدید
                string uploadedCoverUrl = await _fileUploadService.UploadFileAsync(coverPhoto, "deceasedCover", deceasedId);

                // ذخیره مسیر عکس کاور در دیتابیس
                deceased.CoverPhotoUrl = uploadedCoverUrl;
                await _context.SaveChangesAsync();

                return Ok(new { Message = "عکس کاور با موفقیت آپلود شد.", CoverPhotoUrl = uploadedCoverUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در آپلود عکس کاور.");
                return StatusCode(500, new { Message = "خطا در آپلود عکس کاور.", Error = ex.Message });
            }
        }

        [HttpGet("{deceasedId}/cover")]
        public async Task<IActionResult> GetCoverPhotoAsync(int deceasedId)
        {
            var deceased = await _context.Deceaseds.FindAsync(deceasedId);
            if (deceased == null)
            {
                return NotFound("متوفی یافت نشد.");
            }

            if (string.IsNullOrEmpty(deceased.CoverPhotoUrl))
            {
                return NotFound("عکس کاوری برای این متوفی ثبت نشده است.");
            }

            return Ok(new { CoverPhotoUrl = deceased.CoverPhotoUrl });
        }



        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDeceased(int id)
        {
            // بررسی توکن
            var currentUserId = GetCurrentUserId();
            if (currentUserId == 0)
            {
                _logger.LogWarning("Unauthorized access attempt detected.");
                return Unauthorized(new { message = "توکن معتبر نیست.", statusCode = 401 });
            }

            try
            {
                // پیدا کردن متوفی بر اساس id
                var deceased = await _context.Deceaseds.FindAsync(id);
                if (deceased == null)
                {
                    _logger.LogWarning("Deceased with id {DeceasedId} not found.", id);
                    return NotFound(new { Message = "متوفی یافت نشد.", StatusCode = 404 });
                }

                // بررسی مالکیت
                if (deceased.UserId != currentUserId)
                {
                    _logger.LogWarning("User with id {UserId} is not authorized to delete this record.", currentUserId);
                    return Forbid();
                }

                // حذف فایل‌های مرتبط
                List<string> allFilePaths = new List<string>();
                allFilePaths.AddRange(deceased.PhotoUrls);
                allFilePaths.AddRange(deceased.VideoUrls);
                allFilePaths.AddRange(deceased.VoiceUrls);

                // حذف فایل‌ها
                foreach (var filePath in allFilePaths)
                {
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        try
                        {
                            string fileName = Path.GetFileName(filePath);
                            await _fileUploadService.DeleteFileAsync(fileName, "deceased", id);
                        }
                        catch (FileNotFoundException ex)
                        {
                            _logger.LogWarning(ex, "File not found for deletion: {FilePath}", filePath);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
                        }
                    }
                }

                // حذف پوشه متوفی اگر فایل‌ها حذف شدند
                string deceasedFolderPath = Path.Combine("wwwroot", "uploads", "deceased", id.ToString());
                if (Directory.Exists(deceasedFolderPath))
                {
                    try
                    {
                        Directory.Delete(deceasedFolderPath, true);
                        _logger.LogInformation("پوشه {DeceasedFolderPath} با موفقیت حذف شد.", deceasedFolderPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "خطا در حذف پوشه {DeceasedFolderPath}", deceasedFolderPath);
                    }
                }

                // حذف متوفی از دیتابیس
                _context.Deceaseds.Remove(deceased);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deceased with id {DeceasedId} and associated files were successfully deleted.", id);
                return Ok(new { Message = "متوفی و فایل‌های مربوطه حذف شدند.", StatusCode = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting deceased with id {DeceasedId}.", id);
                return StatusCode(500, new { Message = "خطای داخلی سرور.", Error = ex.Message, StatusCode = 500 });
            }
        }

        // // متد برای انتخاب قالب متوفی

        [HttpPost("set-template")]
        public async Task<IActionResult> SetDeceasedTemplate([FromBody] SetTemplateRequest request)
        {
            _logger.LogInformation("Received request to set template for deceased ID {DeceasedId}", request.DeceasedId);

            var deceased = await _context.Deceaseds.FindAsync(request.DeceasedId);
            if (deceased == null)
            {
                _logger.LogWarning("Deceased with ID {DeceasedId} not found.", request.DeceasedId);
                return NotFound(new { message = "متوفی یافت نشد.", statusCode = 404 });
            }

            if (deceased.UserId != request.UserId)
            {
                _logger.LogWarning("User ID {UserId} is not authorized to change template for deceased ID {DeceasedId}.", request.UserId, request.DeceasedId);
                return Unauthorized(new { message = "شما مجاز به ویرایش این متوفی نیستید.", statusCode = 401 });
            }

            deceased.Ghaleb = request.Ghaleb;
            _context.Deceaseds.Update(deceased);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Template updated successfully for deceased ID {DeceasedId}.", request.DeceasedId);
            return Ok(new { message = "قالب با موفقیت به‌روزرسانی شد.", statusCode = 200 });
        }



        // GET api/deceased/{deceasedId}/ghaleb
        [HttpGet("{deceasedId}/ghaleb")]
        public async Task<IActionResult> GetDeceasedGhaleb(int deceasedId)
        {
            _logger.LogInformation("Fetching template for deceased with ID: {DeceasedId}", deceasedId);

            var deceased = await _context.Deceaseds
                .Where(d => d.Id == deceasedId)
                .Select(d => new { d.Ghaleb })
                .FirstOrDefaultAsync();

            if (deceased == null)
            {
                _logger.LogWarning("Deceased with ID {DeceasedId} not found.", deceasedId);
                return NotFound(new { message = "متوفی یافت نشد.", statusCode = 404 });
            }

            return Ok(new { ghaleb = deceased.Ghaleb, statusCode = 200 });
        }

        [HttpGet("get-qrcodes")]
        public async Task<IActionResult> GetAllQRCodes()
        {
            // جستجو برای تمام متوفیان که QR Code دارند
            var deceasedWithQRCode = await _context.Deceaseds
                .Where(d => !string.IsNullOrEmpty(d.QRCodeUrl)) // فقط متوفی‌هایی که QR Code دارند
                .Include(d => d.User) // بارگذاری اطلاعات کاربر مربوطه
                .ToListAsync();

            // بررسی اینکه آیا نتیجه‌ای یافت شد یا خیر
            if (deceasedWithQRCode == null || !deceasedWithQRCode.Any())
            {
                return NotFound(new { message = "هیچ QR Code ای یافت نشد.", statusCode = 404 });
            }

            // استخراج اطلاعات مربوط به QR Codeها، متوفیان و کاربران
            var result = deceasedWithQRCode.Select(d => new
            {
                DeceasedId = d.Id,
                DeceasedName = d.FullName,
                QRCodeUrl = d.QRCodeUrl,
                UserId = d.User.Id,
                UserName = $"{d.User.firstName} {d.User.lastName}"
            }).ToList();

            // بازگرداندن نتیجه
            return Ok(new { message = "اطلاعات QR Code ها با موفقیت بارگذاری شد.", data = result, statusCode = 200 });
        }

    }
}
