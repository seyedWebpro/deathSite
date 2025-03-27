using System;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Middleware;
using api.Model;
using api.Model.AdminModel;
using api.View;
using api.View.DeadsManagement;
using deathSite.Model;
using deathSite.View.DeadsManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class CondolenceMessageController : ControllerBase
    {
        private readonly apiContext _context;
        private readonly ILogger<CondolenceMessageController> _logger;

        public CondolenceMessageController(apiContext context, ILogger<CondolenceMessageController> logger)
        {
            _context = context;
            _logger = logger;
        }


        // افزودن پیام تسلیت برای یک متوفی خاص
        [HttpPost]
        public async Task<IActionResult> CreateCondolenceMessage([FromBody] CondolenceMessageCreateView messageDto)
        {
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            // بررسی وجود متوفی
            var deceased = await _context.Deceaseds.FindAsync(messageDto.DeceasedId);
            if (deceased == null)
            {
                _logger.LogWarning($"Deceased with ID {messageDto.DeceasedId} not found.");
                return NotFound(new { StatusCode = 404, Message = "متوفی یافت نشد." });
            }

            // بررسی وجود نام متوفی
            if (string.IsNullOrEmpty(deceased.FullName))
            {
                _logger.LogWarning("Deceased's full name is missing.");
                return BadRequest(new { StatusCode = 400, Message = "نام متوفی وارد نشده است." });
            }

            // بررسی وجود کاربر
            var user = await _context.users.FindAsync(messageDto.UserId);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {messageDto.UserId} not found.");
                return NotFound(new { StatusCode = 404, Message = "کاربر یافت نشد." });
            }

            var message = new CondolenceMessage
            {
                AuthorName = messageDto.AuthorName,
                PhoneNumber = messageDto.PhoneNumber,
                CreatedDate = DateTime.UtcNow,
                MessageText = messageDto.MessageText,
                // تنظیم وضعیت پیام به صورت پیش‌فرض بر روی Pending
                Status = CommentStatus.Pending,
                DeceasedId = messageDto.DeceasedId,
                UserId = messageDto.UserId,
                DeceasedName = deceased.FullName
            };

            try
            {
                _context.condolenceMessages.Add(message);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Condolence message created for deceased {deceased.FullName} by user {user?.lastName ?? "Unknown"}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the condolence message.");
                return StatusCode(500, new { StatusCode = 500, Message = "خطای داخلی سرور.", Error = ex.Message });
            }

            // بازگشت پاسخ به کاربر با استفاده از DTO
            var response = new CondolenceMessageResponseDto
            {
                Id = message.Id,
                AuthorName = message.AuthorName,
                PhoneNumber = message.PhoneNumber,
                MessageText = message.MessageText,
                Status = message.Status.ToString(), // تبدیل Enum به String
                DeceasedName = message.DeceasedName
            };

            return Ok(new { StatusCode = 201, Message = "پیام تسلیت ثبت شد.", Data = response });
        }

        // به‌روزرسانی وضعیت پیام تسلیت (استفاده از Enum)
        [HttpPut]
        public async Task<IActionResult> UpdateCondolenceMessageStatus([FromBody] CondolenceMessageUpdateView messageDto)
        {
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            var message = await _context.condolenceMessages.FindAsync(messageDto.Id);
            if (message == null)
            {
                return NotFound(new { StatusCode = 404, Message = "پیام تسلیت یافت نشد." });
            }

            // به‌روزرسانی وضعیت پیام بر اساس مقدار دریافتی از DTO
            message.Status = messageDto.Status;
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "وضعیت پیام تسلیت تغییر کرد.", Data = message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCondolenceMessage(int id)
        {
            var message = await _context.condolenceMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound(new { StatusCode = 404, Message = "پیام تسلیت یافت نشد." });
            }

            _context.condolenceMessages.Remove(message);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "پیام تسلیت حذف شد." });
        }

        // دریافت تمام پیام‌های تسلیت به همراه اطلاعات متوفی (استفاده از Enum)
        [HttpGet]
        public async Task<IActionResult> GetAllCondolenceMessages()
        {
            var messages = await _context.condolenceMessages
                .Include(cm => cm.Deceased) // لود کردن اطلاعات متوفی
                .Select(cm => new
                {
                    cm.Id,
                    cm.AuthorName,
                    cm.PhoneNumber,
                    cm.CreatedDate,
                    cm.MessageText,
                    Status = cm.Status.ToString(), // استفاده از Status
                    Deceased = new
                    {
                        cm.Deceased.Id,
                        cm.Deceased.FullName,
                    }
                })
                .ToListAsync();

            return Ok(new { StatusCode = 200, Data = messages });
        }

        [HttpGet("user/{userId}/deceased/{deceasedId}")]
        public async Task<IActionResult> GetCondolenceMessagesForUserAndDeceased(int userId, int deceasedId)
        {
            var messages = await _context.condolenceMessages
                .Where(cm => cm.UserId == userId && cm.DeceasedId == deceasedId)
                .Select(cm => new CondolenceMessageView
                {
                    Id = cm.Id,
                    AuthorName = cm.AuthorName,
                    PhoneNumber = cm.PhoneNumber,
                    CreatedDate = cm.CreatedDate,
                    DeceasedName = cm.Deceased.FullName,
                    MessageText = cm.MessageText,
                    Status = cm.Status.ToString() // استفاده از Status
                })
                .ToListAsync();

            if (!messages.Any())
            {
                return NotFound(new { StatusCode = 404, Message = "هیچ پیامی برای این کاربر و متوفی یافت نشد." });
            }

            return Ok(new { StatusCode = 200, Data = messages });
        }

        // دریافت پیام‌های تسلیت تایید شده برای یک متوفی خاص
        [HttpGet("deceased/{deceasedId}")]
        public async Task<IActionResult> GetMessagesForDeceased(int deceasedId)
        {
            var messages = await _context.condolenceMessages
                .Where(cm => cm.DeceasedId == deceasedId && cm.Status == CommentStatus.Approved)
                .Select(cm => new CondolenceMessageView
                {
                    Id = cm.Id,
                    AuthorName = cm.AuthorName,
                    PhoneNumber = cm.PhoneNumber,
                    CreatedDate = cm.CreatedDate,
                    MessageText = cm.MessageText,
                    Status = cm.Status.ToString() // استفاده از Status
                })
                .ToListAsync();

            if (!messages.Any())
            {
                return NotFound(new { StatusCode = 404, Message = "هیچ پیام تسلیتی تایید شده برای این متوفی یافت نشد." });
            }

            return Ok(new { StatusCode = 200, Data = messages });
        }

        [HttpPost("reply")]
        public async Task<IActionResult> CreateCondolenceReply([FromBody] CondolenceReplyCreateView replyDto)
        {
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            // بررسی وجود پیام تسلیت
            var condolenceMessage = await _context.condolenceMessages.FindAsync(replyDto.CondolenceMessageId);
            if (condolenceMessage == null)
            {
                return NotFound(new { StatusCode = 404, Message = "پیام تسلیت یافت نشد." });
            }

            // بررسی وجود کاربر اگر UserId مقدار داشته باشد
            User user = null;
            if (replyDto.UserId.HasValue)
            {
                user = await _context.users.FindAsync(replyDto.UserId.Value);
                if (user == null)
                {
                    return NotFound(new { StatusCode = 404, Message = "کاربر یافت نشد." });
                }
            }

            var reply = new CondolenceReply
            {
                ReplyText = replyDto.ReplyText,
                CreatedDate = DateTime.UtcNow,
                CondolenceMessageId = replyDto.CondolenceMessageId,
                UserId = replyDto.UserId,
                User = user,
                AuthorName = user != null ? user.lastName : replyDto.AuthorName
            };

            _context.CondolenceReplies.Add(reply);
            await _context.SaveChangesAsync();

            var replyResponse = new CondolenceReplyResponseDto
            {
                Id = reply.Id,
                AuthorName = reply.AuthorName,
                ReplyText = reply.ReplyText,
                CreatedDate = reply.CreatedDate
            };

            return Ok(new { StatusCode = 201, Message = "پاسخ ثبت شد.", Data = replyResponse });
        }

        [HttpGet("condolence/{messageId}/replies")]
        public async Task<IActionResult> GetRepliesForCondolence(int messageId)
        {
            var replies = await _context.CondolenceReplies
                .Where(r => r.CondolenceMessageId == messageId)
                .Select(r => new CondolenceReplyView
                {
                    Id = r.Id,
                    ReplyText = r.ReplyText,
                    CreatedDate = r.CreatedDate,
                    AuthorName = !string.IsNullOrEmpty(r.AuthorName) ? r.AuthorName : (r.User != null ? r.User.lastName : "نامشخص")
                })
                .ToListAsync();

            return Ok(new { StatusCode = 200, Data = replies });
        }
    }
}
