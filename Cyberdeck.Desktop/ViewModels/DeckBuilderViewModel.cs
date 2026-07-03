using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyberdeck.Core.Enums;
using Cyberdeck.Core.Models;
using Cyberdeck.Data;
using Cyberdeck.Desktop.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Cyberdeck.Desktop.ViewModels
{
    public partial class DeckBuilderViewModel : ObservableObject
    {
        private readonly CardService _cardService;

        private List<Card> _allCards = [];

        [ObservableProperty]
        private ObservableCollection<Card> cards = new();

        public ObservableCollection<DeckCard> CurrentDeckCards { get; } = new();
        public ObservableCollection<DeckCard> CurrentDeckLegends { get; } = new();

        [ObservableProperty]
        private Card? selectedCard;

        [ObservableProperty]
        private DeckCard? selectedDeckCard;

        [ObservableProperty]
        private Deck? selectedSavedDeck;


        //FILTERS
        [ObservableProperty]
        private string searchText = "";

        public ObservableCollection<FilterOption<CardColor>> ColorFilters { get; } = [];
        public ObservableCollection<FilterOption<CardType>> TypeFilters { get; } = [];
        public ObservableCollection<FilterOption<string>> TagFilters { get; } = [];
        public ObservableCollection<FilterOption<string>> KeywordFilters { get; } = [];
        public ObservableCollection<FilterOption<int?>> CostFilters { get; } = [];
        public ObservableCollection<FilterOption<int?>> PowerFilters { get; } = [];
        public ObservableCollection<FilterOption<int>> RamFilters { get; } = [];


        [ObservableProperty]
        private Deck? currentDeck;

        [ObservableProperty]
        private string deckNameText = "";

        [ObservableProperty]
        private ObservableCollection<Deck> savedDecks = new();


        private readonly AppDbContext _db;

        
        public DeckBuilderViewModel(AppDbContext db, CardService cardService)
        {
            _db = db;

            _cardService = cardService;

            _ = LoadCardsAsync();
            _ = LoadDecksAsync();

            ColorFilters =
            [
            new(CardColor.Blue, ApplyFilters),
            new(CardColor.Green, ApplyFilters),
            new(CardColor.Red, ApplyFilters),
            new(CardColor.Yellow, ApplyFilters)
            ];

            TypeFilters =
            [
                new(CardType.Gear, ApplyFilters),
                new(CardType.Program, ApplyFilters),
                new(CardType.Legend, ApplyFilters),
                new(CardType.Unit, ApplyFilters)
            ];

            TagFilters =
            [
                new("arasaka", ApplyFilters),
                new("aldecaldo", ApplyFilters),
                new("ganger", ApplyFilters),
                new("maelstrom", ApplyFilters),
                new("merc", ApplyFilters),
                new("corpo", ApplyFilters),
                new("rocker", ApplyFilters),
                new("samurai", ApplyFilters),
                new("militech", ApplyFilters),
                new("valentino", ApplyFilters),
                new("drone", ApplyFilters)
            ];

            KeywordFilters =
            [
                new("go solo", ApplyFilters),
                new("play", ApplyFilters),
                new("quick", ApplyFilters),
                new("blocker", ApplyFilters),
                new("attack", ApplyFilters),
                new("defeated", ApplyFilters)
            ];

            CostFilters =
            [
                new(1, ApplyFilters),
                new(2, ApplyFilters),
                new(3, ApplyFilters),
                new(4, ApplyFilters),
                new(5, ApplyFilters),
                new(6, ApplyFilters),
                new(7, ApplyFilters),
                new(8, ApplyFilters),
                new(9, ApplyFilters)
            ];

            PowerFilters =
            [
                new(0, ApplyFilters),
                new(1, ApplyFilters),
                new(2, ApplyFilters),
                new(3, ApplyFilters),
                new(4, ApplyFilters),
                new(5, ApplyFilters),
                new(6, ApplyFilters),
                new(7, ApplyFilters),
                new(8, ApplyFilters),
                new(9, ApplyFilters)
            ];

            RamFilters =
            [
                new(0, ApplyFilters),
                new(1, ApplyFilters),
                new(2, ApplyFilters),
                new(3, ApplyFilters),
                new(4, ApplyFilters),
                new(5, ApplyFilters),
                new(6, ApplyFilters)
            ];

            NewDeck();
        }

        partial void OnSelectedDeckCardChanged(DeckCard? value)
        {
            if (value is null) return;

            SelectedCard = value.Card;
        }

        partial void OnSelectedSavedDeckChanged(Deck? value)
        {
            if (value is null)
                return;

            _ = OpenDeckAsync(value);
        }

        partial void OnDeckNameTextChanged(string value)
        {
            if (CurrentDeck != null) CurrentDeck.Name = value;
        }

        private void ApplyFilters()
        {
            if (_allCards.Count() == 0) return;

            IEnumerable<Card> filtered = _allCards;


            var selectedColors = ColorFilters
                .Where(f => f.IsSelected)
                .Select(f => f.Value)
                .ToList();

            if (selectedColors.Any())
            {
                filtered = filtered.Where(c =>
                    selectedColors.Contains(c.Color));
            }

            var selectedTypes = TypeFilters
                .Where(f => f.IsSelected)
                .Select(f => f.Value)
                .ToList();

            if (selectedTypes.Any())
            {
                filtered = filtered.Where(c =>
                    selectedTypes.Contains(c.Type));
            }

            var selectedTags = TagFilters
                .Where(f => f.IsSelected)
                .Select(f => f.Value)
                .ToList();

            if (selectedTags.Any())
            {
                filtered = filtered.Where(c =>
                    c.Tags != null &&
                    c.Tags.Any(cardTag => selectedTags.Contains(cardTag)));
            }

            var selectedKeywords = KeywordFilters
                .Where(f => f.IsSelected)
                .Select(f => f.Value)
                .ToList();

            if (selectedKeywords.Any())
            {
                filtered = filtered.Where(c =>
                    c.Keywords != null &&
                    c.Keywords.Any(cardKeyword => selectedKeywords.Contains(cardKeyword)));
            }

            var selectedCosts = CostFilters
                .Where(f => f.IsSelected)
                .Select(f => f.Value)
                .ToList();

            if (selectedCosts.Any())
            {
                filtered = filtered.Where(c =>
                    selectedCosts.Contains(c.Cost));
            }

            var selectedPowers = PowerFilters
                .Where(f => f.IsSelected)
                .Select(f => f.Value)
                .ToList();

            if (selectedPowers.Any())
            {
                filtered = filtered.Where(c =>
                    selectedPowers.Contains(c.Power));
            }

            var selectedRams = RamFilters
                .Where(f => f.IsSelected)
                .Select(f => f.Value)
                .ToList();

            if (selectedRams.Any())
            {
                filtered = filtered.Where(c =>
                    selectedRams.Contains(c.Ram));
            }

            var result = filtered
                .OrderBy(c => c.Color)
                .ThenBy(c => c.Type)
                .ThenBy(c => c.Cost)
                .ThenBy(c => c.Name)
                .ToList();

            Cards.Clear();

            foreach (var card in result)
            {
                Cards.Add(card);
            }

            
        }

        private async Task LoadCardsAsync()
        {
            var cards = await _cardService.GetAllCardsAsync();

            _allCards = cards.ToList();

            TagFilters.Clear();

           
               

            ApplyFilters();
        }

        [RelayCommand]
        private void AddCardToDeck(Card card)
        {
            if (card is null)
                return;

            if (card.Type == CardType.Legend)
            {
                if (CurrentDeckLegends.Count >= 3) return;

                var existing = CurrentDeckLegends
                .FirstOrDefault(dc => dc.Card.Id == card.Id);

                if (existing is not null) return;

                CurrentDeckLegends.Add(new DeckCard
                {
                    Card = card,
                    CardId = card.Id,
                    Quantity = 1
                });
            }
            else
            {
                var existing = CurrentDeckCards
                .FirstOrDefault(dc => dc.Card.Id == card.Id);

                if (existing is not null && existing.Quantity >= 3) return;

                if (existing is not null)
                {
                    existing.Quantity++;
                }
                else
                {
                    CurrentDeckCards.Add(new DeckCard
                    {
                        Card = card,
                        CardId = card.Id,
                        Quantity = 1
                    });
                }

                OnPropertyChanged(nameof(DeckCardCount));
            }
        }

        [RelayCommand]
        private void RemoveCardFromDeck(Card card)
        {
            if (card is null) return;

            if (card.Type == CardType.Legend)
            {
                var existing = CurrentDeckLegends
                .FirstOrDefault(dc => dc.Card.Id == card.Id);

                if (existing is null) return;

                CurrentDeckLegends.Remove(existing);
            }
            else
            {
                var existing = CurrentDeckCards
                .FirstOrDefault(dc => dc.Card.Id == card.Id);

                if (existing is null) return;

                if (existing.Quantity > 1)
                {
                    existing.Quantity--;
                }
                else
                {
                    CurrentDeckCards.Remove(existing);
                }

                OnPropertyChanged(nameof(DeckCardCount));
            }
        }

        [RelayCommand]
        private void IncreaseDeckCardQuantity(DeckCard deckCard)
        {
            if (deckCard is null || deckCard.Quantity >= 3)
                return;

            deckCard.Quantity++;

            OnPropertyChanged(nameof(DeckCardCount));
        }

        [RelayCommand]
        private void NewDeck()
        {
            CurrentDeck = new Deck
            {
                Name = "Untitled Deck"
            };

            DeckNameText = CurrentDeck.Name;

            CurrentDeckCards.Clear();
            CurrentDeckLegends.Clear();
        }

        [RelayCommand]
        private async Task SaveDeckAsync()
        {
            if (CurrentDeck == null)
                return;

            CurrentDeck.UpdatedAt = DateTime.Now;

            if (CurrentDeck.Id == 0)
            {
                _db.Decks.Add(CurrentDeck);
            }
            else
            {
                _db.Decks.Update(CurrentDeck);

                var oldCards = _db.DeckCards
                    .Where(dc => dc.DeckId == CurrentDeck.Id);

                _db.DeckCards.RemoveRange(oldCards);
            }

            await _db.SaveChangesAsync();

            foreach (var deckCard in CurrentDeckCards.Concat(CurrentDeckLegends))
            {
                _db.DeckCards.Add(new DeckCard
                {
                    DeckId = CurrentDeck.Id,
                    CardId = deckCard.CardId,
                    Quantity = deckCard.Quantity
                });
            }

            await _db.SaveChangesAsync();

            await LoadDecksAsync();
        }

        [RelayCommand]
        private async Task LoadDecksAsync()
        {
            var decks = await _db.Decks
                .OrderByDescending(d => d.UpdatedAt)
                .ToListAsync();

            SavedDecks = new ObservableCollection<Deck>(decks);
        }

        [RelayCommand]
        private async Task OpenDeckAsync(Deck deck)
        {
            var loadedDeck = await _db.Decks
                .Include(d => d.Cards)
                .ThenInclude(dc => dc.Card)
                .FirstOrDefaultAsync(d => d.Id == deck.Id);

            if (loadedDeck == null)
                return;

            CurrentDeck = loadedDeck;
            DeckNameText = CurrentDeck.Name;

            CurrentDeckCards.Clear();
            CurrentDeckLegends.Clear();

            foreach (var deckCard in loadedDeck.Cards)
            {
                if (deckCard.Card.Type == CardType.Legend)
                    CurrentDeckLegends.Add(deckCard);
                else
                    CurrentDeckCards.Add(deckCard);
            }

            OnPropertyChanged(nameof(DeckCardCount));
        }

        [RelayCommand]
        private async Task DeleteDeckAsync()
        {
            if (CurrentDeck == null ) return;

            _db.Decks.Remove(CurrentDeck);
            await _db.SaveChangesAsync();

            CurrentDeck = null;
            SelectedSavedDeck = null;
            CurrentDeckCards.Clear();
            CurrentDeckLegends.Clear();
            DeckNameText = "Untitled Deck";

            await LoadDecksAsync();
        }

        public int DeckCardCount => CurrentDeckCards.Sum(dc => dc.Quantity);
    }
}
