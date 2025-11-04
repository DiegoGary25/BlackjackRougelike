using System.Collections.Generic;
using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Data
{
    /// <summary>
    /// Provides helper methods for generating a standard playing card set at runtime.
    /// </summary>
    public static class StandardDeckBuilder
    {
        private static readonly string[] Suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        private static readonly string[] Ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

        /// <summary>
        /// Creates runtime card assets for a standard deck.
        /// </summary>
        /// <returns>List of Card ScriptableObjects.</returns>
        public static List<CardSO> Create()
        {
            var cards = new List<CardSO>(52);
            foreach (string suit in Suits)
            {
                for (int rankIndex = 0; rankIndex < Ranks.Length; rankIndex++)
                {
                    string rank = Ranks[rankIndex];
                    CardSO card = ScriptableObject.CreateInstance<CardSO>();
                    int baseValue = rankIndex + 1;
                    bool isAce = rank == "A";
                    bool isFace = rank == "J" || rank == "Q" || rank == "K";
                    if (isFace)
                    {
                        baseValue = 10;
                    }
                    else if (rank == "10")
                    {
                        baseValue = 10;
                    }

                    if (isAce)
                    {
                        baseValue = 11;
                    }

                    card.Configure($"{rank}{suit.Substring(0, 1)}", $"{rank} of {suit}", baseValue, isFace, isAce, suit);
                    cards.Add(card);
                }
            }

            return cards;
        }
    }
}
