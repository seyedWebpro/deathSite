using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.PaymentParsian
{
    public class ParsianSettingsDto
    {
        public string PIN { get; set; }

        public string CallBackUrl { get; set; }
        public bool IsActive { get; set; } = true; // مقدار پیش‌فرض

    }
}