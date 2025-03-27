using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace deathSite.Model
{
     public class Sarbarg
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // رابطه بازگشتی (اختیاری) به متوفی
        public Deceased? Deceased { get; set; }
    }
}