using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model.AdminModel;

namespace api.Model
{
    public class Deceased
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string PlaceOfMartyrdom { get; set; }
    public DateTime DateOfMartyrdom { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Age { get; set; }

    public string Description { get; set; }
    public string Khaterat { get; set; }

    public List<string> PhotoUrls { get; set; } = new List<string>();
    public List<string> VideoUrls { get; set; } = new List<string>();
    public List<string> VoiceUrls { get; set; } = new List<string>();

    public DateTime PublishedDate { get; set; }

    public int? OwnerId { get; set; }
    public User? Owner { get; set; }

    public List<CondolenceMessage> CondolenceMessages { get; set; } = new List<CondolenceMessage>();
}
}