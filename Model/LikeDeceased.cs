using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace deathSite.Model
{
    public class LikeDeceased
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int DeceasedId { get; set; }
    public Deceased Deceased { get; set; }

    public DateTime LikedAt { get; set; } = DateTime.UtcNow;
}

}