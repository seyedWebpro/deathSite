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
using deathSite.View.Dead;
using api.Services;

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

         private readonly ISmsService _smsService;

        public PaymentMelatController(
            IMelatPaymentService paymentService,
            ILogger<PaymentMelatController> logger,
            apiContext dbContext,
            IPackageTransactionService packageTransactionService
            , ISmsService smsService)
        {
            _paymentService = paymentService;
            _logger = logger;
            _dbContext = dbContext;
            _packageTransactionService = packageTransactionService;
            _smsService = smsService;
        }
        // اضافه کردن ایدی متوفی 

[HttpPost("PayRequest")]
public async Task<IActionResult> PayRequest([FromBody] ExtendedPaymentRequestDto request)
{
    // چک کردن مدل ورودی
    if (!ModelState.IsValid)
    {
        _logger.LogWarning("Invalid model state for Payment Request: {@Request}", request);
        return BadRequest(ModelState);
    }

    // چک کردن داده‌های مورد نیاز
    if (!request.UserId.HasValue || !request.PackageId.HasValue || !request.DeceasedId.HasValue)
    {
        _logger.LogWarning("Missing required fields in Payment Request: {@Request}", request);
        return BadRequest(new { Message = "UserId, PackageId and DeceasedId are required." });
    }

    // ایجاد شناسه پرداخت و شماره سفارش
    string payerId = DateTime.UtcNow.Ticks.ToString();
    long orderId = DateTime.UtcNow.Ticks;

    if (request.Amount <= 0)
    {
        _logger.LogWarning("Invalid Amount in Payment Request: {@Request}", request);
        return BadRequest(new { Message = "Amount must be greater than zero." });
    }

    // چک کردن وجود کاربر و پکیج
    var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == request.UserId.Value);
    if (user == null)
    {
        _logger.LogWarning("User not found for Payment Request: {@Request}", request);
        return BadRequest(new { Message = "User not found." });
    }

    var package = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == request.PackageId.Value);
    if (package == null)
    {
        _logger.LogWarning("Package not found for Payment Request: {@Request}", request);
        return BadRequest(new { Message = "Package not found." });
    }

    var deceased = await _dbContext.Deceaseds.FirstOrDefaultAsync(d => d.Id == request.DeceasedId.Value);
    if (deceased == null)
    {
        _logger.LogWarning("Deceased not found for Payment Request: {@Request}", request);
        return BadRequest(new { Message = "Deceased not found." });
    }

    // اضافه کردن پکیج به فاکتور
    var factor = new Factors
    {
        UserId = request.UserId.Value,
        PackageId = request.PackageId.Value,
        DeceasedId = request.DeceasedId.Value,
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

    // درخواست پرداخت از درگاه
    var gatewayRequest = new PaymentRequestDto
    {
        OrderId = orderId,
        Amount = request.Amount,
        PayerId = payerId
    };

    try
    {
        var result = await _paymentService.PayRequestAsync(gatewayRequest);
        if (result.Success)
        {
            var savedFactor = await _dbContext.Factors.FirstOrDefaultAsync(f => f.OrderId == orderId);
            if (savedFactor != null)
            {
                savedFactor.TrackingNumber = result.RefId;
                _dbContext.Factors.Update(savedFactor);
                await _dbContext.SaveChangesAsync();
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


// [HttpPost("RenewPackage")]
// public async Task<IActionResult> RenewPackage([FromBody] ExtendedPaymentRequestDto request)
// {
//     // چک کردن مدل ورودی
//     if (!ModelState.IsValid)
//     {
//         _logger.LogWarning("Invalid model state for Package Renewal Request: {@Request}", request);
//         return BadRequest(ModelState);
//     }

//     // چک کردن داده‌های مورد نیاز
//     if (!request.UserId.HasValue || !request.PackageId.HasValue || !request.DeceasedId.HasValue)
//     {
//         _logger.LogWarning("Missing required fields in Package Renewal Request: {@Request}", request);
//         return BadRequest(new { Message = "UserId, PackageId, and DeceasedId are required." });
//     }

//     // ایجاد شناسه پرداخت و شماره سفارش
//     string payerId = DateTime.UtcNow.Ticks.ToString();
//     long orderId = DateTime.UtcNow.Ticks;

//     // چک کردن وجود کاربر و پکیج
//     var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == request.UserId.Value);
//     if (user == null)
//     {
//         _logger.LogWarning("User not found for Package Renewal: {@Request}", request);
//         return BadRequest(new { Message = "User not found." });
//     }

//     var package = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == request.PackageId.Value);
//     if (package == null)
//     {
//         _logger.LogWarning("Package not found for Package Renewal: {@Request}", request);
//         return BadRequest(new { Message = "Package not found." });
//     }

//     var deceased = await _dbContext.Deceaseds.FirstOrDefaultAsync(d => d.Id == request.DeceasedId.Value);
//     if (deceased == null)
//     {
//         _logger.LogWarning("Deceased not found for Package Renewal: {@Request}", request);
//         return BadRequest(new { Message = "Deceased not found." });
//     }

//     // پیدا کردن پکیج فعال برای متوفی
//     var deceasedPackage = await _dbContext.DeceasedPackages
//         .Where(dp => dp.DeceasedId == request.DeceasedId.Value && dp.PackageId == request.PackageId.Value && dp.IsActive)
//         .FirstOrDefaultAsync();

//     if (deceasedPackage == null)
//     {
//         _logger.LogWarning("No active package found to renew for the specified deceased: {@Request}", request);
//         return BadRequest(new { Message = "No active package found to renew." });
//     }

//     // بررسی تاریخ انقضا و مدت باقی‌مانده
//     if (deceasedPackage.ExpirationDate.HasValue)
//     {
//         var daysRemaining = (deceasedPackage.ExpirationDate.Value - DateTime.UtcNow).Days;
        
//         // اگر کمتر از 30 روز باقی مانده باشد
//         if (daysRemaining >= 0 && daysRemaining <= 30)
//         {
//             // تمدید پکیج
//             deceasedPackage.ExpirationDate = deceasedPackage.ExpirationDate?.AddDays(30); // تمدید 30 روزه
//             _dbContext.DeceasedPackages.Update(deceasedPackage);
//             await _dbContext.SaveChangesAsync();

//             // اضافه کردن فاکتور تمدید
//             var factor = new Factors
//             {   
//                 UserId = request.UserId.Value,
//                 PackageId = request.PackageId.Value,
//                 DeceasedId = request.DeceasedId.Value,
//                 TransactionDate = DateTime.UtcNow,
//                 Amount = request.Amount,
//                 Status = "Pending",
//                 TransactionType = "Renewal",
//                 PaymentGateway = "Melat",
//                 Description = $"Package renewal initiated for PackageId: {request.PackageId.Value}",
//                 OrderId = orderId
//             };

//             try
//             {
//                 _dbContext.Factors.Add(factor);
//                 await _dbContext.SaveChangesAsync();
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error saving factor for Package Renewal: {@Request}", request);
//                 return StatusCode(500, new { Message = "Internal server error while saving payment data." });
//             }

//             // درخواست پرداخت از درگاه
//             var gatewayRequest = new PaymentRequestDto
//             {
//                 OrderId = orderId,
//                 Amount = request.Amount,
//                 PayerId = payerId
//             };

//             try
//             {
//                 var result = await _paymentService.PayRequestAsync(gatewayRequest);
//                 if (result.Success)
//                 {
//                     var savedFactor = await _dbContext.Factors.FirstOrDefaultAsync(f => f.OrderId == orderId);
//                     if (savedFactor != null)
//                     {
//                         savedFactor.TrackingNumber = result.RefId;
//                         _dbContext.Factors.Update(savedFactor);
//                         await _dbContext.SaveChangesAsync();
//                     }

//                     var gatewayUrl = $"https://bpm.shaparak.ir/pgwchannel/startpay.mellat?RefId={result.RefId}";
//                     return Ok(new
//                     {
//                         Message = "Renewal request successful",
//                         RefId = result.RefId,
//                         GatewayUrl = gatewayUrl
//                     });
//                 }
//                 else
//                 {
//                     _logger.LogWarning("Payment service returned error for Package Renewal: {@Request}, Error: {Error}", request, result.Message);
//                     return BadRequest(new { Message = result.Message });
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error occurred during payment processing: {@Request}", request);
//                 return StatusCode(500, new { Message = "Internal server error during payment processing." });
//             }
//         }
//         else if (daysRemaining > 30) // اگر بیشتر از 30 روز باقی مانده باشد
//         {
//             _logger.LogWarning("Cannot renew the package: More than 30 days left on the expiry date for the specified deceased: {@Request}", request);
//             return BadRequest(new { Message = "Package cannot be renewed. More than 30 days left on the expiry date." , StatusCode = 404 });
//         }
//         else
//         {
//             _logger.LogWarning("Cannot renew the package: Expired already for the specified deceased: {@Request}", request);
//             return BadRequest(new { Message = "Package has already expired." });
//         }
//     }
//     else
//     {
//         _logger.LogWarning("Expiration date is not set for the package: {@Request}", request);
//         return BadRequest(new { Message = "Expiration date is not set for the package." });
//     }
// }

[HttpPost("RenewPackage")]
public async Task<IActionResult> RenewPackage([FromBody] ExtendedPaymentRequestDto request)
{
    if (!ModelState.IsValid)
    {
        _logger.LogWarning("Invalid model state for Package Renewal Request: {@Request}", request);
        return BadRequest(ModelState);
    }

    if (!request.UserId.HasValue || !request.PackageId.HasValue || !request.DeceasedId.HasValue)
    {
        _logger.LogWarning("Missing required fields in Package Renewal Request: {@Request}", request);
        return BadRequest(new { Message = "UserId, PackageId, and DeceasedId are required." });
    }

    string payerId = DateTime.UtcNow.Ticks.ToString();
    long orderId = DateTime.UtcNow.Ticks;

    var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == request.UserId.Value);
    if (user == null)
    {
        _logger.LogWarning("User not found for Package Renewal: {@Request}", request);
        return BadRequest(new { Message = "User not found." });
    }

    var package = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == request.PackageId.Value);
    if (package == null)
    {
        _logger.LogWarning("Package not found for Package Renewal: {@Request}", request);
        return BadRequest(new { Message = "Package not found." });
    }

    var deceased = await _dbContext.Deceaseds.FirstOrDefaultAsync(d => d.Id == request.DeceasedId.Value);
    if (deceased == null)
    {
        _logger.LogWarning("Deceased not found for Package Renewal: {@Request}", request);
        return BadRequest(new { Message = "Deceased not found." });
    }

    var deceasedPackage = await _dbContext.DeceasedPackages
        .Where(dp => dp.DeceasedId == request.DeceasedId.Value && dp.PackageId == request.PackageId.Value && dp.IsActive)
        .FirstOrDefaultAsync();

    if (deceasedPackage == null)
    {
        _logger.LogWarning("No active package found to renew for the specified deceased: {@Request}", request);
        return BadRequest(new { Message = "No active package found to renew." });
    }

    if (deceasedPackage.ExpirationDate.HasValue)
    {
        var daysRemaining = (deceasedPackage.ExpirationDate.Value - DateTime.UtcNow).Days;
        
        if (daysRemaining > 30)
        {
            _logger.LogWarning("Cannot renew the package: More than 30 days left on the expiry date for the specified deceased: {@Request}", request);
            return BadRequest(new { Message = "Package cannot be renewed. More than 30 days left on the expiry date.", StatusCode = 404 });
        }
    }
    
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

    var gatewayRequest = new PaymentRequestDto
    {
        OrderId = orderId,
        Amount = request.Amount,
        PayerId = payerId
    };

    try
    {
        var result = await _paymentService.PayRequestAsync(gatewayRequest);
        if (result.Success)
        {
            var savedFactor = await _dbContext.Factors.FirstOrDefaultAsync(f => f.OrderId == orderId);
            if (savedFactor != null)
            {
                savedFactor.TrackingNumber = result.RefId;
                _dbContext.Factors.Update(savedFactor);
                await _dbContext.SaveChangesAsync();
            }

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
            _logger.LogWarning("Payment service returned error for Package Renewal: {@Request}, Error: {Error}", request, result.Message);
            return BadRequest(new { Message = result.Message });
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error occurred during payment processing: {@Request}", request);
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

    // بررسی وجود DeceasedId در درخواست
    if (request.DeceasedId == 0)
    {
        _logger.LogWarning("Missing DeceasedId in Upgrade Package Request: {@Request}", request);
        return BadRequest(new { Message = "DeceasedId is required." });
    }

    string payerId = DateTime.UtcNow.Ticks.ToString();
    long orderId = DateTime.UtcNow.Ticks;

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

            // غیرفعال کردن پکیج فعلی
            currentDeceasedPackage.IsActive = false;
            _dbContext.DeceasedPackages.Update(currentDeceasedPackage);
            await _dbContext.SaveChangesAsync();

            // ایجاد پکیج جدید (ارتقا یافته) برای متوفی
            var newDeceasedPackage = new DeceasedPackage
            {
                DeceasedId = request.DeceasedId,
                PackageId = request.NewPackageId,
                ActivationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(30), // تمدید 30 روزه
                IsActive = true,
                IsFreePackage = false,
                FactorId = factor.Id
            };

            _dbContext.DeceasedPackages.Add(newDeceasedPackage);
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
      

        // اضافه کردن ایدی متوفی 

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

        var successRedirect = new UriBuilder("https://new.tarhimcode.ir/successful");
        var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/unsuccessful");

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
                var factor = await _dbContext.Factors.FirstOrDefaultAsync(f => f.TrackingNumber == refId);
                if (factor != null)
                {
                    factor.Status = "Success";
                    factor.PaidAt = DateTime.UtcNow;
                    factor.OrderId = orderId;
                    _dbContext.Factors.Update(factor);
                    await _dbContext.SaveChangesAsync();

                    // دریافت متوفی مربوطه
                    var deceased = await _dbContext.Deceaseds.FirstOrDefaultAsync(d => d.Id == factor.DeceasedId);
                    if (deceased != null)
                    {
                        deceased.IsApproved = ApprovalStatus.Approved; // تغییر وضعیت به تایید شده
                        _dbContext.Deceaseds.Update(deceased);
                        await _dbContext.SaveChangesAsync();

                        // فراخوانی سرویس پکیج بسته به نوع تراکنش
                        var package = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == factor.PackageId);
                        if (package != null)
                        {
                            switch (factor.TransactionType)
                            {
                                case "Register":
                                    await _packageTransactionService.HandleNewPackageRegistration(factor, package);
                                    // ارسال پیامک خرید موفق
                                    var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == factor.UserId);
                                    if (user != null)
                                    {
                                        await _smsService.SendOrderSuccessSmsAsync(user.phoneNumber);
                                    }
                                    break;

                                case "Renewal":
                                    await _packageTransactionService.HandlePackageRenewal(factor, package);
                                    // ارسال پیامک تمدید موفق
                                    var renewalUser = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == factor.UserId);
                                    if (renewalUser != null)
                                    {
                                        await _smsService.SendRenewalSuccessSmsAsync(renewalUser.phoneNumber);
                                    }
                                    break;

                                case "Upgrade":
                                    await _packageTransactionService.HandlePackageUpgrade(factor, package);
                                    // ارسال پیامک ارتقاء موفق
                                    var upgradeUser = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == factor.UserId);
                                    if (upgradeUser != null)
                                    {
                                        await _smsService.SendUpgradeSuccessSmsAsync(upgradeUser.phoneNumber);
                                    }
                                    break;

                                default:
                                    _logger.LogWarning($"Unhandled transaction type: {factor.TransactionType}");
                                    break;
                            }
                        }
                    }

                    query["status"] = "success";
                    successRedirect.Query = query.ToString();
                    return Redirect(successRedirect.ToString());
                }
            }
        }

        query["status"] = "failure";
        failureRedirect.Query = query.ToString();
        return Redirect(failureRedirect.ToString());
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "خطایی در پردازش بازگشت از درگاه پرداخت رخ داده است.");
        var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["message"] = Uri.EscapeDataString(ex.Message);
        failureRedirect.Query = query.ToString();
        return Redirect(failureRedirect.ToString());
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

