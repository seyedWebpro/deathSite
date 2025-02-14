using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Model.AdminModel
{
    public class MenuSiteSettings
    {
        public int Id { get; set; }
        public string Title { get; set; }  // نام منو
        public string Link { get; set; }   // لینک منو
        public int? Order { get; set; }     // ترتیب نمایش منوها
        public string IconImagePath { get; set; }  // مسیر لوگو منو

    }
}