using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.View.PaymentMelat
{
    public class SettleRequestDto
    {
        public long TerminalId { get; set; }
     public string Username { get; set; }
     public string Password { get; set; }
     public long OrderId { get; set; }
     public long SaleOrderId { get; set; }
     public long SaleReferenceId { get; set; }
    }
}