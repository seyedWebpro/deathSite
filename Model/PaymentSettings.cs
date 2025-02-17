using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.Model
{
    public class PaymentSettings
    {
        public int Id { get; set; }
        public string GatewayName { get; set; }  // "Melat" یا "Parsian"
        public string Key { get; set; }          // کلید تنظیمات (مثل "TerminalID", "Username", ...)
        public string Value { get; set; }        // مقدار تنظیمات
        public DateTime LastUpdated { get; set; }
        
    }
}