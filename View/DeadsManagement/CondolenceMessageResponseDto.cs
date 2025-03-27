using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.DeadsManagement
{
   public class CondolenceMessageResponseDto
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string PhoneNumber { get; set; }
        public string MessageText { get; set; }
        public string Status { get; set; } // تغییر از bool به Enum
        public string DeceasedName { get; set; }
    }

}