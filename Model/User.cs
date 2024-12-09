using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Model
{
    public class User
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "نام الزامی است.")]
    [StringLength(50, ErrorMessage = "نام نمی‌تواند بیشتر از 50 کاراکتر باشد.")]
    public string firstName { get; set; }
    
    [Required(ErrorMessage = "نام خانوادگی الزامی است.")]
    [StringLength(50, ErrorMessage = "نام خانوادگی نمی‌تواند بیشتر از 50 کاراکتر باشد.")]
    public string lastName { get; set; }
    
    [Required(ErrorMessage = "رمز عبور الزامی است.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر و حداکثر 100 کاراکتر باشد.")]
    public string password { get; set; }
    
    [Required(ErrorMessage = "شماره تلفن الزامی است.")]
    [Phone(ErrorMessage = "لطفاً یک شماره تلفن معتبر وارد کنید.")]
    public string phoneNumber { get; set; }
    
    public string role { get; set; }

    // رابطه با پکیج
    public int? PackageId { get; set; } // می‌تواند null باشد اگر کاربر پکیجی نداشته باشد
    public Package Package { get; set; }
}

}