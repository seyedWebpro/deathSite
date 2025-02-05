using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.View.News
{
    public class NewsDto
    {
        [Required(ErrorMessage = "عنوان الزامی است.")]
        [StringLength(100, ErrorMessage = "عنوان نباید بیش از ۱۰۰ کاراکتر باشد.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "محتوا الزامی است.")]
        public string Content { get; set; }
    }
}