using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.View.PaymentMelat
{
    public class PaymentRequestDto
    {
        public long OrderId { get; set; }
        public long Amount { get; set; }
        public string PayerId { get; set; }

    }

}