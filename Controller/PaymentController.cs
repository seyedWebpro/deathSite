using System;
using System.ServiceModel;
using System.Threading.Tasks;
using api.View.PaymentMelat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceReference;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly PaymentGatewayClient _client;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
            var endpoint = new EndpointAddress("https://bpm.shaparak.ir/pgwchannel/services/pgw?wsdl");
            var binding = new BasicHttpBinding
            {
                Security = new BasicHttpSecurity
                {
                    Mode = BasicHttpSecurityMode.Transport
                }
            };

            _client = new PaymentGatewayClient(binding, endpoint);
        }

        [HttpPost("PayRequest")]
        public async Task<IActionResult> PayRequest([FromBody] PaymentRequestDto request)
        {
            try
            {
                // فراخوانی تابع bpPayRequest
                var response = await _client.bpPayRequestAsync(
                    request.TerminalId,        // terminalId
                    request.Username,          // userName
                    request.Password,          // userPassword
                    request.OrderId,           // orderId
                    request.Amount,            // amount
                    request.LocalDate,         // localDate
                    request.LocalTime,         // localTime
                    request.AdditionalData,    // additionalData
                    request.CallBackUrl,       // callBackUrl
                    request.PayerId,           // payerId
                    request.MobileNo,          // mobileNo
                    request.EncPan,            // encPan
                    request.PanHiddenMode,     // panHiddenMode
                    request.CartItem,          // cartItem
                    request.Enc                // enc
                );

                // دسترسی به مقدار بازگشتی
                var resultCode = response.Body.@return;

                _logger.LogInformation("bpPayRequest Response: {Response}", resultCode);

                // اگر resultCode جداکننده دارد، آن را پردازش می‌کنیم
                var responseParts = resultCode.Split(',');

                // بررسی نتیجه
                if (responseParts[0] == "0")
                {
                    var refId = responseParts[1]; // گرفتن RefId
                    return Ok(new { Message = "Request successful", RefId = refId });
                }

                return BadRequest(new { Message = "Request failed", Code = responseParts[0] });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PayRequest");
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }


        [HttpPost("VerifyRequest")]
        public async Task<IActionResult> VerifyRequest([FromBody] VerifyRequestDto request)
        {
            try
            {
                // فراخوانی متد سرویس
                var response = await _client.bpVerifyRequestAsync(
                    request.TerminalId,
                    request.Username,
                    request.Password,
                    request.OrderId,
                    request.SaleOrderId,
                    request.SaleReferenceId
                );

                // دسترسی به مقدار بازگشتی از Body
                var resultCode = response.Body.@return;

                _logger.LogInformation("bpVerifyRequest Response: {Response}", resultCode);

                // بررسی نتیجه
                if (resultCode == "0")
                {
                    return Ok(new { Message = "Transaction verified successfully" });
                }

                return BadRequest(new { Message = "Verification failed", Code = resultCode });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VerifyRequest");
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }



        [HttpPost("SettleRequest")]
        public async Task<IActionResult> SettleRequest([FromBody] SettleRequestDto request)
        {
            try
            {
                // فراخوانی متد سرویس
                var response = await _client.bpSettleRequestAsync(
                    request.TerminalId,
                    request.Username,
                    request.Password,
                    request.OrderId,
                    request.SaleOrderId,
                    request.SaleReferenceId
                );

                // دسترسی به مقدار بازگشتی از Body
                var resultCode = response.Body.@return;

                _logger.LogInformation("bpSettleRequest Response: {Response}", resultCode);

                // بررسی نتیجه
                if (resultCode == "0")
                {
                    return Ok(new { Message = "Settlement successful" });
                }

                return BadRequest(new { Message = "Settlement failed", Code = resultCode });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SettleRequest");
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        // متد خصوصی برای ایجاد کلاینت SOAP (اختیاری)
        private PaymentGatewayClient CreatePaymentGatewayClient()
        {
            var endpoint = new EndpointAddress("https://bpm.shaparak.ir/pgwchannel/services/pgw?wsdl");
            var binding = new BasicHttpBinding
            {
                Security = new BasicHttpSecurity
                {
                    Mode = BasicHttpSecurityMode.Transport
                }
            };

            return new PaymentGatewayClient(binding, endpoint);
        }

        

    }
}
