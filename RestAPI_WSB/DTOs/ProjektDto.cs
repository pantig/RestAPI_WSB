using System.ComponentModel.DataAnnotations;

namespace RestAPI_WSB.DTOs;

public class ProjektCreateDto
{
    [Required]
    [MaxLength(200)]
    public string Nazwa { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Opis { get; set; }
    
    public DateTime? DataZakonczenia { get; set; }
}

public class ProjektUpdateDto
{
    [Required]
    [MaxLength(200)]
    public string Nazwa { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Opis { get; set; }
    
    public DateTime? DataZakonczenia { get; set; }
}

public class ProjektResponseDto
{
    public int Id { get; set; }
    public string Nazwa { get; set; } = string.Empty;
    public string? Opis { get; set; }
    public DateTime DataUtworzenia { get; set; }
    public DateTime? DataZakonczenia { get; set; }
    public string WlascicielId { get; set; } = string.Empty;
    public string WlascicielUserName { get; set; } = string.Empty;
    public bool JestWlascicielem { get; set; }
    public int LiczbaZadan { get; set; }
}