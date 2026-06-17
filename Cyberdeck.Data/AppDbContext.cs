using Cyberdeck.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using System.Windows;

namespace Cyberdeck.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<Deck> Decks => Set<Deck>();
    public DbSet<DeckCard> DeckCards => Set<DeckCard>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeckCard>()
            .HasIndex(dc => new { dc.DeckId, dc.CardId })
            .IsUnique();
    }
}