using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.View.PaymentParsian;
using deathSite.View.PaymentParsian;

namespace deathSite.Services.Payment
{
    public interface IParsianPaymentService
    {
        Task<(bool Success, string Message, string Token, string PaymentUrl)> RequestPaymentAsync(PaymentRequestModel model);
        Task<(bool Success, string Message, PaymentVerificationResult Result)> VerifyPaymentAsync(PaymentVerifyModel model);
    }
}