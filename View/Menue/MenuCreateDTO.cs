using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.Menue
{
    public class MenuCreateDTO
    {
         public string Title { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }
    }
}