// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using api.Context;
// using deathSite.Model;
// using deathSite.View.Dead;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;

// namespace deathSite.Controller
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class SavedDeceasedController : ControllerBase
//     {
//          private readonly ILogger<SavedDeceasedController> _logger;
//         private readonly apiContext _context;


//         public SavedDeceasedController(ILogger<SavedDeceasedController> logger, apiContext context)
//         {
//             _logger = logger;
//             _context = context;
//         }

//         // اضافه کردن متوفی به لیست ذخیره‌شده‌های یک کاربر
//         [HttpPost]
//         public async Task<IActionResult> SaveDeceased([FromBody] SaveDeceasedRequest request)
//         {
//             var user = await _context.users.FindAsync(request.UserId);
//             var deceased = await _context.Deceaseds.FindAsync(request.DeceasedId);

//             if (user == null || deceased == null)
//             {
//                 return NotFound(new
//                 {
//                     status = 404,
//                     message = "کاربر یا متوفی یافت نشد."
//                 });
//             }

//             // بررسی اینکه آیا متوفی قبلاً ذخیره شده است
//             var existingRecord = await _context.SavedDeceaseds
//                 .FirstOrDefaultAsync(sd => sd.UserId == request.UserId && sd.DeceasedId == request.DeceasedId);

//             if (existingRecord != null)
//             {
//                 return BadRequest(new
//                 {
//                     status = 400,
//                     message = "این متوفی قبلاً ذخیره شده است."
//                 });
//             }

//             var savedDeceased = new SavedDeceased
//             {
//                 UserId = request.UserId,
//                 DeceasedId = request.DeceasedId,
//                 SavedAt = DateTime.UtcNow
//             };

//             _context.SavedDeceaseds.Add(savedDeceased);
//             await _context.SaveChangesAsync();

//             return Ok(new
//             {
//                 status = 200,
//                 message = "متوفی با موفقیت ذخیره شد.",
//                 data = new
//                 {
//                     savedDeceased.UserId,
//                     savedDeceased.DeceasedId,
//                     savedDeceased.SavedAt
//                 }
//             });
//         }

//         // حذف متوفی از لیست ذخیره‌شده‌های کاربر
//         [HttpDelete]
//         public async Task<IActionResult> UnsaveDeceased([FromBody] SaveDeceasedRequest request)
//         {
//             var savedDeceased = await _context.SavedDeceaseds
//                 .FirstOrDefaultAsync(sd => sd.UserId == request.UserId && sd.DeceasedId == request.DeceasedId);

//             if (savedDeceased == null)
//             {
//                 return NotFound(new
//                 {
//                     status = 404,
//                     message = "متوفی در لیست ذخیره‌شده‌های کاربر یافت نشد."
//                 });
//             }

//             _context.SavedDeceaseds.Remove(savedDeceased);
//             await _context.SaveChangesAsync();

//             return Ok(new
//             {
//                 status = 200,
//                 message = "متوفی از لیست ذخیره‌شده‌های کاربر حذف شد."
//             });
//         }

//         // دریافت لیست متوفیان ذخیره‌شده توسط یک کاربر
//         [HttpGet("{userId}")]
// public async Task<IActionResult> GetSavedDeceaseds(int userId)
// {
//     var savedDeceaseds = await _context.SavedDeceaseds
//         .Where(sd => sd.UserId == userId)
//         .Include(sd => sd.Deceased) // شامل کردن اطلاعات متوفی
//         .Select(sd => new
//         {
//             sd.Deceased.Id,
//             sd.Deceased.FullName,
//             sd.Deceased.DateOfBirth,
//             sd.Deceased.DateOfMartyrdom,
//             PhotoUrls = sd.Deceased.PhotoUrls // لیست URL های تصاویر
//         })
//         .ToListAsync();

//     if (!savedDeceaseds.Any())
//     {
//         return NotFound(new
//         {
//             status = 404,
//             message = "هیچ متوفی‌ای در لیست ذخیره‌شده‌های کاربر یافت نشد."
//         });
//     }

//     return Ok(new
//     {
//         status = 200,
//         message = "لیست متوفیان ذخیره‌شده با موفقیت دریافت شد.",
//         data = savedDeceaseds
//     });
// }

        
//     }
// }