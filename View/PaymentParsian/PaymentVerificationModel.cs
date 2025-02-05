using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.View.PaymentParsian
{
    public class PaymentVerificationModel
    {
        public long Token { get; set; }
        public string OrderId { get; set; }
        public int Status { get; set; }
        public string RRN { get; set; }
    }
}