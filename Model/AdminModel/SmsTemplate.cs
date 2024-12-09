using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Model.AdminModel
{
    public class SmsTemplate
    {
        public int Id { get; set; }

        // تغییر نوع MessageType به enum
        public string MessageType { get; set; } // نوع پیام به صورت Enum

        public string Message { get; set; } // متن پیام
    }

}