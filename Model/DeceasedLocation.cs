using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace deathSite.Model
{
    public class DeceasedLocation
{
    public int Id { get; set; }

    public int DeceasedId { get; set; }
    public Deceased Deceased { get; set; }

    public string? Balad { get; set; }
    public string? Neshan { get; set; }
    public string? GoogleMap { get; set; }

public string? Mokhtasat { get; set; } // ذخیره به صورت رشته JSON در دیتابیس

}

}