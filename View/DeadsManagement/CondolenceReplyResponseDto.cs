using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.DeadsManagement
{
    public class CondolenceReplyResponseDto
{
    public int Id { get; set; }
    public string AuthorName { get; set; }
    public string ReplyText { get; set; }
    public DateTime CreatedDate { get; set; }
}

}