using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.Packages
{
    public class UserPackageDto
{
    [Display(Name = "شناسه پکیج")]
    public int PackageId { get; set; }

    [Display(Name = "نام پکیج")]
    public string PackageName { get; set; }

    [Display(Name = "قیمت پکیج")]
    public decimal Price { get; set; }

    [Display(Name = "تاریخ خرید")]
    public DateTime PurchaseDate { get; set; }

    [Display(Name = "تاریخ انقضا")]
    public DateTime ExpiryDate { get; set; }

    [Display(Name = "وضعیت")]
    public string Status { get; set; }

    [Display(Name = "تعداد تصاویر استفاده‌شده")]
    public int UsedImageCount { get; set; }

    [Display(Name = "تعداد ویدیوهای استفاده‌شده")]
    public int UsedVideoCount { get; set; }

    [Display(Name = "تعداد اعلان‌های استفاده‌شده")]
    public int UsedNotificationCount { get; set; }

    [Display(Name = "تعداد فایل‌های صوتی استفاده‌شده")]
    public int UsedAudioFileCount { get; set; }
}
}