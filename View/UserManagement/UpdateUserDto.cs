using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.View.UserManagement
{
    // DTO برای به‌روزرسانی کاربر
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "نام الزامی است.")]
        [StringLength(50, ErrorMessage = "نام نمی‌تواند بیشتر از 50 کاراکتر باشد.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی الزامی است.")]
        [StringLength(50, ErrorMessage = "نام خانوادگی نمی‌تواند بیشتر از 50 کاراکتر باشد.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "شماره تلفن الزامی است.")]
        [Phone(ErrorMessage = "لطفاً یک شماره تلفن معتبر وارد کنید.")]
        public string PhoneNumber { get; set; }

        public string Role { get; set; }

        // رمز عبور جدید (اختیاری)
        public string Password { get; set; }
    }
}