using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Middleware;
using api.Model.AdminModel;
using api.View;
using api.View.DeadsManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class CondolenceMessageController : ControllerBase
    {

        private readonly apiContext _context;

        public CondolenceMessageController(apiContext context)
        {
            _context = context;
        }

        // متد برای اضافه کردن پیام تسلیت
        [HttpPost]
        public async Task<IActionResult> CreateCondolenceMessage([FromBody] CondolenceMessageCreateView messageDto)
        {
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            var message = new CondolenceMessage
            {
                AuthorName = messageDto.AuthorName,
                PhoneNumber = messageDto.PhoneNumber,
                CreatedDate = DateTime.UtcNow,
                DeceasedName = messageDto.DeceasedName,
                MessageText = messageDto.MessageText,
                IsApproved = false // به طور پیش‌فرض پیام‌ها تایید نشده‌اند
            };

            _context.condolenceMessages.Add(message);
            await _context.SaveChangesAsync();

            var messageDtoResponse = new CondolenceMessageView
            {
                Id = message.Id,
                AuthorName = message.AuthorName,
                PhoneNumber = message.PhoneNumber,
                CreatedDate = message.CreatedDate,
                DeceasedName = message.DeceasedName,
                MessageText = message.MessageText,
                IsApproved = message.IsApproved
            };

            return Ok(new { StatusCode = 201, Message = "پیام تسلیت با موفقیت ایجاد شد.", Data = messageDtoResponse });
        }

        // متد برای نمایش پیام‌های تسلیت
        [HttpGet]
        public async Task<IActionResult> GetcondolenceMessages()
        {
            var messages = await _context.condolenceMessages.ToListAsync();
            var messageDtos = messages.Select(m => new CondolenceMessageView
            {
                Id = m.Id,
                AuthorName = m.AuthorName,
                PhoneNumber = m.PhoneNumber,
                CreatedDate = m.CreatedDate,
                DeceasedName = m.DeceasedName,
                MessageText = m.MessageText,
                IsApproved = m.IsApproved
            }).ToList();

            return Ok(new { StatusCode = 200, Data = messageDtos });
        }

        // متد برای تایید پیام تسلیت
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveCondolenceMessage(int id)
        {
            var message = await _context.condolenceMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound(new { StatusCode = 404, Message = "پیام تسلیت پیدا نشد." });
            }

            message.IsApproved = true;
            await _context.SaveChangesAsync();

            var messageDtoResponse = new CondolenceMessageView
            {
                Id = message.Id,
                AuthorName = message.AuthorName,
                PhoneNumber = message.PhoneNumber,
                CreatedDate = message.CreatedDate,
                DeceasedName = message.DeceasedName,
                MessageText = message.MessageText,
                IsApproved = message.IsApproved
            };

            return Ok(new { StatusCode = 200, Message = "پیام تسلیت با موفقیت تایید شد.", Data = messageDtoResponse });
        }

        // متد برای حذف پیام تسلیت
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCondolenceMessage(int id)
        {
            var message = await _context.condolenceMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound(new { StatusCode = 404, Message = "پیام تسلیت پیدا نشد." });
            }

            _context.condolenceMessages.Remove(message);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "پیام تسلیت با موفقیت حذف شد." });
        }


    }
}