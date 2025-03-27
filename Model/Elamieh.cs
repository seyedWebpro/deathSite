using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace deathSite.Model
{
    public class Elamieh
    {
        public int Id { get; set; }  // شناسه اعلامیه
        public List<string> PhotoUrls { get; set; } = new List<string>(); // لیست تصاویر
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // تاریخ ثبت
        public int DeceasedId { get; set; } // شناسه متوفی
        public Deceased Deceased { get; set; } // ارتباط با متوفی
        public int UserId { get; set; } // شناسه کاربری که اعلامیه را ثبت کرده است
        public User User { get; set; } // ارتباط با کاربر ثبت‌کننده
    }
}