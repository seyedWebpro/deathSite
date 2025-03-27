using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace deathSite.View.Dead
{
    public class UpdateDeceasedDto
    {
        public string? FatherName { get; set; }
        public string? Job { get; set; }
        public string? Tahsilat { get; set; }
        public List<string>? ChildrenNames { get; set; } = new List<string>();
        public string? HowDeath { get; set; }

        public string? OstanMazar { get; set; }
        public string? CityMazar { get; set; }
        public string? Aramestan { get; set; }
        public string? GhesteMazar { get; set; }
        public string? RadifMazar { get; set; }
        public string? NumberMazar { get; set; }

        // فیلدهای جدید برای متوفی
        public string? PlaceOfMartyrdom { get; set; } // محل رحلت

        [DataType(DataType.Date)] // تأکید بر دریافت فقط تاریخ
        public DateTime? DateOfMartyrdom { get; set; } // تاریخ وفات

        [DataType(DataType.Date)] // تأکید بر دریافت فقط تاریخ
        public DateTime? DateOfBirth { get; set; } // تاریخ تولد
        public int? Age { get; set; } // سن
        public string? Description { get; set; } // مشروح زندگینامه
        public string? Khaterat { get; set; } // خاطرات
        public string? PlaceBirth { get; set; }

        // فیلد جدید برای سربرگ
        public SarbargDto? Sarbarg { get; set; }

    }


}