using CommunityToolkit.Mvvm.ComponentModel;
using Cyberdeck.Core.Models;
using Cyberdeck.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Cyberdeck.Desktop.ViewModels;

public partial class BrowseCardsViewModel : ObservableObject
{
    private readonly List<Card> _allCards = new();

    public ObservableCollection<Card> Cards { get; } = new();

    [ObservableProperty]
    private Card? selectedCard;

    [ObservableProperty]
    private string searchText = "";

    [ObservableProperty]
    private string selectedColor = "All";

    [ObservableProperty]
    private string selectedType = "All";

    public ObservableCollection<string> Colors { get; } = new();
    public ObservableCollection<string> Types { get; } = new();

    public BrowseCardsViewModel()
    {
        LoadCards();
        LoadFilterOptions();
        ApplyFilters();
    }

    private void LoadCards()
    {
        using var db = new AppDbContext();

        _allCards.Clear();

        _allCards.AddRange(
            db.Cards
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToList()
        );
    }

    private void LoadFilterOptions()
    {
        Colors.Clear();
        Colors.Add("All");

        foreach (var color in _allCards
                     .Select(c => c.Color.ToString())
                     .Where(c => !string.IsNullOrWhiteSpace(c))
                     .Distinct()
                     .OrderBy(c => c))
        {
            Colors.Add(color);
        }

        Types.Clear();
        Types.Add("All");

        foreach (var type in _allCards
                     .Select(c => c.Type.ToString())
                     .Where(t => !string.IsNullOrWhiteSpace(t))
                     .Distinct()
                     .OrderBy(t => t))
        {
            Types.Add(type);
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilters();
    }

    partial void OnSelectedColorChanged(string value)
    {
        ApplyFilters();
    }

    partial void OnSelectedTypeChanged(string value)
    {
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        Cards.Clear();

        var filtered = _allCards.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(c =>
                c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        if (SelectedColor != "All")
        {
            filtered = filtered.Where(c => c.Color.ToString() == SelectedColor);
        }

        if (SelectedType != "All")
        {
            filtered = filtered.Where(c => c.Type.ToString() == SelectedType);
        }

        foreach (var card in filtered)
        {
            Cards.Add(card);
        }

        SelectedCard = Cards.FirstOrDefault();
    }
}