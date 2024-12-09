using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.View
{
    public class VerifyCodeView
    {
        [Required(ErrorMessage = "شماره تلفن الزامی است.")]

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = " وارد کردن کد الزامی است.")]

        public string Code { get; set; }
    }
}