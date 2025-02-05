using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.View
{
    public class ContactMeFormDto
{
    [Required(ErrorMessage = "نام و نام خانوادگی الزامی است.")]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = "ایمیل الزامی است.")]
    [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "شماره تلفن الزامی است.")]
    [Phone(ErrorMessage = "شماره تلفن معتبر نیست.")]
    public string PhoneNumber { get; set; } = "";

    [Required(ErrorMessage = "موضوع الزامی است.")]
    public string Subject { get; set; } = "";

    [Required(ErrorMessage = "متن پیام الزامی است.")]
    public string Message { get; set; } = "";
}


}