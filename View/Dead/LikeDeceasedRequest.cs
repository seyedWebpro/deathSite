using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.Dead
{
    public class LikeDeceasedRequest
{
    [Required(ErrorMessage = "شناسه کاربر الزامی است.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "شناسه متوفی الزامی است.")]
    public int DeceasedId { get; set; }
}
}