using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.PaymentParsian
{
    public class ExtendedPaymentParisinaRequestDto
    {
        public long Amount { get; set; }
        // public long OrderId { get; set; }
        public string Description { get; set; }

        // فیلدهای اضافی برای منطق داخلی (برای ثبت پکیج)
        public int? UserId { get; set; }
        public int? PackageId { get; set; }
    }
}