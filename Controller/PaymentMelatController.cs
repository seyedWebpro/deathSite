// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using api.Services;
// using api.View.PaymentMelat;
// using deathSite.Services.Payment;
// using deathSite.View.PaymentMelat;

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
//                 return Ok(new { Message = "Request successful", RefId = result.RefId });
//             }
//             return BadRequest(new { Message = result.Message });
//         }

//         [HttpPost("PayRequest2")]
//         public async Task<IActionResult> PayRequest2([FromBody] PaymentRequestDto request)
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
//         public async Task<IActionResult> Callback([FromForm] CallbackRequestDto callbackRequest)
//         {
//             _logger.LogInformation("Callback received: RefId={RefId}, ResCode={ResCode}", callbackRequest.RefId, callbackRequest.ResCode);

//             if (callbackRequest.ResCode == "0") // تراکنش موفق
//             {
//                 // تأیید تراکنش
//                 var verifyResult = await _paymentService.VerifyRequestAsync(new VerifyRequestDto
//                 {
//                     OrderId = callbackRequest.OrderId,
//                     SaleOrderId = callbackRequest.SaleOrderId,
//                     SaleReferenceId = callbackRequest.SaleReferenceId
//                 });

//                 if (verifyResult.Success)
//                 {
//                     // انجام عملیات موردنظر پس از تأیید
//                     return Ok(new { Message = "Transaction verified successfully" });
//                 }
//                 else
//                 {
//                     return BadRequest(new { Message = verifyResult.Message });
//                 }
//             }

//             return BadRequest(new { Message = "Transaction failed", Code = callbackRequest.ResCode });
//         }

//     }
// }

// اضافه کردن کالبک 

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.Services;
using api.View.PaymentMelat;
using deathSite.Services.Payment;
using deathSite.View.PaymentMelat;
using System.Web;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMelatController : ControllerBase
    {
        private readonly IMelatPaymentService _paymentService;
        private readonly ILogger<PaymentMelatController> _logger;

        public PaymentMelatController(IMelatPaymentService paymentService, ILogger<PaymentMelatController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("PayRequest")]
        public async Task<IActionResult> PayRequest([FromBody] PaymentRequestDto request)
        {
            var result = await _paymentService.PayRequestAsync(request);
            if (result.Success)
            {
                var gatewayUrl = $"https://bpm.shaparak.ir/pgwchannel/startpay.mellat?RefId={result.RefId}";
                return Ok(new
                {
                    Message = "Request successful",
                    RefId = result.RefId,
                    GatewayUrl = gatewayUrl
                });
            }
            return BadRequest(new { Message = result.Message });
        }

        [HttpPost("VerifyRequest")]
        public async Task<IActionResult> VerifyRequest([FromBody] VerifyRequestDto request)
        {
            var result = await _paymentService.VerifyRequestAsync(request);
            if (result.Success)
            {
                return Ok(new { Message = "Transaction verified successfully" });
            }
            return BadRequest(new { Message = result.Message });
        }

        [HttpPost("SettleRequest")]
        public async Task<IActionResult> SettleRequest([FromBody] SettleRequestDto request)
        {
            var result = await _paymentService.SettleRequestAsync(request);
            if (result.Success)
            {
                return Ok(new { Message = "Settlement successful" });
            }
            return BadRequest(new { Message = result.Message });
        }

       [HttpPost("Callback")]
public async Task<IActionResult> Callback()
{
    try
    {
        // استخراج پارامترها از فرم
        var resCode = Request.Form["ResCode"].ToString();
        var refId = Request.Form["RefId"].ToString();
        var orderId = Convert.ToInt64(Request.Form["OrderId"]);
        var saleOrderId = Convert.ToInt64(Request.Form["SaleOrderId"]);
        var saleReferenceId = Convert.ToInt64(Request.Form["SaleReferenceId"]);

        // آدرس‌های ریدایرکت
        var successRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-success");
        var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["refId"] = refId;
        query["orderId"] = orderId.ToString();
        
        // بررسی موفقیت تراکنش بر اساس ResCode
        if (resCode == "0")
        {
            // فراخوانی متد Verify برای تایید نهایی تراکنش
            var verifyResult = await _paymentService.VerifyRequestAsync(new VerifyRequestDto
            {
                OrderId = orderId,
                SaleOrderId = saleOrderId,
                SaleReferenceId = saleReferenceId
            });

            if (verifyResult.Success)
            {
                successRedirect.Query = query.ToString();
                return Redirect(successRedirect.ToString()); // هدایت به صفحه موفقیت
            }
        }

        // در صورتی که تراکنش ناموفق باشد، کاربر به صفحه ناموفق هدایت می‌شود
        failureRedirect.Query = query.ToString();
        return Redirect(failureRedirect.ToString());
    }
    catch (Exception ex)
    {
        // در صورت بروز خطا، کاربر را به صفحه ناموفق هدایت کرده و پیام خطا را نمایش می‌دهیم
        var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["message"] = Uri.EscapeDataString(ex.Message);
        failureRedirect.Query = query.ToString();
        return Redirect(failureRedirect.ToString());
    }
}

        // بدون فراخوانی خودکار متد وریفای 

//         [HttpPost("Callback")]
// public async Task<IActionResult> Callback()
// {
//     try
//     {
//         // دریافت پارامترهای پاسخ از بانک
//         var resCode = Request.Form["ResCode"].ToString();
//         var refId = Request.Form["RefId"].ToString();
//         var orderId = Convert.ToInt64(Request.Form["OrderId"]);
//         var saleOrderId = Convert.ToInt64(Request.Form["SaleOrderId"]);
//         var saleReferenceId = Convert.ToInt64(Request.Form["SaleReferenceId"]);

//         // آدرس ثابت برای ریدایرکت
//         var redirectBuilder = new UriBuilder("https://new.tarhimcode.ir/payment-result");
//         var query = HttpUtility.ParseQueryString(redirectBuilder.Query);
//         query["refId"] = refId;
//         query["orderId"] = orderId.ToString();
//         query["saleOrderId"] = saleOrderId.ToString();
//         query["saleReferenceId"] = saleReferenceId.ToString();
//         query["resCode"] = resCode;  // اضافه کردن ResCode برای بررسی در کلاینت
//         redirectBuilder.Query = query.ToString();

//         // بدون فراخوانی VerifyRequest
//         return Redirect(redirectBuilder.ToString());
//     }
//     catch (Exception ex)
//     {
//         return Redirect($"https://new.tarhimcode.ir/payment-result?status=error&message={Uri.EscapeDataString(ex.Message)}");
//     }
// }


    }
}
