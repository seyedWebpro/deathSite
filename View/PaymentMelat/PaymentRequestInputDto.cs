using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.PaymentMelat
{
     public class PaymentRequestInputDto
    {
        public long OrderId { get; set; }
        public long Amount { get; set; }
        public string AdditionalData { get; set; }
        public string PayerId { get; set; }
        public string MobileNo { get; set; }
        public string EncPan { get; set; }
        public string PanHiddenMode { get; set; }
        public string CartItem { get; set; }
        public string Enc { get; set; }
    }

}