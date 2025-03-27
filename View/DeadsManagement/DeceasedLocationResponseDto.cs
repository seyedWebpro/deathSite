using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.DeadsManagement
{
    public class DeceasedLocationResponseDto
    {
        public int Id { get; set; }
        public int DeceasedId { get; set; }
        public string? Balad { get; set; }
        public string? Neshan { get; set; }
        public string? GoogleMap { get; set; }
        public double[]? Mokhtasat { get; set; }
    }
}