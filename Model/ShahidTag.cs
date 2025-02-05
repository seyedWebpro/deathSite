using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model.AdminModel;

namespace api.Model
{
   public class ShahidTag
{
    public int ShahidId { get; set; }
    public Shahid Shahid { get; set; }

    public int TagId { get; set; }
    public Tag Tag { get; set; }
}

}