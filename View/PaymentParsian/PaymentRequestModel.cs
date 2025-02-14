using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.View.PaymentParsian
{
    public class PaymentRequestModel
    {
        public long Amount { get; set; }
        public long OrderId { get; set; }
        public string Description { get; set; }
    }

}