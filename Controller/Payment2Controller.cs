using api.View.PaymentParsian;
using Microsoft.AspNetCore.Mvc;
using ServiceReference1;
using ServiceReference2;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class Payment2Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private const string PIN = "Eeb8qci2yRPG64h6DC1Y";
        private readonly string _callbackUrl;

        public Payment2Controller(IConfiguration configuration)
        {
            _configuration = configuration;
            _callbackUrl = "http://new.tarhimcode.ir"; // Update with your actual callback URL   https://your-domain.com/api/payment/verify
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestPayment([FromBody] PaymentRequestModel model)
        {
            try
            {
                var client = new SaleServiceSoapClient(SaleServiceSoapClient.EndpointConfiguration.SaleServiceSoap);

                var request = new ClientSaleRequestData
                {
                    LoginAccount = PIN,
                    Amount = model.Amount,
                    OrderId = model.OrderId,
                    CallBackUrl = _callbackUrl,
                    AdditionalData = model.Description
                };

                var response = await client.SalePaymentRequestAsync(request);
                var result = response.Body.SalePaymentRequestResult;

                if (result.Status == 0) // Success
                {
                    return Ok(new
                    {
                        Token = result.Token,
                        PaymentUrl = $"https://pec.shaparak.ir/NewIPG/?Token={result.Token}"
                    });
                }

                return BadRequest(new { Message = result.Message, Status = result.Status });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing the payment request", Error = ex.Message });
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyPayment([FromBody] PaymentVerifyModel model)
        {
            try
            {
                var client = new ConfirmServiceSoapClient(ConfirmServiceSoapClient.EndpointConfiguration.ConfirmServiceSoap);

                var request = new ClientConfirmRequestData
                {
                    LoginAccount = PIN,
                    Token = model.Token
                };

                var response = await client.ConfirmPaymentAsync(request);
                var result = response.Body.ConfirmPaymentResult;

                // لیست کدهای خطای احتمالی
                var errorMessages = new Dictionary<int, string>
        {
            { -1531, "توکن نامعتبر است" },
            { -1532, "تراکنش قبلاً تأیید شده" },
            // سایر کدهای خطا را اضافه کنید
        };

                if (result.Status == 0)
                {
                    return Ok(new
                    {
                        Status = result.Status,
                        RRN = result.RRN,
                        CardNumberMasked = result.CardNumberMasked
                    });
                }

                return BadRequest(new
                {
                    Message = errorMessages.ContainsKey(result.Status)
                        ? errorMessages[result.Status]
                        : "خطا در تأیید پرداخت",
                    Status = result.Status
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "خطای سیستمی در تأیید پرداخت", Error = ex.Message });
            }
        }

    }
}