using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.View.PaymentMelat;

namespace deathSite.Services.Payment
{
    public interface IMelatPaymentService
    {
        Task<(bool Success, string Message, string RefId)> PayRequestAsync(PaymentRequestDto request);
        Task<(bool Success, string Message)> VerifyRequestAsync(VerifyRequestDto request);
        Task<(bool Success, string Message)> SettleRequestAsync(SettleRequestDto request);
    }
}