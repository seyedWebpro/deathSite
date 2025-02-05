using api.Model;

public class CondolenceMessage
{
    public int Id { get; set; }
    public string AuthorName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedDate { get; set; }
    public string DeceasedName { get; set; }
    public string MessageText { get; set; }
    public bool IsApproved { get; set; }

    // Foreign Keys for User and Deceased
 public int? UserId { get; set; } // nullable
    public User? User { get; set; } // nullable
    public int DeceasedId { get; set; }
    public Deceased Deceased { get; set; } // رابطه با متوفی
}
