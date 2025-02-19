// // اضافه کردن کالبک 

// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using api.Services;
// using api.View.PaymentMelat;
// using deathSite.Services.Payment;
// using deathSite.View.PaymentMelat;
// using System.Web;

// namespace api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class PaymentMelatController : ControllerBase
//     {
//         private readonly IMelatPaymentService _paymentService;
//         private readonly ILogger<PaymentMelatController> _logger;

//         public PaymentMelatController(IMelatPaymentService paymentService, ILogger<PaymentMelatController> logger)
//         {
//             _paymentService = paymentService;
//             _logger = logger;
//         }

//         [HttpPost("PayRequest")]
//         public async Task<IActionResult> PayRequest([FromBody] PaymentRequestDto request)
//         {
//             var result = await _paymentService.PayRequestAsync(request);
//             if (result.Success)
//             {
//                 var gatewayUrl = $"https://bpm.shaparak.ir/pgwchannel/startpay.mellat?RefId={result.RefId}";
//                 return Ok(new
//                 {
//                     Message = "Request successful",
//                     RefId = result.RefId,
//                     GatewayUrl = gatewayUrl
//                 });
//             }
//             return BadRequest(new { Message = result.Message });
//         }

//         [HttpPost("VerifyRequest")]
//         public async Task<IActionResult> VerifyRequest([FromBody] VerifyRequestDto request)
//         {
//             var result = await _paymentService.VerifyRequestAsync(request);
//             if (result.Success)
//             {
//                 return Ok(new { Message = "Transaction verified successfully" });
//             }
//             return BadRequest(new { Message = result.Message });
//         }

//         [HttpPost("SettleRequest")]
//         public async Task<IActionResult> SettleRequest([FromBody] SettleRequestDto request)
//         {
//             var result = await _paymentService.SettleRequestAsync(request);
//             if (result.Success)
//             {
//                 return Ok(new { Message = "Settlement successful" });
//             }
//             return BadRequest(new { Message = result.Message });
//         }

//         [HttpPost("Callback")]
//         public async Task<IActionResult> Callback()
//         {
//             try
//             {
//                 // استخراج پارامترها از فرم
//                 var resCode = Request.Form["ResCode"].ToString();
//                 var refId = Request.Form["RefId"].ToString();
//                 var orderId = Convert.ToInt64(Request.Form["OrderId"]);
//                 var saleOrderId = Convert.ToInt64(Request.Form["SaleOrderId"]);
//                 var saleReferenceId = Convert.ToInt64(Request.Form["SaleReferenceId"]);

//                 // آدرس‌های ریدایرکت
//                 var successRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-success");
//                 var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");

//                 var query = HttpUtility.ParseQueryString(string.Empty);
//                 query["refId"] = refId;
//                 query["orderId"] = orderId.ToString();

//                 // بررسی موفقیت تراکنش بر اساس ResCode
//                 if (resCode == "0")
//                 {
//                     // فراخوانی متد Verify برای تایید نهایی تراکنش
//                     var verifyResult = await _paymentService.VerifyRequestAsync(new VerifyRequestDto
//                     {
//                         OrderId = orderId,
//                         SaleOrderId = saleOrderId,
//                         SaleReferenceId = saleReferenceId
//                     });

//                     if (verifyResult.Success)
//                     {
//                         successRedirect.Query = query.ToString();
//                         return Redirect(successRedirect.ToString()); // هدایت به صفحه موفقیت
//                     }
//                 }

//                 // در صورتی که تراکنش ناموفق باشد، کاربر به صفحه ناموفق هدایت می‌شود
//                 failureRedirect.Query = query.ToString();
//                 return Redirect(failureRedirect.ToString());
//             }
//             catch (Exception ex)
//             {
//                 // در صورت بروز خطا، کاربر را به صفحه ناموفق هدایت کرده و پیام خطا را نمایش می‌دهیم
//                 var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");
//                 var query = HttpUtility.ParseQueryString(string.Empty);
//                 query["message"] = Uri.EscapeDataString(ex.Message);
//                 failureRedirect.Query = query.ToString();
//                 return Redirect(failureRedirect.ToString());
//             }
//         }


