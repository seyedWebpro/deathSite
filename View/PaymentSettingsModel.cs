using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View
{
    public class PaymentSettingsModel
    {
        // Melat Settings
    public long MelatTerminalID { get; set; }
    public string MelatUsername { get; set; }
    public string MelatPassword { get; set; }
    public string MelatCallbackUrl { get; set; }

    // Parsian Settings
    public string ParsianPIN { get; set; }
    public string ParsianCallbackUrl { get; set; }
    }
}