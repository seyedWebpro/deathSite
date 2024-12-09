using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Model.AdminModel;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class SmsService : ISmsService
    {

        private readonly HttpClient _httpClient;
        private readonly ICalculatorService _codeGenerator;
        private readonly ICacheService _cacheService;
        private readonly apiContext _context;

        private readonly string _userName = "tarhimcode"; // نام کاربری ثابت
        private readonly string _password = "6900023"; // کلمه عبور ثابت
        private readonly string _domainName = "sepahansms"; // نام دامنه ثابت
        private readonly string _senderNumber = "30006403868611"; // شماره فرستنده ثابت

        public SmsService(HttpClient httpClient, ICalculatorService codeGenerator, ICacheService cacheService, apiContext context)
        {
            _httpClient = httpClient;
            _codeGenerator = codeGenerator;
            _cacheService = cacheService;
            _context = context;
        }

        public async Task SendAuthenticationSmsAsync(string phoneNumber)
        {
            var code = _codeGenerator.GenerateRandomCode();
            _cacheService.Set(phoneNumber, code, TimeSpan.FromMinutes(5));

            var template = await _context.smsTemplates.FirstOrDefaultAsync(t => t.MessageType == "Authentication");
            var message = template != null ? $"{template.Message} {code}" : $"کد احراز هویت شما: {code}";

            // لاگ‌گذاری برای بررسی
            Console.WriteLine($"Sending SMS to {phoneNumber}: {message}");

            await SendSmsAsync(phoneNumber, message);
        }

        public async Task SendPasswordResetSmsAsync(string phoneNumber)
        {
            var code = _codeGenerator.GenerateRandomCode();
            _cacheService.Set(phoneNumber, code, TimeSpan.FromMinutes(5));

            var template = await _context.smsTemplates.FirstOrDefaultAsync(t => t.MessageType == "PasswordReset");
            var message = template != null ? $"{template.Message} {code}" : $"کد بازیابی رمز عبور شما: {code}";

            // لاگ‌گذاری برای بررسی
            Console.WriteLine($"Sending SMS to {phoneNumber}: {message}");

            await SendSmsAsync(phoneNumber, message);
        }

        public async Task SendOrderSuccessSmsAsync(string phoneNumber)
        {
            var template = await _context.smsTemplates.FirstOrDefaultAsync(t => t.MessageType == "OrderSuccess");
            var message = template != null ? template.Message : "سفارش شما با موفقیت ثبت شد.";
            await SendSmsAsync(phoneNumber, message);
        }

        public async Task SendRenewalSuccessSmsAsync(string phoneNumber)
        {
            var template = await _context.smsTemplates.FirstOrDefaultAsync(t => t.MessageType == "RenewalSuccess");
            var message = template != null ? template.Message : "تمدید شما با موفقیت انجام شد.";
            await SendSmsAsync(phoneNumber, message);
        }

        public async Task SendUpgradeSuccessSmsAsync(string phoneNumber)
        {
            var template = await _context.smsTemplates.FirstOrDefaultAsync(t => t.MessageType == "UpgradeSuccess");
            var message = template != null ? template.Message : "ارتقاء شما با موفقیت انجام شد.";
            await SendSmsAsync(phoneNumber, message);
        }

        public async Task SendRenewalReminderSmsAsync(string phoneNumber)
        {
            var template = await _context.smsTemplates.FirstOrDefaultAsync(t => t.MessageType == "RenewalReminder");
            var message = template != null ? template.Message : "یادآوری: زمان تمدید شما نزدیک است.";
            await SendSmsAsync(phoneNumber, message);
        }

        private async Task SendSmsAsync(string phoneNumber, string message)
        {
            var url = $"http://sepahansms.com/sendSmsViaURL.aspx?userName={_userName}&password={_password}&domainName={_domainName}&smsText={message}&reciverNumber={phoneNumber}&senderNumber={_senderNumber}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error sending SMS: {response.StatusCode} - {errorMessage}");
            }
        }

    }
}