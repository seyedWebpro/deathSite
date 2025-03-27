using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.Model.AdminModel
{
    public class AboutUs
    {
        public int Id { get; set; }

    [MaxLength(550)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; } // آدرس عکس

    public int? Poshtiban { get; set; } // آیا پشتیبان دارد؟

    public int? Sabeghe { get; set; } // سابقه (به سال)

    public int? ShahidSabt { get; set; } // تعداد شهدا ثبت شده
    }
}