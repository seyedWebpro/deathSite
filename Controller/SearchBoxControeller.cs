using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using deathSite.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace deathSite.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchBoxControeller : ControllerBase
    {

        private readonly ILogger<SearchBoxControeller> _logger;
        private readonly apiContext _context;


        public SearchBoxControeller(ILogger<SearchBoxControeller> logger, apiContext context)
        {
            _logger = logger;
            _context = context;
        }

[HttpGet("search")]
public async Task<IActionResult> Search(string query)
{
    try
    {
        _logger.LogInformation("شروع جستجو برای عبارت: {Query}", query);

        var shahids = await _context.shahids
            .Where(s => s.FullName != null && s.FullName.Contains(query))
            .Select(s => new SearchResultDto
            {
                Id = s.Id,
                FullName = s.FullName ?? "نام موجود نیست",
                Biography = s.Biography ?? "بیوگرافی موجود نیست",
                FatherName = s.FatherName ?? "نام پدر موجود نیست",
                CoverPhoto = s.PhotoUrls.FirstOrDefault() ?? "عکس موجود نیست", // مقداردهی امن
                Type = "Shahid"
            })
            .ToListAsync();

        var deceased = await _context.Deceaseds
            .Where(d => d.FullName != null && d.FullName.Contains(query))
            .Select(d => new SearchResultDto
            {
                Id = d.Id,
                FullName = d.FullName ?? "نام موجود نیست",
                Biography = d.Description ?? "بیوگرافی موجود نیست",
                FatherName = d.FatherName ?? "نام پدر موجود نیست",
                CoverPhoto = d.CoverPhotoUrl ?? "عکس موجود نیست",
                Type = "Deceased"
            })
            .ToListAsync();

        var result = shahids.Concat(deceased).ToList();
        _logger.LogInformation("تعداد کل نتایج جستجو: {Count}", result.Count);

        return Ok(result);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "خطای داخلی در هنگام جستجو برای عبارت: {Query}", query);
        return StatusCode(500, new { Message = "خطای داخلی سرور.", Error = ex.Message });
    }
}


        // [HttpGet("search")]
        // public async Task<IActionResult> Search(string query)
        // {
        //     try
        //     {
        //         _logger.LogInformation("شروع جستجو برای عبارت: {Query}", query);

        //         var shahids = await _context.shahids
        //             .Where(s => s.FullName != null && s.FullName.Contains(query))
        //             .Select(s => new SearchResultDto
        //             {
        //                 Id = s.Id,
        //                 FullName = s.FullName ?? "نام موجود نیست", // بررسی null
        //                 Biography = s.Biography ?? "بیوگرافی موجود نیست", // بررسی null
        //                 FatherName = s.FatherName ?? "نام پدر موجود نیست", // بررسی null
        //                 Type = "Shahid"
        //             })
        //             .ToListAsync();

        //         var deceased = await _context.Deceaseds
        //             .Where(d => d.FullName != null && d.FullName.Contains(query))
        //             .Select(d => new SearchResultDto
        //             {
        //                 Id = d.Id,
        //                 FullName = d.FullName ?? "نام موجود نیست", // بررسی null
        //                 Biography = d.Description ?? "بیوگرافی موجود نیست", // بررسی null
        //                 FatherName = d.FatherName ?? "نام پدر موجود نیست", // بررسی null
        //                 CoverPhoto = d.CoverPhotoUrl ?? "عکس موجود نیست", // بررسی null
        //                 Type = "Deceased"
        //             })
        //             .ToListAsync();

        //         var result = shahids.Concat(deceased).ToList();
        //         _logger.LogInformation("تعداد کل نتایج جستجو: {Count}", result.Count);

        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "خطای داخلی در هنگام جستجو برای عبارت: {Query}", query);
        //         return StatusCode(500, new { Message = "خطای داخلی سرور.", Error = ex.Message });
        //     }
        // }


    }
}