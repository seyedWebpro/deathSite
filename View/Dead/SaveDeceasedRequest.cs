using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.Dead
{
    public class SaveDeceasedRequest
    {
        public int UserId { get; set; }
        public int DeceasedId { get; set; }
    }
}