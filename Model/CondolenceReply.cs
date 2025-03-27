using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace deathSite.Model
{
    public class CondolenceReply
    {
        public int Id { get; set; }
        public string ReplyText { get; set; }
        public DateTime CreatedDate { get; set; }

        // پیام تسلیت مربوطه
        public int CondolenceMessageId { get; set; }
        public CondolenceMessage CondolenceMessage { get; set; }

        // کاربری که پاسخ داده است (صاحب متوفی)
        public int? UserId { get; set; }
        public User User { get; set; }

        public string AuthorName { get; set; } // نام نویسنده (برای کاربران مهمان)
    }

}