using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.View
{
    public class SendSmsView
    {
        [Required(ErrorMessage = "شماره تلفن الزامی است.")]
        [Phone(ErrorMessage = "لطفاً یک شماره تلفن معتبر وارد کنید.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = " متن پیام الزامی است.")]
        public string Message { get; set; }
    }
}