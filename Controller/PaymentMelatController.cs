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
using deathSite.Services.PackageService;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMelatController : ControllerBase
    {
        private readonly IMelatPaymentService _paymentService;
        private readonly IPackageTransactionService _packageTransactionService;

        private readonly ILogger<PaymentMelatController> _logger;
        private readonly apiContext _dbContext;

        public PaymentMelatController(
            IMelatPaymentService paymentService,
            ILogger<PaymentMelatController> logger,
            apiContext dbContext,
            IPackageTransactionService packageTransactionService)
        {
            _paymentService = paymentService;
            _logger = logger;
            _dbContext = dbContext;
            _packageTransactionService = packageTransactionService;
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

            string payerId = DateTime.UtcNow.Ticks.ToString();  // تبدیل Ticks به string
            long orderId = DateTime.UtcNow.Ticks;  // استفاده از Ticks برای OrderId به صورت long

            // اعتبارسنجی فیلدهای ضروری مربوط به درگاه
            if (request.Amount <= 0)
            {
                _logger.LogWarning("Invalid Amount in Payment Request: {@Request}", request);
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
                    TransactionType = "Register",
                    PaymentGateway = "Melat",
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
                    _logger.LogError(ex, "Error saving factor for Payment Request: {@Request}", request);
                    return StatusCode(500, new { Message = "Internal server error while saving payment data." });
                }
            }

            // نگاشت داده‌های ورودی خارجی به DTO داخلی (تنها فیلدهای لازم برای درگاه)
            var gatewayRequest = new PaymentRequestDto
            {
                OrderId = orderId,  // به صورت long
                Amount = request.Amount,
                PayerId = payerId  // تبدیل Ticks به string برای PayerId
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

            string payerId = DateTime.UtcNow.Ticks.ToString();  // تبدیل Ticks به string
            long orderId = DateTime.UtcNow.Ticks;  // استفاده از Ticks برای OrderId به صورت long

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

            // Prepare payment request
            var gatewayRequest = new PaymentRequestDto
            {
                OrderId = orderId,  // به صورت long
                Amount = request.Amount,
                PayerId = payerId  // تبدیل Ticks به string برای PayerId
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
        public async Task<IActionResult> UpgradePackage([FromBody] UpgradePackageRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Package Upgrade Request: {@Request}", request);
                return BadRequest(ModelState);
            }

            string payerId = DateTime.UtcNow.Ticks.ToString();  // تبدیل Ticks به string
            long orderId = DateTime.UtcNow.Ticks;  // استفاده از Ticks برای OrderId به صورت long

            // Validate essential fields
            if (request.UserId <= 0 || request.PackageId <= 0 || request.NewPackageId <= 0 || request.OrderId <= 0 || string.IsNullOrEmpty(request.PayerId))
            {
                _logger.LogWarning("Invalid request parameters for Package Upgrade: {@Request}", request);
                return BadRequest(new { Message = "Invalid request parameters." });
            }

            // Check if user exists
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
            {
                _logger.LogWarning("User not found for Package Upgrade: {@Request}", request);
                return BadRequest(new { Message = "User not found." });
            }

            // Check if new package exists and is more expensive than the current package
            var currentPackage = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == request.PackageId);
            var newPackage = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == request.NewPackageId);
            if (newPackage == null || newPackage.Price <= currentPackage.Price)
            {
                _logger.LogWarning("New package not found or not more expensive for Package Upgrade: {@Request}", request);
                return BadRequest(new { Message = "New package not found or not more expensive." });
            }

            // Find current active package
            var currentUserPackage = await _dbContext.UserPackages
                .FirstOrDefaultAsync(up => up.UserId == request.UserId && up.IsActive);
            if (currentUserPackage == null)
            {
                _logger.LogWarning("No active package found for upgrade: {@Request}", request);
                return BadRequest(new { Message = "No active package found to upgrade." });
            }

            // Calculate the price difference
            var priceDifference = (long)(newPackage.Price - currentPackage.Price);

            // Create pending factor
            var factor = new Factors
            {
                UserId = request.UserId,
                PackageId = request.NewPackageId,
                UserPackageId = currentUserPackage.Id,
                TransactionDate = DateTime.UtcNow,
                Amount = priceDifference,
                Status = "Pending",
                TransactionType = "Upgrade",
                PaymentGateway = "Melat",
                Description = $"Package upgrade payment from PackageId: {currentUserPackage.PackageId} to PackageId: {request.NewPackageId}",
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

            // Prepare payment request
            var gatewayRequest = new PaymentRequestDto
            {
                OrderId = orderId,
                Amount = priceDifference,
                PayerId = payerId
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

        //                         if (factor.PackageId.HasValue)
        //                         {
        //                             var package = await _dbContext.packages
        //                                 .FirstOrDefaultAsync(p => p.Id == factor.PackageId.Value);

        //                             if (package != null)
        //                             {
        //                                 switch (factor.TransactionType)
        //                                 {
        //                                     case "Register":
        //                                         // Existing logic for new package registration
        //                                         await HandleNewPackageRegistration(factor, package);
        //                                         break;

        //                                     case "Renewal":
        //                                         // Handle package renewal
        //                                         await HandlePackageRenewal(factor, package);
        //                                         break;

        //                                     case "Upgrade":
        //                                         // Handle package upgrade
        //                                         await HandlePackageUpgrade(factor, package);
        //                                         break;
        //                                 }
        //                             }
        //                         }
        //                     }
        //                     catch (Exception ex)
        //                     {
        //                         _logger.LogError(ex, "Error processing package transaction for TrackingNumber: {RefId}", refId);
        //                         return StatusCode(500, new { Message = "Internal server error while processing package transaction." });
        //                     }

        //                     successRedirect.Query = query.ToString();
        //                     return Redirect(successRedirect.ToString());
        //                 }
        //             }
        //         }

        //         // Handle failed transaction
        //         var failedFactor = await _dbContext.Factors.FirstOrDefaultAsync(f => f.TrackingNumber == refId);
        //         if (failedFactor != null)
        //         {
        //             failedFactor.Status = "Failed";
        //             failedFactor.Description = "Transaction failed with ResCode: " + resCode;
        //             _dbContext.Factors.Update(failedFactor);
        //             await _dbContext.SaveChangesAsync();
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
        //         failureRedirect.Query = query.ToString();
        //         return Redirect(failureRedirect.ToString());
        //     }
        // }

        // استفاده از سرویس های تمید و ارتقا و خرید در کالبک

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
                                                // استفاده از سرویس برای ثبت پکیج جدید
                                                await _packageTransactionService.HandleNewPackageRegistration(factor, package);
                                                break;

                                            case "Renewal":
                                                // استفاده از سرویس برای تمدید پکیج
                                                await _packageTransactionService.HandlePackageRenewal(factor, package);
                                                break;

                                            case "Upgrade":
                                                // استفاده از سرویس برای ارتقاء پکیج
                                                await _packageTransactionService.HandlePackageUpgrade(factor, package);
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
                // Check if package is about to expire (1 month left)
                var daysToExpiry = (userPackage.ExpiryDate - DateTime.UtcNow).Days;
                if (daysToExpiry <= 30)
                {
                    // Extend the expiry date
                    if (userPackage.ExpiryDate < DateTime.UtcNow)
                    {
                        // Package has already expired, extend from the expiry date
                        userPackage.ExpiryDate = DateTime.UtcNow.AddDays(int.Parse(package.ValidityPeriod));
                    }
                    else
                    {
                        // Package has not expired yet, extend for ValidityPeriod days
                        userPackage.ExpiryDate = userPackage.ExpiryDate.AddDays(int.Parse(package.ValidityPeriod));
                    }
                }
                else
                {
                    // Package has not expired yet, extend for ValidityPeriod days
                    userPackage.ExpiryDate = userPackage.ExpiryDate.AddDays(int.Parse(package.ValidityPeriod));
                }

                userPackage.IsActive = true;

                _dbContext.UserPackages.Update(userPackage);
                await _dbContext.SaveChangesAsync();
            }
        }


        private async Task HandlePackageUpgrade(Factors factor, Package newPackage)
        {
            // Find the current active package for the user
            var currentUserPackage = await _dbContext.UserPackages
                .FirstOrDefaultAsync(up => up.Id == factor.UserPackageId);

            if (currentUserPackage != null)
            {
                // Deactivate the current package
                currentUserPackage.IsActive = false;
                _dbContext.UserPackages.Update(currentUserPackage);

                // Create a new package with the upgraded features
                var newUserPackage = new UserPackage
                {
                    UserId = factor.UserId,
                    PackageId = newPackage.Id,
                    // Preserve the original PurchaseDate and ExpiryDate
                    PurchaseDate = currentUserPackage.PurchaseDate,
                    ExpiryDate = currentUserPackage.ExpiryDate,
                    IsActive = true,
                    // Copy the usage counts from the previous package
                    UsedImageCount = currentUserPackage.UsedImageCount,
                    UsedVideoCount = currentUserPackage.UsedVideoCount,
                    UsedNotificationCount = currentUserPackage.UsedNotificationCount,
                    UsedAudioFileCount = currentUserPackage.UsedAudioFileCount
                };

                // Add the new package and save the changes
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
