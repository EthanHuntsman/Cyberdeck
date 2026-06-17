using Cyberdeck.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cyberdeck.Core.Models
{
    public class Card
    {
        public int Id { get; set; }
        public int Ram { get; set; }
        public int? Cost { get; set; }
        public int? Power { get; set; }

        public string Name { get; set; } = string.Empty;
        public string CardId { get; set; } = string.Empty;
        public string? SubName { get; set; }
        public string? CardText { get; set; }
        public string? ImagePath { get; set; }

        public CardColor Color { get; set; }
        public CardType Type { get; set; }
        public CardRarity Rarity { get; set; }
        
        public bool Sellable { get; set; }
        public bool? PowerScale { get; set; }

        public List<string> Tags { get; set; } = new();
        public List<string> Keywords { get; set; } = new();
    }
}
