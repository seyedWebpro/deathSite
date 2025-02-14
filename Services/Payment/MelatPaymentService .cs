// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.ServiceModel;
// using System.Threading.Tasks;
// using api.View.PaymentMelat;
// using ServiceReference;

// namespace deathSite.Services.Payment
// {
//     public class MelatPaymentService : IMelatPaymentService
//     {
//         private readonly ILogger<MelatPaymentService> _logger;
//         private readonly PaymentGatewayClient _client;

//         public MelatPaymentService(ILogger<MelatPaymentService> logger)
//         {
//             _logger = logger;
//             var endpoint = new EndpointAddress("https://bpm.shaparak.ir/pgwchannel/services/pgw?wsdl");
//             var binding = new BasicHttpBinding
//             {
//                 Security = new BasicHttpSecurity
//                 {
//                     Mode = BasicHttpSecurityMode.Transport
//                 }
//             };

//             _client = new PaymentGatewayClient(binding, endpoint);
//         }

//         public async Task<(bool Success, string Message, string RefId)> PayRequestAsync(PaymentRequestDto request)
//         {
//             try
//             {
//                 var response = await _client.bpPayRequestAsync(
//                     request.TerminalId,
//                     request.Username,
//                     request.Password,
//                     request.OrderId,
//                     request.Amount,
//                     request.LocalDate,
//                     request.LocalTime,
//                     request.AdditionalData,
//                     request.CallBackUrl,
//                     request.PayerId,
//                     request.MobileNo,
//                     request.EncPan,
//                     request.PanHiddenMode,
//                     request.CartItem,
//                     request.Enc
//                 );

//                 var resultCode = response.Body.@return;
//                 _logger.LogInformation("bpPayRequest Response: {Response}", resultCode);
//                 var responseParts = resultCode.Split(',');

//                 if (responseParts[0] == "0")
//                 {
//                     // دریافت RefId از پاسخ
//                     var refId = responseParts[1];
//                     return (true, "Request successful", refId);
//                 }
//                 else
//                 {
//                     return (false, $"Request failed, Code: {responseParts[0]}", null);
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error in PayRequest");
//                 return (false, ex.Message, null);
//             }
//         }

//         public async Task<(bool Success, string Message)> VerifyRequestAsync(VerifyRequestDto request)
//         {
//             try
//             {
//                 var response = await _client.bpVerifyRequestAsync(
//                     request.TerminalId,
//                     request.Username,
//                     request.Password,
//                     request.OrderId,
//                     request.SaleOrderId,
//                     request.SaleReferenceId
//                 );

//                 var resultCode = response.Body.@return;
//                 _logger.LogInformation("bpVerifyRequest Response: {Response}", resultCode);

//                 if (resultCode == "0")
//                 {
//                     return (true, "Transaction verified successfully");
//                 }
//                 else
//                 {
//                     return (false, $"Verification failed, Code: {resultCode}");
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error in VerifyRequest");
//                 return (false, ex.Message);
//             }
//         }

//         public async Task<(bool Success, string Message)> SettleRequestAsync(SettleRequestDto request)
//         {
//             try
//             {
//                 var response = await _client.bpSettleRequestAsync(
//                     request.TerminalId,
//                     request.Username,
//                     request.Password,
//                     request.OrderId,
//                     request.SaleOrderId,
//                     request.SaleReferenceId
//                 );

//                 var resultCode = response.Body.@return;
//                 _logger.LogInformation("bpSettleRequest Response: {Response}", resultCode);

//                 if (resultCode == "0")
//                 {
//                     return (true, "Settlement successful");
//                 }
//                 else
//                 {
//                     return (false, $"Settlement failed, Code: {resultCode}");
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error in SettleRequest");
//                 return (false, ex.Message);
//             }
//         }
//     }
// }

// اضافه کردن خواندن برخی موارد از تنظیمات

// using System;
// using System.ServiceModel;
// using System.Threading.Tasks;
// using api.View.PaymentMelat;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Logging;
// using ServiceReference;

// namespace deathSite.Services.Payment
// {
//     public class MelatPaymentService : IMelatPaymentService
//     {
//         private readonly ILogger<MelatPaymentService> _logger;
//         private readonly PaymentGatewayClient _client;

