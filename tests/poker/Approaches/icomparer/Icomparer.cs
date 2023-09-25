using System;
using System.Collections.Generic;
using System.Linq;

public static class Poker
{
    public static IEnumerable<string> BestHands(IEnumerable<string> hands)
    {
        var parsedHands = hands.ToDictionary(hand => hand, Parser.ParseHand);
        var bestHand = parsedHands.Values.Max();

        return parsedHands
            .Where(hand => hand.Value.CompareTo(bestHand) == 0)
            .Select(hand => hand.Key)
            .ToArray();
    }

    private enum Suit { Diamonds, Clubs, Hearts, Spades }

    private enum Rank { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

    private enum Category { HighCard, OnePair, TwoPair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush }

    private record Card(Rank Rank, Suit Suit);

    private record Hand(Card[] Cards) : IComparable<Hand>
    {
        private readonly Rank[] ranks = Cards
            .Select(card => card.Rank)
            .OrderByDescending(rank => Cards.Count(card => card.Rank == rank))
            .ThenByDescending(rank => rank)
            .ToArray();

        private readonly int[] rankCounts = Cards
            .GroupBy(card => card.Rank)
            .Select(grouping => grouping.Count())
            .OrderDescending()
            .ToArray();

        private readonly int suitCount = Cards.DistinctBy(card => card.Suit).Count();

        public int CompareTo(Hand other) =>
            Category.CompareTo(other.Category) switch
            {
                < 0 => -1,
                > 0 => 1,
                0 => CategoryRanks.CompareTo(other.CategoryRanks)
            };

        private Category Category =>
            rankCounts switch
            {
                [1, 1, 1, 1, 1] when SameSuit && SequentialRanks => Category.StraightFlush,
                [4, 1] => Category.FourOfAKind,
                [3, 2] => Category.FullHouse,
                _ when SameSuit => Category.Flush,
                [1, 1, 1, 1, 1] when SequentialRanks => Category.Straight,
                [3, 1, 1] => Category.ThreeOfAKind,
                [2, 2, 1] => Category.TwoPair,
                [2, 1, 1, 1] => Category.OnePair,
                _ => Category.HighCard
            };

        private bool SameSuit => suitCount == 1;
        private bool SequentialRanks => ranks[0] - ranks[4] == 4 || SequentialRanksWithLowAce;
        private bool SequentialRanksWithLowAce => ranks is [Rank.Ace, Rank.Five, Rank.Four, Rank.Three, Rank.Two];

        private (Rank, Rank, Rank, Rank, Rank) CategoryRanks =>
            Category switch
            {
                Category.StraightFlush or Category.Straight when SequentialRanksWithLowAce => (ranks[1], ranks[2], ranks[3], ranks[4], ranks[0]),
                _ => (ranks[0], ranks[1], ranks[2], ranks[3], ranks[4])
            };
    }

    private static class Parser
    {
        public static Hand ParseHand(string hand) => new(ParseCards(hand));
        private static Card[] ParseCards(string hand) => hand.Split(' ').Select(ParseCard).ToArray();
        private static Card ParseCard(string card) => new(ParseRank(card), ParseSuit(card));

        private static Rank ParseRank(string card) =>
            card[0] switch
            {
                '2' => Rank.Two,
                '3' => Rank.Three,
                '4' => Rank.Four,
                '5' => Rank.Five,
                '6' => Rank.Six,
                '7' => Rank.Seven,
                '8' => Rank.Eight,
                '9' => Rank.Nine,
                '1' => Rank.Ten,
                'J' => Rank.Jack,
                'Q' => Rank.Queen,
                'K' => Rank.King,
                'A' => Rank.Ace,
            };

        private static Suit ParseSuit(string card) =>
            card[^1] switch
            {
                'H' => Suit.Hearts,
                'S' => Suit.Spades,
                'D' => Suit.Diamonds,
                'C' => Suit.Clubs,
            };
    }
}
