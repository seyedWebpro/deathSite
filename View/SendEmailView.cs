using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.View
{
    public class SendEmailView
    {
        [Required(ErrorMessage = "The 'To' field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string To { get; set; }

        [Required(ErrorMessage = "The 'Subject' field is required.")]
        [StringLength(100, ErrorMessage = "The subject cannot be longer than 100 characters.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "The 'Body' field is required.")]
        [StringLength(5000, ErrorMessage = "The body cannot be longer than 5000 characters.")]
        public string Body { get; set; }
    }
}