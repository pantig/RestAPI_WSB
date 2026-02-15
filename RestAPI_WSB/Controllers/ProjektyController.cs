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
public class ProjektyController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProjektyController(ApplicationDbContext context)
    {
        _context = context;
    }

    private string GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? throw new UnauthorizedAccessException("Użytkownik nie jest zalogowany");
    }

    /// <summary>
    /// Pobiera wszystkie projekty dostępne dla zalogowanego użytkownika
    /// (własne projekty + projekty z przypisanymi zadaniami)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjektResponseDto>>> GetProjekty()
    {
        var userId = GetCurrentUserId();

        // Projekty gdzie użytkownik jest właścicielem lub ma przypisane zadania
        var projekty = await _context.Projekty
            .Include(p => p.Wlasciciel)
            .Include(p => p.Zadania)
            .Where(p => p.WlascicielId == userId || 
                       p.Zadania.Any(z => z.PrzypisanyUzytkownikId == userId))
            .Select(p => new ProjektResponseDto
            {
                Id = p.Id,
                Nazwa = p.Nazwa,
                Opis = p.Opis,
                DataUtworzenia = p.DataUtworzenia,
                DataZakonczenia = p.DataZakonczenia,
                WlascicielId = p.WlascicielId,
                WlascicielUserName = p.Wlasciciel.UserName ?? string.Empty,
                JestWlascicielem = p.WlascicielId == userId,
                LiczbaZadan = p.Zadania.Count
            })
            .ToListAsync();

        return Ok(projekty);
    }

    /// <summary>
    /// Pobiera szczegóły konkretnego projektu
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjektResponseDto>> GetProjekt(int id)
    {
        var userId = GetCurrentUserId();

        var projekt = await _context.Projekty
            .Include(p => p.Wlasciciel)
            .Include(p => p.Zadania)
            .Where(p => p.Id == id && 
                       (p.WlascicielId == userId || 
                        p.Zadania.Any(z => z.PrzypisanyUzytkownikId == userId)))
            .Select(p => new ProjektResponseDto
            {
                Id = p.Id,
                Nazwa = p.Nazwa,
                Opis = p.Opis,
                DataUtworzenia = p.DataUtworzenia,
                DataZakonczenia = p.DataZakonczenia,
                WlascicielId = p.WlascicielId,
                WlascicielUserName = p.Wlasciciel.UserName ?? string.Empty,
                JestWlascicielem = p.WlascicielId == userId,
                LiczbaZadan = p.Zadania.Count
            })
            .FirstOrDefaultAsync();

        if (projekt == null)
            return NotFound(new { message = "Projekt nie istnieje lub brak dostępu" });

        return Ok(projekt);
    }

    /// <summary>
    /// Tworzy nowy projekt
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProjektResponseDto>> CreateProjekt([FromBody] ProjektCreateDto projektDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();

        var projekt = new Projekt
        {
            Nazwa = projektDto.Nazwa,
            Opis = projektDto.Opis,
            DataZakonczenia = projektDto.DataZakonczenia,
            WlascicielId = userId,
            DataUtworzenia = DateTime.UtcNow
        };

        _context.Projekty.Add(projekt);
        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(userId);

        var response = new ProjektResponseDto
        {
            Id = projekt.Id,
            Nazwa = projekt.Nazwa,
            Opis = projekt.Opis,
            DataUtworzenia = projekt.DataUtworzenia,
            DataZakonczenia = projekt.DataZakonczenia,
            WlascicielId = projekt.WlascicielId,
            WlascicielUserName = user?.UserName ?? string.Empty,
            JestWlascicielem = true,
            LiczbaZadan = 0
        };

        return CreatedAtAction(nameof(GetProjekt), new { id = projekt.Id }, response);
    }

    /// <summary>
    /// Aktualizuje projekt (tylko właściciel)
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProjekt(int id, [FromBody] ProjektUpdateDto projektDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();

        var projekt = await _context.Projekty.FindAsync(id);
        if (projekt == null)
            return NotFound(new { message = "Projekt nie istnieje" });

        if (projekt.WlascicielId != userId)
            return Forbid();

        projekt.Nazwa = projektDto.Nazwa;
        projekt.Opis = projektDto.Opis;
        projekt.DataZakonczenia = projektDto.DataZakonczenia;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Usuwa projekt (tylko właściciel)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProjekt(int id)
    {
        var userId = GetCurrentUserId();

        var projekt = await _context.Projekty.FindAsync(id);
        if (projekt == null)
            return NotFound(new { message = "Projekt nie istnieje" });

        if (projekt.WlascicielId != userId)
            return Forbid();

        _context.Projekty.Remove(projekt);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}