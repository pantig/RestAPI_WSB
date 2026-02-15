using System.ComponentModel.DataAnnotations;

namespace RestAPI_WSB.Models;

public class Projekt
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Nazwa { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Opis { get; set; }
    
    public DateTime DataUtworzenia { get; set; } = DateTime.UtcNow;
    
    public DateTime? DataZakonczenia { get; set; }
    
    [Required]
    public string WlascicielId { get; set; } = string.Empty;
    
    public ApplicationUser Wlasciciel { get; set; } = null!;
    
    public ICollection<Zadanie> Zadania { get; set; } = new List<Zadanie>();
}