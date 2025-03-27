using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.Dead
{
    public class DeceasedDetailsDto
    {
        public int Id { get; set; } // شناسه منحصربه‌فرد متوفی

        public string FullName { get; set; } // نام کامل متوفی

    }
}