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


        // ری دایرکت کاربر به صفحه موفق یا ناموفق

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

                // آدرس‌های ریدایرکت
                var successRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-success");
                var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");

                var query = HttpUtility.ParseQueryString(string.Empty);
                query["token"] = token.ToString();  // اضافه کردن توکن برای نمایش در صفحه

                // در صورت موفق بودن پرداخت
                if (status == 0 && rrn > 0)
                {
                    var verifyModel = new PaymentVerifyModel { Token = token };
                    var result = await _paymentService.VerifyPaymentAsync(verifyModel);

                    if (result.Success)
                    {
                        successRedirect.Query = query.ToString();
                        return Redirect(successRedirect.ToString()); // هدایت به صفحه موفقیت
                    }
                }

                // در صورتی که پرداخت ناموفق باشد یا وریفای شکست بخورد
                failureRedirect.Query = query.ToString();
                return Redirect(failureRedirect.ToString());
            }
            catch (Exception ex)
            {
                // در صورت بروز خطا، به صفحه ناموفق هدایت می‌کنیم و پیام خطا را هم اضافه می‌کنیم
                var failureRedirect = new UriBuilder("https://new.tarhimcode.ir/payment-failure");
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["message"] = Uri.EscapeDataString(ex.Message);
                failureRedirect.Query = query.ToString();
                return Redirect(failureRedirect.ToString());
            }
        }

    }
}