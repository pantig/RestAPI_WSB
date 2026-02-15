using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestAPI_WSB.Models;

namespace RestAPI_WSB.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Projekt> Projekty { get; set; }
    public DbSet<Zadanie> Zadania { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguracja relacji Użytkownik - Projekt
        modelBuilder.Entity<Projekt>()
            .HasOne(p => p.Wlasciciel)
            .WithMany(u => u.Projekty)
            .HasForeignKey(p => p.WlascicielId)
            .OnDelete(DeleteBehavior.Restrict);

        // Konfiguracja relacji Projekt - Zadanie
        modelBuilder.Entity<Zadanie>()
            .HasOne(z => z.Projekt)
            .WithMany(p => p.Zadania)
            .HasForeignKey(z => z.ProjektId)
            .OnDelete(DeleteBehavior.Cascade);

        // Konfiguracja relacji Użytkownik - Zadanie (przypisanie)
        modelBuilder.Entity<Zadanie>()
            .HasOne(z => z.PrzypisanyUzytkownik)
            .WithMany(u => u.PrzypisaneZadania)
            .HasForeignKey(z => z.PrzypisanyUzytkownikId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indeksy dla lepszej wydajności
        modelBuilder.Entity<Projekt>()
            .HasIndex(p => p.WlascicielId);

        modelBuilder.Entity<Zadanie>()
            .HasIndex(z => z.ProjektId);

        modelBuilder.Entity<Zadanie>()
            .HasIndex(z => z.PrzypisanyUzytkownikId);
    }
}