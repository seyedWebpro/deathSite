using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.View.PaymentParsian
{
    public class PaymentResponseModel
{
    public bool IsSuccessful { get; set; }
    public string PaymentUrl { get; set; }
    public string Token { get; set; }
    public string ErrorMessage { get; set; }
}

}