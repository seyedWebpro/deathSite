using System;
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

        //  افزودن پیام تسلیت برای یک متوفی خاص
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
        return NotFound(new { StatusCode = 404, Message = "متوفی یافت نشد." });
    }

    // بررسی وجود کاربر
    var user = await _context.users.FindAsync(messageDto.UserId);
    if (user == null)
    {
        return NotFound(new { StatusCode = 404, Message = "کاربر یافت نشد." });
    }

    var message = new CondolenceMessage
    {
        AuthorName = messageDto.AuthorName,
        PhoneNumber = messageDto.PhoneNumber,
        CreatedDate = DateTime.UtcNow,
        MessageText = messageDto.MessageText,
        IsApproved = false,
        DeceasedId = messageDto.DeceasedId,
        UserId = messageDto.UserId
    };

    _context.condolenceMessages.Add(message);
    await _context.SaveChangesAsync();

    return Ok(new { StatusCode = 201, Message = "پیام تسلیت ثبت شد.", Data = message });
}

      [HttpPut]
public async Task<IActionResult> ApproveCondolenceMessage([FromBody] CondolenceMessageUpdateView messageDto)
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

    message.IsApproved = messageDto.IsApproved;
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

[HttpGet]
public async Task<IActionResult> GetAllCondolenceMessages()
{
    var messages = await _context.condolenceMessages
        .Select(cm => new CondolenceMessageView
        {
            Id = cm.Id,
            AuthorName = cm.AuthorName,
            PhoneNumber = cm.PhoneNumber,
            CreatedDate = cm.CreatedDate,
            DeceasedName = cm.Deceased.FirstName + " " + cm.Deceased.LastName,
            MessageText = cm.MessageText,
            IsApproved = cm.IsApproved
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
            DeceasedName = cm.Deceased.FirstName + " " + cm.Deceased.LastName,
            MessageText = cm.MessageText,
            IsApproved = cm.IsApproved
        })
        .ToListAsync();

    if (!messages.Any())
    {
        return NotFound(new { StatusCode = 404, Message = "هیچ پیامی برای این کاربر و متوفی یافت نشد." });
    }

    return Ok(new { StatusCode = 200, Data = messages });
}

    }
}
