using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.View
{
    public class LoginView
    {
        [Required(ErrorMessage = "شماره تلفن الزامی است.")]
        [Phone(ErrorMessage = "لطفاً یک شماره تلفن معتبر وارد کنید.")]
        public string phoneNumber { get; set; }

        [Required(ErrorMessage = "رمز عبور الزامی است.")]
        public string password { get; set; }
    }
}