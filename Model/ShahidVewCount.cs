using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace deathSite.Model
{
    public class ShahidViewCount
    {
        public int Id { get; set; }
        public int ShahidId { get; set; }
        public Shahid Shahid { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }

        public string IPAddress { get; set; }
        public DateTime ViewDate { get; set; } = DateTime.UtcNow;
    }
}