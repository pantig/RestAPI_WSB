using System.ComponentModel.DataAnnotations;

namespace RestAPI_WSB.Models;

public class Zadanie
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Tytul { get; set; } = string.Empty;
    
    [MaxLength(2000)]
    public string? Opis { get; set; }
    
    public DateTime DataUtworzenia { get; set; } = DateTime.UtcNow;
    
    public DateTime? TerminRealizacji { get; set; }
    
    [Required]
    public StatusZadania Status { get; set; } = StatusZadania.DoWykonania;
    
    [Required]
    public int ProjektId { get; set; }
    
    public Projekt Projekt { get; set; } = null!;
    
    public string? PrzypisanyUzytkownikId { get; set; }
    
    public ApplicationUser? PrzypisanyUzytkownik { get; set; }
}

public enum StatusZadania
{
    DoWykonania = 0,
    WTrakcie = 1,
    Zakonczone = 2,
    Anulowane = 3
}