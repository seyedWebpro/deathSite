using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.PaymentMelat
{
    public class ExtendedPaymentRequestDto
    {
        // فیلدهای مربوط به درگاه
        // public long OrderId { get; set; }
        public long Amount { get; set; }
        // public string PayerId { get; set; }
        
        // فیلدهای اضافی برای منطق داخلی (برای ثبت پکیج)
        public int? UserId { get; set; }
         public int? DeceasedId { get; set; }   
        public int? PackageId { get; set; }
    }   
}