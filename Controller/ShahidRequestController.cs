using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Services;
using deathSite.Model;
using deathSite.View.ShahidRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace deathSite.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShahidRequestController : ControllerBase
    {
        private readonly ILogger<ShahidRequestController> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly apiContext _context;
        private readonly IConfiguration _configuration;


        public ShahidRequestController(ILogger<ShahidRequestController> logger, IFileUploadService fileUploadService, apiContext context, IConfiguration configuration)
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
            _context = context;
            _configuration = configuration;
        }


        [HttpPost("{id}/update-request")]
        public async Task<IActionResult> CreateUpdateRequest(int id, [FromForm] ShahidUpdateRequestDto updateRequestDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation failed for update request.");
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
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (userIdClaim == null)
                {
                    return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
                }
                var userId = int.Parse(userIdClaim);

                // یافتن شهید مربوطه
                var shahid = await _context.shahids.FindAsync(id);
                if (shahid == null)
                {
                    return NotFound(new { StatusCode = 404, Message = "شهید پیدا نشد." });
                }

                // نگاشت داده‌های دریافتی از DTO به مدل ShahidUpdateRequest
                var updateRequest = new ShahidUpdateRequest
                {
                    Biography = updateRequestDto.Biography,
                    Memories = updateRequestDto.Memories,
                    Will = updateRequestDto.Will,
                    ShahidId = id,
                    UserId = userId,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow,
                    PhotoUrls = new List<string>(),
                    VideoUrls = new List<string>(),
                    VoiceUrls = new List<string>()
                };

                // پردازش فایل‌های ارسالی در صورت وجود
                if (updateRequestDto.PhotoFiles != null && updateRequestDto.PhotoFiles.Any())
                {
                    foreach (var file in updateRequestDto.PhotoFiles)
                    {
                        if (file != null && file.Length > 0)
                        {
                            var uploadedPath = await _fileUploadService.UploadFileAsync(file, "shahids", id);
                            updateRequest.PhotoUrls.Add(uploadedPath);
                        }
                    }
                }

                if (updateRequestDto.VideoFiles != null && updateRequestDto.VideoFiles.Any())
                {
                    foreach (var file in updateRequestDto.VideoFiles)
                    {
                        if (file != null && file.Length > 0)
                        {
                            var uploadedPath = await _fileUploadService.UploadFileAsync(file, "shahids", id);
                            updateRequest.VideoUrls.Add(uploadedPath);
                        }
                    }
                }

                if (updateRequestDto.VoiceFiles != null && updateRequestDto.VoiceFiles.Any())
                {
                    foreach (var file in updateRequestDto.VoiceFiles)
                    {
                        if (file != null && file.Length > 0)
                        {
                            var uploadedPath = await _fileUploadService.UploadFileAsync(file, "shahids", id);
                            updateRequest.VoiceUrls.Add(uploadedPath);
                        }
                    }
                }

                await _context.ShahidUpdateRequests.AddAsync(updateRequest);
                await _context.SaveChangesAsync();

                return Ok(new { StatusCode = 200, Message = "درخواست به‌روزرسانی ثبت شد و در انتظار تایید ادمین می‌باشد." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing update request.");
                return Unauthorized(new { StatusCode = 401, Message = "توکن معتبر نیست." });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("update-request/{updateRequestId}/approve")]
        public async Task<IActionResult> ApproveUpdateRequest(int updateRequestId)
        {
            try
            {
                var updateRequest = await _context.ShahidUpdateRequests
                                        .Include(r => r.Shahid)
                                        .FirstOrDefaultAsync(r => r.Id == updateRequestId);
                if (updateRequest == null)
                {
                    return NotFound(new { StatusCode = 404, Message = "درخواست به‌روزرسانی پیدا نشد." });
                }

                var shahid = updateRequest.Shahid;

                // الحاق متن‌ها با یک خط فاصله
                if (!string.IsNullOrEmpty(updateRequest.Biography))
                    shahid.Biography = string.Join(Environment.NewLine,
                        shahid.Biography,
                        updateRequest.Biography);

                if (!string.IsNullOrEmpty(updateRequest.Memories))
                    shahid.Memories = string.Join(Environment.NewLine,
                        shahid.Memories,
                        updateRequest.Memories);

                if (!string.IsNullOrEmpty(updateRequest.Will))
                    shahid.Will = string.Join(Environment.NewLine,
                        shahid.Will,
                        updateRequest.Will);

                // اضافه کردن فایل‌ها به لیست موجود
                if (updateRequest.PhotoUrls != null && updateRequest.PhotoUrls.Any())
                    shahid.PhotoUrls.AddRange(updateRequest.PhotoUrls);

                if (updateRequest.VideoUrls != null && updateRequest.VideoUrls.Any())
                    shahid.VideoUrls.AddRange(updateRequest.VideoUrls);

                if (updateRequest.VoiceUrls != null && updateRequest.VoiceUrls.Any())
                    shahid.VoiceUrls.AddRange(updateRequest.VoiceUrls);

                updateRequest.Status = "Approved";
                await _context.SaveChangesAsync();

                return Ok(new { StatusCode = 200, Message = "درخواست به‌روزرسانی تایید شد و اطلاعات به شهید اضافه گردید." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while approving update request.");
                return StatusCode(500, new { StatusCode = 500, Message = "خطای سرور." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update-request/{updateRequestId}/reject")]
        public async Task<IActionResult> RejectUpdateRequest(int updateRequestId)
        {

            try
            {

                var updateRequest = await _context.ShahidUpdateRequests.FindAsync(updateRequestId);
                if (updateRequest == null)
                {
                    return NotFound(new { StatusCode = 404, Message = "درخواست به‌روزرسانی پیدا نشد." });
                }

                updateRequest.Status = "Rejected";

                await _context.SaveChangesAsync();

                return Ok(new { StatusCode = 200, Message = "درخواست به‌روزرسانی رد شد." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while rejecting update request.");
                return StatusCode(500, new { StatusCode = 500, Message = "خطای سرور." });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("update-requests")]
        public async Task<IActionResult> GetAllUpdateRequests()
        {
            var updateRequests = await _context.ShahidUpdateRequests
                .Include(r => r.User)
                .Include(r => r.Shahid) // اضافه کردن Include برای رابطه Shahid
                .ToListAsync();

            var result = updateRequests.Select(r => new
            {
                UpdateRequestId = r.Id,
                r.Biography,
                r.Memories,
                r.Will,
                r.Status,
                r.CreatedAt,
                r.PhotoUrls,
                r.VideoUrls,
                r.VoiceUrls,
                Shahid = new // اضافه کردن اطلاعات شهید
                {
                    r.Shahid.Id,
                    r.Shahid.FullName
                },
                User = new
                {
                    r.User.Id,
                    r.User.firstName,
                    r.User.lastName,
                    r.User.phoneNumber,
                    r.User.role
                }
            });

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("update-requests")]
        public async Task<IActionResult> DeleteUpdateRequests([FromQuery] DeleteUpdateRequestsDto deleteDto)
        {
            if (deleteDto?.UpdateRequestIds == null || !deleteDto.UpdateRequestIds.Any())
            {
                return BadRequest(new { StatusCode = 400, Message = "هیچ درخواست انتخاب نشده است." });
            }

            // دریافت درخواست‌هایی که شناسه آن‌ها در لیست ارسال شده موجود است
            var updateRequests = await _context.ShahidUpdateRequests
                .Where(r => deleteDto.UpdateRequestIds.Contains(r.Id))
                .ToListAsync();

            if (!updateRequests.Any())
            {
                return NotFound(new { StatusCode = 404, Message = "هیچ درخواست مورد نظر یافت نشد." });
            }

            // حذف درخواست‌های انتخاب شده
            _context.ShahidUpdateRequests.RemoveRange(updateRequests);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "درخواست‌های انتخاب شده حذف شدند." });
        }

    }
}