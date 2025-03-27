using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace deathSite.Model
{
    public class ShahidUpdateRequest
    {
        public int Id { get; set; }
        
        // ارجاع به شهید مورد نظر
        public int ShahidId { get; set; }
        public Shahid Shahid { get; set; }
        
        // داده‌های پیشنهادی جهت به‌روزرسانی
        public string? Biography { get; set; }
        public string? Memories { get; set; }
        public string? Will { get; set; }
        public List<string> PhotoUrls { get; set; } = new List<string>();
        public List<string> VideoUrls { get; set; } = new List<string>();
        public List<string> VoiceUrls { get; set; } = new List<string>();

        // مشخصات کاربری که درخواست به‌روزرسانی را ارسال کرده است
        public int UserId { get; set; }
        public User User { get; set; }
        
        // وضعیت درخواست: Pending, Approved, Rejected
        public string Status { get; set; } = "Pending";
                
        // تاریخ ثبت درخواست
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}