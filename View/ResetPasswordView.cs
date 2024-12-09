using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.View
{
    public class ResetPasswordView
    {
        [Required(ErrorMessage = "شماره تلفن الزامی است.")]
        [Phone(ErrorMessage = "لطفاً یک شماره تلفن معتبر وارد کنید.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = " رمز عبور الزامی است.")]
        public string NewPassword  { get; set; }

        [Required(ErrorMessage = "تکرار رمز عبور الزامی است.")]
        [Compare("NewPassword", ErrorMessage = "رمز عبور و تکرار آن باید یکسان باشند.")]
        public string ConfirmPassword  { get; set; }
    }
}