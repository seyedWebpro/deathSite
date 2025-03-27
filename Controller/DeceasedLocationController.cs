using System.Linq;
using System.Threading.Tasks;
using api.Context;
using deathSite.Model;
using deathSite.View.Dead;
using deathSite.View.DeadsManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace deathSite.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeceasedLocationController : ControllerBase
    {
        private readonly apiContext _context;

        public DeceasedLocationController(apiContext context)
        {
            _context = context;
        }

[HttpPost]
public async Task<IActionResult> UpsertLocation([FromBody] DeceasedLocationDto locationDto)
{
    // چک کردن اینکه آیا متوفی با شناسه مربوطه وجود دارد یا نه
    var deceased = await _context.Deceaseds
        .FirstOrDefaultAsync(d => d.Id == locationDto.DeceasedId && d.UserId == locationDto.UserId);

    if (deceased == null)
    {
        return NotFound(new { status = 404, message = "متوفی موردنظر برای این کاربر یافت نشد." });
    }

    var existingLocation = await _context.DeceasedLocations
        .FirstOrDefaultAsync(dl => dl.DeceasedId == locationDto.DeceasedId);

    if (existingLocation == null)
    {
        // ایجاد موقعیت جدید
        var newLocation = new DeceasedLocation
        {
            DeceasedId = locationDto.DeceasedId,
            Balad = locationDto.Balad,
            Neshan = locationDto.Neshan,
            GoogleMap = locationDto.GoogleMap,
            Mokhtasat = locationDto.Mokhtasat != null ? JsonConvert.SerializeObject(locationDto.Mokhtasat) : null
        };

        _context.DeceasedLocations.Add(newLocation);
        await _context.SaveChangesAsync();

        // استفاده از DTO برای ریسپانس
        var responseDto = new DeceasedLocationResponseDto
        {
            Id = newLocation.Id,
            DeceasedId = newLocation.DeceasedId,
            Balad = newLocation.Balad,
            Neshan = newLocation.Neshan,
            GoogleMap = newLocation.GoogleMap,
            Mokhtasat = newLocation.Mokhtasat != null ? JsonConvert.DeserializeObject<double[]>(newLocation.Mokhtasat) : null
        };

        return StatusCode(201, new { status = 201, message = "موقعیت جدید اضافه شد.", data = responseDto });
    }
    else
    {
        // به‌روزرسانی موقعیت موجود
        existingLocation.Balad = locationDto.Balad;
        existingLocation.Neshan = locationDto.Neshan;
        existingLocation.GoogleMap = locationDto.GoogleMap;
        existingLocation.Mokhtasat = locationDto.Mokhtasat != null ? JsonConvert.SerializeObject(locationDto.Mokhtasat) : existingLocation.Mokhtasat;

        await _context.SaveChangesAsync();

        // استفاده از DTO برای ریسپانس
        var responseDto = new DeceasedLocationResponseDto
        {
            Id = existingLocation.Id,
            DeceasedId = existingLocation.DeceasedId,
            Balad = existingLocation.Balad,
            Neshan = existingLocation.Neshan,
            GoogleMap = existingLocation.GoogleMap,
            Mokhtasat = existingLocation.Mokhtasat != null ? JsonConvert.DeserializeObject<double[]>(existingLocation.Mokhtasat) : null
        };

        return Ok(new { status = 200, message = "موقعیت به‌روزرسانی شد.", data = responseDto });
    }
}


        [HttpGet("{deceasedId}")]
        public async Task<IActionResult> GetLocation(int deceasedId)
        {
            var location = await _context.DeceasedLocations.FirstOrDefaultAsync(dl => dl.DeceasedId == deceasedId);
            if (location == null)
            {
                return NotFound(new { status = 404, message = "موقعیت متوفی یافت نشد." });
            }

            return Ok(new 
            { 
                status = 200, 
                message = "موقعیت یافت شد.", 
                data = new 
                {
                    location.Id,
                    location.DeceasedId,
                    location.Balad,
                    location.Neshan,
                    location.GoogleMap,
                    Mokhtasat = string.IsNullOrEmpty(location.Mokhtasat) ? null : JsonConvert.DeserializeObject<double[]>(location.Mokhtasat)
                }
            });
        }

        [HttpDelete("{deceasedId}")]
        public async Task<IActionResult> DeleteLocation(int deceasedId)
        {
            var location = await _context.DeceasedLocations.FirstOrDefaultAsync(dl => dl.DeceasedId == deceasedId);
            if (location == null)
            {
                return NotFound(new { status = 404, message = "موقعیت متوفی یافت نشد." });
            }

            _context.DeceasedLocations.Remove(location);
            await _context.SaveChangesAsync();
            return Ok(new { status = 200, message = "موقعیت حذف شد." });
        }
    }
}
