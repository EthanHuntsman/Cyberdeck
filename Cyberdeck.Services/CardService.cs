using Cyberdeck.Core.Models;
using Cyberdeck.Data;
using Microsoft.EntityFrameworkCore;

public class CardService
{
    private readonly AppDbContext _context;

    public CardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Card>> GetAllCardsAsync()
    {
        return await _context.Cards.ToListAsync();
    }
}