//         private readonly long _terminalID;
//         private readonly string _username;
//         private readonly string _password;
//         private readonly string _callBackUrl;

//         public MelatPaymentService(ILogger<MelatPaymentService> logger, IConfiguration configuration)
//         {
//             _logger = logger;

//             // دریافت مستقیم تنظیمات از IConfiguration
//             _terminalID = configuration.GetValue<long>("MelatSettings:TerminalID");
//             _username = configuration["MelatSettings:Username"];
//             _password = configuration["MelatSettings:Password"];
//             _callBackUrl = configuration["MelatSettings:CallBackUrl"];

//             var endpoint = new EndpointAddress("https://bpm.shaparak.ir/pgwchannel/services/pgw?wsdl");
//             var binding = new BasicHttpBinding
//             {
//                 Security = new BasicHttpSecurity
//                 {
//                     Mode = BasicHttpSecurityMode.Transport
//                 }
//             };

//             _client = new PaymentGatewayClient(binding, endpoint);
//         }

//         public async Task<(bool Success, string Message, string RefId)> PayRequestAsync(PaymentRequestDto request)
//         {
//             try
//             {
//                 var response = await _client.bpPayRequestAsync(
//                     _terminalID,             // از تنظیمات خوانده شده
//                     _username,               // از تنظیمات خوانده شده
//                     _password,               // از تنظیمات خوانده شده
//                     request.OrderId,
//                     request.Amount,
//                     request.LocalDate,
//                     request.LocalTime,
//                     request.AdditionalData,
//                     _callBackUrl,            // از تنظیمات خوانده شده
//                     request.PayerId,
//                     request.MobileNo,
//                     request.EncPan,
//                     request.PanHiddenMode,
//                     request.CartItem,
//                     request.Enc
//                 );

//                 var resultCode = response.Body.@return;
//                 _logger.LogInformation("bpPayRequest Response: {Response}", resultCode);
//                 var responseParts = resultCode.Split(',');

//                 if (responseParts[0] == "0")
//                 {
//                     var refId = responseParts[1];
//                     return (true, "Request successful", refId);
//                 }
//                 else
//                 {
//                     return (false, $"Request failed, Code: {responseParts[0]}", null);
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error in PayRequest");
//                 return (false, ex.Message, null);
//             }
//         }

//         public async Task<(bool Success, string Message)> VerifyRequestAsync(VerifyRequestDto request)
//         {
//             try
//             {
//                 var response = await _client.bpVerifyRequestAsync(
//                     _terminalID,             // از تنظیمات خوانده شده
//                     _username,
//                     _password,
//                     request.OrderId,
//                     request.SaleOrderId,
//                     request.SaleReferenceId
//                 );

//                 var resultCode = response.Body.@return;
//                 _logger.LogInformation("bpVerifyRequest Response: {Response}", resultCode);

//                 if (resultCode == "0")
//                 {
//                     return (true, "Transaction verified successfully");
//                 }
//                 else
//                 {
//                     return (false, $"Verification failed, Code: {resultCode}");
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error in VerifyRequest");
//                 return (false, ex.Message);
//             }
//         }

//         public async Task<(bool Success, string Message)> SettleRequestAsync(SettleRequestDto request)
//         {
//             try
//             {
//                 var response = await _client.bpSettleRequestAsync(
//                     _terminalID,             // از تنظیمات خوانده شده
//                     _username,
//                     _password,
//                     request.OrderId,
//                     request.SaleOrderId,
//                     request.SaleReferenceId
//                 );

//                 var resultCode = response.Body.@return;
//                 _logger.LogInformation("bpSettleRequest Response: {Response}", resultCode);

//                 if (resultCode == "0")
//                 {
//                     return (true, "Settlement successful");
//                 }
//                 else
//                 {
//                     return (false, $"Settlement failed, Code: {resultCode}");
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error in SettleRequest");
//                 return (false, ex.Message);
//             }
//         }
//     }
// }

// حذف فیلد های غیر ضروری

// using System;
// using System.ServiceModel;
// using System.Threading.Tasks;
// using api.View.PaymentMelat;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Logging;
// using ServiceReference;

