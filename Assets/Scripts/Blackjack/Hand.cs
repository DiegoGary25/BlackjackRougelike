using System.Collections.Generic;

namespace HouseTakes21.Blackjack
{
    /// <summary>
    /// Represents a player hand in blackjack, supporting splits.
    /// </summary>
    public sealed class Hand
    {
        private readonly List<CardInstance> cards = new();

        /// <summary>
        /// Gets the cards in the hand.
        /// </summary>
        public IReadOnlyList<CardInstance> Cards => cards;

        /// <summary>
        /// Gets or sets a value indicating whether this hand has been resolved.
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this hand was doubled.
        /// </summary>
        public bool IsDouble { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this hand resulted from a split and represents the right stack.
        /// </summary>
        public bool IsRightSplit { get; set; }

        /// <summary>
        /// Gets or sets an internal modifier for hidden bonuses.
        /// </summary>
        public int HiddenModifier { get; set; }

        /// <summary>
        /// Adds a card to the hand.
        /// </summary>
        /// <param name="card">Card to add.</param>
        public void AddCard(CardInstance card)
        {
            cards.Add(card);
        }

        /// <summary>
        /// Removes a card from the hand.
        /// </summary>
        /// <param name="card">Card to remove.</param>
        public void RemoveCard(CardInstance card)
        {
            cards.Remove(card);
        }

        /// <summary>
        /// Clears all cards from the hand.
        /// </summary>
        public void Clear()
        {
            cards.Clear();
            IsComplete = false;
            IsDouble = false;
            IsRightSplit = false;
            HiddenModifier = 0;
        }
    }
}
