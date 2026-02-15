using Microsoft.AspNetCore.Identity;

namespace RestAPI_WSB.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Projekt> Projekty { get; set; } = new List<Projekt>();
    public ICollection<Zadanie> PrzypisaneZadania { get; set; } = new List<Zadanie>();
}