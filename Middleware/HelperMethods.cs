using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Middleware
{
    public static class HelperMethods
    {
        public static IActionResult HandleValidationErrors(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return new BadRequestObjectResult(new
                {
                    StatusCode = 400,
                    Message = "پر کردن فیلد ها الزامی است",
                    errors = modelState
                });
            }
            return null; // یا می‌توانید یک نتیجه موفقیت‌آمیز برگردانید
        }

        
    }
}