using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace deathSite.Model
{
    public class PaymentInvoice
    {
        public int Id { get; set; }

        // نام کاربر (یا شناسه کاربر)
        public string UserName { get; set; }

        // تاریخ تراکنش
        public DateTime TransactionDate { get; set; }

        // مبلغ تراکنش
        public decimal Amount { get; set; }

        // وضعیت تراکنش (مثلاً "Success" یا "Failed")
        public string Status { get; set; }

        // نوع تراکنش (مثلاً "Register" یا "Update")
        public string TransactionType { get; set; }

        // شماره پیگیری تراکنش (مثلاً RefId یا Token دریافتی از درگاه)
        public string TrackingNumber { get; set; }

        // درگاه پرداخت استفاده شده (مانند "Melat" یا "Parsian")
        public string PaymentGateway { get; set; }

        // توضیحات اضافی (اختیاری)
        public string Description { get; set; }

         // کلید خارجی به کاربر
        public int UserId { get; set; }

        // پراپرتی ناوبری به مدل User
        public virtual User User { get; set; }
    }
}