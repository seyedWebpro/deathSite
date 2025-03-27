using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Services;
using deathSite.Model;
using deathSite.View.GolestanShahid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace deathSite.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class GolestanShahidController : ControllerBase
    {
        private readonly apiContext _context;
        private readonly ILogger<GolestanShahidController> _logger;
        private readonly IFileUploadService _fileUploadService;




        public GolestanShahidController(apiContext context, ILogger<GolestanShahidController> logger, IFileUploadService fileUploadService)
        {
            _context = context;
            _logger = logger;
            _fileUploadService = fileUploadService;
        }

        private static readonly List<GolestanShohadaSection> FixedSections = new()
{
    new GolestanShohadaSection { Id = 1, Icon = null, Link = null },
    new GolestanShohadaSection { Id = 2, Icon = null, Link = null },
    new GolestanShohadaSection { Id = 3, Icon = null, Link = null },
    new GolestanShohadaSection { Id = 4, Icon = null, Link = null },
    new GolestanShohadaSection { Id = 5, Icon = null, Link = null },
    new GolestanShohadaSection { Id = 6, Icon = null, Link = null }
};


        [HttpPost("update-section")]
        public async Task<IActionResult> UpdateSection([FromForm] GolestanShohadaSectionDTO model)
        {
            _logger.LogInformation("شروع به‌روزرسانی سکشن با شناسه: {SectionId}", model.Id);

            // چک کردن اینکه آیا سکشن در لیست ثابت وجود دارد
            var section = FixedSections.FirstOrDefault(s => s.Id == model.Id);
            if (section == null)
            {
                _logger.LogWarning("سکشن با شناسه {SectionId} یافت نشد.", model.Id);
                return NotFound(new { StatusCode = 404, Message = "سکشن موردنظر یافت نشد." });
            }

            // اگر فایل جدید آپلود شده باشد
            if (model.IconFile != null)
            {
                if (!model.IconFile.ContentType.StartsWith("image/"))
                {
                    _logger.LogWarning("آپلود فایل نامعتبر برای سکشن {SectionId}. فقط تصاویر مجاز هستند.", model.Id);
                    return BadRequest(new { StatusCode = 400, Message = "فقط فایل‌های تصویری مجاز هستند." });
                }

                // حذف تصویر قبلی در صورت وجود
                if (!string.IsNullOrEmpty(section.Icon))
                {
                    string oldFileName = Path.GetFileName(section.Icon);
                    await _fileUploadService.DeleteFileAsync(oldFileName, "GolestanSections", section.Id);
                    _logger.LogInformation("تصویر قبلی سکشن {SectionId} حذف شد.", model.Id);
                }

                // آپلود تصویر جدید
                string uploadedFilePath = await _fileUploadService.UploadFileAsync(model.IconFile, "GolestanSections", section.Id);
                section.Icon = uploadedFilePath;
                _logger.LogInformation("تصویر جدید برای سکشن {SectionId} آپلود شد: {FilePath}", model.Id, uploadedFilePath);
            }

            // بروزرسانی لینک در صورت ارسال مقدار جدید
            if (!string.IsNullOrEmpty(model.Link))
            {
                section.Link = model.Link;
                _logger.LogInformation("لینک جدید برای سکشن {SectionId} تنظیم شد: {Link}", model.Id, model.Link);
            }

            return Ok(new { StatusCode = 200, Message = "سکشن با موفقیت بروزرسانی شد.", Data = section });
        }


        [HttpGet("get-sections")]
        public IActionResult GetSections()
        {
            return Ok(new { StatusCode = 200, Data = FixedSections });
        }


    }
}
