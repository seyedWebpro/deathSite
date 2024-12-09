using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Model
{
    public class Package
{
    public int Id { get; set; }

    [Required(ErrorMessage = "نام پکیج الزامی است.")]
    [StringLength(100, ErrorMessage = "نام پکیج نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "مدت زمان الزامی است.")]
    public string Duration { get; set; }

    [Required(ErrorMessage = "قیمت الزامی است.")]
    [Range(0, double.MaxValue, ErrorMessage = "قیمت باید یک عدد مثبت باشد.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "مبلغ تمدید دوره ای الزامی است.")]
    [Range(0, double.MaxValue, ErrorMessage = "مبلغ تمدید باید یک عدد مثبت باشد.")]
    public decimal RenewalFee { get; set; } // مبلغ تمدید دوره ای

    [Required(ErrorMessage = "مدت زمان اعتبار به روز الزامی است.")]
    public string ValidityPeriod { get; set; } // مدت زمان اعتبار به روز

    [Range(0, int.MaxValue, ErrorMessage = "تعداد تصاویر باید یک عدد غیر منفی باشد.")]
    public int ImageCount { get; set; } // تعداد تصاویر

    [Range(0, int.MaxValue, ErrorMessage = "تعداد ویدیو باید یک عدد غیر منفی باشد.")]
    public int VideoCount { get; set; } // تعداد ویدیو

    [Range(0, int.MaxValue, ErrorMessage = "تعداد اعلامیه باید یک عدد غیر منفی باشد.")]
    public int NotificationCount { get; set; } // تعداد اعلامیه

    [Range(0, int.MaxValue, ErrorMessage = "تعداد مجاز فایل صوتی باید یک عدد غیر منفی باشد.")]
    public int AudioFileLimit { get; set; } // تعداد مجاز فایل صوتی افراد

    public bool BarcodeEnabled { get; set; } // امکان ایجاد بارکد
    public bool DisplayEnabled { get; set; } // امکان نمایش محتوا
    public bool TemplateSelectionEnabled { get; set; } // قابلیت انتخاب قالب
    public bool CondolenceMessageEnabled { get; set; } // قابلیت قراردادن پیام تسلیت
    public bool VisitorContentSubmissionEnabled { get; set; } // قابلیت ارسال محتوا از سوی بازدید کننده
    public bool LocationAndNavigationEnabled { get; set; } // قابلیت لوکیشن و مسیریابی
    public bool SharingEnabled { get; set; } // قابلیت به اشتراک گذاری
    public bool File360DegreeEnabled { get; set; } // مجوز قراردادن فایل 360 درجه
    public bool UpdateCapabilityEnabled { get; set; } // قابلیت به روز رسانی

    public ICollection<User> Users { get; set; } // برای مدیریت کاربران که این پکیج را دارند
}

}