using api.Context;
using api.Model;
using api.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

[Route("api/[controller]")]
[ApiController]
public class ContactUsController : ControllerBase
{
    private readonly apiContext _context;

    public ContactUsController(apiContext context)
    {
        _context = context;
    }

    [HttpPost("contactus")]
    public async Task<IActionResult> ContactUs([FromBody] ContactMeFormDto contactUsDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "ورودی نامعتبر است.", errors = ModelState });
        }

        try
        {
            var contactMeForm = new ContactMeForm
            {
                FullName = contactUsDto.FullName,
                Email = contactUsDto.Email,
                PhoneNumber = contactUsDto.PhoneNumber,
                Subject = contactUsDto.Subject,
                Message = contactUsDto.Message
            };

            await _context.ContactMeForms.AddAsync(contactMeForm);
            await _context.SaveChangesAsync();

            return Ok(new { message = "پیام شما با موفقیت ارسال شد.", statusCode = 200 });
        }
        catch (DbUpdateException dbEx)
        {
            return StatusCode(500, new { message = "خطای پایگاه داده.", error = dbEx.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "خطای داخلی سرور.", error = ex.Message });
        }
    }

    [HttpGet("contactus")]
    public async Task<ActionResult<IEnumerable<ContactMeForm>>> GetContactUsForms()
    {
        try
        {
            var contactForms = await _context.ContactMeForms.ToListAsync();

            if (contactForms == null || !contactForms.Any())
            {
                return NotFound(new { message = "فرمی یافت نشد.", statusCode = 404 });
            }

            return Ok(new { contactForms, statusCode = 200 });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "خطای داخلی سرور.", error = ex.Message, statusCode = 500 });
        }
    }

    [HttpDelete("contactus/{id}")]
    public async Task<IActionResult> DeleteContactUs(int id)
    {
        try
        {
            var contactMeForm = await _context.ContactMeForms.FindAsync(id);

            if (contactMeForm == null)
            {
                return NotFound(new { message = "فرم موردنظر یافت نشد.", statusCode = 404 });
            }

            _context.ContactMeForms.Remove(contactMeForm);
            await _context.SaveChangesAsync();

            return Ok(new { message = "فرم با موفقیت حذف شد.", statusCode = 200 });
        }
        catch (DbUpdateException dbEx)
        {
            return StatusCode(500, new { message = "خطای پایگاه داده.", error = dbEx.Message, statusCode = 500 });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "خطای داخلی سرور.", error = ex.Message, statusCode = 500 });
        }
    }
}

