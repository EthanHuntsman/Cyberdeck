using Cyberdeck.Core.Enums;
using Cyberdeck.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cyberdeck.Core.Rules
{
    public static class DeckValidator
    {
        public static DeckValidationResult Validate(IEnumerable<DeckCard> deckCards)
        {
            var result = new DeckValidationResult();

            var cards = deckCards.ToList();

            var mainDeckCards = deckCards
                .Where(dc => dc.Card.Type != CardType.Legend)
                .ToList();

            var legends = deckCards
                .Where(dc => dc.Card.Type == CardType.Legend)
                .ToList();

            ValidateDeckSize(mainDeckCards, result);
            ValidateCopyLimits(mainDeckCards, result);

            ValidateLegends(legends, result);

            var ramLimits = GetRamLimitsFromLegends(legends);

            ValidateRamLimits(mainDeckCards, ramLimits, result);

            return result;
        }

        private static Dictionary<CardColor, int> GetRamLimitsFromLegends(IEnumerable<DeckCard> legends)
        {
            return legends
                .GroupBy(dc => dc.Card.Color)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(dc => dc.Card.Ram)
                );
        }

        private static void ValidateRamLimits(
            IEnumerable<DeckCard> mainDeckCards,
            IReadOnlyDictionary<CardColor, int> ramLimits,
            DeckValidationResult result)
        {
            foreach (var deckCard in mainDeckCards)
            {
                var card = deckCard.Card;

                var cardRam = card.Ram;

                if (!ramLimits.TryGetValue(card.Color, out var allowedRam))
                {
                    result.Errors.Add(
                        $"{card.Name} is {card.Color}, but you have no {card.Color} Legend."
                    );

                    continue;
                }

                if (cardRam > allowedRam)
                {
                    result.Errors.Add(
                        $"{card.Name} costs {cardRam} RAM, but your {card.Color} RAM limit is {allowedRam}."
                    );
                }
            }
        }

        private static void ValidateDeckSize(
            IEnumerable<DeckCard> mainDeckCards,
            DeckValidationResult result)
        {
            int mainDeckCount = mainDeckCards.Sum(dc => dc.Quantity);

            if (mainDeckCount < 40)
                result.Errors.Add($"Deck must contain more than 40 non-Legend cards. Current: {mainDeckCount}");

            if (mainDeckCount > 50)
                result.Errors.Add($"Deck cannot contain more than 50 non-Legend cards. Current: {mainDeckCount}");
        }

        private static void ValidateCopyLimits(
            IEnumerable<DeckCard> mainDeckCards,
            DeckValidationResult result)
        {
            foreach (var card in mainDeckCards)
            {
                if (card.Quantity > 3)
                {
                    result.Errors.Add($"{card.Card.Name} exceeds the 3-copy limit.");
                }
            }
        }

        private static void ValidateLegends(
            IEnumerable<DeckCard> legends,
            DeckValidationResult result)
        {
            var totalLegendCount = legends.Sum(dc => dc.Quantity);

            if (totalLegendCount != 3)
            {
                result.Errors.Add($"Deck must contain exactly 3 Legends. Current: {totalLegendCount}");
            }

            foreach (var legend in legends)
            {
                if (legend.Quantity > 1)
                {
                    result.Errors.Add($"Legend {legend.Card.Name} cannot have duplicates.");
                }
            }
        }
    }
}
