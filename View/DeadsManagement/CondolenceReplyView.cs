using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.DeadsManagement
{
    public class CondolenceReplyView
{
    public int Id { get; set; } // شناسه ریپلای
    public string AuthorName { get; set; } // نام نویسنده ریپلای
    public string ReplyText { get; set; } // متن ریپلای
    public DateTime CreatedDate { get; set; } // تاریخ ایجاد ریپلای
}

}