using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.Factors
{
      public class FactorsViewDto
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int? UserPackageId { get; set; }
        
        public string? UserName { get; set; } // اضافه کردن نام کاربر
        
        public DateTime TransactionDate { get; set; }
        
        public decimal Amount { get; set; }
        
        public string Status { get; set; }
        
        public string TransactionType { get; set; }
        
        public string TrackingNumber { get; set; }
        
        public string PaymentGateway { get; set; }
        
        public string Description { get; set; }
        
        public long OrderId { get; set; }

        // تغییرات جدید برای پشتیبانی از چند متوفی
        public List<int>? DeceasedIds { get; set; } = new List<int>();

        public List<string>? DeceasedNames { get; set; } = new List<string>();
    }
}