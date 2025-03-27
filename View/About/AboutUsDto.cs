using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.About
{
   public class AboutUsDto
{
    public string? Title { get; set; }  // عنوان (اختیاری)

    public string? Description { get; set; }  // توضیحات (اختیاری)

    public int? Poshtiban { get; set; }  // آیا پشتیبان دارد؟ (اختیاری)

    public int? Sabeghe { get; set; }  // سابقه (اختیاری)

    public int? ShahidSabt { get; set; }  // تعداد شهدا ثبت شده (اختیاری)
}

}