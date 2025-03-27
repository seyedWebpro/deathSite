using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.ShahidRequest
{
    public class ShahidUpdateRequestDto
    {
        public string? Biography { get; set; }
        public string? Memories { get; set; }
        public string? Will { get; set; }
         // فایل‌های ارسالی (اختیاری)
    public List<IFormFile>? PhotoFiles { get; set; }
    public List<IFormFile>? VideoFiles { get; set; }
    public List<IFormFile>? VoiceFiles { get; set; }
    }
}