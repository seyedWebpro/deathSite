using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Model.AdminModel
{
    public class Tag
    {
        public int Id { get; set;}
        [Required(ErrorMessage = "نام برچسب الزامی است.")]
    [StringLength(30, ErrorMessage = "نام برچسب نمی‌تواند بیشتر از 30 کاراکتر باشد.")]
    public string Name { get; set; }
    }
}