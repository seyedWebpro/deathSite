using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ShahidView
{
    [Required(ErrorMessage = "شناسه الزامی است.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "نام کامل الزامی است.")]
    [StringLength(100, ErrorMessage = "نام کامل نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "نام پدر الزامی است.")]
    [StringLength(100, ErrorMessage = "نام پدر نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string FatherName { get; set; }

    [Required(ErrorMessage = "تاریخ تولد الزامی است.")]
    public DateOnly BirthBorn { get; set; }

    [Required(ErrorMessage = "تاریخ شهادت الزامی است.")]
    public DateOnly BirthDead { get; set; }

    [Required(ErrorMessage = "محل شهادت الزامی است.")]
    [StringLength(200, ErrorMessage = "محل شهادت نمی‌تواند بیشتر از 200 کاراکتر باشد.")]
    public string PlaceDead { get; set; }

    [Required(ErrorMessage = "ادرس مزار الزامی است.")]
    [StringLength(200, ErrorMessage = "ادرس محل مزار نمی‌تواند بیشتر از 200 کاراکتر باشد.")]

    public string BurialSiteLink { get; set; }

        [Required(ErrorMessage = "ادرس رسانه الزامی است.")]
    [StringLength(200, ErrorMessage = "ادرس رسانه نمی‌تواند بیشتر از 200 کاراکتر باشد.")]
    public string MediaLink { get; set; } // لینک رسانه

        [Required(ErrorMessage = "ادرس محل شهادت الزامی است.")]
    [StringLength(200, ErrorMessage = "ادرس محل شهادت نمی‌تواند بیشتر از 200 کاراکتر باشد.")]
    public string DeadPlaceLink { get; set; } // لینک آدرس محل شهدات

        [Required(ErrorMessage = "لینک زیارت مجازی الزامی است.")]
    [StringLength(200, ErrorMessage = " لینک زیارت مجازی نمی‌تواند بیشتر از 200 کاراکتر باشد.")]
    public string virtualLink { get; set; } // لینک زیارت مجازی


    [Required(ErrorMessage = "محل دفن الزامی است.")]
    [StringLength(200, ErrorMessage = "محل دفن نمی‌تواند بیشتر از 200 کاراکتر باشد.")]
    public string PlaceOfBurial { get; set; } // اضافه کردن محل دفن

    [Required(ErrorMessage = "مسئولیت ها الزامی است.")]
    public List<string> Responsibilities { get; set; } = new List<string>(); // مسئولیت ها

    [Required(ErrorMessage = "عملیات ها الزامی است.")]
    public List<string> Operations { get; set; } = new List<string>(); // عملیات ها

    [StringLength(2000, ErrorMessage = "زندگی نامه نمی‌تواند بیشتر از 2000 کاراکتر باشد.")]
    public string Biography { get; set; } // زندگی نامه

    [StringLength(2000, ErrorMessage = "وصیت نامه نمی‌تواند بیشتر از 2000 کاراکتر باشد.")]
    public string Will { get; set; } // وصیت نامه

    [JsonIgnore]
    public List<string> PhotoUrls { get; set; } = new List<string>(); // لیست عکس‌ها
    
    [JsonIgnore]
    public List<string> VideoUrls { get; set; } = new List<string>(); // لیست ویدیوها
    
    [JsonIgnore]
    public List<string> VoiceUrls { get; set; } = new List<string>(); // لیست ویس‌ها

    [StringLength(500, ErrorMessage = "علت شهادت نمی‌تواند بیشتر از 500 کاراکتر باشد.")]
    public string CauseOfMartyrdom { get; set; } // علت شهادت

    [StringLength(500, ErrorMessage = "آخرین مسئولیت نمی‌تواند بیشتر از 500 کاراکتر باشد.")]
    public string LastResponsibility { get; set; } // آخرین مسئولیت

    [StringLength(200, ErrorMessage = "گروه شهدا نمی‌تواند بیشتر از 200 کاراکتر باشد.")]
    public string Gorooh { get; set; } // گروه شهدا

    [StringLength(200, ErrorMessage = "یگان نمی‌تواند بیشتر از 200 کاراکتر باشد.")]
    public string Yegan { get; set; } // یگان

    [StringLength(200, ErrorMessage = "نیرو نمی‌تواند بیشتر از 200 کاراکتر باشد.")]
    public string Niru { get; set; } // نیرو

    [StringLength(200, ErrorMessage = "قشر نمی‌تواند بیشتر از 200 کاراکتر باشد.")]
    public string Ghesmat { get; set; } // قشر

    [StringLength(1000, ErrorMessage = "بیت اول شعر نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string PoemVerseOne { get; set; } // بیت اول شعر

    [StringLength(1000, ErrorMessage = "بیت دوم شعر نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string PoemVerseTwo { get; set; } // بیت دوم شعر

    [StringLength(1000, ErrorMessage = "خاطرات نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string Memories { get; set; } // خاطرات
}
