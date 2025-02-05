using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Model.AdminModel
{
     public class Banner
    {
        // شناسه یکتا برای هر بنر
        public int Id { get; set; }

        // مسیر تصویر برای سایز موبایل
        public string? MobileImagePath { get; set; }

        // مسیر تصویر برای سایز دسکتاپ
        public string? DesktopImagePath { get; set; }

        // تاریخ انتشار بنر
        public DateTime PublishedDate { get; set; }
    }
}