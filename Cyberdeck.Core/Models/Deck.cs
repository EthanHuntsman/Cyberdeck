using Cyberdeck.Core.Enums;
using Cyberdeck.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cyberdeck.Core.Models;

public class Deck
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<DeckCard> Cards { get; set; } = new();

    [NotMapped]
    public IEnumerable<DeckCard> Legends =>
        Cards.Where(dc => dc.Card.Type == CardType.Legend);

    [NotMapped]
    public IEnumerable<DeckCard> MainDeckCards =>
        Cards.Where(dc => dc.Card.Type != CardType.Legend);

    [NotMapped]
    public int MainDeckCount =>
        MainDeckCards.Sum(dc => dc.Quantity);
}