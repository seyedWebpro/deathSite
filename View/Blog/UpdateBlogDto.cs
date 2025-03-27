using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deathSite.View.Blog
{
    public class UpdateBlogDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
    }
}