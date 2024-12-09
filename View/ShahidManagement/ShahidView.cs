using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api.View.ShahidManagement
{
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

        [StringLength(500, ErrorMessage = "مسئولیت ها نمی‌تواند بیشتر از 500 کاراکتر باشد.")]
        public string Responsibilities { get; set; }

        [StringLength(500, ErrorMessage = "عملیات ها نمی‌تواند بیشتر از 500 کاراکتر باشد.")]
        public string Operations { get; set; }

        [StringLength(2000, ErrorMessage = "زندگی نامه نمی‌تواند بیشتر از 2000 کاراکتر باشد.")]
        public string Biography { get; set; }

        [StringLength(2000, ErrorMessage = "وصیت نامه نمی‌تواند بیشتر از 2000 کاراکتر باشد.")]
        public string Will { get; set; }

        // فیلدهای URL که در پاسخ JSON نمایش داده نمی‌شوند
    [JsonIgnore]
    public List<string> PhotoUrls { get; set; } = new List<string>();
    
    [JsonIgnore]
    public List<string> VideoUrls { get; set; } = new List<string>();
    
    [JsonIgnore]
    public List<string> VoiceUrls { get; set; } = new List<string>();

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

        [StringLength(1000, ErrorMessage = "شعر نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
        public string Poem { get; set; } // شعر

        [StringLength(1000, ErrorMessage = "خاطرات نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
        public string Memories { get; set; } // خاطرات
    }
}
