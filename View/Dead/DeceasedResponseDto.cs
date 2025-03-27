using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.Dead
{
public class DeceasedResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Gender { get; set; }
    public DateTime DateOfMartyrdom { get; set; }
    public DateTime PublishedDate { get; set; }
    public DateTime BirthDate { get; set; }
    public string IsApproved { get; set; } // تغییر به رشته برای خوانایی بهتر در فرانت‌اند
    public string QRCodeUrl { get; set; }
}

}