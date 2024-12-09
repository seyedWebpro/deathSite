using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.View
{
    public class registerView
    {
        [Required(ErrorMessage = "نام الزامی است.")]
        [StringLength(50, ErrorMessage = "نام نمی‌تواند بیشتر از 50 کاراکتر باشد.")]
        public string firstName { get; set; }
        [Required(ErrorMessage = "نام خانوادگی الزامی است.")]
        [StringLength(50, ErrorMessage = "نام خانوادگی نمی‌تواند بیشتر از 50 کاراکتر باشد.")]

        public string lastName { get; set; }
        [Required(ErrorMessage = "رمز عبور الزامی است.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر و حداکثر 100 کاراکتر باشد.")]


        public string password { get; set; }

        [Required(ErrorMessage = "تأیید رمز عبور الزامی است.")]
        [Compare("password", ErrorMessage = "رمز عبور و تأیید رمز عبور باید مطابقت داشته باشند.")]
        public string confirmPassword { get; set; }

        [Required(ErrorMessage = "شماره تلفن الزامی است.")]
        [Phone(ErrorMessage = "لطفاً یک شماره تلفن معتبر وارد کنید.")]
        public string phoneNumber { get; set; }
    }
}