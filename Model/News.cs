using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Model
{
public class News
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string? FilePath { get; set; } // تغییر از لیست مسیرها به یک مسیر تکی
    public DateTime PublishedDate { get; set; }
}

}