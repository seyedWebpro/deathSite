using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using deathSite.Model;

namespace api.Model
{
    public class Shahid
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public DateOnly BirthBorn { get; set; }
        public DateOnly BirthDead { get; set; }
        public string PlaceDead { get; set; }
        public string PlaceOfBurial { get; set; } // اضافه کردن محل دفن
        public string BurialSiteLink { get; set; } // لینک آدرس مزار
        public string MediaLink { get; set; } // لینک رسانه
        public string DeadPlaceLink { get; set; } // لینک آدرس محل شهدات
        public string virtualLink { get; set; } // لینک زیارت مجازی
        public List<string> Responsibilities { get; set; } = new List<string>(); // مسئولیت ها
        public List<string> Operations { get; set; } = new List<string>(); // عملیات ها
        public string Biography { get; set; } // زندگی نامه
        public string Will { get; set; } // وصیت نامه
        public string Memories { get; set; } // خاطرات
        public List<string> PhotoUrls { get; set; } = new List<string>(); // لیست عکس‌ها
        public List<string> VideoUrls { get; set; } = new List<string>(); // لیست ویدیوها
        public List<string> VoiceUrls { get; set; } = new List<string>(); // لیست ویس‌ها
        public string CauseOfMartyrdom { get; set; } // علت شهادت
        public string LastResponsibility { get; set; } // آخرین مسئولیت
        public string Gorooh { get; set; } // گروه شهدا
        public string Yegan { get; set; } // یگان
        public string Niru { get; set; } // نیرو
        public string Ghesmat { get; set; } // قشر
        public string PoemVerseOne { get; set; } // بیت اول شعر
        public string PoemVerseTwo { get; set; } // بیت دوم شعر

        // شناسه کاربر
        public int UserId { get; set; }
        public User User { get; set; }

        // وضعیت تایید شدن (توسط ادمین)
        public string Approved { get; set; } = "Pending"; // مقدار پیش‌فرض: Pending
        public string? RejectionReason { get; set; } // دلیل رد شدن (در صورت وجود)


        // رابطه Many-to-Many
        public List<ShahidTag> ShahidTags { get; set; } = new List<ShahidTag>();

        public ICollection<ShahidViewCount> ShahidViews { get; set; } = new List<ShahidViewCount>();

         // رابطه یک به چند با درخواست‌های به‌روزرسانی (اختیاری)
    public ICollection<ShahidUpdateRequest> UpdateRequests { get; set; } = new List<ShahidUpdateRequest>();
    }


}