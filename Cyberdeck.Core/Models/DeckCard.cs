using Cyberdeck.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cyberdeck.Core.Models;

public partial class DeckCard : ObservableObject
{
    public int Id { get; set; }

    public int DeckId { get; set; }

    public Deck Deck { get; set; } = null!;

    public int CardId { get; set; }

    public Card Card { get; set; } = null!;

    [ObservableProperty]
    private int quantity;
}