// namespace deathSite.Services.Payment
// {
//     public class MelatPaymentService : IMelatPaymentService
//     {
//         private readonly ILogger<MelatPaymentService> _logger;
//         private readonly PaymentGatewayClient _client;

//         private readonly long _terminalID;
//         private readonly string _username;
//         private readonly string _password;
//         private readonly string _callBackUrl;

//         public MelatPaymentService(ILogger<MelatPaymentService> logger, IConfiguration configuration)
//         {
//             _logger = logger;

//             // دریافت مستقیم تنظیمات از IConfiguration
//             _terminalID = configuration.GetValue<long>("MelatSettings:TerminalID");
//             _username = configuration["MelatSettings:Username"];
//             _password = configuration["MelatSettings:Password"];
//             _callBackUrl = configuration["MelatSettings:CallBackUrl"];

//             var endpoint = new EndpointAddress("https://bpm.shaparak.ir/pgwchannel/services/pgw?wsdl");
//             var binding = new BasicHttpBinding
//             {
//                 Security = new BasicHttpSecurity
//                 {
//                     Mode = BasicHttpSecurityMode.Transport
//                 }
//             };

//             _client = new PaymentGatewayClient(binding, endpoint);
//         }

//         public async Task<(bool Success, string Message, string RefId)> PayRequestAsync(PaymentRequestDto request)
//         {
//             try
//             {
//                 // تولید خودکار تاریخ و زمان
//                 var localDate = DateTime.Now.ToString("yyyyMMdd");
//                 var localTime = DateTime.Now.ToString("HHmmss");

//                 var response = await _client.bpPayRequestAsync(
//                     _terminalID,             // از تنظیمات خوانده شده
//                     _username,               // از تنظیمات خوانده شده
//                     _password,               // از تنظیمات خوانده شده
//                     request.OrderId,
//                     request.Amount,
//                     localDate,               // به صورت خودکار تولید شده
//                     localTime,               // به صورت خودکار تولید شده
//                     string.Empty,            // additionalData (خالی)
//                     _callBackUrl,            // از تنظیمات خوانده شده
//                     request.PayerId,            // payerId (خالی)
//                     string.Empty,            // mobileNo (خالی)
//                     string.Empty,            // encPan (خالی)
//                     string.Empty,            // panHiddenMode (خالی)
//                     string.Empty,            // cartItem (خالی)
//                     string.Empty             // enc (خالی)
//                 );

//                 var resultCode = response.Body.@return;
//                 _logger.LogInformation("bpPayRequest Response: {Response}", resultCode);
//                 var responseParts = resultCode.Split(',');

//                 if (responseParts[0] == "0")
//                 {
//                     var refId = responseParts[1];
//                     return (true, "Request successful", refId);
//                 }
//                 else
//                 {
//                     return (false, $"Request failed, Code: {responseParts[0]}", null);
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error in PayRequest");
//                 return (false, ex.Message, null);
//             }
//         }

//         public async Task<(bool Success, string Message)> VerifyRequestAsync(VerifyRequestDto request)
//         {
//             try
//             {
//                 var response = await _client.bpVerifyRequestAsync(
//                     _terminalID,             // از تنظیمات خوانده شده
//                     _username,
//                     _password,
//                     request.OrderId,
//                     request.SaleOrderId,
//                     request.SaleReferenceId
//                 );

//                 var resultCode = response.Body.@return;
//                 _logger.LogInformation("bpVerifyRequest Response: {Response}", resultCode);

//                 if (resultCode == "0")
//                 {
//                     return (true, "Transaction verified successfully");
//                 }
//                 else
//                 {
//                     return (false, $"Verification failed, Code: {resultCode}");
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error in VerifyRequest");
//                 return (false, ex.Message);
//             }
//         }

//         public async Task<(bool Success, string Message)> SettleRequestAsync(SettleRequestDto request)
//         {
//             try
//             {
//                 var response = await _client.bpSettleRequestAsync(
//                     _terminalID,             // از تنظیمات خوانده شده
//                     _username,
//                     _password,
//                     request.OrderId,
//                     request.SaleOrderId,
//                     request.SaleReferenceId
//                 );

//                 var resultCode = response.Body.@return;
//                 _logger.LogInformation("bpSettleRequest Response: {Response}", resultCode);

