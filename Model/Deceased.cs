using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api.Model.AdminModel;
using deathSite.Model;

namespace api.Model
{
    public class Deceased
    {
        public int Id { get; set; }
        public string FullName { get; set; } // نام و نام خانوادگی
        public string? PlaceOfMartyrdom { get; set; } // محل رحلت
        public DateTime? DateOfMartyrdom { get; set; } // تاریخ وفات
        public DateTime? DateOfBirth { get; set; } // تاریخ تولد
        public string? Gender { get; set; } // جنسیت
        public string? Description { get; set; } // مشروح زندگینامه
        public string? Khaterat { get; set; } // خاطرات

        public List<string>? PhotoUrls { get; set; } = new List<string>(); // تصاویر
        public List<string>? VideoUrls { get; set; } = new List<string>(); // ویدیوها
        public List<string>? VoiceUrls { get; set; } = new List<string>(); // صداها
        public DateTime PublishedDate { get; set; }
        public DateTime BirthDate { get; set; }
        public string? PlaceBirth { get; set; }

        [JsonIgnore]
        public List<CondolenceMessage> CondolenceMessages { get; set; } = new List<CondolenceMessage>();

        // اطلاعات جدید که اضافه شدند
        public string? FatherName { get; set; } // نام پدر
        public string? Job { get; set; } // شغل
        public string? Tahsilat { get; set; } // تحصیلات
        public List<string>? ChildrenNames { get; set; } = new List<string>(); // اسامی فرزندان
        public string? HowDeath { get; set; } // نحوه وفات

        // اطلاعات مکان دفن
        public string? OstanMazar { get; set; } // استان محل مزار
        public string? CityMazar { get; set; } // شهر محل مزار
        public string? Aramestan { get; set; } // آرامستان
        public string? GhesteMazar { get; set; } // قطعه مزار
        public string? RadifMazar { get; set; } // ردیف مزار
        public string? NumberMazar { get; set; } // شماره مزار
        public string? CoverPhotoUrl { get; set; }
        public string? Ghaleb { get; set; } = "first";

        public ApprovalStatus IsApproved { get; set; }

        // رابطه یک به یک با DeceasedLocation (همچنان حفظ شده)
        public DeceasedLocation? DeceasedLocation { get; set; }

        // کاربرانی که این متوفی را ذخیره کرده‌اند (رابطه چند به چند)
        public ICollection<SavedDeceased> SavedByUsers { get; set; } = new List<SavedDeceased>();

        // کاربرانی که این متوفی را لایک کرده‌اند
        public ICollection<LikeDeceased> LikedByUsers { get; set; } = new List<LikeDeceased>();

        public ICollection<DeathViewCount> DeathViews { get; set; } = new List<DeathViewCount>();

        public ICollection<Elamieh> Elamiehs { get; set; } = new List<Elamieh>();

        // رابطه اختیاری با سربرگ (Sarbarg)
        public int? SarbargId { get; set; }

        public string? QRCodeUrl { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        // جایگزین با کالکشن پکیج‌ها
        public ICollection<DeceasedPackage> Packages { get; set; } = new List<DeceasedPackage>();

        public Sarbarg Sarbarg { get; set; }
    }

    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
