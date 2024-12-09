using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    public string Responsibilities { get; set; } // مسئولیت ها
    public string Operations { get; set; } // عملیات ها
    public string Biography { get; set; } // زندگی نامه
    public string Will { get; set; } // وصیت نامه
    public string Memories { get; set; } //  خاطرات
   public List<string> PhotoUrls { get; set; } = new List<string>(); // لیست عکس‌ها
    public List<string> VideoUrls { get; set; } = new List<string>(); // لیست ویدیوها
    public List<string> VoiceUrls { get; set; } = new List<string>(); // لیست ویس‌ها
    public string CauseOfMartyrdom { get; set; } // علت شهادت
    public string LastResponsibility { get; set; } // آخرین مسئولیت
    public string Gorooh { get; set; } // گروه شهدا
    public string Yegan { get; set; } // یگان
    public string Niru { get; set; } // نیرو
    public string Ghesmat { get; set; } // قشر
    public string Poem { get; set; } // شعر
    }
}