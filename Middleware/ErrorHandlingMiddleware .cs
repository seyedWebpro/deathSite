using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace api.Middleware
{
    public class ErrorHandlingMiddleware
    {
            private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // ادامه پردازش درخواست
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex); // مدیریت خطا
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        // تنظیم وضعیت پاسخ
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // ایجاد شیء خطا
        var errorResponse = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "خطای داخلی سرور.",
            Error = ex.Message,
            // StackTrace = ex.StackTrace // می‌توانید استک ترسیم را اضافه کنید (فقط در محیط توسعه)
        };

        // نوشتن پاسخ خطا
        return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
    }

    }
}