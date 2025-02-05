using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ShahidUpdateView
{
    public string? FullName { get; set; }

    public string? FatherName { get; set; }

    public DateOnly? BirthBorn { get; set; }

    public DateOnly? BirthDead { get; set; }

    public string? PlaceDead { get; set; }

    public string? PlaceOfBurial { get; set; }

    public List<string>? Responsibilities { get; set; } = new List<string>();

    public List<string>? Operations { get; set; } = new List<string>();

    public string? Biography { get; set; }

    public string? Will { get; set; }

    [JsonIgnore]
    public List<string>? PhotoUrls { get; set; } = new List<string>();

    [JsonIgnore]
    public List<string>? VideoUrls { get; set; } = new List<string>();

    [JsonIgnore]
    public List<string>? VoiceUrls { get; set; } = new List<string>();

    public string? CauseOfMartyrdom { get; set; }

    public string? LastResponsibility { get; set; }

    public string? Gorooh { get; set; }

    public string? Yegan { get; set; }

    public string? Niru { get; set; }

    public string? Ghesmat { get; set; }

    public string? PoemVerseOne { get; set; }

    public string? PoemVerseTwo { get; set; }

    public string? Memories { get; set; }
}
