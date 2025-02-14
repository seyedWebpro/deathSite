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

    public string Name { get; set; }

    public string Duration { get; set; }

    public decimal Price { get; set; }

    public decimal RenewalFee { get; set; } // مبلغ تمدید دوره ای

    public string ValidityPeriod { get; set; } // مدت زمان اعتبار به روز

    public int ImageCount { get; set; } // تعداد تصاویر

    public int VideoCount { get; set; } // تعداد ویدیو

    public int NotificationCount { get; set; } // تعداد اعلامیه

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