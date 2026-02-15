using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI_WSB.Data;
using RestAPI_WSB.DTOs;
using RestAPI_WSB.Models;
using System.Security.Claims;

namespace RestAPI_WSB.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ZadaniaController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ZadaniaController(ApplicationDbContext context)
    {
        _context = context;
    }

    private string GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? throw new UnauthorizedAccessException("Użytkownik nie jest zalogowany");
    }

    /// <summary>
    /// Pobiera wszystkie zadania dostępne dla zalogowanego użytkownika
    /// (zadania z projektów własnych + zadania przypisane do użytkownika)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ZadanieResponseDto>>> GetZadania()
    {
        var userId = GetCurrentUserId();

        var zadania = await _context.Zadania
            .Include(z => z.Projekt)
            .Include(z => z.PrzypisanyUzytkownik)
            .Where(z => z.Projekt.WlascicielId == userId || 
                       z.PrzypisanyUzytkownikId == userId)
            .Select(z => new ZadanieResponseDto
            {
                Id = z.Id,
                Tytul = z.Tytul,
                Opis = z.Opis,
                DataUtworzenia = z.DataUtworzenia,
                TerminRealizacji = z.TerminRealizacji,
                Status = z.Status,
                StatusNazwa = z.Status.ToString(),
                ProjektId = z.ProjektId,
                ProjektNazwa = z.Projekt.Nazwa,
                PrzypisanyUzytkownikId = z.PrzypisanyUzytkownikId,
                PrzypisanyUzytkownikUserName = z.PrzypisanyUzytkownik != null ? z.PrzypisanyUzytkownik.UserName : null
            })
            .ToListAsync();

        return Ok(zadania);
    }

    /// <summary>
    /// Pobiera szczegóły konkretnego zadania
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ZadanieResponseDto>> GetZadanie(int id)
    {
        var userId = GetCurrentUserId();

        var zadanie = await _context.Zadania
            .Include(z => z.Projekt)
            .Include(z => z.PrzypisanyUzytkownik)
            .Where(z => z.Id == id && 
                       (z.Projekt.WlascicielId == userId || 
                        z.PrzypisanyUzytkownikId == userId))
            .Select(z => new ZadanieResponseDto
            {
                Id = z.Id,
                Tytul = z.Tytul,
                Opis = z.Opis,
                DataUtworzenia = z.DataUtworzenia,
                TerminRealizacji = z.TerminRealizacji,
                Status = z.Status,
                StatusNazwa = z.Status.ToString(),
                ProjektId = z.ProjektId,
                ProjektNazwa = z.Projekt.Nazwa,
                PrzypisanyUzytkownikId = z.PrzypisanyUzytkownikId,
                PrzypisanyUzytkownikUserName = z.PrzypisanyUzytkownik != null ? z.PrzypisanyUzytkownik.UserName : null
            })
            .FirstOrDefaultAsync();

        if (zadanie == null)
            return NotFound(new { message = "Zadanie nie istnieje lub brak dostępu" });

        return Ok(zadanie);
    }

    /// <summary>
    /// Tworzy nowe zadanie (tylko właściciel projektu)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ZadanieResponseDto>> CreateZadanie([FromBody] ZadanieCreateDto zadanieDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();

        // Sprawdź czy projekt istnieje i czy użytkownik jest właścicielem
        var projekt = await _context.Projekty.FindAsync(zadanieDto.ProjektId);
        if (projekt == null)
            return NotFound(new { message = "Projekt nie istnieje" });

        if (projekt.WlascicielId != userId)
            return Forbid();

        // Jeśli przypisano użytkownika, sprawdź czy istnieje
        if (!string.IsNullOrEmpty(zadanieDto.PrzypisanyUzytkownikId))
        {
            var przypisanyUzytkownik = await _context.Users.FindAsync(zadanieDto.PrzypisanyUzytkownikId);
            if (przypisanyUzytkownik == null)
                return BadRequest(new { message = "Przypisany użytkownik nie istnieje" });
        }

        var zadanie = new Zadanie
        {
            Tytul = zadanieDto.Tytul,
            Opis = zadanieDto.Opis,
            TerminRealizacji = zadanieDto.TerminRealizacji,
            ProjektId = zadanieDto.ProjektId,
            PrzypisanyUzytkownikId = zadanieDto.PrzypisanyUzytkownikId,
            Status = StatusZadania.DoWykonania,
            DataUtworzenia = DateTime.UtcNow
        };

        _context.Zadania.Add(zadanie);
        await _context.SaveChangesAsync();

        // Załaduj powiązane dane
        await _context.Entry(zadanie)
            .Reference(z => z.Projekt)
            .LoadAsync();
        
        if (!string.IsNullOrEmpty(zadanie.PrzypisanyUzytkownikId))
        {
            await _context.Entry(zadanie)
                .Reference(z => z.PrzypisanyUzytkownik)
                .LoadAsync();
        }

        var response = new ZadanieResponseDto
        {
            Id = zadanie.Id,
            Tytul = zadanie.Tytul,
            Opis = zadanie.Opis,
            DataUtworzenia = zadanie.DataUtworzenia,
            TerminRealizacji = zadanie.TerminRealizacji,
            Status = zadanie.Status,
            StatusNazwa = zadanie.Status.ToString(),
            ProjektId = zadanie.ProjektId,
            ProjektNazwa = zadanie.Projekt.Nazwa,
            PrzypisanyUzytkownikId = zadanie.PrzypisanyUzytkownikId,
            PrzypisanyUzytkownikUserName = zadanie.PrzypisanyUzytkownik?.UserName
        };

        return CreatedAtAction(nameof(GetZadanie), new { id = zadanie.Id }, response);
    }

    /// <summary>
    /// Aktualizuje zadanie (właściciel projektu lub przypisany użytkownik)
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateZadanie(int id, [FromBody] ZadanieUpdateDto zadanieDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();

        var zadanie = await _context.Zadania
            .Include(z => z.Projekt)
            .FirstOrDefaultAsync(z => z.Id == id);

        if (zadanie == null)
            return NotFound(new { message = "Zadanie nie istnieje" });

        // Tylko właściciel projektu lub przypisany użytkownik może edytować zadanie
        if (zadanie.Projekt.WlascicielId != userId && zadanie.PrzypisanyUzytkownikId != userId)
            return Forbid();

        // Jeśli próbuje zmienić przypisanego użytkownika, sprawdź uprawnienia
        if (zadanie.PrzypisanyUzytkownikId != zadanieDto.PrzypisanyUzytkownikId)
        {
            // Tylko właściciel projektu może zmieniać przypisanie
            if (zadanie.Projekt.WlascicielId != userId)
                return Forbid();

            if (!string.IsNullOrEmpty(zadanieDto.PrzypisanyUzytkownikId))
            {
                var przypisanyUzytkownik = await _context.Users.FindAsync(zadanieDto.PrzypisanyUzytkownikId);
                if (przypisanyUzytkownik == null)
                    return BadRequest(new { message = "Przypisany użytkownik nie istnieje" });
            }
        }

        zadanie.Tytul = zadanieDto.Tytul;
        zadanie.Opis = zadanieDto.Opis;
        zadanie.TerminRealizacji = zadanieDto.TerminRealizacji;
        zadanie.Status = zadanieDto.Status;
        zadanie.PrzypisanyUzytkownikId = zadanieDto.PrzypisanyUzytkownikId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Usuwa zadanie (tylko właściciel projektu)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteZadanie(int id)
    {
        var userId = GetCurrentUserId();

        var zadanie = await _context.Zadania
            .Include(z => z.Projekt)
            .FirstOrDefaultAsync(z => z.Id == id);

        if (zadanie == null)
            return NotFound(new { message = "Zadanie nie istnieje" });

        if (zadanie.Projekt.WlascicielId != userId)
            return Forbid();

        _context.Zadania.Remove(zadanie);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}