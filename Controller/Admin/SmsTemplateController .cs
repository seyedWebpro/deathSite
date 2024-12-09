using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Middleware;
using api.Model.AdminModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmsTemplateController : ControllerBase
    {
        private readonly apiContext _context;

        public SmsTemplateController(apiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplates()
        {
            var templates = await _context.smsTemplates.ToListAsync();
            var response = templates.Select(template => new
            {
                template.Id,
                template.MessageType,
                template.Message,
                MessageTypeFa = GetMessageTypeFa(template.MessageType) // اضافه کردن نوع پیام فارسی
            });

            return Ok(new { StatusCode = 200, Message = "قالب‌ها با موفقیت دریافت شدند.", Data = response });
        }

        [HttpPost]
        public async Task<IActionResult> CreateTemplate([FromBody] SmsTemplate template)
        {
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            if (!IsValidMessageType(template.MessageType))
            {
                return BadRequest(new { StatusCode = 400, Message = "نوع پیام نامعتبر است." });
            }

            _context.smsTemplates.Add(template);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTemplates), new 
            { 
                StatusCode = 201, 
                Message = "قالب با موفقیت ایجاد شد.", 
                Data = new 
                {
                    template.Id,
                    template.MessageType,
                    template.Message,
                    MessageTypeFa = GetMessageTypeFa(template.MessageType) // اضافه کردن نوع پیام فارسی
                } 
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTemplate(int id, [FromBody] SmsTemplate template)
        {
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            if (template == null || id != template.Id)
            {
                return BadRequest(new { StatusCode = 400, Message = "عدم تطابق ID قالب یا داده‌های قالب نامعتبر." });
            }

            var existingTemplate = await _context.smsTemplates.FindAsync(id);
            if (existingTemplate == null)
            {
                return NotFound(new { StatusCode = 404, Message = "قالب پیدا نشد." });
            }

            if (!IsValidMessageType(template.MessageType))
            {
                return BadRequest(new { StatusCode = 400, Message = "نوع پیام نامعتبر است." });
            }

            existingTemplate.MessageType = template.MessageType;
            existingTemplate.Message = template.Message;

            _context.Entry(existingTemplate).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new 
            { 
                StatusCode = 200, 
                Message = "قالب با موفقیت به‌روزرسانی شد.", 
                Data = new 
                {
                    existingTemplate.Id,
                    existingTemplate.MessageType,
                    existingTemplate.Message,
                    MessageTypeFa = GetMessageTypeFa(existingTemplate.MessageType) // اضافه کردن نوع پیام فارسی
                } 
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemplate(int id)
        {
            var template = await _context.smsTemplates.FindAsync(id);
            if (template == null)
            {
                return NotFound(new { StatusCode = 404, Message = "قالب پیدا نشد." });
            }

            _context.smsTemplates.Remove(template);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "قالب با موفقیت حذف شد." });
        }

        // متد کمکی برای بررسی نوع پیام
        private bool IsValidMessageType(string messageType)
        {
            var validMessageTypes = new[] { "Authentication", "PasswordReset", "OrderSuccess", "RenewalSuccess", "UpgradeSuccess", "RenewalReminder" };
            return validMessageTypes.Contains(messageType);
        }

         // متد کمکی برای دریافت نوع پیام فارسی
        private string GetMessageTypeFa(string messageType)
        {
            return messageType switch
            {
                "Authentication" => "احراز هویت",
                "PasswordReset" => "فراموشی رمز عبور",
                "OrderSuccess" => "ثبت سفارش موفق",
                "RenewalSuccess" => "ثبت تمدید موفق",
                "UpgradeSuccess" => "ارتقا کاربر موفق",
                "RenewalReminder" => "یادآوری زمان تمدید",
                _ => "نوع پیام نامشخص" // در صورت عدم تطابق، پیام پیش‌فرض
            };
        }

    }
}