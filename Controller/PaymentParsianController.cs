using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.View.PaymentParsian;
using deathSite.Services.Payment;
using System.Web;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentParsianController : ControllerBase
    {
        private readonly IParsianPaymentService _paymentService;

        public PaymentParsianController(IParsianPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("RequestPayment")]
        public async Task<IActionResult> RequestPayment([FromBody] PaymentRequestModel model)
        {
            var result = await _paymentService.RequestPaymentAsync(model);
            if (result.Success)
            {
                return Ok(new { Token = result.Token, PaymentUrl = result.PaymentUrl });
            }
            return BadRequest(new { Message = result.Message });
        }


        [HttpPost("VerifyPayment")]
        public async Task<IActionResult> VerifyPayment([FromBody] PaymentVerifyModel model)
        {
            var result = await _paymentService.VerifyPaymentAsync(model);
            if (result.Success)
            {
                return Ok(result.Result);
            }
            return BadRequest(new { Message = result.Message });
        }

        [HttpPost("Callback")]
        public async Task<IActionResult> Callback()
        {
            try
            {
                // دریافت پارامترهای دیگر از فرم
                var status = Convert.ToInt16(Request.Form["status"]);
                var token = Convert.ToInt64(Request.Form["Token"]);
                var orderId = Convert.ToInt64(Request.Form["OrderId"]);
                var amount = Request.Form["Amount"];
                var terminalNo = Convert.ToInt32(Request.Form["TerminalNo"]);
                var rrn = Convert.ToInt64(Request.Form["RRN"]);

                // آدرس ثابت ریدایرکت و فقط توکن در URL
                var redirectBuilder = new UriBuilder("https://new.tarhimcode.ir/payment-result");  // آدرس ثابت
                var query = HttpUtility.ParseQueryString(redirectBuilder.Query);
                query["token"] = token.ToString();  // فقط توکن را اضافه کنید
                redirectBuilder.Query = query.ToString();

                // در صورت موفق بودن پرداخت
                if (status == 0 && rrn > 0)
                {
                    var verifyModel = new PaymentVerifyModel { Token = token };
                    var result = await _paymentService.VerifyPaymentAsync(verifyModel);

                    if (result.Success)
                    {
                        return Redirect(redirectBuilder.ToString()); // فقط توکن به آدرس ریدایرکت اضافه می‌شود
                    }

                    return BadRequest(new
                    {
                        Success = false,
                        Message = "پرداخت ناموفق",
                        Status = status,
                        Token = token
                    });
                }

                // در صورتی که پرداخت ناموفق باشد
                return BadRequest(new
                {
                    Success = false,
                    Message = "پرداخت ناموفق",
                    Status = status,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                // در صورت خطا، به صفحه خطا هدایت می‌کنیم
                return Redirect($"https://new.tarhimcode.ir/payment-result?status=error&message={Uri.EscapeDataString(ex.Message)}");
            }
        }

    }
}



//  // [HttpPost("Callback")]
// public async Task<IActionResult> Callback()
// {
//     try
//     {
//         // دریافت پارامترها از Form Data
//         var status = Convert.ToInt16(Request.Form["status"]);
//         var token = Convert.ToInt64(Request.Form["Token"]);
//         var orderId = Convert.ToInt64(Request.Form["OrderId"]);

//         // اگر پرداخت موفق بود
//         if (status == 0)
//         {
//             var verifyModel = new PaymentVerifyModel
//             {
//                 Token = token
//             };

//             // فراخوانی متد verify
//             var result = await _paymentService.VerifyPaymentAsync(verifyModel);

//             if (result.Success)
//             {
//                 // ذخیره اطلاعات تراکنش در دیتابیس
//                 return Ok(new
//                 {
//                     Message = "پرداخت با موفقیت انجام شد",
//                     RRN = result.RRN,
//                     CardNumber = result.CardNumberMasked
//                 });
//             }
//         }

//         return BadRequest(new { Message = "پرداخت ناموفق" });
//     }
//     catch (Exception ex)
//     {
//         return BadRequest(new { Message = ex.Message });
//     }
// }

//         [HttpPost("Callback")]
// public async Task<IActionResult> Callback()
// {
//     try 
//     {
//         var status = Convert.ToInt16(Request.Form["status"]);
//         var token = Convert.ToInt64(Request.Form["Token"]);
//         var orderId = Convert.ToInt64(Request.Form["OrderId"]);
//         var amount = Request.Form["Amount"];
//         var terminalNo = Convert.ToInt32(Request.Form["TerminalNo"]);
//         var rrn = Convert.ToInt64(Request.Form["RRN"]);

//         // بررسی وضعیت تراکنش
//         if (status == 0 && rrn > 0)
//         {
//             var verifyModel = new PaymentVerifyModel 
//             { 
//                 Token = token 
//             };

//             var result = await _paymentService.VerifyPaymentAsync(verifyModel);

//             if (result.Success)
//             {
//                 // ذخیره در دیتابیس
//                 // await SaveTransaction(new TransactionModel
//                 // {
//                 //     Token = token,
//                 //     OrderId = orderId,
//                 //     Amount = amount,
//                 //     RRN = rrn,
//                 //     CardNumber = result.CardNumberMasked,
//                 //     TerminalNo = terminalNo,
//                 //     PaymentDate = DateTime.Now
//                 // });

//                 return Ok(new { 
//                     Message = "پرداخت با موفقیت انجام شد",
//                     RRN = result.RRN,
//                     CardNumber = result.CardNumberMasked,
//                     OrderId = orderId,
//                     Amount = amount
//                 });
//             }
//         }

//         return BadRequest(new { 
//             Message = "پرداخت ناموفق",
//             Status = status
//         });
//     }
//     catch (Exception ex)
//     {
//         // لاگ خطا
//         return BadRequest(new { Message = ex.Message });
//     }
// }