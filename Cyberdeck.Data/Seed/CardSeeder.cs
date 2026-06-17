using Cyberdeck.Core.Enums;
using Cyberdeck.Core.Models;
using Cyberdeck.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Cyberdeck.Data.Seed
{
    public static class CardSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (db.Cards.Any())
                return;

            db.Cards.AddRange(
                new Card
                {
                    Name = "Royce",
                    SubName = "Psycho on the Edge",
                    Color = CardColor.Red,
                    Type = CardType.Legend,
                    Rarity = CardRarity.Rare,
                    Ram = 2,
                    Cost = 6,
                    Power = 6,
                    PowerScale = true,
                    Sellable = true,
                    Tags = new List<string>
                    {
                        "ganger",
                        "maelstrom"
                    },
                    Keywords = new List<string>
                    {
                        "go solo"
                    },
                    CardText = "During your turn, this Legend has +2 power for each of its equipped Gear.",
                    ImagePath = "/Assets/Cards/royce_psycho.png"
                },
                new Card
                {
                    Name = "Minotaur",
                    Color = CardColor.Red,
                    Type = CardType.Unit,
                    Rarity = CardRarity.Uncommon,
                    Ram = 2,
                    Cost = 7,
                    Power = 9,
                    PowerScale = false,
                    Sellable = false,
                    Tags = new List<string>
                    {
                        "arasaka",
                        "drone",
                        "militech"
                    },
                    Keywords = new List<string>
                    {
                        "play"
                    },
                    CardText = "If you have more Street Cred than a Rival, defeat a rival unit with power 5 or less."
                },
                new Card
                {
                    Name = "Mantis Blades",
                    Color = CardColor.Red,
                    Type = CardType.Gear,
                    Rarity = CardRarity.Common,
                    Ram = 1,
                    Cost = 1,
                    Power = 2,
                    PowerScale = false,
                    Sellable = true,
                    Tags = new List<string>
                    {
                        "cyberware"
                    }
                },
                new Card
                {
                    Name = "Carnage at the Colosseum",
                    Color = CardColor.Red,
                    Type = CardType.Program,
                    Rarity = CardRarity.Common,
                    Ram = 3,
                    Cost = 6,
                    Sellable = true,
                    Tags = new List<string>
                    {
                        "braindance",
                        "extreme"
                    },
                    CardText = "Play this Program for -1 E$ for each friendly Gig with 8+ value, to a minimum of 1 E$.\n" +
                    "Defeat a rival unit with less power than a friendly one."
                },
                new Card
                {
                    Name = "Alt Cunningham",
                    SubName = "Soulkiller Architect",
                    Color = CardColor.Blue,
                    Type = CardType.Legend,
                    Rarity = CardRarity.Rare,
                    Ram = 2,
                    Sellable = true,
                    Tags = new List<string>
                    {
                        "merc",
                        "netrunner"
                    },
                    CardText = "Spend: Your next Program this turn plays for -1 E$ for each friendly min Gig, to a minimum of 1 E$\n" +
                    "1 E$, Spend: Play a Program from your trash. Bottom-deck it after you play it. (You still pay its cost.)"
                },
                new Card
                {
                    Name = "MT0D12 Flathead",
                    Color = CardColor.Blue,
                    Type = CardType.Unit,
                    Rarity = CardRarity.Uncommon,
                    Ram = 3,
                    Cost = 5,
                    Power = 7,
                    PowerScale = false,
                    Sellable= true,
                    Tags = new List<string>
                    {
                        "drone",
                        "militech"
                    },
                    CardText = "If you have less Street Cred than a Rival, this Unit can't be blocked."
                },
                new Card
                {
                    Name = "Dying Night",
                    SubName = "V's Pistol",
                    Color = CardColor.Blue,
                    Type = CardType.Gear,
                    Rarity = CardRarity.Rare,
                    Ram = 2,
                    Cost = 2,
                    Power = 2,
                    PowerScale = false,
                    Sellable = true,
                    Tags = new List<string>
                    {
                        "merc",
                        "weapon"
                    },
                    Keywords = new List<string>
                    {
                        "attack"
                    },
                    CardText = "Decrease a Gig by up to 2. At the end of your turn, if this Unit is named 'V', ready 2 eddies."
                },
                new Card
                {
                    Name = "Chrome Reverie",
                    Color = CardColor.Blue,
                    Type = CardType.Program,
                    Rarity = CardRarity.Common,
                    Ram = 1,
                    Cost = 3,
                    Sellable = true,
                    Tags = new List<string>
                    {
                        "braindance"
                    },
                    CardText = "A rival Unit can't attack until your next turn. If you control a min Gig, you may Call a Legend for free."
                },
                new Card
                {
                    Name = "Panam Palmer",
                    SubName = "Nomad Cavalry",
                    Color = CardColor.Green,
                    Type = CardType.Legend,
                    Rarity = CardRarity.IconicRare,
                    Ram = 2,
                    Sellable = true,
                    Tags = new List<string>
                    {
                        "aldecaldo",
                        "merc",
                        "nomad"
                    },
                    CardText = "2 E$, Spend: Move a Gear from this Legend to an unequipped friendly Unit. If you do, ready that Unit.\n" +
                    "At the end of your turn, if 5 or more friendly Units and/or Legends are equipped, ready them."
                },
                new Card
                {
                    Name = "Sandayu Oda",
                    SubName = "Hanako's Guardian",
                    Color = CardColor.Green,
                    Type = CardType.Unit,
                    Rarity = CardRarity.Rare,
                    Ram = 2,
                    Cost = 7,
                    Power = 8,
                    PowerScale = false,
                    Sellable = false,
                    Tags = new List<string>
                    {
                        "arasaka",
                        "corpo"
                    },
                    Keywords = new List<string>
                    {
                        "play"
                    },
                    CardText = "Spend a rival Unit for each friendly value-pair of Gigs.\n" +
                    "This Unit can attack a rival Units the turn it's played."
                },
                new Card
                {
                    Name = "Sandevistan",
                    Color = CardColor.Green,
                    Type = CardType.Gear,
                    Rarity = CardRarity.Uncommon,
                    Ram = 3,
                    Cost = 3,
                    Power = 2,
                    PowerScale = false,
                    Sellable = true,
                    Tags = new List<string>
                    {
                        "cyberware"
                    },
                    CardText = "At the end of your turn, ready this Unit or Legend."
                },
                new Card
                {
                    Name = "Peace Offering",
                    Color = CardColor.Green,
                    Type = CardType.Program,
                    Rarity = CardRarity.Common,
                    Ram = 1,
                    Cost = 1,
                    Sellable= true,
                    Tags = new List<string>
                    {
                        "braindance",
                    },
                    CardText = "You may set a Gig's value to the value of another Gig. Then, if you control a value-pair, draw 1."
                },
                new Card
                {
                    Name = "Dum Dum",
                    SubName = "Maelstrom Triggerman",
                    Type = CardType.Legend,
                    Color = CardColor.Yellow,
                    Rarity = CardRarity.Rare,
                    Ram = 2,
                    Sellable = true,
                    Tags = new List<string>
                    {
                        "ganger",
                        "maelstrom"
                    },
                    Keywords = new List<string>
                    {
                        "call",
                        "quick"
                    },
                    CardText = "You may defeat a friendly Gear. If you do, draw 2. Otherwise, draw 1.\n" +
                    "1 E$, Spend: Give a friendly Unit +1 power this turn for each of its equipped Gear."
                },
                new Card
                {
                    Name = "Jackie Welles",
                    SubName = "Ride or Die Choom",
                    Type = CardType.Unit,
                    Color = CardColor.Yellow,
                    Rarity = CardRarity.IconicRare,
                    Ram = 2,
                    Cost = 6,
                    Power = 8,
                    PowerScale = true,
                    Sellable = false,
                    Tags = new List<string>
                    {
                        "merc",
                        "valentino"
                    },
                    Keywords = new List<string>
                    {
                        "attack",
                        "defeated"
                    },
                    CardText = "Give this Unit +2 power this turn for each friendly Gig with an even value.\n" +
                    "Draw 1 for each friendly Gig with an odd value."
                },
                new Card
                {
                    Name = "Gorilla Arms",
                    Type = CardType.Gear,
                    Color = CardColor.Yellow,
                    Rarity = CardRarity.Common,
                    Ram = 3,
                    Cost = 4,
                    Power = 3,
                    PowerScale = true,
                    Sellable = true,
                    Tags = new List<string>
                    {
                        "cyberware"
                    },
                    CardText = "The first time this Unit steals 1 or more Gigs each turn, steal a rival Gig with a value not shared by a friendly Gig."
                },
                new Card
                {
                    Name = "Cyberpsychosis",
                    Type = CardType.Program,
                    Color = CardColor.Yellow,
                    Rarity = CardRarity.Uncommon,
                    Ram = 2,
                    Cost = 3,
                    Sellable = true,
                    Tags = new List<string>
                    {
                        "quickhack"
                    },
                    Keywords = new List<string>
                    {
                        "quick"
                    },
                    CardText = "Give an equipped Unit +3 power this turn for each of its equipped Gears. If that Unit steals or fights, defeat it at the end of this turn."
                }
            );

            db.SaveChanges();
        }
    }
}
