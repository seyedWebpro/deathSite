using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.Factors
{
    public class FactorsFilterDto
    {
        public DateTime? FromDate { get; set; }
        
        public DateTime? ToDate { get; set; }
        
        public int? UserId { get; set; }
        
        public string? Status { get; set; }
        
        public string? PaymentGateway { get; set; }
        
        public string? TransactionType { get; set; }
    }
}