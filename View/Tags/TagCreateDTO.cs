using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.View.Tags
{
    public class TagCreateDTO
    {
        [Required(ErrorMessage = "عنوان برچسب الزامی است.")]
        [StringLength(100, ErrorMessage = "عنوان برچسب نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "نوع برچسب الزامی است.")]
        [StringLength(50, ErrorMessage = "نوع برچسب نمی‌تواند بیشتر از 50 کاراکتر باشد.")]
        public string Type { get; set; }

        [StringLength(500, ErrorMessage = "توضیحات برچسب نمی‌تواند بیشتر از 500 کاراکتر باشد.")]
        public string Description { get; set; }
    }
}