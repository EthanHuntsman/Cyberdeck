using Cyberdeck.Core.Models;

namespace Cyberdeck.Core.Models;

public class Deck
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<DeckCard> Cards { get; set; } = new();
}