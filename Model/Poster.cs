using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.Model
{
    public class Poster
    {
         public int Id { get; set; } // شناسه پوستر
    public string FilePath { get; set; } // مسیر فایل
    public string Category { get; set; } // دسته‌بندی (People یا Shahid)
    }
}