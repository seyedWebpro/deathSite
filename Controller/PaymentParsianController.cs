using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.View.PaymentParsian;
using deathSite.Services.Payment;
using System.Web;
using deathSite.Services.PackageService;
using api.Context;
using Microsoft.EntityFrameworkCore;
using deathSite.Model;
using deathSite.View.PaymentParsian;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentParsianController : ControllerBase
    {
        private readonly IParsianPaymentService _paymentService;
        private readonly IPackageTransactionService _packageTransactionService;
        private readonly ILogger<PaymentParsianController> _logger;

        private readonly apiContext _dbContext;

        public PaymentParsianController(IParsianPaymentService paymentService,
         ILogger<PaymentParsianController> logger,
            apiContext dbContext,
            IPackageTransactionService packageTransactionService)
        {
            _paymentService = paymentService;
            _logger = logger;
            _dbContext = dbContext;
            _packageTransactionService = packageTransactionService;
        }

        // درخواست تمدید پکیج



        // [HttpPost("RequestPayment2")]
        // public async Task<IActionResult> RequestPayment2([FromBody] PaymentRequestModel model)
        // {
        //     var result = await _paymentService.RequestPaymentAsync(model);
        //     if (result.Success)
        //     {
        //         return Ok(new { Token = result.Token, PaymentUrl = result.PaymentUrl });
        //     }
        //     return BadRequest(new { Message = result.Message });
        // }


        // [HttpPost("VerifyPayment")]
        // public async Task<IActionResult> VerifyPayment([FromBody] PaymentVerifyModel model)
        // {
        //     var result = await _paymentService.VerifyPaymentAsync(model);
        //     if (result.Success)
        //     {
        //         return Ok(result.Result);
        //     }
        //     return BadRequest(new { Message = result.Message });
        // }

        [HttpPost("RequestPackagePayment")]
        public async Task<IActionResult> RequestPackagePayment([FromBody] ExtendedPaymentParisinaRequestDto request)
        {
            // بررسی مدل ورودی
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Package Payment Request: {@Request}", request);
                return BadRequest(ModelState);
            }

            long orderId = DateTime.UtcNow.Ticks;  // استفاده از Ticks برای OrderId به صورت long

            // اعتبارسنجی فیلدهای ضروری مربوط به درگاه
            if (request.Amount <= 0)
            {
                _logger.LogWarning("Invalid Amount in Package Payment Request: {@Request}", request);
                return BadRequest(new { Message = "Amount must be greater than zero." });
            }

            // بررسی اینکه آیا پرداخت مربوط به پکیج است یا خیر
            bool isPackagePayment = request.UserId.HasValue && request.UserId.Value > 0 &&
                                    request.PackageId.HasValue && request.PackageId.Value > 0;
            Factors factor = null;

            if (isPackagePayment)
            {
                // بررسی وجود کاربر
                var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == request.UserId.Value);
                if (user == null)
                {
                    _logger.LogWarning("User not found for Package Payment Request: {@Request}", request);
                    return BadRequest(new { Message = "User not found." });
                }

                // بررسی وجود پکیج
                var package = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == request.PackageId.Value);
                if (package == null)
                {
                    _logger.LogWarning("Package not found for Package Payment Request: {@Request}", request);
                    return BadRequest(new { Message = "Package not found." });
                }

                // ایجاد رکورد فاکتور با وضعیت Pending
                factor = new Factors
                {
                    UserId = request.UserId.Value,
                    PackageId = request.PackageId.Value,
                    TransactionDate = DateTime.UtcNow,
                    Amount = request.Amount,
                    Status = "Pending",
                    TransactionType = "Register",
                    PaymentGateway = "Parsian",
                    Description = $"Package payment initiated for PackageId: {request.PackageId.Value}",
                    OrderId = orderId

                };

                try
                {
                    _dbContext.Factors.Add(factor);
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving factor for Package Payment Request: {@Request}", request);
                    return StatusCode(500, new { Message = "Internal server error while saving payment data." });
                }
            }

            // ارسال درخواست به درگاه
            var gatewayRequest = new PaymentRequestModel
            {
                OrderId = orderId,
                Amount = request.Amount,
                Description = request.Description
            };

            try
            {
                var result = await _paymentService.RequestPaymentAsync(gatewayRequest);
                if (result.Success)
                {
                    // به‌روزرسانی فاکتور در صورت پرداخت پکیج
                    if (isPackagePayment && factor != null)
                    {
                        factor.TrackingNumber = result.Token;
                        _dbContext.Factors.Update(factor);
                        try
                        {
                            await _dbContext.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error updating factor with TrackingNumber for Package Payment Request: {@Request}", request);
                            return StatusCode(500, new { Message = "Internal server error while updating payment data." });
                        }
                    }

                    var gatewayUrl = result.PaymentUrl;
                    return Ok(new
                    {
                        Message = "Request successful",
                        Token = result.Token,
                        GatewayUrl = gatewayUrl
                    });
                }
                else
                {
                    _logger.LogWarning("Payment service returned error for Package Payment Request: {@Request}, Error: {Error}", request, result.Message);
                    return BadRequest(new { Message = result.Message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during Package Payment request processing: {@Request}", request);
                return StatusCode(500, new { Message = "Internal server error during payment request processing." });
            }
        }

        [HttpPost("RenewPackagePayment")]
        public async Task<IActionResult> RenewPackagePayment([FromBody] ExtendedPaymentParisinaRequestDto request)
        {
            // بررسی مدل ورودی
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Package Renewal Request: {@Request}", request);
                return BadRequest(ModelState);
            }

            // مقداردهی خودکار OrderId
            long orderId = DateTime.UtcNow.Ticks;  // استفاده از Ticks برای OrderId به صورت long

            // اعتبارسنجی فیلدهای ضروری مربوط به درگاه
            if (request.Amount <= 0 || !request.UserId.HasValue || !request.PackageId.HasValue)
            {
                _logger.LogWarning("Invalid request parameters for Package Renewal: {@Request}", request);
                return BadRequest(new { Message = "Invalid request parameters." });
            }

            // بررسی اینکه آیا کاربر موجود است
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == request.UserId.Value);
            if (user == null)
            {
                _logger.LogWarning("User not found for Package Renewal: {@Request}", request);
                return BadRequest(new { Message = "User not found." });
            }

            // بررسی وجود پکیج
            var package = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == request.PackageId.Value);
            if (package == null)
            {
                _logger.LogWarning("Package not found for Package Renewal: {@Request}", request);
                return BadRequest(new { Message = "Package not found." });
            }

            // بررسی اینکه آیا کاربر پکیج را دارد
            var userPackage = await _dbContext.UserPackages
                .FirstOrDefaultAsync(up => up.UserId == request.UserId.Value && up.PackageId == request.PackageId.Value);
            if (userPackage == null)
            {
                _logger.LogWarning("User doesn't have this package to renew: {@Request}", request);
                return BadRequest(new { Message = "No active package found to renew." });
            }

            // ایجاد فاکتور برای تمدید پکیج
            var factor = new Factors
            {
                UserId = request.UserId.Value,
                PackageId = request.PackageId.Value,
                UserPackageId = userPackage.Id,
                TransactionDate = DateTime.UtcNow,
                Amount = request.Amount,
                Status = "Pending",
                TransactionType = "Renewal",
                PaymentGateway = "Parsian",  // درگاه پرداخت پارسیان
                Description = $"Package renewal payment for PackageId: {request.PackageId.Value}",
                OrderId = orderId

            };

            try
            {
                _dbContext.Factors.Add(factor);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving factor for Package Renewal: {@Request}", request);
                return StatusCode(500, new { Message = "Internal server error while saving payment data." });
            }

            // آماده‌سازی درخواست پرداخت به درگاه پارسیان
            var gatewayRequest = new PaymentRequestModel
            {
                OrderId = orderId,
                Amount = request.Amount,
                Description = request.Description
            };

            try
            {
                var result = await _paymentService.RequestPaymentAsync(gatewayRequest);
                if (result.Success)
                {
                    // به‌روزرسانی فاکتور در صورت موفقیت در پرداخت
                    factor.TrackingNumber = result.Token;
                    _dbContext.Factors.Update(factor);
                    try
                    {
                        await _dbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error updating factor with TrackingNumber for Package Renewal: {@Request}", request);
                        return StatusCode(500, new { Message = "Internal server error while updating payment data." });
                    }

                    var gatewayUrl = result.PaymentUrl;
                    return Ok(new
                    {
                        Message = "Renewal request successful",
                        RefId = result.Token,
                        GatewayUrl = gatewayUrl
                    });
                }
                else
                {
                    _logger.LogWarning("Payment service returned error for Package Renewal: {@Request}, Error: {Error}",
                        request, result.Message);
                    return BadRequest(new { Message = result.Message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during Package Renewal payment request processing: {@Request}", request);
                return StatusCode(500, new { Message = "Internal server error during payment request processing." });
            }
        }



        [HttpPost("UpgradePackagePayment")]
        public async Task<IActionResult> UpgradePackagePayment([FromBody] UpgradePackagePaymentParsianRequestDto request)
        {
            // بررسی مدل ورودی
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Package Upgrade Request: {@Request}", request);
                return BadRequest(ModelState);
            }

            long orderId = DateTime.UtcNow.Ticks;  // استفاده از Ticks برای OrderId به صورت long

            // بررسی اینکه آیا کاربر موجود است
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
            {
                _logger.LogWarning("User not found for Package Upgrade: {@Request}", request);
                return BadRequest(new { Message = "User not found." });
            }

            // بررسی وجود پکیج‌های فعلی و جدید
            var currentPackage = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == request.PackageId);
            var newPackage = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == request.NewPackageId);
            if (currentPackage == null || newPackage == null || newPackage.Price <= currentPackage.Price)
            {
                _logger.LogWarning("New package not found or not more expensive for Package Upgrade: {@Request}", request);
                return BadRequest(new { Message = "New package not found or not more expensive." });
            }

            // بررسی اینکه آیا پکیج فعلی فعال است
            var currentUserPackage = await _dbContext.UserPackages
                .FirstOrDefaultAsync(up => up.UserId == request.UserId && up.PackageId == request.PackageId && up.IsActive);
            if (currentUserPackage == null)
            {
                _logger.LogWarning("No active package found for upgrade: {@Request}", request);
                return BadRequest(new { Message = "No active package found to upgrade." });
            }

            // محاسبه تفاوت قیمت
            var priceDifference = (long)(newPackage.Price - currentPackage.Price);

            // ایجاد فاکتور برای پرداخت
            var factor = new Factors
            {
                UserId = request.UserId,
                PackageId = request.NewPackageId,
                UserPackageId = currentUserPackage.Id,
                TransactionDate = DateTime.UtcNow,
                Amount = priceDifference,
                Status = "Pending",
                TransactionType = "Upgrade",
                PaymentGateway = "Parsian",
                Description = $"Package upgrade payment from PackageId: {currentPackage.Id} to PackageId: {newPackage.Id}",
                OrderId = orderId
            };

            try
            {
                _dbContext.Factors.Add(factor);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving factor for Package Upgrade: {@Request}", request);
                return StatusCode(500, new { Message = "Internal server error while saving payment data." });
            }

            // ارسال درخواست پرداخت به درگاه پارسیان
            var gatewayRequest = new PaymentRequestModel
            {
                OrderId = orderId,
                Amount = priceDifference,
                Description = $"Upgrade payment for user {request.UserId}, Package from {currentPackage.Id} to {newPackage.Id}"
            };

            try
            {
                var result = await _paymentService.RequestPaymentAsync(gatewayRequest);
                if (result.Success)
                {
                    // به‌روزرسانی فاکتور با شماره رهگیری در صورت پرداخت موفق
                    factor.TrackingNumber = result.Token;
                    _dbContext.Factors.Update(factor);

                    try
                    {
                        await _dbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error updating factor with TrackingNumber for Package Upgrade: {@Request}", request);
                        return StatusCode(500, new { Message = "Internal server error while updating payment data." });
                    }

                    var gatewayUrl = result.PaymentUrl;
                    return Ok(new
                    {
                        Message = "Upgrade request successful",
                        Token = result.Token,
                        GatewayUrl = gatewayUrl
                    });
                }
                else
                {
                    _logger.LogWarning("Payment service returned error for Package Upgrade: {@Request}, Error: {Error}", request, result.Message);
                    return BadRequest(new { Message = result.Message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during Package Upgrade payment request processing: {@Request}", request);
                return StatusCode(500, new { Message = "Internal server error during payment request processing." });
            }
        }



        // اضافه کردن تمدید و ارتقا

        [HttpPost("Callback")]
        public async Task<IActionResult> Callback()
        {
            try
            {
                var status = Convert.ToInt16(Request.Form["status"]);
                var token = Convert.ToInt64(Request.Form["Token"]);
                var rrn = Convert.ToInt64(Request.Form["RRN"]);

                var successRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-success");
                var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");

                var query = HttpUtility.ParseQueryString(string.Empty);
                query["token"] = token.ToString();
                query["RRN"] = rrn.ToString();

                // اگر وضعیت پرداخت موفق است
                if (status == 0 && rrn > 0)
                {
                    var verifyModel = new PaymentVerifyModel { Token = token };
                    var result = await _paymentService.VerifyPaymentAsync(verifyModel);

                    if (result.Success)
                    {
                        // جستجو برای فاکتور با استفاده از شماره پیگیری (TrackingNumber)
                        var factor = await _dbContext.Factors.FirstOrDefaultAsync(f => f.TrackingNumber == token.ToString());
                        if (factor != null)
                        {
                            // تغییر وضعیت فاکتور به "Success" و ثبت زمان پرداخت
                            factor.Status = "Success";
                            factor.PaidAt = DateTime.UtcNow;
                            _dbContext.Factors.Update(factor);
                            await _dbContext.SaveChangesAsync();

                            // اگر پکیج برای فاکتور وجود دارد، اقدامات مختلف را انجام می‌دهیم
                            if (factor.PackageId.HasValue)
                            {
                                var package = await _dbContext.packages
                                    .FirstOrDefaultAsync(p => p.Id == factor.PackageId.Value);

                                if (package != null)
                                {
                                    switch (factor.TransactionType)
                                    {
                                        case "Register":
                                            // ثبت پکیج جدید برای کاربر
                                            await _packageTransactionService.HandleNewPackageRegistration(factor, package);
                                            break;

                                        case "Renewal":
                                            // تمدید پکیج موجود برای کاربر
                                            await _packageTransactionService.HandlePackageRenewal(factor, package);
                                            break;

                                        case "Upgrade":
                                            // ارتقاء پکیج برای کاربر
                                            await _packageTransactionService.HandlePackageUpgrade(factor, package);
                                            break;

                                        default:
                                            _logger.LogWarning($"Unhandled transaction type: {factor.TransactionType}");
                                            break;
                                    }
                                }
                            }
                        }

                        successRedirect.Query = query.ToString();
                        return Redirect(successRedirect.ToString());
                    }
                }

                // در صورتی که وضعیت پرداخت موفق نباشد، ارجاع به صفحه شکست پرداخت
                failureRedirect.Query = query.ToString();
                return Redirect(failureRedirect.ToString());
            }
            catch (Exception ex)
            {
                // در صورت بروز خطا، ارجاع به صفحه شکست پرداخت با پیام خطا
                var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["message"] = Uri.EscapeDataString(ex.Message);
                failureRedirect.Query = query.ToString();
                return Redirect(failureRedirect.ToString());
            }
        }




        // اضافه کردن نسبت دادن پکیج به کاربر

        // [HttpPost("Callback")]
        // public async Task<IActionResult> Callback()
        // {
        //     try
        //     {
        //         var status = Convert.ToInt16(Request.Form["status"]);
        //         var token = Convert.ToInt64(Request.Form["Token"]);
        //         var rrn = Convert.ToInt64(Request.Form["RRN"]);

        //         var successRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-success");
        //         var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");

        //         var query = HttpUtility.ParseQueryString(string.Empty);
        //         query["token"] = token.ToString();  
        //         query["RRN"] = rrn.ToString();  

        //         if (status == 0 && rrn > 0)
        //         {
        //             var verifyModel = new PaymentVerifyModel { Token = token };
        //             var result = await _paymentService.VerifyPaymentAsync(verifyModel);

        //             if (result.Success)
        //             {
        //                 var factor = await _dbContext.Factors.FirstOrDefaultAsync(f => f.TrackingNumber == token.ToString());
        //                 if (factor != null)
        //                 {
        //                     factor.Status = "Success";
        //                     factor.PaidAt = DateTime.UtcNow;
        //                     _dbContext.Factors.Update(factor);
        //                     await _dbContext.SaveChangesAsync();

        //                     // پس از تایید پرداخت، پکیج را به کاربر اختصاص می‌دهیم
        //                     var package = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == factor.PackageId);
        //                     if (package != null)
        //                     {
        //                         // استفاده از سرویس PackageTransactionService برای نسبت دادن پکیج به کاربر
        //                         await _packageTransactionService.HandleNewPackageRegistration(factor, package);
        //                     }
        //                 }

        //                 successRedirect.Query = query.ToString();
        //                 return Redirect(successRedirect.ToString());
        //             }
        //         }

        //         failureRedirect.Query = query.ToString();
        //         return Redirect(failureRedirect.ToString());
        //     }
        //     catch (Exception ex)
        //     {
        //         var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");
        //         var query = HttpUtility.ParseQueryString(string.Empty);
        //         query["message"] = Uri.EscapeDataString(ex.Message);
        //         failureRedirect.Query = query.ToString();
        //         return Redirect(failureRedirect.ToString());
        //     }
        // }


        // تبدیل فاکتور به Success

        // [HttpPost("Callback")]
        // public async Task<IActionResult> Callback()
        // {
        //     try
        //     {
        //         // دریافت پارامترهای دیگر از فرم
        //         var status = Convert.ToInt16(Request.Form["status"]);
        //         var token = Convert.ToInt64(Request.Form["Token"]);
        //         var orderId = Convert.ToInt64(Request.Form["OrderId"]);
        //         var amount = Request.Form["Amount"];
        //         var terminalNo = Convert.ToInt32(Request.Form["TerminalNo"]);
        //         var rrn = Convert.ToInt64(Request.Form["RRN"]);

        //         // آدرس‌های ریدایرکت
        //         var successRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-success");
        //         var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");

        //         var query = HttpUtility.ParseQueryString(string.Empty);
        //         query["token"] = token.ToString();  // اضافه کردن توکن برای نمایش در صفحه
        //         query["RRN"] = rrn.ToString();  

        //         // در صورت موفق بودن پرداخت
        //         if (status == 0 && rrn > 0)
        //         {
        //             var verifyModel = new PaymentVerifyModel { Token = token };
        //             var result = await _paymentService.VerifyPaymentAsync(verifyModel);

        //             if (result.Success)
        //             {
        //                 // تغییر وضعیت فاکتور به Success پس از تایید پرداخت
        //                 var factor = await _dbContext.Factors.FirstOrDefaultAsync(f => f.TrackingNumber == token.ToString());
        //                 if (factor != null)
        //                 {
        //                     factor.Status = "Success";
        //                     factor.PaidAt = DateTime.UtcNow;
        //                     _dbContext.Factors.Update(factor);
        //                     await _dbContext.SaveChangesAsync();
        //                 }

        //                 successRedirect.Query = query.ToString();
        //                 return Redirect(successRedirect.ToString()); // هدایت به صفحه موفقیت
        //             }
        //         }

        //         // در صورتی که پرداخت ناموفق باشد یا وریفای شکست بخورد
        //         failureRedirect.Query = query.ToString();
        //         return Redirect(failureRedirect.ToString());
        //     }
        //     catch (Exception ex)
        //     {
        //         // در صورت بروز خطا، به صفحه ناموفق هدایت می‌کنیم و پیام خطا را هم اضافه می‌کنیم
        //         var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");
        //         var query = HttpUtility.ParseQueryString(string.Empty);
        //         query["message"] = Uri.EscapeDataString(ex.Message);
        //         failureRedirect.Query = query.ToString();
        //         return Redirect(failureRedirect.ToString());
        //     }
        // }



        // ری دایرکت کاربر به صفحه موفق یا ناموفق

        // [HttpPost("Callback")]
        // public async Task<IActionResult> Callback()
        // {
        //     try
        //     {
        //         // دریافت پارامترهای دیگر از فرم
        //         var status = Convert.ToInt16(Request.Form["status"]);
        //         var token = Convert.ToInt64(Request.Form["Token"]);
        //         var orderId = Convert.ToInt64(Request.Form["OrderId"]);
        //         var amount = Request.Form["Amount"];
        //         var terminalNo = Convert.ToInt32(Request.Form["TerminalNo"]);
        //         var rrn = Convert.ToInt64(Request.Form["RRN"]);

        //         // آدرس‌های ریدایرکت
        //         var successRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-success");
        //         var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");

        //         var query = HttpUtility.ParseQueryString(string.Empty);
        //         query["token"] = token.ToString();  // اضافه کردن توکن برای نمایش در صفحه

        //         // در صورت موفق بودن پرداخت
        //         if (status == 0 && rrn > 0)
        //         {
        //             var verifyModel = new PaymentVerifyModel { Token = token };
        //             var result = await _paymentService.VerifyPaymentAsync(verifyModel);

        //             if (result.Success)
        //             {
        //                 successRedirect.Query = query.ToString();
        //                 return Redirect(successRedirect.ToString()); // هدایت به صفحه موفقیت
        //             }
        //         }

        //         // در صورتی که پرداخت ناموفق باشد یا وریفای شکست بخورد
        //         failureRedirect.Query = query.ToString();
        //         return Redirect(failureRedirect.ToString());
        //     }
        //     catch (Exception ex)
        //     {
        //         // در صورت بروز خطا، به صفحه ناموفق هدایت می‌کنیم و پیام خطا را هم اضافه می‌کنیم
        //         var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");
        //         var query = HttpUtility.ParseQueryString(string.Empty);
        //         query["message"] = Uri.EscapeDataString(ex.Message);
        //         failureRedirect.Query = query.ToString();
        //         return Redirect(failureRedirect.ToString());
        //     }
        // }

    }
}