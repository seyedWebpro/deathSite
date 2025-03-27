using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace deathSite.Model
{
    public class Factors
    {
        public int Id { get; set; }

        public int? DeceasedId { get; set; } // اضافه شده برای ارتباط با متوفی

        // تاریخ تراکنش
        public DateTime TransactionDate { get; set; }

        // تاریخ پرداخت تراکنش
        public DateTime? PaidAt { get; set; }

        // مبلغ تراکنش
        public decimal Amount { get; set; }

        // وضعیت تراکنش (مثلاً "Success" یا "Failed")
        public string Status { get; set; }

        // نوع تراکنش (مثلاً "Register" یا "Update")
        public string TransactionType { get; set; }

        // شماره پیگیری تراکنش (مثلاً RefId یا Token دریافتی از درگاه)
        public string? TrackingNumber { get; set; }

        // درگاه پرداخت استفاده شده (مانند "Melat" یا "Parsian")
        public string PaymentGateway { get; set; }

        // توضیحات اضافی (اختیاری)
        public string? Description { get; set; }

        // شناسه سفارش (در صورت نیاز به ردیابی تراکنش با سفارش خاص)
        public long OrderId { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }

        public int PackageId { get; set; }
        public Package Package { get; set; }

        // رابطه با DeceasedPackage
        public List<DeceasedPackage> DeceasedPackages { get; set; } = new List<DeceasedPackage>();
    }
}