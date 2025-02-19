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

        public int UserId { get; set; }

        public int? UserPackageId { get; set; }  // می‌تواند null باشد برای پرداخت‌های غیر مرتبط با پکیج
        public int? PackageId { get; set; }  // فیلد جدید برای ذخیره شناسه پکیج

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

        public User User { get; set; }
        public UserPackage? UserPackage { get; set; }  // رابطه با پکیج کاربر (اختیاری)
    }
}