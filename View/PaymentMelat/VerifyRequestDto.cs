using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.View.PaymentMelat
{
    public class VerifyRequestDto
    {
     public long OrderId { get; set; }
     public long SaleOrderId { get; set; }
     public long SaleReferenceId { get; set; }
    }
}