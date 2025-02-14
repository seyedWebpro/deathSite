using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.View.PaymentParsian;
using deathSite.View.PaymentParsian;
using ServiceReference1;
using ServiceReference2;

namespace deathSite.Services.Payment
{
    public class ParsianPaymentService : IParsianPaymentService
    {
        // مقادیر ثابت – در صورت نیاز می‌توان از IConfiguration استفاده کرد
        // private const string PIN = "Eeb8qci2yRPG64h6DC1Y";
        // private readonly string _callbackUrl = "https://new.tarhimcode.ir/api/PaymentParsian/Callback";  //https://new.tarhimcode.ir/api/PaymentParsian/Callback
        // public async Task<(bool Success, string Message, string Token, string PaymentUrl)> RequestPaymentAsync(PaymentRequestModel model)
        // {
        //     try
        //     {
        //         var client = new SaleServiceSoapClient(SaleServiceSoapClient.EndpointConfiguration.SaleServiceSoap);

        //         var request = new ClientSaleRequestData
        //         {
        //             LoginAccount = PIN,
        //             Amount = model.Amount,
        //             OrderId = model.OrderId,
        //             CallBackUrl = _callbackUrl,
        //             AdditionalData = model.Description
        //         };

        //         var response = await client.SalePaymentRequestAsync(request);
        //         var result = response.Body.SalePaymentRequestResult;

        //         if (result.Status == 0) // Success
        //         {
        //             // تبدیل Token از long به string
        //             string tokenStr = result.Token.ToString();
        //             string paymentUrl = $"https://pec.shaparak.ir/NewIPG/?Token={tokenStr}";
        //             return (true, "Request successful", tokenStr, paymentUrl);
        //         }

        //         return (false, result.Message, null, null);
        //     }
        //     catch (Exception ex)
        //     {
        //         return (false, ex.Message, null, null);
        //     }
        // }


        // public async Task<(bool Success, string Message, int Status, string RRN, string CardNumberMasked)> VerifyPaymentAsync(PaymentVerifyModel model)
        // {
        //     try
        //     {
        //         var client = new ConfirmServiceSoapClient(ConfirmServiceSoapClient.EndpointConfiguration.ConfirmServiceSoap);

        //         var request = new ClientConfirmRequestData
        //         {
        //             LoginAccount = PIN,
        //             Token = model.Token
        //         };

        //         var response = await client.ConfirmPaymentAsync(request);
        //         var result = response.Body.ConfirmPaymentResult;

        //         if (result.Status == 0)
        //         {
        //             // تبدیل RRN از long به string
        //             return (true, "Transaction verified successfully", (int)result.Status, result.RRN.ToString(), result.CardNumberMasked);
        //         }

        //         return (false, "Verification failed", (int)result.Status, null, null);
        //     }
        //     catch (Exception ex)
        //     {
        //         return (false, ex.Message, 0, null, null);
        //     }
        // }

        // ری دایرکت بهتر کاربر برای تجربه کاربری بهتر
        private const string PIN = "Eeb8qci2yRPG64h6DC1Y";
        private readonly string _callbackUrl = "https://new.tarhimcode.ir/api/PaymentParsian/Callback";

        public async Task<(bool Success, string Message, string Token, string PaymentUrl)> RequestPaymentAsync(PaymentRequestModel model)
        {
            try
            {
                var client = new SaleServiceSoapClient(SaleServiceSoapClient.EndpointConfiguration.SaleServiceSoap);

                // استفاده از آدرس کالبک ثابت
                var callbackWithRedirect = _callbackUrl;

                var request = new ClientSaleRequestData
                {
                    LoginAccount = PIN,
                    Amount = model.Amount,
                    OrderId = model.OrderId,
                    CallBackUrl = callbackWithRedirect,
                    AdditionalData = model.Description
                };

                var response = await client.SalePaymentRequestAsync(request);
                var result = response.Body.SalePaymentRequestResult;

                if (result.Status == 0)
                {
                    string tokenStr = result.Token.ToString();
                    string paymentUrl = $"https://pec.shaparak.ir/NewIPG/?Token={tokenStr}";
                    return (true, "درخواست با موفقیت ثبت شد", tokenStr, paymentUrl);
                }

                return (false, result.Message, null, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null, null);
            }
        }


        public async Task<(bool Success, string Message, PaymentVerificationResult Result)> VerifyPaymentAsync(PaymentVerifyModel model)
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

                if (result.Status == 0)
                {
                    var verificationResult = new PaymentVerificationResult
                    {
                        Status = (int)result.Status,
                        RRN = result.RRN.ToString(),
                        CardNumberMasked = result.CardNumberMasked,
                        Token = result.Token,
                        TransactionDate = DateTime.Now
                    };

                    return (true, "تراکنش با موفقیت تایید شد", verificationResult);
                }

                return (false, "تایید تراکنش ناموفق بود", null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

    }
}