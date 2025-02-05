using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.View.PaymentMelat
{
    public class PaymentRequestDto
    {
        public long TerminalId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long OrderId { get; set; }
        public long Amount { get; set; }
        public string LocalDate { get; set; }
        public string LocalTime { get; set; }
        public string AdditionalData { get; set; }
        public string CallBackUrl { get; set; }
        public string PayerId { get; set; }

        // خواص جدیدی که باید اضافه شوند
        public string MobileNo { get; set; }
        public string EncPan { get; set; }
        public string PanHiddenMode { get; set; }
        public string CartItem { get; set; }
        public string Enc { get; set; }
    }

}