//                 if (resultCode == "0")
//                 {
//                     return (true, "Settlement successful");
//                 }
//                 else
//                 {
//                     return (false, $"Settlement failed, Code: {resultCode}");
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error in SettleRequest");
//                 return (false, ex.Message);
//             }
//         }
//     }
// }


// خواندن اطلاعات فرم دیتا بعد از پرداخت کاربر
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using api.View.PaymentMelat;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceReference;

namespace deathSite.Services.Payment
{
    public class MelatPaymentService : IMelatPaymentService
    {
        private readonly ILogger<MelatPaymentService> _logger;
        private readonly PaymentGatewayClient _client;

        private readonly long _terminalID;
        private readonly string _username;
        private readonly string _password;
        private readonly string _callBackUrl;

        public MelatPaymentService(ILogger<MelatPaymentService> logger, IConfiguration configuration)
        {
            _logger = logger;

            // دریافت تنظیمات از IConfiguration
            _terminalID = configuration.GetValue<long>("MelatSettings:TerminalID");
            _username = configuration["MelatSettings:Username"];
            _password = configuration["MelatSettings:Password"];
            // اطمینان حاصل کنید که CallBackUrl به صورت کامل و صحیح (مثلاً با مسیر کامل API) تنظیم شده است
            _callBackUrl = configuration["MelatSettings:CallBackUrl"];

            var endpoint = new EndpointAddress("https://bpm.shaparak.ir/pgwchannel/services/pgw?wsdl");
            var binding = new BasicHttpBinding
            {
                Security = new BasicHttpSecurity
                {
                    Mode = BasicHttpSecurityMode.Transport
                }
            };

            _client = new PaymentGatewayClient(binding, endpoint);
        }

        public async Task<(bool Success, string Message, string RefId)> PayRequestAsync(PaymentRequestDto request)
        {
            try
            {
                var localDate = DateTime.Now.ToString("yyyyMMdd");
                var localTime = DateTime.Now.ToString("HHmmss");

                var response = await _client.bpPayRequestAsync(
                    _terminalID,
                    _username,
                    _password,
                    request.OrderId,
                    request.Amount,
                    localDate,
                    localTime,
                    string.Empty,        // additionalData
                    _callBackUrl,        // استفاده از callback تنظیم‌شده در appsettings.json
                    request.PayerId,
                    string.Empty,        // mobileNo
                    string.Empty,        // encPan
                    string.Empty,        // panHiddenMode
                    string.Empty,        // cartItem
                    string.Empty         // enc
                );

                var resultCode = response.Body.@return;
                _logger.LogInformation("bpPayRequest Response: {Response}", resultCode);
                var responseParts = resultCode.Split(',');

                if (responseParts[0] == "0")
                {
                    var refId = responseParts[1];
                    return (true, "Request successful", refId);
                }
                else
                {
                    return (false, $"Request failed, Code: {responseParts[0]}", null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PayRequest");
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool Success, string Message)> VerifyRequestAsync(VerifyRequestDto request)
        {
            try
            {
                var response = await _client.bpVerifyRequestAsync(
                    _terminalID,
                    _username,
                    _password,
                    request.OrderId,
                    request.SaleOrderId,
                    request.SaleReferenceId
                );

                var resultCode = response.Body.@return;
                _logger.LogInformation("bpVerifyRequest Response: {Response}", resultCode);

                if (resultCode == "0")
                {
                    return (true, "Transaction verified successfully");
                }
                else
                {
                    return (false, $"Verification failed, Code: {resultCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VerifyRequest");
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> SettleRequestAsync(SettleRequestDto request)
        {
            try
            {
                var response = await _client.bpSettleRequestAsync(
                    _terminalID,
                    _username,
                    _password,
                    request.OrderId,
                    request.SaleOrderId,
                    request.SaleReferenceId
                );

                var resultCode = response.Body.@return;
                _logger.LogInformation("bpSettleRequest Response: {Response}", resultCode);

                if (resultCode == "0")
                {
                    return (true, "Settlement successful");
                }
                else
                {
                    return (false, $"Settlement failed, Code: {resultCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SettleRequest");
                return (false, ex.Message);
            }
        }
    }
}
