using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.View.PaymentParsian;

namespace deathSite.Services.Payment
{
    public interface IParsianPaymentService
    {
        Task<(bool Success, string Message, string Token, string PaymentUrl)> RequestPaymentAsync(PaymentRequestModel model);
        Task<(bool Success, string Message, int Status, string RRN, string CardNumberMasked)> VerifyPaymentAsync(PaymentVerifyModel model);
    }
}