//     }
// }

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Web;
using Microsoft.EntityFrameworkCore;
using deathSite.Model;
using deathSite.Services.Payment;
using api.Context;
using deathSite.View.PaymentMelat;
using api.View.PaymentMelat;
using deathSite.View.Packages;
using api.Model;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMelatController : ControllerBase
    {
        private readonly IMelatPaymentService _paymentService;
        private readonly ILogger<PaymentMelatController> _logger;
        private readonly apiContext _dbContext;

        public PaymentMelatController(
            IMelatPaymentService paymentService,
            ILogger<PaymentMelatController> logger,
            apiContext dbContext)
        {
            _paymentService = paymentService;
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// درخواست پرداخت - دریافت اطلاعات ورودی، بررسی صحت، ذخیره فاکتور و هدایت به درگاه.
        /// </summary>
        [HttpPost("PayRequest")]
        public async Task<IActionResult> PayRequest([FromBody] ExtendedPaymentRequestDto request)
        {
            // بررسی مدل ورودی
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Payment Request: {@Request}", request);
                return BadRequest(ModelState);
            }

            // اعتبارسنجی فیلدهای ضروری مربوط به درگاه
            if (request.Amount <= 0)
            {
                _logger.LogWarning("Invalid Amount in Payment Request: {@Request}", request);
                return BadRequest(new { Message = "Amount must be greater than zero." });
            }

            if (string.IsNullOrWhiteSpace(request.PayerId))
            {
                _logger.LogWarning("PayerId is required in Payment Request: {@Request}", request);
                return BadRequest(new { Message = "PayerId is required." });
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
                    _logger.LogWarning("User not found for Payment Request: {@Request}", request);
                    return BadRequest(new { Message = "User not found." });
                }

                // بررسی وجود پکیج
                var package = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == request.PackageId.Value);
                if (package == null)
                {
                    _logger.LogWarning("Package not found for Payment Request: {@Request}", request);
                    return BadRequest(new { Message = "Package not found." });
                }

                // ایجاد رکورد فاکتور با وضعیت Pending
                factor = new Factors
                {
                    UserId = request.UserId.Value,
                    PackageId = request.PackageId.Value, // ذخیره شناسه پکیج
                    TransactionDate = DateTime.UtcNow,
                    Amount = request.Amount,
                    Status = "Pending",
                    //TransactionType = string.IsNullOrEmpty(request.ActionType) ? "Register" : request.ActionType,

                    TransactionType = "Register",
                    PaymentGateway = "Melat",
                    Description = $"Package payment initiated for PackageId: {request.PackageId.Value}"
                };

                try
                {
                    _dbContext.Factors.Add(factor);
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving factor for Payment Request: {@Request}", request);
                    return StatusCode(500, new { Message = "Internal server error while saving payment data." });
                }
            }

            // نگاشت داده‌های ورودی خارجی به DTO داخلی (تنها فیلدهای لازم برای درگاه)
            var gatewayRequest = new PaymentRequestDto
            {
                OrderId = request.OrderId,
                Amount = request.Amount,
                PayerId = request.PayerId
            };

            try
            {
                var result = await _paymentService.PayRequestAsync(gatewayRequest);
                if (result.Success)
                {
                    // به‌روزرسانی فاکتور در صورت پرداخت پکیج با ثبت TrackingNumber دریافتی از درگاه
                    if (isPackagePayment && factor != null)
                    {
                        factor.TrackingNumber = result.RefId;
                        _dbContext.Factors.Update(factor);
                        try
                        {
                            await _dbContext.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error updating factor with TrackingNumber for Payment Request: {@Request}", request);
                            return StatusCode(500, new { Message = "Internal server error while updating payment data." });
                        }
                    }

                    var gatewayUrl = $"https://bpm.shaparak.ir/pgwchannel/startpay.mellat?RefId={result.RefId}";
                    return Ok(new
                    {
                        Message = "Request successful",
                        RefId = result.RefId,
                        GatewayUrl = gatewayUrl
                    });
                }
                else
                {
                    _logger.LogWarning("Payment service returned error for Payment Request: {@Request}, Error: {Error}", request, result.Message);
                    return BadRequest(new { Message = result.Message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during payment request processing: {@Request}", request);
                return StatusCode(500, new { Message = "Internal server error during payment request processing." });
            }
        }

        [HttpPost("RenewPackage")]
        public async Task<IActionResult> RenewPackage([FromBody] ExtendedPaymentRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Package Renewal Request: {@Request}", request);
                return BadRequest(ModelState);
            }

            // Validate essential fields
            if (request.Amount <= 0 || !request.UserId.HasValue || !request.PackageId.HasValue)
            {
                _logger.LogWarning("Invalid request parameters for Package Renewal: {@Request}", request);
                return BadRequest(new { Message = "Invalid request parameters." });
            }

            // Check if user exists
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == request.UserId.Value);
            if (user == null)
            {
                _logger.LogWarning("User not found for Package Renewal: {@Request}", request);
                return BadRequest(new { Message = "User not found." });
            }

            // Check if package exists
            var package = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == request.PackageId.Value);
            if (package == null)
            {
                _logger.LogWarning("Package not found for Package Renewal: {@Request}", request);
                return BadRequest(new { Message = "Package not found." });
            }

            // Check if user has this package
            var userPackage = await _dbContext.UserPackages
                .FirstOrDefaultAsync(up => up.UserId == request.UserId.Value &&
                                         up.PackageId == request.PackageId.Value);
            if (userPackage == null)
            {
                _logger.LogWarning("User doesn't have this package to renew: {@Request}", request);
                return BadRequest(new { Message = "No active package found to renew." });
            }

            // Create pending factor
            var factor = new Factors
            {
                UserId = request.UserId.Value,
                PackageId = request.PackageId.Value,
                UserPackageId = userPackage.Id,
                TransactionDate = DateTime.UtcNow,
                Amount = request.Amount,
                Status = "Pending",
                TransactionType = "Renewal",
                PaymentGateway = "Melat",
                Description = $"Package renewal payment for PackageId: {request.PackageId.Value}"
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

            // Prepare payment request
            var gatewayRequest = new PaymentRequestDto
            {
                OrderId = request.OrderId,
                Amount = request.Amount,
                PayerId = request.PayerId
            };

            try
            {
                var result = await _paymentService.PayRequestAsync(gatewayRequest);
                if (result.Success)
                {
                    factor.TrackingNumber = result.RefId;
                    _dbContext.Factors.Update(factor);
                    await _dbContext.SaveChangesAsync();

                    var gatewayUrl = $"https://bpm.shaparak.ir/pgwchannel/startpay.mellat?RefId={result.RefId}";
                    return Ok(new
                    {
                        Message = "Renewal request successful",
                        RefId = result.RefId,
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
                _logger.LogError(ex, "Error occurred during renewal payment processing: {@Request}", request);
                return StatusCode(500, new { Message = "Internal server error during payment processing." });
            }
        }

        [HttpPost("UpgradePackage")]
        public async Task<IActionResult> UpgradePackage([FromBody] ExtendedPaymentRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Package Upgrade Request: {@Request}", request);
                return BadRequest(ModelState);
            }

            // Validate essential fields
            if (request.Amount <= 0 || !request.UserId.HasValue || !request.PackageId.HasValue)
            {
                _logger.LogWarning("Invalid request parameters for Package Upgrade: {@Request}", request);
                return BadRequest(new { Message = "Invalid request parameters." });
            }

            // Check if user exists
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == request.UserId.Value);
            if (user == null)
            {
                _logger.LogWarning("User not found for Package Upgrade: {@Request}", request);
                return BadRequest(new { Message = "User not found." });
            }

            // Check if new package exists
            var newPackage = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == request.PackageId.Value);
            if (newPackage == null)
            {
                _logger.LogWarning("New package not found for Package Upgrade: {@Request}", request);
                return BadRequest(new { Message = "New package not found." });
            }

            // Find current active package
            var currentUserPackage = await _dbContext.UserPackages
                .FirstOrDefaultAsync(up => up.UserId == request.UserId.Value && up.IsActive);
            if (currentUserPackage == null)
            {
                _logger.LogWarning("No active package found for upgrade: {@Request}", request);
                return BadRequest(new { Message = "No active package found to upgrade." });
            }

            // Create pending factor
            var factor = new Factors
            {
                UserId = request.UserId.Value,
                PackageId = request.PackageId.Value,
                UserPackageId = currentUserPackage.Id,
                TransactionDate = DateTime.UtcNow,
                Amount = request.Amount,
                Status = "Pending",
                TransactionType = "Upgrade",
                PaymentGateway = "Melat",
                Description = $"Package upgrade payment from PackageId: {currentUserPackage.PackageId} to PackageId: {request.PackageId.Value}"
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

            // Prepare payment request
            var gatewayRequest = new PaymentRequestDto
            {
                OrderId = request.OrderId,
                Amount = request.Amount,
                PayerId = request.PayerId
            };

            try
            {
                var result = await _paymentService.PayRequestAsync(gatewayRequest);
                if (result.Success)
                {
                    factor.TrackingNumber = result.RefId;
                    _dbContext.Factors.Update(factor);
                    await _dbContext.SaveChangesAsync();

                    var gatewayUrl = $"https://bpm.shaparak.ir/pgwchannel/startpay.mellat?RefId={result.RefId}";
                    return Ok(new
                    {
                        Message = "Upgrade request successful",
                        RefId = result.RefId,
                        GatewayUrl = gatewayUrl
                    });
                }
                else
                {
                    _logger.LogWarning("Payment service returned error for Package Upgrade: {@Request}, Error: {Error}",
                        request, result.Message);
                    return BadRequest(new { Message = result.Message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during upgrade payment processing: {@Request}", request);
                return StatusCode(500, new { Message = "Internal server error during payment processing." });
            }
        }

        // اضافه کردن اپدیت و تمدید

        // Update the Callback method to handle renewals and upgrades
        [HttpPost("Callback")]
        public async Task<IActionResult> Callback()
        {
            try
            {
                var resCode = Request.Form["ResCode"].ToString();
                var refId = Request.Form["RefId"].ToString();
                var orderId = Convert.ToInt64(Request.Form["OrderId"]);
                var saleOrderId = Convert.ToInt64(Request.Form["SaleOrderId"]);
                var saleReferenceId = Convert.ToInt64(Request.Form["SaleReferenceId"]);

                var successRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-success");
                var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");

                var query = HttpUtility.ParseQueryString(string.Empty);
                query["refId"] = refId;
                query["orderId"] = orderId.ToString();

                if (resCode == "0")
                {
                    var verifyResult = await _paymentService.VerifyRequestAsync(new VerifyRequestDto
                    {
                        OrderId = orderId,
                        SaleOrderId = saleOrderId,
                        SaleReferenceId = saleReferenceId
                    });

                    if (verifyResult.Success)
                    {
                        var factor = await _dbContext.Factors
                            .FirstOrDefaultAsync(f => f.TrackingNumber == refId);

                        if (factor != null)
                        {
                            factor.Status = "Success";
                            factor.PaidAt = DateTime.UtcNow;
                            factor.OrderId = orderId;
                            _dbContext.Factors.Update(factor);

                            try
                            {
                                await _dbContext.SaveChangesAsync();

                                if (factor.PackageId.HasValue)
                                {
                                    var package = await _dbContext.packages
                                        .FirstOrDefaultAsync(p => p.Id == factor.PackageId.Value);

                                    if (package != null)
                                    {
                                        switch (factor.TransactionType)
                                        {
                                            case "Register":
                                                // Existing logic for new package registration
                                                await HandleNewPackageRegistration(factor, package);
                                                break;

                                            case "Renewal":
                                                // Handle package renewal
                                                await HandlePackageRenewal(factor, package);
                                                break;

                                            case "Upgrade":
                                                // Handle package upgrade
                                                await HandlePackageUpgrade(factor, package);
                                                break;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error processing package transaction for TrackingNumber: {RefId}", refId);
                                return StatusCode(500, new { Message = "Internal server error while processing package transaction." });
                            }

                            successRedirect.Query = query.ToString();
                            return Redirect(successRedirect.ToString());
                        }
                    }
                }

                // Handle failed transaction
                var failedFactor = await _dbContext.Factors.FirstOrDefaultAsync(f => f.TrackingNumber == refId);
                if (failedFactor != null)
                {
                    failedFactor.Status = "Failed";
                    failedFactor.Description = "Transaction failed with ResCode: " + resCode;
                    _dbContext.Factors.Update(failedFactor);
                    await _dbContext.SaveChangesAsync();
                }

                failureRedirect.Query = query.ToString();
                return Redirect(failureRedirect.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during Callback processing");
                var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["message"] = Uri.EscapeDataString(ex.Message);
                failureRedirect.Query = query.ToString();
                return Redirect(failureRedirect.ToString());
            }
        }

        private async Task HandleNewPackageRegistration(Factors factor, Package package)
        {
            var userPackage = await _dbContext.UserPackages
                .FirstOrDefaultAsync(up => up.UserId == factor.UserId && up.PackageId == factor.PackageId.Value);

            if (userPackage == null)
            {
                var newUserPackage = new UserPackage
                {
                    UserId = factor.UserId,
                    PackageId = package.Id,
                    PurchaseDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddDays(int.Parse(package.ValidityPeriod)),
                    IsActive = true,
                    UsedImageCount = 0,
                    UsedVideoCount = 0,
                    UsedNotificationCount = 0,
                    UsedAudioFileCount = 0
                };

                _dbContext.UserPackages.Add(newUserPackage);
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task HandlePackageRenewal(Factors factor, Package package)
        {
            var userPackage = await _dbContext.UserPackages
                .FirstOrDefaultAsync(up => up.Id == factor.UserPackageId);

            if (userPackage != null)
            {
                // Extend the expiry date
                userPackage.ExpiryDate = userPackage.ExpiryDate > DateTime.UtcNow
                    ? userPackage.ExpiryDate.AddDays(int.Parse(package.ValidityPeriod))
                    : DateTime.UtcNow.AddDays(int.Parse(package.ValidityPeriod));

                userPackage.IsActive = true;

                _dbContext.UserPackages.Update(userPackage);
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task HandlePackageUpgrade(Factors factor, Package package)
        {
            var currentUserPackage = await _dbContext.UserPackages
                .FirstOrDefaultAsync(up => up.Id == factor.UserPackageId);

            if (currentUserPackage != null)
            {
                // Deactivate current package
                currentUserPackage.IsActive = false;
                _dbContext.UserPackages.Update(currentUserPackage);

                // Create new package with upgraded features
                var newUserPackage = new UserPackage
                {
                    UserId = factor.UserId,
                    PackageId = package.Id,
                    PurchaseDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddDays(int.Parse(package.ValidityPeriod)),
                    IsActive = true,
                    UsedImageCount = currentUserPackage.UsedImageCount,
                    UsedVideoCount = currentUserPackage.UsedVideoCount,
                    UsedNotificationCount = currentUserPackage.UsedNotificationCount,
                    UsedAudioFileCount = currentUserPackage.UsedAudioFileCount
                };

                _dbContext.UserPackages.Add(newUserPackage);
                await _dbContext.SaveChangesAsync();
            }
        }

        // [HttpPost("Callback")]
        // public async Task<IActionResult> Callback()
        // {
        //     try
        //     {
        //         var resCode = Request.Form["ResCode"].ToString();
        //         var refId = Request.Form["RefId"].ToString();
        //         var orderId = Convert.ToInt64(Request.Form["OrderId"]);
        //         var saleOrderId = Convert.ToInt64(Request.Form["SaleOrderId"]);
        //         var saleReferenceId = Convert.ToInt64(Request.Form["SaleReferenceId"]);

        //         var successRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-success");
        //         var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");

        //         var query = HttpUtility.ParseQueryString(string.Empty);
        //         query["refId"] = refId;
        //         query["orderId"] = orderId.ToString();

        //         if (resCode == "0")
        //         {
        //             var verifyResult = await _paymentService.VerifyRequestAsync(new VerifyRequestDto
        //             {
        //                 OrderId = orderId,
        //                 SaleOrderId = saleOrderId,
        //                 SaleReferenceId = saleReferenceId
        //             });

        //             if (verifyResult.Success)
        //             {
        //                 var factor = await _dbContext.Factors
        //                     .FirstOrDefaultAsync(f => f.TrackingNumber == refId);

        //                 if (factor != null)
        //                 {
        //                     factor.Status = "Success";
        //                     factor.PaidAt = DateTime.UtcNow;
        //                     factor.OrderId = orderId;
        //                     _dbContext.Factors.Update(factor);

        //                     try
        //                     {
        //                         await _dbContext.SaveChangesAsync();

        //                         // ثبت پکیج برای کاربر در صورت عدم وجود پکیج فعال قبلی
        //                         if (factor.PackageId.HasValue)
        //                         {
        //                             var userPackage = await _dbContext.UserPackages
        //                                 .FirstOrDefaultAsync(up => up.UserId == factor.UserId && up.PackageId == factor.PackageId.Value);

        //                             var package = await _dbContext.packages
        //                                 .FirstOrDefaultAsync(p => p.Id == factor.PackageId.Value);

        //                             if (package != null && userPackage == null)
        //                             {
        //                                 // ثبت پکیج جدید برای کاربر
        //                                 var newUserPackage = new UserPackage
        //                                 {
        //                                     UserId = factor.UserId,
        //                                     PackageId = package.Id,
        //                                     PurchaseDate = DateTime.UtcNow,
        //                                     ExpiryDate = DateTime.UtcNow.AddDays(int.Parse(package.ValidityPeriod)),
        //                                     IsActive = true,
        //                                     UsedImageCount = 0,
        //                                     UsedVideoCount = 0,
        //                                     UsedNotificationCount = 0,
        //                                     UsedAudioFileCount = 0
        //                                 };

        //                                 _dbContext.UserPackages.Add(newUserPackage);
        //                                 await _dbContext.SaveChangesAsync();
        //                                 _logger.LogInformation("New package registered for User {UserId} with Package {PackageId}", factor.UserId, package.Id);
        //                             }
        //                         }
        //                     }
        //                     catch (Exception ex)
        //                     {
        //                         _logger.LogError(ex, "Error updating factor and registering package during Callback for TrackingNumber: {RefId}", refId);
        //                         return StatusCode(500, new { Message = "Internal server error while updating payment data and registering package." });
        //                     }

        //                     successRedirect.Query = query.ToString();
        //                     return Redirect(successRedirect.ToString());
        //                 }
        //             }
        //         }

        //         // در صورت عدم موفقیت تراکنش
        //         var failedFactor = await _dbContext.Factors.FirstOrDefaultAsync(f => f.TrackingNumber == refId);
        //         if (failedFactor != null)
        //         {
        //             failedFactor.Status = "Failed";
        //             failedFactor.Description = "Transaction failed with ResCode: " + resCode;
        //             _dbContext.Factors.Update(failedFactor);
        //             try
        //             {
        //                 await _dbContext.SaveChangesAsync();
        //             }
        //             catch (Exception ex)
        //             {
        //                 _logger.LogError(ex, "Error updating failed factor during Callback for TrackingNumber: {RefId}", refId);
        //             }
        //         }

        //         failureRedirect.Query = query.ToString();
        //         return Redirect(failureRedirect.ToString());
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error occurred during Callback processing");
        //         var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");
        //         var query = HttpUtility.ParseQueryString(string.Empty);
        //         query["message"] = Uri.EscapeDataString(ex.Message);
        //         query["stackTrace"] = Uri.EscapeDataString(ex.StackTrace);  // اضافه کردن استک ترِیس
        //         failureRedirect.Query = query.ToString();
        //         return Redirect(failureRedirect.ToString());
        //     }
        // }


    }
}
