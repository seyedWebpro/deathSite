using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.PaymentParsian
{
    public class PaymentVerificationResult
    {
    public int Status { get; set; }
    public string RRN { get; set; }
    public string CardNumberMasked { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    }
}