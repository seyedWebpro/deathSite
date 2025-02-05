using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Model.AdminModel
{
    public class SmsTemplate
    {
        public int Id { get; set; }

        public string MessageType { get; set; } 

        public string Message { get; set; } // متن پیام
    }

}