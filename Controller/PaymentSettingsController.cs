using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using deathSite.View.PaymentMelat;
using deathSite.View.PaymentParsian;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using api.Context;
using deathSite.Model;
using deathSite.View;

namespace deathSite.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentSettingsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _configFilePath;
        private readonly apiContext _context;
        private readonly ILogger<PaymentSettingsController> _logger;

        public PaymentSettingsController(IConfiguration configuration, IWebHostEnvironment env, 
                                         apiContext context, ILogger<PaymentSettingsController> logger)
        {
            _configuration = configuration;
            _configFilePath = Path.Combine(env.ContentRootPath, "appsettings.json");
            _context = context;
            _logger = logger;
        }

        // متد کمکی برای دریافت تنظیمات از دیتابیس - بدون استفاده از کش
        private async Task<Dictionary<string, string>> GetGatewaySettingsAsync(string gatewayName)
        {
            return await _context.PaymentSettings
                .Where(p => p.GatewayName == gatewayName)
                .ToDictionaryAsync(p => p.Key, p => p.Value);
        }

        // متد کمکی برای به‌روزرسانی یک تنظیم در دیتابیس - بدون کش
        private async Task<bool> UpdateGatewaySettingAsync(string gatewayName, string key, string value)
        {
            try
            {
                var setting = await _context.PaymentSettings
                    .FirstOrDefaultAsync(p => p.GatewayName == gatewayName && p.Key == key);
                
                if (setting == null)
                {
                    setting = new PaymentSettings
                    {
                        GatewayName = gatewayName,
                        Key = key,
                        Value = value,
                        LastUpdated = DateTime.Now
                    };
                    await _context.PaymentSettings.AddAsync(setting);
                }
                else
                {
                    setting.Value = value;
                    setting.LastUpdated = DateTime.Now;
                    _context.PaymentSettings.Update(setting);
                }
                
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در به‌روزرسانی تنظیم {key} برای درگاه {gatewayName}");
                return false;
            }
        }

        // متد کمکی برای به‌روزرسانی چندین تنظیم در دیتابیس - بدون کش
        private async Task<bool> UpdateGatewaySettingsAsync(string gatewayName, Dictionary<string, string> settings)
        {
            try
            {
                foreach (var setting in settings)
                {
                    await UpdateGatewaySettingAsync(gatewayName, setting.Key, setting.Value);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در به‌روزرسانی تنظیمات درگاه {gatewayName}");
                return false;
            }
        }

        // افزودن متد برای دریافت وضعیت فعال بودن درگاه
        private async Task<bool> IsGatewayActiveAsync(string gatewayName)
        {
            var settings = await GetGatewaySettingsAsync(gatewayName);
            return settings.ContainsKey("IsActive") && settings["IsActive"].ToLower() == "true";
        }

        // افزودن متد برای تغییر وضعیت فعال بودن درگاه
        private async Task<bool> SetGatewayActiveStatusAsync(string gatewayName, bool isActive)
        {
            return await UpdateGatewaySettingAsync(gatewayName, "IsActive", isActive.ToString());
        }

        [HttpGet("melat")]
        public async Task<IActionResult> GetMelatSettings()
        {
            try
            {
                var settings = await GetGatewaySettingsAsync("Melat");
                
                var dto = new MelatSettingsDto
                {
                    TerminalID = settings.ContainsKey("TerminalID") ? long.Parse(settings["TerminalID"]) : 0,
                    Username = settings.ContainsKey("Username") ? settings["Username"] : string.Empty,
                    Password = settings.ContainsKey("Password") ? settings["Password"] : string.Empty,
                    CallBackUrl = settings.ContainsKey("CallBackUrl") ? settings["CallBackUrl"] : string.Empty,
                    IsActive = settings.ContainsKey("IsActive") && settings["IsActive"].ToLower() == "true"
                };

                return Ok(new { message = "تنظیمات درگاه پرداخت بانک ملت با موفقیت دریافت شد.", settings = dto });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت تنظیمات بانک ملت");
                return StatusCode(500, new { message = "خطا در دریافت تنظیمات بانک ملت.", error = ex.Message });
            }
        }

        [HttpPost("melat")]
        public async Task<IActionResult> UpdateMelatSettings([FromBody] MelatSettingsDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "اطلاعات وارد شده نامعتبر است.", errors = ModelState });
            }

            try
            {
                var settings = new Dictionary<string, string>();
                
                if (model.TerminalID.HasValue)
                    settings["TerminalID"] = model.TerminalID.Value.ToString();
                    
                if (!string.IsNullOrWhiteSpace(model.Username))
                    settings["Username"] = model.Username;
                    
                if (!string.IsNullOrWhiteSpace(model.Password))
                    settings["Password"] = model.Password;
                    
                if (!string.IsNullOrWhiteSpace(model.CallBackUrl))
                    settings["CallBackUrl"] = model.CallBackUrl;

                // اضافه کردن وضعیت فعال بودن به تنظیمات
                settings["IsActive"] = model.IsActive.ToString();

                if (settings.Count > 0)
                {
                    await UpdateGatewaySettingsAsync("Melat", settings);
                }

                return Ok(new { message = "تنظیمات درگاه پرداخت بانک ملت با موفقیت به‌روزرسانی شد.", statusCode = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در به‌روزرسانی تنظیمات بانک ملت");
                return StatusCode(500, new { message = "خطای داخلی سرور.", error = ex.Message });
            }
        }

        [HttpGet("parsian")]
        public async Task<IActionResult> GetParsianSettings()
        {
            try
            {
                var settings = await GetGatewaySettingsAsync("Parsian");
                
                var dto = new ParsianSettingsDto
                {
                    PIN = settings.ContainsKey("PIN") ? settings["PIN"] : string.Empty,
                    CallBackUrl = settings.ContainsKey("CallBackUrl") ? settings["CallBackUrl"] : string.Empty,
                    IsActive = settings.ContainsKey("IsActive") && settings["IsActive"].ToLower() == "true"
                };

                return Ok(new { message = "تنظیمات درگاه پرداخت بانک پارسیان با موفقیت دریافت شد.", settings = dto });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در دریافت تنظیمات بانک پارسیان");
                return StatusCode(500, new { message = "خطا در دریافت تنظیمات بانک پارسیان.", error = ex.Message });
            }
        }

        [HttpPost("parsian")]
        public async Task<IActionResult> UpdateParsianSettings([FromBody] ParsianSettingsDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "اطلاعات وارد شده نامعتبر است.", errors = ModelState });
            }

            try
            {
                var settings = new Dictionary<string, string>();
                
                if (!string.IsNullOrWhiteSpace(model.PIN))
                    settings["PIN"] = model.PIN;
                    
                if (!string.IsNullOrWhiteSpace(model.CallBackUrl))
                    settings["CallBackUrl"] = model.CallBackUrl;

                // اضافه کردن وضعیت فعال بودن به تنظیمات
                settings["IsActive"] = model.IsActive.ToString();

                if (settings.Count > 0)
                {
                    await UpdateGatewaySettingsAsync("Parsian", settings);
                }

                return Ok(new { message = "تنظیمات درگاه پرداخت بانک پارسیان با موفقیت به‌روزرسانی شد.", statusCode = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در به‌روزرسانی تنظیمات بانک پارسیان");
                return StatusCode(500, new { message = "خطای داخلی سرور.", error = ex.Message });
            }
        }

        // متد جدید برای فعال/غیرفعال کردن درگاه‌ها
        [HttpPost("{gatewayName}/status")]
        public async Task<IActionResult> ToggleGatewayStatus(string gatewayName, [FromBody] StatusUpdateDto model)
        {
            if (gatewayName != "Melat" && gatewayName != "Parsian")
            {
                return BadRequest(new { message = "نام درگاه نامعتبر است." });
            }

            try
            {
                await SetGatewayActiveStatusAsync(gatewayName, model.IsActive);
                string statusMessage = model.IsActive ? "فعال" : "غیرفعال"; 
                return Ok(new { message = $"درگاه پرداخت {gatewayName} با موفقیت {statusMessage} شد.", statusCode = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در تغییر وضعیت درگاه {gatewayName}");
                return StatusCode(500, new { message = "خطای داخلی سرور.", error = ex.Message });
            }
            
                }
                }
}