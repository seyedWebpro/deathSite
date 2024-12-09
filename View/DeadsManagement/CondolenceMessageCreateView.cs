using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.View.DeadsManagement
{
    public class CondolenceMessageCreateView
    {
        
    [Required(ErrorMessage = "نام و نام خانوادگی الزامی است.")]
    public string AuthorName { get; set; }

    [Required(ErrorMessage = "شماره موبایل الزامی است.")]
    [Phone(ErrorMessage = "شماره موبایل معتبر نیست.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "نام متوفی الزامی است.")]
    public string DeceasedName { get; set; }

    [Required(ErrorMessage = "متن پیام الزامی است.")]
    public string MessageText { get; set; }
    }
}