using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.Factors
{
    public class FactorsCreateDto
    {
                [Required(ErrorMessage = "شناسه کاربر الزامی است")]
        public int UserId { get; set; }
        
        public int? UserPackageId { get; set; }
        
        public string? UserName { get; set; }
        
        [Required(ErrorMessage = "تاریخ تراکنش الزامی است")]
        public DateTime TransactionDate { get; set; }
        
        [Required(ErrorMessage = "مبلغ تراکنش الزامی است")]
        public decimal Amount { get; set; }
        
        [Required(ErrorMessage = "وضعیت تراکنش الزامی است")]
        public string Status { get; set; }
        
        [Required(ErrorMessage = "نوع تراکنش الزامی است")]
        public string TransactionType { get; set; }
        
        [Required(ErrorMessage = "شماره پیگیری الزامی است")]
        public string TrackingNumber { get; set; }
        
        [Required(ErrorMessage = "درگاه پرداخت الزامی است")]
        public string PaymentGateway { get; set; }
        
        public string Description { get; set; }
        
        [Required(ErrorMessage = "شناسه سفارش الزامی است")]
        public long OrderId { get; set; }
    }
}