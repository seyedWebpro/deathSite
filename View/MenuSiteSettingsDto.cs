using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.View
{
    public class MenuSiteSettingsDto
    {
        public string Title { get; set; }  // نام منو
        public string Link { get; set; }   // لینک منو
        public int? Order { get; set; }     // ترتیب نمایش منوها (اختیاری)
    }
}