//         var successRedirect = new UriBuilder("https://new.tarhimcode.ir/successful");
//         var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/unsuccessful");

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
//                 var factor = await _dbContext.Factors.FirstOrDefaultAsync(f => f.TrackingNumber == refId);
//                 if (factor != null)
//                 {
//                     factor.Status = "Success";
//                     factor.PaidAt = DateTime.UtcNow;
//                     factor.OrderId = orderId;
//                     _dbContext.Factors.Update(factor);
//                     await _dbContext.SaveChangesAsync();

//                     // بازیابی پکیج مربوطه از دیتابیس
//                     var package = await _dbContext.packages.FirstOrDefaultAsync(p => p.Id == factor.PackageId);
//                     if (package == null)
//                     {
//                         _logger.LogWarning("Package not found in callback for factor: {RefId}", refId);
//                         return BadRequest(new { Message = "Package not found in callback." });
//                     }

//                     // بر اساس نوع تراکنش، عملیات مربوطه انجام می‌شود
//                     if (factor.TransactionType == "Register")
//                     {
//                         // خرید پکیج جدید
//                         var deceasedPackage = await _packageTransactionService.HandleNewPackageRegistration(factor, package);
//                     }
//                     else if (factor.TransactionType == "Renewal")
//                     {
//                         await _packageTransactionService.HandlePackageRenewal(factor, package);
//                     }
//                     else if (factor.TransactionType == "Upgrade")
//                     {
//                         await _packageTransactionService.HandlePackageUpgrade(factor, package);
//                     }

