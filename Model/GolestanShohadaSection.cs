using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.Model
{
    public class GolestanShohadaSection
    {
        public int Id { get; set; }
        public string? Icon { get; set; } // آیکون بخش (می‌تواند URL یا نام کلاس CSS باشد)
        public string? Link { get; set; } // لینک بخش
    }

}