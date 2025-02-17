using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.PaymentMelat
{
    public class MelatSettingsDto
    {
    public long? TerminalID { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string CallBackUrl { get; set; }
            public bool IsActive { get; set; } = true; // مقدار پیش‌فرض

    }
}