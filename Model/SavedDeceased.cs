using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace deathSite.Model
{
    public class SavedDeceased
    {
        // کلید ترکیبی: شناسه کاربر و شناسه متوفی
        public int UserId { get; set; }
        public User User { get; set; }

        public int DeceasedId { get; set; }
        public Deceased Deceased { get; set; }

        // تاریخ ذخیره متوفی
        public DateTime SavedAt { get; set; } = DateTime.Now;
    }
}