//                     successRedirect.Query = query.ToString();
//                     return Redirect(successRedirect.ToString());
//                 }
//             }
//         }

//         failureRedirect.Query = query.ToString();
//         return Redirect(failureRedirect.ToString());
//     }
//     catch (Exception ex)
//     {
//         _logger.LogError(ex, "Error in payment callback processing.");
//         return StatusCode(500, new { Message = "Internal server error." });
//     }
// }




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

//         var successRedirect = new UriBuilder("https://new.tarhimcode.ir/successful");
//         var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/unsuccessful");

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
//                 var factor = await _dbContext.Factors.FirstOrDefaultAsync(f => f.TrackingNumber == refId);

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
//                                 // نیازی به استفاده از شناسه متوفی در این مرحله نیست.
//                                 // ثبت پکیج جدید بدون نیاز به متوفی
//                                 if (factor.TransactionType == "Renewal")
//                                 {
//                                     await _packageTransactionService.HandlePackageRenewal(factor, package);
//                                 }
//                                 else if (factor.TransactionType == "Upgrade")
//                                 {
//                                     var newPackage = await _dbContext.packages
//                                         .FirstOrDefaultAsync(p => p.Id == factor.PackageId.Value);
//                                     await _packageTransactionService.HandlePackageUpgrade(factor, newPackage);
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

//         failureRedirect.Query = query.ToString();
//         return Redirect(failureRedirect.ToString());
//     }
//     catch (Exception ex)
//     {
//         _logger.LogError(ex, "Error in payment callback processing.");
//         return StatusCode(500, new { Message = "Internal server error." });
//     }
// }

    }
}
