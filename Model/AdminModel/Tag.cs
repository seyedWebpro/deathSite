using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Model.AdminModel
{
    public class Tag
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        // رابطه Many-to-Many
        public List<ShahidTag> ShahidTags { get; set; } = new List<ShahidTag>();
    }
}