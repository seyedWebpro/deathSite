using System.Text.Json.Serialization;
using api.Model;
using deathSite.Model;

public class CondolenceMessage
{
    public int Id { get; set; }
    public string AuthorName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedDate { get; set; }
    public string DeceasedName { get; set; }
    public string MessageText { get; set; }

    // استفاده از Enum برای وضعیت کامنت
    public CommentStatus Status { get; set; }  // تغییر از bool به Enum

    // Foreign Keys for User and Deceased
    public int? UserId { get; set; } // nullable

    [JsonIgnore]
    public User? User { get; set; } // nullable
    public int DeceasedId { get; set; }
    public Deceased Deceased { get; set; } // رابطه با متوفی

    public List<CondolenceReply> Replies { get; set; } = new List<CondolenceReply>();
}

public enum CommentStatus
    {
        Pending = 0, // در انتظار تایید
        Approved = 1, // تایید شده
        Rejected = 2 // رد شده
    }