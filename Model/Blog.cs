using System;
using System.Collections.Generic;

namespace api.Model
{
   public class Blog
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string? FilePath { get; set; } 
    public DateTime PublishedDate { get; set; }
}

}
