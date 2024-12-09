using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Model
{
    public class Deads
    {
    public int Id { get; set; }

        [Required(ErrorMessage = "نام و نام خانوادگی الزامی است.")]
        [StringLength(100, ErrorMessage = "نام و نام خانوادگی نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "محل شهادت الزامی است.")]
        [StringLength(200, ErrorMessage = "محل شهادت نمی‌تواند بیشتر از 200 کاراکتر باشد.")]
        public string PlaceOfMartyrdom { get; set; }

        [Required(ErrorMessage = "تاریخ شهادت الزامی است.")]
        public DateTime DateOfMartyrdom { get; set; }

        [Required(ErrorMessage = "تاریخ تولد الزامی است.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "سن الزامی است.")]
        [Range(0, 150, ErrorMessage = "سن باید بین 0 تا 150 سال باشد.")]
        public int Age { get; set; }
        
        [Required(ErrorMessage = "معرفی الزامی است.")]
        [StringLength(700, ErrorMessage = "معرفی نمی‌تواند بیشتر از 700 کاراکتر باشد.")]
        public string Introduction { get; set; } // معرفی

        [Required(ErrorMessage = "وصیت نامه الزامی است.")]
        [StringLength(1500, ErrorMessage = "وصیت نامه نمی‌تواند بیشتر از 1500 کاراکتر باشد.")]
        public string Will { get; set; } // وصیت نامه
    }
}