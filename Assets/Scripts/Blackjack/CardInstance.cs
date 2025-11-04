namespace HouseTakes21.Blackjack
{
    /// <summary>
    /// Represents a runtime card in the shoe or a hand.
    /// </summary>
    public sealed class CardInstance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardInstance"/> class.
        /// </summary>
        /// <param name="card">Backing data.</param>
        public CardInstance(CardSO card)
        {
            Card = card;
            ValueOverride = card.BaseValue;
        }

        /// <summary>
        /// Gets the data for this card.
        /// </summary>
        public CardSO Card { get; }

        /// <summary>
        /// Gets or sets the effective value for scoring.
        /// </summary>
        public int ValueOverride { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the card is marked as golden.
        /// </summary>
        public bool IsGolden { get; set; }
    }
}
