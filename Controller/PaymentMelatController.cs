using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.Services;
using api.View.PaymentMelat;
using deathSite.Services.Payment;
using deathSite.View.PaymentMelat;

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
                return Ok(new { Message = "Request successful", RefId = result.RefId });
            }
            return BadRequest(new { Message = result.Message });
        }

        [HttpPost("PayRequest2")]
        public async Task<IActionResult> PayRequest2([FromBody] PaymentRequestDto request)
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
        public async Task<IActionResult> Callback([FromForm] CallbackRequestDto callbackRequest)
        {
            _logger.LogInformation("Callback received: RefId={RefId}, ResCode={ResCode}", callbackRequest.RefId, callbackRequest.ResCode);

            if (callbackRequest.ResCode == "0") // تراکنش موفق
            {
                // تأیید تراکنش
                var verifyResult = await _paymentService.VerifyRequestAsync(new VerifyRequestDto
                {
                    OrderId = callbackRequest.OrderId,
                    SaleOrderId = callbackRequest.SaleOrderId,
                    SaleReferenceId = callbackRequest.SaleReferenceId
                });

                if (verifyResult.Success)
                {
                    // انجام عملیات موردنظر پس از تأیید
                    return Ok(new { Message = "Transaction verified successfully" });
                }
                else
                {
                    return BadRequest(new { Message = verifyResult.Message });
                }
            }

            return BadRequest(new { Message = "Transaction failed", Code = callbackRequest.ResCode });
        }

    }
}