using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Model
{
    public class UserToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public bool IsExpired { get; set; } = false; // وضعیت انقضا
    }
}