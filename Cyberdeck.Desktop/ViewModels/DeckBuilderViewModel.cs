using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyberdeck.Core.Enums;
using Cyberdeck.Core.Models;
using Cyberdeck.Core.Rules;
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
        private DeckValidationResult deckValidation = new();

        [ObservableProperty]
        private Card? selectedCard;

        [ObservableProperty]
        private DeckCard? selectedDeckCard;

        [ObservableProperty]
        private int? selectedCardQuantity;

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
        public ObservableCollection<FilterOption<bool>> SellableFilters { get; } = [];


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

            _ = InitializeAsync();

            NewDeck();
        }

        private async Task InitializeAsync()
        {
            try
            {
                await LoadCardsAsync();
                await LoadDecksAsync();

                LoadFilters();
                NewDeck();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                // Log or display the initialization error.
                Debug.WriteLine(ex);
            }
        }

        partial void OnSelectedDeckCardChanged(DeckCard? value)
        {
            if (value is null) return;

            SelectedCard = value.Card;
            SelectedCardQuantity = value.Quantity;
        }

        partial void OnSelectedCardChanged(Card? value)
        {
            if (value is null) return;

            if (value.Type == CardType.Legend)
            {
                var exists = CurrentDeckLegends.FirstOrDefault(
                    c => c.Card.Id == value.Id);

                if (exists is not null)
                {
                    SelectedCardQuantity = exists.Quantity;
                }
                else
                {
                    SelectedCardQuantity = 0;
                }
            }
            else
            {
                var exists = CurrentDeckCards.FirstOrDefault(
                    c => c.Card.Id == value.Id);

                if (exists is not null)
                {
                    SelectedCardQuantity = exists.Quantity;
                }
                else
                {
                    SelectedCardQuantity = 0;
                }
            }
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

        partial void OnSearchTextChanged(string? oldValue, string newValue)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_allCards.Count() == 0) return;

            IEnumerable<Card> filtered = _allCards;


            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(c =>
                    c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    (c.CardText?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    c.Tags.Any(t => t.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    c.Keywords.Any(k => k.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            }

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

            var selectedSellable = SellableFilters
                .Where(f => f.IsSelected)
                .Select(f => f.Value)
                .ToList();

            if (selectedSellable.Any())
            {
                filtered = filtered.Where(c =>
                    selectedSellable.Contains(c.Sellable));
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

        [RelayCommand]
        private void ClearFilters()
        {
            DeselectAll(ColorFilters);
            DeselectAll(TypeFilters);
            DeselectAll(CostFilters);
            DeselectAll(PowerFilters);
            DeselectAll(RamFilters);
            DeselectAll(TagFilters);
            DeselectAll(KeywordFilters);
            DeselectAll(SellableFilters);

            SearchText = string.Empty;

            ApplyFilters();
        }

        private void LoadFilters()
        {
            PopulateFilters(
                ColorFilters,
                _allCards
                    .Select(c => c.Color)
                    .Distinct()
                    .OrderBy(c => c));

            PopulateFilters(
                TypeFilters,
                _allCards
                    .Select(c => c.Type)
                    .Distinct()
                    .OrderBy(t => t));

            PopulateFilters(
                TagFilters,
                _allCards
                    .SelectMany(c => c.Tags ?? Enumerable.Empty<string>())
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(t => t));

            PopulateFilters(
                KeywordFilters,
                _allCards
                    .SelectMany(c => c.Keywords ?? Enumerable.Empty<string>())
                    .Where(k => !string.IsNullOrWhiteSpace(k))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(k => k));

            PopulateFilters(
                CostFilters,
                _allCards
                    .Select(c => c.Cost)
                    .Distinct()
                    .OrderBy(c => c));

            PopulateFilters(
                PowerFilters,
                _allCards
                    .Select(c => c.Power)
                    .Distinct()
                    .OrderBy(p => p));

            PopulateFilters(
                RamFilters,
                _allCards
                    .Select(c => c.Ram)
                    .Distinct()
                    .OrderBy(r => r));

            PopulateFilters(
                SellableFilters,
                _allCards
                    .Select(c => c.Sellable)
                    .Distinct()
                    .OrderByDescending(s => s));
        }

        private static void DeselectAll<T>(IEnumerable<FilterOption<T>> filters)
        {
            foreach (var filter in filters)
                filter.IsSelected = false;
        }

        private void PopulateFilters<T>(
            ObservableCollection<FilterOption<T>> filters,
            IEnumerable<T> values)
        {
            filters.Clear();

            foreach (var value in values)
            {
                filters.Add(new FilterOption<T>(value, ApplyFilters));
            }
        }

        private async Task LoadCardsAsync()
        {
            var cards = await _cardService.GetAllCardsAsync();

            _allCards = cards.ToList();
        }

        [RelayCommand]
        private void AddCardToDeck(Card card)
        {
            if (card is null)
                return;

            if (card.Type == CardType.Legend)
            {
                var existing = CurrentDeckLegends
                .FirstOrDefault(dc => dc.Card.Id == card.Id);

                if (existing is not null)
                {
                    existing.Quantity++;

                    SelectedCardQuantity++;
                }
                else
                {
                    CurrentDeckLegends.Add(new DeckCard
                    {
                        Card = card,
                        CardId = card.Id,
                        Quantity = 1
                    });

                    SelectedCardQuantity = 1;
                }
            }
            else
            {
                var existing = CurrentDeckCards
                .FirstOrDefault(dc => dc.Card.Id == card.Id);

                if (existing is not null)
                {
                    existing.Quantity++;

                    SelectedCardQuantity++;
                }
                else
                {
                    CurrentDeckCards.Add(new DeckCard
                    {
                        Card = card,
                        CardId = card.Id,
                        Quantity = 1
                    });

                    SelectedCardQuantity = 1;
                }
            }

            ValidateDeck();
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

                if (existing.Quantity > 1)
                {
                    existing.Quantity--;

                    SelectedCardQuantity--;
                }
                else
                {
                    CurrentDeckLegends.Remove(existing);

                    SelectedCardQuantity = 0;
                }
            }
            else
            {
                var existing = CurrentDeckCards
                .FirstOrDefault(dc => dc.Card.Id == card.Id);

                if (existing is null) return;

                if (existing.Quantity > 1)
                {
                    existing.Quantity--;

                    SelectedCardQuantity--;
                }
                else
                {
                    CurrentDeckCards.Remove(existing);

                    SelectedCardQuantity = 0;
                }
            }

            ValidateDeck();
        }

        [RelayCommand]
        private void IncreaseDeckCardQuantity(DeckCard deckCard)
        {
            if (deckCard is null || deckCard.Quantity >= 3)
                return;

            deckCard.Quantity++;

            ValidateDeck();
        }

        [RelayCommand]
        private void SortDeck()
        {
            var sortedLegends = CurrentDeckLegends
                .OrderBy(c => c.Card.Color)
                .ThenBy(c => c.Card.Name)
                .ToList();

            CurrentDeckLegends.Clear();

            foreach (var legend in sortedLegends)
            {
                CurrentDeckLegends.Add(legend);
            }

            var sortedCards = CurrentDeckCards
                .OrderBy(c => c.Card.Color)
                .ThenBy(c => c.Card.Type)
                .ThenByDescending(c => c.Quantity)
                .ThenBy(c => c.Card.Name)
                .ToList();

            CurrentDeckCards.Clear();

            foreach(var card in sortedCards)
            {
                CurrentDeckCards.Add(card);
            }
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

            ValidateDeck();
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

            ValidateDeck();
        }

        [RelayCommand]
        private async Task DeleteDeckAsync()
        {
            if (CurrentDeck == null ) return;

            _db.Decks.Remove(CurrentDeck);
            await _db.SaveChangesAsync();

            NewDeck();

            await LoadDecksAsync();
        }

        private void ValidateDeck()
        {
            DeckValidation = DeckValidator.Validate(CurrentDeckCards.Concat(CurrentDeckLegends));
        }
    }
}
