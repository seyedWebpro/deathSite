using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.View
{
    public class CondolenceMessageView
    {
        public int Id { get; set; }
    public string AuthorName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedDate { get; set; }
    public string DeceasedName { get; set; }
    public string MessageText { get; set; }
    public bool IsApproved { get; set; }
    }
}