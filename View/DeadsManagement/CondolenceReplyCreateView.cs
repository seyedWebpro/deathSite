using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.DeadsManagement
{
    public class CondolenceReplyCreateView
{
    public int CondolenceMessageId { get; set; } // پیام تسلیت مربوطه
    public string ReplyText { get; set; } // متن پاسخ
    public int? UserId { get; set; } // `nullable` برای کاربران مهمان
    public string AuthorName { get; set; } // نام نویسنده (برای مهمان‌ها)
}


}