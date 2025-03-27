using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.Dead
{
    public class DeceasedDto
    {
        
        public string FullName { get; set; }
        public string Gender { get; set; }
        
        [DataType(DataType.Date)] // تأکید بر دریافت فقط تاریخ
        public DateTime BirthDate { get; set; }

        [DataType(DataType.Date)] // تأکید بر دریافت فقط تاریخ
        public DateTime DateOfMartyrdom { get; set; }
    }
}