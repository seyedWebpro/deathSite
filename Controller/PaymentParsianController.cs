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

        [HttpPost("RequestPackagePayment")]
        public async Task<IActionResult> RequestPackagePayment([FromBody] ExtendedPaymentParisinaRequestDto request)
        {
            // بررسی مدل ورودی
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Package Payment Request: {@Request}", request);
                return BadRequest(ModelState);
            }

            // بررسی فیلدهای ضروری
            if (!request.UserId.HasValue || !request.PackageId.HasValue || !request.DeceasedId.HasValue)
            {
                _logger.LogWarning("Missing required fields in Package Payment Request: {@Request}", request);
                return BadRequest(new { Message = "UserId, PackageId and DeceasedId are required." });
            }

            // ایجاد شماره سفارش
            long orderId = DateTime.UtcNow.Ticks;

            if (request.Amount <= 0)
            {
                _logger.LogWarning("Invalid Amount in Package Payment Request: {@Request}", request);
                return BadRequest(new { Message = "Amount must be greater than zero." });
            }

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

            // بررسی وجود متوفی
            var deceased = await _dbContext.Deceaseds.FirstOrDefaultAsync(d => d.Id == request.DeceasedId.Value);
            if (deceased == null)
            {
                _logger.LogWarning("Deceased not found for Package Payment Request: {@Request}", request);
                return BadRequest(new { Message = "Deceased not found." });
            }

            // ایجاد فاکتور جدید
            var factor = new Factors
            {
                UserId = request.UserId.Value,
                PackageId = request.PackageId.Value,
                DeceasedId = request.DeceasedId.Value,
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

            // درخواست پرداخت از درگاه
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
                    var savedFactor = await _dbContext.Factors.FirstOrDefaultAsync(f => f.OrderId == orderId);
                    if (savedFactor != null)
                    {
                        savedFactor.TrackingNumber = result.Token;
                        _dbContext.Factors.Update(savedFactor);
                        await _dbContext.SaveChangesAsync();
                    }

                    return Ok(new
                    {
                        Message = "Request successful",
                        Token = result.Token,
                        GatewayUrl = result.PaymentUrl
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

            long orderId = DateTime.UtcNow.Ticks;  // استفاده از Ticks برای OrderId به صورت long

            // اعتبارسنجی فیلدهای ضروری (بدون نیاز به DeceasedId)
            if (request.Amount <= 0 || !request.UserId.HasValue || !request.PackageId.HasValue)
            {
                _logger.LogWarning("Invalid request parameters for Package Renewal: {@Request}", request);
                return BadRequest(new { Message = "Invalid request parameters." });
            }

            // بررسی وجود کاربر
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

            // بررسی اینکه متوفی پکیج فعال دارد
            // پیدا کردن پکیج فعال برای متوفی
            var deceasedPackage = await _dbContext.DeceasedPackages
                .Where(dp => dp.DeceasedId == request.DeceasedId.Value && dp.PackageId == request.PackageId.Value && dp.IsActive)
                .FirstOrDefaultAsync();

            if (deceasedPackage == null)
            {
                _logger.LogWarning("No active package found to renew for the specified deceased: {@Request}", request);
                return BadRequest(new { Message = "No active package found to renew." });
            }

            // ایجاد رکورد فاکتور با وضعیت Pending
            var factor = new Factors
            {
                UserId = request.UserId.Value,
                PackageId = request.PackageId.Value,
                DeceasedId = request.DeceasedId.Value,
                TransactionDate = DateTime.UtcNow,
                Amount = request.Amount,
                Status = "Pending",
                TransactionType = "Renewal",
                PaymentGateway = "Melat",
                Description = $"Package renewal initiated for PackageId: {request.PackageId.Value}",
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
                    _logger.LogWarning("Payment service returned error for Package Renewal: {@Request}, Error: {Error}", request, result.Message);
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

            // بررسی وجود DeceasedId در درخواست
            if (request.DeceasedId == 0)
            {
                _logger.LogWarning("Missing DeceasedId in Upgrade Package Request: {@Request}", request);
                return BadRequest(new { Message = "DeceasedId is required." });
            }


            long orderId = DateTime.UtcNow.Ticks;  // استفاده از Ticks برای OrderId به صورت long

            // بررسی وجود کاربر
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
            {
                _logger.LogWarning("User not found for Package Upgrade: {@Request}", request);
                return BadRequest(new { Message = "User not found." });
            }

            // بررسی وجود پکیج فعلی و پکیج جدید
            var currentPackage = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == request.PackageId);
            var newPackage = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == request.NewPackageId);
            if (newPackage == null || newPackage.Price <= currentPackage.Price)
            {
                _logger.LogWarning("New package not found or not more expensive for Package Upgrade: {@Request}", request);
                return BadRequest(new { Message = "New package not found or not more expensive." });
            }


            // یافتن پکیج فعال فعلی برای متوفی مشخص
            var currentDeceasedPackage = await _dbContext.DeceasedPackages
                 .FirstOrDefaultAsync(dp => dp.DeceasedId == request.DeceasedId && dp.PackageId == request.PackageId && dp.IsActive);
            if (currentDeceasedPackage == null)
            {
                _logger.LogWarning("No active package found for upgrade for the specified deceased: {@Request}", request);
                return BadRequest(new { Message = "No active package found to upgrade." });
            }

            // بررسی تاریخ انقضای پکیج فعال فعلی برای جلوگیری از ارتقا زمانی که هنوز تاریخ انقضا باقی است
            if (currentDeceasedPackage.ExpirationDate.HasValue && currentDeceasedPackage.ExpirationDate.Value > DateTime.UtcNow)
            {
                _logger.LogWarning("Package cannot be upgraded as the current package is still valid: {@Request}", request);
                return BadRequest(new { Message = "Current package is still valid and cannot be upgraded." });
            }

            var priceDifference = (long)(newPackage.Price - currentPackage.Price);

            var factor = new Factors
            {
                UserId = request.UserId,
                PackageId = request.NewPackageId,
                DeceasedId = request.DeceasedId,
                TransactionDate = DateTime.UtcNow,
                Amount = priceDifference,
                Status = "Pending",
                TransactionType = "Upgrade",
                PaymentGateway = "Melat",
                Description = $"Package upgrade payment from PackageId: {request.PackageId} to PackageId: {request.NewPackageId}",
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

        //         [HttpPost("Callback")]
        // public async Task<IActionResult> Callback()
        // {
        //     try
        //     {
        //         var status = Convert.ToInt16(Request.Form["status"]);
        //         var token = Convert.ToInt64(Request.Form["Token"]);
        //         var rrn = Convert.ToInt64(Request.Form["RRN"]);

        //         var successRedirect = new UriBuilder("https://new.tarhimcode.ir/successful");
        //         var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/unsuccessful");

        //         var query = HttpUtility.ParseQueryString(string.Empty);
        //         query["RRN"] = rrn.ToString();

        //         // اگر وضعیت پرداخت موفق است
        //         if (status == 0 && rrn > 0)
        //         {
        //             var verifyModel = new PaymentVerifyModel { Token = token };
        //             var result = await _paymentService.VerifyPaymentAsync(verifyModel);

        //             if (result.Success)
        //             {
        //                 // جستجو برای فاکتور با استفاده از شماره پیگیری (TrackingNumber)
        //                 var factor = await _dbContext.Factors.FirstOrDefaultAsync(f => f.TrackingNumber == token.ToString());
        //                 if (factor != null)
        //                 {
        //                     // تغییر وضعیت فاکتور به "Success" و ثبت زمان پرداخت
        //                     factor.Status = "Success";
        //                     factor.PaidAt = DateTime.UtcNow;
        //                     _dbContext.Factors.Update(factor);
        //                     await _dbContext.SaveChangesAsync();

        //                     // در صورتی که فاکتور مربوط به پکیج باشد
        //                     if (factor.PackageId.HasValue)
        //                     {
        //                         var package = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == factor.PackageId.Value);
        //                         if (package != null)
        //                         {
        //                             switch (factor.TransactionType)
        //                             {
        //                                 case "Register":
        //                                     // استفاده از سرویس برای ثبت پکیج جدید به همراه شناسه متوفی
        //                                     await _packageTransactionService.HandleNewPackageRegistration(
        //                                         factor, 
        //                                         package, 
        //                                         factor.DeceasedId ?? 0  // اضافه شده
        //                                     );
        //                                     break;

        //                                 case "Renewal":
        //                                     await _packageTransactionService.HandlePackageRenewal(factor, package);
        //                                     break;

        //                                 case "Upgrade":
        //                                     await _packageTransactionService.HandlePackageUpgrade(factor, package);
        //                                     break;

        //                                 default:
        //                                     _logger.LogWarning($"Unhandled transaction type: {factor.TransactionType}");
        //                                     break;
        //                             }
        //                         }
        //                     }
        //                 }
        //                 successRedirect.Query = query.ToString();
        //                 return Redirect(successRedirect.ToString());
        //             }
        //         }

        //         // در صورتی که وضعیت پرداخت موفق نباشد، ارجاع به صفحه شکست پرداخت
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

        [HttpPost("Callback")]
        public async Task<IActionResult> Callback()
        {
            try
            {
                var status = Convert.ToInt16(Request.Form["status"]);
                var token = Convert.ToInt64(Request.Form["Token"]);
                var rrn = Convert.ToInt64(Request.Form["RRN"]);

                var successRedirect = new UriBuilder("https://new.tarhimcode.ir/successful");
                var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/unsuccessful");

                var query = HttpUtility.ParseQueryString(string.Empty);
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

                            // در صورتی که فاکتور مربوط به پکیج باشد
                            if (factor.PackageId != 0)  // فرض کنید صفر به معنای عدم وجود PackageId است
                            {
                                var package = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == factor.PackageId);
                                if (package != null)
                                {
                                    // فراخوانی متدهای مربوط به تراکنش پکیج بدون استفاده از DeceasedId
                                    switch (factor.TransactionType)
                                    {
                                        case "Register":
                                            await _packageTransactionService.HandleNewPackageRegistration(factor, package);
                                            break;
                                        case "Renewal":
                                            await _packageTransactionService.HandlePackageRenewal(factor, package);
                                            break;
                                        case "Upgrade":
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


    }
}