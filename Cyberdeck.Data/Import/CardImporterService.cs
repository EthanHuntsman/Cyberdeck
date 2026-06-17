using Cyberdeck.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberdeck.Data.Import;

public class CardImporterService
{
    private readonly AppDbContext _context;

    public CardImporterService(AppDbContext context)
    {
        _context = context;
    }

    public async Task ImportCardsAsync(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Card import file not found.", filePath);

        var json = await File.ReadAllTextAsync(filePath);

        var cards = JsonSerializer.Deserialize<List<Card>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        });

        if (cards is null || cards.Count == 0)
            return;

        foreach (var importedCard in cards)
        {
            var existingCard = await _context.Cards
                .FirstOrDefaultAsync(c => c.CardId == importedCard.CardId);

            if (existingCard is null)
            {
                _context.Cards.Add(importedCard);
            }
            else
            {
                existingCard.Name = importedCard.Name;
                existingCard.SubName = importedCard.SubName;
                existingCard.Color = importedCard.Color;
                existingCard.Type = importedCard.Type;
                existingCard.Cost = importedCard.Cost;
                existingCard.Ram = importedCard.Ram;
                existingCard.Tags = importedCard.Tags;
                existingCard.Keywords = importedCard.Keywords;
                existingCard.Sellable = importedCard.Sellable;
                existingCard.CardText = importedCard.CardText;
                existingCard.ImagePath = importedCard.ImagePath;
            }
        }

        await _context.SaveChangesAsync();
    }
}