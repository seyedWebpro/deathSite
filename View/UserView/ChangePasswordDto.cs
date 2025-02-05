using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.View.UserView
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "لطفاً رمز عبور فعلی را وارد کنید.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "لطفاً رمز عبور جدید را وارد کنید.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور جدید باید حداقل ۶ کاراکتر باشد.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "لطفاً تکرار رمز عبور جدید را وارد کنید.")]
        [Compare("NewPassword", ErrorMessage = "رمز عبور جدید و تکرار آن یکسان نیست.")]
        public string ConfirmNewPassword { get; set; }
    }
}