using System.ComponentModel.DataAnnotations;
using RestAPI_WSB.Models;

namespace RestAPI_WSB.DTOs;

public class ZadanieCreateDto
{
    [Required]
    [MaxLength(200)]
    public string Tytul { get; set; } = string.Empty;
    
    [MaxLength(2000)]
    public string? Opis { get; set; }
    
    public DateTime? TerminRealizacji { get; set; }
    
    [Required]
    public int ProjektId { get; set; }
    
    public string? PrzypisanyUzytkownikId { get; set; }
}

public class ZadanieUpdateDto
{
    [Required]
    [MaxLength(200)]
    public string Tytul { get; set; } = string.Empty;
    
    [MaxLength(2000)]
    public string? Opis { get; set; }
    
    public DateTime? TerminRealizacji { get; set; }
    
    [Required]
    public StatusZadania Status { get; set; }
    
    public string? PrzypisanyUzytkownikId { get; set; }
}

public class ZadanieResponseDto
{
    public int Id { get; set; }
    public string Tytul { get; set; } = string.Empty;
    public string? Opis { get; set; }
    public DateTime DataUtworzenia { get; set; }
    public DateTime? TerminRealizacji { get; set; }
    public StatusZadania Status { get; set; }
    public string StatusNazwa { get; set; } = string.Empty;
    public int ProjektId { get; set; }
    public string ProjektNazwa { get; set; } = string.Empty;
    public string? PrzypisanyUzytkownikId { get; set; }
    public string? PrzypisanyUzytkownikUserName { get; set; }
}