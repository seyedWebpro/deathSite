using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.PaymentMelat
{
   public class CallbackRequestDto
{
    public string RefId { get; set; }
    public string ResCode { get; set; }
    public long OrderId { get; set; }
    public long SaleOrderId { get; set; }
    public long SaleReferenceId { get; set; }
}


}